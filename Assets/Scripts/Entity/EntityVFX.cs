using System.Collections;
using UnityEditor.Timeline;
using UnityEngine;

public class EntityVFX : MonoBehaviour
{
    private SpriteRenderer sr;

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

    private void Awake()
    {
        sr = GetComponentInChildren<SpriteRenderer>();
        originalMaterial = sr.material;
    }

    public void CreateHitVFX(Transform target)
    {
        if (hitVFX != null)
        {
            GameObject vfx = Instantiate(hitVFX, target.position, Quaternion.identity);
            vfx.GetComponentInChildren<SpriteRenderer>().color = hitVFXColor;
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
