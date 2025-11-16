using UnityEngine;

[CreateAssetMenu(menuName = "RPG Setup/Skill Data", fileName = "Skill data")]
public class SkillDataSO : ScriptableObject
{
    public int cost;

    [Header("Skill Description")]
    public string displayName;

    [TextArea]
    public string description;
    public Sprite icon;
}
