using System.Collections;
using UnityEngine;

[RequireComponent(typeof(EnemyShooter))]
public class ScoutController : MonoBehaviour
{
    [SerializeField] private float speed = 3f;
    [SerializeField] private float engageDuration = 15f;
    [SerializeField] private float fireInterval = 2f;
    
    [SerializeField] private Vector2 attackXRange = new(3f, 6f);
    [SerializeField] private Vector2 attackYRange = new(-3f, 3f);
    
    [SerializeField] private Transform firePoint;

    private EnemyShooter shooter;
    private float fireTimer;
    private bool isEngaging = false;

    void Awake()
    {
        shooter = GetComponent<EnemyShooter>();
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    IEnumerator Start()
    {
        float targetX = Random.Range(
            attackXRange.x,
            attackXRange.y
        );
        
        Vector3 firstPosition = new(
            targetX,
            transform.position.y,
            transform.position.z
        );
        
        // enter combat area
        yield return MoveTo(firstPosition);
        
        // Enable shooting
        isEngaging = true;
        
        float elapsed = 0f;
        while(elapsed < engageDuration)
        {   
            Vector3 newPosition = new(
                Random.Range(
                    attackXRange.x,
                    attackXRange.y
                ),
                Random.Range(
                    attackYRange.x,
                    attackYRange.y
                ),
                transform.position.z
            );
            
            float start = Time.time;
            
            yield return MoveTo(newPosition);
            
            elapsed += Time.time - start;
            
            yield return new WaitForSeconds(0.5f);
            elapsed += 0.5f;
        }
        
        // Stop shooting before retreat
        isEngaging = false;
        
        // retreat to the right
        Vector3 retreat = new(
            attackXRange.y + 5f,
            transform.position.y,
            transform.position.z
        );
        
        yield return MoveTo(retreat);
    }
    
    private IEnumerator MoveTo(Vector3 destination)
    {
        while(
            Vector3.Distance(
                transform.position,
                destination
            ) > 0.05f
        )
        {
            transform.position = Vector3.MoveTowards(
                transform.position,
                destination,
                speed * Time.deltaTime
            );
            
            yield return null;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(!isEngaging)
            return;
        
        fireTimer += Time.deltaTime;
        
        if (fireTimer >= fireInterval)
        {
            shooter.FireAimed(firePoint);
            fireTimer -= fireInterval;
        }
    }
}
