using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundController : MonoBehaviour
{
    private bool isSoundOn = true;
    private const string SoundKey = "SoundState";

    private void Start()
    {
        // Load the sound state from PlayerPrefs
        LoadSoundState();

        // Set the AudioListener state based on the loaded sound state
        AudioListener.pause = !isSoundOn;
    }

    public void ToggleSound()
    {
        // Invert the current sound state
        isSoundOn = !isSoundOn;

        // Update AudioListener and save the sound state
        UpdateAudioListener();
        SaveSoundState();
    }

    private void UpdateAudioListener()
    {
        // Pause or unpause the AudioListener based on the sound state
        AudioListener.pause = !isSoundOn;

        // Optionally, you can do more here like updating UI to reflect the sound state
    }

    private void SaveSoundState()
    {
        // Save the sound state to PlayerPrefs
        PlayerPrefs.SetInt(SoundKey, isSoundOn ? 1 : 0);
        PlayerPrefs.Save();
    }

    private void LoadSoundState()
    {
        // Load the sound state from PlayerPrefs, defaulting to true if the key is not present
        isSoundOn = PlayerPrefs.GetInt(SoundKey, 1) == 1;
    }
}