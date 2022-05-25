using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PropScript : MonoBehaviour
{
    public float messValue = 100f;
    public float distanceMessMin = 0.1f;
    public float distanceMessMax = 1f;
    public float rotMessMin = 3f;
    public float rotMessMax = 20f;

    public PropManager pm;

    Vector2 position;
    Health health;
    float hp;

    // Start is called before the first frame update
    void Start()
    {
        position = transform.position;
        health = GetComponent<Health>();
        hp = health.health;
    }
    
    public float EvaluateMess(bool isDestroyed)
    {
        if (isDestroyed)
            return messValue * 2;
        else
        {
            float estimatePos = 0, estimateRot = 0;
            float temp;

            temp = ((Vector2)transform.position - position).magnitude;
            if(temp > distanceMessMin)
            {
                estimatePos = Mathf.Min(1f, temp / distanceMessMax);
            }
            temp = Mathf.DeltaAngle(transform.rotation.eulerAngles.z, 0);
            if (temp < 0)
                temp *= -1f;
            if(temp > rotMessMin)
            {
                estimateRot = Mathf.Min(1f, temp / rotMessMax);
            }
            temp = (estimateRot + estimatePos) / 2f;

            return temp * messValue * (1f + health.health/hp);
        }
    }

    private void OnDestroy()
    {
        pm.messScore += EvaluateMess(true);
        pm.props.Remove(this);
        pm.destroyed++;
    }
}
