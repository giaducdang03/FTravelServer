using FTravel.Repository.EntityModels;
using FTravel.Repository.Repositories.Interface;
using FTravel.Service.Services.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FTravel.Service.Services
{
    public class SettingService : ISettingService
    {
        private readonly ISettingRepository _settingRepository;

        public SettingService(ISettingRepository settingRepository) 
        {
            _settingRepository = settingRepository;
        }
        public async Task<Setting> CreateNewSettingAsync(string key, string value, string description)
        {
            var existSetting = await _settingRepository.GetByKey(key);
            if (existSetting == null) 
            {
                Setting newSetting = new Setting()
                {
                    Key = key,
                    Value = value,
                    Description = description
                };
                await _settingRepository.AddAsync(newSetting);
                return newSetting;
            }
            return null;
        }

        public async Task<List<Setting>> GetAllSettingsAsync()
        {
            return await _settingRepository.GetAllAsync();
        }

        public async Task<Setting> GetSettingByKeyAsync(string key)
        {
            var setting = await _settingRepository.GetByKey(key);
            return setting;
        }

        public async Task<string> GetValueByKeyAsync(string key)
        {
            var setting = await _settingRepository.GetByKey(key);
            return setting != null ? setting.Value : "";
        }
    }
}
