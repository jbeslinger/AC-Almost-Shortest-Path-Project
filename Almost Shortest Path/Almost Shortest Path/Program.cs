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
            
            Console.WriteLine(string.Format("Now, our heap is of size {0}", heap.GetSize()));

            string key = "This";
            int newPriority = 0;

            Console.WriteLine(string.Format("Attempt to add key '{0}' at priority {1} resulted in {2}", key, newPriority, heap.Insert(key,newPriority)));

            Console.WriteLine(string.Format("The minimum priority value of this tree is '{0}'.", heap.FindMin()));

            heap.ExtractMin();

            Console.WriteLine("Smallest priority deleted.");
            
            Console.WriteLine(heap.ToString());

            Console.WriteLine(string.Format("Now it is of size {0}", heap.GetSize()));

            key = "Test";
            newPriority = 1;

            heap.ChangeKey(key, newPriority);

            Console.WriteLine(string.Format("Key '{0}' priority changed to {1}.", key, newPriority));

            Console.WriteLine(heap.ToString());

            key = "This key does not exist";
            newPriority = 999;

            Console.WriteLine(string.Format("Attempt to change key '{0}' priority to {1} resulted in {2}", key, newPriority, heap.ChangeKey(key, newPriority)));
        }
    }
}
