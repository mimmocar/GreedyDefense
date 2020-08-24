using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DamagePackage;

public class Features : MonoBehaviour
{

    protected int cost;
    //protected float range=0;
    //protected float explosionRadius=0;
    //protected float explosionPower = 0;
    //protected float fireRate = 0;
    //protected float turnSpeed = 0;
    
    //protected _Damage damage = null;

    // Start is called before the first frame update
    protected virtual void Start()
    {
        
    }

    public virtual void Awake()
    {

    }

    

    public int Cost
    {
        get
        {
            return cost;
        }
    }
}
