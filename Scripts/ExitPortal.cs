using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class ExitPortal : MonoBehaviour
{
    private bool loading = false;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (loading) return;

        if (other.CompareTag("Player"))
        {
            loading = true;
            StartCoroutine(LoadNextMap());
        }
    }

    IEnumerator LoadNextMap()
    {
        UIManager.Instance.ShowNextMapText();

        SeedManager.currentSeed++;
        SeedManager.worldLevel++;
        
        if (HUDManager.Instance != null)
{
    HUDManager.Instance.RefreshWorld();
}

        yield return new WaitForSeconds(1f);

        SceneManager.LoadScene(
            SceneManager.GetActiveScene().buildIndex
        );
    }
}