using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Meteorite : MonoBehaviour
{
    [Header("Other Scripts")]
    private HpBar hpBar;
    [Space(20f)]

    [Header("Objects")]
    [SerializeField] private Transform playerShip;
    [Space(20f)]

    [Header("Variables")]
    [SerializeField] private float speed = 5f;
    private bool hasDirection = false;
    private Vector3 direction;
    private Camera mainCamera;
    private float bufferDistance = 1.5f;
    private void Start()
    {
        hpBar = GameObject.FindObjectOfType(typeof(HpBar)) as HpBar;
        //gameManager.AddToTrashList(gameObject);
        playerShip = GameObject.FindGameObjectWithTag("Player").transform;
        mainCamera = Camera.main;

        float cameraHeight = mainCamera.orthographicSize * 0.13f;
        float cameraWidth = cameraHeight * mainCamera.aspect;

        float scaleX = cameraWidth / transform.localScale.x;
        float scaleY = cameraHeight / transform.localScale.y;

        float objectScale = Mathf.Min(scaleX, scaleY);
        transform.localScale = new Vector3(objectScale, objectScale, 1f);
    }
    private void Update()
    {
        if (!hasDirection)
        {
            direction = playerShip.position - transform.position;
            hasDirection = true;
        }
        transform.rotation = Quaternion.LookRotation(Vector3.forward, direction);
        transform.Translate(Vector3.up * speed * Time.deltaTime);
        if (IsObjectOutsideScreen())
        {
            gameObject.SetActive(false);
            hasDirection = false;
        }
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.transform.CompareTag("Player"))
        {
            hpBar.SetHealth(30f);
            hasDirection = false;
            gameObject.SetActive(false);
        }
    }
    private bool IsObjectOutsideScreen()
    {
        Vector3 screenPoint = mainCamera.WorldToViewportPoint(transform.position);
        bool isOutsideScreen = screenPoint.x < -bufferDistance ||
                               screenPoint.x > 1 + bufferDistance ||
                               screenPoint.y < -bufferDistance ||
                               screenPoint.y > 1 + bufferDistance;

        return isOutsideScreen;
    }
}
