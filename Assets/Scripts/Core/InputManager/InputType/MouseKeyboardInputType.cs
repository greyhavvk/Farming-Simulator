﻿using UnityEngine;

namespace Core.InputManager.InputType
{
    [CreateAssetMenu(fileName = "Mouse Keyboard Input Type", menuName = "Input System/Mouse Keyboard Input Type")]
    public class MouseKeyboardInputType : InputType
    {
        public override float GetHorizontalInput()
        {
            return Input.GetAxis("Horizontal");
        }

        public override float GetVerticalInput()
        {
            return Input.GetAxis("Vertical");
        }

        public override bool IsInteractButtonPressed()
        {
            return Input.GetMouseButton(0);
        }
        
        public override bool IsInteractButtonPressedUp()
        {
            return Input.GetMouseButtonUp(0);
        }

        public override bool GetInventoryUITriggerInput()
        {
            return Input.GetKeyDown(KeyCode.Tab);
        }

        public override bool IsInteractButtonPressedDown()
        {
            return Input.GetMouseButtonDown(0);
        }

        public override bool GetSettingUITriggerInput()
        {
            return Input.GetKeyDown(KeyCode.Escape);
        }

        public override float GetMouseXInput()
        {
            return Input.GetAxis("Mouse X");
        }

        public override float GetMouseYInput()
        {
            return Input.GetAxis("Mouse Y");
        }

        public override bool IsPlacementConfirmed()
        {
            return Input.GetMouseButtonDown(0);
        }

        public override Vector3 ConfirmedPosition()
        {
            return Input.mousePosition;
        }

        public override bool RotateButtonDown()
        {
            return Input.GetMouseButtonDown(1);
        }

        public override float GetScrollDelta()
        {
            return Input.mouseScrollDelta.y;
        }
    }
}