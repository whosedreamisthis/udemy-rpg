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

    public void CreateShard()
    {
        GameObject shard = Instantiate(shardPrefab, transform.position, Quaternion.identity);
        currentShard = shard.GetComponent<SkillObject_Shard>();
        currentShard.SetupShard(detonationTime);
    }
}
