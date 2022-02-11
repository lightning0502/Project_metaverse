using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Extension;

public class Scene_Main : Singleton<Scene_Main>
{
    public RawImage MainTitleImage;
    public RawImage CompanyLogo;
    public RawImage MainTitleBackgroundImage;

    // readonly
    private Color ReadonlyColor_Alpha = new Color(1, 1, 1, 0.02f);
    private float ReadonlyFloat_FrameTick = 0.04f;
    private float ReadonlyFloat_Waiting = 1;

    private void OnEnable()
    {
        MainTitleImage.SetAlpha(0);
        CompanyLogo.SetAlpha(0);

        StartCoroutine(MainTitleFlow());
    }

    public void LoginDivider(bool isLoginOk)
    {
        if (isLoginOk)
        {
            Debug.Log("login OK!");
        }

        else // login failed
        {
            Debug.LogError("Login Failed");
            JavaScriptLibrary.Instance.OnAlert("로그인을 해주세요! (Login Failed)");
        }
    }

    private IEnumerator MainTitleFlow()
    {
        yield return Coop.WaitForSeconds(ReadonlyFloat_Waiting);

        while (CompanyLogo.color.a < 1)
        {
            CompanyLogo.color += ReadonlyColor_Alpha;
            yield return Coop.WaitForSeconds(ReadonlyFloat_FrameTick);
        }

        yield return Coop.WaitForSeconds(ReadonlyFloat_Waiting);

        while (CompanyLogo.color.a > 0)
        {
            CompanyLogo.color -= ReadonlyColor_Alpha;
            yield return Coop.WaitForSeconds(ReadonlyFloat_FrameTick);
        }

        yield return Coop.WaitForSeconds(ReadonlyFloat_Waiting);

        while (MainTitleImage.color.a < 1)
        {
            MainTitleImage.color += ReadonlyColor_Alpha;
            yield return Coop.WaitForSeconds(ReadonlyFloat_FrameTick);
        }
    }
}