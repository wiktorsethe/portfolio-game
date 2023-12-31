using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class MenuMisteriousPlace : MonoBehaviour
{
    [Header("Other Scripts")]
    public PlayerStats playerStats;
    public ShipProgress shipProgress;
    public CardsDatabase cardsDB;
    private WalkerMovement walkerMovement;
    private CameraSize camSize;
    [Space(20f)]
    [Header("Other GameObjects")]
    [SerializeField] private GameObject pauseMenu;
    [SerializeField] private GameObject gameMenu;
    [SerializeField] private GameObject misteriousManMenu;
    [SerializeField] private GameObject forgeManMenu;
    [SerializeField] private GameObject changeManMenu;
    [SerializeField] private GameObject cardPrefab;
    private Camera mainCam;
    [Space(20f)]
    [Header("Lists")]
    [SerializeField] private List<Card> generatedCards = new List<Card>();
    [SerializeField] private Animator transition;
    [SerializeField] private GameObject levelLoader;
    private void Start()
    {
        camSize = GameObject.FindObjectOfType(typeof(CameraSize)) as CameraSize;
        mainCam = Camera.main;
    }
    public void ExitMenu()
    {
        Time.timeScale = 1f;
        gameMenu.SetActive(true);
        pauseMenu.SetActive(false);
        misteriousManMenu.SetActive(false);
        forgeManMenu.SetActive(false);
        changeManMenu.SetActive(false);
    }
    public void PauseMenu()
    {
        Time.timeScale = 0f;
        pauseMenu.SetActive(true);
        gameMenu.SetActive(false);
        misteriousManMenu.SetActive(false);
        forgeManMenu.SetActive(false);
        changeManMenu.SetActive(false);
    }
    public void Resume()
    {
        Time.timeScale = 1f;
        gameMenu.SetActive(true);
        pauseMenu.SetActive(false);
        misteriousManMenu.SetActive(false);
        forgeManMenu.SetActive(false);
        changeManMenu.SetActive(false);

    }
    public void MisteriousManMenu()
    {
        gameMenu.SetActive(false);
        pauseMenu.SetActive(false);
        misteriousManMenu.SetActive(true);
        forgeManMenu.SetActive(false);
        changeManMenu.SetActive(false);
        for (int i=0; i<cardsDB.cards.Length; i++)
        {
            GameObject obj = Instantiate(cardPrefab, misteriousManMenu.transform.Find("Scroll").transform.Find("Panel").transform);
            obj.GetComponent<CardMenu>().index = i;
            obj.transform.Find("Image").GetComponent<Image>().sprite = cardsDB.cards[i].image;
            obj.transform.Find("DescriptionText").GetComponent<TMP_Text>().text = cardsDB.cards[i].description.ToString();
            generatedCards.Add(cardsDB.cards[i]);
            //shipPartsInstantiate.Add(obj);
            obj.GetComponent<Button>().onClick.AddListener(() => ChooseCard(obj.GetComponent<CardMenu>().index));
        }
    }
    public void ForgeManMenu()
    {
        gameMenu.SetActive(false);
        pauseMenu.SetActive(false);
        misteriousManMenu.SetActive(false);
        forgeManMenu.SetActive(true);
        changeManMenu.SetActive(false);
    }
    public void ChangeManMenu()
    {
        gameMenu.SetActive(false);
        pauseMenu.SetActive(false);
        misteriousManMenu.SetActive(false);
        forgeManMenu.SetActive(false);
        changeManMenu.SetActive(true);
    }
    public void Restart()
    {
        Resume();
        playerStats.Reset();
        shipProgress.Reset();
        StartCoroutine("LoadUniverse");
    }
    public void BackToMenu()
    {
        Resume();
        StartCoroutine("LoadMenu");
    }
    public void ChooseCard(int i)
    {
        playerStats.normalGunDamageValue += generatedCards[i].normalGunDamageValue * 2;
        playerStats.laserGunDamageValue += generatedCards[i].laserGunDamageValue * 2;
        playerStats.bigGunDamageValue += generatedCards[i].bigGunDamageValue * 2;
        playerStats.doubleGunDamageValue += generatedCards[i].doubleGunDamageValue * 2;
        playerStats.normalGunAttackSpeedValue += generatedCards[i].normalGunAttackSpeedValue * 2;
        playerStats.laserGunAttackSpeedValue += generatedCards[i].laserGunAttackSpeedValue * 2;
        playerStats.bigGunAttackSpeedValue += generatedCards[i].bigGunAttackSpeedValue * 2;
        playerStats.doubleGunAttackSpeedValue += generatedCards[i].doubleGunAttackSpeedValue * 2;
        playerStats.shipSpeedValue += generatedCards[i].shipSpeedValue * 2;
        playerStats.oreMiningBonusValue = generatedCards[i].oreMiningBonus * 2;
        gameMenu.SetActive(true);
        pauseMenu.SetActive(false);
        misteriousManMenu.SetActive(false);
        Time.timeScale = 1f;
        camSize.CamSize(mainCam.orthographicSize / 3f, 5f);
        Invoke("ChangeScene", 5f);
    }
    void ChangeScene()
    {
        SceneManager.LoadScene("Universe");

    }
    private IEnumerator LoadMenu()
    {
        levelLoader.SetActive(true);
        transition.SetTrigger("Start");
        yield return new WaitForSeconds(1f);
        SceneManager.LoadScene("main");
    }
    private IEnumerator LoadUniverse()
    {
        levelLoader.SetActive(true);
        transition.SetTrigger("Start");
        yield return new WaitForSeconds(1f);
        SceneManager.LoadScene("Universe");
    }
}
