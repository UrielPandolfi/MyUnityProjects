using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;
    public AudioMixerGroup musicMixer, sfxMixer;
    public AudioSource[] musicList;
    public AudioSource[] sfxList;

    private void Awake()
    {
        instance = this;
    }
    
    public void playMusic(int musicNumber)
    {
        musicList[musicNumber].Play();
    }

    public void stopMusic(int musicNumber)
    {
        musicList[musicNumber].Stop();
    }

    public void playSFX(int sfxNumber)
    {
        sfxList[sfxNumber].Play();
    }

}
