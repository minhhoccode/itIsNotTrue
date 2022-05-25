using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PropManager : MonoBehaviour
{
    public List<PropScript> props;
    public float messScore = 0;
    public int destroyed = 0;

    private void Start()
    {
        props = new List<PropScript>(GetComponentsInChildren<PropScript>());
        foreach (PropScript p in props)
        {
            p.pm = this;
        }
    }

    public float RetrieveMess()
    {
        foreach(PropScript p in props)
        {
            messScore += p.EvaluateMess(false);
        }

        return messScore;
    }
}
