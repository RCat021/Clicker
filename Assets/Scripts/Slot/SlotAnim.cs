using UnityEngine;

public class SlotAnim : MonoBehaviour
{
    [SerializeField] private Animator _animator;
    private const string ANIMATION_PARAMETER = "int";

    private void Start()
    {
        _animator = GetComponent<Animator>();
    }

    public void StartSpinAnim()
    {
        _animator.SetInteger(ANIMATION_PARAMETER, 1);

    }

    public void StopSpinAnim()
    {

        _animator.SetInteger(ANIMATION_PARAMETER, 2);
    }
}