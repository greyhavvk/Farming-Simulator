using System;

namespace Core.UIManager.Inventory
{
    public class DropItemContainerPanel : InteractablePanel
    {
        private Action _onDropItemContainerPanelClick;
        
        public void Initialize(Action onDropItemContainerPanelClick)
        {
            _onDropItemContainerPanelClick = onDropItemContainerPanelClick;
        }

        protected override void OnClick(ActionType actionType)
        {
            switch (actionType)
            {
                case ActionType.Drop or ActionType.PointerUp:
                    _onDropItemContainerPanelClick?.Invoke();
                    break;
            }
        }

        public void DisableListeners()
        {
            _onDropItemContainerPanelClick = null;
        }
    }
}
