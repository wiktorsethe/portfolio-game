using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPool : MonoBehaviour
{
    public List<GameObject> pooledObjects = new List<GameObject>();
    [SerializeField] private GameObject prefab;
    public string type;
    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < 1; i++)
        {
            InstantiateObject();
        }
    }

    public GameObject GetPooledObject()
    {
        bool active = false;
        foreach (GameObject obj in pooledObjects)
        {
            if (!obj.activeSelf)
            {
                goto Next;
            }
            else if (obj.activeSelf)
            {
                active = true;
            }
        }

        if (active)
        {
            InstantiateObject();
        }

        Next:
        for (int i=0; i<pooledObjects.Count; i++)
        {
            if (!pooledObjects[i].activeInHierarchy)
            {
                return pooledObjects[i];
            }
        }


        return null;
    }
    private void InstantiateObject()
    {
        GameObject bullet = Instantiate(prefab);
        bullet.SetActive(false);
        pooledObjects.Add(bullet);
    }
}
