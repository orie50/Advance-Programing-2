namespace SearchAlgorithmsLib.Algorithms
{
    /// <summary>
    ///     interface of the searcher algorithm
    /// </summary>
    /// <typeparam name="T">
    ///     generic type
    /// </typeparam>
    public interface ISearcher<T>
    {
        /// <summary>
        ///     the main search method.
        /// </summary>
        /// <param name="searchable">The searchable.</param>
        /// <returns>
        ///     return some data structure of the solution type
        /// </returns>
        ISolution<T> Search(ISearchable<T> searchable);

        /// <summary>
        ///     Gets the number of types that evaluated until the solution has returned.
        /// </summary>
        /// <returns>
        ///     return the number of nodes that evalueted
        /// </returns>
        int GetNumberOfNodesEvaluated();
    }
}