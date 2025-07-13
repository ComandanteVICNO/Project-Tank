// © 2025 Francisco Gonçalves. All Rights Reserved.
// For portfolio viewing only – usage or redistribution is prohibited.

using System;
using System.Collections.Generic;
using UnityEngine;


public class SpawnManager : MonoBehaviour
{
    [SerializeField] float timeBetweenSpawns;
    [SerializeField] List<Transform> spawnPoints;
    [SerializeField] GameObject spawnPrefab;
    
    float elapsedTime;


    private void Update()
    {
        CountTimeUntilNextSpawn();
    }

    void CountTimeUntilNextSpawn()
    {
        elapsedTime += Time.deltaTime;
        if (elapsedTime >= timeBetweenSpawns)
        {
            SpawnEnemies();
            elapsedTime = 0;
        }
    }
    
    
    void SpawnEnemies()
    {
        int spawnPointIndex = UnityEngine.Random.Range(0, spawnPoints.Count);
        Transform spawnPoint = spawnPoints[spawnPointIndex];
        
        GameObject enemy = Instantiate(spawnPrefab, spawnPoint.position, spawnPoint.rotation);
        
        
    }
    
}
