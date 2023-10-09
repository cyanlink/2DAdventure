using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Inventory))]
public class SlotMachineState : MonoBehaviour
{
    //第一次免费
    public int GoldNeededToRoll = 0;
    public int RerollPenalty = 10;

    public Inventory inventory;

    private PenaltyType penaltyType = PenaltyType.Addition;
    private void Awake()
    {
        inventory = GetComponent<Inventory>();
    }
    public void RollSlotMachine()
    {
        GetRandomResult();
        
        ApplyRerollPenalty();
    }

    private void GetRandomResult()
    {
        
    }

    private void ApplyRerollPenalty()
    {
        GoldNeededToRoll = (penaltyType) switch
        {
            PenaltyType.Addition => GoldNeededToRoll + RerollPenalty,
            PenaltyType.Multiply => GoldNeededToRoll == 0 ? RerollPenalty : GoldNeededToRoll * 2,
            _ => throw new NotImplementedException(),
        };
    }


}

public enum PenaltyType
{
    Addition, Multiply
}
