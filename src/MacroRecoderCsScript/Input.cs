using System;
using System.Runtime.InteropServices;

namespace UserInputMacro
{
	[Flags]
	public enum XButtonFlag : uint
	{
		/// <summary>
		/// Press or release 1st extend mouse button
		/// </summary>
		XButton1 = 0x0001,

		/// <summary>
		/// Press or release 2nd extend mouse button
		/// </summary>
		XButton2 = 0x0002
	};

	[Flags]
	public enum MouseEvent : uint
	{
		/// <summary>
		/// No mouse event
		/// </summary>
		None = 0x0000,

		/// <summary>
		/// Coordinate is absolute(if not specified, coordinate is relative)
		/// </summary>
		Absolute = 0x8000,

		/// <summary>
		/// Mouse wheel was moved horizontally(if specified, mouseData is the amount of movement)
		/// </summary>
		Hwheel = 0x01000,

		/// <summary>
		/// Mouse was moved
		/// </summary>
		Move = 0x0001,

		/// <summary>
		/// 
		/// </summary>
		MoveNoCoalesce = 0x2000,

		/// <summary>
		/// Mouse left button was pressed
		/// </summary>
		LeftDown = 0x0002,

		/// <summary>
		/// Mouse left button was released
		/// </summary>
		LeftUp = 0x0004,

		/// <summary>
		/// Mouse right button was pressed
		/// </summary>
		RightDown = 0x0008,

		/// <summary>
		/// Mouse right button was released
		/// </summary>
		RightUp = 0x0010,

		/// <summary>
		/// Mouse middle button was pressed
		/// </summary>
		MiddleDown = 0x0020,

		/// <summary>
		/// Mouse middle button was released
		/// </summary>
		MiddleUp = 0x0040,

		/// <summary>
		/// Coordinate maps to entire desktop(and must used with Absolute)
		/// </summary>
		VirtualDesk = 0x4000,

		/// <summary>
		/// Mouse wheel was moved(if specified, mouseData is the amount of movement)
		/// </summary>
		Wheel = 0x0800,

		/// <summary>
		/// Mouse extend button was pressed
		/// </summary>
		XDown = 0x0080,

		/// <summary>
		/// Mouse extend button was released
		/// </summary>
		XUp = 0x0100
	};

	[Flags]
	public enum KeyEvent : uint
	{
		/// <summary>
		/// No keyboard event
		/// </summary>
		None = 0x0000,

		/// <summary>
		/// Scan code was proceeded by a prefix byle(0xE0)
		/// </summary>
		ExtendEdKey = 0x0001,

		/// <summary>
		/// Released key(if not specified, pressed key)
		/// </summary>
		KeyUp = 0x0002,

		/// <summary>
		/// Key was identified by scan code and ignored virtual key
		/// </summary>
		ScanCode = 0x0008,

		/// <summary>
		/// 
		/// </summary>
		Unicode = 0x0004
	};

	public enum InputType : uint
	{
		Mouse    = 0,
		Keyboard = 1,
		Handware = 2
	};

	public struct MouseInput
	{
		public int coordinateX;
		public int coordinateY;
		public int mouseData;
		public MouseEvent flags;
		public uint time;
		public IntPtr extraInfo;
	};

	public struct KeyInput
	{
		public ushort virtualKey;
		public ushort scanCode;
		public KeyEvent flags;
		public uint time;
		public IntPtr extraInfo;
	};

	public struct HandwareInput
	{
		public uint msg;
		public ushort paramL;
		public ushort paramH;
	};

	[StructLayout( LayoutKind.Explicit )]
	public struct InputUnion
	{
		[FieldOffset( 0 )]
		public MouseInput mouseInput;

		[FieldOffset( 0 )]
		public KeyInput keyInput;

		[FieldOffset( 0 )]
		public HandwareInput handwareInput;
	};

	[StructLayout(LayoutKind.Sequential)]
	public struct Input
	{
		public InputType type;
		public InputUnion inputInfo;
	}
}