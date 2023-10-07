using Cysharp.Threading.Tasks;
using Cysharp.Threading.Tasks.Triggers;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bomb : MonoBehaviour
{
    [Header("≈‰÷√")]
    public float DelayDetonateTime;
    

    private Rigidbody2D rb;
    private Collider2D col;
    private Animator anim;
    private BombExplodeDamage damager;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        col = GetComponent<Collider2D>();
        anim = GetComponent<Animator>();
        damager = GetComponentInChildren<BombExplodeDamage>();
    }


    // Start is called before the first frame update
    void Start()
    {
        anim.Play("blink");
        _ = DetonateAfterCountDown();
    }

    private async UniTaskVoid DetonateAfterCountDown()
    {
        await UniTask.Delay(TimeSpan.FromSeconds(DelayDetonateTime));
        anim.Play("explode");
        DealDamage();
    }

    private void DealDamage()
    {
        damager.DealExplodeDamage();
    }
}
