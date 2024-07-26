using TMPro;
using UnityEngine;

namespace Core.UIManager.Finance
{
    public class FinanceUIPanel : MonoBehaviour
    {
        [SerializeField] private TMP_Text moneyText;
        [SerializeField] private GameObject panel;

        public void UpdateFinancePanelUI(int value)
        {
            moneyText.text = value.ToString();
        }

        public void SetActivePanel(bool active)
        {
            panel.SetActive(active);
        }
    }
}