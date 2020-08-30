using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneFader : MonoBehaviour
{
    [SerializeField] protected Image fading;
    [SerializeField] protected AnimationCurve fadingCurve;
    

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(FadeIn());
        
    }

    public void FadeTo(string scene)
    {
        StartCoroutine(FadeOut(scene));
        Debug.Log("FADING");
    }

    IEnumerator FadeIn()
    {
        float t = 1f;
        while (t > 0f)
        {
            t -= Time.deltaTime;
            float a = fadingCurve.Evaluate(t);
            fading.color = new Color(0f, 0f, 0f, a);
            yield return 0;

        }
    }

    IEnumerator FadeOut(string scene)
    {
        float t = 0f;
        while (t < 1f)
        {
            t += Time.deltaTime;
            float a = fadingCurve.Evaluate(t);
            fading.color = new Color(0f, 0f, 0f, a);
            yield return 0;

        }

        SceneManager.LoadScene(scene);
    }
}
