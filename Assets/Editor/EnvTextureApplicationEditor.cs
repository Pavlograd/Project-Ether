using NUnit;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(EnvTextureApplication))]
[CanEditMultipleObjects]
public class EnvTextureApplicationEditor : Editor
{
    private EnvTextureApplication _target;

    private SerializedProperty _spriteToUse;
    private SerializedProperty _originalSpriteWall;
    private SerializedProperty _createdSpriteWall;
    private SerializedProperty _createdSpriteGround;

    void OnEnable()
    {
        _spriteToUse = serializedObject.FindProperty("spriteToUse");
        _originalSpriteWall = serializedObject.FindProperty("originalSpriteWall");
        _createdSpriteWall = serializedObject.FindProperty("createdSpriteWall");
        _createdSpriteGround = serializedObject.FindProperty("createdSpriteGround");
        _target = (EnvTextureApplication) target;
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();
        EditorGUILayout.PropertyField(_spriteToUse);
        EditorGUILayout.PropertyField(_originalSpriteWall);
        EditorGUILayout.PropertyField(_createdSpriteWall);
        EditorGUILayout.PropertyField(_createdSpriteGround);
        if (GUILayout.Button("Apply textures"))
        {
            _target.ApplyTextureOnEnv("Textures/TextureApplicationOnEnv/PreApplication");
        }

        serializedObject.ApplyModifiedProperties();
    }
}