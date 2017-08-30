using System.Collections.Generic;
using MazeLib;
using Newtonsoft.Json.Linq;

namespace ClientGUI
{
    /// <summary>
    ///     gives serialization abilities to move msg
    /// </summary>
    internal class Move
    {

        /// <summary>
        ///     Initializes a new instance of the <see cref="Move" /> class.
        /// </summary>
        /// <param name="moveDirection">The move direction.</param>
        /// <param name="name">The name.</param>
        /// <param name="clientId">The client identifier.</param>
        public Move(Direction moveDirection, string name, int clientId = -1)
        {
            MoveDirection = moveDirection;
            ClientId = clientId;
            Name = name;
        }

        public Direction MoveDirection { get; set; }
        public string Name { get; set; }
        public int ClientId { get; set; }

        /// <summary>
        /// Returns a <see cref="System.String" /> that represents this instance.
        /// </summary>
        /// <returns>
        /// A <see cref="System.String" /> that represents this instance.
        /// </returns>
        public override string ToString()
        {
	        JObject play = new JObject
	        {
		        ["Name"] = Name,
		        ["Direction"] = MoveDirection.ToString()
	        };
	        return play.ToString();
        }

        /// <summary>
        ///     serialize Move obj to json string.
        /// </summary>
        /// <returns>
        ///     json represantation of Move.
        /// </returns>
        public string ToJson()
        {
	        JObject play = new JObject
	        {
		        ["Name"] = Name,
		        ["Direction"] = MoveDirection.ToString(),
		        ["Id"] = ClientId
	        };
	        return play.ToString();
        }

        /// <summary>
        ///     deserialize Move From json string.
        /// </summary>
        /// <param name="str">json represantation of MazeSolution.</param>
        /// <returns>
        ///     Move object
        /// </returns>
        public static Move FromJson(string str)
        {
            Dictionary<string, Direction> moves = new Dictionary<string, Direction>
            {
                {Direction.Up.ToString(), Direction.Up},
                {Direction.Down.ToString(), Direction.Down},
                {Direction.Right.ToString(), Direction.Right},
                {Direction.Left.ToString(), Direction.Left}
            };
            JObject json = JObject.Parse(str);
            string name = (string)json["Name"];
            int id = (int)json["Id"];
            return new Move(moves[(string)json["Direction"]], name, id);
        }
    }
}
