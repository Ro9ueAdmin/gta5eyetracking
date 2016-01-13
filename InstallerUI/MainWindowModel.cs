using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Microsoft.Tools.WindowsInstallerXml.Bootstrapper;

namespace InstallerUI
{
	public class MainWindowModel : INotifyPropertyChanged
	{
		public MainWindowModel(BootstrapperApplication bootstrapper)
		{
			this.IsThinking = false;
			this.Bootstrapper = bootstrapper;
			this.Bootstrapper.ApplyComplete += this.OnApplyComplete;
			this.Bootstrapper.DetectPackageComplete += this.OnDetectPackageComplete;
			this.Bootstrapper.PlanComplete += this.OnPlanComplete;
		}
		private bool _installEnabled;
		public bool InstallEnabled
		{
			get { return _installEnabled; }
			set
			{
				_installEnabled = value;
				OnNotifyPropertyChanged("InstallEnabled");
			}
		}
		private bool _uninstallEnabled;
		public bool UninstallEnabled
		{
			get { return _uninstallEnabled; }
			set
			{
				_uninstallEnabled = value;
				OnNotifyPropertyChanged("UninstallEnabled");
			}
		}
		private bool _isThinking;
		public bool IsThinking
		{
			get { return _isThinking; }
			set
			{
				_isThinking = value;
				OnNotifyPropertyChanged("IsThinking");
				OnNotifyPropertyChanged("CanRemove");
				OnNotifyPropertyChanged("CanInstall");
			}
		}
		public BootstrapperApplication Bootstrapper { get; private set; }

		private void InstallExecute()
		{
			IsThinking = true;
			Bootstrapper.Engine.Plan(LaunchAction.Install);
		}
		private void UninstallExecute()
		{
			IsThinking = true;
			Bootstrapper.Engine.Plan(LaunchAction.Uninstall);
		}
		private void ExitExecute()
		{
			InstallerBootstrapperApplication.BootstrapperDispatcher.InvokeShutdown();
		}
		/// <summary>
		/// Method that gets invoked when the Bootstrapper ApplyComplete event is fired.
		/// This is called after a bundle installation has completed. Make sure we updated the view.
		/// </summary>
		private void OnApplyComplete(object sender, ApplyCompleteEventArgs e)
		{
			IsThinking = false;
			InstallEnabled = false;
			UninstallEnabled = false;
		}
		/// <summary>
		/// Method that gets invoked when the Bootstrapper DetectPackageComplete event is fired.
		/// Checks the PackageId and sets the installation scenario. The PackageId is the ID
		/// specified in one of the package elements (msipackage, exepackage, msppackage,
		/// msupackage) in the WiX bundle.
		/// </summary>
		private void OnDetectPackageComplete(object sender, DetectPackageCompleteEventArgs e)
		{
			if (e.PackageId == "DummyInstallationPackageId")
			{
				if (e.State == PackageState.Absent)
					InstallEnabled = true;
				else if (e.State == PackageState.Present)
					UninstallEnabled = true;
			}
		}
		/// <summary>
		/// Method that gets invoked when the Bootstrapper PlanComplete event is fired.
		/// If the planning was successful, it instructs the Bootstrapper Engine to
		/// install the packages.
		/// </summary>
		private void OnPlanComplete(object sender, PlanCompleteEventArgs e)
		{
			if (e.Status >= 0)
				Bootstrapper.Engine.Apply(System.IntPtr.Zero);
		}

		//private RelayCommand installCommand;
		//public RelayCommand InstallCommand
		//{
		//	get
		//	{
		//		if (installCommand == null)
		//			installCommand = new RelayCommand(() => InstallExecute(), () => InstallEnabled == true);
		//		return installCommand;
		//	}
		//}
		//private RelayCommand uninstallCommand;
		//public RelayCommand UninstallCommand
		//{
		//	get
		//	{
		//		if (uninstallCommand == null)
		//			uninstallCommand = new RelayCommand(() => UninstallExecute(), () => UninstallEnabled == true);
		//		return uninstallCommand;
		//	}
		//}
		//private RelayCommand exitCommand;
		//public RelayCommand ExitCommand
		//{
		//	get
		//	{
		//		if (exitCommand == null)
		//			exitCommand = new RelayCommand(() => ExitExecute());
		//		return exitCommand;
		//	}
		//}




		private string _gtaPathText;
		private string _windowTitle;
		private string _scriptHookVVersion;
		private string _gtaVersion;
		private string _modVersion;
		private bool _canInstall;
		private bool _canRemove;
		private string _scriptHookVAvailableVersion;
		private string _modAvailableVersion;

		public event PropertyChangedEventHandler PropertyChanged;
		private void OnNotifyPropertyChanged([CallerMemberName] String propertyName = "")
		{
			var handler = PropertyChanged;
			if (handler != null)
			{
				handler(this, new PropertyChangedEventArgs(propertyName));
			}
		}

		public string GtaPathText
		{
			get { return _gtaPathText; }
			set
			{
				_gtaPathText = value;
				OnNotifyPropertyChanged("GtaPathText");
			}
		}

		public string WindowTitle
		{
			get { return _windowTitle; }
			set
			{
				_windowTitle = value;
				OnNotifyPropertyChanged("WindowTitle");
			}
		}

		public string GtaVersion
		{
			get { return _gtaVersion; }
			set
			{
				_gtaVersion = value;
				OnNotifyPropertyChanged("GtaVersion");
			}
		}

		public bool CanInstall
		{
			get { return _canInstall && !_isThinking; }
			set
			{
				_canInstall = value;
				OnNotifyPropertyChanged("CanInstall");
			}
		}

		public bool CanRemove
		{
			get { return _canRemove && !_isThinking; }
			set
			{
				_canRemove = value;
				OnNotifyPropertyChanged("CanRemove");
			}
		}

		public string ScriptHookVVersion
		{
			get { return _scriptHookVVersion; }
			set
			{
				_scriptHookVVersion = value;
				OnNotifyPropertyChanged("ScriptHookVVersion");
			}
		}

		public string ScriptHookVAvailableVersion
		{
			get { return _scriptHookVAvailableVersion; }
			set
			{
				_scriptHookVAvailableVersion = value;
				OnNotifyPropertyChanged("ScriptHookVAvailableVersion");
			}
		}

		public string ModVersion
		{
			get { return _modVersion; }
			set
			{
				_modVersion = value;
				OnNotifyPropertyChanged("ModVersion");
			}
		}

		public string ModAvailableVersion
		{
			get { return _modAvailableVersion; }
			set
			{
				_modAvailableVersion = value;
				OnNotifyPropertyChanged("ModAvailableVersion");
			}
		}		

	}
}