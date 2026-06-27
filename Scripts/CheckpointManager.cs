using UnityEngine;
using System.Collections.Generic;

public class CheckpointManager : MonoBehaviour
{
    public static CheckpointManager Instance;

    private List<Checkpoint> checkpoints = new List<Checkpoint>();

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void RegisterCheckpoint(Checkpoint checkpoint)
    {
        if (!checkpoints.Contains(checkpoint))
        {
            checkpoints.Add(checkpoint);

            if (HUDManager.Instance != null)
            {
                HUDManager.Instance.RefreshCheckpointCount();
            }
        }

        Debug.Log("Checkpoint Registered. Total: " + checkpoints.Count);
    }

    public void RemoveCheckpoint(Checkpoint checkpoint)
    {
        checkpoints.Remove(checkpoint);

        if (HUDManager.Instance != null)
        {
            HUDManager.Instance.RefreshCheckpointCount();
        }

        Debug.Log("Checkpoint Removed. Total: " + checkpoints.Count);
    }

    public Checkpoint GetLatestCheckpoint()
    {
        if (checkpoints.Count == 0)
            return null;

        return checkpoints[checkpoints.Count - 1];
    }

    public int GetCheckpointCount()
    {
        return checkpoints.Count;
    }
}