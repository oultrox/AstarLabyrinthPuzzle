using UnityEngine;
using UnityEngine.Events;

public class TreasureDestroyer : MonoBehaviour
{
    [SerializeField] private GameObject _treasureParticle;
    private ParticleSystem _particleSystem;
    
    private void Start()
    {
        _treasureParticle = Instantiate(_treasureParticle,transform.position,Quaternion.identity);
        _particleSystem = _treasureParticle.GetComponent<ParticleSystem>();
        _particleSystem.Stop();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            _particleSystem.Play();
            gameObject.SetActive(false);
        }
    }
}