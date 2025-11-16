using Microsoft.Unity.VisualStudio.Editor;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UITreeNode
    : MonoBehaviour,
        IPointerEnterHandler,
        IPointerExitHandler,
        IPointerDownHandler
{
    [SerializeField]
    private UI ui;

    private RectTransform rect;

    [SerializeField]
    private SkillDataSO skillData;

    [SerializeField]
    private string skillName;

    [SerializeField]
    private UnityEngine.UI.Image skillIcon;

    [SerializeField]
    private string lockedColorHex = "#5E5C5C";
    private Color lastColor;
    public bool isUnlocked;
    public bool isLocked;

    private void Awake()
    {
        ui = GetComponentInParent<UI>();
        rect = GetComponent<RectTransform>();
        isLocked = false;
        isUnlocked = false;
        UpdateIconColor(GetColorByHex(lockedColorHex));
    }

    private void Unlock()
    {
        isUnlocked = true;
        UpdateIconColor(Color.white);
    }

    private void UpdateIconColor(Color color)
    {
        if (skillIcon == null)
        {
            return;
        }
        lastColor = skillIcon.color;
        skillIcon.color = color;
    }

    private bool CanBeUnlocked()
    {
        if (isLocked || isUnlocked)
        {
            return false;
        }
        return true;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (CanBeUnlocked())
        {
            Unlock();
        }
        else
        {
            Debug.Log("Cannot be unlocked");
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        ui.skillTookTip.ShowToolTip(true, rect, skillData);
        if (!isUnlocked)
        {
            UpdateIconColor(Color.white * 0.9f);
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        ui.skillTookTip.ShowToolTip(false, rect);
        if (!isUnlocked)
        {
            UpdateIconColor(lastColor);
        }
    }

    private Color GetColorByHex(string hexNumber)
    {
        ColorUtility.TryParseHtmlString(hexNumber, out Color color);
        return color;
    }

    private void OnValidate()
    {
        if (skillData == null)
            return;

        skillName = skillData.displayName;
        skillIcon.sprite = skillData.icon;
        gameObject.name = $"UITreeNode - {skillName}";
    }
}
