using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Windows.Devices.Enumeration;
using Windows.Devices.Bluetooth;
using Windows.Devices.Bluetooth.GenericAttributeProfile;
using System.Reflection;
using System.Threading;
using System.Text.RegularExpressions;
using System.Diagnostics.Eventing.Reader;
using Windows.Foundation;
using System.Diagnostics;


namespace BLEConsole
{
    class Program
    {

        static DeviceInformation devicex = null;

        // static void Main(string[] args)
        static async Task Main(string[] args)
        {

            // Query for extra properties you want returned
            string[] requestedProperties = { "System.Devices.Aep.DeviceAddress", "System.Devices.Aep.IsConnected" };

            DeviceWatcher deviceWatcher =
                        DeviceInformation.CreateWatcher(
                                BluetoothLEDevice.GetDeviceSelectorFromPairingState(false),
                                requestedProperties,
                                DeviceInformationKind.AssociationEndpoint);

            // Register event handlers before starting the watcher.
            // Added, Updated and Removed are required to get all nearby devices
            deviceWatcher.Added += DeviceWatcher_Added;
            deviceWatcher.Updated += DeviceWatcher_Updated;
            deviceWatcher.Removed += DeviceWatcher_Removed;

            // EnumerationCompleted and Stopped are optional to implement.
            deviceWatcher.EnumerationCompleted += DeviceWatcher_EnumerationCompleted;
            deviceWatcher.Stopped += DeviceWatcher_Stopped;

            // Start the watcher.
            deviceWatcher.Start();

            Console.WriteLine("Searching for SR800+. Please turn ON the device ....");

            while ( true )
            {
                if ( devicex == null )
                {
                    Thread.Sleep(500);
                }
                else
                {
                    Console.WriteLine("Connecting to the device ....");
                    BluetoothLEDevice bluetoothLeDevice = await BluetoothLEDevice.FromIdAsync(devicex.Id);
                    // bluetoothLeDevice.

                    Thread.Sleep(1000);
                    Console.WriteLine("Connection status is ..... " + bluetoothLeDevice.ConnectionStatus);

                    /*
                     * this is working code
                    var device = await BluetoothLEDevice.FromIdAsync(devicex.Id);
                    // Console.WriteLine($"BLEWATCHER Found: {device.name}");
                    // SERVICES!!
                    var gatt = await device.GetGattServicesAsync();
                    Console.WriteLine($"{device.Name} Services: {gatt.Services.Count}, {gatt.Status}, {gatt.ProtocolError}");

                    */

                    // below pairing didn't work-
                    // var pairingResult = await bluetoothLeDevice.DeviceInformation.Pairing.Custom.PairAsync(DevicePairingKinds.ProvidePin, DevicePairingProtectionLevel.None);
                    // Console.WriteLine(pairingResult.ToString());

                    // device.Pairing.ProtectionLevel = DevicePairingProtectionLevel.EncryptionAndAuthentication;





                    // DevicePairingResult pairingResult = await device.Pairing.PairAsync(DevicePairingProtectionLevel.EncryptionAndAuthentication);

                    /*
                    device.Pairing.Custom.PairingRequested += CustomOnPairingRequested;

                    var result = await device.Pairing.Custom.PairAsync(
                          DevicePairingKinds.ConfirmOnly, DevicePairingProtectionLevel.None);
                    device.Pairing.Custom.PairingRequested -= CustomOnPairingRequested;
                    */

                    /*
                    if (pairingResult.Status == DevicePairingResultStatus.Paired)
                    {
                        Console.WriteLine(pairingResult.Status);
                        Console.WriteLine("Pairing successful");
                    }
                    else
                    {
                        Console.WriteLine(pairingResult.Status);
                        Console.WriteLine("Pairing failed");
                    }
                    */


                    GattDeviceServicesResult result = await bluetoothLeDevice.GetGattServicesAsync();

                    if (result.Status == GattCommunicationStatus.Success)
                    {
                        var services = result.Services;
                        foreach (var service in services)
                        {
                            Console.WriteLine(service.Uuid);
                        }
                    }
                    Console.WriteLine("Connection status is ..... " + bluetoothLeDevice.ConnectionStatus);

                    /*        
                    var customPairing = devicex.Pairing.Custom;

                    var result2 = await customPairing.PairAsync(DevicePairingKinds.None, DevicePairingProtectionLevel.Encryption);

                    Console.WriteLine(result2);
                    if (result2.Status == DevicePairingResultStatus.Paired)
                    {
                        Console.WriteLine("Device paired successfully!");
                    }
                    else
                    {
                        Console.WriteLine($"Pairing failed. Status: {result2.Status}");
                    }
                    */

                    break;
                }
            }

            while ( true )
            {
                Console.WriteLine("Press any key to exit");
                Console.ReadKey();
            }

        }

        private static void DeviceWatcher_Stopped(DeviceWatcher sender, object args)
        {
            // throw new NotImplementedException();
        }

        private static void DeviceWatcher_EnumerationCompleted(DeviceWatcher sender, object args)
        {
            // throw new NotImplementedException();
        }

        private static void DeviceWatcher_Removed(DeviceWatcher sender, DeviceInformationUpdate args)
        {
            // throw new NotImplementedException();
        }

        private static void DeviceWatcher_Updated(DeviceWatcher sender, DeviceInformationUpdate args)
        {
            // throw new NotImplementedException();
        }

        private static async void DeviceWatcher_Added(DeviceWatcher sender, DeviceInformation args)
        {
             
       
            var id = args.Id;
            var bluetoothLEDevice = await BluetoothLEDevice.FromIdAsync(id);
            GattDeviceServicesResult gatServiceAsync = await bluetoothLEDevice.GetGattServicesAsync(BluetoothCacheMode.Uncached);

            if (gatServiceAsync.Status == GattCommunicationStatus.Success && args.Name == "SR800+" && devicex == null )
            {
                devicex = args;
                Console.WriteLine(args.Name + " device found");
            }
           

            /*
            if (args.Name == "SR800+") 
            {
                devicex = args;
                Console.WriteLine(args.Name+" found from Windows cache");
            }
            */

            // throw new NotImplementedException();
        }
    }
} 
