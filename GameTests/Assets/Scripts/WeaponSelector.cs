using System.IO;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Globalization;


public class WeaponSelector : MonoBehaviour
{
    private const char NEW_LINE = '\n';
    private const char EQUALS = '=';

    public UnityEngine.UI.Button[] levelButtons;
    private string[] unlockedWeapons;
    private int[] prices;
    private Utility.Utility util;
    private int weaponToUnlock = -1;
    private int skullsCurrency;
    //public const float MAX_RANGE = 60f;
    //public const float MAX_RATE = 10f;
    //public const float MAX_DAMAGE = 99f;

    public GameObject confirmationPanel;
    public Text currentCurrency;
    private int weaponSelected;
    private int indexRequestedWeapon = 0;
    private bool confirmPurchase = false;
    private bool buyWeapon = false;

    private void Start()
    {
        unlockedWeapons = new string[levelButtons.Length];
        prices = new int[levelButtons.Length];
        util = Utility.Utility.Instance();


        //Istruzione per testing
        skullsCurrency = PlayerPrefs.GetInt("skullsCurrency", 0);

        currentCurrency.text = "" + skullsCurrency;
        weaponSelected = PlayerPrefs.GetInt("weaponSelected", 1);
        for (int i = 0; i < levelButtons.Length; i++)
        {
            string filePath = "File/weapon" + (i + 1) + "Features";




            //int price = int.Parse(sr.ReadLine(), CultureInfo.InvariantCulture);
            //prices[i] = price;

            Image rangeIm = levelButtons[i].transform.Find("RangePanel").Find("RangeFront").GetComponent<Image>();
            Image fireIm = levelButtons[i].transform.Find("FireRatePanel").Find("FireFront").GetComponent<Image>();
            Image damageIm = levelButtons[i].transform.Find("DamagePanel").Find("DamageFront").GetComponent<Image>();

            TextAsset data = Resources.Load<TextAsset>(filePath);
            string[] lines = data.text.Split(NEW_LINE);

            for (int j = 0; j < lines.Length; j++)
            {
                string line = lines[j];
                string[] token = line.Split(EQUALS);

                switch (token[0])
                {
                    case "range":
                        float range = float.Parse(token[1], CultureInfo.InvariantCulture);
                        rangeIm.fillAmount = range / util.Max_Range;
                        break;
                    case "fireRate":
                        float fireRate = float.Parse(token[1], CultureInfo.InvariantCulture);
                        fireIm.fillAmount = fireRate / util.Max_Rate;
                        break;
                    case "damage":
                        float damage = float.Parse(token[1], CultureInfo.InvariantCulture);
                        damageIm.fillAmount = damage / util.Max_Damage;
                        break;
                    case "price":
                        int price = int.Parse(token[1], CultureInfo.InvariantCulture);
                        prices[i] = price;
                        break;
                    default:
                        break;


                }

            }


            //rangeIm.fillAmount = range / MAX_RANGE;
            //fireIm.fillAmount = fireRate / MAX_RATE;
            //damageIm.fillAmount = damage / MAX_DAMAGE;



            if ((i + 1) == weaponSelected)
            {
                unlockedWeapons[i] = "true";
                levelButtons[i].transform.Find("SelectPanel").gameObject.SetActive(true);
                levelButtons[i].transform.Find("LockPanel").gameObject.SetActive(false);

            }
            else
            {
                string isAvailable = PlayerPrefs.GetString("isWeapon" + (i + 1) + "Unlocked", "false");

                if (i == 0)
                    isAvailable = "true";


                if (isAvailable == "true")
                {
                    unlockedWeapons[i] = "true";
                    levelButtons[i].transform.Find("LockPanel").gameObject.SetActive(false);

                }
                else
                {
                    unlockedWeapons[i] = "false";
                    GameObject pricePanel = levelButtons[i].transform.Find("PricePanel").gameObject;
                    pricePanel.transform.Find("Value").GetComponent<Text>().text = "" + prices[i];
                    pricePanel.SetActive(true);
                    if (prices[i] > skullsCurrency)
                        levelButtons[i].interactable = false;

                }

            }
        }
    }


    private void Update()
    {
        if (confirmPurchase)
        {
            confirmationPanel.SetActive(true);
            Debug.Log("Panel Conferma attivo");
            if (buyWeapon)
            {
                Debug.Log("Buy true");
                Buy();
                confirmPurchase = false;
                buyWeapon = false;
            }
        }
        else
        {
            confirmationPanel.SetActive(false);
        }

        if (indexRequestedWeapon <= 0) return;


        //Debug.Log(indexRequestedWeapon);
        if (unlockedWeapons[indexRequestedWeapon - 1] == "true")
            SelectWeapon(indexRequestedWeapon - 1);
        else
        {
            //Debug.Log(indexRequestedWeapon);
            UnlockWeapon(indexRequestedWeapon - 1);
        }

        indexRequestedWeapon = 0;




    }

    public void Select(int i)
    {

        indexRequestedWeapon = i;

    }


    private void SelectWeapon(int index)
    {
        levelButtons[weaponSelected - 1].transform.Find("SelectPanel").gameObject.SetActive(false);
        levelButtons[index].transform.Find("SelectPanel").gameObject.SetActive(true);

        weaponSelected = index + 1;
        PlayerPrefs.SetInt("weaponSelected", weaponSelected);
    }

    private void UnlockWeapon(int index)
    {


        Image img = levelButtons[index].transform.Find("ImagePanel").gameObject.transform.Find("Image").gameObject.GetComponent<Image>();
        confirmationPanel.transform.Find("Weapon").gameObject.GetComponent<Image>().sprite = img.sprite;

        weaponToUnlock = index;
        confirmPurchase = true;
    }

    private void Buy()
    {
        if (skullsCurrency >= prices[weaponToUnlock])
        {
            skullsCurrency -= prices[weaponToUnlock];
            PlayerPrefs.SetInt("skullsCurrency", skullsCurrency);

            PlayerPrefs.SetString("isWeapon" + (weaponToUnlock + 1) + "Unlocked", "true");
            unlockedWeapons[weaponToUnlock] = "true";


            levelButtons[weaponToUnlock].transform.Find("LockPanel").gameObject.SetActive(false);
            levelButtons[weaponToUnlock].transform.Find("PricePanel").gameObject.SetActive(false);

            currentCurrency.text = "" + skullsCurrency;
            //confirmationPanel.gameObject.SetActive(false);
            confirmPurchase = false;


        }
        else
        {
            confirmationPanel.transform.Find("Log").gameObject.SetActive(true);
        }
    }

    public void Confirm()
    {
        buyWeapon = true;

    }

    public void Cancel()
    {

        buyWeapon = false;
        confirmPurchase = false;
    }

    public void BackButton()
    {

        Debug.Log("Back pressed");
        GameControl.UnloadWeapon();
    }
}
