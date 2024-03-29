using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;
using System;
using Random = UnityEngine.Random;

public class DailyChallenges : MonoBehaviour
{
    public ChallengesDatabase challengesDB;
    public PlayerStats playerStats;
    [SerializeField] private GameObject[] challengesObjects;
    [SerializeField] private GameObject[] rewardsObjects;
    [SerializeField] private GameObject[] claimButtonObjects;
    [SerializeField] private AudioSource buttonSound;
    //public SaveManager save;
    private List<int> except = new List<int>();
    private void Start()
    {
        DateTime currentDate = DateTime.Now;
        DateTime loginTime = DateTime.Parse(playerStats.loginTime);
        double elapsedHours = (currentDate - loginTime).TotalHours;
        if (elapsedHours >= 20f)
        {
            Array.Clear(playerStats.challenges, 0, playerStats.challenges.Length);
            playerStats.todayBestTime = 0;
            playerStats.todayMostGoldEarned = 0;
            playerStats.todayMostKills = 0;
            playerStats.loginTime = DateTime.Now.ToString();

            for(int i=0; i<4; i++)
            {
                int randIndex = RandomRangeExcept(0, challengesDB.challenges.Length);

                challengesObjects[i].transform.Find("MissionTxt").GetComponent<TMP_Text>().text = challengesDB.challenges[randIndex].task;
                rewardsObjects[i].transform.Find("RewardTxt").GetComponent<TMP_Text>().text = challengesDB.challenges[randIndex].amount.ToString();

                if (challengesDB.challenges[randIndex].bestTimeTask <= playerStats.todayBestTime
                    && challengesDB.challenges[randIndex].mostGoldEarnedTask <= playerStats.todayMostGoldEarned
                    && challengesDB.challenges[randIndex].mostKillsTask <= playerStats.todayMostKills)
                {
                    claimButtonObjects[i].GetComponent<Button>().interactable = true;
                }
                else
                {
                    claimButtonObjects[i].GetComponent<Button>().interactable = false;
                }

                challengesDB.challenges[randIndex].isDone = false;
                except.Add(randIndex);
                playerStats.challenges[i] = challengesDB.challenges[randIndex];
            }
        }
        else
        {
            for (int i = 0; i < 4; i++)
            {
                if (!playerStats.challenges[i].isDone)
                {
                    challengesObjects[i].transform.Find("MissionTxt").GetComponent<TMP_Text>().text = playerStats.challenges[i].task;
                    rewardsObjects[i].transform.Find("RewardTxt").GetComponent<TMP_Text>().text = playerStats.challenges[i].amount.ToString();
                    if (playerStats.challenges[i].bestTimeTask <= playerStats.todayBestTime
                    && playerStats.challenges[i].mostGoldEarnedTask <= playerStats.todayMostGoldEarned
                    && playerStats.challenges[i].mostKillsTask <= playerStats.todayMostKills)
                    {
                        claimButtonObjects[i].GetComponent<Button>().interactable = true;
                    }
                    else
                    {
                        claimButtonObjects[i].GetComponent<Button>().interactable = false;
                    }
                }
                else
                {
                    challengesObjects[i].transform.Find("MissionTxt").GetComponent<TMP_Text>().text = "Task done";
                    rewardsObjects[i].transform.Find("RewardTxt").GetComponent<TMP_Text>().text = "";
                    claimButtonObjects[i].GetComponent<Button>().interactable = false;
                }
            }
        }
    }
    int RandomRangeExcept(int min, int max)
    {
        int number;
        do
        {
            number = Random.Range(min, max);
        } while (except.Contains(number));
        return number;
    }
    public void CollectReward(int index)
    {
        if (playerStats.challenges[index].reward.lootName == "Gold")
        {
            playerStats.gold += playerStats.challenges[index].amount;
        }
        claimButtonObjects[index].GetComponent<Button>().interactable = false;

        playerStats.challenges[index].isDone = true;

        for (int i = 0; i < challengesObjects.Length; i++)
        {
            challengesObjects[index].transform.Find("MissionTxt").GetComponent<TMP_Text>().text = "Task done";
            rewardsObjects[index].transform.Find("RewardTxt").GetComponent<TMP_Text>().text = "";
            //rewardsObjects[index].transform.Find("RewardIcon").GetComponent<Image>().enabled = false;
            claimButtonObjects[index].GetComponent<Button>().interactable = false;
        }
        buttonSound.Play();
        //save.LocalSavePlayerStats();
    }
}
