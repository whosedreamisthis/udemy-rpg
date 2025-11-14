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

    [Header("Element Colors")]
    [SerializeField]
    private Color chilledVFXColor = Color.cyan;

    [SerializeField]
    private Color burnedVFXColor = Color.red;

    [SerializeField]
    private Color electrifiedVFXColor = Color.yellow;
    private Color originalHitVFXColor;

    private void Awake()
    {
        entity = GetComponent<Entity>();
        sr = GetComponentInChildren<SpriteRenderer>();
        originalMaterial = sr.material;
        originalHitVFXColor = hitVFXColor;
    }

    public void UpdateOnHitColor(ElementType element)
    {
        switch (element)
        {
            case ElementType.Ice:
                hitVFXColor = chilledVFXColor;
                break;
            case ElementType.Fire:
                hitVFXColor = burnedVFXColor;
                break;
            case ElementType.Lightning:
                hitVFXColor = electrifiedVFXColor;
                break;
            default:
                hitVFXColor = originalHitVFXColor;
                break;
        }
    }

    public void PlayerOnStatusVFX(float duration, ElementType element)
    {
        if (element == ElementType.Ice)
        {
            StartCoroutine(PlayerStatusVFXCoroutine(duration, chilledVFXColor));
        }
        if (element == ElementType.Fire)
        {
            StartCoroutine(PlayerStatusVFXCoroutine(duration, burnedVFXColor));
        }
        if (element == ElementType.Lightning)
        {
            StartCoroutine(PlayerStatusVFXCoroutine(duration, electrifiedVFXColor));
        }
    }

    public void StopAllVFX()
    {
        StopAllCoroutines();
        sr.color = Color.white;
        sr.material = originalMaterial;
    }

    private IEnumerator PlayerStatusVFXCoroutine(float duration, Color effectColor)
    {
        float tickInterval = 0.25f;
        float timer = 0;

        Color lightColor = effectColor * 1.2f;
        Color darkColor = effectColor * 0.8f;

        bool toggle = false;

        while (timer < duration)
        {
            sr.color = toggle ? lightColor : darkColor;
            toggle = !toggle;
            yield return new WaitForSeconds(tickInterval);
            timer = timer + tickInterval;
        }
        sr.color = Color.white;
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
