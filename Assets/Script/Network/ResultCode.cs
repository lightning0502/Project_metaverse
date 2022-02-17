using System.Collections.Generic;
using UnityEngine;

public class ResultCode : Singleton<ResultCode>
{
    private const byte Success = 0;
    private const byte SystemError = 255;

    private const byte ArgumentError = 1;

    private const byte CharacterNotFound = 11;
    private const byte NicknameSizeExceed = 12;
    private const byte NotAvailableNickname = 13;

    private const byte CharacterExistInMap = 21;
    private const byte CharacterNotExistInMap = 22;

    private Dictionary<byte, string> ErrorCodeDictionary;

    private void Awake()
    {
        ErrorCodeDictionary = new Dictionary<byte, string>();

        ErrorCodeDictionary.Add(Success, string.Empty);

        ErrorCodeDictionary.Add(SystemError, "내부 시스템 오류");

        ErrorCodeDictionary.Add(ArgumentError, "요청 파라메터 오류");

        ErrorCodeDictionary.Add(CharacterNotFound, "캐릭터 없음");
        ErrorCodeDictionary.Add(NicknameSizeExceed, "닉네임 최대글자수 초과");
        ErrorCodeDictionary.Add(NotAvailableNickname, "사용할 수 없는 닉네임");

        ErrorCodeDictionary.Add(CharacterExistInMap, "캐릭터가 이미 맵안에 있음");
        ErrorCodeDictionary.Add(CharacterNotExistInMap, "캐릭터가 맵안에 없음");
    }

    public bool IsOK(byte responseData)
    {
        if (ErrorCodeDictionary[responseData].Length == 0)
            return true;

        else // fail flow
        {
            OnNoticePopup(ErrorCodeDictionary[responseData]);
            return false;
        }
    }

    private void OnNoticePopup(string errorText)
    {
        // TO DO :: notice ui for error code here.
        DebugText.Instance.LogError("OnNoticePopup : ", errorText);

        if (Application.platform == RuntimePlatform.WindowsEditor)
            return;

        JavaScriptLibrary.Instance.OnAlert(errorText);
    }
}