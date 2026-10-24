using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    [SerializeField] private int maxHealth = 6;
    [SerializeField] private SpriteRenderer spriteRenderer;
    
    [SerializeField] private int currentHealth;

    void Awake()
    {
        currentHealth = maxHealth;
        
        if(spriteRenderer == null)
            spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    public void TakeDamage(int damage)
    {
        if (damage <= 0 || currentHealth <= 0)
            return;
            
        currentHealth -= damage;
        
        if (currentHealth <= 0)
            Die();
    }
    
    private void Die()
    {
        Destroy(gameObject);
    }
}
