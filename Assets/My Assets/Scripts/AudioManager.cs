using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public enum SoundName
{
    BgMusic,
    Victory,
    ButtonClick,

}
public class AudioManager : MonoBehaviour
{
   
    public static AudioManager Instane { get;private set; }   
    [SerializeField] AudioSource buttonClick;
    [SerializeField] AudioSource audioSource;
    [SerializeField] AudioSource bgMusic;
    [Header("Audio Clips")]
    [SerializeField] AudioClip vicotryAudioClip;
    public bool isAudioOn { get { return GameData.SoundSettingsIndex == 1; } }
    public bool isMusicOn { get { return GameData.MusicSettingsIndex == 1; } }
    [Space(10)]
    [Hamza(1, 1, 1, 7)]
    [Header("Button Section"),Space(10)]
    [SerializeField] Button musicOnBtn;
    [SerializeField] Button musicOffBtn;
    [SerializeField] Button soundONBtn;
    [SerializeField] Button soundOffBtn;
    public Scenes scene;
    private void Awake()
    {
        Instane = this;
        
    }
    private void Start()
    {
        if (scene != Scenes.splash )
        {
            SoundButtonEnable(isAudioOn);
            MusicButtonsEnable(isMusicOn);
        }
        if(scene != Scenes.gamePlay)
        {
            PlayBgMusic(isMusicOn);
        }
       
    }
    private SoundName soundName;
    public void PlaySound(SoundName sound)
    {
        if (isAudioOn)
        {
            switch (sound)
            {
                case SoundName.BgMusic:
                    {                        
                        bgMusic.Play(); break;
                    }
                case SoundName.Victory:
                    {
                        audioSource.clip = vicotryAudioClip;
                        audioSource.Play(); break;
                    }
                case SoundName.ButtonClick:
                    {
                        buttonClick.Play(); break;
                    }
            }
        }
      
    }
    public void PauseSound(SoundName sound)
    {
        switch (sound)
        {
            case SoundName.Victory:
                {                   
                    audioSource.Stop(); break;
                }
              case SoundName.ButtonClick:
                {
                    buttonClick.Stop(); break;
                }
            case SoundName.BgMusic:
                {
                    bgMusic.Stop(); break;
                }
        }
    }
    public void PlayButtonClickSound()
    {
        if (isAudioOn)
        {
            PlaySound(SoundName.ButtonClick);
        }
    }
    public void PlayBgMusic(bool musicOn)
    {
        if (musicOn)
        {
            PlaySound(SoundName.BgMusic);
        }
        else
        {
            PauseSound(SoundName.BgMusic);
        }
    }
    public void OnMusicOnButtonClick()
    {
        PauseSound(SoundName.ButtonClick);
        GameData.SaveMusicSettings(1);       
        MusicButtonsEnable(isMusicOn);
        PlayBgMusic(true);
    }
    public void OnMusicOffButtonClick()
    {
        PauseSound(SoundName.ButtonClick);
        GameData.SaveMusicSettings(0);      
        MusicButtonsEnable(isMusicOn);
        PlayBgMusic(false);
    }
    public void ONSoundOnButtonClick()
    {
        PauseSound(SoundName.ButtonClick);
        GameData.SaveSoundSetting(1);        
        SoundButtonEnable(isAudioOn);
    }
    public void OnSoundOffButtonClick()
    {

        PauseSound(SoundName.ButtonClick);
        GameData.SaveSoundSetting(0);    
        SoundButtonEnable(isAudioOn);

    }
    private void SoundButtonEnable(bool isSoundON)
    {

        soundONBtn.gameObject.SetActive(!isSoundON);
        soundOffBtn.gameObject.SetActive(isSoundON);

    }
    private void MusicButtonsEnable(bool isMusicON)
    {
        musicOnBtn.gameObject.SetActive(!isMusicON);
        musicOffBtn.gameObject.SetActive(isMusicON);

    }
}
public enum Scenes
{
    mainMenu,
    gamePlay,
    splash,
}