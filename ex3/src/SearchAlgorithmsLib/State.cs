namespace SearchAlgorithmsLib
{
    /// <summary>
    ///     class of generic state.
    /// </summary>
    /// <typeparam name="T">
    ///     the type of the state
    /// </typeparam>
    public class State<T>
    {
        /// <summary>
        ///     constructor <see cref="State{T}" />.
        /// </summary>
        /// <param name="data">The data type.</param>
        public State(T data)
        {
            Data = data;
            Cost = 0;
            CameFrom = null;
        }

        /// <summary>
        ///     Gets data.
        /// </summary>
        /// <value>
        ///     The data type.
        /// </value>
        public T Data { get; }

        /// <summary>
        ///     Gets or sets the cost.
        /// </summary>
        /// <value>
        ///     The cost of the state.
        /// </value>
        public float Cost { get; set; }

        /// <summary>
        ///     Gets or sets the came from.
        /// </summary>
        /// <value>
        ///     The state that came before the current state.
        /// </value>
        public State<T> CameFrom { get; set; }

        /// <summary>
        ///     compare between 2 states.
        /// </summary>
        /// <param name="s">The state to compare with.</param>
        /// <returns>
        ///     return if the data of the states is equal
        /// </returns>
        public bool Equals(State<T> s)
        {
            return Data.Equals(s.Data);
        }

        /// <summary>
        ///     Returns a hash code for this instance.
        /// </summary>
        /// <returns>
        ///     A hash code of the string of the data.
        /// </returns>
        public override int GetHashCode()
        {
            return Data.ToString().GetHashCode();
        }
    }
}