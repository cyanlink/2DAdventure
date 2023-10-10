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
            //TODO �����������Ӳ����localScaleΪ1�Ķ�Ӧ�ø��ģ����ǲ��ɱ�������һЩԪ�ص�����
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
