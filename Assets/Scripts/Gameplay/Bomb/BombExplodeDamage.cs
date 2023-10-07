using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BombExplodeDamage : MonoBehaviour, IAttack
{
    private Collider2D col;
    private Dictionary<GameObject, ITakeDamage> damageTakers = new Dictionary<GameObject, ITakeDamage>();

    public int damage;
    public float attackRange;
    public float attackRate;

    int IAttack.Damage =>damage;

    float IAttack.AttackRange => attackRange;

    float IAttack.AttackRate => attackRate;

    public Transform Transform => transform;

    private void Awake()
    {
        col = GetComponent<Collider2D>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        var taker = collision.gameObject.GetComponent<ITakeDamage>();
        if (taker != null)
        {
            damageTakers.Add(collision.gameObject, taker);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        damageTakers.Remove(collision.gameObject);
    }
    public void DealExplodeDamage()
    {
        foreach(var taker in damageTakers.Values)
        {
            taker.TakeDamage(this);
        }
    }
}
