using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZombieAI : MonoBehaviour
{
    public float moveSpeed = 1.5f;
    public int health = 2; // Number of hits to kill zombie
    public float walkDirection = -1f; // -1 = left, 1 = right

    private Rigidbody2D rb;
    private SpriteRenderer zombieSprite;
    private int currentHealth;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        zombieSprite = GetComponent<SpriteRenderer>();
        currentHealth = health; // Set starting health

        Debug.Log("Zombie started walking " + (walkDirection < 0 ? "left" : "right"));
    }

    void FixedUpdate()
    {
        // Walk in straight line (constant direction)
        Vector2 movement = new Vector2(walkDirection, 0) * moveSpeed * Time.fixedDeltaTime;
        rb.MovePosition(rb.position + movement);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        // If zombie touches bullet
        if (other.CompareTag("Bullet"))
        {
            // Reduce health by 1
            currentHealth--;

            // Destroy the bullet
            Destroy(other.gameObject);

            Debug.Log("Zombie hit! Health: " + currentHealth + "/" + health);

            // Visual feedback - flash red
            if (zombieSprite != null)
            {
                StartCoroutine(FlashRed());
            }

            // Check if zombie should die
            if (currentHealth <= 0)
            {
                Debug.Log("Zombie killed!");
                Destroy(gameObject);
            }
        }
    }

    IEnumerator FlashRed()
    {
        // Change color to red
        zombieSprite.color = Color.red;

        // Wait a tiny moment
        yield return new WaitForSeconds(0.1f);

        // Change back to original color (white)
        zombieSprite.color = Color.white;
    }
}