using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroySelf : MonoBehaviour
{
    public float TTL = 1f;
    void Start()
    {
        Destroy(this.gameObject, TTL);
    }
}
