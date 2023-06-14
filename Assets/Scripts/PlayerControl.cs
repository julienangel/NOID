using System;
using ScriptableObjectArchitecture;
using Unity.Collections;
using UnityEngine;
using Zenject;

public class PlayerControl : MonoBehaviour, IUpdater
{
    [SerializeField] private float acceleration = 10f;
    [SerializeField] private float turnSpeed = 10f;
    
    [SerializeField] private Rigidbody myRigidbody;
    [SerializeField] private Transform controlDirection;
    [SerializeField] private Transform displayObj;

    [Header("--- Car's Transform ----")]
    private Transform _cachedTransform;
    private Vector3 _cachedInitialPos;
    private Vector3 _cachedInitialRot;
    
    [Header("--- Car's Physics ---")]
    private Vector3 _cachedInitialRigidbodyPos;

    [Inject(Id = nameof(_gameStartedEvent)), ReadOnly] private GameEvent _gameStartedEvent;
    [Inject(Id = nameof(_gameFinishedEvent)), ReadOnly] private GameEvent _gameFinishedEvent;
    [Inject(Id = nameof(_gameobjectToFollowVariable)), ReadOnly] private GameObjectVariable _gameobjectToFollowVariable;

    public Vector3 CarPosition => _cachedTransform.position;
    public Quaternion CarRotation => _cachedTransform.rotation;

    private bool _canControl;

    private void Awake()
    {
        _cachedTransform = displayObj.transform;
        _cachedInitialPos = _cachedTransform.position;

        _cachedInitialRigidbodyPos = myRigidbody.position;
        _cachedInitialRot = controlDirection.rotation.eulerAngles;
    }

    private void OnEnable()
    {
        _gameStartedEvent.AddListener(StartCar);
        _gameFinishedEvent.AddListener(ResetCar);
    }

    private void OnDisable()
    {
        _gameStartedEvent.RemoveListener(StartCar);
        _gameFinishedEvent.RemoveListener(ResetCar);
    }

    private void ProcessControls(float dt)
    {
        if (Input.GetKey(KeyCode.W))
        {
            myRigidbody.velocity += controlDirection.forward.normalized * (acceleration * dt);
        }
        
        if (Input.GetKey(KeyCode.S))
        {
            myRigidbody.velocity -= controlDirection.forward.normalized * (acceleration * dt);
        }
        
        if (Input.GetKey(KeyCode.A))
        {
            controlDirection.RotateAround(controlDirection.position, Vector3.up, turnSpeed * -dt);
        }
        
        if (Input.GetKey(KeyCode.D))
        {
            controlDirection.RotateAround(controlDirection.position, Vector3.up, turnSpeed * dt);
        }

        displayObj.position = myRigidbody.transform.position;
        displayObj.forward = controlDirection.forward;
    }
    
    private void StartCar()
    {
        _gameobjectToFollowVariable.SetValue(displayObj.gameObject);
        InitialState();
        UpdateManager.Instance.AddBehaviour(this);
    }
    
    private void ResetCar()
    {
        InitialState();
        UpdateManager.Instance.RemoveBehaviour(this);
    }

    private void InitialState()
    {
        myRigidbody.position = _cachedInitialRigidbodyPos;
        
        _cachedTransform.SetPositionAndRotation(_cachedInitialPos, Quaternion.identity);
        controlDirection.SetPositionAndRotation(Vector3.zero, Quaternion.Euler(_cachedInitialRot));
    }

    public void UpdateNormal(float dt)
    {
        ProcessControls(dt);
    }

    public void UpdateFixed()
    {
        
    }

    public void UpdateLate()
    {
        
    }
}
