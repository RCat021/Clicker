using TMPro;
using UnityEngine;

public class CoinsUI : MonoBehaviour
{
    [SerializeField] private TMP_Text coinsText;

    private void OnEnable()
    {
        // Подписываемся на событие
        CoinManager.OnCoinsChanged += UpdateUI;

        // Сразу обновляем UI при включении
        UpdateUI(CoinManager.Coins);
    }

    private void OnDisable()
    {
        // Отписываемся, чтобы не было утечек
        CoinManager.OnCoinsChanged -= UpdateUI;
    }

    private void UpdateUI(int coins)
    {
        coinsText.text = "" + coins;
    }
}
