using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class GameManager : MonoBehaviour
{
    public MapHandler map;
    public WeaponHandler wepH;
    public PlayerScript player;
    public PropManager pm;

    public SubSpawner groundSpawner, airSpawner;

    public int maxEnemies = 6;
    public int minEnemies = 1;
    int currentEnemies = 0;
    public float spawnRate = 1f;
    public float spawnDelay = 1f;
    float currentSpawnDelay = 0f;
    public float spawnDistanceGround = 5f;
    public float spawnDistanceGhost = 10f;
    public float failedSpawnTimerReduction = 0.5f;
    public int playerHits = 0;

    bool isNight = false;

    private void Start()
    {
        wepH.HideWeapons();
    }

    public void ToggleNight()
    {
        isNight = !isNight;
        map.ToggleNight();
        if (!map.wantLit)
        {
            wepH.ShowWeapons();
        }
        else
        {
            wepH.GetRidOfWeapons();
            airSpawner.Purge();
            groundSpawner.Purge();
            map.ClosedDoorMap.enabled = false;
            pm.RetrieveMess();
        }
    }
    private void Update()
    {
        if (isNight)
        {
            ManageSpawns();
        }
    }

    private void ManageSpawns()
    {
        if (currentSpawnDelay > 0)
        {
            currentSpawnDelay -= Time.deltaTime;
            return;
        }
        else
        {
            if (currentEnemies < maxEnemies)
                UpdateSpawnDelay();
            else
                return;
        }

        if(currentEnemies < minEnemies)
        {
            if (!SpawnAir())
                currentSpawnDelay *= 1f - failedSpawnTimerReduction;
        }
        else if(currentEnemies < maxEnemies)
        {
            float t = Random.Range(0,2);
            if(t == 0) // grounded
            {
                if(!SpawnGround())
                    currentSpawnDelay *= 1f - failedSpawnTimerReduction;
            }
            else // air
            {
                if(!SpawnAir())
                    currentSpawnDelay *= 1f - failedSpawnTimerReduction;
            }
        }
    }

    void UpdateSpawnDelay()
    {
        currentSpawnDelay = spawnRate / ((float)(maxEnemies - currentEnemies) / (float)(maxEnemies));
    }

    bool SpawnGround()
    {
        Transform[] temp;
        int tCount;
        temp = groundSpawner.GetTransforms(spawnDistanceGround, player.transform);
        tCount = temp.Length;
        if (tCount > 0)
        {
            DoSpawn(temp[Random.Range(0, temp.Length)].position, groundSpawner, false);
            return true;
        }
        else
        {
            return SpawnAir();
        }
    }
    bool SpawnAir()
    {
        Transform[] temp;
        int tCount;
        temp = airSpawner.GetTransforms(spawnDistanceGhost, player.transform);
        tCount = temp.Length;
        if (tCount > 0)
        {
            DoSpawn(temp[Random.Range(0, temp.Length)].position, airSpawner, true);
            return true;
        }
        else
        {
            return false;
        }
    }

    private void DoSpawn(Vector2 pos, SubSpawner s, bool isAir)
    {
        bool outcome;
        if (isAir)
        {
            outcome = s.SpawnAir(pos);
        }
        else
        {
            outcome = s.SpawnGround(pos);
        }
        if (outcome)
        {
            currentEnemies++;
        }
    }

    public void EnemyDestroyed(bool isPremade) {
        if (isPremade)
            return;
        currentEnemies--;
        UpdateSpawnDelay();
    }

    public void PlayerHit()
    {
        playerHits++;
    }
}
