namespace Mvvm.Maui.Models;

public class Finance
{
    public Guid id { get; set; } 
    public string Name { get; set; }
    public string Description { get; set; }
    public decimal Amount { get; set; }
    public DateTime TimeStamp { get; set; }
    public Finance()
    {
        
    }

    public Finance(Guid id, string name, string description, decimal amount, DateTime timeStamp)
    {
        this.id = id;
        Name = name;
        Description = description;
        Amount = amount;
        TimeStamp = timeStamp;
    }
}