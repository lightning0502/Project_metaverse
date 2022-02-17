using System;
using UnityEngine;
using HybridWebSocket;
using System.Text;
using System.Threading;
using System.Collections;

// https://github.com/sta/websocket-sharp
public class WebSocketClient : Singleton<WebSocketClient>
{
    // readonly
    private readonly string IP = "183.100.13.54";
    private readonly string PORT = "80";
    // private readonly string SERVICE_NAME = "/Chat";
    private readonly string SERVICE_NAME = "/Service";

    // values
    private ActionTrigger ActionTriggerInstance;
    private WebSocket WebSocketInstance;
    private string _url;
    private bool IsInitialize;
    private SynchronizationContext SyncContext;

    private void Awake()
    {
        IsInitialize = false;
        ActionTriggerInstance = ActionTrigger.Instance;
        _url = "ws://" + IP + ":" + PORT + SERVICE_NAME;
    }

    private void WebSocketConnector()
    {
        if (IsInitialize)
            return;

        else
            IsInitialize = true;

        Debug.Log("socket initialize...");
        SyncContext = SynchronizationContext.Current;
        WebSocketInstance = WebSocketFactory.CreateInstance(_url);
        WebSocketInstance.OnOpen += OnOpen;
        WebSocketInstance.OnError += OnError;
        WebSocketInstance.OnClose += OnClose;
        WebSocketInstance.OnMessage += OnMessage;
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

        Debug.Log("on message protocolType : " + protocolType);

        if (Application.platform == RuntimePlatform.WindowsEditor)
        {
            ThreadPool.QueueUserWorkItem(delegate
            {
                SyncContext.Post(delegate
                {
                    ActionTriggerInstance.OnResponseTrigger(protocolType, data);
                }, null);
            });
        }

        else
            ActionTriggerInstance.OnResponseTrigger(protocolType, data);
    }

    private void OnClose(WebSocketCloseCode closeCode)
    {
        Debug.LogError("socket close : " + closeCode);
        // TO DO :: restart application
    }

    private void OnError(string errorMessage)
    {
        Debug.LogError("socket error : " + errorMessage);
    }

    private void OnOpen()
    {
        Debug.Log("socket open !");

        if (Application.platform == RuntimePlatform.WindowsEditor)
        {
            ThreadPool.QueueUserWorkItem(delegate
            {
                SyncContext.Post(delegate
                {
                    ActionTriggerInstance.OnRequestTrigger(ProtocolType.Request_Login_0);
                }, null);
            });
        }

        else // will call to ActionTriggerInstance.OnActionTrigger(ProtocolType.Request_Login);
            StartCoroutine(JavaScriptLibrary.Instance.WaitingForAccountWalletLogin());
    }

    public void Connect()
    {
        WebSocketConnector();

        try
        {
            WebSocketInstance.Connect();
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
            WebSocketInstance.Send(message);
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

    private void OnApplicationQuit()
    {
        try
        {
            WebSocketInstance.Close();
        }

        catch (Exception error)
        {
            DebugText.Instance.LogError(error);
        }
    }

    /*
    public void Close()
    {

    }
    */
}