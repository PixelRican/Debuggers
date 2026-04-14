using UnityEngine;

public sealed class MeleeErromiteController : ErromiteController
{
    protected override void Attack(GameObject target, int damage)
    {
        GameObject sphere = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        Destroy(sphere.GetComponent<Collider>());
        Destroy(sphere, 1.0f);
        sphere.transform.position = target.transform.position;
        target.GetComponent<HealthController>().TakeDamage(damage);
    }
}
