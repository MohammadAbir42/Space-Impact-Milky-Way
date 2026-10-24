using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] private GameObject scout;
    [SerializeField] GameObject interceptor;
    [SerializeField] GameObject gunner;
    [SerializeField] GameObject charger;

    [SerializeField] private float spawnX = 9f;
    
    public enum EnemyType
    {
        Scout,
        Interceptor,
        Gunner,
        Charger
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    public void Spawn(EnemyType type, float y)
    {
        GameObject prefab = type switch
        {
            EnemyType.Scout => scout,
            EnemyType.Interceptor => interceptor,
            EnemyType.Gunner => gunner,
            EnemyType.Charger => charger,
            _ => null
        };
        
        if(prefab == null)
        {
            Debug.LogWarning($"Missing Prefab: {type}");
            return;
        }
        
        Vector3 position = new Vector3(spawnX, y , 0f);
        
        Instantiate(prefab, position, prefab.transform.rotation);
    }
}
