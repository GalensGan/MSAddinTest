using MSAddinTest.Core.Settings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace MSAddinTest.Core.Command
{
    /// <summary>
    /// 更新设置
    /// </summary>
    internal class UpdateSettingsCommand : CommandBase
    {
        // 允许更新的字段名称
        private List<string> _allowUpdatingFields = new List<string>()
        {
            "autoLoad","autoReload"
        };

        private List<UpdateSettingArg> _updateSettings;
        /// <summary>
        /// 传入更新字符串 <br/>
        /// 格式为 plugin1Name.field1Name = value,field2Name=value,plugin2Name.field3Name=value
        /// </summary>
        /// <param name="updateString"></param>
        public UpdateSettingsCommand(string updateString)
        {
            // 将字符串解析为对象
            _updateSettings = new List<UpdateSettingArg>();

            // 清除等号两侧的空格
            var regex = new Regex(@"\s*=+\s*");
            updateString = regex.Replace(updateString, "=");
            var settings = updateString.Split(',');
            foreach (var setting in settings)
            {
                var args = setting.Split('=');
                if (args.Length != 2) continue;

                var names = args[0].Split('.');
                if (names.Length == 0) continue;

                if (names.Length == 1 && _updateSettings.Count == 0) continue;

                if (names.Length == 1)
                {
                    _updateSettings.Add(new UpdateSettingArg()
                    {
                        Name = _updateSettings.Last().Name,
                        FieldName = args[0],
                        Value = args[1]
                    });
                }
                else if (names.Length == 2)
                {
                    _updateSettings.Add(new UpdateSettingArg()
                    {
                        Name = names[0],
                        FieldName = names[1],
                        Value = args[1],
                    });
                }
            }
        }

        public override FuncResult Start()
        {
            if (_updateSettings.Count < 1) return new FuncResult(false);
            _updateSettings = _updateSettings.FindAll(x => _allowUpdatingFields.Contains(x.FieldName));

            // 开始进行设置
            foreach(var setting in _updateSettings)
            {
                PluginSetting.UpdatePluginSetting(setting);
            }

            // 保存设置
            PluginSetting.Save();

            return new FuncResult(true);
        }
    }
}
