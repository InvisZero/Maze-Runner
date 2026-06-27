using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerShooting : MonoBehaviour
{
    public GameObject laserPrefab;
    public Transform firePoint;

    public float fireCooldown = 0.25f;

    private float nextFireTime = 0f;

    void Update()
    {
        AimAtMouse();

        if (Mouse.current.leftButton.wasPressedThisFrame)
        {
            Shoot();
        }
    }

    void AimAtMouse()
    {
        Vector3 mouse = Mouse.current.position.ReadValue();
        mouse = Camera.main.ScreenToWorldPoint(mouse);
        mouse.z = 0f;

        Vector2 dir = mouse - transform.position;

        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;

        transform.rotation = Quaternion.Euler(0, 0, angle - 90f);
    }

    void Shoot()
    {
        if (Time.time < nextFireTime)
            return;

        nextFireTime = Time.time + fireCooldown;

        GameObject laser = Instantiate(
            laserPrefab,
            firePoint.position,
            Quaternion.identity
        );

        Laser laserScript = laser.GetComponent<Laser>();

        laserScript.firedByPlayer = true;

        Vector2 dir =
            ((Vector2)Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue()) -
             (Vector2)firePoint.position).normalized;

        laserScript.SetDirection(dir);
    }
}