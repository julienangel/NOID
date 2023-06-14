using UnityEngine;

public class NPC : MonoBehaviour
{
    private ParticleSystem _pickupParticles;

    private GameRunner _cachedGameRunner;

    private void Awake()
    {
        _pickupParticles = GetComponentInChildren<ParticleSystem>();
    }

    public void SetParticlesActive(bool isActive)
    {
        _pickupParticles.gameObject.SetActive(isActive);
    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            _cachedGameRunner ??= GameObject.Find("GameRunner").GetComponent<GameRunner>();
            if (!_cachedGameRunner.IsOnAssignment)
            {
                _cachedGameRunner.SetLocationForNPC(this);
                _cachedGameRunner.IsOnAssignment = true;
                _cachedGameRunner.SetAllNPCParticles(false);
                
                //Destroy(gameObject);
                gameObject.SetActive(false);
            }
        }
    }
}