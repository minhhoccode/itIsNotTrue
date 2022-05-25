using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NeonScript : MonoBehaviour
{
    public SpriteRenderer baseSR, sr;
    public Gradient g;
    public float gradientSpeed = 1f;
    float anim = 0f;

    // Update is called once per frame
    void Update()
    {
        if(baseSR.color == new Color(0, 0, 0, 0))
        {
            sr.color = new Color(0, 0, 0, 0);
            return;
        }
        anim += Time.deltaTime * gradientSpeed;
        if (anim > 1)
            anim -= 1;
        sr.color = g.Evaluate(anim);
        
        if (baseSR.sortingOrder > 10)
            sr.sortingOrder = baseSR.sortingOrder + 2;
        else
            sr.sortingOrder = baseSR.sortingOrder + 1;
            
        sr.flipY = baseSR.flipY;
    }
}
