using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarController : MonoBehaviour
{
    [SerializeField]
    private float speed = 1.0f;

    [SerializeField]
    private float torque = 1.0f;

    [SerializeField]
    private float minSpeedBeforeIdle = 0.2f;

    public Direction CurrentDirection { get; set; } = Direction.MoveBackward;

    public Rigidbody CarRigidbody { get; set; }

    public enum Direction
    {
        //Idle,
        MoveForward,
        MoveBackward,
        TurnLeft,
        TurnRight,
        Idle
    }

    void Awake()
    {
        CarRigidbody = GetComponent<Rigidbody>();
    }

    void Update()
    {
        if (CarRigidbody.velocity.magnitude <= minSpeedBeforeIdle)
        {
            CurrentDirection = Direction.Idle;

        }
    }

    void FixedUpdate() => ApplyMovement();

    public void ApplyMovement()
    {
        if (CurrentDirection == Direction.MoveForward)
        {
            CarRigidbody.AddForce(transform.forward * speed, ForceMode.Acceleration);
        }

        if (CurrentDirection == Direction.MoveBackward)
        {
            CarRigidbody.AddForce(-transform.forward * speed, ForceMode.Acceleration);
        }

        if (CurrentDirection == Direction.TurnLeft)
        {
            CarRigidbody.AddTorque(transform.up * -torque);
        }

        if (CurrentDirection == Direction.TurnRight)
        {
            CarRigidbody.AddTorque(transform.up * torque);
        }

    }


}
