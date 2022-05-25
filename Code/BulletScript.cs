using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletScript : MonoBehaviour
{
    public float damage = 5f;
    public float speed = 5f;
    public float pushForce = 0.5f;

    public GameObject HitEffect;
    Rigidbody2D rb;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.velocity = transform.right * speed;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //if (collision.attachedRigidbody.isKinematic == false)
        {
            Destroy(this.gameObject);

            collision.attachedRigidbody.AddForce(rb.velocity.normalized * pushForce, ForceMode2D.Impulse);
            Instantiate(HitEffect, transform.position, transform.rotation);
            Health health = collision.gameObject.GetComponent<Health>();
            if(health != null)  health.TakeDamage(damage);
        }
    }
}
