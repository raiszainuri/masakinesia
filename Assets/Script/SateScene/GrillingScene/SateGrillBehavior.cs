using System;
using Unity.VisualScripting;
using UnityEngine;

public class SateGrillBehavior : MonoBehaviour
{
    public String targetName;
    private bool isMoving;


    public float moveSpeed = 15f;
    public float rotateSpeed = 15f;

    private Vector3 targetPosition;
    private Quaternion targetRotation;

    public AudioClip audioClip;
    private AudioSource audioSource;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.clip = audioClip;
    }

    // Update is called once per frame
    void Update()
    {
        if (isMoving)
        {
            transform.SetPositionAndRotation(Vector3.Lerp(transform.position, targetPosition, Time.deltaTime * moveSpeed), Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * rotateSpeed));
            if (Vector3.Distance(transform.position, targetPosition) < 0.01f &&
                Quaternion.Angle(transform.rotation, targetRotation) < 1f)
            {
                transform.SetPositionAndRotation(targetPosition, targetRotation);
                isMoving = false;
            }
        }
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag(targetName))
        {
            GetComponent<BoxCollider2D>().enabled = false;
            collision.enabled = false;
            targetPosition = collision.transform.position;
            targetPosition.y -= 1f;
            targetRotation = Quaternion.Euler(0f, 0f, -90f);

            GameObject.Find("SceneController").GetComponent<GrillingSceneController>().RegisterDrop();

            isMoving = true;
            PlayAudio();
        }
    }

    private void PlayAudio()
    {
        audioSource.Play();
    }

    public ShapeLoadingBar shb;
    void OnMouseDown()
    {
        if (shb.isDone && shb.step == 1)
        {
            shb.step += 1;
            shb.timer = 0f;
            shb.started = true;
        }
        else if (shb.isDone && shb.step == 2) { 
            shb.step += 1;
        }
    }
}
