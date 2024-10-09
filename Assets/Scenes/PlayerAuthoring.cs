using Unity.Entities;
using UnityEngine;

public class PlayerAuthoring : MonoBehaviour
{
    public float Speed;
    public Transform BulletPrefab;
    [Range(1f, 10f)]
    public float SpreadBullet;
    public int MaxNumOfBullet = 50;

    public class PLayerBaker : Baker<PlayerAuthoring>
    {
        public override void Bake(PlayerAuthoring authoring)
        {
            Entity player = GetEntity(TransformUsageFlags.None);
            AddComponent(player, new PlayerComponent
            {
                Speed = authoring.Speed,
                BulletPrefab = GetEntity(authoring.BulletPrefab, TransformUsageFlags.None),
                MaxNumOfBullet = authoring.MaxNumOfBullet,
                Spread = authoring.SpreadBullet
            });
        }
    }


}


