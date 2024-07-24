namespace Core.InputManager
{
    public interface IPlayerInput
    {
        float GetHorizontalInput();
        float GetVerticalInput();
        bool IsInteractButtonPressedUp();
        bool IsInteractButtonPressed();
        float GetMouseXInput();
        float GetMouseYInput();
        float GetScrollDelta();
    }
}