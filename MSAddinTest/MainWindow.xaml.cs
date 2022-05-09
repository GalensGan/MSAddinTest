using MSAddinTest.Core;
using MSAddinTest.Core.Command;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace MSAddinTest
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            // 初始化
            new PluginManager(null);

            TestBtn.Click += TestBtn_Click;
            LoadPluginBtn.Click += LoadPluginBtn_Click;
            UnloadPluginBtn.Click += UnloadPluginBtn_Click;
        }

        private void UnloadPluginBtn_Click(object sender, RoutedEventArgs e)
        {
            // 卸载插件
            var cmd = new UnloadPluginCommand();
            PluginManager.Manager.InvokeCommand(cmd);
        }

        private void LoadPluginBtn_Click(object sender, RoutedEventArgs e)
        {
            var cmd = new LoadPluginCommand();
            PluginManager.Manager.InvokeCommand(cmd);
        }

        private void TestBtn_Click(object sender, RoutedEventArgs e)
        {
            var cmd = new RunPluginCommand("test", new Plugin.PluginArg());
            PluginManager.Manager.InvokeCommand(cmd);
        }
    }
}
