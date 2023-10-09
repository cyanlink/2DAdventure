using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlotMachineController : MonoBehaviour, IInteractable
{
    private SlotMachineState state;
    private Inventory inventory;

    public List<SlotMachineRow> rows;

    private void Awake()
    {
        state = GlobalState.SlotMachine;
        inventory = GlobalState.Inventory;
    }

    public void TriggerAction()
    {
        TryRollSlotMachine();
    }

    public void TryRollSlotMachine()
    {
        if (inventory.Gold < state.GoldNeededToRoll)
        { PlayDenySound(); return; }
        inventory.Gold -= state.GoldNeededToRoll;
        state.RollSlotMachine();

    }

    private void PlayDenySound()
    {
        
    }


}
