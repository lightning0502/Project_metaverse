using System;
using System.IO;
using System.Text;
using UnityEngine;

namespace Assets.Script.Network.Handler
{
    public class LoginRequest : IBinarySerializer
    {
        public int MessageType { set; get; }
        public string Wallet { set; get; }

        public byte[] Serialize()
        {
            using (BinaryWriter writer = new BinaryWriter(new MemoryStream()))
            {
                writer.Write(MessageType);

                byte[] temp = Encoding.UTF8.GetBytes(Wallet);

                writer.Write((short)temp.Length);
                writer.Write(temp);

                return (((MemoryStream)(writer.BaseStream)).ToArray());
            }
        }
    }

    public class LoginResponse
    {
        public int MessageType { set; get; }
        public byte ResultCode { set; get; }

        public static LoginResponse Create(byte[] data)
        {
            LoginResponse response = new LoginResponse();
            using (BinaryReader reader = new BinaryReader(new MemoryStream(data)))
            {
                response.MessageType = reader.ReadInt32();
                response.ResultCode = reader.ReadByte();
            }

            return response;
        }
    }

    public class LoginHandler : IMessageHandler
    {
        public int MessageType
        {
            get { return (int)ProtocolType.Request_Login; }
        }

        public static void SendLogin(WebSocketClient session, string wallet)
        {
            LoginRequest request = new LoginRequest();
            request.MessageType = (int)ProtocolType.Request_Login;
            request.Wallet = wallet;

            byte[] data = request.Serialize();
            session.Send(data);
        }

        public void HandleMessage(WebSocketClient session, byte[] message)
        {
            try
            {
                LoginResponse response = LoginResponse.Create(message);
                if (ResultCode.Instance.IsOK(response.ResultCode))
                {
                    // todo : 인증성공
                }

                else
                {
                    // todo : 오류 처리
                }
            }

            catch (Exception e2)
            {
                Debug.Log(e2);
            }
        }
    }
}