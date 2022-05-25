using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyScript : MonoBehaviour
{
    public GameManager gm;
    public GameObject SpawnFX, DestroyFX;
    public bool isPremade = false;

    void Start()
    {
        if (SpawnFX != null)
            Destroy(Instantiate(SpawnFX, transform.position, Quaternion.identity),1);
    }
    
    void Update()
    {
        
    }

    private void OnDestroy()
    {
        if (DestroyFX != null)
            Destroy(Instantiate(DestroyFX, transform.position, Quaternion.identity),1);
        gm.EnemyDestroyed(isPremade);
    }
}
