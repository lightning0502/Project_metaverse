using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScriptManager : Singleton<ScriptManager>
{
    // ui
    public Transform ParentScene;
    private List<GameObject> SceneScriptList;
    private GameObject CurrentScene;

    public enum SceneIndex : int
    {
        MainLobby = 0,
    }

    private void Awake()
    {
        // 모니터 v싱크
        QualitySettings.vSyncCount = 0;

        // 프레임 고정
        Application.targetFrameRate = 40;

        // 회전 고정
        // Screen.orientation = ScreenOrientation.Portrait;
        Screen.orientation = ScreenOrientation.AutoRotation;
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
        // DebugText.Instance.OnDebugMode = Application.platform != RuntimePlatform.WindowsEditor;
        DebugText.Instance.OnDebugMode = true;

        SceneScriptList = new List<GameObject>();
        int i, count = ParentScene.childCount;
        for (i = 0; i < count; ++i)
        {
            // Debug.LogError("object name : " + SceneTransform.GetChild(i).gameObject);
            SceneScriptList.Add(ParentScene.GetChild(i).gameObject);
            SceneScriptList[i].SetActive(false);
        }

        OnScene(SceneIndex.MainLobby);
    }

    public IEnumerator OnScene(SceneIndex sceneName, float delay)
    {
        yield return Coop.WaitForSeconds(delay);
        OnScene(sceneName);
    }

    public void OnScene(SceneIndex sceneName)
    {
        int sceneIndex = (int)sceneName;

        // 이미 열려있는 scene
        if (object.ReferenceEquals(CurrentScene, SceneScriptList[sceneIndex]))
            return;

        // 아무것도 열려있는 scene이 없을 때
        if (object.ReferenceEquals(CurrentScene, null))
        {
            CurrentScene = SceneScriptList[sceneIndex];
            CurrentScene.SetActive(true);
        }

        // scene 교체
        else
        {
            CurrentScene.SetActive(false);
            CurrentScene = SceneScriptList[sceneIndex];
            CurrentScene.SetActive(true);
        }
    }
}
