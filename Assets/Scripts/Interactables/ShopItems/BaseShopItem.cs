using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseShopItem : MonoBehaviour, IInteractable, IModifyState
{
    public PlayAudioEventSO FXEvent;
    public AudioClip DenyAudio;
    public int GoldPrice;

    private Inventory inventory;
    private Character character;
    private void Awake()
    {
        inventory = GlobalState.Inventory;
        character = GlobalState.PlayerCharacter;
    }
    public void TriggerAction()
    {
        TryPurchase();
    }

    /// <summary>
    /// ÂòµÀ¾ßÂß¼­
    /// </summary>
    private void TryPurchase()
    {
        if (inventory.Gold < GoldPrice)
        { PlayDenySound(); return; }
        inventory.Gold -= GoldPrice;
        ModifyInventory(inventory);
        ModifyCharacter(character);
    }


    private void PlayDenySound()
    {
        FXEvent.RaiseEvent(DenyAudio);
    }

    public abstract void ModifyInventory(Inventory inventory);
    public abstract void ModifyCharacter(Character character);
}
