using System;

namespace Systems.FinanceSystem
{
    public class FinanceModel
    {
        private int _currentMoney;
        public int CurrentMoney => _currentMoney;

        private Action<int> _onMoneyChanged;

        public void Initialize(int initialMoney,  Action<int> onMoneyChanged)
        {
            _onMoneyChanged = onMoneyChanged;
            _currentMoney = initialMoney;
            NotifyMoneyChanged();
        }

        public void OnDestroy()
        {
            _onMoneyChanged = null;
        }

        public void AddMoney(int amount)
        {
            _currentMoney += amount;
            NotifyMoneyChanged();
        }

        public void SubtractMoney(int amount)
        {
            if (_currentMoney >= amount)
            {
                _currentMoney -= amount;
                NotifyMoneyChanged();
            }
            else
            {
                Console.WriteLine("Not enough money!");
            }
        }

        private void NotifyMoneyChanged()
        {
            _onMoneyChanged?.Invoke(_currentMoney);
        }
    }
}