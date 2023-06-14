using System;
using UnityEngine;
using UnityEngine.UI;

public class IndicatorArrow : MonoBehaviour, IUpdater
{
    private Transform _cachedPlayerTransform;
    private Transform _cachedCameraTransform;
    private GameRunner _cachedGameRunner;
    private RectTransform _cachedRectTransform;
    private Image _cachedImage;

    private void OnEnable()
    {
        UpdateManager.Instance.AddBehaviour(this);
    }

    private void Start()
    {
        _cachedPlayerTransform = GameObject.Find("Sphere").transform;
        _cachedCameraTransform = Camera.main.gameObject.transform;
        _cachedGameRunner = GameObject.Find("GameRunner").GetComponent<GameRunner>();
        _cachedRectTransform = GetComponent<RectTransform>();
        _cachedImage = GetComponent<Image>();
    }

    private void OnDisable()
    {
        UpdateManager.Instance.RemoveBehaviour(this);
    }

    public void UpdateNormal(float dt)
    {
        _cachedImage.enabled = _cachedGameRunner.IsOnAssignment;
        if (_cachedGameRunner.IsOnAssignment)
        {
            _cachedImage.enabled = true;

            var goVector = _cachedGameRunner.TargetDestination.transform.position - _cachedPlayerTransform.position;
            goVector.y = 0f;
            float angle = Vector3.SignedAngle(Vector3.forward, goVector, Vector3.down);
            angle += _cachedCameraTransform.rotation.eulerAngles.y;
            _cachedRectTransform.rotation = Quaternion.Euler(0f,0f, angle);
        }
    }

    public void UpdateFixed()
    {
    }

    public void UpdateLate()
    {
    }
}
