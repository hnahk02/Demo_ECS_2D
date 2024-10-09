
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;

public partial struct PlayerSystem : ISystem
{
    private EntityManager _entityManager;

    private Entity _playerEntity;
    private Entity _inputEntity;

    private PlayerComponent _playerComponent;
    private InputComponent _inputComponent;

    private void OnUpdate(ref SystemState state)
    {
        _entityManager = state.EntityManager;

        _playerEntity = SystemAPI.GetSingletonEntity<PlayerComponent>();
        _inputEntity = SystemAPI.GetSingletonEntity<InputComponent>();

        _playerComponent = _entityManager.GetComponentData<PlayerComponent>(_playerEntity);
        _inputComponent = _entityManager.GetComponentData<InputComponent>(_inputEntity);

        Move(ref state);
        Shoot(ref state);

    }

    public void Move(ref SystemState state)
    {
        //move the player
        LocalTransform playerTransform = _entityManager.GetComponentData<LocalTransform>(_playerEntity);
        playerTransform.Position += new float3(_inputComponent.Movement * _playerComponent.Speed * SystemAPI.Time.DeltaTime, 0f);

        //look toward the mouse\
        Vector2 dir = (Vector2)_inputComponent.MousePosition - (Vector2)Camera.main.WorldToScreenPoint(playerTransform.Position);
        float angle = math.degrees(math.atan2(dir.y, dir.x));
        playerTransform.Rotation = Quaternion.AngleAxis(angle, Vector3.forward);


        _entityManager.SetComponentData(_playerEntity, playerTransform);
    }

    public void Shoot(ref SystemState state)
    {
        if (_inputComponent.Shoot)
        {
            for (int i = 0; i < _playerComponent.MaxNumOfBullet; i++)
            {
                EntityCommandBuffer ECB = new EntityCommandBuffer(Allocator.Temp);

                Entity bulletEntity = _entityManager.Instantiate(_playerComponent.BulletPrefab);

                ECB.AddComponent(bulletEntity, new BulletComponent
                {
                    Speed = 25f
                });

                ECB.AddComponent(bulletEntity, new BulletLifeTimeComponent{
                    RemainingLifeTime = 1.5f
                });

                LocalTransform bulletTransform = _entityManager.GetComponentData<LocalTransform>(bulletEntity);
                LocalTransform playerTransform = _entityManager.GetComponentData<LocalTransform>(_playerEntity);

                bulletTransform.Rotation =  playerTransform.Rotation;

                float randomOffset = UnityEngine.Random.Range(-_playerComponent.Spread, _playerComponent.Spread);
                bulletTransform.Position = playerTransform.Position + (playerTransform.Right() * 1.65f) + (bulletTransform.Up() * randomOffset);

                ECB.SetComponent(bulletEntity, bulletTransform);
                ECB.Playback(_entityManager);
                ECB.Dispose();

            }

        }
    }


}
