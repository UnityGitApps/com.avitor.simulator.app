using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public AudioSource auso;

    public void SetSound(AudioClip ac) 
    {
        auso.clip = ac;
        auso.Play();
    }
}