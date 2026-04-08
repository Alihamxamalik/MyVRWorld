using UnityEngine;

public class BulitScript : MonoBehaviour
{
    [Header("Bullet Settings")]
    public float speed = 1f;
    public float damage = 10f;
    public float range = 50f;

    private Vector3 direction;
    private Vector3 startPosition;
    private bool isMoving;

    private void Update()
    {
        if (!isMoving) return;

        // Move bullet
        transform.position += direction * speed * Time.deltaTime;

        // Check range
        float distanceTravelled = Vector3.Distance(startPosition, transform.position);
        if (distanceTravelled >= range)
        {
            Destroy(gameObject);
        }
    }

    public void StartMoving(Vector3 forwardDirection)
    {
        direction = forwardDirection.normalized;
        startPosition = transform.position;
        isMoving = true;

        transform.rotation = Quaternion.LookRotation(direction);
    }

    private void OnTriggerEnter(Collider other)
    {
        // Try to apply damage if target has health component
        
        Destroy(gameObject);
    }
}
