using System;

namespace UserInputMacro
{
	[Flags]
	public enum LowLevelKeyEvent : uint
	{
		Extended = 0x01,
		Injected = 0x10,
		AltDown = 0x20,
		Up = 0x80
	};

	public struct KeyHookStruct
	{
		public ushort virtualKey;
		public ushort scanCode;
		public LowLevelKeyEvent flags;
		public uint time;
		public IntPtr extraInfo;
	};
}
