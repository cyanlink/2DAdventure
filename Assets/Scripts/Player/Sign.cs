using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.DualShock;
using UnityEngine.InputSystem.Switch;

public class Sign : MonoBehaviour
{
    private Animator anim;
    public GameObject signSprite;

    private SpriteRenderer signRenderer;
    private bool canPress;

    public PlayerModeSwitcher playerModeSwitcher;
    private Transform playerSpriteTransform => playerModeSwitcher.CurrentPlayerGO.transform;

    private PlayerInputControl playerInput;
    private IInteractable targetItem;
    public InputRouter inputRouter;

    private void Awake()
    {
        anim = signSprite.GetComponent<Animator>();
        signRenderer = signSprite.GetComponent<SpriteRenderer>();
        playerInput = inputRouter.InputControls;
        playerInput.Gameplay.Confirm.started += OnConfirm;
    }

    private void OnEnable()
    {
        InputSystem.onActionChange += OnActionChange;
    }
    private void OnDisable()
    {
        canPress = false;
        InputSystem.onActionChange -= OnActionChange;
    }

    private void Start()
    {
        
    }

    private void Update()
    {
        signRenderer.enabled = canPress;
        signSprite.transform.localScale = playerSpriteTransform.localScale;
    }

    private void OnConfirm(InputAction.CallbackContext context)
    {
        if (canPress)
        {
            targetItem.TriggerAction();
            GetComponent<AudioDefination>()?.PlayAudioClip();
        }
    }

    /// <summary>
    /// 设备输入同时切换动画
    /// </summary>
    /// <param name="obj"></param>
    /// <param name="actionChange"></param>
    private void OnActionChange(object obj, InputActionChange actionChange)
    {
        if (actionChange == InputActionChange.ActionStarted)
        {
            // Debug.Log(((InputAction)obj).activeControl.device);
            var d = ((InputAction)obj).activeControl.device;
            switch (d.device)
            {
                case Keyboard:
                    anim.Play("keyboard");
                    break;
                case SwitchProControllerHID:
                    anim.Play("switch");
                    break;
                case DualShockGamepad:
                    anim.Play("ps4");
                    break;
            }
        }

    }



    private void OnTriggerStay2D(Collider2D other)
    {
        //// TODO: 根据输入设备播放不同动画
        if (other.CompareTag("Interactable"))
        {
            canPress = true;
            targetItem = other.GetComponent<IInteractable>();
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        canPress = false;
    }
}
