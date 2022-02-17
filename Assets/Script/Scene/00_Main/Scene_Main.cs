using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Extension;

public class Scene_Main : Singleton<Scene_Main>
{
    // ui
    public RawImage BackgroundImage_Main;
    public RawImage LogoImage_GameTitle;
    public RawImage LogoImage_Company;
    public RawImage Image_TouchToStart;
    public GameObject Text_WaitingForLogin;
    public bool IsAliveMainSceneObject
    {
        get
        {
            return gameObject.activeSelf;
        }
    }

    // readonly
    private Color ReadonlyColor_Alpha = new Color(0, 0, 0, 0.02f);
    private float ReadonlyFloat_FrameTick = 0.04f;

    // values
    private bool IsLoginOK;

    private void OnEnable()
    {
        // initialize
        IsLoginOK = false;

        BackgroundImage_Main.SetAlpha(0);
        LogoImage_Company.SetAlpha(0);
        LogoImage_Company.gameObject.SetActive(true);
        LogoImage_GameTitle.SetAlpha(0);
        LogoImage_GameTitle.gameObject.SetActive(false);
        Image_TouchToStart.gameObject.SetActive(false);
        Text_WaitingForLogin.SetActive(false);

        StartCoroutine(MainTitleFlow());
    }

    public void LoginDivider(bool isLoginOk)
    {
        IsLoginOK = isLoginOk;
    }

    private IEnumerator MainTitleFlow()
    {
        // ui flow
        yield return Coop.WaitForSeconds(1);

        while (LogoImage_Company.color.a < 1)
        {
            LogoImage_Company.color += ReadonlyColor_Alpha;
            yield return Coop.WaitForSeconds(ReadonlyFloat_FrameTick);
        }

        yield return Coop.WaitForSeconds(1);

        while (LogoImage_Company.color.a > 0)
        {
            LogoImage_Company.color -= ReadonlyColor_Alpha;
            yield return Coop.WaitForSeconds(ReadonlyFloat_FrameTick);
        }

        yield return Coop.WaitForSeconds(1);

        while (BackgroundImage_Main.color.a < 1)
        {
            BackgroundImage_Main.color += ReadonlyColor_Alpha;
            yield return Coop.WaitForSeconds(ReadonlyFloat_FrameTick);
        }

        LogoImage_GameTitle.gameObject.SetActive(true);
        while (LogoImage_GameTitle.color.a < 1)
        {
            LogoImage_GameTitle.color += ReadonlyColor_Alpha;
            yield return Coop.WaitForSeconds(ReadonlyFloat_FrameTick);
        }

        // login information check & waiting
        Text_WaitingForLogin.SetActive(true);
        while (IsLoginOK == false)
            yield return Coop.WaitForSeconds(1);

        Text_WaitingForLogin.SetActive(false);
        Image_TouchToStart.SetAlpha(0);
        Image_TouchToStart.gameObject.SetActive(true);
        Image_TouchToStart.AnimationAlphaRally(0.04f, 0.2f);
    }
}