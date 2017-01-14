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

		private static readonly Dictionary<int, string> MouseFuncDic = new Dictionary<int, string>()
		{
			{ HookEventID.WM_MOUSEMOVE,   nameof( MacroScript.SetMousePos ) },
			{ HookEventID.WM_LBUTTONDOWN, nameof( MacroScript.PushLeftButton ) },
			{ HookEventID.WM_LBUTTONUP,   nameof( MacroScript.PullLeftButton ) },
			{ HookEventID.WM_MOUSEWHEEL,  nameof( MacroScript.WheelMouse ) },
			{ HookEventID.WM_MOUSEHWHEEL, nameof( MacroScript.HWheelMouse ) },
			{ HookEventID.WM_RBUTTONDOWN, nameof( MacroScript.PushRightButton ) },
			{ HookEventID.WM_RBUTTONUP,   nameof( MacroScript.PullRightButton ) },
			{ HookEventID.WM_MBUTTONDOWN, nameof( MacroScript.PushMiddleButton ) },
			{ HookEventID.WM_MBUTTONUP,   nameof( MacroScript.PullMiddleButton ) },
		};

		private static readonly Dictionary<int, string> KeyFuncDic = new Dictionary<int, string>()
		{
			{ HookEventID.WM_KEYDOWN,    nameof( MacroScript.PressKey ) },
			{ HookEventID.WM_KEYUP,      nameof( MacroScript.ReleaseKey ) },
			{ HookEventID.WM_SYSKEYDOWN, nameof( MacroScript.PressKey ) },
			{ HookEventID.WM_SYSKEYUP,   nameof( MacroScript.ReleaseKey ) },
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
				MouseHook = RecordMouseLog
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

		private void RecordKeyLog( KeyHookStruct keyStr, int keyEvent )
		{
			recordScript.Append( $"Delay({delayWatch.ElapsedMilliseconds});\r\n" );
			delayWatch.Restart();

			recordScript.Append( ToKeyMacroFormat( keyStr, keyEvent ) );
		}

		private void RecordMouseLog( MouseHookStruct hookStr, int mouseEvent )
		{
			recordScript.Append( $"Delay({delayWatch.ElapsedMilliseconds});\r\n" );
			delayWatch.Restart();

			recordScript.Append( ToMouseMacroFormat( hookStr, mouseEvent ) );
		}

		private string ToKeyMacroFormat( KeyHookStruct keyStr, int keyEvent )
		{
			var funcName = KeyFuncDic[ keyEvent ];

			return $"{funcName}({keyStr.virtualKey});\r\n";
		}

		private string ToMouseMacroFormat( MouseHookStruct hookStr, int mouseEvent )
		{
			var funcName = MouseFuncDic[ mouseEvent ];

			if( mouseEvent == HookEventID.WM_MOUSEWHEEL || mouseEvent == HookEventID.WM_MOUSEHWHEEL ) {
				return $"{funcName}({hookStr.coordinatePoint.x}, {hookStr.coordinatePoint.y}, {GetWheelData( hookStr.mouseData )});\r\n";
			}
			else {
				return $"{funcName}({hookStr.coordinatePoint.x}, {hookStr.coordinatePoint.y});\r\n";
			}
		}

		private int GetWheelData( int mouseData )
		{
			// Get high-order word
			return  mouseData >> 16;
		}
	}
}
