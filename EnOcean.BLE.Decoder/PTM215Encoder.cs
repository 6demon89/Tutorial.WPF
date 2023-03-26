using System;
using System.Collections.Generic;
using System.Linq;

namespace EnOcean.BLE.Decoder
{
    public class PTM215Encoder: IEnOceanBLEEncoder
    {
        public ICollection<TelegramDescriptor> Encode(Span<byte> data)
        {
            var result = new List<TelegramDescriptor>();
            data.Reverse();
            //First 4 Bytes are the Sequence Counter
            result.Add(new("Sequence Counter", Convert.ToHexString(data.Slice(0, 4).ToArray())));
            //Last 4 Bytes are the Security Signature
            result.Add(new("Security Signature", Convert.ToHexString(data.Slice(data.Length - 4, 4).ToArray())));

            //Fifth byte is the actual data that we care about
            byte swtichState = data[4];
            //Telegram can have also optional data, which is between index 4 and data.length-4
            if (data.Length > 9)
            {
                int OptionalDataLength = data.Length - 9;
                result.Add(new("Optional Data", Convert.ToHexString(data.Slice(4, OptionalDataLength).ToArray())));
            }

            //Get Switch Action
            //0b0001
            result.Add(new("Action Type", (swtichState & 1) == 1 ? "Pressed" : "Released"));

            //Get Switch buttons

            //0b0010
            result.Add(new("A0", (swtichState & 2) == 2 ? "Applied" : "N/A"));
            //0b0100
            result.Add(new("A1", (swtichState & 4) == 4 ? "Applied" : "N/A"));
            //0b1000
            result.Add(new("B0", (swtichState & 8) == 8 ? "Applied" : "N/A"));
            //0b1_0000
            result.Add(new("B1", (swtichState & 10) == 10 ? "Applied" : "N/A"));
            return result;
        }
    }
}
