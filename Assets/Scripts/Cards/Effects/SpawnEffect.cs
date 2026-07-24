using UnityEngine;

[CreateAssetMenu(menuName = "Cards/Effects/Spawn")]
public class SpawnEffect : CardEffect
{
    public GameObject prefab;

    public Vector2 offset;



    public override void Execute(CardContext context)
    {
        if(prefab == null)
            return;


        Instantiate(
            prefab,
            context.playerTransform.position + (Vector3)offset,
            Quaternion.identity
        );
    }
}