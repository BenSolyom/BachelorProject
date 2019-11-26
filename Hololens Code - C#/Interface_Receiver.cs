using UnityEngine;
#if ENABLE_WINMD_SUPPORT
using Windows.Devices.Bluetooth.Advertisement;
using System.Runtime.InteropServices.WindowsRuntime;
#endif
public class Interface_Receiver : MonoBehaviour
{
#if ENABLE_WINMD_SUPPORT
BluetoothLEAdvertisementWatcher watcher;
public static ushort BEACON_ID = 1775;
#endif
    private Decoder decoder;
    void Awake()
    {
        decoder = GameObject.Find("GameManager").GetComponent<Decoder>();
#if ENABLE_WINMD_SUPPORT
watcher = new BluetoothLEAdvertisementWatcher();
var manufacturerData = new BluetoothLEManufacturerData
{
CompanyId = BEACON_ID
};
watcher.AdvertisementFilter.Advertisement.ManufacturerData.Add(manufacturerData);
watcher.Received += Watcher_Received;
watcher.Start();
        
#endif
    }
#if ENABLE_WINMD_SUPPORT
private async void Watcher_Received(BluetoothLEAdvertisementWatcher sender, BluetoothLEAdvertisementReceivedEventArgs args)
{
ushort identifier = args.Advertisement.ManufacturerData[0].CompanyId;
    if(identifier == BEACON_ID){
        byte[] data = args.Advertisement.ManufacturerData[0].Data.ToArray();
        Debug.Log(data[0]);
        decoder.Decode(data);
    }
}
#endif
}