using System.Data.Common;
using UnityEngine;
using UnityEngine.InputSystem;


public class StandardGameMode : GameMode
{
    public override InputActionMap inputActionMap => Shell.instance.inputData.game.actionMap;
    public Vector2 movement;
    public Vector2 vel;
    StandardCameraController cameraController;
    Shell shell;

    // Could use this for cutscenes
    public override void OnBegin()
    {
        shell = Shell.instance;
        shell.roomManager.Generate();
        shell.roomManager.RoomExited += OnExitRoom;

        cameraController = new StandardCameraController();
        shell.cameraManager.PushController(cameraController);
        shell.cameraManager.TransitionEnded += OnCameraControllerTransitionEnd;

        // play cutscene
    }

    
    public override void OnActivate()
    {
        base.OnActivate();

        shell.LoadPlayer();

        InputData gameInput = Shell.instance.inputData;
        gameInput.game.pause.performed += OnPause;
        gameInput.game.interact.performed += OnInteract;
        gameInput.game.fire.performed += OnFire;
    }

    public override void OnDeactivate()
    {
        base.OnDeactivate();
        InputData gameInput = Shell.instance.inputData;
        gameInput.game.pause.performed -= OnPause;
        gameInput.game.interact.performed -= OnInteract;
        gameInput.game.fire.performed -= OnFire;
    }

    public override void OnEnd()
    {
        base.OnEnd();
        shell.cameraManager.PopController();
        shell.roomManager.RoomExited -= OnExitRoom;
        shell.cameraManager.TransitionEnded -= OnCameraControllerTransitionEnd;
    }

    void OnPause(InputAction.CallbackContext context)
    {
        Debug.Log("Paused");
        shell.player.DEBUG_ChangeWeapon();
    }

    void OnInteract(InputAction.CallbackContext context)
    {
        shell.player.Interact();
    }

    void OnFire(InputAction.CallbackContext context)
    {
        shell.player.Fire();
    }

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
                child.rotation = Quaternion.AngleAxis(angle - 90, child.forward);
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

    public void OnExitRoom(RoomController neighbor, Direction direction)
    {
        if (IsPlayerExiting(direction))
        {
            Direction oppositeDirection = direction.GetOppositeDirection();
            MovePlayerToRoom(neighbor, oppositeDirection);
            cameraController.MoveCameraToRoom(neighbor);
        }
    }

    bool IsPlayerExiting(Direction direction)
    {
        switch(direction)
        {
            case Direction.North:
                return movement.y > 0;
            case Direction.East:
                return movement.x > 0;
            case Direction.South:
                return movement.y < 0;
            case Direction.West:
                return movement.x < 0;
            default:
                return false;
        }
    }

    void MovePlayerToRoom(RoomController controller, Direction direction)
    {
        inputActionMap.Disable();
        Wall wall = controller.GetWallAt(direction);
        
        if (wall != null && wall.entranceStart != null)
        {
            Transform start = wall.entranceStart.transform;
            Vector3 waypointWorldPosition = controller.transform.TransformPoint(start.localPosition);
            shell.player.transform.position = waypointWorldPosition;
            shell.player.transform.rotation = start.localRotation;
        }
    }

    void OnCameraControllerTransitionEnd()
    {
        shell.player.gameObject.SetActive(true);
        inputActionMap.Enable();
        shell.roomManager.OnRoomEntered();
    }
}