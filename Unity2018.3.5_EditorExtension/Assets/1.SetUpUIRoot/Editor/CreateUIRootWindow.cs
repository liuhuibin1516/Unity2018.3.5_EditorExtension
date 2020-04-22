using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System.IO;


public class CreateUIRootWindow : EditorWindow
{
    [MenuItem("编辑器扩展/创建UIRoot",true)]
    private static bool ValidateUIRoot()
    {
        return !GameObject.Find("UIRoot");
    }

    private string mWidth="1920";
    private string mHight="1080";

    private void OnGUI()
    {
        GUILayout.BeginHorizontal();
        GUILayout.Label("width:", GUILayout.Width(45));
        mWidth=GUILayout.TextField(mWidth);
        GUILayout.Label("*", GUILayout.Width(10));
        GUILayout.Label("hight:", GUILayout.Width(50));
        mHight=GUILayout.TextField(mHight);
        GUILayout.EndHorizontal();
        if (GUILayout.Button("SetUp"))
        {
            SetUp(float.Parse(mWidth),float.Parse(mHight));
            Close();
        }

    }
    [MenuItem("编辑器扩展/创建UIRoot")]
    private static void CreateUIRoot()
    {
        var window= GetWindow<CreateUIRootWindow>();
        window.Show();    
    }

    private static void SetUp(float width,float hight)
    {
        var uiRoot = new GameObject("UIRoot");
        uiRoot.layer = LayerMask.NameToLayer("UI");
        var uiRootScript=uiRoot.AddComponent<UIRoot>();

        var canvas = new GameObject("Canvas");
        canvas.transform.SetParent(uiRoot.transform);
        canvas.layer = LayerMask.NameToLayer("UI");
        canvas.AddComponent<Canvas>().renderMode = RenderMode.ScreenSpaceOverlay;
        var canvasScaler=canvas.AddComponent<CanvasScaler>();
        canvasScaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
        canvasScaler.referenceResolution = new Vector2(width,hight);
        canvas.AddComponent<GraphicRaycaster>();

        var eventSystem = new GameObject("EventSystem");
        eventSystem.layer = LayerMask.NameToLayer("UI");
        eventSystem.AddComponent<EventSystem>();
        eventSystem.AddComponent<StandaloneInputModule>();
        eventSystem.transform.SetParent(uiRoot.transform);

        var common = new GameObject("Common");
        common.transform.SetParent(canvas.transform);
        common.AddComponent<RectTransform>();
        common.transform.localPosition = Vector3.zero;
        uiRootScript.Common = common.transform;
        //序列化属性赋值
        var uiRootScriptSerializedObj = new SerializedObject(uiRootScript);
        uiRootScriptSerializedObj.FindProperty("RootCanvas").objectReferenceValue = canvas;
        uiRootScriptSerializedObj.ApplyModifiedPropertiesWithoutUndo();

        //自动生成Prefab

        var saveFolder = Application.dataPath + "/Resources";

        if(!Directory.Exists(saveFolder))
        {
            Directory.CreateDirectory(saveFolder);
        }

        var saveFilePath = saveFolder + "/UIRoot.prefab";

        PrefabUtility.SaveAsPrefabAssetAndConnect(uiRoot, saveFilePath, InteractionMode.AutomatedAction);
    }

}
