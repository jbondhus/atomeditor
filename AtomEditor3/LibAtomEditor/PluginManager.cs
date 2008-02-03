using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Windows.Forms;
using System.Reflection;

namespace Kirishima16.Libraries.AtomEditor
{
	/// <summary>
	/// �v���O�C���̌^�����Ǘ����A�K�v�ȂƂ��ɃA�N�Z�X�ł���悤�ɂ��܂��B
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
		/// �v���O�C�����i�[����Ă���f�B���N�g�����擾���܂��B
		/// </summary>
		public string PluginDirectory
		{
			get { return pluginDirectory; }
		}

		private List<Assembly> pluginedAssemblies;

		/// <summary>
		/// �v���O�C���Ƃ��ēǂݍ��܂ꂽ�A�Z���u���̃��X�g���擾���܂��B
		/// </summary>
		public List<Assembly> PluginedAssemblies
		{
			get { return pluginedAssemblies; }
		}

		private List<Type> boxPlugines;

		/// <summary>
		/// BoxTreeNode���p�������^�̃��X�g���擾���܂��B
		/// </summary>
		public List<Type> BoxPlugines
		{
			get { return boxPlugines; }
		}

		/// <summary>
		/// ����̃v���O�C���t�H���_���Q�Ƃ���v���O�C���}�l�[�W�������������܂��B
		/// </summary>
		public PluginManager()
			: this(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), @"AtomEditor\Plugins")) { }

		/// <summary>
		/// �w�肵���v���O�C���t�H���_���Q�Ƃ���v���O�C���}�l�[�W�������������܂��B
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
