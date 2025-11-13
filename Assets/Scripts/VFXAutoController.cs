using UnityEngine;

public class VFXAutoController : MonoBehaviour
{
    [SerializeField]
    private bool autoDestroy = true;

    [SerializeField]
    private float destroyDelay = 1;

    [Space]
    [SerializeField]
    private bool randomOffset = true;

    [SerializeField]
    private bool randomRotation = true;

    [Header("Random Position")]
    [SerializeField]
    private float xMinOffset = -0.3f;

    [SerializeField]
    private float xMaxOffset = 0.3f;

    [Space]
    [SerializeField]
    private float yMinOffset = -0.3f;

    [SerializeField]
    private float yMaxOffset = 0.3f;

    private void Start()
    {
        ApplyRandomOffset();
        ApplyRandomRotation();
        if (autoDestroy)
        {
            Destroy(gameObject, destroyDelay);
        }
    }

    private void ApplyRandomOffset()
    {
        if (randomOffset)
        {
            float xOffset = Random.Range(xMinOffset, xMaxOffset);
            float yOffset = Random.Range(yMinOffset, yMaxOffset);

            transform.position = new Vector3(
                transform.position.x + xOffset,
                transform.position.y + yOffset,
                transform.position.z
            );
        }
    }

    private void ApplyRandomRotation()
    {
        if (randomRotation)
        {
            float zRotation = Random.Range(0f, 360f);
            transform.rotation = Quaternion.Euler(0f, 0f, zRotation);
        }
    }
}
