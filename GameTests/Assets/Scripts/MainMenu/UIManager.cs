using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class UIManager : MonoBehaviour
{
   

    public void Select(string selectedButton)
    {
        GameControl.Load(selectedButton);
        
    }

    public void Quit()
    {
        
        GameControl.Quit();
    }
}
