using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZombieAI : MonoBehaviour
{
    public float moveSpeed = 1.5f;
    public int health = 2; // Number of hits to kill zombie
    public float walkDirection = -1f; // -1 = left, 1 = right

    // Reference to animator
    private Animator animator;
    private Rigidbody2D rb;
    private SpriteRenderer zombieSprite;
    private int currentHealth;
    private bool isDead = false;
    private bool isAttacking = false;

    // Reference to gun
    private Transform gun;
    private float attackRange = 0.5f; // Distance to start attack animation

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        zombieSprite = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
        currentHealth = health;

        // Find gun
        GameObject gunObj = GameObject.Find("Gun");
        if (gunObj != null)
        {
            gun = gunObj.transform;
        }

        // Start with walk animation
        if (animator != null)
        {
            animator.SetBool("IsWalking", true);
            animator.SetBool("IsAttacking", false);
            animator.SetBool("IsDead", false);
        }

        // Flip zombie sprite based on direction
        UpdateFacingDirection();

        Debug.Log("Zombie started walking " + (walkDirection < 0 ? "left" : "right"));
    }

    void FixedUpdate()
    {
        if (!isDead && !isAttacking)
        {
            // Walk in straight line
            Vector2 movement = new Vector2(walkDirection, 0) * moveSpeed * Time.fixedDeltaTime;
            rb.MovePosition(rb.position + movement);

            // Check if close to gun for attack
            if (gun != null)
            {
                float distanceToGun = Vector2.Distance(transform.position, gun.position);
                if (distanceToGun <= attackRange)
                {
                    StartAttack();
                }
            }
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        // If zombie touches bullet
        if (other.CompareTag("Bullet") && !isDead)
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
                Die();
            }
        }
    }

    void StartAttack()
    {
        if (isDead || isAttacking) return;

        isAttacking = true;

        // Stop movement
        rb.linearVelocity = Vector2.zero;

        // Play attack animation
        if (animator != null)
        {
            animator.SetBool("IsWalking", false);
            animator.SetBool("IsAttacking", true);

            // Optional: Start attack coroutine to trigger gun damage
            StartCoroutine(AttackGun());
        }
    }

    IEnumerator AttackGun()
    {
        // Wait for attack animation to complete (adjust timing based on your animation)
        yield return new WaitForSeconds(0.5f);

        // Damage the gun
        GunShooting gunScript = gun?.GetComponent<GunShooting>();
        if (gunScript != null)
        {
            // This will trigger the gun's destruction
            // You may need to modify GunShooting to handle damage from zombie attacks
            Debug.Log("Zombie attacked the gun!");

            // Optional: Reduce gun health directly
            // You can add a TakeDamage method to GunShooting
        }

        // Reset attack state
        isAttacking = false;

        // Go back to walking if not dead
        if (!isDead && animator != null)
        {
            animator.SetBool("IsWalking", true);
            animator.SetBool("IsAttacking", false);
        }
    }

    void Die()
    {
        if (isDead) return;

        isDead = true;

        // Stop movement
        rb.linearVelocity = Vector2.zero;

        // Disable collider to prevent further interactions
        Collider2D col = GetComponent<Collider2D>();
        if (col != null) col.enabled = false;

        // Play death animation
        if (animator != null)
        {
            animator.SetBool("IsWalking", false);
            animator.SetBool("IsAttacking", false);
            animator.SetBool("IsDead", true);

            // Wait for death animation to complete before destroying
            float deathAnimationLength = GetAnimationLength("Death");
            Destroy(gameObject, deathAnimationLength);
        }
        else
        {
            // If no animator, destroy immediately
            Destroy(gameObject);
        }

        Debug.Log("Zombie killed!");
    }

    void UpdateFacingDirection()
    {
        // Flip sprite based on walk direction
        if (zombieSprite != null)
        {
            if (walkDirection > 0)
            {
                transform.localScale = new Vector3(1, 1, 1); // Face right
            }
            else
            {
                transform.localScale = new Vector3(-1, 1, 1); // Face left
            }
        }
    }

    float GetAnimationLength(string animationName)
    {
        // Get the length of the animation clip
        if (animator != null)
        {
            RuntimeAnimatorController ac = animator.runtimeAnimatorController;
            for (int i = 0; i < ac.animationClips.Length; i++)
            {
                if (ac.animationClips[i].name == animationName)
                {
                    return ac.animationClips[i].length;
                }
            }
        }
        return 0.5f; // Default fallback
    }

    IEnumerator FlashRed()
    {
        if (zombieSprite != null)
        {
            zombieSprite.color = Color.red;
            yield return new WaitForSeconds(0.1f);
            zombieSprite.color = Color.white;
        }
    }
}