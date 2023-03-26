using System;
using System.Buffers.Binary;
using System.Collections.Generic;

namespace EnOcean.BLE.Decoder
{
    public class STM550BEncoder : IEnOceanBLEEncoder
    {
        public enum SensorDataDescriptor : byte
        {
            TEMPERATURE = 0,
            VOLTAGE = 1,
            ENERGY_LEVEL = 2,
            ILLUMINATION_CELL = 4,
            ILLUMINATION_SENSOR = 5,
            HUMIDITY = 6,
            ACCELERATION = 10,
            MAGNET = 35,
            OPTIONAL_DATA = 60
        }

        record SensorDescriptor(SensorDataDescriptor desciprot, byte datalength);

        private SensorDescriptor GetDescriptor(byte desciptorByte)
        {
            byte data = (byte)(desciptorByte & 0x3F);
            byte dataLength = (byte)(desciptorByte & 0xC0);
            SensorDataDescriptor descriptor = (SensorDataDescriptor)data;
            //Checking which of the first to bits are enabled
            if ((dataLength & 0x80) == 0x80)
                return new SensorDescriptor(descriptor, 4);
            if ((dataLength & 0x40) == 0x40)
                return new SensorDescriptor(descriptor, 2);
            return new SensorDescriptor(descriptor, 1);
        }

        public ICollection<TelegramDescriptor> Encode(Span<byte> data)
        {
            List<TelegramDescriptor> result = new();
            result.Add(new("Sequence Counter", Convert.ToHexString(data.Slice(0, 4).ToArray())));
            result.Add(new("Security Signature", Convert.ToHexString(data.Slice(data.Length - 4, 4).ToArray())));

            Queue<byte> buffer = new Queue<byte>(data.Slice(4, data.Length - 8).ToArray());
            while (buffer.Count > 0)
            {
                var value = buffer.Dequeue();
                var dataset = new List<byte>();
                var currentDescriptor = GetDescriptor(value);
                while (dataset.Count != currentDescriptor.datalength)
                    dataset.Add(buffer.Dequeue());
                result.Add(SensorDataEncoderSelector(currentDescriptor.desciprot, dataset));
            }
            return result;
        }

        private TelegramDescriptor SensorDataEncoderSelector(SensorDataDescriptor descriptor, List<byte> dataset)
        {
            switch (descriptor)
            {
                case SensorDataDescriptor.TEMPERATURE: return new TelegramDescriptor(descriptor.ToString(), TemperatureEncoder(dataset));
                case SensorDataDescriptor.VOLTAGE: return new TelegramDescriptor(descriptor.ToString(), VoltageEncoder(dataset));
                case SensorDataDescriptor.ENERGY_LEVEL: return new TelegramDescriptor(descriptor.ToString(), EnergyLeverEncoder(dataset));
                case SensorDataDescriptor.ILLUMINATION_CELL: return new TelegramDescriptor(descriptor.ToString(), IlluminationEncoder(dataset));
                case SensorDataDescriptor.ILLUMINATION_SENSOR: return new TelegramDescriptor(descriptor.ToString(), IlluminationEncoder(dataset));
                case SensorDataDescriptor.HUMIDITY: return new TelegramDescriptor(descriptor.ToString(), HumidityEncoder(dataset));
                case SensorDataDescriptor.ACCELERATION: return new TelegramDescriptor(descriptor.ToString(), AccelerationEncoder(dataset));
                case SensorDataDescriptor.MAGNET: return new TelegramDescriptor(descriptor.ToString(), MagnetEncoder(dataset));
                case SensorDataDescriptor.OPTIONAL_DATA: return new TelegramDescriptor(descriptor.ToString(),Convert.ToHexString(dataset.ToArray()));
            }
            return new TelegramDescriptor("unknown", "descriptor");
        }

        private string MagnetEncoder(List<byte> dataset)
        {
            if (dataset.Count != 1) return "Invalid data length";
            return dataset[0] == 0b10 ? "Closed" : "Open";
        }

        private string AccelerationEncoder(List<byte> dataset)
        {
            if (dataset.Count != 4) return "Invalid data Length";
            var value = (dataset[3] << 24) | (dataset[2] << 16) | (dataset[1] << 8) | (dataset[0]);
            var status = value >> 30;
            string statusString = "out of bound";
            if (status == 1) statusString = "Periodic update";
            else if (status == 2) statusString = "Acceleration wake";
            else if (status == 3) statusString = "Disabled";

            float ZVector = value & 0b11_1111_1111;
            float YVector = value & 0b11_1111_1111_00_0000_0000 >> 10;
            float XVector = value & 0b11_1111_1111_00_0000_0000_00_0000_0000 >> 20;
            ZVector = (ZVector - 512) / 100;
            YVector = (YVector - 512) / 100;
            XVector = (XVector - 512) / 100;

            return $"{statusString} X = {XVector}g Y = {YVector}g Z = {ZVector}g";
        }

        private string HumidityEncoder(List<byte> dataset)
        {
            if (dataset.Count != 1) 
                return "Invalid data Length";
            float value = ((float)dataset[0] / 2);
            return $"{value} % r.h.";
        }

        private string IlluminationEncoder(List<byte> dataset)
        {
            if (dataset.Count != 2)
                return "Invalid data Length";
            var value = ((dataset[1] << 8) | dataset[0]);
            return $"{value} lx";
        }

        private string EnergyLeverEncoder(List<byte> dataset)
        {
            if (dataset.Count != 1) 
                return "Invalid data Length";
            float value = ((float)dataset[0] / 2);
            return $"{value} %";
        }

        private string VoltageEncoder(List<byte> dataset)
        {
            if (dataset.Count != 2)
                return "Invalid data Length";
            short value = 0;
            if (BitConverter.IsLittleEndian)
                value = BinaryPrimitives.ReadInt16BigEndian(dataset.ToArray());
            value = BinaryPrimitives.ReadInt16LittleEndian(dataset.ToArray());
            return $"{(value * 0.5)} mV";
        }

        private string TemperatureEncoder(List<byte> dataset)
        {
            if (dataset.Count != 2) 
                return "Invalid data Length";
            short value = 0;
            if (BitConverter.IsLittleEndian)
                value = BinaryPrimitives.ReadInt16BigEndian(dataset.ToArray());
            value = BinaryPrimitives.ReadInt16LittleEndian(dataset.ToArray());
            return $"{(value * 0.01).ToString("0.00")} °C";
        }
    }
}
