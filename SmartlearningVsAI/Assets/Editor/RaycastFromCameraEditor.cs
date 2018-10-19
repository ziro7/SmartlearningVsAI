using UnityEditor;

// Custom Editor makes it possible to add options to the inspector which would not
// othervise be possible

[CustomEditor(typeof(RaycastFromCamera))]
public class RaycastFromCameraEditor : Editor
{
	// This say that the UI Editor we are about to make is unfolded.
	bool isLayerPrioritiesUnfolded = true;

	// This method override the normale Gui with ours
	public override void OnInspectorGUI()
    {
		// Serialize raycastFromCamera instance 
		serializedObject.Update();

		// If the bool from earlier is changed this menu is expanded with header “Layer Priorities”
		isLayerPrioritiesUnfolded = EditorGUILayout.Foldout(isLayerPrioritiesUnfolded, "Layer Priorities");
        if (isLayerPrioritiesUnfolded)
        {
			// This simple indent a linie
			EditorGUI.indentLevel++;
            {
				// To have more clear menu the info is pushed to methods.
				BindArraySize();
                BindArrayElements();
            }
			// Returns to the indent as before.
			EditorGUI.indentLevel--;
        }

		// De-serialize back to cameraRaycaster (and create undo point)
		serializedObject.ApplyModifiedProperties(); 
    }

	// This method creates an array with the size of the Layers we need
	void BindArraySize()
    {
        int currentArraySize = serializedObject.FindProperty("layerPriorities.Array.size").intValue;
        int requiredArraySize = EditorGUILayout.IntField("Size", currentArraySize);
        if (requiredArraySize != currentArraySize)
        {
            serializedObject.FindProperty("layerPriorities.Array.size").intValue = requiredArraySize;
        }
    }

	// This method binds the elements from the unity layers inspector to the array so we get a dropdown with options from unity layers
	void BindArrayElements()
    {
        int currentArraySize = serializedObject.FindProperty("layerPriorities.Array.size").intValue;
        for (int i = 0; i < currentArraySize; i++)
        {
            var prop = serializedObject.FindProperty(string.Format("layerPriorities.Array.data[{0}]", i));
            prop.intValue = EditorGUILayout.LayerField(string.Format("Layer {0}:", i), prop.intValue);
        }
    }
}
