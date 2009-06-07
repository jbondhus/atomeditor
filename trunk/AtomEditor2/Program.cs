using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Reflection;

namespace Kirishima16.Applications.AtomEditor2
{
    static class Program
    {
		public static Assembly[] PluginedAssemblies;

        /// <summary>
        /// アプリケーションのメイン エントリ ポイントです。
        /// </summary>
        [STAThread]
        static void Main()
        {
			Properties.Settings.Default.Upgrade();

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new MainForm());
        }
    }
}