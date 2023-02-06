using UnityEditor;
using UnityEngine;
using System.Collections.Generic;

public abstract class SeedDataDrawer : PropertyDrawer
{
	private float height = 20;
	private int numOfAttributes;
	protected List<string> listOfProperties;
	protected List<string> listOfLabels;

	public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
	{
		EditorGUI.BeginProperty(position, label, property);

		EditorGUI.indentLevel++;
		EditorGUIUtility.labelWidth = 200;

		for (int i = 0; i < numOfAttributes; i++)
		{
			var rect = new Rect(position.x, position.y + height * i, 320, height);
			EditorGUI.PropertyField(rect, property.FindPropertyRelative(listOfProperties[i]), new GUIContent(listOfLabels[i]));
		}

		EditorGUI.indentLevel--;

		EditorGUI.EndProperty(); ;
	}

	public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
	{
		if (listOfProperties == null)
		{
			SetElementLists();
		}
		numOfAttributes = listOfProperties.Count;
		return height * numOfAttributes;
	}

	protected abstract void SetElementLists();
}
