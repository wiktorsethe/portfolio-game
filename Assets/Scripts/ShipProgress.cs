using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ShipProgress", menuName = "GameData/Ship Progress")]
public class ShipProgress : ScriptableObject
{
    public List<ShipPartInScene> shipParts = new List<ShipPartInScene>();
    public List<Vector3> usedContstructPoints = new List<Vector3>();
}