using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace UserInputMacro
{
	public class ButtonState
	{
		public bool IsRecording { get; set; } = false;
		public bool IsPlaying { get; set; } = false;

		public ButtonState()
		{
		}
	}
}
