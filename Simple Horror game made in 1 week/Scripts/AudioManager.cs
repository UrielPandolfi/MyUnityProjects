using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    #region Singleton
    public static AudioManager instance;
    private void Awake()
    {
        instance = this;
    }
    #endregion
    
    public AudioSource[] soundsEffects;
    
    public void PlaySFX(int sfx)
    {
        soundsEffects[sfx].Play();
    }
}
