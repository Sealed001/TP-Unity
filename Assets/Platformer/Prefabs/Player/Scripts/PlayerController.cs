using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public abstract class DeathContext<T>
{
    public float RespawnDuration = 5f;
    public T Data;
}

#region Drowning

public struct DrowningData
{
    public Vector3 BubblesSpawnPoint;
}
public class DrowningContext : DeathContext<DrowningData>
{
    public DrowningContext(DrowningData drowningData)
    {
        Data = drowningData;
    }
}

#endregion

public class PlayerController : MonoBehaviour
{
    public bool canAct = true;
    public GameObject bubblesPadParticlesPrefab;

    public void Die<T>(DeathContext<T> deathContext)
    {
        StartCoroutine(DieCoroutine(deathContext));
    }

    private IEnumerator DieCoroutine<T>(DeathContext<T> deathContext)
    {
        canAct = false;

        switch (deathContext)
        {
            case DrowningContext drowningContext:
                Instantiate(bubblesPadParticlesPrefab, drowningContext.Data.BubblesSpawnPoint, Quaternion.identity);
                break;
        }

        yield return new WaitForSeconds(deathContext.RespawnDuration);
        
        ReSpawn();
        
        canAct = true;
    }

    private void ReSpawn()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
