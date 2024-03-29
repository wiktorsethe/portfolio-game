using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;
public class ExpBar : MonoBehaviour
{
    [Header("Other Scripts")]
    public PlayerStats playerStats;
    private HpBar hpBar;
    private Menu menu;
    [Space(20f)]
    [Header("Other")]
    [SerializeField] private TMP_Text expText;
    [SerializeField] private Slider expBar;
    
    private void Start()
    {
        GetExperience();
        hpBar = GameObject.FindObjectOfType(typeof(HpBar)) as HpBar;
        menu = GameObject.FindObjectOfType(typeof(Menu)) as Menu;
    }
    private void Update()
    {
        if (expBar.value >= expBar.maxValue)
        {
            StartNewLevel();
        }
        if(PlayerPrefs.GetString("IsLevelUp") == "true")
        {
            if (menu)
            {
                Invoke("EnterCardMenu", 2f);
                PlayerPrefs.DeleteKey("IsLevelUp");

            }
        }
    }
    public void GetExperience()
    {
        if (playerStats.level == 1)
        {
            expText.text = "Lvl 1";
            expBar.maxValue = 70;
        }
        else if (playerStats.level == 2)
        {
            expText.text = "Lvl 2";
            expBar.maxValue = 100;
        }
        else if (playerStats.level == 3)
        {
            expText.text = "Lvl 3";
            expBar.maxValue = 130;
        }
        else if (playerStats.level == 4)
        {
            expText.text = "Lvl 4";
            expBar.maxValue = 170;
        }
        else if (playerStats.level == 5)
        {
            expText.text = "Lvl 5";
            expBar.maxValue = 200;
        }
        else if (playerStats.level == 6)
        {
            expText.text = "Lvl 6";
            expBar.maxValue = 230;
        }
        else if (playerStats.level == 7)
        {
            expText.text = "Lvl 7";
            expBar.maxValue = 250;
        }
        else if (playerStats.level == 8)
        {
            expText.text = "Lvl 8";
            expBar.maxValue = 280;
        }
        else if (playerStats.level == 9)
        {
            expText.text = "Lvl 9";
            expBar.maxValue = 310;
        }
        else if (playerStats.level == 10)
        {
            expText.text = "Lvl 10";
            expBar.maxValue = 330;
        }
        else if (playerStats.level == 11)
        {
            expText.text = "Lvl 11";
            expBar.maxValue = 360;
        }
        else if (playerStats.level == 12)
        {
            expText.text = "Lvl 12";
            expBar.maxValue = 400;
        }
        else if (playerStats.level == 13)
        {
            expText.text = "Lvl 13";
            expBar.maxValue = 420;
        }
        else if (playerStats.level == 14)
        {
            expText.text = "Lvl 14";
            expBar.maxValue = 450;
        }
        else if (playerStats.level == 15)
        {
            expText.text = "Lvl 15";
            expBar.maxValue = 480;
        }
        expBar.value = playerStats.experience;
    }
    public void SetExperience(int exp)
    {
        playerStats.experience += exp;
        DOTween.To(() => expBar.value, x => expBar.value = x, playerStats.experience, 1.5f);
    }
    public void StartNewLevel()
    {
        playerStats.level++;
        playerStats.experience = 0;
        SetExperience(0);
        Invoke("EnterCardMenu", 2f);
        hpBar.RegenerateHealth();
        GetExperience();
    }
    private void EnterCardMenu()
    {
        if (menu)
        {
            menu.CardMenu();

        }
        else
        {
            PlayerPrefs.SetString("IsLevelUp", "true");
        }
    }
}
