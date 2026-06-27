using UnityEngine;

public class MaterialPickup : MonoBehaviour
{
    public string materialType;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player"))
            return;

        Inventory.Instance.AddMaterial(materialType);

        Destroy(gameObject);
    }
}