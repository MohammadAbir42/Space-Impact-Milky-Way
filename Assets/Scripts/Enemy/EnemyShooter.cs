using UnityEngine;

public class EnemyShooter : MonoBehaviour
{
    [SerializeField] EnemyBulletController enemyBullet;
    [SerializeField] private float speedAngle = 15f;
    
    private Transform player;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        GameObject playerObject = GameObject.FindGameObjectWithTag("Player");
        
        if (playerObject != null)
            player = playerObject.transform;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    public void FireAimed(params Transform[] firePoints)
    {
        if(player == null) return;
        
        foreach(Transform firePoint in firePoints)
        {
            Vector2 direction = (player.position - firePoint.position).normalized;
        
            SpawnBullet(direction, firePoint);
        }
    }
    
    public void FireTripple(Transform firePoint)
    {
        if(player == null) return;
        
        Vector2 direction = (player.position - firePoint.position).normalized;
        
        SpawnBullet(RotateBullet(direction, -speedAngle), firePoint);
        SpawnBullet(direction, firePoint);
        SpawnBullet(RotateBullet(direction, speedAngle), firePoint);
    }
    
    private Vector2 RotateBullet(Vector2 direction, float degrees)
    {
        float radians = degrees * Mathf.Deg2Rad;
        
        return new Vector2(
            direction.x * Mathf.Cos(radians) - direction.y * Mathf.Sin(radians),
            direction.x * Mathf.Sin(radians) + direction.y * Mathf.Cos(radians)
        );
    }
    
    private void SpawnBullet(Vector2 direction, Transform firePoint)
    {
        EnemyBulletController bullet = Instantiate(
            enemyBullet,
            firePoint.position,
            firePoint.rotation
        );
        
        bullet.Initialize(direction);
    }
}
