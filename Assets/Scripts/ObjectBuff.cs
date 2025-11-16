using System;
using System.Collections;
using UnityEngine;

public class ObjectBuff : MonoBehaviour
{
    private SpriteRenderer sr;

    [Header("Floaty Movement")]
    [SerializeField]
    private float floatSpeed = 1;

    [SerializeField]
    private float floatRange = 0.1f;
    private Vector3 startPosition;

    [Header("Buff Details")]
    [SerializeField]
    private float buffDuration = 4;

    [SerializeField]
    private bool canBeUsed = true;

    private void Awake()
    {
        startPosition = transform.position;
        sr = GetComponentInChildren<SpriteRenderer>();
    }

    private void Update()
    {
        float yOffset = Mathf.Sin(Time.time * floatSpeed) * floatRange;

        transform.position = startPosition + new Vector3(0, yOffset);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!canBeUsed)
            return;

        StartCoroutine(BuffCoroutine(buffDuration));
    }

    private IEnumerator BuffCoroutine(float duration)
    {
        canBeUsed = false;
        sr.color = Color.clear;
        Debug.Log($"Buff is applied for {duration} seconds.");
        yield return new WaitForSeconds(duration);
        Debug.Log($"Buff is removed.");
        Destroy(gameObject);
    }
}
