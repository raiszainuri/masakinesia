using System.Collections;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer), typeof(AudioSource), typeof(ObjectDraging))]
public class KnifeBehavior : MonoBehaviour
{
    [Header("Scene Settings")]
    public string nextScene;
    public string targetName;

    [Header("Position & Rotation Targets")]
    public Vector3[] knifeTargetPositions = {
    };

    public Vector3[] knifeTargetRotations = {
    };

    [Header("Animation Settings")]
    public float moveSpeed = 15f;
    public float rotateSpeed = 15f;

    [Header("Visuals")]
    public Sprite[] sprites;
    private GameObject objectDetected;
    private SpriteRenderer meatSpriteRenderer;

    [Header("Audio")]
    public AudioClip sliceSound;

    // === Private Variables ===
    private int interactionStep = 0;
    private bool isMoving = false;
    private bool isDone = false;
    private bool isOnTarget = false;

    private Vector3 currentTargetPosition;
    private Quaternion currentTargetRotation;

    private AudioSource audioSource;

    // === Unity Lifecycle ===
    private void Start()
    {
        audioSource = GetComponent<AudioSource>();

        GetComponent<BoxCollider2D>().enabled = false;
    }

    private void Update()
    {
        if (isDone)
        { 
            transform.SetPositionAndRotation(Vector3.Lerp(transform.position, currentTargetPosition, Time.deltaTime * moveSpeed), Quaternion.Slerp(transform.rotation, currentTargetRotation, Time.deltaTime * rotateSpeed));
        }
        
        if (!isMoving) return;

        transform.SetPositionAndRotation(Vector3.Lerp(transform.position, currentTargetPosition, Time.deltaTime * moveSpeed), Quaternion.Slerp(transform.rotation, currentTargetRotation, Time.deltaTime * rotateSpeed));
        if (Vector3.Distance(transform.position, currentTargetPosition) < 0.01f &&
            Quaternion.Angle(transform.rotation, currentTargetRotation) < 1f)
        {
            transform.SetPositionAndRotation(currentTargetPosition, currentTargetRotation);
            isMoving = false;
            interactionStep++;
        }
    }

    private void OnMouseDown()
    {
        if (!isMoving && isOnTarget)
        {
            ApplyStep();
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag(targetName))
        {
            objectDetected = other.gameObject;
            meatSpriteRenderer = objectDetected.GetComponent<SpriteRenderer>();

            isOnTarget = true;
            ApplyStep();
        }
    }

    // === Core Logic ===
    private void ApplyStep()
    {
        if (interactionStep >= knifeTargetPositions.Length || interactionStep >= knifeTargetRotations.Length)
        {
            return;
        }

        Debug.Log("Pisau Step ke: " + interactionStep);

        currentTargetPosition = knifeTargetPositions[interactionStep];
        currentTargetRotation = Quaternion.Euler(knifeTargetRotations[interactionStep]);

        isMoving = true;

        if (meatSpriteRenderer != null && sprites != null && interactionStep < sprites.Length)
        {
            meatSpriteRenderer.sprite = sprites[interactionStep];
        }

        if (interactionStep == knifeTargetPositions.Length - 1)
        {
            Debug.Log("Step Selesai");
            var spriteRenderer = GetComponent<SpriteRenderer>();
            StartCoroutine(FadeAlpha(spriteRenderer, spriteRenderer.color.a, 0.2f, 0.2f));
            isDone = true;
        }

        if (interactionStep != 0) PlaySliceSound();
    }

    private void PlaySliceSound()
    {
        if (sliceSound != null && audioSource != null)
        {
            audioSource.PlayOneShot(sliceSound);
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
}
