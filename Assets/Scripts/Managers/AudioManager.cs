using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance { get; private set; }

    private AudioSource _musicSource;
    private AudioSource _sfxSource;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);

            _musicSource = gameObject.AddComponent<AudioSource>();
            _sfxSource = gameObject.AddComponent<AudioSource>();
        }
    }

    private void Start()
    {
        AudioClip musicClip = Resources.Load<AudioClip>("Audio/Music/BleedItOut");
        if (musicClip != null)
        {
            _musicSource.clip = musicClip;
            _musicSource.loop = true;  
            _musicSource.Play();
            _musicSource.volume = 0.15f;
        }
    }

    public void PlaySFX(AudioClip clip, float volume = 0.2f)
    {
        AudioClip sfxClip = clip;
        _sfxSource.volume= volume;
        _sfxSource.PlayOneShot(sfxClip);
    }
}
