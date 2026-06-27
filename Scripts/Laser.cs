using UnityEngine;

public class Laser : MonoBehaviour
{
    public float speed = 10f;
    public float lifeTime = 2f;

    // True = fired by player
    // False = fired by robot
    public bool firedByPlayer = false;

    private Vector2 direction;

    public void SetDirection(Vector2 dir)
    {
        direction = dir.normalized;

        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, angle - 90f);
    }

    void Start()
    {
        Destroy(gameObject, lifeTime);
    }

    void Update()
    {
        transform.position += (Vector3)(direction * speed * Time.deltaTime);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        // Ignore trigger colliders
        if (other.isTrigger)
            return;

        // Destroy laser on walls
        if (other.CompareTag("Wall"))
        {
            Destroy(gameObject);
            return;
        }

        // Player laser
        if (firedByPlayer)
        {
            if (other.CompareTag("Robot"))
            {
                // TODO: Replace this with RobotHealth later
                Destroy(other.gameObject);

                Destroy(gameObject);
            }

            return;
        }

        // Robot laser
        if (other.CompareTag("Player"))
        {
            PlayerHealth health = other.GetComponent<PlayerHealth>();

            if (health != null)
            {
                health.TakeDamage();
            }

            Destroy(gameObject);
        }
    }
}