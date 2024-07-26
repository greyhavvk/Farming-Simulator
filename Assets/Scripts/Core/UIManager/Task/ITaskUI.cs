using Systems.FarmingSystems;

namespace Core.UIManager
{
    public interface ITaskUI
    {
        void UpdateTaskUI(PlantType plantType, string taskMessage);
        void UpdateMoveTaskUI();
        void ClearTaskUI();
    }
}