
using UnityEngine;

public class BulletController : MonoBehaviour
{
    [SerializeField] private float speed = 25f;
    [SerializeField] private float lifetime = 2f;
    
    public void Initialize(float bulletSpeed)
    {
        speed = bulletSpeed;
    }

    void Start()
    {
        Destroy(gameObject, lifetime);
    }

    void Update()
    {
        transform.position += Vector3.right * speed * Time.deltaTime;
    }
    
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            return; // Ignore collision with the player
        }
    }
}
