using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunShooting : MonoBehaviour
{
    public GameObject bulletPrefab;
    public float bulletSpeed = 5f;
    public float fireRate = 1f;
    public float health = 3f;

    // NEW: Bullet spawn offset (position relative to gun)
    public Vector2 bulletSpawnOffset = Vector2.zero;

    private float nextFireTime = 0f;
    private bool isDestroying = false;
    private SpriteRenderer gunSprite;

    void Start()
    {
        // Get the sprite renderer
        gunSprite = GetComponent<SpriteRenderer>();

        // Log that gun is working
        Debug.Log("Gun started! Shooting at " + fireRate + " shots per second");
        Debug.Log("Bullet spawn offset: " + bulletSpawnOffset);
    }

    void Update()
    {
        // Auto-shoot on a timer
        if (Time.time >= nextFireTime && !isDestroying)
        {
            ShootStraight();
            nextFireTime = Time.time + (1f / fireRate);
        }
    }

    void ShootStraight()
    {
        // Calculate spawn position with offset
        Vector3 spawnPosition = transform.position + (Vector3)bulletSpawnOffset;

        // Create bullet at calculated position
        GameObject bullet = Instantiate(bulletPrefab, spawnPosition, Quaternion.identity);

        // Get bullet's rigidbody
        Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();

        if (rb != null)
        {
            // Shoot bullet straight to the right (positive X direction)
            rb.linearVelocity = Vector2.right * bulletSpeed;
            Debug.Log("Bullet fired from position: " + spawnPosition);
        }
        else
        {
            Debug.LogError("Bullet has no Rigidbody2D!");
        }

        // Destroy bullet after 3 seconds
        Destroy(bullet, 3f);
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Zombie") && !isDestroying)
        {
            Debug.Log("Zombie touched the gun!");
            isDestroying = true;
            StartCoroutine(DestroyGunAfterDelay());

            // Change gun color to show it's damaged
            if (gunSprite != null)
            {
                gunSprite.color = Color.red;
            }
        }
    }

    IEnumerator DestroyGunAfterDelay()
    {
        // Wait for specified seconds
        yield return new WaitForSeconds(health);

        // Destroy the gun
        Debug.Log("Gun destroyed!");
        Destroy(gameObject);
    }
}