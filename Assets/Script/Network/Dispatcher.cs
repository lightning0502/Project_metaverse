using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Script.Network
{
    public class Dispatcher
    {
        Dictionary<int, IMessageHandler> _handlers = new Dictionary<int, IMessageHandler>();

        internal void Dispatch(WebSocketClient session, int messageType, byte[] data)
        {
            IMessageHandler handler;
            if (_handlers.TryGetValue(messageType, out handler) == false)
            {
                Debug.Log("not found message num : " + messageType);
                return;
            }

            handler.HandleMessage(session, data);
        }

        /// <summary>
        /// 어셈블리(dll)에서 구현된 메시지 핸들러를 등록합니다. 추가적으로 현재 호출되는 어셈블리(exe)의 메시지 핸들러도 같이 등록됩니다.
        /// </summary>
        /// <param name="assemblyFiles"></param>
        public void RegistHandler(string[] assemblyFiles)
        {
            foreach (string assemblyFile in assemblyFiles)
            {
                Assembly assembly = Assembly.LoadFrom(assemblyFile);
                if (object.ReferenceEquals(assembly, null))
                    throw new ArgumentException("file not found : " + assemblyFile);

                Regist(assembly);
            }

            Regist(Assembly.GetCallingAssembly());
        }

        /// <summary>
        /// 현재 호출되는 어셈블리의 메시지 핸들러를 등록합니다.
        /// </summary>
        public void RegistHandler()
        {
            Regist(Assembly.GetCallingAssembly());
        }

        void Regist(Assembly assembly)
        {
            Type[] allTypes = assembly.GetTypes();
            foreach (Type t in allTypes)
            {
                if (t.IsClass == false || t.IsAbstract == true)
                    continue;

                Type t2 = t.GetInterface("Assets.Script.Network.IMessageHandler");
                if (t2 == null)
                    continue;

                IMessageHandler handler = Activator.CreateInstance(t) as IMessageHandler;
                if (_handlers.ContainsKey(handler.MessageType) == false)
                    _handlers.Add(handler.MessageType, handler);
                else
                    throw new ArgumentException("handler exist. num : " + handler.MessageType);
            }
        }
    }
}
