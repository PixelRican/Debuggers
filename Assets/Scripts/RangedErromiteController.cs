using UnityEngine;

public sealed class RangedErromiteController : ErromiteController
{
    [SerializeField] private GameObject projectilePrefab;

    protected override void Attack(GameObject target, int damage)
    {
        GameObject projectile = Instantiate(projectilePrefab, transform.position + new Vector3(0.0f, 0.25f, 0.0f), Quaternion.identity);
        projectile.GetComponent<ProjectileController>().SetTarget(target, damage);
        Destroy(projectile, 10.0f);
    }
}
