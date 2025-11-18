using System.Collections;
using UnityEngine;

public class Skill_Shard : Skill_Base
{
    private SkillObject_Shard currentShard;

    [SerializeField]
    private GameObject shardPrefab;

    [SerializeField]
    private float detonationTime = 2;

    [Header("Moving Shard Upgrade")]
    [SerializeField]
    private float shardSpeed = 7;

    [Header("Multicast Shard Upgrade")]
    [SerializeField]
    private int maxCharges = 3;

    [SerializeField]
    private int currentCharges;

    [SerializeField]
    private bool isRecharging;

    protected override void Awake()
    {
        base.Awake();

        currentCharges = maxCharges;
    }

    public override void TryUseSkill()
    {
        if (CanUseSkill() == false)
            return;

        if (UnLocked(SkillUpgradeType.Shard))
        {
            HandleShardRegular();
        }

        if (UnLocked(SkillUpgradeType.Shard_MoveToEnemy))
        {
            HandleShardMoving();
        }

        if (UnLocked(SkillUpgradeType.Shard_MultiCast))
        {
            HandleMulticast();
        }
    }

    private void HandleShardRegular()
    {
        CreateShard();
        SetSkillOnCooldown();
    }

    private void HandleShardMoving()
    {
        CreateShard();
        currentShard.MoveTowardsClosestTarget(shardSpeed);
        SetSkillOnCooldown();
    }

    private void HandleMulticast()
    {
        if (currentCharges <= 0)
            return;
        CreateShard();
        currentShard.MoveTowardsClosestTarget(shardSpeed);
        currentCharges--;

        if (isRecharging == false)
        {
            StartCoroutine(ShardRechargeCoroutine());
        }
        // SetSkillOnCooldown();
    }

    private IEnumerator ShardRechargeCoroutine()
    {
        isRecharging = true;

        while (currentCharges < maxCharges)
        {
            yield return new WaitForSeconds(cooldown);
            currentCharges++;
        }

        isRecharging = false;
    }

    public void CreateShard()
    {
        GameObject shard = Instantiate(shardPrefab, transform.position, Quaternion.identity);
        currentShard = shard.GetComponent<SkillObject_Shard>();
        currentShard.SetupShard(detonationTime);
    }
}
