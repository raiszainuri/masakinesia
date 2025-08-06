using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class KnifeTofuBehavior : MonoBehaviour
{
    public string nextScene;
    // Target knife positions for each step
    public Vector3[] knifeTargetPositions = {
        new Vector3(-1.69f, -0.44f, 0f), // Step 1
        new Vector3(-1.71f, 0.95f, 0f),  // Step 2
        new Vector3(6.30f, -0.18f, 0f)   // Step 3
    };

    // Target knife rotations for each step (Euler Z degrees)
    private Vector3[] knifeTargetRotations = {
        new Vector3(0f, 0f, -70f), // Step 1
        new Vector3(0f, 0f, 27f),  // Step 2
        new Vector3(0f, 0f, -90f)  // Step 3
    };

    // Tofu slice sprites for each step
    public Sprite[] tofuSlicedSprites;

    // Tofu object and its SpriteRenderer
    private GameObject tofuObject;
    private SpriteRenderer tofuRenderer;

    // Animation speed
    public float moveSpeed = 5f;
    public float rotateSpeed = 5f;

    // Slice sound effect
    public AudioClip sliceSound;
    private AudioSource audioSource;

    private Vector3 currentTargetPosition;
    private Quaternion currentTargetRotation;
    private bool isMoving = false;

    // Interaction step (starts from 0 → 1 → 2 → 3)
    private int interactionStep = 0;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }
    }

    void Update()
    {
        if (isMoving)
        {
            transform.position = Vector3.Lerp(transform.position, currentTargetPosition, Time.deltaTime * moveSpeed);
            transform.rotation = Quaternion.Slerp(transform.rotation, currentTargetRotation, Time.deltaTime * rotateSpeed);

            if (Vector3.Distance(transform.position, currentTargetPosition) < 0.01f &&
                Quaternion.Angle(transform.rotation, currentTargetRotation) < 1f)
            {
                transform.position = currentTargetPosition;
                transform.rotation = currentTargetRotation;
                isMoving = false;
            }
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (interactionStep == 0 && other.CompareTag("Tofu"))
        {
            tofuObject = other.gameObject;
            tofuRenderer = tofuObject.GetComponent<SpriteRenderer>();

            ApplyStep(interactionStep);

            // Disable knife drag after hitting tofu
            var dragScript = GetComponent<ObjectDraging>();
            if (dragScript != null) dragScript.DisableDrag();

            interactionStep++;
        }
    }

    void OnMouseDown()
    {
        // Go to the next step on each click if within range
        if (interactionStep > 0 && interactionStep < knifeTargetPositions.Length)
        {
            ApplyStep(interactionStep);
            interactionStep++;
        }
    }

    void ApplyStep(int step)
    {
        // Change tofu sprite if available
        if (tofuRenderer != null && tofuSlicedSprites != null && step < tofuSlicedSprites.Length)
        {
            tofuRenderer.sprite = tofuSlicedSprites[step];
        }

        // Play slicing sound if available
        if (sliceSound != null && audioSource != null)
        {
            audioSource.PlayOneShot(sliceSound);
        }

        // Move and rotate knife to target
        if (step < knifeTargetPositions.Length && step < knifeTargetRotations.Length)
        {
            currentTargetPosition = knifeTargetPositions[step];
            currentTargetRotation = Quaternion.Euler(knifeTargetRotations[step]);
            isMoving = true;
        }

        if (step == 2)
        {
            StartCoroutine(LoadSceneWithDelay(1f));
        }
    }

    IEnumerator LoadSceneWithDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        SceneManager.LoadScene(nextScene);
    }
}
