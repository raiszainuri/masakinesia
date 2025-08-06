using UnityEngine;

public class MeatPieceBehavior : MonoBehaviour
{
    private Vector3 initialPosition;
    

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        initialPosition = transform.position;
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = Vector3.Lerp(transform.position, initialPosition, Time.deltaTime * 5f);
        if (Vector3.Distance(transform.position, initialPosition) < 0.01f)
        {
            transform.position = initialPosition;
        }
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("DropArea"))
        {
            Destroy(gameObject);
        }
    }
}
