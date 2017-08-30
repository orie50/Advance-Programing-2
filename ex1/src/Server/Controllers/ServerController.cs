using Server.Commands;
using Server.Models;

namespace Server.Controllers
{
    /// <summary>
    ///     server controller handle the commands before the multiple game or one player game commands
    /// </summary>
    /// <seealso cref="Controller" />
    internal class ServerController : Controller
    {
        /// <summary>
        ///     constructor of the <see cref="ServerController" /> class.
        /// </summary>
        /// <param name="model">The model.</param>
        public ServerController(IModel model)
        {
            // addcommands to the base commands dicitionary
            Commands.Add("generate", new Generate(model));
            Commands.Add("solve", new Solve(model));
            Commands.Add("start", new Start(model));
            Commands.Add("join", new Join(model));
            Commands.Add("list", new List(model));
        }
    }
}