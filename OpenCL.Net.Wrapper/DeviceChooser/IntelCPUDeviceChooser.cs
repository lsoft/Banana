﻿using System;
using System.Linq;


namespace OpenCL.Net.Wrapper.DeviceChooser
{
    public class IntelCPUDeviceChooser : IDeviceChooser
    {
        private readonly bool _showSelectedVendor;

        public IntelCPUDeviceChooser(bool showSelectedVendor = true)
        {
            _showSelectedVendor = showSelectedVendor;
        }

        public void ChooseDevice(
            out DeviceType choosedDeviceType,
            out Device choosedDevice)
        {
            ErrorCode error;

            var platforms = Cl.GetPlatformIDs(out error);
            if (error != ErrorCode.Success)
            {
                throw new OpenCLException(
                    string.Format(
                        "Unable to retrieve an OpenCL Device, error code is: {0}!",
                        error));
            }

            foreach (var platform in platforms)
            {
                var deviceIds = Cl.GetDeviceIDs(platform, DeviceType.Cpu, out error);
                if (deviceIds.Any())
                {
                    foreach (var device in deviceIds)
                    {
                        //var vendor = Cl.GetDeviceInfo(device, DeviceInfo.Vendor, out error).ToString();
                        //if (vendor.ToUpper().Contains("INTEL"))

                        var vendorId = Cl.GetDeviceInfo(device, DeviceInfo.VendorId, out error).CastTo<int>();
                        if (vendorId == CLParameters.IntelVendorId)
                        {
                            if (_showSelectedVendor)
                            {
                                var deviceInfo = Cl.GetDeviceInfo(device, DeviceInfo.Name, out error).ToString();

                                Console.WriteLine(
                                    "Choosed device: {0}",
                                    deviceInfo);
                            }

                            choosedDevice = deviceIds.First();
                            choosedDeviceType = DeviceType.Cpu;
                            return;
                        }
                    }
                }
            }

            throw new OpenCLException("There is no Intel CPU device");
        }
    }
}
