using System.Collections.Generic;

namespace SearchAlgorithmsLib.Algorithms
{
    /// <summary>
    ///     encapsulate the BFS algorithm
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <seealso cref="Searcher{T}" />
    public class BestFirstSearch<T> : Searcher<T>
    {
        /// <summary>
        ///     the main search method.
        /// </summary>
        /// <param name="searchable">The searchable.</param>
        /// <returns>
        ///     return the search path in a ISolution data structure
        /// </returns>
        public override ISolution<T> Search(ISearchable<T> searchable)
        {
            Reset();
            //quick acsses states pool for the states in the open queue
            Dictionary<int, State<T>> states = new Dictionary<int, State<T>>();
            HashSet<State<T>> closed = new HashSet<State<T>>();

            State<T> current = searchable.GetInintialState();
            State<T> goal = searchable.GetGoalState();

            current.Cost = 0;
            Push(current, current.Cost);

            while (!IsEmpty())
            {
                current = Pop();
                closed.Add(current);
                states.Remove(current.GetHashCode());

                if (current.Equals(goal)) return BackTrace(current);

                List<State<T>> succesors = searchable.GetAllPossibleState(current);
                foreach (State<T> s in succesors)
                    if (!closed.Contains(s) && !Contains(s))
                    {
                        Push(s, s.Cost);
                        states.Add(s.GetHashCode(), s);
                    }
                    else
                    {
                        State<T> lastPath;
                        if (states.TryGetValue(s.GetHashCode(), out lastPath))
                            if (s.Cost < lastPath.Cost)
                            {
                                lastPath.Cost = s.Cost;
                                lastPath.CameFrom = s.CameFrom;
                                Update(lastPath);
                            }
                    }
            }
            return null;
        }
    }
}