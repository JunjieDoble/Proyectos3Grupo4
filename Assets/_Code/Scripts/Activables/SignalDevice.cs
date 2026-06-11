using UnityEngine;

[RequireComponent(typeof(Animator))]
public class SignalDevice : MonoBehaviour
{
    private Animator _animator;

    private void Start()
    {
        _animator = GetComponent<Animator>();
    }

    public void Activate()
    {
        _animator.Play("send_signal", 0, 0f);
    }
}
