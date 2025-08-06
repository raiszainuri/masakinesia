using UnityEngine;

public class ServingHelper : MonoBehaviour
{
    // Status penyajian
    public bool bumbuIncluded = false;
    public bool meatIncluded = false;
    public bool jerukIncluded = false;
    public bool onionIncluded = false;
    public int meatCount = 0;

    // Referensi objek
    public GameObject mascotObject;
    public GameObject plateNonClickableObject;
    public GameObject confetti;

    // Animasi confetti
    private Vector3 baseScale = Vector3.one;   // Skala dasar sebelum animasi
    private float scaleSpeed = 2f;             // Kecepatan animasi
    private bool animateConfetti = false;      // Apakah animasi dimulai

    // Syarat animasi confetti
    private readonly float minScale = 1.09f;
    private readonly float maxScale = 2.5f;

    private AudioSource audioSource;
    public AudioClip audioClip;


    void Start()
    {
        audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.clip = audioClip;
    }

    void Update()
    {
        if (animateConfetti && confetti != null)
        {
            float t = Mathf.PingPong(Time.time * scaleSpeed, 1f);
            float currentScale = Mathf.Lerp(minScale, maxScale, t);
            confetti.transform.localScale = baseScale * currentScale;
           
        }
    }

    public bool IsAllIncluded()
    {
        // Semua bahan harus ada, dan daging minimal 4
        return bumbuIncluded && meatIncluded && meatCount > 3 && jerukIncluded && onionIncluded;
    }

    public void ShowInformation()
    {
        if (plateNonClickableObject != null)
            Destroy(plateNonClickableObject);

        if (mascotObject != null)
            mascotObject.transform.position = new Vector3(-5.42f, -2.19f, 0f);

        if (confetti != null)
        {
            confetti.SetActive(true);
            baseScale = Vector3.one; // atau confetti.transform.localScale kalau confetti punya skala awal tertentu
            animateConfetti = true;
            audioSource.Play();
        }
    }
}
