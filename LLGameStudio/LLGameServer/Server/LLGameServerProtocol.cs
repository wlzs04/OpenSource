using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LLGameServer.Server
{
    class LLGameServerProtocol: LLGameProtocol
    {
        public LLGameServerProtocol(string name) :base(name)
        {
        }

        public void AddContent(string key, string value)
        {
            contentMap[key] = value;
        }

        public void SendContentBySocket()
        {
            byte[] msg = Encoding.UTF8.GetBytes(ExportContentToWString());
            msg = DecodeProtocol(msg);
            socket.Send(msg);
        }

        public byte[] DecodeProtocol(byte[] content)
        {
            List<byte> byteSource = new List<byte>();
            byteSource.AddRange(Encoding.UTF8.GetBytes(String.Format("{0:0000}", content.Length)));
            byteSource.AddRange(content);
            return byteSource.ToArray();
        }
    }
}
