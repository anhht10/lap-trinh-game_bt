using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Projectile : MonoBehaviour
{
    [SerializeField] private ProjectileData _data;
    private Hitbox _hitBox;

    public bool OutOfFuel
    {
        get
        {
            if (_data != null && _data.LaunchOnAwake)
            {
                _duration -= Time.deltaTime;
                return _duration <= 0f;
            }
            return false;
        }
    }
    private Rigidbody _rb;
    private float _duration;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody>();
    }

    private void Start()
    {
        bool launchOnAwake = _data != null ? _data.LaunchOnAwake : true;
        if (launchOnAwake)
        {
            _rb.AddForce(transform.forward * (_data != null ? _data.LaunchForce : 5000f));
            _duration = _data != null ? _data.Range : 5f;
        }

    }

    // public void ApplyDamageMultiplier(float multiplier)
    // {
    //     _damageMultiplier *= Mathf.Max(0f, multiplier);
    // }

    private void Update()
    {
        if (OutOfFuel)
        {
            Destroy(gameObject);
        }
    }

    // private void OnTriggerEnter(Collider other)
    // {
    //     Debug.Log("Projectile hit: " + other.gameObject.name);
    // }

    public void ApplyDamage(Ship owner, float attack)
    {
        if (_hitBox == null)
        {
            _hitBox = GetComponentInChildren<Hitbox>();
        }

        if (_hitBox != null)
        {
            _hitBox.Activate(owner, attack);
        }
    }
}
