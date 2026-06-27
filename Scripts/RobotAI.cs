using UnityEngine;
using System.Collections.Generic;

public class RobotAI : MonoBehaviour
{
    public float moveSpeed = 3f;
    public float visionRadius = 6f;
    public Transform player;
    public LayerMask wallLayer;

    public GameObject laserPrefab;
    public Transform firePoint;

    public float fireCooldown = 1f;
    public float attackRange = 5f;

    private float nextFireTime = 0f;

    private SpriteRenderer spriteRenderer;
    private bool playerDetected = false;

    private Vector2Int currentCell;
    private Vector2Int direction;
    private Vector3 targetPosition;

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();

        if (player == null)
        {
            GameObject p = GameObject.FindGameObjectWithTag("Player");

            if (p != null)
                player = p.transform;
        }

        currentCell = WorldToCell(transform.position);

        PickInitialDirection();

        ChooseNextTarget();
    }

    void Update()
    {
        CheckVision();

        // Stop patrolling while player is detected
        if (playerDetected)
        {
            FacePlayer();
            return;
        }

        transform.position = Vector3.MoveTowards(
            transform.position,
            targetPosition,
            moveSpeed * Time.deltaTime);

        if (Vector3.Distance(transform.position, targetPosition) < 0.02f)
        {
            transform.position = targetPosition;

            currentCell += direction;

            ChooseNextTarget();
        }
    }

    void ChooseNextTarget()
    {
        List<Vector2Int> choices = GetAvailableDirections();

        // Never reverse unless forced
        choices.Remove(-direction);

        if (choices.Count == 0)
        {
            direction = -direction;
        }
        else if (choices.Count == 1)
        {
            direction = choices[0];
        }
        else
        {
            direction = choices[Random.Range(0, choices.Count)];
        }

        UpdateRotation(direction);

        targetPosition = CellToWorld(currentCell + direction);
    }

    List<Vector2Int> GetAvailableDirections()
    {
        List<Vector2Int> result = new List<Vector2Int>();

        Vector2Int[] dirs =
        {
            Vector2Int.up,
            Vector2Int.right,
            Vector2Int.down,
            Vector2Int.left
        };

        foreach (Vector2Int dir in dirs)
        {
            Vector2Int next = currentCell + dir;

            if (next.x < 0 ||
                next.y < 0 ||
                next.x >= MazeGenerator.width ||
                next.y >= MazeGenerator.height)
                continue;

            if (MazeGenerator.maze[next.x, next.y] == 0)
            {
                result.Add(dir);
            }
        }

        return result;
    }

    void PickInitialDirection()
    {
        List<Vector2Int> choices = GetAvailableDirections();

        direction = choices[Random.Range(0, choices.Count)];

        UpdateRotation(direction);
    }

    Vector2Int WorldToCell(Vector3 worldPos)
    {
        return new Vector2Int(
            Mathf.RoundToInt(worldPos.x / MazeGenerator.tileSize),
            Mathf.RoundToInt(worldPos.y / MazeGenerator.tileSize)
        );
    }

    Vector3 CellToWorld(Vector2Int cell)
    {
        return new Vector3(
            cell.x * MazeGenerator.tileSize,
            cell.y * MazeGenerator.tileSize,
            0
        );
    }

    void CheckVision()
    {
        if (player == null)
            return;

        Vector2 directionToPlayer = player.position - transform.position;
        float distance = directionToPlayer.magnitude;

        if (distance > visionRadius)
        {
            playerDetected = false;
            spriteRenderer.color = Color.red;
            return;
        }

        RaycastHit2D hit = Physics2D.Raycast(
            transform.position,
            directionToPlayer.normalized,
            distance,
            wallLayer
        );

        if (hit.collider == null)
        {
            playerDetected = true;
            spriteRenderer.color = Color.yellow;

            if (distance <= attackRange)
            {
                Shoot();
            }
        }
        else
        {
            playerDetected = false;
            spriteRenderer.color = Color.red;
        }
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

    // This laser belongs to the robot
    laserScript.firedByPlayer = false;

    Vector2 laserDirection =
        (player.position - firePoint.position).normalized;

    laserScript.SetDirection(laserDirection);
}

    void UpdateRotation(Vector2Int dir)
    {
        if (dir == Vector2Int.up)
            transform.rotation = Quaternion.Euler(0, 0, 0);
        else if (dir == Vector2Int.right)
            transform.rotation = Quaternion.Euler(0, 0, -90);
        else if (dir == Vector2Int.down)
            transform.rotation = Quaternion.Euler(0, 0, 180);
        else if (dir == Vector2Int.left)
            transform.rotation = Quaternion.Euler(0, 0, 90);
    }

    void FacePlayer()
    {
        Vector2 lookDir = player.position - transform.position;

        float angle = Mathf.Atan2(lookDir.y, lookDir.x) * Mathf.Rad2Deg;

        transform.rotation = Quaternion.Euler(0, 0, angle - 90);
    }
}