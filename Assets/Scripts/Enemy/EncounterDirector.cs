using System.Collections;
using UnityEngine;

public class EncounterDirector : MonoBehaviour
{
    [SerializeField] private EnemySpawner enemySpawner;
    
    [Header("Spawn Bounds")]
    [SerializeField] private float minY = -3f;
    [SerializeField] private float maxY = 3f;
    
    [Header("Encounter Timing")]
    [SerializeField] private float breakBetweenCycles = 3f;
    
    private Coroutine encounterCoroutine;
    private bool gameOver;

    void OnEnable()
    {
        encounterCoroutine = StartCoroutine(RunEncounters());
    }

    void OnDisable()
    {
        if(encounterCoroutine != null)
        {
            StopCoroutine(encounterCoroutine);
            encounterCoroutine = null;
        }
    }
    
    public void StartEncounters()
    {
        gameOver = true;
        
        if(encounterCoroutine != null)
        {
            StopCoroutine(encounterCoroutine);
            encounterCoroutine = null;
        }
    }
    
    private float RandomY()
    {
        return Random.Range(minY, maxY);
    }
    
    private IEnumerator RunEncounters()
    {
        yield return new WaitForSeconds(2f);
        
        while(!gameOver)
        {
            float formationY = Random.Range(-2f, 2f);
            // Encounter 1: Scout Patrol
            for(int i = 0; i < 3; i++)
            {
                float y = Mathf.Clamp(
                    formationY + (i -1) * 0.7f,
                    -3f,
                    3f
                );
                enemySpawner.Spawn(EnemySpawner.EnemyType.Scout, y);
                yield return new WaitForSeconds(0.6f);
            }
            
            yield return new WaitForSeconds(2f);
            
            // Encounter 2: Interceptor crosses a firing lane
            enemySpawner.Spawn(
                EnemySpawner.EnemyType.Interceptor,
                Random.value < 0.5f ? maxY : minY
            );
            
            yield return new WaitForSeconds(3f);
            
            // Encounter 3: Protected Gunner
            float gunnerY = Random.Range(-2f, 2f);
            
            enemySpawner.Spawn(
                EnemySpawner.EnemyType.Gunner,
                gunnerY
            );
            
            yield return new WaitForSeconds(0.8f);
            
            enemySpawner.Spawn(
                EnemySpawner.EnemyType.Interceptor,
                Mathf.Clamp(gunnerY + 1.5f, minY, maxY)
            );
            
            yield return new WaitForSeconds(4f);
            
            // Encounter 4: Charger
            enemySpawner.Spawn(
                EnemySpawner.EnemyType.Charger,
                RandomY()
            );
            
            yield return new WaitForSeconds(2f);
            
            // Encounter 5: Scout Distraction
            enemySpawner.Spawn(
                EnemySpawner.EnemyType.Scout,
                RandomY()
            );
            
            yield return new WaitForSeconds(0.7f);
            
            enemySpawner.Spawn(
                EnemySpawner.EnemyType.Scout,
                RandomY()
            );
            
            // short recovery before next cycle
            yield return new WaitForSeconds(breakBetweenCycles);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
