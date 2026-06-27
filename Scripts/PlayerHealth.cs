using UnityEngine;
using TMPro;
using System.Collections;

public class PlayerHealth : MonoBehaviour
{
    public TMP_Text heartsText;

    public int maxHearts = 3;
    public int currentHearts;

    public float invincibleTime = 2f;

    private bool invincible = false;

    void Start()
    {
        currentHearts = maxHearts;
        UpdateHearts();

        if (GameUIManager.Instance != null)
        {
            GameUIManager.Instance.SetPlayer(this);
        }
    }

    public void TakeDamage()
    {
        if (invincible)
            return;

        currentHearts--;

        UpdateHearts();

        if (currentHearts <= 0)
        {
            Die();
            return;
        }

        StartCoroutine(Invincibility());
    }

    void Die()
    {
        Checkpoint checkpoint = CheckpointManager.Instance.GetLatestCheckpoint();

        if (checkpoint != null)
        {
            GameUIManager.Instance.ShowRespawn();
        }
        else
        {
            GameUIManager.Instance.ShowGameOver();
        }
    }

    public void RespawnFromCheckpoint()
    {
        Checkpoint checkpoint = CheckpointManager.Instance.GetLatestCheckpoint();

        if (checkpoint == null)
            return;

        Rigidbody2D rb = GetComponent<Rigidbody2D>();

        if (rb != null)
        {
            rb.linearVelocity = Vector2.zero;
            rb.angularVelocity = 0f;
            rb.position = checkpoint.transform.position;
        }
        else
        {
            transform.position = checkpoint.transform.position;
        }

        checkpoint.UseCharge();

        currentHearts = maxHearts;

        UpdateHearts();

        StartCoroutine(Invincibility());

        Debug.Log("Respawned");
    }

    IEnumerator Invincibility()
    {
        invincible = true;

        yield return new WaitForSeconds(invincibleTime);

        invincible = false;
    }

    void UpdateHearts()
    {
        if (heartsText == null)
            return;

        string hearts = "";

        for (int i = 0; i < maxHearts; i++)
        {
            if (i < currentHearts)
                hearts += "♥";
            else
                hearts += "×";
        }

        heartsText.text = hearts;
    }
}