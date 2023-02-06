using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(ParallaxCtrl))]
public class ParallaxCtrlEditor : Editor
{
	ParallaxCtrl objToSerialize;
	SerializedObject tgt;
	SerializedProperty parallaxLayersList;
	int listSize;
	GUIStyle boldStyle;
	private bool initDone = false;
	private bool errUnknownLoopingBackgroundObj = false;
	private bool errNoChildWithSprite = false;

	void InitStyles()
	{
		initDone = true;
		boldStyle = new GUIStyle(GUI.skin.textField)
		{
			alignment = TextAnchor.MiddleCenter,
			fontSize = 13,
			fontStyle = FontStyle.Bold,
			fixedHeight = 25,
			margin = new RectOffset(80, 80, 0, 15)
		};
	}


	void OnEnable()
	{
		objToSerialize = (ParallaxCtrl)base.target;
		tgt = new SerializedObject(objToSerialize);
		parallaxLayersList = tgt.FindProperty("parallaxLayers");
	}

	public override void OnInspectorGUI()
	{
		if (!initDone)
		{
			InitStyles();
		}

		tgt.Update();

		EditorGUILayout.Space();
		EditorGUILayout.Space();

		//Display our list to the inspector window

		for (int i = 0; i < parallaxLayersList.arraySize; i++)
		{
			SerializedProperty listRef = parallaxLayersList.GetArrayElementAtIndex(i);
			SerializedProperty name = listRef.FindPropertyRelative("layerName");
			SerializedProperty sprite = listRef.FindPropertyRelative("layerSprite");
			SerializedProperty speedX = listRef.FindPropertyRelative("speedX");
			SerializedProperty speedY = listRef.FindPropertyRelative("speedY");
			SerializedProperty orderInLayer = listRef.FindPropertyRelative("orderInLayer");
			SerializedProperty position = listRef.FindPropertyRelative("position");
			SerializedProperty verticalParallax = listRef.FindPropertyRelative("verticalParallax");
			SerializedProperty horizontalRepeat = listRef.FindPropertyRelative("horizontalRepeat");
			SerializedProperty horizontalBoundsOffset = listRef.FindPropertyRelative("horizontalBoundsOffset");
			SerializedProperty horizontalLooping = listRef.FindPropertyRelative("horizontalLooping");
			SerializedProperty loopingBackgrounds = listRef.FindPropertyRelative("loopingBackgrounds");
			SerializedProperty verticalRepeat = listRef.FindPropertyRelative("verticalRepeat");
			SerializedProperty verticalBoundsOffset = listRef.FindPropertyRelative("verticalBoundsOffset");
			SerializedProperty showBounds = listRef.FindPropertyRelative("showBounds");
			SerializedProperty boundsColor = listRef.FindPropertyRelative("boundsColor");

			EditorGUI.BeginChangeCheck();
			name.stringValue = EditorGUILayout.TextField(name.stringValue, boldStyle);

			EditorGUILayout.Space();
			EditorGUILayout.Space();
			sprite.objectReferenceValue = EditorGUILayout.ObjectField("Sprite", sprite.objectReferenceValue, typeof(Sprite), true, GUILayout.Height(EditorGUIUtility.singleLineHeight));
			orderInLayer.intValue = EditorGUILayout.IntField("Order In Layer", orderInLayer.intValue);
			if (EditorGUI.EndChangeCheck())
			{
				tgt.ApplyModifiedProperties();
				objToSerialize.ChangedSprite(i);
				objToSerialize.ChangedOrderInLayer(i);
				objToSerialize.ChangedName(i);
			}

			EditorGUI.BeginChangeCheck();
			position.vector2Value = EditorGUILayout.Vector2Field("Position", position.vector2Value);
			if (EditorGUI.EndChangeCheck())
			{
				tgt.ApplyModifiedProperties();
				objToSerialize.ChangedPosition(i);
			}

			EditorGUILayout.Space();
			EditorGUILayout.Space();

			speedX.floatValue = EditorGUILayout.Slider("Horizontal Speed", speedX.floatValue, 0f, 1f);
			verticalParallax.boolValue = EditorGUILayout.Toggle("Enable Vertical Parallax", verticalParallax.boolValue);
			if (verticalParallax.boolValue)
			{
				speedY.floatValue = EditorGUILayout.Slider("Vertical Speed", speedY.floatValue, 0f, 1f);
			}

			EditorGUILayout.Space();
			EditorGUILayout.Space();

			horizontalRepeat.boolValue = EditorGUILayout.Toggle("Enable Horizontal Repeat", horizontalRepeat.boolValue);
			if (horizontalRepeat.boolValue)
			{
				if (!horizontalLooping.boolValue)
				{
					horizontalBoundsOffset.floatValue = EditorGUILayout.FloatField("Horizontal Bounds Offset", horizontalBoundsOffset.floatValue);
				}

				EditorGUI.BeginChangeCheck();
				horizontalLooping.boolValue = EditorGUILayout.Toggle("Enable Continuous Looping", horizontalLooping.boolValue);
				if (EditorGUI.EndChangeCheck())
				{
					if (!horizontalLooping.boolValue)
					{
						while (loopingBackgrounds.arraySize > 1)
						{
							DestroyImmediate(loopingBackgrounds.GetArrayElementAtIndex(1).objectReferenceValue);
							loopingBackgrounds.DeleteArrayElementAtIndex(1);
						}
					}
					else
					{
						if (loopingBackgrounds.arraySize == 0)
						{
							if (objToSerialize.parallaxLayers[i].layerGO.transform.childCount == 1 
								&& objToSerialize.parallaxLayers[i].layerGO.transform.GetChild(0).gameObject.GetComponent<SpriteRenderer>() != null
								&& objToSerialize.parallaxLayers[i].layerGO.transform.GetChild(0).gameObject.GetComponent<SpriteRenderer>().sprite != null)
							{
								loopingBackgrounds.InsertArrayElementAtIndex(0);
								loopingBackgrounds.GetArrayElementAtIndex(0).objectReferenceValue = objToSerialize.parallaxLayers[i].layerGO.transform.GetChild(0).gameObject;
								errNoChildWithSprite = false;
							}
							else
							{
								errNoChildWithSprite = true;
							}
						}

						if (!errNoChildWithSprite)
						{
							for (int j = 1; j < 3; j++)
							{
								GameObject newChild = Instantiate((GameObject)loopingBackgrounds.GetArrayElementAtIndex(0).objectReferenceValue,
									(((GameObject)loopingBackgrounds.GetArrayElementAtIndex(0).objectReferenceValue).transform.position +
										new Vector3(((GameObject)loopingBackgrounds.GetArrayElementAtIndex(0).objectReferenceValue).GetComponent<SpriteRenderer>().size.x * j, 0f, 0f)),
									Quaternion.identity, objToSerialize.parallaxLayers[i].layerGO.transform);
								newChild.GetComponent<SpriteRenderer>().flipX = j % 2 == 0;
								newChild.name = ((GameObject)loopingBackgrounds.GetArrayElementAtIndex(0).objectReferenceValue).name + (j + 1);
								loopingBackgrounds.InsertArrayElementAtIndex(j);
								loopingBackgrounds.GetArrayElementAtIndex(j).objectReferenceValue = newChild;
							} 
						}
					}
				}
				if (horizontalLooping.boolValue)
				{
					if (!errNoChildWithSprite)
					{
						if (loopingBackgrounds.arraySize > 0)
						{
							EditorGUILayout.HelpBox("The list below is only for tracking the order of looping background elements", MessageType.Info);
							if (errUnknownLoopingBackgroundObj)
							{
								EditorGUILayout.HelpBox("The list of looping backgrounds contains a faulty element that is not a child of this layer", MessageType.Error);
							}
						}
						EditorGUI.BeginChangeCheck();
						for (int j = 0; j < loopingBackgrounds.arraySize; j++)
						{
							SerializedProperty background = loopingBackgrounds.GetArrayElementAtIndex(j);
							background.objectReferenceValue = EditorGUILayout.ObjectField("Background " + (j + 1).ToString(), background.objectReferenceValue, typeof(GameObject), true, GUILayout.Height(EditorGUIUtility.singleLineHeight));
						}
						if (EditorGUI.EndChangeCheck())
						{
							tgt.ApplyModifiedProperties();
							if (!objToSerialize.CheckLoopingBackgrounds(i))
							{
								errUnknownLoopingBackgroundObj = true;
							}
							else
							{
								errUnknownLoopingBackgroundObj = false;
							}
						} 
					}
					else
					{
						EditorGUILayout.HelpBox("For a continuous background, this layer must have a child object with a sprite", MessageType.Error);
					}
				}
			}

			EditorGUILayout.Space();
			EditorGUILayout.Space();

			verticalRepeat.boolValue = EditorGUILayout.Toggle("Enable Vertical Repeat", verticalRepeat.boolValue);
			if (verticalRepeat.boolValue)
			{
				verticalBoundsOffset.floatValue = EditorGUILayout.FloatField("Vertical Bounds Offset", verticalBoundsOffset.floatValue);
			}

			showBounds.boolValue = EditorGUILayout.Toggle("Show boundaries", showBounds.boolValue);
			if (showBounds.boolValue)
			{
				boundsColor.colorValue = EditorGUILayout.ColorField("Boundary color", boundsColor.colorValue);
			}

			EditorGUILayout.Space();
			EditorGUILayout.Space();

			//Remove this index from the List
			GUILayout.BeginHorizontal();
			GUILayout.Space(Screen.width / 2 - 100);
			if (GUILayout.Button("Remove Layer", GUILayout.Width(200)))
			{
				GameObject go = (GameObject)parallaxLayersList.GetArrayElementAtIndex(i).FindPropertyRelative("layerGO").objectReferenceValue;
				DestroyImmediate(go);
				parallaxLayersList.DeleteArrayElementAtIndex(i);
			}
			GUILayout.EndHorizontal();
			EditorGUILayout.Space();
			EditorGUILayout.Space();

			DrawUILine(Color.gray);
		}
		GUILayout.BeginHorizontal();
		GUILayout.Space(Screen.width / 2 - 100);
		if (GUILayout.Button("Add New Layer", GUILayout.Width(200)))
		{
			objToSerialize.parallaxLayers.Add(new ParallaxLayer());
			tgt.Update();
			GameObject go = objToSerialize.CreateObject();
			parallaxLayersList.GetArrayElementAtIndex(parallaxLayersList.arraySize - 1).FindPropertyRelative("layerGO").objectReferenceValue = go;
		}
		GUILayout.EndHorizontal();
		EditorGUILayout.Space();
		EditorGUILayout.Space();
		//Apply the changes to our list
		tgt.ApplyModifiedProperties();
	}

	public static void DrawUILine(Color color, int thickness = 2, int padding = 30)
	{
		Rect r = EditorGUILayout.GetControlRect(GUILayout.Height(padding + thickness));
		r.height = thickness;
		r.y += padding / 2;
		r.x -= 2;
		r.width += 6;
		EditorGUI.DrawRect(r, color);
	}

}
