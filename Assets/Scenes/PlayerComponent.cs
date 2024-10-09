

using Unity.Entities;

public struct PlayerComponent : IComponentData
{
    public float Speed;
    public int MaxNumOfBullet;
    public Entity BulletPrefab;

    public float Spread;

    
}
