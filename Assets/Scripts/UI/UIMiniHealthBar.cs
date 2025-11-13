using UnityEngine;

public class UIMiniHealthBar : MonoBehaviour
{
    private Entity entity;

    private void Awake()
    {
        entity = GetComponentInParent<Entity>();
    }

    private void OnEnable()
    {
        entity.onFlipped += UpdateRotation;
        UpdateRotation();
    }

    private void OnDisable()
    {
        entity.onFlipped -= UpdateRotation;
    }

    // Update is called once per frame
    private void UpdateRotation()
    {
        transform.rotation = Quaternion.identity;
    }
}
