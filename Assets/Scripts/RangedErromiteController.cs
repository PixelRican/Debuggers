using UnityEngine;

public sealed class RangedErromiteController : ErromiteController
{
    protected override void Attack(GameObject target)
    {
        Debug.Log($"Attacking {target}");
    }
}
