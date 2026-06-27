using UnityEngine;

public class Inventory : MonoBehaviour
{
    public static Inventory Instance;

    public int materialX;
    public int materialY;
    public int materialZ;

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
            return;
        }
    }

    public bool AddMaterial(string type)
    {
        switch (type)
        {
            case "X":
                if (materialX >= 2)
                    return false;

                materialX++;
                break;

            case "Y":
                if (materialY >= 2)
                    return false;

                materialY++;
                break;

            case "Z":
                if (materialZ >= 2)
                    return false;

                materialZ++;
                break;
        }

        if (InventoryUI.Instance != null)
            InventoryUI.Instance.Refresh();

        Debug.Log($"X:{materialX}/2  Y:{materialY}/2  Z:{materialZ}/2");
        return true;
    }

    public bool CanCraftCheckpoint()
    {
        return materialX > 0 &&
               materialY > 0 &&
               materialZ > 0;
    }

    public void ConsumeCheckpointMaterials()
    {
        materialX--;
        materialY--;
        materialZ--;

        if (InventoryUI.Instance != null)
            InventoryUI.Instance.Refresh();

        Debug.Log($"Crafted Checkpoint | X:{materialX}/2  Y:{materialY}/2  Z:{materialZ}/2");
    }
}