using System;
using System.Runtime.InteropServices;

namespace UserInputMacro
{
	[Flags]
	public enum XButtonFlag : uint
	{
		/// <summary>
		/// 最初の拡張ボタンを押下・開放した
		/// </summary>
		XButton1 = 0x0001,

		/// <summary>
		/// 2番目の拡張ボタンを押下・開放した
		/// </summary>
		XButton2 = 0x0002
	};

	[Flags]
	public enum MouseEvent : uint
	{
		/// <summary>
		/// MouseInputのxとyが絶対座標か(設定されていない場合は相対座標)
		/// </summary>
		Absolute = 0x8000,

		/// <summary>
		/// マウスホイールを横方向に動作した
		/// (MouseInputのmouseDataにマウスホイールの回転量を設定する)
		/// </summary>
		Hwheel = 0x01000,

		/// <summary>
		/// マウスカーソルを移動した
		/// </summary>
		Move = 0x0001,

		/// <summary>
		/// 
		/// </summary>
		MoveNoCoalesce = 0x2000,

		/// <summary>
		/// マウスの左ボタンを押下した
		/// </summary>
		LeftDown = 0x0002,

		/// <summary>
		/// マウスの左ボタンを開放した
		/// </summary>
		LeftUp = 0x0004,

		/// <summary>
		/// マウスの右ボタンを押下した
		/// </summary>
		RightDown = 0x0008,

		/// <summary>
		/// マウスの右ボタンを開放した
		/// </summary>
		RightUp = 0x0010,

		/// <summary>
		/// マウスの中央ボタンを押下した
		/// </summary>
		MiddleDown = 0x0020,

		/// <summary>
		/// マウスの中央ボタンを開放した
		/// </summary>
		MiddleUp = 0x0040,

		/// <summary>
		/// デスクトップ全体に対応する
		/// (必ずAbsoluteと共に使用する)
		/// </summary>
		VirtualDesk = 0x4000,

		/// <summary>
		/// マウスホイールを動作した
		/// (MouseInputのmouseDataにマウスホイールの回転量を設定する)
		/// </summary>
		Wheel = 0x0800,

		/// <summary>
		/// 拡張ボタンを押下した
		/// </summary>
		XDown = 0x0080,

		/// <summary>
		/// 拡張ボタンを開放した
		/// </summary>
		XUp = 0x0100
	};

	[Flags]
	public enum KeyEvent : uint
	{
		/// <summary>
		/// スキャンコードが接頭辞ビット(0xE0)によって先行している
		/// </summary>
		ExtendEdKey = 0x0001,

		/// <summary>
		/// キーが開放された(指定が無ければ押下)
		/// </summary>
		KeyUp = 0x0002,

		/// <summary>
		/// ScanCodeによってキーが識別され、VirtualKeyが無効になる
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
		public uint mouseData;
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