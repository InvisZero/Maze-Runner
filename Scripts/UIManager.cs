using UnityEngine;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;

    public GameObject nextMapText;

    private void Awake()
    {
        Instance = this;

        if (nextMapText != null)
            nextMapText.SetActive(false);
    }

    public void ShowNextMapText()
    {
        if (nextMapText != null)
            nextMapText.SetActive(true);
    }
}