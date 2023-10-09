using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeartShopItem : BaseShopItem
{
    public override void ModifyCharacter(Character character)
    {
        character.Heal(1);
    }

    public override void ModifyInventory(Inventory inventory)
    {
        //DO nothing
    }
}
