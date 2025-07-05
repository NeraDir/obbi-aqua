using System.Collections;
using UnityEngine;

public class PenComponent : MonoBehaviour
{
    private AnimationController m_AnimationController;

    private IEnumerator Start()
    {
        m_AnimationController = GetComponent<AnimationController>();
        while (true)
        {
            yield return new WaitForSeconds(1);
            m_AnimationController.SetAnimationState("penState", Random.Range(1,10));
            yield return new WaitForSeconds(3);
            m_AnimationController.SetAnimationState("penState", 0);
        }
    }
}
