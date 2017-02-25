namespace UserInputMacro
{
	public enum WindowsHookID : int
	{
		KeyBoardLowLevel = 13,
		MouseLowLevel = 14
	};

	public enum KeyHookEvent : int
	{
		KeyDown = 0x100,
		KeyUp = 0x101,
		SysKeyDown = 0x104,
		SysKeyUp = 0x105
	};

	public enum MouseHookEvent : int
	{
		Move = 0x0200,
		LeftDown = 0x0201,
		LeftUp = 0x0202,
		RightDown = 0x0204,
		RightUp = 0x0205,
		MiddleDown = 0x0207,
		MiddleUp = 0x0208,
		Wheel = 0x020A,
		Hwheel = 0x020E
	};
}
