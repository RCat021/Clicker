using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class SlotReelUI : MonoBehaviour
{
    [SerializeField] private float maxSpeed = 1500f; // Максимальная скорость
    [SerializeField] private float acceleration = 3000f; // Ускорение при старте
    [SerializeField] private float deceleration = 2000f; // Замедление при остановке
    [SerializeField] private float minSpeed = 100f; // Минимальная скорость перед остановкой
    [SerializeField] private Sprite[] fruitSprites;
    [SerializeField] private Image[] fruitSlots;
    [SerializeField] private float spacing = 50f;

    private bool isSpinning;
    private bool isStopping;
    private float currentSpeed;
    private float totalElementHeight;
    private SpinState currentState = SpinState.Stopped;

    private enum SpinState
    {
        Stopped,
        Accelerating,
        Cruising,
        Decelerating
    }

    private void Start()
    {
        if (fruitSlots.Length > 0)
        {
            totalElementHeight = fruitSlots[0].rectTransform.rect.height + spacing;
        }

        // Инициализация позиций
        for (int i = 0; i < fruitSlots.Length; i++)
        {
            float yPos = i * totalElementHeight;
            fruitSlots[i].rectTransform.localPosition = new Vector3(
                fruitSlots[i].rectTransform.localPosition.x,
                yPos,
                fruitSlots[i].rectTransform.localPosition.z
            );

            fruitSlots[i].sprite = fruitSprites[Random.Range(0, fruitSprites.Length)];
        }
    }

    void Update()
    {
        HandleSpinState();
        MoveElements();
    }

    private void HandleSpinState()
    {
        switch (currentState)
        {
            case SpinState.Stopped:
                // Ничего не делаем
                break;

            case SpinState.Accelerating:
                currentSpeed += acceleration * Time.deltaTime;
                if (currentSpeed >= maxSpeed)
                {
                    currentSpeed = maxSpeed;
                    currentState = SpinState.Cruising;
                }
                break;

            case SpinState.Cruising:
                // Поддерживаем постоянную скорость
                if (isStopping)
                {
                    currentState = SpinState.Decelerating;
                }
                break;

            case SpinState.Decelerating:
                currentSpeed -= deceleration * Time.deltaTime;
                if (currentSpeed <= minSpeed)
                {
                    currentSpeed = 0f;
                    currentState = SpinState.Stopped;
                    isSpinning = false;
                    isStopping = false;
                    OnReelStopped();
                }
                break;
        }
    }

    private void MoveElements()
    {
        if (currentSpeed <= 0) return;

        for (int i = 0; i < fruitSlots.Length; i++)
        {
            var rt = fruitSlots[i].rectTransform;
            rt.localPosition += Vector3.down * currentSpeed * Time.deltaTime;

            // Если элемент ушел за пределы видимости
            if (rt.localPosition.y < -totalElementHeight * 1.5f)
            {
                // Находим самый верхний элемент
                float maxY = fruitSlots.Max(slot => slot.rectTransform.localPosition.y);

                // Перемещаем текущий элемент выше всех
                rt.localPosition = new Vector3(
                    rt.localPosition.x,
                    maxY + totalElementHeight,
                    rt.localPosition.z
                );

                // Меняем спрайт только если еще не начали останавливаться
                if (!isStopping)
                {
                    fruitSlots[i].sprite = fruitSprites[Random.Range(0, fruitSprites.Length)];
                }
            }
        }
    }

    private void OnReelStopped()
    {
        // Выравниваем элементы после остановки (опционально)
        AlignElements();
        Debug.Log("Reel stopped completely");
    }

    private void AlignElements()
    {
        // Выравниваем элементы по сетке после остановки
        for (int i = 0; i < fruitSlots.Length; i++)
        {
            float targetY = i * totalElementHeight;
            fruitSlots[i].rectTransform.localPosition = new Vector3(
                fruitSlots[i].rectTransform.localPosition.x,
                targetY,
                fruitSlots[i].rectTransform.localPosition.z
            );
        }
    }

    public void StartSpin()
    {
        if (currentState != SpinState.Stopped) return;

        isSpinning = true;
        isStopping = false;
        currentState = SpinState.Accelerating;
        currentSpeed = 0f;
    }

    public void StopSpin()
    {
        if (!isSpinning || isStopping) return;

        isStopping = true;
    }

    // Метод для принудительной остановки
    public void ForceStop()
    {
        currentSpeed = 0f;
        currentState = SpinState.Stopped;
        isSpinning = false;
        isStopping = false;
        OnReelStopped();
    }

    // Свойства для получения текущего состояния
    public bool IsSpinning => isSpinning;
    public bool IsStopping => isStopping;
    public float CurrentSpeed => currentSpeed;
}