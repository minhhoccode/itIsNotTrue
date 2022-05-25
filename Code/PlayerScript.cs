using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerScript : MonoBehaviour
{
    public float moveSpeed = 5f;


    Rigidbody2D rb;
    public SpriteRenderer sr;
    public Transform spriteHolder;
    public WallPlayerCheckerScript wc;
    bool hasBonked = false;

    public Transform aimDir;

    // bool useOffset = true;
    // public bool offsetStage = true;

    Vector2 movement;
    public Animator animator;


    //dash
    public float dashSpeedMult = 2f;
    public float dashDuration = 0.2f;
    public float dashCooldown = 0.5f;
    private float dashTimer = 0;
    private Vector2 dashDirection = Vector2.zero;

    public float invincibilityDuration = 0.3f;
    float invincibilityCurrent = 0f;

    public GameManager gm;

    void Start()
    {   
        if(rb == null)
        {
            rb = GetComponent<Rigidbody2D>();
        }

        if(wc == null)
        {
            wc = GetComponentInChildren<WallPlayerCheckerScript>();
        }

        sr.flipX = false;
    }
    
    private void FixedUpdate()
    {
        rb.MovePosition(spriteHolder.position);
        spriteHolder.localPosition = Vector3.zero;
    }
    
    void Update()
    {
        // movement logic
        movement.x = Input.GetAxisRaw("Horizontal");
        movement.y = Input.GetAxisRaw("Vertical");

        if (hasBonked)
        {
            if(wc.left && wc.right && wc.up && wc.down)
            {
                hasBonked = false;
            }
        }
        

        movement.Normalize();
        animator.SetFloat("Speed", movement.magnitude);
        // flip sprite if moving left
        if (movement.x < -0.01f && !sr.flipX)
        {   
            //animator.SetTrigger("flip");
            sr.flipX = true;

        }
        if (movement.x > 0.01f && sr.flipX)
        {   
            //animator.SetTrigger("flip");
            sr.flipX = false;
        }
        

        if (Input.GetKeyDown(KeyCode.Space) && movement.magnitude > 0f)
        {
            if (dashTimer <= 0f)
            {
                if (sr.flipX)
                    animator.SetTrigger("dashFlipped");
                else
                    animator.SetTrigger("dash");
                dashDirection = movement;
                dashTimer = dashCooldown;
            }
        }

        if (dashTimer > 0)
        {

            dashTimer = Mathf.Max(0, dashTimer - Time.deltaTime);
            if(dashTimer <=0)
            {
                animator.SetTrigger("blink");
            }
        }
        
        if(invincibilityCurrent > 0)
        {
            invincibilityCurrent -= Time.deltaTime;
        }

        Vector3 tempDirection;
        if (dashTimer > 0 && dashCooldown - dashTimer < dashDuration)
        {
            tempDirection = dashDirection;
            if (hasBonked)
            {
                if (!wc.left && tempDirection.x < 0)
                    tempDirection.x = 0;
                else if (!wc.right && tempDirection.x > 0)
                    tempDirection.x = 0;
                if (!wc.up && tempDirection.y > 0)
                    tempDirection.y = 0;
                else if (!wc.down && tempDirection.y < 0)
                    tempDirection.y = 0;
            }
            

            spriteHolder.position += tempDirection * moveSpeed * dashSpeedMult * Time.deltaTime;
        }
        else
        {
            tempDirection = movement;
            if (hasBonked)
            {
                if (!wc.left && tempDirection.x < 0)
                    tempDirection.x = 0;
                else if (!wc.right && tempDirection.x > 0)
                    tempDirection.x = 0;
                if (!wc.up && tempDirection.y > 0)
                    tempDirection.y = 0;
                else if (!wc.down && tempDirection.y < 0)
                    tempDirection.y = 0;
            }
            spriteHolder.position += tempDirection * moveSpeed * Time.deltaTime;
        }
        
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.gameObject.layer == LayerMask.NameToLayer("Default") || collision.collider.gameObject.layer == LayerMask.NameToLayer("Walls"))
        {
            animator.SetTrigger("wallBonk");
            hasBonked = true;
        }
        else if(invincibilityCurrent <= 0)
        {
            animator.SetTrigger("hit");
            invincibilityCurrent = invincibilityDuration;
            gm.PlayerHit();
            //transform.position -= (((Vector3)collision.GetContact(0).point - transform.position).normalized * hitKnockback);
        }
    }
}
