using UnityEngine;

public class AnimationController : MonoBehaviour
{
    [SerializeField] private Animator[] _animator;

    public void SetAnimationState(string anima = "", int value = 0)
    {
        if (_animator.Length <= 0)
            return;
        foreach (var item in _animator)
        {
            item.SetInteger(anima, value);
        }
    }
}
