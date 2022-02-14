using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Text;

namespace Almost_Shortest_Path.Tools
{
    /// <summary>
    /// An implementation of a Min Heap.
    /// </summary>
    class BinaryHeap<T>
    {
        #region Members
        private HeapNode<T>[] _nodes;
        private int _size;
        private Dictionary<HeapNode<T>, int> _nodeDict;
        #endregion

        #region Constructors
        public BinaryHeap()
        {
            StartHeap(100);
        }
        #endregion

        #region Methods
        /// <summary>
        /// Inserts the item, item, with an ordering value, value, into the heap at the end of the array, then uses Heapify_Up to position the item so as to maintain the heap order.
        /// </summary>
        public void Insert(T item, int priority)
        {
            // TODO: Implement
        }

        /// <summary>
        /// Identifies the minimum element in the heap, which is located at index 1, but does not remove it.
        /// </summary>
        private T FindMin()
        {
            // TODO: Implement
            return default(T);
        }

        /// <summary>
        /// Deletes the element in the specified heap position by moving the item in the last array position to index, then using Heapify_Down to reposition that item.
        /// </summary>
        public void Delete(int index)
        {
            // TODO: Implement
        }

        /// <summary>
        /// Identifies and deletes the element with the minimum key value, located at index 1, from the heap.
        /// </summary>
        public void ExtractMin()
        {
            // TODO: Implement
        }

        /// <summary>
        /// Deletes the element item form the heap.
        /// </summary>
        public void Delete(T item)
        {
            // TODO: Implement
        }

        /// <summary>
        /// Changes the key value of element v to newPriority.
        /// </summary>
        public void ChangeKey(T item, int newPriority)
        {
            // TODO: Implement
        }

        /// <summary>
        /// Returns the index of the item provided.
        /// </summary>
        private int Position(T item)
        {
            // TODO: Implement
            return 0;
        }

        /// <summary>
        /// Moves an element located at the specified index upwards in the heap to correctly reposition an element whose value is less than the value of its parent.
        /// </summary>
        private void Heapify_Up(int index)
        {
            // TODO: Implement
        }

        /// <summary>
        /// Moves an element located at the specified index downwards in the heap to correctly reposition an element whose value is greater than the value of either of its children.
        /// </summary>
        private void Heapify_Down(int index)
        {
            // TODO: Implement
        }

        /// <summary>
        /// Initializes an empty heap that is set up to store at most N elements.
        /// </summary>
        private void StartHeap(int n)
        {
            _nodes = new HeapNode<T>[n];
            _size = 0;
            _nodeDict = new Dictionary<HeapNode<T>, int>();
        }
        #endregion

        #region Inner Classes
        class HeapNode<E> : IComparable<HeapNode<E>>
        {
            #region Fields
            public E data;
            public int priority;
            #endregion

            #region Methods
            public int CompareTo(HeapNode<E> other)
            {
                return this.priority.CompareTo(other.priority);
            }
            #endregion
        }
        #endregion
    }
}
