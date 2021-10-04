using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TitleScreen : MonoBehaviour
{
    public Transform subtitle;

    void Start()
    {
        Jump();
        ChangeSize();
    }

    public void Jump()
    {
        transform.LeanMoveLocalY(150, 0.4f).setEaseOutQuart().setLoopPingPong();
    }

    public void ChangeSize()
    {
        subtitle.LeanScale(new Vector2(0.95f, 1.3f), 0.9f).setLoopPingPong();
    }
}
