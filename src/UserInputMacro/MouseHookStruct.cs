using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UserInputMacro
{
	[Flags]
	public enum LowLevelMouseEvent : uint
	{
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
		public uint mouseData;
		public LowLevelMouseEvent flags;
		public uint time;
		public IntPtr extraInfo;
	};
}
