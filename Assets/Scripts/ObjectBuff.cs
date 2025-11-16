using System;
using System.Collections;
using System.Runtime.CompilerServices;
using UnityEngine;

[Serializable]
public class Buff
{
    public StatType type;
    public float value;
}

public class ObjectBuff : MonoBehaviour
{
    private SpriteRenderer sr;
    private EntityStats statsToModify;

    [Header("Floaty Movement")]
    [SerializeField]
    private float floatSpeed = 1;

    [SerializeField]
    private float floatRange = 0.1f;
    private Vector3 startPosition;

    [Header("Buff Details")]
    [SerializeField]
    private string buffName;

    [SerializeField]
    private Buff[] buffs;

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

        statsToModify = collision.GetComponent<EntityStats>();
        StartCoroutine(BuffCoroutine(buffDuration));
    }

    private IEnumerator BuffCoroutine(float duration)
    {
        canBeUsed = false;
        sr.color = Color.clear;

        ApplyBuff(true);

        yield return new WaitForSeconds(duration);
        ApplyBuff(false);

        Destroy(gameObject);
    }

    private void ApplyBuff(bool apply)
    {
        foreach (var buff in buffs)
        {
            if (apply)
            {
                statsToModify.GetStatByType(buff.type).AddModifier(buff.value, buffName);
            }
            else
            {
                statsToModify.GetStatByType(buff.type).RemoveModifier(buffName);
            }
        }
    }
}
