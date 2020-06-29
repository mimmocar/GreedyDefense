using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStatus : MonoBehaviour
{
    private int stamina ; //prevista lettura valore da file

    public int Stamina{
        get {

            return stamina;
        }

        set
        {
            stamina = value;
        }
    }

    // Start is called before the first frame update
    private void Awake()
    {
        stamina = 3;
    }

    void Start()
    {
        Debug.Log("Current Stamina Level of " + gameObject + " on Start is :" + stamina);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
