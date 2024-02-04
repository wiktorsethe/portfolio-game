using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LootBag : MonoBehaviour
{
    [Header("Other GameObjects")]
    [SerializeField] private GameObject droppedItemPrefab;
    [Space(20f)]

    [Header("List of Loot Items")]
    [SerializeField] private List<Loot> lootList = new List<Loot>();

    // Metoda do wyboru przedmiotu, kt�ry zostanie upuszczony
    Loot GetDroppedItem()
    {
        int randomNumber = Random.Range(1, 101);
        List<Loot> possibleItems = new List<Loot>();

        // Przeszukaj list� element�w do upuszczenia
        foreach (Loot item in lootList)
        {
            if (randomNumber <= item.dropChance)
            {
                possibleItems.Add(item); // Dodaj element do listy mo�liwych przedmiot�w, je�li spe�nia warunek szansy
            }
        }
        // Je�li istniej� mo�liwe przedmioty, wybierz jeden z nich losowo
        if (possibleItems.Count > 0)
        {
            Loot droppedItem = possibleItems[Random.Range(0, possibleItems.Count)];
            return droppedItem;
        }
        return null; // Zwr�� null, je�li nie uda�o si� wybra� przedmiotu
    }

    // Metoda do instancjonowania przedmiotu na podanej pozycji
    public void InstantiateLoot(Vector3 spawnPosition)
    {
        Loot droppedItem = GetDroppedItem(); // Wybierz przedmiot do upuszczenia
        if (droppedItem != null)
        {
            // Instancjonuj obiekt przedmiotu na podanej pozycji
            GameObject lootGameObject = Instantiate(droppedItemPrefab, spawnPosition, Quaternion.identity);
            // Przypisz nazw� przedmiotu do komponentu CollectLoot
            lootGameObject.GetComponent<CollectLoot>().lootName = droppedItem.lootName;
        }
    }
}
