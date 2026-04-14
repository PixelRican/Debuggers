using UnityEngine;

public sealed class MeleeErromiteController : ErromiteController
{
    protected override void Attack(GameObject target)
    {
        Debug.Log($"Attacking {target}");
    }
}
