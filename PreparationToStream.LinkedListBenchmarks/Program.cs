// See https://aka.ms/new-console-template for more information
List<int> list = new List<int>();
try
{
    while (true)
    {
        list.Add(0);
        if (list.Count % 100_000 == 0)
            Console.WriteLine(list.Count);
    }

}
finally
{
    Console.WriteLine("Finally: " + list.Count);
}