using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class KetupatKnifeBehavior : MonoBehaviour
{

    public string nextScene;
    // Target knife positions for each step
    private Vector3[] knifeTargetPositions = {
        new Vector3(-1.4f,-0.44f,0f), // Step 1
        new Vector3(-1.35f, 2.17f, 0f),  // Step 2
        new Vector3(-1.26f, 0.66f, 0f),   // Step 3
         new Vector3(6.30f, -0.18f, 0f)   // Step 4
    };

    // Target knife rotations for each step (Euler Z degrees)
    private Vector3[] knifeTargetRotations = {
        new Vector3(0,0,-66f), // Step 1
        new Vector3(0,0,27f), // Step 2
        new Vector3(0f, 0f, 27f),  // Step 3
        new Vector3(0f, 0f, -90f)  // Step 4
    };

    // Ketupat slice sprites for each step
    public Sprite[] ketupatSlicedSprites;

    // Ketupat object and its SpriteRenderer
    private GameObject ketupatObject;
    private SpriteRenderer ketupatRenderer;

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
        if (interactionStep == 0 && other.CompareTag("Ketupat"))
        {
            ketupatObject = other.gameObject;
            ketupatRenderer = ketupatObject.GetComponent<SpriteRenderer>();

            ApplyStep(interactionStep);

            // Disable knife drag after hitting ketupat
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
        // Change ketupat sprite if available
        if (ketupatRenderer != null && ketupatSlicedSprites != null && step < ketupatSlicedSprites.Length)
        {
            ketupatRenderer.sprite = ketupatSlicedSprites[step];
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


        if (step == 3)
        {
            StartCoroutine(LoadSceneWithDelay(2f));
        }
    }

    IEnumerator LoadSceneWithDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        SceneManager.LoadScene(nextScene);
    }
}
