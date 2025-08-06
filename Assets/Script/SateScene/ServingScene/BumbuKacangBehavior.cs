using System;
using UnityEngine;

public class BumbuKacangBehavior : MonoBehaviour
{

    public GameObject bumbuKacangPlate;
    public String targetName;
     public ServingHelper gameHelper;

    public AudioClip audioClip;
    private AudioSource audioSource;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        audioSource = gameObject.AddComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag(targetName))
        {
            audioSource.PlayOneShot(audioClip);
            var bumbuKacangSpriteRenderer = bumbuKacangPlate.GetComponent<SpriteRenderer>();
            Color bumbuKacangPlateColor = bumbuKacangSpriteRenderer.color;
            bumbuKacangPlateColor.a = 1f;
            bumbuKacangSpriteRenderer.color = bumbuKacangPlateColor;
            Destroy(gameObject);
            gameHelper.bumbuIncluded = true;
        } 
    }
}
