using UnityEngine;

public class StandardCameraController : CameraController
{
    CameraTransition moveRoomTransition = new CameraTransition()
    {
        duration = 1,
        easing = Ease.EaseInSine
    };

    readonly Vector3 cameraOffset = new Vector3(0, 0, -10);

    public void MoveCameraToRoom(RoomController room)
    {
        moveRoomTransition.targetPosition = room.transform.position + cameraOffset;
        Shell.instance.cameraManager.BeginTransition(moveRoomTransition);
    }
}
