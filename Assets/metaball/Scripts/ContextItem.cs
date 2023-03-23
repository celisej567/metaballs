#if UNITY_EDITOR
using System;
using UnityEngine;
using UnityEditor;
using UnityEditor.SceneManagement;
using MetaBalls;

public  class ContextItem
{
    [MenuItem("GameObject/Metaballs/Volume")]
    public static void CreateMetaballVoid(MenuCommand menuCommand)
    {
        GameObject newObject = ObjectFactory.CreateGameObject("Metaballs", typeof(MeshRenderer)); // spawn
        newObject.AddComponent<MeshFilter>(); newObject.GetComponent<MeshRenderer>().material = null; newObject.AddComponent<Container>(); //Adding things for render
        newObject.transform.localScale = new Vector3(5, 5, 5);

        SceneView lastView = SceneView.lastActiveSceneView;
        newObject.transform.position = lastView ? lastView.pivot : Vector3.zero; // spawn position

        StageUtility.PlaceGameObjectInCurrentStage(newObject); 
        GameObjectUtility.EnsureUniqueNameForSibling(newObject);

        if(!Application.isPlaying)
            EditorSceneManager.MarkSceneDirty(EditorSceneManager.GetActiveScene());

    }

    [MenuItem("GameObject/Metaballs/Ball")]
    public static void CreateBall(MenuCommand menuCommand)
    {
        GameObject newObject = ObjectFactory.CreateGameObject("Ball", typeof(MetaBall)); // spawn

        SceneView lastView = SceneView.lastActiveSceneView;
        newObject.transform.position = lastView ? lastView.pivot : Vector3.zero; // spawn position

        StageUtility.PlaceGameObjectInCurrentStage(newObject);
        GameObjectUtility.EnsureUniqueNameForSibling(newObject);


        if (!Application.isPlaying)
            EditorSceneManager.MarkSceneDirty(EditorSceneManager.GetActiveScene());
    }

    [MenuItem("GameObject/Metaballs/Bouncing Ball")]
    public static void CreateBouncingBall(MenuCommand menuCommand)
    {
        GameObject newObject = ObjectFactory.CreateGameObject("Ball", typeof(BouncingBall)); // spawn

        SceneView lastView = SceneView.lastActiveSceneView;
        newObject.transform.position = lastView ? lastView.pivot : Vector3.zero; // spawn position

        StageUtility.PlaceGameObjectInCurrentStage(newObject);
        GameObjectUtility.EnsureUniqueNameForSibling(newObject);


        if (!Application.isPlaying)
            EditorSceneManager.MarkSceneDirty(EditorSceneManager.GetActiveScene());
    }
}
#endif
