using System.Collections;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    [Header("Health")]
    [SerializeField] private int maxHealth = 30;
    
    [Header("Damage Feedback")]
    [SerializeField] private float invincibilityDuration = 2f;
    [SerializeField] private float flashInterval = 0.1f;
    [SerializeField] private Color hitColor = Color.orange;
    [SerializeField] private SpriteRenderer spriteRenderer;
    
    [SerializeField] private int currentHealth;
    private bool isInvincible;
    private bool isDead;
    private Color originalColor;
    
    public int CurrentHealth => currentHealth;
    public int MaxHealth => maxHealth;
    
    void Awake()
    {
        currentHealth = maxHealth;
        
        if(spriteRenderer == null)
            spriteRenderer = GetComponentInChildren<SpriteRenderer>();
            
        if(spriteRenderer != null)
            originalColor = spriteRenderer.color;
    }
    
    public void TakeDamage(int damage)
    {
        if (isDead || isInvincible || damage <= 0)
            return;
        
        currentHealth = Mathf.Max(0, currentHealth - damage);
        
        if(currentHealth <= 0)
        {
            Die();
            return;
        }
        
        StartCoroutine(DamageFeedback());
    }
    
    private IEnumerator DamageFeedback()
    {
        isInvincible = true;
        
        float elapsed = 0f;
        bool flashingRed = false;
        
        while(elapsed < invincibilityDuration)
        {
            flashingRed = !flashingRed;
            
            if(spriteRenderer != null)
                spriteRenderer.color = flashingRed ? hitColor : originalColor;
            
            yield return new WaitForSeconds(flashInterval);
            
            elapsed += flashInterval;
        }
        
        if(spriteRenderer != null)
            spriteRenderer.color = originalColor;
        
        isInvincible = false;
    }
    
    void Die()
    {
        isDead = true;
        
        if(spriteRenderer != null)
            spriteRenderer.color = Color.red;
        
        Destroy(gameObject);
    }
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
