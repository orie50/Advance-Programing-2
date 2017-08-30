using System.Collections.Generic;

namespace SearchAlgorithmsLib.Algorithms
{
    public class DepthFirstSearch<T> : Searcher<T>
    {
        /// <summary>
        ///     the implement of the serach method forDFS algorithm.
        /// </summary>
        /// <param name="searchable">The searchable.</param>
        /// <returns>
        ///     return stack of the solution type
        /// </returns>
        public override ISolution<T> Search(ISearchable<T> searchable)
        {
            Reset();
            // get the first state and the goal state.
            State<T> root = searchable.GetInintialState();
            State<T> goal = searchable.GetGoalState();
            root.Cost = 0;
            // push the first state
            Push(root, -root.Cost);

            // while the priority queue contain any state
            while (!IsEmpty())
            {
                // get one state
                State<T> current = Pop();
                // if it's the goal state return the solution
                if (current.Equals(goal)) return BackTrace(current);

                // get all the closes states
                List<State<T>> succesors = searchable.GetAllPossibleState(current);
                foreach (State<T> s in succesors)
                {
                    // forevery close state if it dont have came from, current = came from
                    if (s.CameFrom == null && s != current) s.CameFrom = current;
                    // add s to the priority queue
                    Push(s, -s.Cost);
                }
            }
            // if cant find solution return null
            return null;
        }
    }
}