using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControllerTopDown : MonoBehaviour
{
    public float runSpeed;

    private PlayerInputControl inputControl;

    private Rigidbody2D rb;
    private CapsuleCollider2D capsuleCollider;

    private Vector2 inputDir;

    private void Awake()
    {
        inputControl = InputRouter.Instance.InputControls;
        rb = GetComponent<Rigidbody2D>();

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
    }

    private void FixedUpdate()
    {
        rb.velocity = new Vector2(inputDir.x * runSpeed  , inputDir.y * runSpeed);
    }
}
