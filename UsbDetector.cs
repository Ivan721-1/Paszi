using System;
using System.Collections.Generic;
using System.Management;

namespace RoleAuthenticationApp
{
    public class UsbDetector
    {
        public string GetUsbSerialNumber()
        {
            // Сначала пробуем найти USB флешки
            var usbDrive = GetUsbDriveSerial();
            if (!string.IsNullOrEmpty(usbDrive))
                return usbDrive;

            // Если флешек нет, ищем USB мыши
            var usbMouse = GetUsbMouseSerial();
            if (!string.IsNullOrEmpty(usbMouse))
                return usbMouse;

            // Если и мышей нет, ищем любые USB устройства
            return GetAnyUsbDeviceSerial();
        }

        private string GetUsbDriveSerial()
        {
            try
            {
                var searcher = new ManagementObjectSearcher(
                    "SELECT * FROM Win32_DiskDrive WHERE InterfaceType='USB'");
                foreach (ManagementBaseObject mbo in searcher.Get())
                {
                    var disk = (ManagementObject)mbo;
                    if (disk["PNPDeviceID"] is string pid)
                    {
                        var parts = pid.Split('\\');
                        if (parts.Length > 1)
                            return $"DRIVE_{parts[1]}";
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"USB drive detection error: {ex.Message}");
            }
            return null;
        }

        private string GetUsbMouseSerial()
        {
            try
            {
                var searcher = new ManagementObjectSearcher(
                    "SELECT * FROM Win32_PointingDevice WHERE PNPDeviceID LIKE 'USB%'");
                foreach (ManagementBaseObject mbo in searcher.Get())
                {
                    var mouse = (ManagementObject)mbo;
                    if (mouse["PNPDeviceID"] is string pid)
                    {
                        var parts = pid.Split('\\');
                        if (parts.Length > 1)
                            return $"MOUSE_{parts[1]}";
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"USB mouse detection error: {ex.Message}");
            }
            return null;
        }

        private string GetAnyUsbDeviceSerial()
        {
            try
            {
                var searcher = new ManagementObjectSearcher(
                    "SELECT * FROM Win32_PnPEntity WHERE PNPDeviceID LIKE 'USB%'");
                foreach (ManagementBaseObject mbo in searcher.Get())
                {
                    var device = (ManagementObject)mbo;
                    if (device["PNPDeviceID"] is string pid && device["Name"] is string name)
                    {
                        var parts = pid.Split('\\');
                        if (parts.Length > 1)
                            return $"USB_{parts[1]}";
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"USB device detection error: {ex.Message}");
            }
            return null;
        }

        public List<string> GetAllUsbDevices()
        {
            var devices = new List<string>();
            try
            {
                var searcher = new ManagementObjectSearcher(
                    "SELECT * FROM Win32_PnPEntity WHERE PNPDeviceID LIKE 'USB%'");
                foreach (ManagementBaseObject mbo in searcher.Get())
                {
                    var device = (ManagementObject)mbo;
                    if (device["PNPDeviceID"] is string pid && device["Name"] is string name)
                    {
                        var parts = pid.Split('\\');
                        if (parts.Length > 1)
                            devices.Add($"{name}: {parts[1]}");
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"USB devices enumeration error: {ex.Message}");
            }
            return devices;
        }
    }
}
