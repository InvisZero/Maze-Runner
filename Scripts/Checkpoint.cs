using UnityEngine;
using TMPro;

public class Checkpoint : MonoBehaviour
{
    public int charges;

    public TMP_Text chargeText;

    void Start()
    {
        // Random charges between 2 and 5
        charges = Random.Range(2, 6);

        UpdateText();

        // Register this checkpoint
        if (CheckpointManager.Instance != null)
        {
            CheckpointManager.Instance.RegisterCheckpoint(this);
        }
    }

    public void UseCharge()
    {
        charges--;

        UpdateText();

        if (charges <= 0)
        {
            Destroy(gameObject);
        }
    }

    void UpdateText()
    {
        if (chargeText != null)
        {
            chargeText.text = charges.ToString();
        }
    }

    private void OnDestroy()
    {
        if (CheckpointManager.Instance != null)
        {
            CheckpointManager.Instance.RemoveCheckpoint(this);
        }
    }
}