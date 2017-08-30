using System.Collections.Generic;

namespace SearchAlgorithmsLib
{
    /// <summary>
    ///     interface of the solution data structure
    /// </summary>
    /// <typeparam name="T"> generic type</typeparam>
    public interface ISolution<T> : IEnumerable<State<T>>
    {
        /// <summary>
        ///     Adds the state to the data structure.
        /// </summary>
        /// <param name="state">The state.</param>
        void Add(State<T> state);

        /// <summary>
        ///     Get state from the data structure.
        /// </summary>
        /// <returns>
        ///     return the state
        /// </returns>
        State<T> Get();
    }
}