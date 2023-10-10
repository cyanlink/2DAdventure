using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoarChaseState : BaseState
{
    public override void OnEnter(Enemy enemy)
    {
        Debug.Log("Enter Chase State.");
        currentEnemy = enemy;
        currentEnemy.currentSpeed = currentEnemy.chaseSpeed;
        currentEnemy.anim.SetBool("run", true);
    }

    Vector3 TurnVector = new (-1, 1, 1);
    public override void LogicUpdate()
    {
        if(currentEnemy.loseTimeCounter<=0)
        {
            currentEnemy.SwitchState(NPCState.Patrol);
        }

        if (!currentEnemy.physicsCheck.isGround || (currentEnemy.physicsCheck.touchLeftWall && currentEnemy.faceDir.x < 0) || (currentEnemy.physicsCheck.touchRightWall && currentEnemy.faceDir.x > 0))
        {
            //TODO 代码各处这种硬编码localScale为1的都应该更改，我们不可避免会调整一些元素的缩放
            var oldScale = currentEnemy.transform.localScale;
            currentEnemy.transform.localScale = Vector3.Scale(TurnVector, oldScale);
        }
    }
    public override void PhysicsUpdate()
    {

    }
    public override void OnExit()
    {
        currentEnemy.loseTimeCounter = currentEnemy.loseTime;
        currentEnemy.anim.SetBool("run", false);
    }
}
