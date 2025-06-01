using UnityEngine;

public class BGMManager : MonoBehaviour
{
    public static BGMManager instance;

    private AudioSource audioSource;
    private float previousVolume = 1f; // 儲存靜音前音量
    private bool isMuted = false;

    void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;
        DontDestroyOnLoad(gameObject);

        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            Debug.LogError("BGMManager 需要掛 AudioSource！");
        }
    }

    public void SetVolume(float volume)
    {
        if (audioSource != null)
        {
            audioSource.volume = volume;
            isMuted = volume <= 0f;
        }
    }

    public void ToggleMute()
    {
        if (audioSource != null)
        {
            if (isMuted)
            {
                audioSource.volume = previousVolume;
                isMuted = false;
            }
            else
            {
                previousVolume = audioSource.volume;
                audioSource.volume = 0f;
                isMuted = true;
            }
        }
    }

    public float GetVolume()
    {
        return audioSource != null ? audioSource.volume : 0f;
    }

    public bool IsMuted()
    {
        return isMuted;
    }
}
