using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayKetoprakScript : MonoBehaviour
{
    public string nextScene;
    public AudioClip crushSound;
    private AudioSource audioSource;
    void OnMouseDown()
    {
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }
        audioSource.PlayOneShot(crushSound);
        SceneManager.LoadScene(nextScene);
    }
}
