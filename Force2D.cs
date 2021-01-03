using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Used to move a Kinetic RigidBody2D with forces like the RigidBody2D.AddForce method.
/// Silos the force and its resultant velocity to allow more control over movement.
/// </summary>
public class Force2D {
    
    float _maxSpeed = 1000000;
    float _mass = 1;
    float _drag = 0;
    Vector2 _force;
    Vector2 _impulse;
    Vector2 _velocity;

    const float VelocityThreshold = 0.001f;


    #region Properties

    public float maxSpeed
    {
        get { return _maxSpeed; }
        set { _maxSpeed = Mathf.Clamp(value, 0, 1000000); }
    }

    public float mass
    {
        get { return _mass; }
        set { _mass = Mathf.Clamp(value, 0.0001f, 1000000); }
    }

    public float drag
    {
        get { return _drag; }
        set { _drag = Mathf.Clamp(value, 0, 1000000); }
    }

    public Vector2 velocity
    {
        get { return _velocity; }
        set { _velocity = Vector2.ClampMagnitude(value, _maxSpeed); ; }
    }

    public Rigidbody values
    {
        set
        {
            _mass = value.mass;
            _drag = value.drag;
        }
    }

    #endregion


    public Force2D() { }

    public Force2D(float mass, float drag)
    {
        _mass = mass;
        _drag = drag;
    }

    public void AddForce(Vector2 force)
    {
        AddForce(force, ForceMode2D.Force);
    }

    public void AddForce(Vector2 force, ForceMode2D forceMode)
    {
        switch (forceMode)
        {
            case ForceMode2D.Force:
                _force += force;
                break;
            case ForceMode2D.Impulse:
                _impulse += force;
                break;
            default:
                throw new UnityException("Force mode not supported!");
        }
    }

    /// <summary>
    /// Interpolates one step. Call once every FixedUpdate.
    /// </summary>
    public Vector2 Interpolate()
    {
        //Apply acceleration to velocity
        _velocity = _velocity + (_force * Time.deltaTime + _impulse) / _mass;

        //Apply drag to velocity
        _velocity = _velocity * (1 - Time.deltaTime * _drag);

        //Check velocity against threshold
        if (_velocity.sqrMagnitude < VelocityThreshold * VelocityThreshold)
            _velocity = Vector2.zero;
        else
            //Limit velocity to maxSpeed
            _velocity = Vector2.ClampMagnitude(_velocity, _maxSpeed);

        //Reset input variables
        _force = Vector2.zero;
        _impulse = Vector2.zero;

        return _velocity;
    }
}