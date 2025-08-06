using System.Collections;
using UnityEngine;

public class HintBlinkerEffect : MonoBehaviour
{
    public float blinkAlphaMin = 0.5f;
    public float blinkAlphaMax = 1f;
    public float blinkSpeed = 1f;

    private SpriteRenderer sr;
    private Coroutine blinkRoutine;

    void Awake()
    {
        sr = GetComponent<SpriteRenderer>();
    }

    public void StartBlinking()
    {
        if (blinkRoutine == null)
            blinkRoutine = StartCoroutine(Blink());
    }

    public void StopBlinking()
    {
        if (blinkRoutine != null)
        {
            StopCoroutine(blinkRoutine);
            blinkRoutine = null;
            if (sr != null)
            {
                Color color = sr.color;
                color.a = 1f;
                sr.color = color;
            }
        }
    }

    private IEnumerator Blink()
    {
        while (true)
        {
            float t = 0f;
            while (t < 1f)
            {
                t += Time.deltaTime * blinkSpeed;
                float a = Mathf.Lerp(blinkAlphaMin, blinkAlphaMax, Mathf.PingPong(Time.time * blinkSpeed, 1));
                Color c = sr.color;
                c.a = a;
                sr.color = c;
                yield return null;
            }
        }
    }
}
