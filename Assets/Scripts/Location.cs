using UnityEngine;

public class Location : MonoBehaviour
{
    private ParticleSystem _pickupParticles;
    private GameRunner _cachedGameRunner;

    private void Start()
    {
        _cachedGameRunner = GameObject.Find("GameRunner").GetComponent<GameRunner>();
        _cachedGameRunner.RegisterLocation(this);
        _pickupParticles = GetComponentInChildren<ParticleSystem>();
        _pickupParticles.gameObject.SetActive(false);
    }

    public void SetActiveLocation(bool isActive)
    {
        _pickupParticles.gameObject.SetActive(isActive);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer != LayerMask.NameToLayer("Player"))
            return;

        if (_cachedGameRunner.IsOnAssignment && _cachedGameRunner.TargetDestination == this)
        {
            _cachedGameRunner.IsOnAssignment = false;
            _pickupParticles.gameObject.SetActive(false);
            _cachedGameRunner.SetAllNPCParticles(true);
        }
    }
}
