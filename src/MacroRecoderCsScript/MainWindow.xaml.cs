using System.Windows;

namespace UserInputMacro
{
	/// <summary>
	/// MainWindow.xaml の相互作用ロジック
	/// </summary>
	public partial class MainWindow : Window
	{
		private MainWindowViewModel userModel = new MainWindowViewModel();
		public MainWindow()
		{
			DataContext = userModel;
			InitializeComponent();

			AppEnvironment.GetInstance().DpiSetting();
			userModel.WinDispacher = Application.Current.Dispatcher;
		}
	}
}
