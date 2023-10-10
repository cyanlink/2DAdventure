using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Antlr3.Runtime.Tree;
using UnityEngine;
using UnityEngine.InputSystem;

public partial class PlayerController : MonoBehaviour, ITakeDamage
{
    [Header("监听事件")]
    public SceneLoadEventSO sceneLoadEvent;
    public VoidEventSO afterSceneLoadedEvent;
    public VoidEventSO loadDataEvent;
    public VoidEventSO backToMenuEvent;

    [Header("组件")]
    public InputRouter inputRouter;
    private PlayerInputControl inputControl;
    public Vector2 inputDirection;
    public PhysicsCheck physicsCheck;
    public SpriteRenderer spriteRenderer;
    public CapsuleCollider2D coll;
    private Rigidbody2D rb;
    public PlayerAnimation playAnimation;
    private Character character;


    [Header("基本参数")]
    public float speed;
    private float walkSpeed => speed / 2f;
    private float runSpeed;
    public float jumpForce; //跳跃力，跳跃瞬间施加的力
    public float wallJumpForce;
    public float hurtForce;
    public int slidePowerCost;

    public float slideDistance;
    public float slideSpeed;
    private Vector2 originalOffset;
    private Vector2 originalSize;
    [Header("状态")]
    public bool isCrouch;
    public bool isHurt;
    public bool isDead;
    public bool isAttack;
    public bool isMyCode;
    public bool wallJump;
    public bool isSlide;

    //-1或者1
    public bool flipX;
    [Header("物理材质")]
    public PhysicsMaterial2D wall;
    public PhysicsMaterial2D normal;


    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        inputControl = InputRouter.Instance.InputControls;
        physicsCheck = GetComponent<PhysicsCheck>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        coll = GetComponent<CapsuleCollider2D>();
        playAnimation = GetComponent<PlayerAnimation>();
        character = GlobalState.PlayerCharacter;

        originalOffset = coll.offset;
        originalSize = coll.size;

    }

    private void OnEnable()
    {
        RegisterInputActions();
   
        sceneLoadEvent.LoadRequestEvent += OnLoadEvent;
        afterSceneLoadedEvent.OnEventRaised += OnAfterLoadedEvent;
        loadDataEvent.OnEventRaised += OnLoadDataEvent;
        backToMenuEvent.OnEventRaised += OnLoadDataEvent;
    }

    private void OnDisable()
    {
        inputControl.Disable();
        sceneLoadEvent.LoadRequestEvent -= OnLoadEvent;
        afterSceneLoadedEvent.OnEventRaised -= OnAfterLoadedEvent;
        loadDataEvent.OnEventRaised -= OnLoadDataEvent;
        backToMenuEvent.OnEventRaised -= OnLoadDataEvent;

    }

    private void Update()
    {
        inputDirection = inputControl.Gameplay.Move.ReadValue<Vector2>();

    }

    private void FixedUpdate()
    {
        CheckState();

        if (!isHurt && !isAttack)
            Move();
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.CompareTag("Water"))
        {
            character.InstantKill();
        }
    }

    private void RegisterInputActions()
    {
        inputControl.Gameplay.Jump.performed += Jump;

        #region 强制走路
        runSpeed = speed;
        inputControl.Gameplay.MoveButton.performed += ctx =>
        {
            if (physicsCheck.isGround)
                speed = walkSpeed;
        };
        inputControl.Gameplay.MoveButton.canceled += ctx =>
        {
            if (physicsCheck.isGround)
                speed = runSpeed;
        };
        #endregion
        //攻击
        inputControl.Gameplay.Attack.started += PlayerAttack;
        //滑铲
        inputControl.Gameplay.Slide.started += Slide;

        //炸弹
        inputControl.Gameplay.Bomb.performed += BombAction;
        //弓箭
        inputControl.Gameplay.Arrow.started += ArrowAction;

        inputControl.Enable();
    }
    private void OnLoadEvent(GameSceneSO arg0, Vector3 arg1, bool arg2)
    {
        inputControl.Gameplay.Disable();
    }

    private void OnAfterLoadedEvent()
    {
        inputControl.Gameplay.Enable();
    }
    private void OnLoadDataEvent()
    {
        isDead = false;
    }

    public void Move()
    {
        //人物移动
        if (isSlide)
            return;
        if (!isCrouch && !wallJump)
            rb.velocity = new Vector2(Mathf.Clamp(rb.velocity.x + inputDirection.x * speed, -speed, speed ), rb.velocity.y);

        //人物翻转
        //方法1 改变transform.scale   
        //faceDir = (int)FaceDir();
        //if (inputDirection.x > 0)
        //    faceDir = 1;
        //if (inputDirection.x < 0)
        //    faceDir = -1;
        //transform.localScale = new Vector3(faceDir, 1, 1);

        //方法2 改变SpriteRenderer.flipX
        flipX = spriteRenderer.flipX;
        if (inputDirection.x > 0)
            flipX = false;
        if (inputDirection.x < 0)
            flipX = true;
        spriteRenderer.flipX = flipX;

        //蹲下
        isCrouch = inputDirection.y < -0.5f && physicsCheck.isGround;
        if (isCrouch)
        {
            //修改碰撞体积
            coll.offset = new Vector2(-0.05f, 0.85f);
            coll.size = new Vector2(0.7f, 1.7f);
        }
        else
        {
            //还原碰撞体积
            coll.offset = originalOffset;
            coll.size = originalSize;
        }
    }

    private void Jump(InputAction.CallbackContext ctx)
    {
        if (physicsCheck.isGround)
        {
            rb.AddForce(transform.up * jumpForce, ForceMode2D.Impulse);
            isSlide = false;
            StopAllCoroutines();
            GetComponent<AudioDefination>()?.PlayAudioClip();
        }
        else if (physicsCheck.onWall)
        //else if (physicsCheck.touchLeftWall || physicsCheck.touchRightWall)
        {
            rb.AddForce(new Vector2(-inputDirection.x, 2f) * wallJumpForce, ForceMode2D.Impulse);
            wallJump = true;
            GetComponent<AudioDefination>()?.PlayAudioClip();
        }
    }

    private void Slide(InputAction.CallbackContext context)
    {
        if (!isSlide && physicsCheck.isGround && character.currentPower >= slidePowerCost && !isHoldingBomb &&!isHoldingArrow)
        {
            isSlide = true;


            var targetPos = new Vector3(transform.position.x + slideDistance * FaceDir(), transform.position.y);

            gameObject.layer = LayerMask.NameToLayer("Enemy");
            StartCoroutine(TriggerSlide(targetPos));

            character.OnSlide(slidePowerCost);
        }

    }

    private IEnumerator TriggerSlide(Vector3 target)
    {
        do
        {
            yield return null;
            //掉落悬崖
            if (!physicsCheck.isGround)
            {
                break;
            }

            if (physicsCheck.touchLeftWall && FaceDir() < 0f || physicsCheck.touchRightWall && FaceDir() > 0f)
            {
                break;
            }
            if (physicsCheck.touchLeftWall || physicsCheck.touchRightWall)
            {
                isSlide = false;
                break;
            }

            rb.MovePosition(new Vector2(transform.position.x + FaceDir() * slideSpeed, transform.position.y));
        } while (Mathf.Abs(target.x - transform.position.x) > 0.1f);

        isSlide = false;
        gameObject.layer = LayerMask.NameToLayer("Player");
    }

    private int FaceDir()
    {
        return flipX ? -1 : 1;
    }

    private void PlayerAttack(InputAction.CallbackContext obj)
    {
        if (!isHoldingArrow && !isHoldingBomb)
        {
            isAttack = true;
            playAnimation.PlayAttack();
        }
    }

    public void GetHurt(Transform attacker)
    {
        isHurt = true;
        rb.velocity = Vector2.zero;
        Vector2 dir =
            new Vector2((transform.position.x - attacker.transform.position.x), 0).normalized;
        rb.AddForce(dir * hurtForce, ForceMode2D.Impulse);
    }

    public void PlayerDead()
    {
        isDead = true;
        inputControl.Gameplay.Disable();
    }

    private void CheckState()
    {
        coll.sharedMaterial = physicsCheck.isGround ? normal : wall;
        if (physicsCheck.onWall)
        {
            if (!isMyCode)
                rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y / 2);
            else
                rb.velocity = new Vector2(rb.velocity.x, -1);
        }
        else
        {
            rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y);
        }
        if (wallJump && rb.velocity.y < 0f)
        {
            wallJump = false;
        }
    }

    public void TakeDamage(IAttack attacker)
    {
        character.TakeDamage(attacker);
    }
}
