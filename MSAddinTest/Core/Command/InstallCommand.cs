using Bentley.DgnPlatformNET;
using Bentley.MstnPlatformNET;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MSAddinTest.Core.Command
{
    internal class InstallCommand : CommandBase
    {
        public override FuncResult Start()
        {
            // 安装
            // 修改自动加载
            string personalConfPath = Path.Combine(ConfigurationManager.GetVariable("_USTN_HOMEROOT"), "prefs\\Personal.ucf");

            if (!File.Exists(personalConfPath)) return new FuncResult(false, "用户配置文件不存在");

            // 读取值并修改
            string configContent = File.ReadAllText(personalConfPath);
            string autoloadSentence = "\r\n%level Organization\r\nMS_DGNAPPS > MSAddinTest";
            if (!configContent.Contains(autoloadSentence))
            {
                // 添加语句使其自动加载
                StreamWriter configWriter = new StreamWriter(personalConfPath, true);
                configWriter.Write(autoloadSentence);
                configWriter.Close();
            }

            MessageCenter.Instance.ShowInfoMessage("成功设置 MSAddinTest 自动加载!", "", false);

            return new FuncResult(true);
        }
    }
}
