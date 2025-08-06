using System.Collections;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ShapeLoadingBar : MonoBehaviour
{
    public Transform fillBar;
    public float duration = 2f;
    public float fullWidth = 1f;

    public float timer = 0f;
    private Vector3 originalScale;
    private Vector3 originalPosition;

    public GameObject[] gameObjectParents;
    public GameObject[] gameObjects;
    public Vector3[] objectPositions;

    private readonly float moveSpeed = 15f;
    private readonly float rotateSpeed = 15f;

    private GrillingSceneController sceneController;
    public bool started = false;
    public bool isDone = false;
    public int step = 1;

    private AudioSource audioSource;
    public AudioClip audioClip;
    public string nextScene = "ServingScene";

    [System.Obsolete]
    void Start()
    {
        if (fillBar != null)
        {
            originalScale = fillBar.localScale;
            originalPosition = fillBar.localPosition;
        }

        sceneController = FindObjectOfType<GrillingSceneController>();
        gameObject.GetComponent<SpriteRenderer>().enabled = false;

        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
            audioSource.loop = true;
        }
    }

    void Update()
    {
        if (timer == 0f && step == 1 && sceneController != null && sceneController.passed)
        {
            started = true;
            audioSource.PlayOneShot(audioClip);
        }

        if (started)
        {
            gameObject.GetComponent<SpriteRenderer>().enabled = true;
            timer += Time.deltaTime;
            float progress = Mathf.Clamp01(timer / duration);

            float currentWidth = progress * fullWidth;
            fillBar.localScale = new Vector3(currentWidth, originalScale.y, originalScale.z);

            float offset = (currentWidth - fullWidth) / 2f;
            fillBar.localPosition = new Vector3(originalPosition.x + offset, originalPosition.y, originalPosition.z);

            for (int i = 0; i < gameObjects.Length; i++)
            {
                var obj = gameObjects[i].GetComponent<SpriteRenderer>();
                Color c = obj.color;
                c.a = progress;
                obj.color = c;

                if (timer >= duration) gameObjectParents[i].GetComponent<BoxCollider2D>().enabled = true;
            }

            if (timer >= duration)
            {
                started = false;
                isDone = true;
            }
        }
        else if (isDone && step == 3)
        {
            for (int i = 0; i < gameObjects.Length; i++)
            {
                Vector3 targetPosition = objectPositions[i];
                Quaternion targetRotation = Quaternion.Euler(0f, 0f, -90f);

                Transform currentTransform = gameObjectParents[i].transform;

                currentTransform.SetPositionAndRotation(Vector3.Lerp(currentTransform.position, targetPosition, Time.deltaTime * moveSpeed), Quaternion.Slerp(currentTransform.rotation, targetRotation, Time.deltaTime * rotateSpeed));
                if (Vector3.Distance(currentTransform.position, targetPosition) < 0.01f &&
                    Quaternion.Angle(currentTransform.rotation, targetRotation) < 1f)
                {
                    currentTransform.SetPositionAndRotation(targetPosition, targetRotation);
                    gameObject.GetComponent<SpriteRenderer>().enabled = false;

                    if (i == gameObjects.Length - 1)
                    {
                        audioSource.Stop();
                        LoadWithDelay();
                    }
                }
            }
        }
    }

    public void LoadWithDelay()
    {
        StartCoroutine(LoadSceneAfterDelay());
    }

    IEnumerator LoadSceneAfterDelay()
    {
        yield return new WaitForSeconds(1f); // ⏳ Delay 1 detik
        SceneManager.LoadScene(nextScene); // 🔁 Ganti scene
    }
}
