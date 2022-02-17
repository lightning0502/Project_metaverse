using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEngine;

public class ActionTrigger : Singleton<ActionTrigger>
{
    // values
    private BinaryWriter Writer;
    private BinaryReader Reader;
    private Dictionary<int, Action> ActionDictionary_Request;
    private Dictionary<int, Action<string>> ActionDictionary_RequestArguments;
    private Dictionary<int, Action<byte[]>> ActionDictionary_Response;

    // instance
    private WebSocketClient WebSocketInstance;
    private JavaScriptLibrary JavaScriptLibraryInstance;
    private ResultCode ResultCodeInstance;

    // temporary
    private Information_Login LoginObject;

    private void Awake()
    {
        WebSocketInstance = WebSocketClient.Instance;
        JavaScriptLibraryInstance = JavaScriptLibrary.Instance;
        ResultCodeInstance = ResultCode.Instance;

        // request
        ActionDictionary_Request = new Dictionary<int, Action>();
        ActionDictionary_Request.Add(ProtocolType.Request_Login_0, Request_Login);
        ActionDictionary_Request.Add(ProtocolType.Request_CharacterInformation_10, Request_CharacterInformation);
        ActionDictionary_Request.Add(ProtocolType.Request_EnterWorld_20, Request_EnterWorld);
        ActionDictionary_Request.Add(ProtocolType.Request_ExitWorld_23, Request_ExitWorld);
        ActionDictionary_Request.Add(ProtocolType.Request_MoveWorld_26, Request_MoveWorld);
        ActionDictionary_Request.Add(ProtocolType.Request_GlobalChatting_110, Request_GlobalChatting);

        // request + arguments
        ActionDictionary_RequestArguments = new Dictionary<int, Action<string>>();
        ActionDictionary_RequestArguments.Add(ProtocolType.Request_VerifyNickname_12, Request_VerifyNickname);
        ActionDictionary_RequestArguments.Add(ProtocolType.Request_CreateCharacter_14, Request_CreateCharacter);

        // response
        ActionDictionary_Response = new Dictionary<int, Action<byte[]>>();
        ActionDictionary_Response.Add(ProtocolType.Response_Login_1, Response_Login);
        ActionDictionary_Response.Add(ProtocolType.Response_CharacterInformation_11, Response_CharacterInformation);
        ActionDictionary_Response.Add(ProtocolType.Response_VerifyNickname_13, Response_VerifyNickname);
        ActionDictionary_Response.Add(ProtocolType.Response_CreateCharacter_15, Response_CreateCharacter);
        ActionDictionary_Response.Add(ProtocolType.Response_EnterWorld_21, Response_EnterWorld);
        ActionDictionary_Response.Add(ProtocolType.Response_ExitWorld_24, Response_ExitWorld);
        ActionDictionary_Response.Add(ProtocolType.Response_MoveWorld_27, Response_MoveWorld);
        ActionDictionary_Response.Add(ProtocolType.Response_GlobalChatting_111, Response_GlobalChatting);

        // notice
        // ActionDictionary_Request.Add(ProtocolType.Notice_EnterWorld, Notice_EnterWorld);
        // ActionDictionary_Request.Add(ProtocolType.Notice_ExitWorld, Notice_ExitWorld);
        // ActionDictionary_Request.Add(ProtocolType.Notice_MoveWorld, Notice_MoveWorld);
        // ActionDictionary_Request.Add(ProtocolType.Notice_SystemChatting, Notice_SystemChatting);
        // ActionDictionary_Request.Add(ProtocolType.Notice_GlobalChatting, Notice_GlobalChatting);
    }

    /// <param name="type"> ProtocolType. + ctrl + space bar </param>
    public void OnResponseTrigger(int type, byte[] responseData)
    {
        if (ActionDictionary_Response.ContainsKey(type) == false)
        {
            DebugText.Instance.LogError("Error -> OnActionTrigger ActionDictionary_Response.ContainsKey() false");
            return;
        }

        else
        {
            if (Application.platform == RuntimePlatform.WindowsEditor)
                StartCoroutine(ResponseActionForWindowsEditor((responseData) => ActionDictionary_Response[type](responseData), responseData));

            else
                ActionDictionary_Response[type](responseData);
        }
    }

    /// <param name="type"> ProtocolType. + ctrl + space bar </param>
    public void OnRequestTrigger(int type)
    {
        if (ActionDictionary_Request.ContainsKey(type) == false)
        {
            DebugText.Instance.LogError("Error -> OnActionTrigger ActionDictionary_Request.ContainsKey() false");
            return;
        }

        else
        {
            if (Application.platform == RuntimePlatform.WindowsEditor)
                StartCoroutine(RequestActionForWindowsEditor(() => ActionDictionary_Request[type]()));

            else
                ActionDictionary_Request[type]();
        }
    }

    /// <param name="type"> ProtocolType. + ctrl + space bar </param>
    public void OnRequestTrigger(int type, string stringData)
    {
        if (ActionDictionary_RequestArguments.ContainsKey(type) == false)
        {
            DebugText.Instance.LogError("Error -> OnActionTrigger ActionDictionary_RequestArguments.ContainsKey() false");
            return;
        }

        else
        {
            if (Application.platform == RuntimePlatform.WindowsEditor)
                StartCoroutine(RequestActionForWindowsEditor(() => ActionDictionary_RequestArguments[type](stringData)));

            else
                ActionDictionary_RequestArguments[type](stringData);
        }
    }



    //////////////////////////////////////////////////////////// desirialize ////////////////////////////////////////////////////////////
    private byte Deserializer_Type_A(in byte[] responseData)
    {
        using (Reader = new BinaryReader(new MemoryStream(responseData)))
        {
            Reader.ReadInt32();
            return Reader.ReadByte();
        }
    }

    private Information_PlayerObject Deserializer_Type_CharacterInformation(in byte[] responseData)
    {
        using (Reader = new BinaryReader(new MemoryStream(responseData)))
        {
            Reader.ReadInt32();

            byte resultCode = Reader.ReadByte();
            if (resultCode != 0)
                return new Information_PlayerObject();

            byte characterNum = Reader.ReadByte();

            short length = Reader.ReadInt16();
            byte[] byteArray = Reader.ReadBytes(length);
            string nickname = Encoding.UTF8.GetString(byteArray);

            float x = Reader.ReadSingle();
            float y = Reader.ReadSingle();

            return new Information_PlayerObject(characterNum, nickname, x, y);
        }
    }

    private void Deserializer_Type_EnterWorld(in byte[] responseData)
    {
        using (Reader = new BinaryReader(new MemoryStream(responseData)))
        {
            Reader.ReadInt32();

            byte resultCode = Reader.ReadByte();
            if (resultCode != 0)
            {
                DebugText.Instance.LogError("Deserializer_Type_EnterWorld result code : " + resultCode);
                return;
            }

            // current player information
            float x = Reader.ReadSingle(); // ?
            float y = Reader.ReadSingle(); // ?

            // another player informations
            int playerCount = Reader.ReadInt32(), i;
            Debug.Log("playerCount : " + playerCount);

            /*
            for (i = 0; i < playerCount; ++i)
            {
                Id = reader.ReadInt32();
                ClassType = reader.ReadByte();
                short len = reader.ReadInt16();
                byte[] temp = reader.ReadBytes(len);
                Name = Encoding.UTF8.GetString(temp);
                PosX = reader.ReadSingle();
                PosY = reader.ReadSingle();
                PlayerManager.Instance.AddPlayer(0, playerObject); // 0 == current player
            }
            */
        }
    }




    //////////////////////////////////////////////////////////// serialize ////////////////////////////////////////////////////////////
    private byte[] Serializer_Type_B(in int protocol, in string data)
    {
        using (Writer = new BinaryWriter(new MemoryStream()))
        {
            Writer.Write(protocol);

            byte[] byteData = Encoding.UTF8.GetBytes(data);

            Writer.Write((short)byteData.Length);
            Writer.Write(byteData);

            return (((MemoryStream)(Writer.BaseStream)).ToArray());
        }
    }

    private byte[] Serializer_Type_C(in int protocol, in string data, in byte characterNumber)
    {
        using (Writer = new BinaryWriter(new MemoryStream()))
        {
            Writer.Write(protocol);
            Writer.Write(characterNumber);

            byte[] byteData = Encoding.UTF8.GetBytes(data);

            Writer.Write((short)byteData.Length);
            Writer.Write(byteData);

            return (((MemoryStream)(Writer.BaseStream)).ToArray());
        }
    }

    private byte[] Serializer_Type_A(in int protocol)
    {
        using (Writer = new BinaryWriter(new MemoryStream()))
        {
            Writer.Write(protocol);
            return (((MemoryStream)(Writer.BaseStream)).ToArray());
        }
    }



    private IEnumerator ResponseActionForWindowsEditor(Action<byte[]> action, byte[] responseData)
    {
        yield return Coop.WaitForSeconds(2);
        action(responseData);
    }

    private IEnumerator RequestActionForWindowsEditor(Action action)
    {
        yield return Coop.WaitForSeconds(2);
        action();
    }



    // request
    private void Request_Login()
    {
        WebSocketInstance.Send(Serializer_Type_B(ProtocolType.Request_Login_0, JavaScriptLibraryInstance.GetCurrentAccountID));
    }

    private void Request_CharacterInformation()
    {
        WebSocketInstance.Send(Serializer_Type_A(ProtocolType.Request_CharacterInformation_10));
    }

    private void Request_VerifyNickname(string nickname)
    {
        WebSocketInstance.Send(Serializer_Type_B(ProtocolType.Request_VerifyNickname_12, nickname));
    }

    private void Request_CreateCharacter(string nickname)
    {
        WebSocketInstance.Send(Serializer_Type_C(ProtocolType.Request_CreateCharacter_14, nickname, 1)); // 1 is TEST Value
    }

    private void Request_EnterWorld()
    {
        WebSocketInstance.Send(Serializer_Type_A(ProtocolType.Request_EnterWorld_20));
    }



    // response
    private void Response_Login(byte[] responseData)
    {
        Scene_Main.Instance.LoginDivider(ResultCodeInstance.IsOK(Deserializer_Type_A(responseData)));
    }

    private void Response_CharacterInformation(byte[] responseData)
    {
        Scene_Lobby.Instance.NewBieChecker(Deserializer_Type_CharacterInformation(responseData));
    }

    private void Response_VerifyNickname(byte[] responseData)
    {
        Scene_Lobby.Instance.Response_ConfirmNickname(ResultCodeInstance.IsOK(Deserializer_Type_A(responseData)));
    }

    private void Response_CreateCharacter(byte[] responseData)
    {
        Scene_Lobby.Instance.Response_CreatePlayer(ResultCodeInstance.IsOK(Deserializer_Type_A(responseData)));
    }

    private void Response_EnterWorld(byte[] responseData)
    {
        Debug.Log("Response_EnterWorld.Length : " + responseData.Length);
    }


















    private void Notice_EnterWorld()
    {

    }

    private void Request_ExitWorld()
    {

    }

    private void Response_ExitWorld(byte[] responseData)
    {

    }

    private void Notice_ExitWorld()
    {

    }

    private void Request_MoveWorld()
    {

    }

    private void Response_MoveWorld(byte[] responseData)
    {

    }

    private void Notice_MoveWorld()
    {

    }

    private void Notice_SystemChatting()
    {

    }

    private void Request_GlobalChatting()
    {

    }

    private void Response_GlobalChatting(byte[] responseData)
    {

    }

    private void Notice_GlobalChatting()
    {

    }
}
