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
        ActionDictionary_Request.Add(ProtocolType.Request_Login, Request_Login);
        ActionDictionary_Request.Add(ProtocolType.Request_CharacterInformation, Request_CharacterInformation);
        ActionDictionary_Request.Add(ProtocolType.Request_VerifyNickname, Request_VerifyNickname);
        ActionDictionary_Request.Add(ProtocolType.Request_CreateCharacter, Request_CreateCharacter);
        ActionDictionary_Request.Add(ProtocolType.Request_EnterWorld, Request_EnterWorld);
        ActionDictionary_Request.Add(ProtocolType.Request_ExitWorld, Request_ExitWorld);
        ActionDictionary_Request.Add(ProtocolType.Request_MoveWorld, Request_MoveWorld);
        ActionDictionary_Request.Add(ProtocolType.Request_GlobalChatting, Request_GlobalChatting);

        // response
        ActionDictionary_Response = new Dictionary<int, Action<byte[]>>();
        ActionDictionary_Response.Add(ProtocolType.Response_Login, Response_Login);
        ActionDictionary_Response.Add(ProtocolType.Response_CharacterInformation, Response_CharacterInformation);
        ActionDictionary_Response.Add(ProtocolType.Response_VerifyNickname, Response_VerifyNickname);
        ActionDictionary_Response.Add(ProtocolType.Response_CreateCharacter, Response_CreateCharacter);
        ActionDictionary_Response.Add(ProtocolType.Response_EnterWorld, Response_EnterWorld);
        ActionDictionary_Response.Add(ProtocolType.Response_ExitWorld, Response_ExitWorld);
        ActionDictionary_Response.Add(ProtocolType.Response_MoveWorld, Response_MoveWorld);
        ActionDictionary_Response.Add(ProtocolType.Response_GlobalChatting, Response_GlobalChatting);

        // notice
        ActionDictionary_Request.Add(ProtocolType.Notice_EnterWorld, Notice_EnterWorld);
        ActionDictionary_Request.Add(ProtocolType.Notice_ExitWorld, Notice_ExitWorld);
        ActionDictionary_Request.Add(ProtocolType.Notice_MoveWorld, Notice_MoveWorld);
        ActionDictionary_Request.Add(ProtocolType.Notice_SystemChatting, Notice_SystemChatting);
        ActionDictionary_Request.Add(ProtocolType.Notice_GlobalChatting, Notice_GlobalChatting);
    }

    public void OnTrigger(int type, byte[] responseData)
    {
        if (ActionDictionary_Response.ContainsKey(type) == false)
        {
            DebugText.Instance.LogError("Error -> OnActionTrigger ActionDictionary_Response.ContainsKey() false");
            return;
        }

        else
            ActionDictionary_Response[type](responseData);
    }

    public void OnTrigger(int type)
    {
        if (ActionDictionary_Request.ContainsKey(type) == false)
        {
            DebugText.Instance.LogError("Error -> OnActionTrigger ActionDictionary_Request.ContainsKey() false");
            return;
        }

        else
            ActionDictionary_Request[type]();
    }

    private byte Deserializer_Login(in byte[] responseData)
    {
        using (Reader = new BinaryReader(new MemoryStream(responseData)))
        {
            Reader.ReadInt32();
            return Reader.ReadByte();
        }
    }

    private byte[] Serializer_Login(in int protocol, in string walletID)
    {
        using (Writer = new BinaryWriter(new MemoryStream()))
        {
            Writer.Write(protocol);

            byte[] id = Encoding.UTF8.GetBytes(walletID);

            Writer.Write((short)id.Length);
            Writer.Write(id);

            return (((MemoryStream)(Writer.BaseStream)).ToArray());
        }
    }

    private void Request_Login()
    {
        WebSocketInstance.Send(Serializer_Login(ProtocolType.Request_Login, JavaScriptLibraryInstance.GetCurrentAccountID));
    }

    private void Response_Login(byte[] responseData)
    {
        Scene_Main.Instance.LoginDivider(ResultCodeInstance.IsOK(Deserializer_Login(responseData)));
    }

    private void Request_CharacterInformation()
    {

    }

    private void Response_CharacterInformation(byte[] responseData)
    {

    }

    private void Request_VerifyNickname()
    {

    }

    private void Response_VerifyNickname(byte[] responseData)
    {

    }

    private void Request_CreateCharacter()
    {

    }

    private void Response_CreateCharacter(byte[] responseData)
    {

    }

    private void Request_EnterWorld()
    {

    }

    private void Response_EnterWorld(byte[] responseData)
    {

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
