using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Inventory")]
public class InventorySO : ScriptableObject
{
    public int BombCount;
    public int ArrowCount;

    public int BombCountMax;
    public int ArrowCountMax;
}