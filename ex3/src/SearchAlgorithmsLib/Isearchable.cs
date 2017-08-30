using System.Collections.Generic;

namespace SearchAlgorithmsLib
{
    /// <summary>
    /// </summary>
    /// <typeparam name="T">
    ///     generic type
    /// </typeparam>
    public interface ISearchable<T>
    {
        /// <summary>
        ///     Gets the state of the inintial.
        /// </summary>
        /// <returns>
        ///     return the initial state
        /// </returns>
        State<T> GetInintialState();

        /// <summary>
        ///     Gets the state of the goal.
        /// </summary>
        /// <returns>
        ///     return the goal state
        /// </returns>
        State<T> GetGoalState();

        /// <summary>
        ///     get a state and return all the possible state that next to ir (according the Isearchable).
        /// </summary>
        /// <param name="state">The current state.</param>
        /// <returns>
        ///     return list of the states
        /// </returns>
        List<State<T>> GetAllPossibleState(State<T> state);
    }
}