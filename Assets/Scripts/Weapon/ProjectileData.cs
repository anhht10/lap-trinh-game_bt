using UnityEngine;

[CreateAssetMenu(fileName = "ProjectileData", menuName = "Weapon/Projectile Data")]
public class ProjectileData : ScriptableObject
{
    [Header("Basic")]
    [SerializeField][Range(200f, 5500f)] private float _launchForce = 5000f;
    [SerializeField][Range(2f, 40f)] private float _range = 5f;

    [Header("Behavior")]
    [SerializeField] private bool _launchOnAwake = true;
    [SerializeField] private bool _isHoming;
    [SerializeField][Min(0f)] private float _homingStrength = 8f;

    public float LaunchForce => _launchForce;
    public float Range => _range;
    public bool LaunchOnAwake => _launchOnAwake;
    public bool IsHoming => _isHoming;
    public float HomingStrength => _homingStrength;
}