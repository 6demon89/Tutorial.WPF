using System;
using System.Collections.Generic;
using System.Linq;
using System.Management;

namespace Wpf.COMPORT;

public delegate void ReadersListUpdatedDelegate(object sender, List<string> ports);

public class SerialManager : IDisposable
{
    /// <summary>
    /// Subscribes to temporary event notifications based on a specified event query.
    /// https://docs.microsoft.com/en-us/dotnet/api/system.management.managementeventwatcher?view=dotnet-plat-ext-5.0
    /// </summary>
    ManagementEventWatcher watcherConnect;
    ManagementEventWatcher watcherDisconnect;

    /// <summary>
    /// Represents a WMI event query in WQL format.
    /// https://docs.microsoft.com/en-us/dotnet/api/system.management.wqleventquery?view=dotnet-plat-ext-5.0
    /// </summary>
    WqlEventQuery ConnectedQuery;
    WqlEventQuery DisconnectedQuery;

    /// <summary>
    /// Collection of avalable com ports.
    /// </summary>
    private List<string> avalableports = new List<string>();
    public event ReadersListUpdatedDelegate ReadersListUpdated;

    /// <summary>
    /// Vender and Product ID string. Please note that format in which this string is written can changed with diffrent drivers
    /// </summary>
    private string VID_PID = "VID_0403+PID_6001";
    public SerialManager()
    {
        //creating WMI event query, which is checking creations of entities instances in Win32_PnPEntity
        //if your device is NOT registred in Win32_PnPEntity, you can check other entries such as Win32_SerialPort
        //this is depending on the drivers, but usually everything is in the Win32_PnPEntity
        ConnectedQuery = new WqlEventQuery("SELECT * FROM __InstanceCreationEvent WITHIN 2 WHERE TargetInstance ISA 'Win32_PnPEntity'");

        //same query, but we are checking the deletion of Win32_PnPEntiy events
        DisconnectedQuery = new WqlEventQuery("SELECT * FROM __InstanceDeletionEvent WITHIN 2 WHERE TargetInstance ISA 'Win32_PnPEntity'");

        //creating our event watcher to the queries and subscribing action to the raised event. 
        watcherConnect = new ManagementEventWatcher(ConnectedQuery);
        watcherConnect.EventArrived += WatcherConnect_EventArrived;
        watcherConnect.Start();

        watcherDisconnect = new ManagementEventWatcher(DisconnectedQuery);
        watcherDisconnect.EventArrived += WatcherDisconnect_EventArrived; ;
        watcherDisconnect.Start();
    }

    /// <summary>
    /// Accitional query over Win32_PnPEntity. This is needed to get data which is already connected to the machine.
    /// Sorting all entries by the VID_PID string and from Caption recieving the virtual port name.
    /// </summary>
    public void CheckCurrentConnectedDevice()
    {
        ManagementClass dev = new ManagementClass("Win32_PnPEntity");
        ManagementObjectCollection collection = dev.GetInstances();
        foreach (var item in collection)
        {
            if (item.GetPropertyValue("PNPDeviceID").ToString().Contains(VID_PID))
            {
                var port = new string(item.Properties["Caption"]?.Value.ToString()
                    .SkipWhile(x => x != '(').Skip(1).TakeWhile(x => x != ')').ToArray());
                if (!avalableports.Contains(port))
                    avalableports.Add(port);

            }
        }
        collection.Dispose();
        ReadersListUpdated?.Invoke(this, avalableports.ToList());
    }

    private void WatcherDisconnect_EventArrived(object sender, EventArrivedEventArgs e)
    {
        GetNewList(e, true);
    }

    private void WatcherConnect_EventArrived(object sender, EventArrivedEventArgs e)
    {
        GetNewList(e);
    }

    /// <summary>
    /// Handing when new Entity is created or deleted.
    /// Checking entier collection for our VID_PID, recieving from property Caption virtual port name
    /// checking if that port is already in our list (sometimes events can trigger multiple times, so we dont want to add or remove same entry)
    /// Checking if our operation is deleation or creation and based on the adding or removing from the list
    /// Over our event, notifing subscribers that list of avalable ports changed
    /// </summary>
    /// <param name="_event">event instance, to get instance of the ManagementObject</param>
    /// <param name="remove">device was added or removed</param>
    private void GetNewList(EventArrivedEventArgs _event, bool remove = false)
    {
        ManagementBaseObject instance = (ManagementBaseObject)_event.NewEvent["TargetInstance"];
        var port = new string(instance.Properties["Caption"]?.Value.ToString()
            .SkipWhile(x => x != '(').Skip(1).TakeWhile(x => x != ')').ToArray());
        var t = instance.Properties["PNPDeviceID"]?.Value.ToString();
        if (instance.Properties["PNPDeviceID"]?.Value.ToString().Contains(VID_PID) ?? false)
        {
            if (remove)
            {
                if (avalableports.Contains(port))
                    avalableports.Remove(port);
            }
            else
            if (!avalableports.Contains(port))
                avalableports.Add(port);
        }
        ReadersListUpdated?.Invoke(this, avalableports.ToList());
    }

    public void Dispose()
    {
        watcherConnect.Dispose();
        watcherDisconnect.Dispose();
    }
}
