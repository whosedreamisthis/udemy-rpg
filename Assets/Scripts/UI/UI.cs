using UnityEngine;

public class UI : MonoBehaviour
{
    public UISkillToolTip skillTookTip;

    private void Awake()
    {
        skillTookTip = GetComponentInChildren<UISkillToolTip>();
    }
}
