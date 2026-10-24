using System.Collections;
using UnityEngine;

[RequireComponent(typeof(EnemyShooter))]
public class InterceptorController : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] private float speed = 2.8f;
    [SerializeField] private float engageDuration = 14f;
    [SerializeField] private float trackingStrength = 0.7f;
    [SerializeField] private float repositionDelay = 0.4f;
    [SerializeField] private Vector2 attackXRange = new(1f, 4f);
    [SerializeField] private Vector2 attackYRange = new(-3f, 3f);
    
    [Header("Shooting")]
    [SerializeField] private Transform firePoint;
    [SerializeField] private float fireInterval = 2.5f;
    [SerializeField] private float burstDelay = 0.2f;
    
    private EnemyShooter shooter;
    private Transform player;
    private float fireTimer;
    private bool isEngaging;
    private bool isBursting;

    void Awake()
    {
        shooter = GetComponent<EnemyShooter>();
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    IEnumerator Start()
    {
        GameObject playerObject = GameObject.FindGameObjectWithTag("Player");
        
        if(playerObject != null)
            player = playerObject.transform;
            
        // Entered Contested Combat Area
        Vector3 entryPosition = new(
            Random.Range(
                attackXRange.x,
                attackXRange.y
            ),
            transform.position.y,
            transform.position.z
        );
        
        yield return MoveTo(entryPosition);
        
        isEngaging = true;
        
        float endTime = Time.time + engageDuration;
        
        while(Time.time < endTime)
        {
            float targetY = Random.Range(
                attackYRange.x,
                attackYRange.y
            );
            
            // Bias movement toward players firing lane
            if(player != null)
            {
                targetY = Mathf.Lerp(
                    targetY,
                    player.position.y,
                    trackingStrength
                );
            }
            
            targetY = Mathf.Clamp(
                targetY,
                attackYRange.x,
                attackYRange.y
            );
            
            Vector3 targetPosition = new(
                Random.Range(
                    attackXRange.x,
                    attackXRange.y
                ),
                targetY,
                transform.position.z
            );
            
            yield return MoveTo(targetPosition);
            
            yield return new WaitForSeconds(repositionDelay);
        }
        
        isEngaging = false;
        
        Vector3 retreatPosition = new(
            attackXRange.y + 6f,
            transform.position.y,
            transform.position.z
        );
        
        yield return MoveTo(retreatPosition);
    }

    // Update is called once per frame
    void Update()
    {
        if(!isEngaging || isBursting)
            return;
        
        fireTimer += Time.deltaTime;
        
        if(fireTimer >= fireInterval)
        {
            StartCoroutine(FireBrust());
            fireTimer -= fireInterval;
        }
    }
    
    private IEnumerator FireBrust()
    {
        isBursting = true;
        
        if(firePoint != null)
            shooter.FireAimed(firePoint);
        
        yield return new WaitForSeconds(burstDelay);
        
        if(isEngaging && firePoint != null)
            shooter.FireAimed(firePoint);
            
        isBursting = false;
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
