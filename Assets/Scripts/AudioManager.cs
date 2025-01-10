using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public enum SoundName
{
    music,
    victory,
    buttonClick,

}
public class AudioManager : MonoBehaviour
{
    public static AudioManager Instane { get;private set; }   
    [SerializeField] AudioSource buttonClick;
    [SerializeField] AudioSource audioSource;
    [Header("Audio Clips")]
    [SerializeField] AudioClip vicotryAudioClip;
    private void Awake()
    {
        Instane = this;
    }

    private SoundName soundName;
    public void PlaySound(SoundName sound)
    {
        switch (sound)
        {             
                case SoundName.victory:
                {
                    audioSource.clip = vicotryAudioClip;
                    audioSource.Play(); break;
                }
                case SoundName.buttonClick:
                {
                    buttonClick.Play(); break;
                }
        }
    }
    public void PauseSound(SoundName sound)
    {
        switch (sound)
        {
            case SoundName.victory:
                {                   
                    audioSource.Stop(); break;
                }
              case SoundName.buttonClick:
                {
                    buttonClick.Stop(); break;
                }
        }
    }

    public void PlayButtonClickSound()
    {
        PlaySound(SoundName.buttonClick);
    }
}
