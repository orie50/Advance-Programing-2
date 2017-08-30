using System.IO;

namespace MessagingLib
{
    internal class MessageWriter
    {
        private readonly TextWriter _writer;

        public MessageWriter(TextWriter writer)
        {
            _writer = writer;
        }

        public void WriteMessage(string msg)
        {
            Message message = new Message(msg);
            _writer.Write(message.ToJson());
        }
    }
}