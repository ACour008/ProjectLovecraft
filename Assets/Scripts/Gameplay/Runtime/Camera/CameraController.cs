using System;
using UnityEngine;



public abstract class CameraController
{
    protected bool _isTransitioning = false;
    public bool isTransitioning => isTransitioning; 

    public virtual void OnBegin() { }
    public virtual void OnEnd() { }
    public virtual void OnResume() { }

    public virtual void OnUpdate() { }
}


public class NullCameraController : CameraController {}
