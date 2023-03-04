namespace MvvmBasics.Model;

public class DataModel
{
    public int ID { get; set; }
    public string? Name { get; set; }
    public byte[]? Raw { get; set; }
    public DataModel(int iD, string? name, byte[]? raw)
    {
        ID = iD;
        Name = name;
        Raw = raw;
    }
}
