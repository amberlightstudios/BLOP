using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TitleScreen : MonoBehaviour
{
    void Start()
    {
        Jump();
    }

    public void Jump()
    {
        transform.LeanMoveLocalY(150, 0.4f).setEaseOutQuart().setLoopPingPong();
    }
}
