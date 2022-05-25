using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using UnityEngine.Tilemaps;

public class MapHandler : MonoBehaviour
{
    public Tilemap[] LitHouseMaps;
    public Tilemap[] DarkHouseMaps;
    public Tilemap ClosedDoorMap;
    public Light2D globalLight;

    public Color darkColor;
    Color currentColor = Color.white;

    public float animSpeed = 1f;

    public bool isLit = true;
    public bool wantLit = true;
    float anim = 0;
    float litAmount = 1;

    private void Start()
    {
        UpdateMap();
    }

    public void ToggleNight()
    {
        wantLit = !wantLit;
        anim = 0;
    }

    private void Update()
    {

        if(isLit && !wantLit)
        {
            litAmount = Slerp(1, 0, anim);
            currentColor = Color.Lerp(Color.white, darkColor, anim);
            
            anim = Mathf.Min(anim + Time.deltaTime * animSpeed, 1f);
            if(anim >= 1)
            {
                isLit = false;
            }
            UpdateMap();
        }
        else if(!isLit && wantLit)
        {
            litAmount = Slerp(0, 1, anim);
            currentColor = Color.Lerp(darkColor, Color.white, anim);
            anim = Mathf.Min(anim + Time.deltaTime * animSpeed, 1f);
            if (anim >= 1)
            {
                isLit = true;
            }
            UpdateMap();
        }
    }

    void UpdateMap()
    {
        foreach (Tilemap t in LitHouseMaps)
        {
            t.color = new Color(1, 1, 1, litAmount);
        }
        foreach (Tilemap t in DarkHouseMaps)
        {
            t.color = new Color(1, 1, 1, 1f - litAmount);
        }
        if (wantLit)
            ClosedDoorMap.gameObject.SetActive(false);
        else
            ClosedDoorMap.gameObject.SetActive(true);
        globalLight.color = currentColor;
    }

    float Slerp(float a, float b, float t)
    {
        //t = t * t * t * (t * (6f * t - 15f) + 10f); //smoother step
        //t = t * t * (3f - 2f * t); // smooth step
        t = Mathf.Sin(t * Mathf.PI * 0.5f); //ease out
        return Mathf.Lerp(a, b, t);
    }
}
