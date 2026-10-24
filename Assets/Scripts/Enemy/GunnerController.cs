using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

[RequireComponent(typeof(EnemyShooter))]
public class GunnerController : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] private float speed = 2.5f;
    [SerializeField] private Vector2 attackXRange = new(4f, 6f);
    [SerializeField] private Vector2 attackYRange = new(-3f, 3f);
    
    [Header("Combat")]
    [SerializeField] private Transform firePoint1;
    [SerializeField] private Transform firePoint2;
    [SerializeField] private Transform firePoint;
    [SerializeField] private int attackCycles = 6;
    [SerializeField] private float aimDuration = 0.8f;
    [SerializeField] private float fireInterval = 1.8f;
    
    private EnemyShooter shooter;
    private float fireTimer;
    
    void Awake()
    {
        shooter = GetComponent<EnemyShooter>();
    }
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    IEnumerator Start()
    {
        Vector3 entryPosition = new(
            Random.Range(
                attackXRange.x,
                attackXRange.y
            ),
            transform.position.y,
            transform.position.z
        );
        
        // Enter playfield
        yield return MoveTo(entryPosition);
        
        // repositioning
        for(int i = 0; i < attackCycles; i++)
        {
            // Aim at the player's current position
            yield return new WaitForSeconds(aimDuration);
            
            if(firePoint != null)
                shooter.FireTripple(firePoint);
            
            // stay breiefly in a position
            yield return new WaitForSeconds(fireInterval);
            
            Vector3 nextPosition = new(
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
            
            yield return MoveTo(nextPosition);
        }
        
        // Retreating after completing attack cycles
        Vector3 retreat = new(
            attackXRange.y + 6f,
            transform.position.y,
            transform.position.z
        );
        
        yield return MoveTo(retreat);
    }

    // Update is called once per frame
    void Update()
    {
        fireTimer += Time.deltaTime;
        
        if(fireTimer >= fireInterval)
        {
            shooter.FireAimed(firePoint1, firePoint2);
            fireTimer -= fireInterval;
        }
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
}
