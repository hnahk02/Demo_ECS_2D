using Unity.Entities;
using UnityEngine;

public partial class InputSystem : SystemBase
{
    private Control _control;

    public World world {get; set;}

    protected override void OnCreate()
    {
        Debug.Log("InputSystem is running in world: " + this.World.Name);
        if(!SystemAPI.TryGetSingleton(out InputComponent input)){
            EntityManager.CreateEntity(typeof(InputComponent));
        }

        _control = new Control();
        _control.Enable();
    }

    protected override void OnUpdate()
    {
        Vector2 moveInput = _control.Player.Move.ReadValue<Vector2>();
        Vector2 mouseInput = _control.Player.MousePos.ReadValue<Vector2>();
        bool shoot = _control.Player.Shoot.IsPressed();     

        SystemAPI.SetSingleton(
            new InputComponent{
                Movement = moveInput,
                MousePosition = mouseInput,
                Shoot = shoot
            }
        ); 
   }


}
