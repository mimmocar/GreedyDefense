using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManagerJoystickScript : MonoBehaviour
{
    
    [SerializeField] VariableJoystick joystick;
    public VariableJoystick VariableJoystick
    {
        get
        {
            return joystick;
        }
    }

    
}

