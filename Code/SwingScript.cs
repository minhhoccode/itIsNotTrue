using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwingScript : MonoBehaviour
{
    public float activeHitbox = 0.1f;
    public float pushForce = 2f;
    public float damage = 5f;
    bool didHitbox;
    public float moveSpeed = 0.5f;
    public float scaleSpeed = 0.5f;

    public Collider2D collider;

    // Start is called before the first frame update
    void Start()
    {
        if (collider == null)
            collider = GetComponent<Collider2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!didHitbox)
        {
            activeHitbox -= Time.deltaTime;
            if (activeHitbox < 0)
            {
                if (collider != null)
                    collider.enabled = false;

                didHitbox = true;
            }

            transform.position += transform.right * Time.deltaTime * moveSpeed;
            transform.localScale += transform.localScale * Time.deltaTime * scaleSpeed;

        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Rigidbody2D rb = collision.attachedRigidbody;
        if(rb.isKinematic == false)
        {
            Vector2 force = rb.transform.position - this.transform.position;
            force.Normalize();
            rb.AddForce(force * pushForce, ForceMode2D.Impulse);
            Health health = collision.gameObject.GetComponent<Health>();
            if (health != null) health.TakeDamage(damage);
        }
    }
}
