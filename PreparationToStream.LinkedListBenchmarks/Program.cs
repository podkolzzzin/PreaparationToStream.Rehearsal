// See https://aka.ms/new-console-template for more information
List<string> list = new List<string>();
try
{
    while (true)
    {
        var str = Guid.NewGuid().ToString();
        list.Add(str);
        if (list.Count % 100_000 == 0)
            Console.WriteLine(list.Count);
    }

}
finally
{
    Console.WriteLine("Finally: " + list.Count);
}