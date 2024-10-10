using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Physics;
using Unity.Transforms;
using UnityEngine;

[BurstCompile]
public partial struct BulletSystem : ISystem
{
    [BurstCompile]
    public void OnUpdate(ref SystemState state)
    {
        EntityManager entityManager = state.EntityManager;
        NativeArray<Entity> allEntities = entityManager.GetAllEntities();

        PhysicsWorldSingleton physicsWorld = SystemAPI.GetSingleton<PhysicsWorldSingleton>();

        foreach (Entity entity in allEntities)
        {
            if (entityManager.HasComponent<BulletComponent>(entity) && entityManager.HasComponent<BulletLifeTimeComponent>(entity))
            {
                // move the bullet
                LocalTransform bulletTransform = entityManager.GetComponentData<LocalTransform>(entity);
                BulletComponent bulletComponent = entityManager.GetComponentData<BulletComponent>(entity);

                bulletTransform.Position += bulletComponent.Speed * SystemAPI.Time.DeltaTime * bulletTransform.Right();
                entityManager.SetComponentData(entity, bulletTransform);

                // bullet timer
                BulletLifeTimeComponent bulletLifeTimeComponent = entityManager.GetComponentData<BulletLifeTimeComponent>(entity);
                bulletLifeTimeComponent.RemainingLifeTime -= SystemAPI.Time.DeltaTime;

                if(bulletLifeTimeComponent.RemainingLifeTime <= 0f){
                    entityManager.DestroyEntity(entity);
                    continue;
                }

                entityManager.SetComponentData(entity, bulletLifeTimeComponent);

                //physics
                NativeList<ColliderCastHit> hits = new NativeList<ColliderCastHit>(Allocator.Temp);
                float3 point1 = new float3(bulletTransform.Position - bulletTransform.Right() * 0.15f);
                float3 point2 = new float3(bulletTransform.Position + bulletTransform.Right() * 0.15f);

                physicsWorld.CapsuleCastAll(point1, point2 , bulletComponent.Size/ 2, float3.zero, 1f, ref hits, new CollisionFilter{
                    BelongsTo = (uint) CollisionLayer.Default,
                    CollidesWith = (uint) CollisionLayer.Wall
                });

                if(hits.Length > 0){
                    entityManager.DestroyEntity(entity);
                }
          
                hits.Dispose();
                
            }
        }
    }
}
