using UnityEngine;

[CreateAssetMenu(fileName = "Abitlity", menuName = "Ability/ProjectileAbility")]
public class ProjectileAbility : Ability
{
    [Header("Effects")]
    [SerializeField] private float _speed;

    public override void Activate(int parentLayer, Transform firePoint, Transform targetTransform, float additionalDamages, bool doesIgnoreObstacle = false)
    {
        // Init the projectile object with direction and angle
        Vector3 vectorTarget = targetTransform.position - firePoint.position;
        float angleZ = Mathf.Atan2(vectorTarget.y, vectorTarget.x) * Mathf.Rad2Deg;
        GameObject projectile = Instantiate(this._particles, firePoint.position, Quaternion.Euler(new Vector3(0.0f, 0.0f, angleZ)));

        Destroy(projectile, 5f);
        // Setup the projectile data
        ProjectileDamage projectileDamage = projectile.GetComponent<ProjectileDamage>();
        if (projectileDamage) {
            DamageData damageData = new DamageData {
                damage = this.damages + additionalDamages,
                parentLayer = parentLayer,
                state = this.state,
            };
            projectileDamage.Setup(doesIgnoreObstacle, damageData);
        }
        // Give movement to the projectile
        Rigidbody2D rb = projectile.GetComponent<Rigidbody2D>();
        MoveProjectile(rb, vectorTarget);
    }

    void MoveProjectile(Rigidbody2D rb, Vector3 vectorTarget)
    {
        Vector2 direction = vectorTarget / vectorTarget.magnitude;
        direction.Normalize();
        if (rb) {
            rb.velocity = direction * this._speed;
        }
    }
}
