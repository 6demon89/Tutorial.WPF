using MvvmBasics.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MvvmBasics.Service;

public class GenerateDataService
{
    readonly Random random= new Random();

    public async Task<List<DataModel>> GetDataAsync()
    {
        List<DataModel> data = new();
        await Task.Delay(TimeSpan.FromSeconds(2));
        for(int i = 0; i < 10_000; i++)
        {
            byte[] buffer = new byte[64];
            for (int b = 0; b < buffer.Length; b++)
                buffer[b] = (byte)random.Next(0x41, 0x5a);
            data.Add(new(i,Encoding.UTF8.GetString(buffer),buffer));
        }
        return data;
    }
}
