using UnityEngine;
using TMPro;

public class HUDManager : MonoBehaviour
{
    public static HUDManager Instance;

    public TMP_Text worldText;
    public TMP_Text checkpointText;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        RefreshWorld();
        RefreshCheckpointCount();
    }

    public void RefreshWorld()
    {
        if (worldText != null)
            worldText.text = "World " + SeedManager.worldLevel;
    }

    public void RefreshCheckpointCount()
    {
        if (checkpointText == null)
            return;

        if (CheckpointManager.Instance == null)
            return;

        checkpointText.text =
            "Checkpoints : " + CheckpointManager.Instance.GetCheckpointCount();
    }
}