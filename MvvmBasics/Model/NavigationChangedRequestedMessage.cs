using CommunityToolkit.Mvvm.Messaging.Messages;

namespace MvvmBasics.Model
{
    public class NavigationChangedRequestedMessage: ValueChangedMessage<NavigationModel>
    {
        public NavigationChangedRequestedMessage(NavigationModel model):base(model) { }
    }
}
