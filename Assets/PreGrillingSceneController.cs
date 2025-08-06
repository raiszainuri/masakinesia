using UnityEngine;

public class PreGrillingSceneController : MonoBehaviour
{

    private int sateCount = 0;

    public void AddSateCount()
    {
        sateCount++;
    }

    public bool CheckSateCount()
    {
        return sateCount >= 4;
    }
}
