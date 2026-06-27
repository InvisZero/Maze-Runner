using UnityEngine;

public class SeedManager : MonoBehaviour
{
    public int startingSeed = 289034;

    public static int currentSeed;
    public static int worldLevel = 1;

    private void Awake()
    {
        if (currentSeed == 0)
        {
            currentSeed = startingSeed;
        }

        Random.InitState(currentSeed);
    }
}