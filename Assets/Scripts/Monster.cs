using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class Monster : MonoBehaviour
{
    enum State { Patrol, Chase, Return }

    public Transform[] waypoints;
    public float patrolSpeed = 1.8f;
    public float chaseSpeed = 4.5f;
    public float detectRange = 10f;
    public float catchDistance = 1.5f;
    public float maxChaseTime = 8f;
    public Animator animator;

    NavMeshAgent agent;
    Transform player;
    State state = State.Patrol;
    int wpIndex;
    float chaseTimer;
    bool catching;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        player = GameObject.FindGameObjectWithTag("Player").transform;
        if (!animator) animator = GetComponentInChildren<Animator>();
        GoToNextWaypoint();
    }

    void Update()
    {
        if (animator) animator.SetFloat("Speed", agent.velocity.magnitude);
        if (catching) return;

        float dist = Vector3.Distance(transform.position, player.position);

        switch (state)
        {
            case State.Patrol:
                agent.speed = patrolSpeed;
                if (!agent.pathPending && agent.remainingDistance < 0.5f) GoToNextWaypoint();
                if (dist < detectRange && CanSeePlayer()) StartChase();
                break;

            case State.Chase:
                agent.speed = chaseSpeed;
                agent.SetDestination(player.position);
                chaseTimer -= Time.deltaTime;
                if (dist < catchDistance) Catch();
                else if (chaseTimer <= 0f || dist > detectRange * 1.6f) state = State.Return;
                break;

            case State.Return:
                agent.speed = patrolSpeed;
                if (waypoints.Length > 0) agent.SetDestination(waypoints[wpIndex].position);
                if (!agent.pathPending && agent.remainingDistance < 0.5f) state = State.Patrol;
                if (dist < detectRange && CanSeePlayer()) StartChase();
                break;
        }
    }

    bool CanSeePlayer()
    {
        Vector3 origin = transform.position + Vector3.up * 1.2f;
        Vector3 target = player.position + Vector3.up * 1f;
        Vector3 dir = target - origin;

        if (Physics.Raycast(origin, dir, out RaycastHit hit, detectRange, ~0, QueryTriggerInteraction.Ignore))
            return hit.transform == player || hit.transform.IsChildOf(player);
        return false;
    }

    void StartChase()
    {
        state = State.Chase;
        chaseTimer = maxChaseTime;
        GameManager.Instance.ShowMessage("Something is chasing you!", 2f);
    }

    void Catch()
    {
        catching = true;
        agent.isStopped = true;
        if (animator) animator.SetTrigger("Attack");
        Invoke(nameof(CatchPlayer), 0.8f);
    }

    void CatchPlayer()
    {
        GameManager.Instance.PlayerCaught();
    }

    void GoToNextWaypoint()
    {
        if (waypoints.Length == 0) return;
        agent.SetDestination(waypoints[wpIndex].position);
        wpIndex = (wpIndex + 1) % waypoints.Length;
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, detectRange);
    }
}