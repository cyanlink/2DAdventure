using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attack : MonoBehaviour, IAttack
{
    [Header("基本属性")]
    public int damage;
    public float attackRange;
    public float attackRate;

    int IAttack.Damage => this.damage;

    float IAttack.AttackRange => this.attackRange;

    float IAttack.AttackRate => this.attackRate;

    Transform IAttack.Transform => this.transform;

    private void OnTriggerStay2D(Collider2D other)
    {
        //?即先询问有没有这个组件，没有则不执行
        other.GetComponent<ITakeDamage>()?.TakeDamage(this);
    }
}

public interface ITakeDamage
{
    public void TakeDamage(IAttack attacker);
}

public interface IAttack {
    public int Damage { get;}
    public float AttackRange { get;}

    public float AttackRate { get;}

    public Transform Transform { get;}
}

