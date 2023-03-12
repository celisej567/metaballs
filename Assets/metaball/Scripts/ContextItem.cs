#if UNITY_EDITOR
using System;
using UnityEngine;
using UnityEditor;
using UnityEditor.SceneManagement;

public  class ContextItem
{
    [MenuItem("GameObject/Metaballs/Volume")]
    public static void CreateMetaballVoid(MenuCommand menuCommand)
    {
        GameObject newObject = ObjectFactory.CreateGameObject("Metaballs", typeof(Container)); // spawn
        newObject.AddComponent<MeshRenderer>(); newObject.AddComponent<MeshFilter>(); newObject.GetComponent<MeshRenderer>().material = null; //Adding things for render
        newObject.transform.localScale = new Vector3(5, 5, 5);

        SceneView lastView = SceneView.lastActiveSceneView;
        newObject.transform.position = lastView ? lastView.pivot : Vector3.zero; // spawn position

        StageUtility.PlaceGameObjectInCurrentStage(newObject); 
        GameObjectUtility.EnsureUniqueNameForSibling(newObject);

        EditorSceneManager.MarkSceneDirty(EditorSceneManager.GetActiveScene());

    }

    [MenuItem("GameObject/Metaballs/Ball")]
    public static void CreateBallVoid(MenuCommand menuCommand)
    {
        GameObject newObject = ObjectFactory.CreateGameObject("Ball", typeof(MetaBall)); // spawn

        SceneView lastView = SceneView.lastActiveSceneView;
        newObject.transform.position = lastView ? lastView.pivot : Vector3.zero; // spawn position

        StageUtility.PlaceGameObjectInCurrentStage(newObject);
        GameObjectUtility.EnsureUniqueNameForSibling(newObject);


        EditorSceneManager.MarkSceneDirty(EditorSceneManager.GetActiveScene());
    }
}
#endif
