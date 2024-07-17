namespace Core.InputManager
{
    public interface IInputManager
    {
        float GetHorizontalInput();
        float GetVerticalInput();
        bool IsInteractButtonPressed();
        float GetMouseXInput();
        float GetMouseYInput();
    }
}