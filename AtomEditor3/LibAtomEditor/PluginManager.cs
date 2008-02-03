using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Windows.Forms;
using System.Reflection;

namespace Kirishima16.Libraries.AtomEditor
{
	/// <summary>
	/// プラグインの型情報を管理し、必要なときにアクセスできるようにします。
	/// </summary>
	class PluginManager
	{
		static PluginManager singleton = new PluginManager();

		public static PluginManager Singleton
		{
			get { return singleton; }
		}

		private string pluginDirectory;

		/// <summary>
		/// プラグインが格納されているディレクトリを取得します。
		/// </summary>
		public string PluginDirectory
		{
			get { return pluginDirectory; }
		}

		private List<Assembly> pluginedAssemblies;

		/// <summary>
		/// プラグインとして読み込まれたアセンブリのリストを取得します。
		/// </summary>
		public List<Assembly> PluginedAssemblies
		{
			get { return pluginedAssemblies; }
		}

		private List<Type> boxPlugines;

		/// <summary>
		/// BoxTreeNodeを継承した型のリストを取得します。
		/// </summary>
		public List<Type> BoxPlugines
		{
			get { return boxPlugines; }
		}

		/// <summary>
		/// 既定のプラグインフォルダを参照するプラグインマネージャを初期化します。
		/// </summary>
		public PluginManager()
			: this(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), @"AtomEditor\Plugins")) { }

		/// <summary>
		/// 指定したプラグインフォルダを参照するプラグインマネージャを初期化します。
		/// </summary>
		/// <param name="dir"></param>
		public PluginManager(string dir)
		{
			pluginDirectory = dir;

			if (Directory.Exists(pluginDirectory)) {
				Directory.CreateDirectory(pluginDirectory);
			}

			pluginedAssemblies = new List<Assembly>();
			boxPlugines = new List<Type>();
		}

		/// <summary>
		/// 
		/// </summary>
		public void LoadPlugins()
		{
			pluginedAssemblies.Clear();
			boxPlugines.Clear();
			string[] dllFiles = Directory.GetFiles(PluginDirectory, "*.dll");
			foreach (string df in dllFiles) {
				bool flg = false;
				Assembly asm = Assembly.LoadFile(df);
				Type[] types = asm.GetTypes();
				foreach (Type t in types) {
					if (!t.IsClass || !t.IsPublic || t.IsAbstract) {
						continue;
					}
					if (t.IsSubclassOf(typeof(BoxTreeNode))) {
						boxPlugines.Add(t);
						flg = true;
					}
				}
				if (flg) {
					pluginedAssemblies.Add(asm);
				}
			}
		}
	}
}
