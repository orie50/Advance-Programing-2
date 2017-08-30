using System.IO;
using System.Text;
using Newtonsoft.Json.Linq;

namespace MessagingLib
{
    /// <summary>
    /// message reader for communication via tcp
    /// </summary>
    public class MessageReader
    {
        private readonly TextReader _reader;

        /// <summary>
        /// Initializes a new instance of the <see cref="MessageReader"/> class.
        /// </summary>
        /// <param name="textReader">The text reader.</param>
        public MessageReader(TextReader textReader)
        {
            _reader = textReader;
        }

        /// <summary>
        /// Reads the message.
        /// </summary>
        /// <returns></returns>
        public string ReadMessage()
        {
            string msg = "";
            while (_reader.Peek() != Constants.Delim)
                msg += (char) _reader.Read();
            int size = Message.ParseMessageSize(msg);
            //clear the msg string and dispose of the delimiter in the stream
            msg = "";
            _reader.Read();
            //read the msg with buffer for speed
            char[] buffer = new char[1024];
            for (; size >= 1024; size -= 1024)
            {
                _reader.ReadBlock(buffer, 0, 1024);
                msg += new string(buffer);
            }
            _reader.ReadBlock(buffer, 0, size);
            msg += new string(buffer,0, size);
            return msg;
        }

        /// <summary>
        /// Closes the underlying reader.
        /// </summary>
        public void Close()
        {
            _reader.Close();
        }
    }
}