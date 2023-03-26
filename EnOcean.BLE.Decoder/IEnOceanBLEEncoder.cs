using System;
using System.Collections.Generic;

namespace EnOcean.BLE.Decoder
{
    public interface IEnOceanBLEEncoder
    {
        public ICollection<TelegramDescriptor> Encode(Span<byte> data);
    }
}
