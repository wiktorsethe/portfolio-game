using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPool : MonoBehaviour
{
    // Lista obiekt�w w puli
    public List<GameObject> pooledObjects = new List<GameObject>();
    [SerializeField] private GameObject prefab; // Prefabrykat obiektu do instancjonowania
    public string type; // Typ obiektu w puli (np. "bullet", "enemy", "explosion")
    void Start()
    {
        // Na starcie utw�rz pocz�tkow� pul� obiekt�w
        for (int i = 0; i < 1; i++)
        {
            InstantiateObject();
        }
    }

    // Metoda do pobierania obiektu z puli
    public GameObject GetPooledObject()
    {
        bool active = false;

        // Sprawd�, czy istnieje nieaktywny obiekt w puli
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

        // Je�li wszystkie obiekty s� aktywne, utw�rz nowy obiekt
        if (active)
        {
            InstantiateObject();
        }

        Next:
        // Znajd� i zwr�� pierwszy nieaktywny obiekt w puli
        for (int i=0; i<pooledObjects.Count; i++)
        {
            if (!pooledObjects[i].activeInHierarchy)
            {
                return pooledObjects[i];
            }
        }

         
        return null; // Zwr�� null, je�li nie ma dost�pnego obiektu w puli
    }
    private void InstantiateObject()
    {
        GameObject bullet = Instantiate(prefab);
        bullet.SetActive(false); // Ustawienie obiektu jako nieaktywnego
        pooledObjects.Add(bullet); // Dodanie obiektu do puli
    }
}
