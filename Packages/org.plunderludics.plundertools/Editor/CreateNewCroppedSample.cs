using UnityEditor;
using UnityEngine;

public class CreateNewCroppedSample: MonoBehaviour
{
    public GameObject croppedSamplePrefab;

    [MenuItem("GameObject/Plunderludics/Cropped sample", false, 10)]
    static void CreateCroppedSample(MenuCommand menuCommand)
    {
        GameObject p = Resources.Load<GameObject>("Cropped sample");
        // Create a custom game object
        GameObject go = GameObject.Instantiate(p);
        go.name = "Sample";
        
        // Ensure it gets reparented if this was a context click (otherwise does nothing)
        GameObjectUtility.SetParentAndAlign(go, menuCommand.context as GameObject);

        if (!go.GetComponentInParent<Canvas>()) {
            // Sample is not on a canvas, try to assign it to one
            var canvas = GameObject.FindObjectOfType<Canvas>();
            if (canvas) {
                Debug.LogWarning("Moving sample onto available Canvas");
                GameObjectUtility.SetParentAndAlign(go, canvas.gameObject);
            } else {
                Debug.LogWarning("Sample will not be visible until it is placed on a Canvas");
            }
        }

        // Register the creation in the undo system
        Undo.RegisterCreatedObjectUndo(go, "Create " + go.name);
    
        Selection.activeObject = go;
    }
}
