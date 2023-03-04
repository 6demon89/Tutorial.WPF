


uint[] buffer = new uint[20];
Console.WriteLine("Enter 20 single Digits");
for (uint i = 0; i < buffer.Length; i++)
{
    buffer[i] = GetDigit();
}
for (uint i = 0; i < buffer.Length; i++)
{
    Console.WriteLine($"{i} --> {buffer[i]}");
}


uint GetDigit()
{
    while (true)
    {
        var input = Console.ReadLine();
        if (uint.TryParse(input, out uint result))
            if (result < 10)
                return result;
        Console.WriteLine("Please make sure that you have entered a single digits!");
    }
}