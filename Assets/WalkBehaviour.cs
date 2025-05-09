using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class WalkBehaviour : StateMachineBehaviour
{
    float timer;
    List<Transform> points = new List<Transform>();
    NavMeshAgent agent;

    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        timer = 0f;

        agent = animator.GetComponent<NavMeshAgent>();
        if (agent == null) return;

        agent.isStopped = false;

        GameObject pointsObject = GameObject.FindWithTag("Points");
        if (pointsObject != null)
        {
            points.Clear();
            foreach (Transform t in pointsObject.transform)
            {
                points.Add(t);
            }
        }

        // Убедимся, что есть хотя бы одна точка
        if (points.Count > 0)
        {
            agent.SetDestination(points[Random.Range(0, points.Count)].position);
        }
        else
        {
            Debug.LogWarning("WalkBehaviour: Не найдены точки с тегом 'Points'.");
        }
    }

    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (agent == null || points.Count == 0) return;

        timer += Time.deltaTime;

        if (agent.remainingDistance <= agent.stoppingDistance)
        {
            agent.SetDestination(points[Random.Range(0, points.Count)].position);
        }

        if (timer >= 10f)
        {
            animator.SetBool("isWalking", false);
        }
    }

    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (agent != null)
        {
            agent.isStopped = true;
        }
    }
}
