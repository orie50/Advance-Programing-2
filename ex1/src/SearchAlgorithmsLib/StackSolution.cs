using System.Collections;
using System.Collections.Generic;

namespace SearchAlgorithmsLib
{
    public class StackSolution<T> : ISolution<T>
    {
        /// <summary>
        ///     stack of the states
        /// </summary>
        private readonly Stack<State<T>> _states;

        /// <summary>
        ///     constructor of the <see cref="StackSolution{T}" /> class.
        /// </summary>
        public StackSolution()
        {
            _states = new Stack<State<T>>();
        }

        /// <summary>
        ///     push new state to the stack.
        /// </summary>
        /// <param name="state">The state.</param>
        public void Add(State<T> state)
        {
            _states.Push(state);
        }

        /// <summary>
        ///     pop the stack.
        /// </summary>
        /// <returns>
        ///     return the state the pulled from the stack
        /// </returns>
        public State<T> Get()
        {
            return _states.Pop();
        }

        /// <summary>
        ///     Returns an enumerator that iterates through the stack.
        /// </summary>
        /// <returns>
        ///     the stack enumaerator.
        /// </returns>
        public IEnumerator<State<T>> GetEnumerator()
        {
            return _states.GetEnumerator();
        }

        /// <summary>
        ///     Returns an enumerator that iterates through the stack.
        /// </summary>
        /// <returns>
        ///     the stack enumaerator.
        /// </returns>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return _states.GetEnumerator();
        }
    }
}