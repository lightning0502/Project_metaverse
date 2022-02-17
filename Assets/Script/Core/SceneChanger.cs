
using UnityEngine.UI;
using System.Collections;
using UnityEngine;
using Extension;

public class SceneChanger : Singleton<SceneChanger>
{
    public RawImage SceneChangerImage;

    private readonly Color ReadonlyColor_Alpha = new Color(0, 0, 0, 0.03f);

    private void Awake()
    {
        SceneChangerImage.SetAlpha(0);
        SceneChangerImage.raycastTarget = true;
        SceneChangerImage.gameObject.SetActive(false);
    }

    /// <param name="screenImage"> true : white, false : black </param>
    public void OnScene(ScriptManager.SceneIndex nextScene)
    {
        if (SceneChangerImage.gameObject.activeSelf)
            return;

        StartCoroutine(Changer(SceneChangerImage, nextScene));
    }

    private IEnumerator Changer(RawImage screenImage, ScriptManager.SceneIndex nextScene)
    {
        screenImage.gameObject.SetActive(true);

        while (screenImage.color.a < 1f)
        {
            screenImage.color += ReadonlyColor_Alpha;
            yield return Coop.WaitForSeconds(0.04f);
        }

        ScriptManager.Instance.OnScene(nextScene);
        yield return Coop.WaitForSeconds(0.5f);

        while (screenImage.color.a > 0f)
        {
            screenImage.color -= ReadonlyColor_Alpha;
            yield return Coop.WaitForSeconds(0.04f);
        }

        screenImage.gameObject.SetActive(false);
    }
}