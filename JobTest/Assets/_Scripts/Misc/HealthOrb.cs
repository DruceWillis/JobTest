using UnityEngine;
using Random = UnityEngine.Random;

public class HealthOrb : MonoBehaviour
{
    [SerializeField] private int _healAmount = 1;
    [SerializeField] private float _launchForce = 2f;
    
    private Rigidbody _rigidbody;
    
    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
        
        LaunchOrb();
    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("Terrain"))
        {
            _rigidbody.isKinematic = true;
        }
        else if (other.gameObject.TryGetComponent(out PlayerController player))
        {
            player.ReceiveHeal(_healAmount);
            gameObject.SetActive(false);
        }
    }

    public void LaunchOrb()
    {
        _rigidbody.isKinematic = false;
        gameObject.SetActive(true);
     
        var randDirection = Random.insideUnitCircle.normalized * 0.25f;
        _rigidbody.AddForce(new Vector3(randDirection.x, 1f, randDirection.y) * _launchForce, ForceMode.Impulse);
    }
}