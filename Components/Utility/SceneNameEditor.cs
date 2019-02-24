using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace VRUtils.Components
{
public class SceneName : PropertyAttribute {}

#if UNITY_EDITOR
[CustomPropertyDrawer(typeof(SceneName))]
public class SceneNameEditor : PropertyDrawer
{
    private List<string> SceneNames() =>
        EditorBuildSettings.scenes
                           .Select(scene => scene.path.Split('/'))
                           .Select(dirs => dirs[dirs.Length - 1])
                           .Select(sceneName => sceneName.Split('.')[0])
                           .ToList();

    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        var nameList = SceneNames();
        var selectedIndex = nameList.IndexOf(property.stringValue);
        
        if (selectedIndex < 0)
            selectedIndex = 0;
        
        selectedIndex = EditorGUI.Popup(position, label.text, selectedIndex, nameList.ToArray());
        property.stringValue = nameList[selectedIndex];
    }
}
#endif
}