using System;
using System.Numerics;
using System.Xml.Serialization;
using Microsoft.VisualBasic;
using Sandbox;

public sealed class PlayerScript : Component
{
	[Property]
    [Category("Components")]
    public GameObject Camera {get; set;}
    [Property]
    [Category("Components")]
    public CharacterController Controller {get; set;}
    [Property]
    [Category("Components")]
    public GameObject Player {get; set;}
    [Property]
    [Category("Components")]
    public GameObject GameOfLife {get; set;}

    [Property]
    [Category("Stats")]
    [Range(0f, 400f, 1f)]
    public float WalkSpeed {get; set;} = 120f;

    [Property]
    [Category("Stats")]
    [Range(0f, 800f, 1f)]
    public float RunSpeed {get; set;} = 250f;

    [Property]
    [Category("Stats")]
    [Range(0f, 1000f, 10f)]
    public float FlyUpDownSpeed {get; set;} = 1f;

    [Property]
    public Vector3 EyePosition {get; set;}
    public Vector3 EyeWorldPosition => Transform.Local.PointToWorld(EyePosition);

    [Property]
    [Category("Stats")]
    [Range(0f, 25f, 1f)]
    public float Friction {get; set;} = 5f;

    public Angles EyeAngles {get; set;}
    Transform _initialCameraTransform;

	protected override void DrawGizmos()
	{
		// base.DrawGizmos();
        Gizmo.Draw.LineSphere(EyePosition, 10f);
	}
	protected override void OnUpdate()
	{
		EyeAngles += Input.AnalogLook;
        EyeAngles = EyeAngles.WithPitch(MathX.Clamp(EyeAngles.pitch, -80f, 80f));
        Transform.Rotation = Rotation.FromYaw(EyeAngles.yaw);

        if(Camera != null)
        {
            Camera.Transform.Local = _initialCameraTransform.RotateAround(EyePosition, EyeAngles.WithYaw(0f));
        }

        int rayLength = 10000;
        if(Input.Pressed("attack1"))
        {
            SceneTraceResult ray = Scene.Trace.Ray(EyeWorldPosition, EyeWorldPosition + EyeAngles.Forward*rayLength).Run();

            if(ray.Hit)
            {
                Log.Info(ray.GameObject.Name);
                CellScript cellScript = ray.GameObject.Components.Get<CellScript>();
                if(cellScript != null)
                {
                    cellScript.makeAlive();
                }
            }
        }
        if(Input.Pressed("attack2"))
        {
            SceneTraceResult ray = Scene.Trace.Ray(EyeWorldPosition, EyeWorldPosition + EyeAngles.Forward*rayLength).Run();

            if(ray.Hit)
            {
                Log.Info(ray.GameObject.Name);
                CellScript cellScript = ray.GameObject.Components.Get<CellScript>();
                if(cellScript != null)
                {
                    cellScript.makeDeath();
                }
            }
        }
        if(Input.Pressed("play_pause"))
        {
            GameOfLife.Components.Get<InputFunctionality>().paused = !GameOfLife.Components.Get<InputFunctionality>().paused;
        }
        if(Input.Pressed("reset"))
        {
            foreach(GameObject child in GameOfLife.Children)
            {
                CellScript cell = child.Components.Get<CellScript>();
                if(cell.alive){cell.makeDeath();}
            }
        }
	}

	protected override void OnFixedUpdate()
	{
		base.OnFixedUpdate();

        if(Controller == null) {return;}

        var wishSpeed = WalkSpeed;
        var wishVelocity = Input.AnalogMove.Normal * wishSpeed * Transform.Rotation;

		if(Input.Down("FlyDown"))
		{
			wishVelocity += Vector3.Down * FlyUpDownSpeed;
		}
		if(Input.Down("FlyUp"))
		{
			wishVelocity += Vector3.Up * FlyUpDownSpeed;
		}


        Controller.Accelerate(wishVelocity);

		Controller.ApplyFriction(Friction);
        Controller.Acceleration = 5f;

        Controller.Move();
	}

	protected override void OnStart()
	{
		// base.OnStart();
        if(Camera != null) {_initialCameraTransform = Camera.Transform.Local;}
	}
}