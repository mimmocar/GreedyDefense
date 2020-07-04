﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DamagePackage;

public class ObjectManager : MonoBehaviour
{
    //private delegate void OnSpawnObject(Vector3 pos, GameObject pref);
    //Start is called before the first frame update
    private int hit = 0, kills = 0, berserk = 100;

    public int Kills
    {
        get
        {
            return kills;
        }
    }

    public int Berserk
    {
        get
        {
            return berserk;
        }
    }
    [SerializeField] GameObject[] prefab;
    void Awake()
    {
        Messenger<Vector3, int>.AddListener(GameEvent.SPAWN_REQUESTED, OnSpawnObject);
        //Messenger<GameObject, int>.AddListener(GameEvent.ENEMY_HIT, OnEnemyHit);
        Messenger<GameObject, _Damage>.AddListener(GameEvent.HANDLE_DAMAGE, OnHandleDamage);

    }

    void OnDestroy()
    {
        Messenger<Vector3, int>.RemoveListener(GameEvent.SPAWN_REQUESTED, OnSpawnObject);
        // Messenger<GameObject, int>.RemoveListener(GameEvent.ENEMY_HIT, OnEnemyHit);
        Messenger<GameObject, _Damage>.RemoveListener(GameEvent.HANDLE_DAMAGE, OnHandleDamage);
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

    //void OnEnemyHit(GameObject target, int damage)
    //{
    //    Enemy enemy = target.GetComponent<Enemy>();
    //    hit += 1;
    //    Debug.Log("Invocazione n " + hit);
    //    enemy.TakeDamage(damage);
    //    Debug.Log(enemy + "  Stamina is: " + enemy.GetComponent<Enemy>().Health);
    //    if (enemy.Dead)
    //    {
    //        enemy.Die();

    //        if(kills >= berserk)
    //        {
    //            kills = 0;
    //        }
    //        kills++;
    //    }

    //}
    void OnHandleDamage(GameObject target, _Damage damage)
    {
        Enemy enemy = target.GetComponent<Enemy>();
        hit += 1;
        Debug.Log("Invocazione n " + hit);

        if (enemy != null)
        {
            float[] damagesMultipilers = enemy.DamagesMultipliers;
            float amount = damage.Amount;

            enemy.Health = enemy.Health - damagesMultipilers[(int)damage.Type] * amount;
            //enemy.TakeDamage(damage);
            Debug.Log(enemy + "  Stamina is: " + enemy.GetComponent<Enemy>().Health);
            if (enemy.Dead)
            {
                enemy.Die();

                if (kills >= berserk)
                {
                    kills = 0;
                }
                kills++;
            }
        }

    }
}
