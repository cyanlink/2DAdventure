using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BombIndicator : MonoBehaviour
{
    private SpriteRenderer bombIcon;
    // Start is called before the first frame update

    private void Awake()
    {
        bombIcon = GetComponent<SpriteRenderer>();
    }
    private void Start()
    {
        bombIcon.enabled = false;
    }
    public void ToggleBombIcon(bool enable)
    {
        bombIcon.enabled = enable;
    }
}
