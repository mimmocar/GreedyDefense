using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public class LevelSelector : MonoBehaviour
{
    
    public UnityEngine.UI.Button[] levelButtons;

    private void Start()
    {
        int levelReached = PlayerPrefs.GetInt("levelReached", 1);
        for (int i = 0; i < levelButtons.Length; i++)
        {
            if (i + 1 > levelReached)
                levelButtons[i].interactable = false;
            else
            {
                int score = PlayerPrefs.GetInt("starForLevel" + (i + 1), 0);

                //Image[] borders = levelButtons[i].GetComponentsInChildren<Image>();
                //Debug.Log("Number of borders found : "+ borders.Length);
                for (int j = 0; j < 3; j++)
                {
                    if (j + 1 <= score)
                    {
                        Transform border = levelButtons[i].transform.GetChild(j + 1);
                        if (border != null)
                            Debug.Log("Border found : " + border);
                        Transform starImage = border.GetChild(0);
                        Debug.Log("Border found : " + starImage);
                        starImage.gameObject.SetActive(true);
                    }

                    else break;
                }
            }
        }
    }

    public void Select(string levelName)
    {
        GameControl.Load(levelName);
    }

    public void SelectWeapon()
    {
        PlayerPrefs.SetString("previousScene", SceneManager.GetActiveScene().name);
        GameControl.Load("WeaponSelection");
        //SceneManager.LoadScene("WeaponSelection", LoadSceneMode.Additive);

    }
}
