using UnityEngine;

public class DestroyDebugger : MonoBehaviour
{
    void OnDestroy()
    {
        Debug.Log("PLAYER DESTROYED!");
        Debug.Log(System.Environment.StackTrace);
    }
}