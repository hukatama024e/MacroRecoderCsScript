using System;

namespace UserInputMacro
{
	[Flags]
	public enum ModeKind : byte
	{
		None = 0x00,
		MouseOnly = 0x01,
		KeyOnly = 0x02,
		CreateLog = 0x04
	};

	class AppEnvironment
	{
		private static AppEnvironment instance = new AppEnvironment();
		private ModeKind mode = ModeKind.None;

		private AppEnvironment()
		{
		}

		public static AppEnvironment GetInstance()
		{
			return instance;
		}

		public ModeKind GetMode()
		{
			return mode;
		}

		public void SetMode( ModeKind kind )
		{
			mode = kind;
		}
	}
}
