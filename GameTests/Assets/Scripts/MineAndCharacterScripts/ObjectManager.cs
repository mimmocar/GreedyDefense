﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectManager : MonoBehaviour
{
    //private delegate void OnSpawnObject(Vector3 pos, GameObject pref);
    //Start is called before the first frame update
    private int hit = 0, kills = 0, berserk = 100;
    [SerializeField] GameObject[] prefab;
    void Awake()
    {
        Messenger<Vector3, int>.AddListener(GameEvent.SPAWN_REQUESTED, OnSpawnObject);
        Messenger<GameObject, int>.AddListener(GameEvent.ENEMY_HIT, OnEnemyHit);

    }

    void OnDestroy()
    {
        Messenger<Vector3, int>.RemoveListener(GameEvent.SPAWN_REQUESTED, OnSpawnObject);
        Messenger<GameObject, int>.RemoveListener(GameEvent.ENEMY_HIT, OnEnemyHit);
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

    void OnEnemyHit(GameObject target, int damage)
    {
        Enemy enemy = target.GetComponent<Enemy>();
        hit += 1;
        Debug.Log("Invocazione n " + hit);
        //enemy.GetComponent<EnemyStatus>().Stamina -= 1;
        //enemy.GetComponent<Enemy>().Health -= damage;
        enemy.TakeDamage(damage);
        Debug.Log(enemy + "  Stamina is: " + enemy.GetComponent<Enemy>().Health);
        if (/*enemy.GetComponent<Enemy>().Health == 0*/enemy.Dead)
        {
            //Destroy(enemy.gameObject);
            enemy.Die();
            
            if(kills >= berserk)
            {
                kills = 0;
            }

            kills++;
            Messenger<int, int>.Broadcast(GameEvent.ENEMY_DIED, kills, berserk);
        }

    }
}
