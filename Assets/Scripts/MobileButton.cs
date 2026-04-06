using UnityEngine;
using UnityEngine.EventSystems;

public class MobileButton : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    public enum ButtonType
    {
        Left,
        Right,
        Jump,
        Attack,
        Dash,
        Pulse
    }

    public ButtonType buttonType;

    public void OnPointerDown(PointerEventData eventData)
    {
        if (MobileControls.instance == null) return;

        switch (buttonType)
        {
            case ButtonType.Left:
                MobileControls.instance.SetMoveLeft(true);
                break;

            case ButtonType.Right:
                MobileControls.instance.SetMoveRight(true);
                break;

            case ButtonType.Jump:
                MobileControls.instance.PressJump();
                break;

            case ButtonType.Attack:
                MobileControls.instance.PressAttack();
                break;

            case ButtonType.Dash:
                MobileControls.instance.PressDash();
                break;

            case ButtonType.Pulse:
                MobileControls.instance.PressPulse();
                break;
        }
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        if (MobileControls.instance == null) return;

        switch (buttonType)
        {
            case ButtonType.Left:
                MobileControls.instance.SetMoveLeft(false);
                break;

            case ButtonType.Right:
                MobileControls.instance.SetMoveRight(false);
                break;
        }
    }
}