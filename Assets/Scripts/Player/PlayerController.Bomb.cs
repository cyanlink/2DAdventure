using Cysharp.Threading.Tasks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.InputSystem;

public partial class PlayerController 
{
    [Header("组件")]
    public BombIndicator bombIcon;
    public GameObject bombPrefab;
    public GameObject arrowPrefab;

    [Header("配置")]
    public float bombInitialSpeed;
    public float arrowInitialSpeed;

    public float bombTossDelay = 0.5f;
    public float bombCoolDown = 5f;
    public bool isHoldingBomb { get; private set; }
    public bool isHoldingArrow { get; private set; }

    private bool allowTossBomb = false;
    private void BombAction(InputAction.CallbackContext ctx)
    {
        if (!isHoldingBomb)
        {
            isHoldingBomb = true;
            HoldBomb();
            AllowTossBombLater().Forget();
        }
        else if(allowTossBomb) 
        {
            TossBomb();
            isHoldingBomb = false;
            allowTossBomb = false;
        }
    }

    private void HoldBomb()
    {
        bombIcon.ToggleBombIcon(true);
    }

    private void TossBomb()
    {
        Vector3 dir = new Vector3(faceDir, 0, 0);

        var bomb = Instantiate(bombPrefab, bombIcon.transform.position, Quaternion.identity);
        bomb.GetComponent<Rigidbody2D>().velocity = dir * bombInitialSpeed;
        bombIcon.ToggleBombIcon(false);
    }

    private async UniTaskVoid AllowTossBombLater()
    {
        await UniTask.Delay(TimeSpan.FromSeconds(bombTossDelay));
        allowTossBomb = true;
    } 

    private void ArrowAction(InputAction.CallbackContext ctx)
    {
        if(!isHoldingArrow)
        {
            isHoldingArrow = true;
            HoldArrow();
        }
        else
        {
            FireArrow();
            isHoldingArrow= false;
        }
    }

    private void HoldArrow()
    {

    }

    private void FireArrow()
    {
        
    }
}