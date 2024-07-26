namespace Systems.FinanceSystem
{
    public interface IFinance
    {
        void AddMoney(int amount);
        void SubtractMoney(int amount);
        int GetCurrentMoney();
    }
}