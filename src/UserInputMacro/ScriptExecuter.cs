using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis.Scripting;
using Microsoft.CodeAnalysis.CSharp.Scripting;

namespace UserInputMacro
{
	static class ScriptExecuter
	{
		public static async Task ExecuteAsync( string scriptPath )
		{
			try {
				using( var hook = new UserInputHook() ) {
					HookSetting( hook );

					var script = CSharpScript.Create( File.ReadAllText( scriptPath ), ScriptOptions.Default, typeof( MacroScript ) );
					await script.RunAsync( new MacroScript() );
				}
			}
			catch( Exception ex ) {
				CommonUtil.HandleException( ex );
			}
		}

		private static void LoggingMouseMacro( MouseHookStruct mouseHookStr, int mouseEvent )
		{
			if( CommonUtil.CheckMode( ModeKind.CreateLog ) ) {
				Logger.WriteMouseEventInfo( mouseHookStr, ( MouseHookEvent ) mouseEvent );
			}
		}

		private static void LoggingKeyMacro( KeyHookStruct keyHookStr, int keyEvent )
		{
			if( CommonUtil.CheckMode( ModeKind.CreateLog ) ) {
				Logger.WriteKeyEventInfo( keyHookStr, ( KeyHookEvent ) keyEvent );
			}
		}

		private static void HookSetting( UserInputHook hook )
		{
			hook.MouseHook = LoggingMouseMacro;
			hook.KeyHook = LoggingKeyMacro;
			hook.HookErrorProc = CommonUtil.HandleException;

			hook.RegisterKeyHook();
			hook.RegisterMouseHook();
		}
	}
}
