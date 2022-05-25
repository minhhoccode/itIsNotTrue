using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SubSpawner : MonoBehaviour
{
    public List<Transform> spawners;
    public GameObject[] enemies;
    public float[] odds;

    public GameManager gm;
    public Transform enemyHolder;
    public Transform player;

    bool isFirstSpawn = true;

    void Start()
    {
        spawners = new List<Transform>(GetComponentsInChildren<Transform>());
        spawners.Remove(this.transform);
    }

    public Transform[] GetTransforms(float dist, Transform target)
    {
        List<Transform> temp = new List<Transform>();

        foreach(Transform t in spawners)
        {
            if(Vector2.Distance(target.position,t.position) < dist)
            {
                temp.Add(t);
            }
        }

        return temp.ToArray();
    }

    public bool SpawnAir(Vector2 pos)
    {
        AcrossTheWall a = Spawn(pos).GetComponent<AcrossTheWall>();
        if (a != null)
        {
            a.target = player;
            a.GetComponent<EnemyScript>().gm = gm;
            return true;
        }
        else
            return false;
    }

    public bool SpawnGround(Vector2 pos)
    {
        FollowPlr a = Spawn(pos).GetComponent<FollowPlr>();
        if (a != null)
        {
            a.target = player;
            a.GetComponent<EnemyScript>().gm = gm;
            return true;
        }
        else
            return false;
    }

    GameObject Spawn(Vector2 pos)
    {
        float t;

        if (isFirstSpawn)
        {
            t = 0;
            isFirstSpawn = false;
        }
        else
        {
            t = Random.Range(0f, 1f);
        }

        for(int i = 0; i < odds.Length; i++)
        {
            if(t < odds[i])
            {
                return Instantiate(enemies[i], pos, Quaternion.identity, enemyHolder);
            }
            else
            {
                t = Mathf.Max(0, t - odds[i]);
            }
        }
        return null;
    }
    public void Purge()
    {
        foreach(EnemyScript e in enemyHolder.GetComponentsInChildren<EnemyScript>())
        {
            Destroy(e.gameObject);
        }
    }
}
