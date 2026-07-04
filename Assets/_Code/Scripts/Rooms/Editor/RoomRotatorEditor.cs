#if UNITY_EDITOR
using Rooms;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(RoomRotator))]
public class RoomRotatorEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        var rotator = (RoomRotator)target;

        EditorGUILayout.Space();
        EditorGUILayout.LabelField("Hologram Tools", EditorStyles.boldLabel);

        var rooms = serializedObject.FindProperty("targetRooms");
        for (int i = 0; i < rooms.arraySize; i++)
        {
            var entry = rooms.GetArrayElementAtIndex(i);
            var roomProp = entry.FindPropertyRelative("room");
            string label = roomProp.objectReferenceValue != null
                ? roomProp.objectReferenceValue.name
                : $"Room {i}";

            if (GUILayout.Button($"Save Hologram Mesh — {label}"))
            {
                Debug.Log($"Button clicked for {label}");
                rotator.CreateHologramMesh(i, saveAsset: true);
                EditorUtility.SetDirty(rotator);
            }
        }
    }
}
#endif
