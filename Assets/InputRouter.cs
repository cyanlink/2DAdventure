using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputRouter : MonoBehaviour
{
    private static InputRouter mInstance;
    public static InputRouter Instance
    {
        get
        {
            if (mInstance == null)
            {
                mInstance = GameObject.FindAnyObjectByType<InputRouter>();
            }
            return mInstance;
        }
    }

    private PlayerInputControl m_Controls;
    public PlayerInputControl InputControls
    {
        get
        {
            if (m_Controls == null)
                m_Controls = new PlayerInputControl();
            return m_Controls;
        }
    }

}
