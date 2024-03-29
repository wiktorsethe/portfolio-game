using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootingPoisonGun : MonoBehaviour
{
    [Header("Other Scripts")]
    public PlayerStats playerStats;
    private ObjectPool objPool;
    [Space(20f)]

    [Header("Objects and List")]
    [SerializeField] private Animator shootAnimator;
    [SerializeField] private Transform firePoint;
    [SerializeField] private AudioSource shootingSound;
    [Space(20f)]

    [Header("Variables")]
    [SerializeField] private float bulletSpeed = 5f;
    [SerializeField] private string target;
    public float shootTimer = 0f;
    private void Start()
    {
        objPool = GetComponent<ObjectPool>();
        shootingSound = GameObject.Find("ShootingSmallGuns").GetComponent<AudioSource>();
    }
    void Update()
    {
        shootTimer += Time.deltaTime;
        if (shootTimer >= playerStats.poisonGunCollisionAttackSpeedValue)
        {
            FireBullet();
            shootTimer = 0f;
        }

    }
    void FireBullet()
    {
        shootAnimator.SetTrigger("Play");
        GameObject bullet = objPool.GetPooledObject();
        shootingSound.Play();
        bullet.transform.position = firePoint.position;
        bullet.GetComponent<ShootingBullet>().startingPos = firePoint.position;
        bullet.transform.rotation = firePoint.rotation;
        bullet.SetActive(true);
        bullet.GetComponent<ShootingBullet>().target = target;
        bullet.GetComponent<ShootingBullet>().damage = playerStats.poisonGunCollisionDamageValue;
        bullet.GetComponent<ShootingBullet>().ChangeSize();
        Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();
        Vector2 bulletVelocity = firePoint.up * bulletSpeed;
        rb.velocity = bulletVelocity;
    }
}
