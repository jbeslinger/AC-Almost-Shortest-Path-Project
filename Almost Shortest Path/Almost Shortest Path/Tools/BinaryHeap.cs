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
        public BinaryHeap(int heapSize)
        {
            StartHeap(heapSize);
        }
        #endregion

        #region Methods
        public void Insert(T item, int priority)
        {
            int newIndex = _size + 1;
            HeapNode<T> newNode = new HeapNode<T>(item, priority);
            _nodeDict.Add(newNode, newIndex);

            _nodes[newIndex] = newNode;
            HeapifyUp(newIndex);
            
            _size += 1;
        }

        public T FindMin()
        {
            return _nodes[1].data;
        }

        public void Delete(int index)
        {
            Swap(index, _size);
            _nodes[_size] = null;
            HeapifyDown(index);

            _size -= 1;
        }

        public void ExtractMin()
        {
            // TODO: Implement
        }

        public void Delete(T item)
        {
            // TODO: Implement
        }

        public void ChangeKey(T item, int newPriority)
        {
            // TODO: Implement
        }

        private int Position(HeapNode<T> item)
        {
            return _nodeDict[item];
        }

        private void HeapifyUp(int index)
        {
            int parentIdx = index / 2;
            HeapNode<T> currentObj = _nodes[index];
            while (index > 1)
            {
                HeapNode<T> parentObj = _nodes[parentIdx];
                int res = currentObj.CompareTo(parentObj);
                if (res < 0)
                {
                    Swap(index, parentIdx);
                }
                else
                {
                    break;
                }
                index = parentIdx;
                parentIdx = index / 2;
            }
        }

        private void HeapifyDown(int index)
        {
            HeapNode<T> currentObj = null;
            if (!IsEmpty())
            {
                currentObj = _nodes[index];
            }
            while (index <= _size)
            {
                HeapNode<T> leftChild = GetLeftChild(index);
                HeapNode<T> rightChild = GetRightChild(index);
                int swapIdx = -1;
                // If currentNode has a left and right child
                if (leftChild != null && rightChild != null)
                {
                    // Do the checks
                    int result = leftChild.CompareTo(rightChild);
                    if (result <= 0)
                    {
                        result = leftChild.CompareTo(currentObj);
                        if (result < 0)
                        {
                            swapIdx = index * 2;
                        }
                    }
                    else
                    {
                        result = rightChild.CompareTo(currentObj);
                        if (result < 0)
                        {
                            swapIdx = index * 2 + 1;
                        }
                    }
                    // If the currentNode has just a left child
                }
                else if (leftChild != null)
                {
                    int result = leftChild.CompareTo(currentObj);
                    if (result < 0)
                    {
                        swapIdx = index * 2;
                    }
                }

                if (swapIdx != -1)
                {
                    Swap(index, swapIdx);
                    index = swapIdx;
                }
                else
                {
                    break;
                }
            }
        }

        private void StartHeap(int n)
        {
            _nodes = new HeapNode<T>[n];
            _size = 0;
            _nodeDict = new Dictionary<HeapNode<T>, int>();
        }

        private void Swap(int idx1, int idx2)
        {
            HeapNode<T> node1 = _nodes[idx1];
            HeapNode<T> node2 = _nodes[idx2];

            _nodes[idx1] = node2;
            _nodes[idx2] = node1;

            _nodeDict[node1] = idx2;
            _nodeDict[node2] = idx1;
        }
        private HeapNode<T> GetRightChild(int index)
        {
            int rightIdx = index * 2 + 1;
            if (rightIdx <= _size)
            {
                return _nodes[rightIdx];
            }
            return null;
        }

        private HeapNode<T> GetLeftChild(int index)
        {
            int leftIdx = index * 2;
            if (leftIdx <= _size)
            {
                return _nodes[leftIdx];
            }
            return null;
        }

        private bool IsEmpty()
        {
            return _size == 0;
        }
        #endregion

        #region Override Methods
        /// <summary>
        /// Returns the heap in level order.
        /// </summary>
        public override String ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("[");
            for (int i = 1; i < _size + 1; i++)
            {
                sb.Append(_nodes[i].ToString());
                if (i < _size - 1)
                {
                    sb.Append(", ");
                }
            }
            sb.Append("]");
            return sb.ToString();
        }
        #endregion

        #region Inner Classes
        class HeapNode<E> : IComparable<HeapNode<E>>
        {
            #region Fields
            public E data;
            public int priority;
            #endregion

            #region Constructors
            public HeapNode(E data, int priority)
            {
                this.data = data;
                this.priority = priority;
            }
            #endregion

            #region Methods
            public int CompareTo(HeapNode<E> other)
            {
                return this.priority.CompareTo(other.priority);
            }
            #endregion

            #region Overrided Methods
            public override string ToString()
            {
                return String.Format("[{0},{1}]", this.data, this.priority);
            }
            #endregion
        }
        #endregion
    }
}
