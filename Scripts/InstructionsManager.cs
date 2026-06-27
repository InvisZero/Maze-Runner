using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;

public class InstructionsManager : MonoBehaviour
{
    private bool canContinue = false;

    void Start()
    {
        Invoke(nameof(EnableInput), 0.3f);
    }

    void EnableInput()
    {
        canContinue = true;
    }

    void Update()
    {
        if (!canContinue)
            return;

        if (Keyboard.current.anyKey.wasPressedThisFrame)
        {
            Time.timeScale = 1f;
            SeedManager.worldLevel = 1;

            SceneManager.LoadScene(2); // Your Game scene
        }
    }
}