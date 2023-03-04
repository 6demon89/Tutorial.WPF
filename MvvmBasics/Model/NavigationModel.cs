using CommunityToolkit.Mvvm.ComponentModel;

namespace MvvmBasics.Model;

public class NavigationModel
{
    public int ID { get; set; }
    public string? Name { get; set; }
    public string? Description { get; set; }
    public ObservableObject? DestinationVM { get; set; }
}
