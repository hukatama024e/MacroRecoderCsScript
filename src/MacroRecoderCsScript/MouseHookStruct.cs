using System;

namespace MacroRecoderCsScript
{
	[Flags]
	public enum LowLevelMouseEvent : uint
	{
		None = 0x00,
		Injected = 0x01,
		LowerIntegrityLevelInjected = 0x02
	};

	public struct Point
	{
		public int x;
		public int y;
	}

	public struct MouseHookStruct
	{
		public Point coordinatePoint;
		public int mouseData;
		public LowLevelMouseEvent flags;
		public uint time;
		public IntPtr extraInfo;
	};
}
