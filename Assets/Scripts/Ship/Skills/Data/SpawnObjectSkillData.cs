

using UnityEngine;

[CreateAssetMenu(fileName = "SpawnObjectSkillData", menuName = "Ship/Skills/SpawnObjectSkillData", order = 1)]
public class SpawnObjectSkillData : SkillData
{
  [Header("Object")]

  [SerializeField]
  private SpawnObject _objectPrefab;

  public SpawnObject ObjectPrefab => _objectPrefab;

  public override ShipSkill CreateSkill(Transform owner)
  {
    return new SpawnObjectSkill(owner, this);
  }
}