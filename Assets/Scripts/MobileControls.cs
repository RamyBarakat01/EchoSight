using UnityEngine;

public class MobileControls : MonoBehaviour
{
    public static MobileControls instance;

    [HideInInspector] public bool moveLeft;
    [HideInInspector] public bool moveRight;
    [HideInInspector] public bool jumpPressed;
    [HideInInspector] public bool attackPressed;
    [HideInInspector] public bool dashPressed;
    [HideInInspector] public bool pulsePressed;

    void Awake()
    {
        instance = this;
    }

    public void SetMoveLeft(bool value)
    {
        moveLeft = value;
    }

    public void SetMoveRight(bool value)
    {
        moveRight = value;
    }

    public void PressJump()
    {
        jumpPressed = true;
    }

    public void PressAttack()
    {
        attackPressed = true;
    }

    public void PressDash()
    {
        dashPressed = true;
    }

    public void PressPulse()
    {
        pulsePressed = true;
    }

    public void ResetActionButtons()
    {
        jumpPressed = false;
        attackPressed = false;
        dashPressed = false;
    }

    public void ResetPulseButton()
    {
        pulsePressed = false;
    }
}