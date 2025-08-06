using System;
using UnityEngine;

public class BrushBehavior : MonoBehaviour
{

    public String targetReceiverName;
    public String targetSoySauceName;
    public Sprite bareSprite;
    public Sprite newSprite;

    private SpriteRenderer brushSprite;
    public AudioClip brush;
    public AudioClip celupBrush;

    private AudioSource audioSource;

    void Start()
    {
        brushSprite = GetComponent<SpriteRenderer>();
        audioSource = GetComponent<AudioSource>();
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag(targetSoySauceName))
        {
            brushSprite.sprite = newSprite;
            audioSource.clip = celupBrush;
            audioSource.Play();

        }

        if (collision.CompareTag(targetReceiverName))
        {
            brushSprite.sprite = bareSprite;
            audioSource.clip = brush;
            audioSource.Play();
        }
    }
}
