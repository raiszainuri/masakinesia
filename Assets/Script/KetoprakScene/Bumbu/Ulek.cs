using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Ulek : MonoBehaviour
{
    [Header("Konfigurasi")]
    public string nextScene;
    public int requiredRotations = 5;
    public ParticleSystem crushEffect;
    public AudioClip crushSound;

    [Header("Visual Ulekan Sendiri")]
    public Sprite ulekSprite; // sprite baru untuk cobek saat progres
    private SpriteRenderer selfRenderer;

    private Transform ulekan;
    private float lastAngle;

    private Dictionary<Dragable, RotationData> bahanInArea = new();

    private void Start()
    {
        selfRenderer = GetComponent<SpriteRenderer>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Ulekan"))
        {
            ulekan = other.transform;
            lastAngle = GetAngleToUlekan();
        }

        if (other.TryGetComponent(out Dragable dragable))
        {
            if (!bahanInArea.ContainsKey(dragable))
            {
                RotationData data = new RotationData
                {
                    accumulatedRotation = 0f,
                    rotationCount = 0,
                    sprite = dragable.GetComponent<SpriteRenderer>(),
                    lastSoundTime = Time.time
                };
                bahanInArea.Add(dragable, data);
            }
        }
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if (ulekan == null || other.CompareTag("Ulekan") == false) return;

        float currentAngle = GetAngleToUlekan();
        float angleDelta = Mathf.Abs(Mathf.DeltaAngle(lastAngle, currentAngle));
        lastAngle = currentAngle;

        if (angleDelta > 5f)
        {
            List<Dragable> toRemove = new();

            foreach (var pair in bahanInArea)
            {
                var dragable = pair.Key;
                var data = pair.Value;

                data.accumulatedRotation += angleDelta;

                if (data.accumulatedRotation >= 360f)
                {
                    data.rotationCount++;
                    data.accumulatedRotation = 0f;

                    if (crushSound != null && Time.time - data.lastSoundTime >= 0.3f)
                    {
                        AudioSource.PlayClipAtPoint(crushSound, dragable.transform.position);
                        data.lastSoundTime = Time.time;
                    }

                    float progress = Mathf.Clamp01((float)data.rotationCount / requiredRotations);
                    UpdateTransparency(data.sprite, 1f - progress);
                    UpdateSelfProgress(progress);

                    Debug.Log($"Progress {dragable.gameObject.name}: {data.rotationCount}/{requiredRotations}");

                    if (data.rotationCount >= requiredRotations)
                    {
                        if (crushEffect != null)
                            Instantiate(crushEffect, dragable.transform.position, Quaternion.identity);

                        dragable.gameObject.SetActive(false);
                        toRemove.Add(dragable);
                    }
                }
            }

            foreach (var done in toRemove)
            {
                bahanInArea.Remove(done);
            }

            if (bahanInArea.Count == 0)
            {
                Destroy(GameObject.FindGameObjectWithTag("Ulekan"));
                Debug.Log("✅ Semua bahan berhasil diulek dan menghilang!");
                StartCoroutine(LoadSceneWithDelay(1f));

            }
        }
    }

    IEnumerator LoadSceneWithDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        SceneManager.LoadScene(nextScene);
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Ulekan"))
        {
            ulekan = null;
        }

        if (other.TryGetComponent(out Dragable dragable))
        {
            if (bahanInArea.ContainsKey(dragable))
            {
                Debug.Log($"ℹ️ {dragable.gameObject.name} keluar dari area, progres tetap disimpan.");
            }
        }
    }

    private float GetAngleToUlekan()
    {
        Vector2 dir = ulekan.position - transform.position;
        return Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
    }

    private void UpdateTransparency(SpriteRenderer sprite, float alpha)
    {
        if (sprite == null) return;
        Color c = sprite.color;
        c.a = alpha;
        sprite.color = c;
    }

    private void UpdateSelfProgress(float progress)
    {
        if (selfRenderer == null) return;

        if (ulekSprite != null)
        {
            selfRenderer.sprite = ulekSprite;
        }

        Color c = selfRenderer.color;
        c.a = 0f + progress; // makin tinggi progress makin transparan
        selfRenderer.color = c;
    }
}