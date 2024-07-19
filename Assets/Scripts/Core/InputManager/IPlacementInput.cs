using UnityEngine;

namespace Core.InputManager
{
    public interface IPlacementInput
    {
        bool PlacementConfirmedButtonDown();
        Vector3 ConfirmedPosition();
        bool RotateButtonDown();
    }
}