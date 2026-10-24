using System.Collections;
using UnityEngine;

public class ChargerController : MonoBehaviour
{
    [SerializeField] private float approachSpeed = 2f;
    [SerializeField] private float chargeSpeed = 9f;
    [SerializeField] private float approachDirection = 1.5f;
    [SerializeField] private float warningDuration = 0.7f;
    
    private float currentSpeed;
    private SpriteRenderer spriteRenderer;

    void Awake()
    {
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        currentSpeed = approachSpeed;
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        StartCoroutine(ChargeSquence());
    }

    // Update is called once per frame
    void Update()
    {
        transform.position += Vector3.left * currentSpeed * Time.deltaTime;
    }
    
    private IEnumerator ChargeSquence()
    {
        // approach
        yield return new WaitForSeconds(approachDirection);
        
        currentSpeed = 0f;
        
        // Record player's current firing lane
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        
        float targetY = player != null
            ? player.transform.position.y
            : transform.position.y;
        
        // Warning: Flash before charging
        if(spriteRenderer != null)
        {
            spriteRenderer.color = Color.yellow;
        }
        
        // Stop visibility move into the selected lane
        float elapsed = 0f;
        float startY = transform.position.y;
        
        while(elapsed < warningDuration)
        {
            elapsed += Time.deltaTime;
            
            float progress = Mathf.Clamp01(elapsed / warningDuration);
            
            Vector3 position = transform.position;
            
            position.y = Mathf.Lerp(
                startY,
                targetY,
                progress
            );
            
            transform.position = position;
            
            yield return null;
        }
        
        yield return new WaitForSeconds(warningDuration);
        if(spriteRenderer != null)
            spriteRenderer.color = Color.red;
            
        currentSpeed = chargeSpeed;
    }
    
    private void OnTriggerEnter2D(Collider2D collision)
    {
        PlayerHealth playerHealth = collision.GetComponentInParent<PlayerHealth>();
        if(playerHealth == null) return;
        if(collision.CompareTag("Player"))
        {
            int damage = Random.Range(20, 25);
            playerHealth.TakeDamage(damage);
        }
            
    }
}
