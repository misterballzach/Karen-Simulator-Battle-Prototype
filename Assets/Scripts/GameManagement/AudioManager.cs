using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance { get; private set; }

    [Header("Audio Sources")]
    public AudioSource musicSource;
    public AudioSource sfxSource;

    [Header("Audio Clips")]
    public AudioClip backgroundMusic;
    public AudioClip clickSound;
    public AudioClip switchSound;
    public AudioClip victoryJingle;
    public AudioClip defeatJingle;

    [Header("Asset Paths")]
    public string backgroundMusicPath = "Audio/Music/jingles_PIZZI00";
    public string clickSoundPath = "Audio/UI/click1";
    public string switchSoundPath = "Audio/UI/switch1";
    public string victoryJinglePath = "Audio/Music/victory";
    public string defeatJinglePath = "Audio/Music/defeat";

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        LoadAudio();
        PlayMusic(backgroundMusic);
    }

    void LoadAudio()
    {
        backgroundMusic = Resources.Load<AudioClip>(backgroundMusicPath);
        clickSound = Resources.Load<AudioClip>(clickSoundPath);
        switchSound = Resources.Load<AudioClip>(switchSoundPath);
        victoryJingle = Resources.Load<AudioClip>(victoryJinglePath);
        defeatJingle = Resources.Load<AudioClip>(defeatJinglePath);
    }

    public void PlayMusic(AudioClip clip)
    {
        if (musicSource != null && clip != null)
        {
            musicSource.clip = clip;
            musicSource.loop = true;
            musicSource.Play();
        }
    }

    public void PlaySoundEffect(AudioClip clip)
    {
        if (sfxSource != null && clip != null)
        {
            sfxSource.PlayOneShot(clip);
        }
    }

    public void PlayClickSound()
    {
        PlaySoundEffect(clickSound);
    }

    public void PlaySwitchSound()
    {
        PlaySoundEffect(switchSound);
    }

    public void PlayVictoryJingle()
    {
        PlayMusic(victoryJingle);
    }

    public void PlayDefeatJingle()
    {
        PlayMusic(defeatJingle);
    }
}
