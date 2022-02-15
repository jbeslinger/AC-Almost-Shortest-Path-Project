using System;
using System.Collections.Generic;
using System.Text;

namespace Almost_Shortest_Path.Tools
{
    class PriorityQueue<T>
    {
        #region Constants
        private const int QUEUE_SIZE = 256;
        #endregion

        #region Members
        private BinaryHeap<T> _heap;
        #endregion

        #region Constructors
        public PriorityQueue()
        {
            _heap = new BinaryHeap<T>(QUEUE_SIZE);
        }
        #endregion

        #region Methods
        public bool Enqueue(T item, int priority)
        {
            return _heap.Insert(item, priority);
        }

        public T Dequeue()
        {
            return _heap.ExtractMin();
        }

        public void Clear()
        {
            _heap = new BinaryHeap<T>(QUEUE_SIZE);
        }

        public bool IsEmpty()
        {
            return _heap.GetSize() == 0;
        }

        public int GetSize()
        {
            return _heap.GetSize();
        }
        #endregion
    }
}
