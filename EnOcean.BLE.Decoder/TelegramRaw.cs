namespace EnOcean.BLE.Decoder
{
    public record TelegramRaw(ulong Address,short dBm,ushort Manufacturer, byte[] Data);
}
