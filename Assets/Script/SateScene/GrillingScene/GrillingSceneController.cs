using UnityEngine;
using UnityEngine.SceneManagement;

public class GrillingSceneController : MonoBehaviour
{
    public int totalRequired = 4; 
    private int droppedCount = 0;
    public bool passed = false;

    public void RegisterDrop()
    {
        droppedCount++;
        Debug.Log("Dropped: " + droppedCount + "/" + totalRequired);

        if (droppedCount >= totalRequired)
        {
            passed = true;
        }
    }
}
