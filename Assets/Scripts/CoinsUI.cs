using TMPro;
using UnityEngine;

public class CoinsUI : MonoBehaviour
{
    [SerializeField] private TMP_Text coinsText;

    private void OnEnable()
    {
        // ������������� �� �������
        CoinManager.OnCoinsChanged += UpdateUI;

        // ����� ��������� UI ��� ���������
        UpdateUI(CoinManager.Coins);
    }

    private void OnDisable()
    {
        // ������������, ����� �� ���� ������
        CoinManager.OnCoinsChanged -= UpdateUI;
    }

    private void UpdateUI(int coins)
    {
        coinsText.text = "" + coins;
    }
}
