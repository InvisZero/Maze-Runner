using UnityEngine;
using UnityEngine.InputSystem;

public class CheckpointPlacer : MonoBehaviour
{
    public GameObject checkpointPrefab;

    public float minimumCheckpointDistance = 5f;

    private int checkpointsPlaced = 0;

    void Update()
    {
        if (Keyboard.current.cKey.wasPressedThisFrame)
        {
            if (checkpointsPlaced >= 2)
            {
                Debug.Log("Maximum checkpoints placed.");
                return;
            }

            if (!Inventory.Instance.CanCraftCheckpoint())
            {
                Debug.Log("Not enough materials.");
                return;
            }

            // Check distance from existing checkpoints
            GameObject[] checkpoints = GameObject.FindGameObjectsWithTag("Checkpoint");

            foreach (GameObject checkpoint in checkpoints)
            {
                float distance = Vector2.Distance(
                    transform.position,
                    checkpoint.transform.position
                );

                if (distance < minimumCheckpointDistance)
                {
                    Debug.Log("Too close to another checkpoint.");
                    return;
                }
            }

            Inventory.Instance.ConsumeCheckpointMaterials();

            Instantiate(
                checkpointPrefab,
                transform.position,
                Quaternion.identity
            );

            checkpointsPlaced++;

            Debug.Log("Checkpoint Placed!");
        }
    }
}