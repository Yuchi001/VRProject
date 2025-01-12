using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

public class AudioManager : MonoBehaviour
{
    [SerializeField] private List<SFXData> sounds = new();
    [SerializeField] private List<MusicData> themes = new();

    #region Singleton

    private static AudioManager Instance { get; set; }
    
    private void Awake()
    {
        if (Instance != null && Instance != this) Destroy(gameObject);
        else Instance = this;
    }

    #endregion
    
    
    private AudioSource mainAudio = null;
    
    public static void PlaySound(ESFXType soundType)
    {
        var clip = Instance.sounds.FirstOrDefault(s => s.SoundType == soundType)?.AudioClip;
        if (clip == null) return;
    
        var audioSource = new GameObject($"Audio: {soundType}", typeof(AudioSource));
        var audioSourceScript = audioSource.GetComponent<AudioSource>();
        audioSourceScript.volume = 0.2f;
        audioSourceScript.loop = false;
        audioSourceScript.pitch -= Random.Range(-0.1f, 0.1f);
        audioSourceScript.PlayOneShot(clip);
                
        Destroy(audioSource, 0.5f);
    }
    
    public static void SetTheme(EMusicType themeType)
    {
        var audioSource = Instance.mainAudio;
        if (audioSource == null)
        {
            var audioSourceObj = new GameObject($"Audio: {themeType}", typeof(AudioSource));
            audioSource = audioSourceObj.GetComponent<AudioSource>();
            Instance.mainAudio = audioSource;
        }
                
        var clip = Instance.themes.FirstOrDefault(s => s.musicType == themeType)?.AudioClip;
        if (clip == null) return;
    
        audioSource.clip = clip;
        audioSource.volume = 0.3f;
        audioSource.loop = true;
        audioSource.Play();
    }
    
    [System.Serializable]
    public class SFXData
    {
        public AudioClip AudioClip;
        public ESFXType SoundType;
    }
    
    [System.Serializable]
    public class MusicData
    {
        public AudioClip AudioClip;
        public EMusicType musicType;
    }
    
    public enum EMusicType
    {
        BaseTheme,
    }
    
    public enum ESFXType
    {
        PistolShoot,
        TargetHIt,
    }
}