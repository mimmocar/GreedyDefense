using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectManager : MonoBehaviour
{
    //private delegate void OnSpawnObject(Vector3 pos, GameObject pref);
    // Start is called before the first frame update
    [SerializeField] GameObject[] prefab;
    void Awake()
    {
        Messenger<Vector3,int>.AddListener(GameEvent.SPAWN_REQUESTED, OnSpawnObject);

    }

    void OnDestroy()
    {
        Messenger<Vector3, int>.RemoveListener(GameEvent.SPAWN_REQUESTED, OnSpawnObject);
    }



    // Update is called once per frame
    void Update()
    {
        
    }

    void OnSpawnObject(Vector3 position, int i)
    {
        Instantiate(prefab[i], position, new Quaternion(0,0,0,0));
    }
}
