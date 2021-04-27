using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tutorial.WPF.Service
{
    public class LoadingUserDataLService
    {
        readonly string _pathToFile;

        public LoadingUserDataLService()
        {
            _pathToFile = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);

        }

        public async Task<bool> SaveUserData(UserModel model)
        {
            try
            {
                var raw = JsonConvert.SerializeObject(model);
                using (StreamWriter outputFile = new StreamWriter(Path.Join(_pathToFile, $"Tutorial_WPF.txt"), false))
                {
                    await outputFile.WriteAsync(raw);
                }
            }
            catch (Exception e)
            {
                return false;
            }
            return true;
        }

        public async Task<UserModel> LoadUserData()
        {
            try
            {
                string read = string.Empty;
                using (StreamReader reader = new StreamReader(Path.Join(_pathToFile, $"Tutorial_WPF.txt"), true))
                {
                    read = await  reader.ReadToEndAsync();
                }
                var result = JsonConvert.DeserializeObject<UserModel>(read);
                return result;
            }
            catch
            {
                throw;
            }
        }
    }

    public class UserModel
    {
        public string UserName { get; set; }
    }
}
