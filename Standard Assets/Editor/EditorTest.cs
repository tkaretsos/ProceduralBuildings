using UnityEngine;
using UnityEditor;

public class EditorTest : EditorWindow 
{
  string myString = "Hello World";
  bool groupEnabled;
  bool myBool = true;
  float myFloat = 1.23f;
  
  // Add menu named "My Window" to the Window menu
  [MenuItem ("Window/My Window")]
  static void Init () 
  {
    // Get existing open window or if none, make a new one:
    //EditorTest window = (EditorTest) EditorWindow.GetWindow(typeof(EditorTest));
    EditorWindow.GetWindow(typeof(EditorTest));
  }
        
  void OnGUI () 
  {
    GUILayout.Label ("Base Settings", EditorStyles.boldLabel);
    myString = EditorGUILayout.TextField ("Text Field", myString);
    if (GUILayout.Button("click"))
    {
      var neo = new Neoclassical(
                new Vector3(8f, 0f, 4f),
                new Vector3(8f, 0f, 0f),
                new Vector3(0f, 0f, 0f),
                new Vector3(0f, 0f, 4f)
                );
      PrefabUtility.CreatePrefab("test", neo.gameObject);
    }
    groupEnabled = EditorGUILayout.BeginToggleGroup ("Optional Settings", groupEnabled);
    myBool = EditorGUILayout.Toggle ("Toggle", myBool);
    myFloat = EditorGUILayout.Slider ("Slider", myFloat, -3, 3);
    EditorGUILayout.EndToggleGroup ();
  }
}