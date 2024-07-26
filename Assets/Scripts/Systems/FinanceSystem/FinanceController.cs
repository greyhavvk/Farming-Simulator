using System;
using Core.UIManager.Finance;
using UnityEngine;

namespace Systems.FinanceSystem
{
    public class FinanceController : MonoBehaviour, IFinance
    {
        [SerializeField] private int initialMoney;
        private FinanceModel _financeModel;
        private IFinanceUI _financeUI;
        private Action _moneyValueChanged;

        public void Initialize(IFinanceUI financeUIPanel, Action moneyValueChanged)
        {
            _moneyValueChanged = moneyValueChanged;
            _financeUI = financeUIPanel;
            _financeModel = new FinanceModel();
            _financeModel.Initialize(initialMoney,HandleMoneyChanged);
        }

        public void DisableListeners()
        {
            _moneyValueChanged = null;
            _financeModel?.DisableListeners();
        }

        private void HandleMoneyChanged(int currentMoney)
        {
            _financeUI.UpdateFinancePanelUI(currentMoney);
            _moneyValueChanged?.Invoke();
        }

        public void AddMoney(int amount)
        {
            _financeModel.AddMoney(amount);
        }

        public void SubtractMoney(int amount)
        {
            _financeModel.SubtractMoney(amount);
        }

        public int GetCurrentMoney()
        {
            return _financeModel.CurrentMoney;
        }
    }
}