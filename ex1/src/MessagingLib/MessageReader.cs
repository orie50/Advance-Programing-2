using System.IO;
using Newtonsoft.Json.Linq;

namespace MessagingLib
{
    internal class MessageReader
    {
        private readonly TextReader _reader;

        public MessageReader(TextReader textReader)
        {
            _reader = textReader;
        }

        public string ReadMessage()
        {
            string msg = "";
            while (_reader.Peek() != ',')
                msg += _reader.Read();

            int size = int.Parse(msg.Substring(Constants.StartSize, msg.Length - 3)) + Constants.EndSize;
            for (; size > 0; size--)
                msg += _reader.Read();

            return Message.FromJson(JObject.Parse(msg).ToString()).Msg;
        }
    }
}