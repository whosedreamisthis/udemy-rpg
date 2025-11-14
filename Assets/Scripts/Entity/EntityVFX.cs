using System.Collections;
using UnityEditor.Timeline;
using UnityEngine;

public class EntityVFX : MonoBehaviour
{
    private SpriteRenderer sr;
    private Entity entity;

    [Header("On Damage VFX")]
    [SerializeField]
    private Material onDamageMaterial;

    [SerializeField]
    private float onDamageVFXDuration = 0.2f;

    private Material originalMaterial;
    private Coroutine onDamageCoroutine;

    [SerializeField]
    private Color hitVFXColor = Color.white;

    [SerializeField]
    private GameObject hitVFX;

    [SerializeField]
    private GameObject critHitVFX;

    private void Awake()
    {
        entity = GetComponent<Entity>();
        sr = GetComponentInChildren<SpriteRenderer>();
        originalMaterial = sr.material;
    }

    public void CreateHitVFX(Transform target, bool isCrit)
    {
        GameObject vfxToUse = isCrit ? critHitVFX : hitVFX;
        GameObject vfx = Instantiate(vfxToUse, target.position, Quaternion.identity);
        if (!isCrit)
            vfx.GetComponentInChildren<SpriteRenderer>().color = hitVFXColor;

        if (entity.facingDir == -1 && isCrit)
        {
            Vector3 scale = vfx.transform.localScale;
            scale.x *= -1; // Changes (1, 1, 1) to (-1, 1, 1)
            vfx.transform.localScale = scale;
        }
    }

    public void PlayOnDamageVFX()
    {
        if (onDamageCoroutine != null)
        {
            StopCoroutine(onDamageCoroutine);
        }

        onDamageCoroutine = StartCoroutine(OnDamageVFXCoroutine());
    }

    private IEnumerator OnDamageVFXCoroutine()
    {
        sr.material = onDamageMaterial;
        yield return new WaitForSeconds(onDamageVFXDuration);
        sr.material = originalMaterial;
    }
}
