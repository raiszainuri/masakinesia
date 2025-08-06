using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SateBehavior : MonoBehaviour
{
    public Sprite newSprite;
    private new BoxCollider2D collider;
    private SpriteRenderer spriteRenderer;
    public PreGrillingSceneController preGrillingSceneController;

    public string nextScene = "GrillingScene";

    void Start()
    {
        collider = GetComponent<BoxCollider2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        if (preGrillingSceneController.CheckSateCount())
        {
            LoadWithDelay();
        }
    }

    // Update is called once per frame
    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Detector"))
        {
            collider.enabled = false;
            spriteRenderer.sprite = newSprite;
            preGrillingSceneController.AddSateCount();
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
