using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using TMPro;
public class FirstBossMinionScript : MonoBehaviour
{
    [Header("Other Scripts")]
    public PlayerStats playerStats;
    [Space(20f)]
    [Header("Variables")]
    public float moveSpeed = 2f;
    [Space(20f)]
    [Header("GameObjects and Rest")]
    public GameObject bossObject;
    [SerializeField] private GameObject damageTextPrefab;
    [Space(20f)]
    [Header("Health System")]
    [SerializeField] private Canvas canvas;
    [SerializeField] private GameObject healthBarCanvas;
    [SerializeField] private Slider healthBar;
    [SerializeField] private Gradient healthGradient;
    [SerializeField] private Image fillBar;
    [SerializeField] private int maxHealth;
    public int currentHealth;
    private float hideTimer = 0f;
    [Space(20f)]
    [Header("Particles and animator staff")]
    public Animator animator;
    private bool isFlameStarted = false;
    private bool isPoisonStarted = false;
    private GameObject flameParticle;
    private GameObject poisonParticle;
    private GameObject dashParticle;
    private ObjectPool[] objPools;
    public Vector2 retreatVector;
    private void Start()
    {
        objPools = GetComponents<ObjectPool>();
        canvas.worldCamera = GameObject.Find("Main Camera").GetComponent<Camera>();
        SetMaxHealth(maxHealth);
        healthBarCanvas.SetActive(false);
    }
    private void Update()
    {
        fillBar.color = healthGradient.Evaluate(healthBar.normalizedValue);
        hideTimer += Time.deltaTime;
        if (hideTimer > 3f)
        {
            healthBarCanvas.SetActive(false);
        }

        if (currentHealth <= 0)
        {
            SetMaxHealth(maxHealth);
            moveSpeed = 2f;
            GameObject.Find("FirstBoss(Clone)").GetComponent<FirstBossScript>().Shield();
            gameObject.SetActive(false);
        }


        if (flameParticle != null)
        {
            if (!flameParticle.GetComponent<ParticleSystem>().IsAlive())
            {
                isFlameStarted = false;
                flameParticle.SetActive(false);
            }
        }

        if (poisonParticle != null)
        {
            if (!poisonParticle.GetComponent<ParticleSystem>().IsAlive())
            {
                isPoisonStarted = false;
                poisonParticle.SetActive(false);
            }
        }

        if (dashParticle != null)
        {
            if (!dashParticle.GetComponent<ParticleSystem>().IsAlive())
            {
                dashParticle.SetActive(false);
            }
        }
        ChangeRotation();
    }
    public void ChangeRotation()
    {
        GameObject ship = FindClosestObject();
        Vector3 vectorToTarget = ship.transform.position - animator.transform.position;
        animator.transform.Find("EnemyShipImage").transform.rotation = Quaternion.LookRotation(Vector3.forward, vectorToTarget);
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
    public void CollisionDetected(int damage)
    {
        healthBarCanvas.SetActive(true);
        hideTimer = 0f;
        currentHealth -= damage;
        SetHealth();
        moveSpeed = 0.5f;
        StartCoroutine(ChangingSpeed());
        if (damageTextPrefab)
        {
            ShowDamageText(damage);
        }
    }
    private void ShowDamageText(int amount)
    {
        var text = Instantiate(damageTextPrefab, transform.position, Quaternion.identity);
        text.GetComponent<TMP_Text>().text = amount.ToString();
    }
    IEnumerator ChangingSpeed()
    {
        yield return new WaitForSeconds(0.3f);

        while (moveSpeed < 2f)
        {
            moveSpeed += 0.1f;
            yield return new WaitForSeconds(0.1f);

        }
    }
    public void StartPoison()
    {
        if (!isPoisonStarted)
        {
            isPoisonStarted = true;
            StartCoroutine("Poison");

        }
    }
    IEnumerator Poison()
    {
        float elapsedTime = 0f;
        foreach (ObjectPool script in objPools)
        {
            if (script.type == "poisonParticle")
            {
                poisonParticle = script.GetPooledObject();
                poisonParticle.transform.parent = transform;
                ParticleSystem.MainModule main = poisonParticle.GetComponent<ParticleSystem>().main;
                main.duration = playerStats.poisonGunDurationValue;
                poisonParticle.SetActive(true);
                poisonParticle.transform.position = transform.position;
            }
        }
        while (elapsedTime <= playerStats.poisonGunDurationValue)
        {
            CollisionDetected((int)playerStats.poisonGunBetweenDamageValue);
            elapsedTime += playerStats.poisonGunBetweenAttackSpeedValue;
            yield return new WaitForSeconds(playerStats.poisonGunBetweenAttackSpeedValue);
        }
    }
    public void StartFlame()
    {
        if (!isFlameStarted)
        {
            isFlameStarted = true;
            StartCoroutine("Flame");
        }
    }
    IEnumerator Flame()
    {
        float elapsedTime = 0f;
        foreach (ObjectPool script in objPools)
        {
            if (script.type == "flameParticle")
            {
                flameParticle = script.GetPooledObject(); //tu cos nie gra
                flameParticle.transform.parent = transform;
                ParticleSystem.MainModule main = flameParticle.GetComponent<ParticleSystem>().main;
                main.duration = playerStats.flameGunDurationValue;
                flameParticle.SetActive(true);
                flameParticle.transform.position = transform.position;
            }
        }
        while (elapsedTime <= playerStats.flameGunDurationValue)
        {
            yield return new WaitForSeconds(0.5f);
            CollisionDetected((int)playerStats.flameGunBetweenDamageValue);
            elapsedTime += 0.5f;
        }
    }
    public void StartStun()
    {
        StartCoroutine("Stun");
    }
    IEnumerator Stun()
    {
        moveSpeed = 0f;
        yield return new WaitForSeconds(playerStats.stunDurationValue);

        while (moveSpeed < 2f)
        {
            moveSpeed += 0.1f;
            yield return new WaitForSeconds(0.1f);

        }
    }
    public GameObject FindClosestObject()
    {
        GameObject[] objectsWithTag = GameObject.FindGameObjectsWithTag("Ship");

        if (objectsWithTag.Length == 0)
        {
            return null;
        }
        float closestDistance = Mathf.Infinity;
        GameObject closestObject = null;

        foreach (GameObject obj in objectsWithTag)
        {
            float distance = Vector3.Distance(transform.position, obj.transform.position);
            if (distance < closestDistance)
            {
                closestDistance = distance;
                closestObject = obj;
            }
        }

        return closestObject;
    }
    public void Pivot(float rotatingSpeed)
    {
        if (bossObject)
        {
            transform.RotateAround(bossObject.transform.position, Vector3.forward, rotatingSpeed * Time.deltaTime);
        }
    }
    public void HealBoss()
    {
        if (bossObject)
        {
            bossObject.GetComponent<FirstBossScript>().currentHealth += 1;
            bossObject.GetComponent<FirstBossScript>().SetHealth();
        }
    }
}
