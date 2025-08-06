using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class MeatBehavior : MonoBehaviour
{
    // === Serialized/Public Fields ===
    [Header("Settings")]
    public string nextScene;
    public string targetName;
    public float moveSpeed = 10f;
    public Vector3 destination;
    public Vector3 platePosition;

    [Header("Audio")]
    public AudioClip sliceSound;

    public GameObject knife;
    public GameObject plate;

    // === Private Fields ===

    private SpriteRenderer knifeRenderer;
    public Sprite spriteDone;
    private AudioSource audioSource;
    private bool isDone = false;
    private bool isChoping = false;
    private bool isChoped = false;


    // === Unity Methods ===
    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }

        knifeRenderer = knife.GetComponent<SpriteRenderer>();
    }

    private void Update()
    {
        if (isChoping)
        {
            Vector3 targetPosition = destination;
            transform.position = Vector3.Lerp(transform.position, targetPosition, Time.deltaTime * moveSpeed);

            if (Vector3.Distance(transform.position, targetPosition) < 0.01f)
            {
                transform.position = targetPosition;
            }
        }

        if (isDone)
        {
            Vector3 targetPosition = platePosition;
            transform.position = Vector3.Lerp(transform.position, targetPosition, Time.deltaTime * moveSpeed);
            GetComponent<SpriteRenderer>().sprite = spriteDone;
            LoadWithDelay();
        }

        if (knifeRenderer != null && !isChoped)
        {
            if (knifeRenderer.color.a <= 0.3f)
            {
                Debug.Log("Masuk sini");
                isChoping = false;
                isChoped = true;
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag(targetName) && !isChoping && !isChoped)
        {
            if (knife != null)
            {
                if (knifeRenderer != null)
                {
                    StartCoroutine(FadeAlpha(knifeRenderer, knifeRenderer.color.a, 1f, 0.5f));
                    if (knife.TryGetComponent<BoxCollider2D>(out var knifeCollider))
                    {
                        StartCoroutine(EnableColliderAfterDelay(knifeCollider, 1f));
                    }
                }
            }
            isChoping = true;
            PlaySliceSound(sliceSound);
        }
        if (other.CompareTag("Plate") && !isChoping && isChoped)
        {
            PlaySliceSound(sliceSound);
            isDone = true;
        }
    }

    private IEnumerator FadeAlpha(SpriteRenderer renderer, float from, float to, float duration)
    {
        float elapsed = 0f;
        Color color = renderer.color;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float newAlpha = Mathf.Lerp(from, to, elapsed / duration);
            color.a = newAlpha;
            renderer.color = color;
            yield return null;
        }

        color.a = to;
        renderer.color = color;
    }

    private IEnumerator EnableColliderAfterDelay(BoxCollider2D collider, float delay)
    {
        yield return new WaitForSeconds(delay);
        collider.enabled = true;
        Debug.Log("Collider Aktif setelah delay");
    }

    private void PlaySliceSound(AudioClip audioClip)
    {
        audioSource.PlayOneShot(audioClip);
    }

    public void LoadWithDelay()
    {
        //PlaySliceSound(applause);
        StartCoroutine(LoadSceneAfterDelay());
    }

    IEnumerator LoadSceneAfterDelay()
    {
        yield return new WaitForSeconds(1f); // ⏳ Delay 1 detik
        SceneManager.LoadScene(nextScene); // 🔁 Ganti scene
    }
}
