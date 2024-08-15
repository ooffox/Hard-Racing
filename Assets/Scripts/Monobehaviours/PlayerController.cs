using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

//  TODO:
//  - Grounded raycast so you cant steer/accelerate mid-air

public class PlayerController : MonoBehaviour
{
    [SerializeField] private CarConfiguration carConfig;
    public bool CanMove;
    public bool IsInPause;
    private float mass;
    private float deceleration;
    private float brakeForce;
    private float acceleration;
    private float topSpeed;
    private float bottomSpeed;
    private float rotateSpeed;

    private float _currentSpeed = 0;
    
    private float _horizontal;
    private float _vertical;
    private TMP_Text _speedText;
    private Vector3 _dir;
    Rigidbody _rigidbody;
    GameManager _gameManager;
    void Start()
    {
        _speedText = GameObject.FindWithTag("Speed Text").GetComponent<TMP_Text>();
        _gameManager = GameObject.FindObjectOfType<GameManager>();
        _rigidbody = GetComponent<Rigidbody>();
        mass = carConfig.Mass;
        deceleration = carConfig.Deceleration;
        brakeForce = carConfig.BrakeForce;
        acceleration = carConfig.Acceleration;
        topSpeed = carConfig.TopSpeed;
        bottomSpeed = carConfig.BottomSpeed;
        rotateSpeed = carConfig.RotateSpeed;

        _rigidbody.mass = mass;
    }


    void Update()
    {

        if (_gameManager.HasFinishedRace)
        {
            _currentSpeed = 0.0f;
            _horizontal = _vertical = 0.0f;
            return;
        }

        // Fetch input
        if (CanMove && !IsInPause)
        {
            _horizontal = Input.GetAxisRaw("Horizontal");
            _vertical = Input.GetAxisRaw("Vertical");
            UpdateVelocity();
        }
        else
        {
            _horizontal = _vertical = 0.0f;
        }
        

        // Rotate car
        float yRotation = rotateSpeed * _horizontal * Time.deltaTime;
        transform.Rotate(0, yRotation, 0);


        // Update speed counter
        UpdateSpeedCounter();
        
    }

    void FixedUpdate()
    {
        if (!CanMove || IsInPause)
        {
            _rigidbody.velocity = Vector3.zero;
        }
        Vector3 dir = transform.forward * _currentSpeed;
        _rigidbody.velocity = new Vector3(dir.x, _rigidbody.velocity.y, dir.z);
    }

    void UpdateSpeedCounter()
    {
        string content = _speedText.text;
        int colonIndex = content.IndexOf(":");
        string res = content.Substring(0, colonIndex + 1);
        double totalSpeed = Math.Round(_currentSpeed);
        res += string.Format(" {0} km/h", totalSpeed);
        _speedText.text = res;
    }

    void UpdateVelocity()
    {
        float accel;
        float maxSpeed;

        if (_vertical < 0.0f)
        {
            accel = brakeForce;
            maxSpeed = bottomSpeed;
        }

        else if (_vertical > 0.0f)
        {

            accel = acceleration;
            maxSpeed = topSpeed;
        }

        else
        {
            accel = deceleration;
            maxSpeed = 0;
        }

        _currentSpeed = Mathf.MoveTowards(_currentSpeed, maxSpeed, accel * Time.deltaTime);
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Wall"))
        {
            _currentSpeed = 0;
        }
    }
}
