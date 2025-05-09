using System.Collections;
using UnityEngine;

public class IdleBehaviour : StateMachineBehaviour
{
    float timer;

    // При входе в Idle сбрасываем таймер
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        timer = 0f;

        // Останавливаем NavMeshAgent
        var agent = animator.GetComponent<UnityEngine.AI.NavMeshAgent>();
        if (agent != null)
        {
            agent.isStopped = true;
        }
    }

    // Таймер считает 5 секунд, затем переключает в Walk
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        timer += Time.deltaTime;
        if (timer >= 5f)
        {
            animator.SetBool("isWalking", true);
        }
    }
}
