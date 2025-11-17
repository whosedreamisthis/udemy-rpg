using UnityEngine;

public class Skill_Base : MonoBehaviour
{
    [Header("General Details")]
    [SerializeField]
    private float coolDown;
    private float lastTimeUsed;

    public virtual void Awake()
    {
        lastTimeUsed = lastTimeUsed - coolDown;
    }

    public bool CanUseSkill()
    {
        if (OnCooldown())
        {
            Debug.Log("On Cooldown");
            return false;
        }

        return true;
    }

    private bool OnCooldown() => Time.time < lastTimeUsed + coolDown;

    public void SetSkillOnCooldown() => lastTimeUsed = Time.time;

    public void ResetCooldownBy(float cooldownReduction) => lastTimeUsed += cooldownReduction;

    public void ResetCooldown() => lastTimeUsed = Time.time;
}
