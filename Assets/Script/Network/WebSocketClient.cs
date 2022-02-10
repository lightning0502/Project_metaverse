using System;
using UnityEngine;
using HybridWebSocket;
using System.Text;
using Assets.Script.Network.Handler;
using Assets.Script.Network;

// https://github.com/sta/websocket-sharp
public class WebSocketClient : Singleton<WebSocketClient>
{
    private MessageManager MessageManagerInstance;

    private readonly string IP = "183.100.13.54";
    private readonly string PORT = "80";
    // private readonly string SERVICE_NAME = "/Chat";
    private readonly string SERVICE_NAME = "/Service";

    private WebSocket _socket;
    private string _url;
    private bool IsInitialize;

    // public static Dispatcher Dispatcher;

    private void Awake()
    {
        IsInitialize = false;
        MessageManagerInstance = MessageManager.Instance;
        _url = "ws://" + IP + ":" + PORT + SERVICE_NAME;
    }

    private void InitializeEventListener()
    {
        if (IsInitialize)
            return;

        else
            IsInitialize = true;

        Debug.Log("socket initialize...");
        _socket = WebSocketFactory.CreateInstance(_url);
        _socket.OnOpen += OnOpen;
        _socket.OnError += OnError;
        _socket.OnClose += OnClose;
        _socket.OnMessage += OnMessage;
    }

    private void OnMessage(byte[] data)
    {
        int protocolType = -1;

        try
        {
            protocolType = BitConverter.ToInt32(data, 0);
        }

        catch (Exception error)
        {
            Debug.Log(error);
        }

        /*
            1. 연결 함수호출(connect)후 콜백으로 연결결과 리턴받음
            2. 연결 성공(onopen)이면 메시지 수신대기시작하고 연결 실패(onclose)면 연결 오류 처리
            3. 연결성공후 메시지 송신(send)은 아무때나 가능
            4. 메시지 수신대기중 언제든지 메시지를 받을 수 있음. 클라이언트가 의도치 않은 타이밍에 엉뚱한 메시지도 받을수 있음
            5. 연결종료(OnClose)는 클라이언트가 할수도 있고 다른 이유(OnError)로 원격에서 종료가 될수 있다.
            다른 이유로 연결종료 되는경우 에러처리 필요(예를 들면 재연결시도..)

            향후 수정예정 -> 캐릭터 정보 요청하고 응답받으면 응답받은 정보로 캐릭터 구성하고 다시 맵입장 메시지 보낸다음 맵입장결과 메시지받은 다음 맵에 띄운다.
        */

        Debug.LogError("protocolType : " + protocolType);
        // Dispatcher.Dispatch(this, protocolType, data);

        /*
        if (protocolType == 1)
            MessageManagerInstance.SetMessage_Chatting = data;

        else if (protocolType > 1)
            MessageManagerInstance.SetMessage_Information(protocolType, data);

        else
            Debug.LogError("OnMessage protocolType : " + protocolType);
        */
    }

    private void OnClose(WebSocketCloseCode closeCode)
    {
        Debug.Log("socket close : " + closeCode);
        // MessageManagerInstance.SetMessageQueue = "disconnected from " + _url;
    }

    private void OnError(string errorMessage)
    {
        Debug.Log("socket error : " + errorMessage);
    }

    private void OnOpen()
    {
        Debug.Log("socket open !");

        // string wallet = "0x1234567890";
        // LoginHandler.SendLogin(this, wallet);

        // 0x322Fcc2d398aa9FD3708719a8fE4077cF6C8B5fb // klaytn
        // 0xdAa9671877150b734998316b145C94aE0579FBeD // metamask

        //
    }

    public void Connect()
    {
        InitializeEventListener();

        try
        {
            _socket.Connect();
        }

        catch (Exception error)
        {
            DebugText.Instance.LogError(error);
        }
    }

    public void Send(byte[] message)
    {
        try
        {
            _socket.Send(message);
        }

        catch (Exception error)
        {
            DebugText.Instance.LogError(error);
        }
    }

    public void Send(string message)
    {
        try
        {
            Send(Encoding.UTF8.GetBytes(message));
        }

        catch (Exception error)
        {
            DebugText.Instance.LogError(error);
        }
    }

    public void Close()
    {
        try
        {
            _socket.Close();
        }

        catch (Exception error)
        {
            DebugText.Instance.LogError(error);
        }
    }
}