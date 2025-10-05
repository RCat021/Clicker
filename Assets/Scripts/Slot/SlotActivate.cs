using System.Collections;
using UnityEngine;

public class SlotActivate : MonoBehaviour
{
   [SerializeField] private SlotReelUI[] _slotActivate;
    [SerializeField] private SlotAnim _slotAnim;
    private float _timeToDiactivate = 1.5f;
    private float _timerActiveSlot = 5f;

    public void ActivateSlot()
    {
        _slotAnim.StartSpinAnim();

        foreach (var slot in _slotActivate)
        {
            slot.StartSpin();
           
        }

        StartCoroutine(SmoothlyDisactivateSlot());
    }

    private void DisactivateSlot(SlotReelUI slot)
    {
        slot.StopSpin();
        _slotAnim.StopSpinAnim();
    }

    IEnumerator SmoothlyDisactivateSlot()
    {
        yield return new WaitForSeconds(_timerActiveSlot);

        foreach (var slot in _slotActivate)
        {
            DisactivateSlot(slot);
            yield return new WaitForSeconds(_timeToDiactivate);
        }
    }
}
