using UnityEngine;

public class EnemyDespawn : MonoBehaviour
{
    private Camera mainCamera;
    private SpriteRenderer spriteRenderer;
    private bool hasEnteredScreen;

    void Awake()
    {
        mainCamera = Camera.main;
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (mainCamera == null || spriteRenderer == null)
            return;
        
        Bounds bounds = spriteRenderer.bounds;
        
        Vector3 min = mainCamera.WorldToViewportPoint(bounds.min);
        Vector3 max = mainCamera.WorldToViewportPoint(bounds.max);
        
        bool overlapScreen = 
            max.x >= 0f &&
            min.x <= 1f &&
            max.y >= 0f &&
            min.y <= 1f;
            
        if (overlapScreen)
            hasEnteredScreen = true;
            
        if (hasEnteredScreen && !overlapScreen)
        {
            Destroy(gameObject);
        }
    }
}
