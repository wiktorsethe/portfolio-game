using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using TMPro;
public class EnemyShaman : MonoBehaviour
{
    [Header("Other Scripts")]
    private ExpBar expBar;
    public PlayerStats playerStats;
    private GameManager gameManager;
    private ObjectPool[] objPools;
    [Space(20f)]
    [Header("Variables")]
    [SerializeField] private int experience; // ??
    [SerializeField] private int gold;
    private float spawnTimer = 0f;
    private float moveSpeed = 2f;
    private float inTarget = 13;
    [Space(20f)]
    [Header("GameObjects and Rest")]
    private GameObject player;
    [SerializeField] private GameObject[] spawnPoints;
    [SerializeField] private GameObject enemyWarriorPrefab;
    private List<GameObject> minions = new List<GameObject>();
    [SerializeField] private GameObject miningTextPrefab;
    [SerializeField] private Animator animator;
    private bool facingRight;
    [SerializeField] private AudioSource spellSound;
    [Space(20f)]
    [Header("Health System")]
    [SerializeField] private Canvas canvas;
    [SerializeField] private GameObject healthBarCanvas;
    [SerializeField] private Slider healthBar;
    [SerializeField] private Gradient healthGradient;
    [SerializeField] private Image fillBar;
    [SerializeField] private int maxHealth;
    private int currentHealth;
    [SerializeField] private float Angle;
    private float hideTimer = 0f;
    private bool isObjectActivated = false;
    private void Start()
    {
        expBar = GameObject.FindObjectOfType(typeof(ExpBar)) as ExpBar;
        gameManager = GameObject.FindObjectOfType(typeof(GameManager)) as GameManager;
        player = GameObject.FindGameObjectWithTag("Player");
        objPools = GetComponents<ObjectPool>();
        spellSound = GameObject.Find("VolumeManager").transform.Find("Spell").GetComponent<AudioSource>();
        canvas.worldCamera = GameObject.Find("Main Camera").GetComponent<Camera>();
        currentHealth = maxHealth;
        SetMaxHealth(maxHealth);
        healthBarCanvas.SetActive(false);
    }
    private void Update()
    {
        fillBar.color = healthGradient.Evaluate(healthBar.normalizedValue);
        hideTimer += Time.deltaTime;

        Vector3 direction = player.transform.position - transform.position;
        if (direction.x > 0 && facingRight)
        {
            Flip();
        }
        if (direction.x < 0 && !facingRight)
        {
            Flip();
        }

        if (hideTimer > 2f)
        {
            healthBarCanvas.SetActive(false);
        }
        spawnTimer += Time.deltaTime;
        float distance = Vector2.Distance(transform.position, player.transform.position);
        if (distance >= (inTarget - 1f) && distance < 30f && spawnTimer >= 2.5f)
        {
            //Vector3 vectorToTarget = player.transform.position - transform.position;
            //transform.Find("EnemyShamanImage").transform.rotation = Quaternion.LookRotation(Vector3.forward, vectorToTarget); // rotacja do przegadania bo pewnie bedzie tylko L/R
            transform.position = Vector2.MoveTowards(transform.position, player.transform.position, moveSpeed * Time.deltaTime);
        }
        else if(distance < inTarget)
        {
            if(minions.Count == 0)
            {
                SpawnMinions();
                Invoke("EnableMinions", 0.4f);
                animator.SetTrigger("Play");
                spawnTimer = 0f;
            }
        }

        if (!isObjectActivated)
        {
            SetMaxHealth(maxHealth);
            isObjectActivated = true;
        }

        if (currentHealth <= 0)
        {
            SetMaxHealth(maxHealth);
            expBar.SetExperience(experience);
            playerStats.gold += gold;
            gameManager.goldEarned += gold;
            gameManager.kills += 1;
            spawnTimer = -10f;
            GetComponent<LootBag>().InstantiateLoot(transform.position);
            isObjectActivated = false;
            gameObject.SetActive(false);
        }
    }
    private void SpawnMinions()
    {
        spellSound.Play();
        foreach (ObjectPool script in objPools)
        {
            if (script.type == "minion1")
            {
                GameObject minion = script.GetPooledObject();
                minion.transform.position = spawnPoints[0].transform.position;
                minion.transform.rotation = Quaternion.identity;
                minions.Add(minion);
                minion.SetActive(false);
            }

            if (script.type == "minion2")
            {
                GameObject minion = script.GetPooledObject();
                minion.transform.position = spawnPoints[1].transform.position;
                minion.transform.rotation = Quaternion.identity;
                minions.Add(minion);
                minion.SetActive(false);
            }

            if (script.type == "minion3")
            {
                GameObject minion = script.GetPooledObject();
                minion.transform.position = spawnPoints[2].transform.position;
                minion.transform.rotation = Quaternion.identity;
                minions.Add(minion);
                minion.SetActive(false);
            }
        }
    }
    private void EnableMinions()
    {
        foreach(GameObject minion in minions)
        {
            minion.SetActive(true);
        }
    }
    public void SetMaxHealth(int health)
    {
        currentHealth = health;
        maxHealth = health;
        healthBar.maxValue = health;
        healthBar.value = health;
        fillBar.color = healthGradient.Evaluate(1f);
    }
    public void SetHealth()
    {
        DOTween.To(() => healthBar.value, x => healthBar.value = x, currentHealth, 1.5f);
    }
    public void CollisionDetected()
    {
        healthBarCanvas.SetActive(true);
        hideTimer = 0f;
        currentHealth -= 10;
        SetHealth();

        if (miningTextPrefab)
        {
            ShowMiningText(10);
        }
    }
    private void ShowMiningText(int amount)
    {
        var text = Instantiate(miningTextPrefab, transform.position, Quaternion.identity);
        text.GetComponent<TMP_Text>().text = amount.ToString();
    }
    private void Flip()
    {
        Vector3 currentScale = gameObject.transform.localScale;
        currentScale.x *= -1;
        gameObject.transform.localScale = currentScale;
        facingRight = !facingRight;
    }
}
