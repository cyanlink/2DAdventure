using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerModeSwitcher : MonoBehaviour
{
    [Header("ÅäÖÃ")]

    [SerializeField]
    private GameObject sideScrollPlayerGO;
    [SerializeField]
    private GameObject topDownPlayerGO;
    [SerializeField]
    private SceneLoader sceneLoader;

    private PlayerModes mPlayerMode;
    public GameObject CurrentPlayerGO { get; private set; }

    private void Awake()
    {
        CurrentPlayerGO = topDownPlayerGO;
    }

    private void Start()
    {
        CurrentPlayerGO = sceneLoader.firstLoadScene.sceneType == SceneType.SafeHouse ? topDownPlayerGO : sideScrollPlayerGO;
    }
    public PlayerModes playerMode
    {
        get => mPlayerMode;
        set
        {
            switch (value)
            {
                case PlayerModes.TopDown:
                    CurrentPlayerGO = topDownPlayerGO;
                    break;

                case PlayerModes.SideScroll:
                    CurrentPlayerGO = sideScrollPlayerGO;
                    break;
                default:
                    throw new NotImplementedException();
            }
        }
    }
}

public enum PlayerModes
{
    SideScroll, TopDown
}