using UnityEngine;

public class PlayerCombat : EntityCombat
{
    [Header("Counter Attack details")]
    [SerializeField]
    private float counterRecovery = 0.1f;

    public bool CounterAttackPerformed()
    {
        bool hasPerformedCounter = false;
        foreach (Collider2D target in GetDetectedColliders())
        {
            ICounterable counterable = target.GetComponent<ICounterable>();
            if (counterable == null)
                continue;

            if (counterable.CanBeCountered)
            {
                counterable?.HandleCounter();
                hasPerformedCounter = true;
            }
        }
        return hasPerformedCounter;
    }

    public float GetCounterRecovery() => counterRecovery;
}
