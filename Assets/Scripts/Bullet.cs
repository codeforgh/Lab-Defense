using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    void Start()
    {
        Debug.Log("Bullet created!");
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        // Just destroy bullet when it hits anything
        if (other.CompareTag("Zombie"))
        {
            Destroy(gameObject); // Destroy bullet only
            // Zombie health is reduced in ZombieAI script
        }
    }
}