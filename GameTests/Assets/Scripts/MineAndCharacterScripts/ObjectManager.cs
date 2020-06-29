﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectManager : MonoBehaviour
{
    //private delegate void OnSpawnObject(Vector3 pos, GameObject pref);
    // Start is called before the first frame update
    private int count = 0;
    [SerializeField] GameObject[] prefab;
    void Awake()
    {
        Messenger<Vector3, int>.AddListener(GameEvent.SPAWN_REQUESTED, OnSpawnObject);
        Messenger<GameObject>.AddListener(GameEvent.ENEMY_HIT, OnEnemyHit);

    }

    void OnDestroy()
    {
        Messenger<Vector3, int>.RemoveListener(GameEvent.SPAWN_REQUESTED, OnSpawnObject);
        Messenger<GameObject>.RemoveListener(GameEvent.ENEMY_HIT, OnEnemyHit);
    }



    // Update is called once per frame
    void Update()
    {

    }

    void OnSpawnObject(Vector3 position, int i)
    {
        position += new Vector3(0, prefab[i].transform.localScale.y / 2, 0);
        Instantiate(prefab[i], position, new Quaternion(0, 0, 0, 0));
    }

    void OnEnemyHit(GameObject enemy)
    {
        count += 1;
        Debug.Log("Invocazione n " + count);
        enemy.GetComponent<EnemyStatus>().Stamina -= 1;
        Debug.Log(enemy + "  Stamina is: " + enemy.GetComponent<EnemyStatus>().Stamina);
        if (enemy.GetComponent<EnemyStatus>().Stamina == 0)
        {
            Destroy(enemy);
        }

    }
}
