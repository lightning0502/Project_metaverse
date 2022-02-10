using System.Text;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MessageManager : Singleton<MessageManager>
{
    // web Socket
    private WebSocketClient WebSocketInstance;
    private ChattingUI ChattingUIInstance;
    private PlayerManager PlayerInformationInstance;

    // values
    private BinaryWriter Writer;
    private BinaryReader Reader;
    private PlayerInformation TemporaryInformation;

    // message handler
    private Queue<PlayerInformation> MessageQueue_PlayerInformation;
    private Queue<byte[]> MessageQueue_ChattingText;
    public byte[] SetMessage_Chatting
    {
        set
        {
            MessageQueue_ChattingText.Enqueue(value);
        }
    }

    private enum ProtocolType : int
    {
        NONE = -1,

        Chatting = 1,

        PlayerEnter = 2,
        PlayerMove = 3,
        PlayerExit = 4,
    }

    private void Awake()
    {
        WebSocketInstance = WebSocketClient.Instance;
        PlayerInformationInstance = PlayerManager.Instance;
        MessageQueue_ChattingText = new Queue<byte[]>();
        MessageQueue_PlayerInformation = new Queue<PlayerInformation>();
    }

    public void StartMessageChecker()
    {
        ChattingUIInstance = ChattingUI.Instance;
        StartCoroutine(MessageCheckCoroutine_Chatting());
        StartCoroutine(MessageCheckCoroutine_PlayerInformation());
    }

    public void SetMessage_Information(int protocolType, byte[] byteData)
    {
        switch ((ProtocolType)protocolType)
        {
            default:
            case ProtocolType.NONE:
            case ProtocolType.Chatting:
                DebugText.Instance.LogError("Error -> MessageCheckCoroutine_Information protocolType : ", protocolType);
                return;

            case ProtocolType.PlayerEnter: // 캐릭터 입장
            case ProtocolType.PlayerMove: // 이동
            case ProtocolType.PlayerExit: // 로그아웃
                MessageQueue_PlayerInformation.Enqueue(GetPlayerInformation(byteData));
                break;
        }
    }

    private IEnumerator MessageCheckCoroutine_Chatting()
    {
        int offset_8 = 8;

        while (gameObject.activeSelf)
        {
            if (MessageQueue_ChattingText.Count == 0)
                yield return Coop.WaitForSeconds(1);

            else
            {
                byte[] data = MessageQueue_ChattingText.Dequeue();
                ChattingUIInstance.SetChattingText(Encoding.UTF8.GetString(data, offset_8, data.Length - offset_8));

                yield return Coop.WaitForSeconds(0.2f);
            }
        }
    }

    private IEnumerator MessageCheckCoroutine_PlayerInformation()
    {
        while (gameObject.activeSelf)
        {
            if (MessageQueue_PlayerInformation.Count == 0)
                yield return Coop.WaitForSeconds(1);

            else
            {
                PlayerInformationInstance.InformationDivider(MessageQueue_PlayerInformation.Dequeue());
                yield return Coop.WaitForSeconds(0.05f);
            }
        }
    }

    public void ToArray_Chatting(string message)
    {
        using (Writer = new BinaryWriter(new MemoryStream()))
        {
            Writer.Write(1); // ProtocolType.Chatting
            byte[] byteData = Encoding.UTF8.GetBytes(message);
            Writer.Write(byteData.Length);
            Writer.Write(byteData);

            WebSocketInstance.Send(((MemoryStream)Writer.BaseStream).ToArray());
        }
    }

    private PlayerInformation GetPlayerInformation(byte[] byteData)
    {
        using (Reader = new BinaryReader(new MemoryStream(byteData)))
        {
            // protocol type
            TemporaryInformation.ProtocolType = Reader.ReadInt32();

            // information
            TemporaryInformation.Player_ID = Reader.ReadInt32();

            // flag exit
            if (TemporaryInformation.ProtocolType == 4)
                return TemporaryInformation;

            else if (TemporaryInformation.ProtocolType == 2)
            {
                int dataLength = Reader.ReadInt32();
                byte[] nameData = Reader.ReadBytes(dataLength);
                TemporaryInformation.Player_Nickname = Encoding.UTF8.GetString(nameData);
            }

            else
                TemporaryInformation.Player_Nickname = string.Empty;

            // position
            TemporaryInformation.Player_XPosition = Reader.ReadSingle();
            TemporaryInformation.Player_YPosition = Reader.ReadSingle();

            return TemporaryInformation;
        }
    }

    public void SendInformation(int id, Vector2 localPosition)
    {
        using (Writer = new BinaryWriter(new MemoryStream()))
        {
            Writer.Write(3); // ProtocolType.PlayerMove
            Writer.Write(id);
            Writer.Write(localPosition.x);
            Writer.Write(localPosition.y);

            WebSocketInstance.Send(((MemoryStream)(Writer.BaseStream)).ToArray());
        }
    }
}