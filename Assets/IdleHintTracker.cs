using UnityEngine;

public class IdleHintTracker : MonoBehaviour
{
    public float idleTimeThreshold = 15f;
    private float idleTimer = 0f;
    private bool isIdle = false;

    private HintBlinkerEffect blinker;

    public bool enableHint = true;

    void Start()
    {
        blinker = GetComponent<HintBlinkerEffect>();
    }

    void Update()
    {
        if (!enableHint) return;

        idleTimer += Time.deltaTime;

        if (!isIdle && idleTimer >= idleTimeThreshold)
        {
            isIdle = true;
            blinker?.StartBlinking();
        }
    }

    public void RegisterInteraction()
    {
        idleTimer = 0f;
        if (isIdle)
        {
            isIdle = false;
            blinker?.StopBlinking();
        }
    }
}
