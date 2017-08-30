using Priority_Queue;

namespace SearchAlgorithmsLib.Algorithms
{
    /// <summary>
    ///     abstract class of the isearcher algorithms
    /// </summary>
    /// <typeparam name="T"> generic type</typeparam>
    /// <seealso cref="ISearcher{T}" />
    public abstract class Searcher<T> : ISearcher<T>
    {
        /// <summary>
        ///     The priority queue that contain the generic states
        /// </summary>
        private readonly SimplePriorityQueue<State<T>> _openList;

        /// <summary>
        ///     The evaluated nodes
        /// </summary>
        private int _evaluatedNodes;

        /// <summary>
        ///     constructor of the <see cref="Searcher{T}" /> class.
        /// </summary>
        protected Searcher()
        {
            _openList = new SimplePriorityQueue<State<T>>();
            _evaluatedNodes = 0;
        }

        /// <summary>
        ///     Gets the number of types that evaluated until the solution has returned.
        /// </summary>
        /// <returns>
        ///     return the number of nodes that evalueted
        /// </returns>
        public int GetNumberOfNodesEvaluated()
        {
            return _evaluatedNodes;
        }

        /// <summary>
        ///     the main search method (abstract).
        /// </summary>
        /// <param name="searchable">The searchable.</param>
        /// <returns>
        ///     return some data structure of the solution type
        /// </returns>
        public abstract ISolution<T> Search(ISearchable<T> searchable);

        /// <summary>
        ///     Pops state from the priority queue.
        /// </summary>
        /// <returns> return the state that pulled</returns>
        protected State<T> Pop()
        {
            _evaluatedNodes++;
            return _openList.Dequeue();
        }

        /// <summary>
        ///     Pushes state from the priority queue.
        /// </summary>
        /// <param name="state">The state.</param>
        /// <param name="priority">The priority in the queue</param>
        protected void Push(State<T> state, float priority)
        {
            _openList.Enqueue(state, priority);
        }

        /// <summary>
        ///     check if the priority queue is empty.
        /// </summary>
        /// <returns>
        ///     <c>true</c> if the priority queue is empty; otherwise, <c>false</c>.
        /// </returns>
        protected bool IsEmpty()
        {
            return _openList.Count == 0;
        }

        /// <summary>
        ///     Updates priority of a given state.
        /// </summary>
        /// <param name="state">The state.</param>
        protected void Update(State<T> state)
        {
            _openList.UpdatePriority(state, state.Cost);
        }

        /// <summary>
        ///     check if priority queue contains a given state.
        /// </summary>
        /// <param name="state">The state.</param>
        /// <returns>
        ///     <c>true</c> if contains the state; otherwise, <c>false</c>.
        /// </returns>
        protected bool Contains(State<T> state)
        {
            return _openList.Contains(state);
        }

        /// <summary>
        ///     create a data structure that contain the algorithm solution.
        /// </summary>
        /// <param name="state">The last state.</param>
        /// <returns>
        ///     return the data structure with the solution
        /// </returns>
        protected ISolution<T> BackTrace(State<T> state)
        {
            ISolution<T> solution = new StackSolution<T>();
            solution.Add(state);
            while (state.CameFrom != null)
            {
                state = state.CameFrom;
                solution.Add(state);
            }
            return solution;
        }

        /// <summary>
        ///     Reset the searcher.
        /// </summary>
        protected void Reset()
        {
            _openList.Clear();
            _evaluatedNodes = 0;
        }
    }
}