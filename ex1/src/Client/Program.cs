namespace Client
{
    /// <summary>
    ///     main method class of the client
    /// </summary>
    internal class Program
    {
        /// <summary>
        ///     Mains.
        /// </summary>
        /// <param name="args">The arguments for the main.</param>
        private static void Main(string[] args)
        {
            Client client = new Client();
            client.Start();
        }
    }
}