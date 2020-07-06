using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class Spawn : MonoBehaviour
{
    public GameObject spawnee;
    public bool stopSpawning = false;
    public float spawnTime;
    public float spawnDelay;
    public int maxNum = 5;
    private int count = 0;
    
    // Start is called before the first frame update
    void Start()
    {
        InvokeRepeating("SpawnObject", spawnTime, spawnDelay);
        
    }

   
    public void SpawnObject()
    {
        Instantiate(spawnee, transform.position, transform.rotation);
        count += 1;
        if (count == maxNum)
        {
            CancelInvoke("SpawnObject");
        }
    }

}
