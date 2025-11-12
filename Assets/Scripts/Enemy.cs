using UnityEngine;

public class Enemy : Entity
{
    public EnemyIdleState idleState;
    public EnemyMoveState moveState;
    public EnemyAttackState attackState;
    public EnemyBattleState battleState;

    [Header("Battle Details")]
    public float battleMoveSpeed = 3f;
    public float attackDistance = 2f;

    [Header("Movement Details")]
    public float idleTime = 2f;
    public float moveSpeed = 1.4f;

    [Range(0, 2)]
    public float moveAnimSpeedMultiplier = 1f;

    [Header("Player detection")]
    [SerializeField]
    private LayerMask whatIsPlayer;

    [SerializeField]
    private Transform playerCheck;

    [SerializeField]
    private float playerCheckDistance = 10;

    public RaycastHit2D PlayerDetection()
    {
        RaycastHit2D hit = Physics2D.Raycast(
            playerCheck.position,
            Vector2.right * facingDir,
            playerCheckDistance,
            whatIsPlayer | whatIsGround
        );

        if (
            hit.collider == null
            || hit.collider.gameObject.layer != LayerMask.NameToLayer("Player")
        )
        {
            return default;
        }
        return hit;
    }

    protected override void OnDrawGizmos()
    {
        base.OnDrawGizmos();
        Gizmos.color = Color.yellow;
        Gizmos.DrawLine(
            playerCheck.position,
            new Vector3(
                playerCheck.position.x + facingDir * playerCheckDistance,
                playerCheck.position.y
            )
        );
        Gizmos.color = Color.blue;
        Gizmos.DrawLine(
            playerCheck.position,
            new Vector3(playerCheck.position.x + facingDir * attackDistance, playerCheck.position.y)
        );
    }
}
