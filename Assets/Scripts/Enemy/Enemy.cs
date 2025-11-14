using System.Collections;
using UnityEngine;

public class Enemy : Entity
{
    public EnemyIdleState idleState;
    public EnemyMoveState moveState;
    public EnemyAttackState attackState;
    public EnemyBattleState battleState;
    public EnemyDeadState deadState;
    public Enemy_StunnedState stunnedState;

    public float battleTimeDuration = 5f;
    public float minRetreatDistance = 1;
    public Vector2 retreatVelocity;

    // public float lastTimeWasInBattle;
    // public float inGameTime;

    [Header("Battle Details")]
    public float battleMoveSpeed = 3f;
    public float attackDistance = 2f;

    [Header("Movement Details")]
    public float idleTime = 2f;
    public float moveSpeed = 1.4f;

    [Range(0, 2)]
    public float moveAnimSpeedMultiplier = 1f;

    [Header("Stunned State Details")]
    [SerializeField]
    public float stunnedDuration = 1f;
    public Vector2 stunnedVelocity = new Vector2(7, 7);

    [SerializeField]
    protected bool canBeStunned;

    [Header("Player detection")]
    [SerializeField]
    private LayerMask whatIsPlayer;

    [SerializeField]
    private Transform playerCheck;

    [SerializeField]
    private float playerCheckDistance = 10;
    public Transform player { get; private set; }

    protected override IEnumerator SlowDownEntityCoroutine(float duration, float slowMultiplier)
    {
        float originalMoveSpeed = moveSpeed;
        float originalBattleSpeed = battleMoveSpeed;
        float originalAnimSpeed = anim.speed;
        float speedMultiplier = 1 - slowMultiplier;

        moveSpeed *= speedMultiplier;
        battleMoveSpeed *= speedMultiplier;
        anim.speed *= speedMultiplier;

        yield return new WaitForSeconds(duration);

        moveSpeed = originalMoveSpeed;
        battleMoveSpeed = originalBattleSpeed;
        anim.speed = originalAnimSpeed;
    }

    public void EnableCounterWindow(bool enable) => canBeStunned = enable;

    public override void EntityDeath()
    {
        stateMachine.ChangeState(deadState);
    }

    private void HandlePlayerDeath()
    {
        stateMachine.ChangeState(idleState);
    }

    public void TryEnterBattleState(Transform player)
    {
        if (stateMachine.currentState == battleState || stateMachine.currentState == attackState)
        {
            return;
        }

        this.player = player;
        stateMachine.ChangeState(battleState);
    }

    public Transform GetPlayerTransform()
    {
        if (player == null)
        {
            player = PlayerDetection().transform;
        }

        return player;
    }

    protected override void Update()
    {
        base.Update();

        // inGameTime = Time.time;
    }

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
        Gizmos.color = Color.green;
        Gizmos.DrawLine(
            playerCheck.position,
            new Vector3(
                playerCheck.position.x + facingDir * minRetreatDistance,
                playerCheck.position.y
            )
        );
    }

    private void OnEnable()
    {
        Player.OnPlayerDeath += HandlePlayerDeath;
    }

    private void OnDisable()
    {
        Player.OnPlayerDeath -= HandlePlayerDeath;
    }
}
