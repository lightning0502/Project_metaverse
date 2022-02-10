using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Script.Network
{
    public interface IMessageHandler
    {
        int MessageType { get; }
        void HandleMessage(WebSocketClient session, byte[] message);
    }
}
