using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScriptManager : Singleton<ScriptManager>
{
    // ui
    public Transform ParentTransform_Scene;
    public Transform ParentTransform_UI;

    private List<GameObject> SceneList_ScriptObject;
    private Dictionary<int, GameObject> SceneDictionary_UIObject;

    private GameObject CurrentScene;
    private GameObject CurrentUI;

    public enum SceneIndex : int
    {
        NONE = -1,

        Main = 0,
        Lobby = 1,
        Map = 2,
    }

    private void Awake()
    {
        // 모니터 v싱크
        QualitySettings.vSyncCount = 1;

        // 프레임 고정, https://forum.unity.com/threads/rendering-without-using-requestanimationframe-for-the-main-loop.373331/
        Application.targetFrameRate = Application.platform == RuntimePlatform.WebGLPlayer ? -1 : 60; // web browser only, best performance

        // 백그라운드 실행
        Application.runInBackground = true;

        // 전체 화면
        Screen.fullScreen = false;

        // 회전 고정
        Screen.orientation = ScreenOrientation.Landscape;
        Screen.autorotateToPortrait = false;
        Screen.autorotateToPortraitUpsideDown = false;
        Screen.autorotateToLandscapeLeft = true;
        Screen.autorotateToLandscapeRight = true;

        // 해상도 설정
        Screen.SetResolution(1600, 900, true); // 1366, 768 || 1600, 900 || 1024, 576

        // 화면 꺼짐 금지
        Screen.sleepTimeout = SleepTimeout.NeverSleep;

        // 애니메이션과 코루틴 속도
        Time.timeScale = 1;

        // 디버그 모드 설정
        DebugText.Instance.OnDebugMode = Application.platform != RuntimePlatform.WindowsEditor;

        SceneList_ScriptObject = new List<GameObject>();
        int i, count = ParentTransform_Scene.childCount;
        for (i = 0; i < count; ++i)
        {
            SceneList_ScriptObject.Add(ParentTransform_Scene.GetChild(i).gameObject);
            SceneList_ScriptObject[i].SetActive(false);
        }

        SceneDictionary_UIObject = new Dictionary<int, GameObject>();
        count = ParentTransform_UI.childCount;
        for (i = 0; i < count; ++i)
        {
            GameObject uiObject = ParentTransform_UI.GetChild(i).gameObject;
            SceneDictionary_UIObject.Add(uiObject.layer, uiObject);
            uiObject.SetActive(false);
        }

        // network
        WebSocketClient.Instance.Connect();

        OnScene(SceneIndex.Main);
    }

    public IEnumerator OnScene(SceneIndex sceneName, float delay)
    {
        yield return Coop.WaitForSeconds(delay);
        OnScene(sceneName);
    }

    public void OnScene(SceneIndex sceneName)
    {
        int index = SupportObjectSetActiver(sceneName);

        // script
        if (object.ReferenceEquals(CurrentScene, SceneList_ScriptObject[index]) || index == -1)
            return;

        else if (object.ReferenceEquals(CurrentScene, null) == false)
            CurrentScene.SetActive(false);

        CurrentScene = SceneList_ScriptObject[index];
        CurrentScene.SetActive(true);

        // ui
        if (SceneDictionary_UIObject.ContainsKey(index) == false || object.ReferenceEquals(SceneDictionary_UIObject[index], null))
            return;

        else if (object.ReferenceEquals(CurrentUI, null) == false)
            CurrentUI.SetActive(false);

        CurrentUI = SceneDictionary_UIObject[index];
        CurrentUI.SetActive(true);
    }

    private int SupportObjectSetActiver(SceneIndex sceneName)
    {
        switch (sceneName)
        {
            default:
            case SceneIndex.NONE:
                DebugText.Instance.LogError("Error -> SceneSetActiver : ", sceneName);
                return -1;

            case SceneIndex.Main:

                return 0;

            case SceneIndex.Lobby:

                return 1;

            case SceneIndex.Map:
                MessageManager.Instance.StartMessageChecker();
                return 2;
        }
    }
}