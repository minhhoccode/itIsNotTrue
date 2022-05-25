using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallPlayerCheckerScript : MonoBehaviour
{
    public bool up = true;
    public bool down = true;
    public bool left = true;
    public bool right = true;

    public CircleCollider2D upC;
    public CircleCollider2D downC;
    public CircleCollider2D leftC;
    public CircleCollider2D rightC;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Check(collision);
    }
    private void OnTriggerStay2D(Collider2D collision)
    {
        Check(collision);
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        Check(collision);
    }

    private void Check(Collider2D collision)
    {
        if (upC.IsTouching(collision))
        {
            up = false;
        }
        else
        {
            up = true;
        }
        if (downC.IsTouching(collision))
        {
            down = false;
        }
        else
        {
            down = true;
        }
        if (leftC.IsTouching(collision))
        {
            left = false;
        }
        else
        {
            left = true;
        }
        if (rightC.IsTouching(collision))
        {
            right = false;
        }
        else
        {
            right = true;
        }
    }
}
