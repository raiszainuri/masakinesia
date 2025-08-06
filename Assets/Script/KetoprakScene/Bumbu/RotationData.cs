using UnityEngine;

[System.Serializable]
public class RotationData
{
    public float accumulatedRotation = 0f;
    public int rotationCount = 0;
    public SpriteRenderer sprite;
    public float lastSoundTime;
}
