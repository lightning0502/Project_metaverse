using System.Collections.Generic;
using UnityEngine;

public class ButtonManager : MonoBehaviour
{
    #region Main Title Scene
    public void OnClickButton_TouchToStart()
    {
        Debug.Log("OnClickButton_TouchToStart");
        SceneChanger.Instance.OnScene(ScriptManager.SceneIndex.Lobby);
    }
    #endregion






    #region Lobby Scene -> create character

    #endregion
}