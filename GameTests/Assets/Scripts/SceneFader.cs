using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneFader : MonoBehaviour
{
    [SerializeField] protected Image fading;
    [SerializeField] protected AnimationCurve fadingCurve;
    private Utility.Utility util;
    private float fSpeed;

    
    void Start()
    {
        StartCoroutine(FadeIn());
        util = Utility.Utility.Instance();
        fSpeed = util.FadingSpeed;
        Debug.Log("FADING into Scene"+ SceneManager.GetActiveScene().name);
        
    }

    

    IEnumerator FadeIn()
    {
        float t = 1f;
        while (t > 0f)
        {
            t -= fSpeed*Time.fixedDeltaTime;
            float a = fadingCurve.Evaluate(t);
            fading.color = new Color(0f, 0f, 0f, a);
            yield return 0;

        }
    }

    
}
