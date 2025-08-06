using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SkewStickBehavior : MonoBehaviour
{

    public Sprite[] sprites;
    public String receiveObject;

    private SpriteRenderer spriteRenderer;
    private int position = 0;
    private bool isDone = false;
    public float moveSpeed = 15f;
    public float rotateSpeed = 15f;
    public Sprite skewSprite;
    public GameObject parentObject;
    public Vector3 targetPlacement;
    private AudioSource audioSource;
    public AudioClip audioClip;
    public AudioClip plopClip;

    public string nextScene = "PreGrillingScene";

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }
    }

    void Update()
    {
        if (isDone)
        {
            Vector3 targetPosition = targetPlacement;
            Quaternion targetRotation = Quaternion.Euler(0f, 0f, 30f);

            transform.SetPositionAndRotation(Vector3.Lerp(transform.position, targetPosition, Time.deltaTime * moveSpeed), Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * rotateSpeed));
            if (Vector3.Distance(transform.position, targetPosition) < 0.01f &&
                Quaternion.Angle(transform.rotation, targetRotation) < 1f)
            {
                if (parentObject.transform.childCount > 0) Spawn();
                else LoadWithDelay();
                transform.SetPositionAndRotation(targetPosition, targetRotation);
                isDone = false;

                GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Dynamic;
                GetComponent<BoxCollider2D>().isTrigger = false;
            }
        }
    }
    public GameObject prefab;
    private void Spawn()
    {
        GameObject spawnedObj = Instantiate(prefab, new(-0.06f, 2.92f, 0f), Quaternion.Euler(0f, 0f, 30f));
        spawnedObj.GetComponent<SpriteRenderer>().sprite = skewSprite;

        // spawnedObj.GetComponent<PolygonCollider2D>().isTrigger = true;


        // GameObject newObj = new("SkewStick" + indexPosition);
        // SpriteRenderer renderer = newObj.AddComponent<SpriteRenderer>();

        // newObj.AddComponent<PolygonCollider2D>();
        // newObj.GetComponent<PolygonCollider2D>().isTrigger = true;
        // newObj.AddComponent<Rigidbody2D>().gravityScale = 0f;
        // newObj.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Kinematic;
        // newObj.AddComponent<SkewStickBehavior>();

        // newObj.transform.localScale = new(0.64f, 0.64f, 0.64f);
        // newObj.transform.position = new(-0.06f, 2.92f, 0);

        // renderer.sprite = skewSprite;
        // renderer.sortingOrder = 2;
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag(receiveObject))
        {
            audioSource.PlayOneShot(audioClip);
            spriteRenderer.sprite = sprites[position];
            position++;

            if (position >= sprites.Length)
            {
                audioSource.PlayOneShot(plopClip);
                isDone = true;
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
