using FTravel.Repository.EntityModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FTravel.Service.Services.Interface
{
    public interface ISettingService
    {
        public Task<Setting> GetSettingByKeyAsync(string key);

        public Task<Setting> CreateNewSettingAsync(string key, string value, string description);

        public Task<List<Setting>> GetAllSettingsAsync();

        public Task<string> GetValueByKeyAsync(string key);
    }
}
