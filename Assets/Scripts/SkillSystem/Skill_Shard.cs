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

    [Header("Teleport Shard Upgrade")]
    [SerializeField]
    private float shardExistDuration = 10;

    protected override void Awake()
    {
        base.Awake();

        currentCharges = maxCharges;
    }

    public override void TryUseSkill()
    {
        if (CanUseSkill() == false)
            return;

        if (Unlocked(SkillUpgradeType.Shard))
        {
            HandleShardRegular();
        }

        if (Unlocked(SkillUpgradeType.Shard_MoveToEnemy))
        {
            HandleShardMoving();
        }

        if (Unlocked(SkillUpgradeType.Shard_MultiCast))
        {
            HandleMulticast();
        }
        if (Unlocked(SkillUpgradeType.Shard_Teleport))
        {
            HandleShardTeleport();
        }
    }

    private void HandleShardTeleport()
    {
        if (currentShard == null)
        {
            CreateShard();
            SetSkillOnCooldown();
        }
        else
        {
            SwapPlayerAndShard();
        }
    }

    private void SwapPlayerAndShard()
    {
        Vector3 shardPosition = currentShard.transform.position;
        Vector3 playerPosition = player.transform.position;

        currentShard.transform.position = player.transform.position;
        currentShard.Explode();
        player.TeleportPlayer(shardPosition);
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
        float time = GetDetonationTime();
        GameObject shard = Instantiate(shardPrefab, transform.position, Quaternion.identity);
        currentShard = shard.GetComponent<SkillObject_Shard>();
        currentShard.SetupShard(time);

        if (
            Unlocked(SkillUpgradeType.Shard_Teleport)
            || Unlocked(SkillUpgradeType.Shard_TeleportAndHeal)
        )
            currentShard.OnExplode += ForceCooldown;
    }

    public float GetDetonationTime()
    {
        if (
            Unlocked(SkillUpgradeType.Shard_Teleport)
            || Unlocked(SkillUpgradeType.Shard_TeleportAndHeal)
        )
        {
            return shardExistDuration;
        }
        return detonationTime;
    }

    private void ForceCooldown()
    {
        if (OnCooldown() == false)
        {
            SetSkillOnCooldown();
            currentShard.OnExplode -= ForceCooldown;
        }
    }
}
