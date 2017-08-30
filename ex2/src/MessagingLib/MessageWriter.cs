using System.IO;

namespace MessagingLib
{
    /// <summary>
    /// message writer for communication via tcp
    /// </summary>
    public class MessageWriter
    {
        private readonly TextWriter _writer;

        /// <summary>
        /// Initializes a new instance of the <see cref="MessageWriter"/> class.
        /// </summary>
        /// <param name="writer">The writer.</param>
        public MessageWriter(TextWriter writer)
        {
            _writer = writer;
        }

        /// <summary>
        /// Writes the message.
        /// </summary>
        /// <param name="msg">The MSG.</param>
        public void WriteMessage(string msg)
        {            
            Message message = new Message(msg);
            _writer.Write(message.ToString());
            _writer.Flush();
        }

        /// <summary>
        /// Closes the underlying writer.
        /// </summary>
        public void Close()
        {
            _writer.Close();
        }
    }
}