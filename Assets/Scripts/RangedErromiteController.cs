using UnityEngine;

public sealed class RangedErromiteController : ErromiteController
{
    protected override void Attack(GameObject target, int damage)
    {
        Debug.Log($"Attacking {target}");
    }
}
