using UnityEngine;

[CreateAssetMenu(menuName = "Cards/Effects/Projectile")]
public class ProjectileEffect : CardEffect
{
    public GameObject projectilePrefab;


    public float projectileSpeed = 10f;



    public override void Execute(CardContext context)
    {
        if(projectilePrefab == null)
            return;


        GameObject projectile =
            Instantiate(
                projectilePrefab,
                context.playerTransform.position,
                Quaternion.identity
            );


        Projectile projectileComponent =
            projectile.GetComponent<Projectile>();


        if(projectileComponent != null)
        {
            projectileComponent.Initialize(
                context.FacingDirection,
                projectileSpeed
            );
        }
    }
}