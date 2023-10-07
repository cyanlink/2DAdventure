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

    private GameObject mCurrentGO;
    public GameObject CurrentPlayerGO { get=> GetCurrentPlayerGO() ; private set { mCurrentGO = value; } }
    
    public static PlayerModeSwitcher Instance { get; private set; }

    private void Awake()
    {
        CurrentPlayerGO = topDownPlayerGO;
        topDownPlayerGO.SetActive(false);
        sideScrollPlayerGO.SetActive(false);
        Instance = this;
        
    }

    private GameObject GetCurrentPlayerGO()
    {
        return CurrentPlayerGO = sceneLoader.currentLoadScene.sceneType == SceneType.SafeHouse ? topDownPlayerGO : sideScrollPlayerGO;
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
                    topDownPlayerGO.SetActive(true);
                    sideScrollPlayerGO.SetActive(false);
                    break;

                case PlayerModes.SideScroll:
                    CurrentPlayerGO = sideScrollPlayerGO;
                    topDownPlayerGO.SetActive(false);
                    sideScrollPlayerGO.SetActive(true);
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