using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DamagePackage;

public class Features : MonoBehaviour
{

    protected int cost;
    
   

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
