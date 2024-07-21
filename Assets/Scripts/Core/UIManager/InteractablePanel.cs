using UnityEngine;
using UnityEngine.EventSystems;

namespace Core.UIManager
{
    public abstract class InteractablePanel : MonoBehaviour, IPointerEnterHandler, IPointerDownHandler,
        IPointerUpHandler, IDragHandler, IDropHandler
    {
        private bool _click;

        public void OnPointerEnter(PointerEventData eventData)
        {
            eventData.pointerPress = gameObject;
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            OnClick(ActionType.PointerDown);
            _click = true;
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            if (!_click) return;

            OnClick(ActionType.PointerUp);
            _click = false;
        }

        public void OnDrop(PointerEventData eventData)
        {
            OnClick(ActionType.Drop);
            _click = false;
        }

        public void OnDrag(PointerEventData eventData)
        {
            if (!_click) return;

            OnClick(ActionType.Drag);
            _click = false;
        }

        protected abstract void OnClick(ActionType actionType);

        protected enum ActionType
        {
            Drag,
            Drop,
            PointerUp,
            PointerDown,
            PointerEnter,
            PointerExit,
        }
    }
}
