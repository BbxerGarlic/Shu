using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

public class RenameRoundObjectsEditor : EditorWindow
{
    [MenuItem("Window/Rename Round Objects")]
    public static void ShowWindow()
    {
        GetWindow<RenameRoundObjectsEditor>("Rename Round Objects");
    }

    private void OnGUI()
    {
        if (GUILayout.Button("Rename Round Objects"))
        {
            RenameRoundObjects();
        }
    }

    private void RenameRoundObjects()
    {
        // 获取当前场景中的所有根对象
        GameObject[] rootObjects = SceneManager.GetActiveScene().GetRootGameObjects();

        // 过滤出名字以"Round"开头的对象
        var roundObjects = new System.Collections.Generic.List<GameObject>();
        foreach (var obj in rootObjects)
        {
            if (obj.name.StartsWith("Round"))
            {
                roundObjects.Add(obj);
            }

            // 递归查找子对象
            FindChildObjects(obj.transform, roundObjects);
        }

        // 为每个对象重新命名
        for (int i = 0; i < roundObjects.Count; i++)
        {
            roundObjects[i].name = $"Round{i}";
        }
    }

    private void FindChildObjects(Transform parent, System.Collections.Generic.List<GameObject> roundObjects)
    {
        foreach (Transform child in parent)
        {
            if (child.gameObject.name.StartsWith("Round"))
            {
                roundObjects.Add(child.gameObject);
            }

            // 递归查找子对象
            FindChildObjects(child, roundObjects);
        }
    }
}