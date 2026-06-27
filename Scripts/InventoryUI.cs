using UnityEngine;
using TMPro;

public class InventoryUI : MonoBehaviour
{
    public static InventoryUI Instance;

    public TMP_Text inventoryText;

    private void Awake()
    {
        Instance = this;
    }

    public void Refresh()
    {
        if (inventoryText == null || Inventory.Instance == null)
            return;

        inventoryText.text =
            $"X: {Inventory.Instance.materialX}/2\n" +
            $"Y: {Inventory.Instance.materialY}/2\n" +
            $"Z: {Inventory.Instance.materialZ}/2";
    }

    void Start()
    {
        Refresh();
    }
}