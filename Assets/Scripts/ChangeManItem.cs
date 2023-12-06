using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class ChangeManItem : MonoBehaviour
{
    public ShipPartsDatabase shipPartsDB;
    public int shipPartNumber;
    [SerializeField] private TMP_Text amountText;
    
    public void Minus()
    {
        if(shipPartsDB.shipParts[shipPartNumber].ownedAmount > 0)
        {
            int changeCards = PlayerPrefs.GetInt("ChangeCards");
            PlayerPrefs.SetInt("ChangeCards", changeCards += 1);

            shipPartsDB.shipParts[shipPartNumber].ownedAmount--;
            amountText.text = shipPartsDB.shipParts[shipPartNumber].ownedAmount.ToString();
        }
    }
    public void Plus()
    {
        int changeCards = PlayerPrefs.GetInt("ChangeCards");
        if (changeCards > 0)
        {
            PlayerPrefs.SetInt("ChangeCards", changeCards -= 1);

            shipPartsDB.shipParts[shipPartNumber].ownedAmount++;
            amountText.text = shipPartsDB.shipParts[shipPartNumber].ownedAmount.ToString();
        }
    }
}
