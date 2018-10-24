using UnityEditor;

// Custom Editor makes it possible to add options to the inspector which would not
// othervise be possible
// It replaces the default editor of the script – so any parameter that is 
// public og [SerializedField] have to be set up to receive input in the Editor Script.

//Sets which inspector editor to overwrite


[CustomEditor(typeof(RaycastFromCamera))]
public class RaycastFromCameraEditor : Editor
{
	// This say that the UI Editor we are about to make is unfolded.
	bool isLayerPrioritiesUnfolded = true;

	// This method override the normale Gui with ours
	public override void OnInspectorGUI()
    {
		// Serialize the raycastFromCamera inspector Gui object instance
		serializedObject.Update();

		// The EditorGUILayout.Foldout method both set the bool – folded or not 
		// and it “draws” the UI for the folded out info (indent, int and array)
		isLayerPrioritiesUnfolded = EditorGUILayout.Foldout(isLayerPrioritiesUnfolded, "Layer Priorities");
        if (isLayerPrioritiesUnfolded)
        {
			// This simple indent a linie
			EditorGUI.indentLevel++;
            {
				// To have more clear menu the info is pushed to methods.
				// First “draws” the array size input field (int input)
				BindArraySize();
				// Second “draws” the array and set a dropdown on each element in the array
				BindArrayElements();
            }
			// Returns to the indent as before.
			EditorGUI.indentLevel--;
        }

		// De-serialize back to the raycastFromCamera gui object (and create undo point)
		serializedObject.ApplyModifiedProperties(); 
    }

	// This method creates an array with the size of the Layers we need
	void BindArraySize()
    {
		// Finds the property called “layerpriorities” from the serialized object
		// (which serialized the gui object element from the RaycastFromCamera 
		// – which had the layerPriorites int array). 
		//It returns a string so it is converted to integer.
		int currentArraySize = serializedObject.FindProperty("layerPriorities.Array.size").intValue;

		// The EditorGUILayout.IntField now “draws” an int field, which is store in variable.
		int requiredArraySize = EditorGUILayout.IntField("Size", currentArraySize);

		// If the requiredfield is changed it finds the value in serialized object 
		// and set it to the required size. 
		// When the serialized object is deseralized the value is stored in RaycastFromCamera variable.
		if (requiredArraySize != currentArraySize)
        {
            serializedObject.FindProperty("layerPriorities.Array.size").intValue = requiredArraySize;
        }
    }

	// This method binds the elements to the array from above.
	void BindArrayElements()
    {
		//Same as before
		int currentArraySize = serializedObject.FindProperty("layerPriorities.Array.size").intValue;

		// It now loops over the elements in the array and find the value in the serialized
		// object and store it in “prop”. It then “draw” the Gui field and populate the 
		// with the layer as format string.
		for (int i = 0; i < currentArraySize; i++)
        {
            var prop = serializedObject.FindProperty(string.Format("layerPriorities.Array.data[{0}]", i));
            prop.intValue = EditorGUILayout.LayerField(string.Format("Layer {0}:", i), prop.intValue);
        }
    }
}
