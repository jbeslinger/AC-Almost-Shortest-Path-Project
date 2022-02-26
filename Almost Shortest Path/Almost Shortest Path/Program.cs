using System;

namespace Almost_Shortest_Path
{
    class Program
    {
        static void Main(string[] args)
        {
            Tools.BinaryHeap<string> heap = new Tools.BinaryHeap<string>(100);

            Console.WriteLine(heap.ToString());

            Console.WriteLine(string.Format("Our empty heap is of size {0}", heap.GetSize()));

            Console.WriteLine("Let's add some values:");

            heap.Insert("This", 1);
            heap.Insert("Is", 2);
            heap.Insert("A", 3);
            heap.Insert("Min", 4);
            heap.Insert("Heap", 5);
            heap.Insert("Test", 6);

            Console.WriteLine(heap.ToString());
        }
    }
}
