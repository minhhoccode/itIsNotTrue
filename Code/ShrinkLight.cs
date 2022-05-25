using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;

public class ShrinkLight : MonoBehaviour
{

    public Light2D l;
    float startSize;
    public float time = 1f;
    float t = 0;
    // Start is called before the first frame update
    void Start()
    {
        startSize = l.pointLightOuterRadius;
    }

    // Update is called once per frame
    void Update()
    {
        t = Mathf.Min(1, t + Time.deltaTime / time);
        l.pointLightOuterRadius = Mathf.Lerp(startSize, 0, t);
    }
}
