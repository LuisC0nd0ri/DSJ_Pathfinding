using Pathfinding;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemigoQueDispara : MonoBehaviour
{

    //para que el raycast evite collider no deseados
    [SerializeField] private LayerMask visionMask;

    public AIPath aIPath;
    Vector2 direction;

    public GameObject bulletPrefab;
    public float shootCooldown = 2f;
    public float shootRange = 20f;

    private float shootTimer = 0f;
    private Transform player;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Jugador").transform;
    }

    // Update is called once per frame
    void Update()
    {
        faceVelocity();

        shootTimer -= Time.deltaTime;

        if (player != null && Vector2.Distance(transform.position, player.position) <= shootRange)
        {
            Vector2 direction = (player.position - transform.position).normalized;

            // Lanzamos un raycast desde el enemigo hacia el jugador
            RaycastHit2D hit = Physics2D.Raycast(transform.position, direction, shootRange, visionMask);

            if (hit.collider != null)
            {
                Debug.Log("Raycast hit: " + hit.collider.name);
            }
            else
            {
                Debug.Log("Raycast hit no golpeo ni un brillo");
            }

            // Si el raycast golpea al jugador directamente
            if (hit.collider != null && 
                hit.collider.CompareTag("Jugador"))
            {
                if (shootTimer <= 0f)
                {
                    Shoot();
                    shootTimer = shootCooldown;
                }
            }
        }
    }

    void Shoot()
    {
        Vector3 spawnPos = transform.position + transform.right * 2.5f; // un poco al frente del enemigo
        Instantiate(bulletPrefab, spawnPos, transform.rotation);
    }

    void faceVelocity()
    {
        direction = aIPath.desiredVelocity;
        transform.right = direction;
    }

}
