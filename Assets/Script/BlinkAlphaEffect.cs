using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class BlinkAlphaEffect : MonoBehaviour
{
    public float blinkSpeed = 1f; // Kecepatan berkedip (semakin tinggi, semakin cepat)
    public float minAlpha = 0.5f;
    public float maxAlpha = 1f;

    private SpriteRenderer spriteRenderer;
    private bool increasing = false;
    private float currentAlpha;

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        currentAlpha = maxAlpha;
    }

    void Update()
    {
        float delta = blinkSpeed * Time.deltaTime;
        
        if (increasing)
        {
            currentAlpha += delta;
            if (currentAlpha >= maxAlpha)
            {
                currentAlpha = maxAlpha;
                increasing = false;
            }
        }
        else
        {
            currentAlpha -= delta;
            if (currentAlpha <= minAlpha)
            {
                currentAlpha = minAlpha;
                increasing = true;
            }
        }

        Color color = spriteRenderer.color;
        color.a = currentAlpha;
        spriteRenderer.color = color;
    }
}
