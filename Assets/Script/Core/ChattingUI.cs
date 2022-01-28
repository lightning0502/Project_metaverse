using System.Collections;
using System.Collections.Generic;
using System.Text;
using TMPro;
using Extension;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ChattingUI : Singleton<ChattingUI>, IPointerEnterHandler, IPointerExitHandler
{
    // queue
    private Queue<string> ChattingTextQueue;
    public string SetTextQueue
    {
        set
        {
            ChattingTextQueue.Enqueue(value);
        }
    }

    // values
    private string TextValue;
    private StringBuilder TextBuilder;
    private bool OnMousePointerEnter;

    // readonly
    private readonly string ReadonlyString_NewLine = System.Environment.NewLine;

    // ui
    public TMP_InputField InputTextField;
    public TextMeshProUGUI OutputTextField;
    public ScrollRect ScrollRectUI;

    // instance
    private MessageManager MessageManagerInstance;

    private void Awake()
    {
        ChattingTextQueue = new Queue<string>();
        TextBuilder = new StringBuilder(128);

        MessageManagerInstance = MessageManager.Instance;
    }

    private void Start()
    {
        OnMousePointerEnter = false;
        StartCoroutine(CheckCoroutine_ScrollHandlePosition());
    }

    #region Interface
    public void OnPointerEnter(PointerEventData eventData)
    {
        OnMousePointerEnter = true;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        OnMousePointerEnter = false;
    }
    #endregion

    private IEnumerator CheckCoroutine_ScrollHandlePosition()
    {
        while (gameObject.activeSelf)
        {
            if (OnMousePointerEnter)
                yield return Coop.WaitForSeconds(0.5f);

            else
            {
                if (ScrollRectUI.verticalNormalizedPosition == 0)
                    yield return Coop.WaitForSeconds(0.5f);

                else
                {
                    ScrollRectUI.verticalNormalizedPosition = 0;
                    yield return Coop.WaitForSeconds(0.25f);
                }
            }
        }
    }

    public void SetChattingText(string text)
    {
        TextBuilder.Length = 0;
        OutputTextField.text += TextBuilder.ClearJoint(text, ReadonlyString_NewLine);
    }

    private void LockInput(InputField input)
    {
        DebugText.Instance.LogError(input.text.Length > 0 ? "Text has been entered" : "Main Input Empty");
    }

    public void OnEndInput()
    {
        TextValue = InputTextField.text;
        InputTextField.ActivateInputField();
        InputTextField.text = string.Empty;

        if (TextValue.Length == 0)
            return;

        else
            MessageManagerInstance.ToArray_Chatting(TextValue);
    }
}