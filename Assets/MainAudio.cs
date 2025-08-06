using UnityEngine;

public class MainAudio : MonoBehaviour
{
    public AudioSource audioSource;
    public AudioClip music;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }

        audioSource.clip = music;
        audioSource.loop = true;
        audioSource.playOnAwake = false;
        audioSource.Play();
    }

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }
}
