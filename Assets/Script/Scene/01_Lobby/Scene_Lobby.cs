using System.Collections;
using Extension;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Text;

public class Scene_Lobby : Singleton<Scene_Lobby>
{
    // readonly
    private readonly char ReadonlyChar_a = 'a';
    private readonly char ReadonlyChar_A = 'A';
    private readonly char ReadonlyChar_z = 'z';
    private readonly char ReadonlyChar_Z = 'Z';
    private readonly char ReadonlyChar_0 = '0';
    private readonly char ReadonlyChar_9 = '9';
    private readonly int ReadonlyInt_Nickname_Maximum = 14;
    private readonly int ReadonlyInt_Nickname_Minimum = 6;
    private readonly string ReadonlyString_IsOK = "pass";
    private readonly string ReadonlyString_AlreadyUsing = "this nickname is already using.";
    private readonly string ReadonlyString_LengthOut = "valid Nicknames are between 6 and 14 characters";
    private readonly Color ReadonlyColor_Green = Color.green;

    // ui : common

    // ui : create scene
    public GameObject CreateSceneObject;
    private TMP_InputField TMPInputField_Nickname;

    private Button NicknameConfirm_Button;
    private Text NicknameConfirm_Text;
    private Image NicknameNotice_ImageObject;
    private Text NicknameNotice_Text;

    private Button CreatePlayer_Button;

    // ui : data load scene
    public GameObject LoadSceneObject;
    private Text LoadedNickName_Text;
    private Text LoadedPosition_Text;
    private Button OnEnterWorld_Button;


    // value
    private StringBuilder TextBuilder;
    private string CurrentPlayerNickname;
    public string GetNickname
    {
        get
        {
            return CurrentPlayerNickname;
        }
    }

    private void Awake()
    {
        TextBuilder = new StringBuilder(32);
        TMPInputField_Nickname = CreateSceneObject.transform.GetChild(0).GetComponent<TMP_InputField>();
        TMPInputField_Nickname.onEndEdit.AddListener(OnEndEdit_Nickname);
        TMPInputField_Nickname.characterLimit = ReadonlyInt_Nickname_Maximum;

        CreateSceneObject.SetActive(false);
        LoadSceneObject.SetActive(false);

        // create button
        NicknameConfirm_Button = CreateSceneObject.transform.GetChild(1).GetComponent<Button>();
        NicknameConfirm_Button.onClick.AddListener(Request_ConfirmNickname);
        NicknameConfirm_Text = NicknameConfirm_Button.transform.GetChild(0).GetComponent<Text>();

        CreatePlayer_Button = CreateSceneObject.transform.GetChild(2).GetComponent<Button>();
        CreatePlayer_Button.onClick.AddListener(Request_CreatePlayer);
        CreatePlayer_Button.interactable = false;

        // create notice ui
        NicknameNotice_ImageObject = NicknameConfirm_Button.transform.GetChild(1).GetComponent<Image>();
        NicknameNotice_ImageObject.gameObject.SetActive(false);
        NicknameNotice_Text = NicknameNotice_ImageObject.transform.GetChild(0).GetComponent<Text>();

        // data load
        LoadedNickName_Text = LoadSceneObject.transform.GetChild(0).GetComponent<Text>();
        LoadedPosition_Text = LoadSceneObject.transform.GetChild(1).GetComponent<Text>();
        OnEnterWorld_Button = LoadSceneObject.transform.GetChild(2).GetComponent<Button>();
        OnEnterWorld_Button.onClick.AddListener(Request_EnterWorld);
        OnEnterWorld_Button.gameObject.SetActive(false);
    }

    private void OnEnable()
    {
        // character information check
        ActionTrigger.Instance.OnRequestTrigger(ProtocolType.Request_CharacterInformation_10);
    }

    public void NewBieChecker(in Information_PlayerObject playerObject)
    {
        bool isNewbie = SceneSetActiver(playerObject.Player_Number == 0);

        if (isNewbie)
        {
            // 캐릭터 생성
            CreateScene();
        }

        else
        {
            // 캐릭터 로드
            DataLoadScene(playerObject);
        }
    }

    private bool SceneSetActiver(bool boolValue)
    {
        CreateSceneObject.SetActive(boolValue);
        LoadSceneObject.SetActive(!boolValue);

        return boolValue;
    }

    private void CreateScene()
    {
        // TO DO :: what?
        CurrentPlayerNickname = string.Empty;
        Debug.Log("캐릭터 생성");
    }

    private void DataLoadScene(in Information_PlayerObject playerObject)
    {
        // set data
        // playerObject.AnimationComponent = new SpriteAnimationComponent();
        PlayerManager.Instance.AddPlayer(0, playerObject); // 0 == current player

        CurrentPlayerNickname = playerObject.Player_Nickname;
        LoadedNickName_Text.text = playerObject.Player_Nickname;
        LoadedPosition_Text.text = MethodExtension.ToStringBuilder(playerObject.Player_XPosition, ", ", playerObject.Player_YPosition);

        OnEnterWorld_Button.gameObject.SetActive(true);
        // TO DO :: instaniate playerObject -> 맵 입장 요청
    }

    private void OnEndEdit_Nickname(string text)
    {
        if (text.Length == 0)
            return;

        TMPInputField_Nickname.text = GetOnlyEnglishAndNumber(text.ToCharArray());
    }

    private string GetOnlyEnglishAndNumber(in char[] charArray)
    {
        int i, count = charArray.Length;
        TextBuilder.Length = 0;

        for (i = 0; i < count; ++i)
        {
            if ((charArray[i] >= ReadonlyChar_a && charArray[i] <= ReadonlyChar_z) || (charArray[i] >= ReadonlyChar_A && charArray[i] <= ReadonlyChar_Z))
                TextBuilder.Append(charArray[i]);

            else if (charArray[i] >= ReadonlyChar_0 && charArray[i] <= ReadonlyChar_9)
                TextBuilder.Append(charArray[i]);
        }

        return TextBuilder.ToString();
    }

    private void Request_CreatePlayer()
    {
        if (NicknameConfirm_Button.interactable || TMPInputField_Nickname.interactable || IsLengthNG(TMPInputField_Nickname.text.Length))
            return;

        CreatePlayer_Button.interactable = false;
        ActionTrigger.Instance.OnRequestTrigger(ProtocolType.Request_CreateCharacter_14, TMPInputField_Nickname.text);
    }

    private bool IsLengthNG(in int textLength)
    {
        return textLength < ReadonlyInt_Nickname_Minimum || textLength > ReadonlyInt_Nickname_Maximum;
    }

    private void Request_ConfirmNickname()
    {
        if (IsLengthNG(TMPInputField_Nickname.text.Length))
        {
            // TO DO :: common notice popup
            NicknameNotice_ImageObject.gameObject.SetActive(true);
            NicknameNotice_Text.text = ReadonlyString_LengthOut;
            return;
        }

        NicknameConfirm_Button.interactable = false;
        TMPInputField_Nickname.interactable = false;

        Debug.Log("Confirm : " + TMPInputField_Nickname.text);
        ActionTrigger.Instance.OnRequestTrigger(ProtocolType.Request_VerifyNickname_12, TMPInputField_Nickname.text);
    }

    private void Request_EnterWorld()
    {
        if (IsLengthNG(LoadedNickName_Text.text.Length))
            return;

        OnEnterWorld_Button.interactable = false;
        ActionTrigger.Instance.OnRequestTrigger(ProtocolType.Request_EnterWorld_20);
    }

    public void Response_ConfirmNickname(bool isOK)
    {
        // TO DO :: nickname changed flow -> from common notice popup
        NicknameNotice_ImageObject.gameObject.SetActive(true);
        NicknameNotice_Text.text = isOK ? ReadonlyString_IsOK : ReadonlyString_AlreadyUsing;
        NicknameConfirm_Button.interactable = !isOK;
        TMPInputField_Nickname.interactable = !isOK;

        if (isOK)
        {
            NicknameNotice_ImageObject.color = ReadonlyColor_Green;
            NicknameNotice_Text.color = ReadonlyColor_Green;
            CreatePlayer_Button.interactable = true;
        }
    }

    public void Response_CreatePlayer(bool isOK)
    {
        if (isOK)
            ActionTrigger.Instance.OnRequestTrigger(ProtocolType.Request_CharacterInformation_10);

        else
        {
            // TO DO :: common notice popup
            // 캐릭터 생성 실패 notice 및 게임 재시작 ?
        }
    }
}
