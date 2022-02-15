using System;

namespace Almost_Shortest_Path
{
    class Program
    {
        static void Main(string[] args)
        {
            Tools.BinaryHeap<string> heap = new Tools.BinaryHeap<string>(100);

            heap.Insert("Hello", 2);
            heap.Insert("World", 3);
            heap.Insert("Up", 6);
            heap.Insert("Whats", 5);
            heap.Insert("Doc", 4);
            heap.Insert("Doc", 7);
            heap.Insert("Bunny", 1);

            Console.WriteLine(heap.ToString());
            Console.WriteLine(string.Format("The minimum priority value of this tree is {0}.", heap.FindMin()));
            heap.Delete(1);
            Console.WriteLine(heap.ToString());
        }
    }
}
