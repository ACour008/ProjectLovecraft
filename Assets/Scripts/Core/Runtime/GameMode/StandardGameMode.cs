using UnityEngine;
using UnityEngine.InputSystem;


public class StandardGameMode : GameMode
{
    public override InputActionMap inputActionMap => Shell.instance.inputData.game.actionMap;
    
    Shell shell;
    // Could use this for cutscenes
    public override void OnBegin()
    {
        Debug.Log($"{this} Begins!");
        shell = Shell.instance;
        shell.roomManager.Generate();

        // play cutscene
    }

    
    public override void OnActivate()
    {
        base.OnActivate();

        shell.LoadPlayer();

        InputData gameInput = Shell.instance.inputData;
        gameInput.game.pause.performed += OnPause;
        gameInput.game.interact.performed += OnInteract;
    }

    public override void OnDeactivate()
    {
        base.OnDeactivate();
        InputData gameInput = Shell.instance.inputData;
        gameInput.game.pause.performed -= OnPause;
    }

    void OnPause(InputAction.CallbackContext context)
    {
        Debug.Log("Paused");
    }

    void OnInteract(InputAction.CallbackContext context)
    {
        Debug.Log("Interact");
    }

    public Vector2 movement;
    public float rotateDirection;
    public Vector2 vel;
    bool isTurning = false;

    public override void OnUpdate()
    {
        if (shell.player)
        {
            var player = shell.player;
            var child = player.transform.GetChild(0);
            // Vector2 mouseDelta = shell.inputData.game.mouseDelta.ReadValue<Vector2>();
            Vector2 inputMovement = shell.inputData.game.movement.ReadValue<Vector2>().normalized;
            Vector2 mousePosition = shell.inputData.game.rotate.ReadValue<Vector2>();

            // if player is in combat
            if (true)
            {
                mousePosition = shell.uiManager.camera.ScreenToWorldPoint(mousePosition);
                Vector3 direction = mousePosition.AsVector3() - child.position;
                float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
                child.rotation = Quaternion.AngleAxis(angle + 90, child.forward);
            }
            else
            {
                Quaternion targetRotation = Quaternion.LookRotation(child.forward, movement);
                Quaternion rotation = Quaternion.RotateTowards(child.rotation, targetRotation, 720f * Time.deltaTime);
                child.rotation = rotation;      
            }


            movement = Vector2.SmoothDamp(movement, inputMovement, ref vel, 0.1f);
            player.transform.position += movement.AsVector3() * Time.deltaTime * 3f;
        }
    }
}