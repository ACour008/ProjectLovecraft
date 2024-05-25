using System;

using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class CameraManager : MonoBehaviour
{
    public Camera mainCamera;
    List<CameraController> controllers = new List<CameraController>() { new NullCameraController() };
    public CameraController controller => controllers[^1];
    public CameraTransition transition;
    public bool isTransitioning => transition != null || controller.isTransitioning;
    public float transitionTime;
    public float transitionSpeed;
    public Vector3 startPosition;
    public Vector3 targetPosition;
    public float easedProgress;

    public event Action TransitionEnded;

    void Awake()
    {
        mainCamera = Camera.main;
        PushController(new NullCameraController());
    }

    public void PushController(CameraController controller, CameraTransition transition = null)
    {
        BeginTransition(transition);
        controllers.Add(controller);
        controller.OnBegin();
    }

    public void PopController(CameraTransition transition = null)
    {
        controller.OnEnd();
        BeginTransition(transition);
        controllers.RemoveAt(controllers.Count - 1);
        controller.OnResume();
    }


    public void ReplaceController(CameraController exisiting, CameraController replacement, CameraTransition transition = null)
    {
        int index = controllers.IndexOf(exisiting);
        if (index == -1)
            return;

        exisiting.OnEnd();
        if (index == controllers.Count - 1)
            BeginTransition(transition);

        controllers[index] = replacement;
        replacement.OnBegin();
    }
    /// Push camera controller if it's not already in the stack
    public void AddController(CameraController controller)
    {
        if (controllers.IndexOf(controller) == -1)
            PushController(controller);
    }

    /// Remove camera controller from anywhere in the stack
    public void RemoveController(CameraController controller, CameraTransition transition = null)
    {
        if (controllers[controllers.Count - 1] == controller)
        {
            PopController(transition);
            return;
        }

        if (controllers.Remove(controller))
            controller.OnEnd();
    }

    public void BeginTransition(CameraTransition transition)
    {
        if (transition == null)
            return;

        this.transition = transition;
        this.transitionTime = 0;
        this.startPosition = mainCamera.transform.position;
        this.targetPosition = transition.targetPosition;
        this.transitionSpeed = transition.duration;
    }

    public void OnUpdate()
    {
        controller.OnUpdate();
        if (transition != null)
        {
            transitionTime += Time.deltaTime / transitionSpeed;
            float easedProgress = EaseFunctions.Evaluate(0, 1, transitionTime, transition.easing);
            mainCamera.transform.position = Vector3.Lerp(startPosition, targetPosition, easedProgress);
            if (transitionTime >= 1f)
            {
                mainCamera.transform.position = targetPosition;
                transition = null;
                TransitionEnded?.Invoke();
            }
        }
    }
}



