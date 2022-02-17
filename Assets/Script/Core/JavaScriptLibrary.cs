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
    private static extern void Request_AccoutClear();

    [DllImport("__Internal")]
    private static extern bool Request_IsExistAccountID();

    [DllImport("__Internal")]
    private static extern string Request_GetAccountID();

    [DllImport("__Internal")]
    private static extern void Request_OnAlert(string str);

    [DllImport("__Internal")]
    private static extern void PrintFloatArray(float[] array, int size);

    [DllImport("__Internal")]
    private static extern int AddNumbers(int x, int y);

    /*
    [DllImport("__Internal")]
    private static extern string StringReturnValueFunction();
    StringReturnValueFunction: function()
    {
        var returnStr = "blabla string";
        var bufferSize = lengthBytesUTF8(returnStr) + 1;
        var buffer = _malloc(bufferSize);
        stringToUTF8(returnStr, buffer, bufferSize);
        return buffer;
    },
    */

    /*
    [DllImport("__Internal")]
    private static extern void BindWebGLTexture(int texture);
    BindWebGLTexture: function (texture) {
        GLctx.bindTexture(GLctx.TEXTURE_2D, GL.textures[texture]);
    },
    */

    // values
    private bool IsExistAccountID;
    private string CurrentAccountID;
    public string GetCurrentAccountID
    {
        get
        {
            if (Application.platform == RuntimePlatform.WindowsEditor) // for develop
            {
                // return 0x322Fcc2d398aa9FD3708719a8fE4077cF6C8B5fb // klaytn
                return "0xdAa9671877150b734998316b145C94aE0579FBeD"; // metamask account 1
                // return "0x322Fcc2d398aa9FD3708719a8fE4077cF6C8B5fb"; // metamask account 3
            }

            else
                return CurrentAccountID;
        }
    }

    // readonly
    private readonly string ReadonlyString_SpaceBar = " ";
    private readonly string ReadonlyString_PleaseConnectAccount = "메타마스크 지갑의 계정 연동을 해주세요!";

    private void Awake()
    {
        IsExistAccountID = false;
        CurrentAccountID = string.Empty;
    }

#if UNITY_WEBGL
    public IEnumerator WaitingForAccountWalletLogin()
    {
        Request_AccoutClear(); // for refresh

        if (IsExistAccountID || CurrentAccountID.Length > 0)
            yield break;

        if (Request_IsAliveWeb3() == false)
        {
            Request_OnAlert("Web3를 찾지 못했습니다. 메타마스크 확장 프로그램을 설치해주세요.");
            yield break;
        }

        int noticeCount = 0;

        while (IsExistAccountID == false)
        {
            IsExistAccountID = Request_IsExistAccountID();

            yield return Coop.WaitForSeconds(2);
            Debug.Log("계속 기다리는 중...");

            if (noticeCount == 5)
            {
                noticeCount = 0;
                Request_OnAlert(ReadonlyString_PleaseConnectAccount);
            }

            else
                ++noticeCount;
        }

        CurrentAccountID = Request_GetAccountID();
        yield return Coop.WaitForSeconds(1);

        if (CurrentAccountID.Length == 0 || CurrentAccountID.Contains(ReadonlyString_SpaceBar)) // error code length 4
        {
            Request_OnAlert("error message : " + CurrentAccountID);
            yield break;
        }

        // Screen.fullScreen = false;
        ActionTrigger.Instance.OnRequestTrigger(ProtocolType.Request_Login_0);
    }

    public void OnAlert(string noticeText)
    {
        Request_OnAlert(noticeText);
    }
#endif
}