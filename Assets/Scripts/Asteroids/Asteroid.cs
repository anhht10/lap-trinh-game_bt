using UnityEngine;


[RequireComponent(typeof(Health))]
public class Asteroid : MonoBehaviour
{
    [SerializeField] private FracturedAsteroid _fracturedAsteroidPrefab;
    [SerializeField] private Explosion _explosionPrefab;

    [SerializeField] private ExplosionData _explosionData;


    private Transform _transform;

    private Health _health;


    private void Awake()
    {
        _health = GetComponent<Health>();
        _transform = transform;
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void Start()
    {

    }

    private void OnEnable()
    {
        if (_health != null)
            _health.OnDied += OnDeath;
    }

    private void OnDisable()
    {
        if (_health != null)
            _health.OnDied -= OnDeath;
    }
    private void OnDeath()
    {
        if (_fracturedAsteroidPrefab != null)
            Instantiate(_fracturedAsteroidPrefab, _transform.position, _transform.rotation);

        if (_explosionPrefab != null)
        {
            Explosion explosion = Instantiate(_explosionPrefab, _transform.position, Quaternion.identity);
            explosion.Initialize(_explosionData);
        }

        Destroy(gameObject);
    }
}
