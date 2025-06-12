using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    public enum EnemyState { Patrol, Wait, Chase, Attack }

    public Transform pointA, pointB;
    public float speed = 2f;
    public float waitTimeMin = 1f, waitTimeMax = 3f;
    public float attackDelay = 1.5f;
    public float detectionRange = 5f;
    public Transform player;

    private EnemyState state = EnemyState.Patrol;
    private Animator animator;
    private Transform targetPoint;
    private float waitTimer;
    private float attackTimer;

    void Start()
    {
        animator = GetComponent<Animator>();
        if (animator == null)
            Debug.LogWarning("Enemy không có Animator, một số hiệu ứng sẽ không hiển thị.");
        targetPoint = pointB;
        waitTimer = 0f;
        attackTimer = attackDelay;

        // Gán player nếu chưa có
        if (player == null)
        {
            GameObject foundPlayer = GameObject.FindGameObjectWithTag("Player");
            if (foundPlayer != null)
                player = foundPlayer.transform;
            else
                Debug.LogError("Không tìm thấy Player! Gán tag 'Player' cho nhân vật.");
        }
        
        

    }

    void Update()
    {
        if (player == null) return;

        float distToPlayer = Vector2.Distance(transform.position, player.position);

        switch (state)
        {
            case EnemyState.Patrol:
                Patrol();
                if (distToPlayer < detectionRange)
                {
                    state = EnemyState.Chase;
                    if (animator != null) 
                    animator.SetTrigger("SeePlayer");
                }
                break;

            case EnemyState.Wait:
                waitTimer -= Time.deltaTime;
                if (waitTimer <= 0)
                    state = EnemyState.Patrol;
                break;

            case EnemyState.Chase:
                attackTimer -= Time.deltaTime;
                if (distToPlayer < 1.5f)
                {
                    if (attackTimer <= 0)
                    {
                        state = EnemyState.Attack;
                        if (animator != null)
                        animator.SetTrigger("Attack");
                    }
                }
                else
                {
                    MoveTowards(player.position);
                    if (animator != null)
                    animator.SetBool("isRunning", true);
                }
                break;

            case EnemyState.Attack:
                // Chờ animation gọi OnAttackEnd()
                break;
        }
    }

    void Patrol()
    {
        MoveTowards(targetPoint.position);
        if (animator != null)
        animator.SetBool("isRunning", true);

        if (Vector2.Distance(transform.position, targetPoint.position) < 0.2f)
        {
            targetPoint = (targetPoint == pointA) ? pointB : pointA;
            state = EnemyState.Wait;
            waitTimer = Random.Range(waitTimeMin, waitTimeMax);
            if (animator != null)
            animator.SetBool("isRunning", false);
        }
    }

    void MoveTowards(Vector2 destination)
    {
        Vector2 dir = (destination - (Vector2)transform.position).normalized;
        transform.position += (Vector3)(dir * speed * Time.deltaTime);

        if (dir.x != 0)
            transform.localScale = new Vector3(Mathf.Sign(dir.x), 1, 1);
    }

    // Gọi hàm này bằng Animation Event ở cuối animation attack
    public void OnAttackEnd()
    {
        state = EnemyState.Patrol;
        attackTimer = attackDelay;
    }
}
