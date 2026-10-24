using UnityEngine;

public class EnemyBulletController : MonoBehaviour
{
    [SerializeField] private float speed = 10f;
    [SerializeField] private float lifetime = 5f;
    [SerializeField] private int damage = 1;
    
    private PlayerController player;
    
    
    private Vector2 direction = Vector2.left;
    
    public void Initialize(Vector2 newDirection)
    {
        direction = newDirection.normalized;
    }
    
    void Awake()
    {
        player = GetComponent<PlayerController>();
    }
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Destroy(gameObject, lifetime);
    }

    // Update is called once per frame
    void Update()
    {
        transform.position += (Vector3)(direction * speed * Time.deltaTime);
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.CompareTag("Player"))
            return;
        
        PlayerHealth playerHealth = collision.GetComponentInParent<PlayerHealth>();
        
        if(playerHealth == null)
            return;
        // Temp: Destroy Health
        int actualDMG = Random.Range(damage, 5);
        playerHealth.TakeDamage(actualDMG);

        Destroy(gameObject);
    }
}
