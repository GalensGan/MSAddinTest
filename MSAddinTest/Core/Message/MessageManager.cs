using Bentley.MstnPlatformNET;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace MSAddinTest.Core.Message
{
    internal class MessageManager
    {
        /// <summary>
        /// 显示异常错误
        /// </summary>
        /// <param name="exception"></param>
        /// <param name="titlePrefix"></param>
        public static void ShowException(Exception exception, string titlePrefix = "")
        {
            if (exception.InnerException != null) exception = exception.InnerException;

            var briefMessage = $"{titlePrefix}{exception.Message}";
            MessageBox.Show(briefMessage);
            MessageCenter.Instance.ShowErrorMessage(briefMessage, exception.StackTrace, false);
        }
    }
}
