using UnityEngine.UI;
using UnityEngine;
using System.Text;

public class DebugText : Singleton<DebugText>
{
    public Text DebugTextObject;
    public bool OnDebugMode;
    private StringBuilder NoticeString;
    private readonly string NewLine = System.Environment.NewLine;

    private void Awake()
    {
        if (object.ReferenceEquals(DebugTextObject, null))
            return;

        if (DebugTextObject.gameObject.activeInHierarchy == false)
            DebugTextObject.gameObject.SetActive(true);

        NoticeString = new StringBuilder(128);
        DebugTextObject.text = string.Empty;
    }

    public void LogError(params object[] objectArray)
    {
        NoticeString.Length = 0;
        int i, count = objectArray.Length;

        for (i = 0; i < count; ++i)
            NoticeString.Append(objectArray[i]);

        if (OnDebugMode == false)
            Debug.LogError(NoticeString);

        else
        {
            if (DebugTextObject.text.Length > 2000)
                DebugTextObject.text = string.Empty;

            DebugTextObject.text += NewLine + NoticeString.ToString();
        }
    }

    public void Initialize_Log()
    {
        DebugTextObject.text = string.Empty;
    }
}