using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
[CustomPropertyDrawer(typeof(CutsceneSpeakEvent))]
public class CutsceneSpeakEditor : PropertyDrawer
{
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        SerializedProperty eventType = property.FindPropertyRelative("Event");
        SerializedProperty name = property.FindPropertyRelative("SpeakerName");

        eventType.intValue = EditorGUILayout.Popup("Event Type", eventType.intValue, eventType.enumNames);
        name.stringValue = EditorGUILayout.TextField("Name", name.stringValue);

        switch (eventType.intValue)
        {
            case (int)CutsceneSpeakEvent.EventType.AddSpeaker:
                SerializedProperty speakerIcon = property.FindPropertyRelative("SpeakerIcon");
                speakerIcon.objectReferenceValue = (Sprite)EditorGUILayout.ObjectField("Icon", speakerIcon.objectReferenceValue, typeof(Sprite), true);
                SerializedProperty speakerPos = property.FindPropertyRelative("SpeakerPos");
                speakerPos.floatValue = EditorGUILayout.FloatField("Pos", speakerPos.floatValue);
                SerializedProperty flipX = property.FindPropertyRelative("FlipX");
                flipX.boolValue = EditorGUILayout.Toggle("Flip X", flipX.boolValue);
                SerializedProperty pitch = property.FindPropertyRelative("Pitch");
                pitch.floatValue = EditorGUILayout.FloatField("Pitch", pitch.floatValue);
                break;
            case (int)CutsceneSpeakEvent.EventType.Speak:
                SerializedProperty text = property.FindPropertyRelative("Text");
                EditorGUILayout.PropertyField(text);
                SerializedProperty voiceOver = property.FindPropertyRelative("VoiceOver");
                voiceOver.objectReferenceValue = (AudioClip)EditorGUILayout.ObjectField("Voice Over", voiceOver.objectReferenceValue, typeof(AudioClip), true);
                break;
            case (int)CutsceneSpeakEvent.EventType.MoveSpeaker:
                SerializedProperty speakerPos1 = property.FindPropertyRelative("SpeakerPos");
                speakerPos1.floatValue = EditorGUILayout.FloatField("Pos", speakerPos1.floatValue);
                SerializedProperty flipX1 = property.FindPropertyRelative("FlipX");
                flipX1.boolValue = EditorGUILayout.Toggle("Flip X", flipX1.boolValue);
                break;
            case (int)CutsceneSpeakEvent.EventType.ActivateTrigger:
                SerializedProperty trigger = property.FindPropertyRelative("Trigger");
                trigger.objectReferenceValue = (Trigger)EditorGUILayout.ObjectField("Trigger", trigger.objectReferenceValue, typeof(Trigger), true);
                break;
        }

        EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);
    }

    public override float GetPropertyHeight(SerializedProperty property, GUIContent label) { return -2f; }
}
