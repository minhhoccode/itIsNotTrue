using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour
{
    public float health = 100;
    public Animator animator;
    public GameObject hitFX, deathFX;

    public void TakeDamage (float damage)
    {
        health -= damage;

        if(animator != null)
        {
            animator.SetTrigger("hit");
        }
        if(hitFX != null)
        {
            Instantiate(hitFX, transform.position, Quaternion.identity);
        }

        if (health <=0 )    Die();
    }
    private void Die(){
        if (deathFX != null)
        {
            Instantiate(deathFX, transform.position, Quaternion.identity);
        }
        Destroy(gameObject);
    }
}
