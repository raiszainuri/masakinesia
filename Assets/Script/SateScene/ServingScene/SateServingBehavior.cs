using System;
using UnityEngine;
using UnityEngine.UIElements;

public class SateServingBehavior : MonoBehaviour
{

    public String targetName;
    public  Vector3 targetPosition;
    public  Quaternion targetRotation;

    public float moveSpeed = 16f;
    public float rotateSpeed = 16f;

    public ServingHelper servingHelper;
    private Vector3 defaultPosition;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        GetComponent<BoxCollider2D>().enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log("included? " + servingHelper.bumbuIncluded);
        if (servingHelper.bumbuIncluded)
        {
            GetComponent<BoxCollider2D>().enabled = true;
        }
        
        // if (!isMoving) return;
        // transform.SetPositionAndRotation(Vector3.Lerp(transform.position, targetPosition, Time.deltaTime * moveSpeed), Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * rotateSpeed));
        // if (Vector3.Distance(transform.position, targetPosition) < 0.01f &&
        //     Quaternion.Angle(transform.rotation, targetRotation) < 1f)
        // {
        //     transform.SetPositionAndRotation(targetPosition, targetRotation);
        //     GetComponent<ObjectDraging>().DisableDrag();
        //     isMoving = false;
        // }
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Plate"))
        {
            servingHelper.meatIncluded = true;
            servingHelper.meatCount += 1;
        }
        // if (collision.CompareTag(targetName))
        // {
        //     GetComponent<BoxCollider2D>().isTrigger = false;
        //     GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Dynamic;
        //     targetPosition = collision.transform.position;
        //     targetRotation = collision.transform.rotation;
        //     isMoving = true;
        // }
    }
}
