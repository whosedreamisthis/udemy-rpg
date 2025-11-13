using UnityEngine;

public class Chest : MonoBehaviour, IDamagable
{
    private Rigidbody2D rb => GetComponentInChildren<Rigidbody2D>();
    private Animator anim => GetComponentInChildren<Animator>();

    private EntityVFX vfx => GetComponent<EntityVFX>();

    [Header("Open details")]
    [SerializeField]
    private Vector2 knockback = new Vector2(0, 3);

    public bool TakeDamage(float damage, Transform damageDealer)
    {
        vfx?.PlayOnDamageVFX();
        anim.SetBool("open", true);
        rb.linearVelocity = knockback;
        rb.angularVelocity = Random.Range(-200, 200);

        return true;
    }
}
