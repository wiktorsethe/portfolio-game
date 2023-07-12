using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelGenerator : MonoBehaviour
{
    public List<GameObject> terrainSpawnPoints = new List<GameObject>();
    public GameObject[] levelParts;
    private Camera mainCamera;
    private float bufferDistance = 1.2f;
    private void Start()
    {
        mainCamera = Camera.main;
    }
    private void Update()
    {
        for (int i = 0; i < terrainSpawnPoints.Count; i++)
        {
            if (!IsObjectOutsideScreen(terrainSpawnPoints[i]))
            {
                int randInt = Random.Range(0, levelParts.Length);
                Instantiate(levelParts[randInt], terrainSpawnPoints[i].transform.position, Quaternion.identity);
                terrainSpawnPoints.RemoveAt(i);
            }
        }
    }
    private bool IsObjectOutsideScreen(GameObject spawnpoint)
    {
        Vector3 screenPoint = mainCamera.WorldToViewportPoint(spawnpoint.transform.position);
        bool isOutsideScreen = screenPoint.x < -bufferDistance ||
                               screenPoint.x > 1 + bufferDistance ||
                               screenPoint.y < -bufferDistance ||
                               screenPoint.y > 1 + bufferDistance;

        return isOutsideScreen;
    }
}