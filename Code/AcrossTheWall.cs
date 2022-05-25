using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AcrossTheWall : MonoBehaviour
{   
    public float speed;
    public Transform target;
    SpriteRenderer sr;

    private void Start()
    {
        sr = GetComponent<SpriteRenderer>();
    }

    private void Update(){
        transform.position = Vector2.MoveTowards(transform.position, target.position, speed * Time.deltaTime);

        if(target.position.x < transform.position.x)
        {
            sr.flipX = true;
        }
        else if(target.position.x > transform.position.x)
        {
            sr.flipX = false;
        }
    }
}