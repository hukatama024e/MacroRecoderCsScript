using System.Collections.Generic;
using System.Text;
using System.Diagnostics;

namespace UserInputMacro
{
	class ScriptRecorder
	{
		private StringBuilder recordScript;
		private Stopwatch delayWatch;
		private UserInputHook hook;

		private static readonly Dictionary<MouseHookEvent, string> MouseFuncDic = new Dictionary<MouseHookEvent, string>()
		{
			{ MouseHookEvent.Move,       nameof( MacroScript.SetMousePos ) },
			{ MouseHookEvent.LeftDown,   nameof( MacroScript.PushLeftButton ) },
			{ MouseHookEvent.LeftUp,     nameof( MacroScript.PullLeftButton ) },
			{ MouseHookEvent.Wheel,      nameof( MacroScript.WheelMouse ) },
			{ MouseHookEvent.Hwheel,     nameof( MacroScript.HWheelMouse ) },
			{ MouseHookEvent.RightDown,  nameof( MacroScript.PushRightButton ) },
			{ MouseHookEvent.RightUp,    nameof( MacroScript.PullRightButton ) },
			{ MouseHookEvent.MiddleDown, nameof( MacroScript.PushMiddleButton ) },
			{ MouseHookEvent.MiddleUp,   nameof( MacroScript.PullMiddleButton ) },
		};

		private static readonly Dictionary<KeyHookEvent, string> KeyFuncDic = new Dictionary<KeyHookEvent, string>()
		{
			{ KeyHookEvent.KeyDown,    nameof( MacroScript.PressKey ) },
			{ KeyHookEvent.KeyUp,      nameof( MacroScript.ReleaseKey ) },
			{ KeyHookEvent.SysKeyDown, nameof( MacroScript.PressKey ) },
			{ KeyHookEvent.SysKeyUp,   nameof( MacroScript.ReleaseKey ) },
		};

		public string Record
		{
			get { return recordScript.ToString(); }
		}

		public ScriptRecorder()
		{
			delayWatch = new Stopwatch();

			hook = new UserInputHook
			{
				KeyHook = RecordKeyLog,
				MouseHook = RecordMouseLog,
				HookErrorProc = CommonUtil.HandleException
			};
		}

		public void StartRecording()
		{
			recordScript = new StringBuilder();

			hook.RegisterKeyHook();
			hook.RegisterMouseHook();

			delayWatch.Start();
		}

		public void EndRecording()
		{
			hook.Dispose();
			delayWatch.Stop();
			delayWatch.Reset();
		}

		private void RecordKeyLog( KeyHookStruct keyHookStr, int keyEvent )
		{
			recordScript.Append( $"Delay({delayWatch.ElapsedMilliseconds});\r\n" );
			delayWatch.Restart();

			recordScript.Append( ToKeyMacroFormat( keyHookStr, ( KeyHookEvent ) keyEvent ) );
		}

		private void RecordMouseLog( MouseHookStruct mouseHookStr, int mouseEvent )
		{
			recordScript.Append( $"Delay({delayWatch.ElapsedMilliseconds});\r\n" );
			delayWatch.Restart();

			recordScript.Append( ToMouseMacroFormat( mouseHookStr, ( MouseHookEvent ) mouseEvent ) );
		}

		private string ToKeyMacroFormat( KeyHookStruct keyHookStr, KeyHookEvent keyEvent )
		{
			var funcName = KeyFuncDic[ keyEvent ];

			return $"{funcName}({keyHookStr.virtualKey});\r\n";
		}

		private string ToMouseMacroFormat( MouseHookStruct mouseHookStr, MouseHookEvent mouseEvent )
		{
			var funcName = MouseFuncDic[ mouseEvent ];

			if( mouseEvent == MouseHookEvent.Wheel || mouseEvent == MouseHookEvent.Hwheel ) {
				return $"{funcName}({mouseHookStr.coordinatePoint.x}, {mouseHookStr.coordinatePoint.y}, {GetWheelData( mouseHookStr.mouseData )});\r\n";
			}
			else {
				return $"{funcName}({mouseHookStr.coordinatePoint.x}, {mouseHookStr.coordinatePoint.y});\r\n";
			}
		}

		private int GetWheelData( int mouseData )
		{
			// Get high-order word
			return  mouseData >> 16;
		}
	}
}
