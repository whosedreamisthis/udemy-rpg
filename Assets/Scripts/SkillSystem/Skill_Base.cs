using UnityEngine;

public class Skill_Base : MonoBehaviour
{
    [Header("General Details")]
    [SerializeField]
    protected SkillType skillType;

    [SerializeField]
    protected SkillUpgradeType upgradeType;

    [SerializeField]
    protected float cooldown;
    private float lastTimeUsed;

    protected virtual void Awake()
    {
        lastTimeUsed = lastTimeUsed - cooldown;
    }

    public virtual void TryUseSkill() { }

    public void SetSkillUpgrade(UpgradeData upgrade)
    {
        upgradeType = upgrade.type;
        cooldown = upgrade.cooldown;
    }

    public bool CanUseSkill()
    {
        if (upgradeType == SkillUpgradeType.None)
        {
            return false;
        }
        if (OnCooldown())
        {
            Debug.Log("On Cooldown");
            // Debug.Log($"Time.time {Time.time} lastTimeUsed {lastTimeUsed} cooldown {cooldown}");
            return false;
        }

        return true;
    }

    protected bool UnLocked(SkillUpgradeType upgradeToCheck) => upgradeType == upgradeToCheck;

    private bool OnCooldown() => Time.time < lastTimeUsed + cooldown;

    public void SetSkillOnCooldown() => lastTimeUsed = Time.time;

    public void ResetCooldownBy(float cooldownReduction) => lastTimeUsed += cooldownReduction;

    public void ResetCooldown() => lastTimeUsed = Time.time;
}
