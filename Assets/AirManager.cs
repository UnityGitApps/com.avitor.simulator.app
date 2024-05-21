using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class AirManager : MonoBehaviour
{
    public static AirManager instance;

    private Animator anima;
    private AudioSource audioSource;

    public TextMeshProUGUI counterT;
    public TextMeshProUGUI balanceT;
    public TextMeshProUGUI flyTimeT;
    public TextMeshProUGUI roundT;

    public TextMeshProUGUI lostT;
    public TextMeshProUGUI lostT2;

    public TextMeshProUGUI winT;
    public TextMeshProUGUI winT2;

    private int counter;
    public float balance = 500;

    public GameObject loseScreen;
    public GameObject wonScreen;

    public float targetTime;
    private float currentTime = 0f;
    private bool timerRunning = false;

    private bool isRunning = true;

    private float round = 0;
    private float roundTime = 0;

    public Button playBttn;

    private void Start()
    {
        instance = this;

        audioSource = GetComponent<AudioSource>();
        anima = GetComponent<Animator>();

        if (balance < PlayerPrefs.GetFloat("balance"))
        {
            balance = PlayerPrefs.GetFloat("balance");
            balanceT.text = balance + ".0".ToString();
        }
        else { balance = 500; }

        round = PlayerPrefs.GetFloat("round");
        roundT.text = round.ToString();
    }

    void Update()
    {
        if (timerRunning)
        {
            currentTime += Time.deltaTime;

            // Check if the timer has reached the target time
            if (currentTime >= targetTime)
            {
                ResetTimer();
            }
        }

        roundTime = (float)Math.Round(currentTime, 1);
        flyTimeT.text = roundTime.ToString();

        if (counter > 0 && isRunning) 
        {
            balanceT.text = (balance + (roundTime * counter)) + ".0".ToString();
        }
        else if (!isRunning)
        {
            balanceT.text = balance + ".0".ToString();
        }

        if(counter == 0)
        {
            playBttn.interactable = false;
        }
        else
        {
            playBttn.interactable = true;
        }
    }

    public void StartBttnClick() 
    {
        FlyStart();
        isRunning = true;
    }

    public void StopBttn()
    {
        isRunning = false;
        balance += roundTime * counter;

        PlayerPrefs.SetFloat("balance", balance);

        anima.SetTrigger("GMP");
        wonScreen.SetActive(true);
        winT.text = roundTime + " x " + counter + ".0".ToString();
        winT2.text = (roundTime * counter).ToString();
        round += roundTime;
        roundT.text = round.ToString();

        PlayerPrefs.SetFloat("round", round);

        ResetTimer();

        audioSource.Stop();
    }

    public void FlyStart()
    {
        StartCoroutine(flying());
    }

    public void SetConuter(int value)
    {
        counter = value;
        counterT.text = counter+".0".ToString();
    }

    public void ChangeValue(int value)
    {
        if (counter + value >= 0)
        {
            counter += value;
        }
        counterT.text = counter + ".0".ToString();
    }

    private IEnumerator flying()
    {
        audioSource.Play();
        float randomFlyTime = UnityEngine.Random.Range(2f, 15f);
        float round = (float)Math.Round(randomFlyTime, 2);
        anima.SetTrigger("Fly");
        targetTime = round;
        StartTimer();
        
        yield return new WaitForSeconds(round);

        anima.SetTrigger("GMP");
        loseScreen.SetActive(true);
        lostT.text = roundTime + " x " + counter + ".0".ToString();
        lostT2.text = (roundTime * counter).ToString();
        PlayerPrefs.SetFloat("round", round);
        ResetTimer();

        audioSource.Stop();
    }


    public void StartTimer()
    {
        timerRunning = true;
    }

    public void StopTimer()
    {
        timerRunning = false;
    }

    public void ResetTimer()
    {
        currentTime = 0f;
        timerRunning = false;
    }

    public void RestartGame()
    {
        SceneManager.LoadScene("SampleScene");
    }
}