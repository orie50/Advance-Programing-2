using Newtonsoft.Json.Linq;

namespace MessagingLib
{
    /// <summary>
    /// tcp messege class with size header
    /// </summary>
    public class Message
    {
        private string data;

        /// <summary>
        /// Initializes a new instance of the <see cref="Message"/> class.
        /// </summary>
        /// <param name="msg">The MSG.</param>
        public Message(string msg)
        {
            data = msg;
        }

        /// <summary>
        /// Returns a <see cref="System.String" /> that represents this instance.
        /// </summary>
        /// <returns>
        /// A <see cref="System.String" /> that represents this instance.
        /// </returns>
        public override string ToString()
        {
            return "size: " + data.Length + "," + data;
        }

        /// <summary>
        /// Parses the size of the message to be read.
        /// </summary>
        /// <param name="msg">The MSG.</param>
        /// <returns></returns>
        public static int ParseMessageSize(string msg)
        {
            string[] parts = msg.Split(':');
            return int.Parse(parts[1]);
        }
    }

    /// <summary>
    /// Message constants
    /// </summary>
    internal static class Constants
    {
        /// <summary>
        /// The delimiter for the size header
        /// </summary>
        public const char Delim = ',';
    }
}