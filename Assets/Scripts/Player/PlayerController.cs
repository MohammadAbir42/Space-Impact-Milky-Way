using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float speed = 10.0f;
    [SerializeField] private InputActionReference moveInput;
    [SerializeField] private InputActionReference fireInput;
    [SerializeField] private Transform firePoint;
    [SerializeField] private float bulletSpeed = 15f;
    
    [SerializeField, Min(0.1f)]
    private float fireRate = 3f; // Minimum time between shots in seconds
    
    private float fireCooldown = 0f; // Time when the player can fire again
    
    private Camera mainCamera;
    
    public GameObject bullet;
    
    void Awake()
    {
        mainCamera = Camera.main;
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        transform.position = new Vector3(-7.0f, 0f, 0f);
    }

    // Update is called once per frame
    void Update()
    {
        MoveAction();
        FireAction();
        ClampToScreen();
    }
    
    
    void OnEnable()
    {
        moveInput.action.Enable();
        fireInput.action.Enable();
    }
    
    void OnDisable()
    {
        moveInput.action.Disable();
        fireInput.action.Disable();
    }

    void MoveAction()
    {
        Vector2 input = moveInput.action.ReadValue<Vector2>();
        
        Vector3 movement = new Vector3(input.x, input.y, 0f);
        
        movement = Vector3.ClampMagnitude(movement, 1.0f);
        
        transform.position += movement * speed * Time.deltaTime;
    }
    
    void FireAction()
    {
        float interval = 1f / fireRate;
        
        if(!fireInput.action.IsPressed())
        {
            fireCooldown = 0f;
            return;
        }
        
        fireCooldown -= Time.deltaTime;
        
        if(fireCooldown <= 0f)
        {
            Instantiate(bullet, firePoint.position, firePoint.rotation);
            
            BulletController bulletController = bullet.GetComponent<BulletController>();
            
            bulletController.Initialize(bulletSpeed);
            
            fireCooldown += interval;
        }
    }
    
    void ClampToScreen()
    {
        float halfHeight = mainCamera.orthographicSize;
        float halfWidth = halfHeight * mainCamera.aspect;

        Vector3 cameraPosition = mainCamera.transform.position;

        float left = cameraPosition.x - halfWidth;
        float right = cameraPosition.x + halfWidth;
        float bottom = cameraPosition.y - halfHeight;
        float top = cameraPosition.y + halfHeight;

        // Standard Unity square: 1 x 1 world units
        float playerHalfWidth = Mathf.Abs(transform.lossyScale.x) / 2f;
        float playerHalfHeight = Mathf.Abs(transform.lossyScale.y) / 2f;

        Vector3 position = transform.position;

        position.x = Mathf.Clamp(
            position.x,
            left + playerHalfWidth,
            right - playerHalfWidth
        );

        position.y = Mathf.Clamp(
            position.y,
            bottom + playerHalfHeight,
            top - playerHalfHeight
        );

        transform.position = position;
    }
}
