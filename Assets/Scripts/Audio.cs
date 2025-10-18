using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance = null;
    public AudioClip backgroundMusic;
    private AudioSource audioSource;

    // Public variable for volume control
    public float musicVolume = 1.0f;

    void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(this.gameObject);
            return;
        }

        instance = this;
        DontDestroyOnLoad(this.gameObject);

        audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.clip = backgroundMusic;
        audioSource.loop = true;
        audioSource.playOnAwake = false;

        // Set initial volume
        audioSource.volume = musicVolume;
    }

    void Start()
    {
        PlayBackgroundMusic();
    }

    void Update()
    {
        // Update the volume based on the public variable
        audioSource.volume = musicVolume;
    }

    public void PlayBackgroundMusic()
    {
        if (audioSource.isPlaying) return;
        audioSource.Play();
    }

    public void StopBackgroundMusic()
    {
        audioSource.Stop();
    }

    public void IncreaseVolume(float amount)
    {
        musicVolume = Mathf.Clamp(musicVolume + amount, 0f, 1f);
    }

    public void DecreaseVolume(float amount)
    {
        musicVolume = Mathf.Clamp(musicVolume - amount, 0f, 1f);
    }
}
