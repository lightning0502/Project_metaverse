using UnityEngine;
using System.Collections;
using System.Runtime.InteropServices;

public class JavaScriptLibrary : Singleton<JavaScriptLibrary>
{
    /*
    [DllImport("__Internal")]
    private static extern void Hello();
    Hello: function () {
        window.alert("Hello, world! from java script");
    },
    */

    [DllImport("__Internal")]
    private static extern bool Request_IsAliveWeb3();

    [DllImport("__Internal")]
    private static extern bool Request_IsExistAccountID();

    [DllImport("__Internal")]
    private static extern string Request_GetAccountID();

    [DllImport("__Internal")]
    private static extern void Request_BrowserAlert(string str);

    [DllImport("__Internal")]
    private static extern void PrintFloatArray(float[] array, int size);

    [DllImport("__Internal")]
    private static extern int AddNumbers(int x, int y);

    [DllImport("__Internal")]
    private static extern string StringReturnValueFunction();

    /*
    [DllImport("__Internal")]
    private static extern void BindWebGLTexture(int texture);
    BindWebGLTexture: function (texture) {
        GLctx.bindTexture(GLctx.TEXTURE_2D, GL.textures[texture]);
    },
    */

    public bool IsExistAccountID;
    private string CurrentAccountID;
    public string GetCurrentID
    {
        get
        {
            return CurrentAccountID;
        }
    }

    public void GetAccountWalletID()
    {
        StartCoroutine(JavaScriptCoroutine());
    }

    public IEnumerator JavaScriptCoroutine()
    {
#if UNITY_WEBGL && UNITY_EDITOR == false
        if (Request_IsAliveWeb3())
        {
            IsExistAccountID = false;

            while (IsExistAccountID == false)
            {
                yield return Coop.WaitForSeconds(2);
                IsExistAccountID = Request_IsExistAccountID();
                Debug.LogError("Request_IsAccountID check!");
            }
        }

        else
        {
            Request_BrowserAlert("Web3를 찾지 못했습니다.");
            yield break;
        }

        CurrentAccountID = Request_GetAccountID();
        Debug.Log("CurrentAccountID : " + CurrentAccountID);
#endif
        yield return null;
    }
}