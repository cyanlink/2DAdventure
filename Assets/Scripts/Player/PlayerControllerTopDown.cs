using System.Collections;
using System.Collections.Generic;
using UnityEditor.Animations;
using UnityEngine;

public class PlayerControllerTopDown : MonoBehaviour
{
    public float runSpeed;
    private Vector2 speedCap;

    private PlayerInputControl inputControl;

    private Rigidbody2D rb;
    private CapsuleCollider2D capsuleCollider;
    private Animator anim;

    private Vector2 inputDir;

    private void Awake()
    {
        inputControl = InputRouter.Instance.InputControls;
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();

    speedCap = new Vector2(runSpeed, runSpeed);

}

private void OnEnable()
    {
        inputControl.Gameplay.Enable();
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        inputDir = inputControl.Gameplay.Move.ReadValue<Vector2>();
        //transform.parent.transform.position = transform.position;
        anim.SetFloat("velocityX", inputDir.x);
        anim.SetFloat("velocityY", inputDir.y);
        anim.SetBool("hasInput", inputDir != Vector2.zero);
    }

    private void FixedUpdate()
    {

        rb.velocity = new Vector2(inputDir.x * runSpeed, inputDir.y * runSpeed);
        //var temp = Vector2.Scale(inputDir, speedCap);
        //var sum = temp + rb.velocity;
        //rb.velocity = new Vector2(clampMaxSpeed(sum.x), clampMaxSpeed(sum.y));
    }

    private float clampMaxSpeed(float value)
    {
        return Mathf.Clamp(value, -runSpeed, runSpeed);
    }
}
