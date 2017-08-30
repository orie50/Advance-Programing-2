using Newtonsoft.Json.Linq;

namespace MessagingLib
{
    public class Message
    {
        private readonly int _msgSize;

        public Message(string msg)
        {
            _msgSize = msg.Length;
            Msg = msg;
        }

        public string Msg { get; }

        public string ToJson()
        {
            JObject msg = new JObject
            {
                ["size"] = _msgSize,
                ["message"] = Msg
            };
            return msg.ToString();
        }

        public static Message FromJson(string msg)
        {
            JObject msgJson = JObject.Parse(msg);
            string massege = (string) msgJson["message"];
            return new Message(massege);
        }
    }

    internal static class Constants
    {
        public const int StartSize = 12;
        public const int EndSize = 12;
    }
}