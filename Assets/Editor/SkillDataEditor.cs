using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(SkillData), true)]
public class SkillDataEditor : Editor
{
  private SerializedProperty _icon;
  private SerializedProperty _cooldown;
  private SerializedProperty _description;

  private SerializedProperty _isTimeSkill;
  private SerializedProperty _duration;
  private SerializedProperty _count;

  private void OnEnable()
  {
    _icon = serializedObject.FindProperty("_icon");
    _cooldown = serializedObject.FindProperty("_cooldown");
    _description = serializedObject.FindProperty("_description");

    _isTimeSkill = serializedObject.FindProperty("_isTimeSkill");
    _duration = serializedObject.FindProperty("_duration");
    _count = serializedObject.FindProperty("_count");
  }

  public override void OnInspectorGUI()
  {
    serializedObject.Update();

    // =========================
    // COMMON
    // =========================

    EditorGUILayout.Space();
    EditorGUILayout.LabelField("Common", EditorStyles.boldLabel);

    EditorGUILayout.PropertyField(_icon);
    EditorGUILayout.PropertyField(_cooldown);
    EditorGUILayout.PropertyField(_description);

    // =========================
    // SKILL TYPE
    // =========================


    EditorGUILayout.PropertyField(
      _isTimeSkill,
      new GUIContent("Is Time Skill")
    );

    if (_isTimeSkill.boolValue)
    {
      EditorGUILayout.PropertyField(
        _duration,
        new GUIContent("Duration")
      );
    }
    else
    {
      EditorGUILayout.PropertyField(
        _count,
        new GUIContent("Count")
      );
    }

    // // =========================
    // // DERIVED CLASS FIELDS
    // // =========================

    // EditorGUILayout.Space();
    // EditorGUILayout.LabelField("Skill Specific", EditorStyles.boldLabel);

    DrawDerivedProperties();

    serializedObject.ApplyModifiedProperties();
  }

  private void DrawDerivedProperties()
  {
    // Duyệt tất cả SerializedProperty của class con
    SerializedProperty property = serializedObject.GetIterator();

    bool enterChildren = true;

    while (property.NextVisible(enterChildren))
    {
      enterChildren = false;

      // Bỏ qua các field đã được vẽ ở SkillData
      if (property.name == "m_Script" ||
        property.name == "_icon" ||
        property.name == "_cooldown" ||
        property.name == "_description" ||
        property.name == "_isTimeSkill" ||
        property.name == "_duration" ||
        property.name == "_count")
      {
        continue;
      }

      EditorGUILayout.PropertyField(property, true);
    }
  }
}
