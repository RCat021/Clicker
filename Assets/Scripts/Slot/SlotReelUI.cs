using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class SlotReelUI : MonoBehaviour
{
    [SerializeField] private float maxSpeed = 1500f; // ������������ ��������
    [SerializeField] private float acceleration = 3000f; // ��������� ��� ������
    [SerializeField] private float deceleration = 2000f; // ���������� ��� ���������
    [SerializeField] private float minSpeed = 100f; // ����������� �������� ����� ����������
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

        // ������������� �������
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
                // ������ �� ������
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
                // ������������ ���������� ��������
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

            // ���� ������� ���� �� ������� ���������
            if (rt.localPosition.y < -totalElementHeight * 1.5f)
            {
                // ������� ����� ������� �������
                float maxY = fruitSlots.Max(slot => slot.rectTransform.localPosition.y);

                // ���������� ������� ������� ���� ����
                rt.localPosition = new Vector3(
                    rt.localPosition.x,
                    maxY + totalElementHeight,
                    rt.localPosition.z
                );

                // ������ ������ ������ ���� ��� �� ������ ���������������
                if (!isStopping)
                {
                    fruitSlots[i].sprite = fruitSprites[Random.Range(0, fruitSprites.Length)];
                }
            }
        }
    }

    private void OnReelStopped()
    {
        // ����������� �������� ����� ��������� (�����������)
        AlignElements();
        Debug.Log("Reel stopped completely");
    }

    private void AlignElements()
    {
        // ����������� �������� �� ����� ����� ���������
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

    // ����� ��� �������������� ���������
    public void ForceStop()
    {
        currentSpeed = 0f;
        currentState = SpinState.Stopped;
        isSpinning = false;
        isStopping = false;
        OnReelStopped();
    }

    // �������� ��� ��������� �������� ���������
    public bool IsSpinning => isSpinning;
    public bool IsStopping => isStopping;
    public float CurrentSpeed => currentSpeed;
}