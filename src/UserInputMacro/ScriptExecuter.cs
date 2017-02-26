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
			using( var hook = new UserInputHook() ) {
				HookSetting( hook );

				var script = CSharpScript.Create( File.ReadAllText( scriptPath ), ScriptOptions.Default, typeof( MacroScript ) );
				await script.RunAsync( new MacroScript() );
			}
		}

		private static void LoggingMouseMacro( MouseHookStruct mouseHookStr, int mouseEvent )
		{
			if( CheckMode( ModeKind.CreateLog ) ) {
				Logger.WriteMouseEventInfo( mouseHookStr, ( MouseHookEvent ) mouseEvent );
			}
		}

		private static void LoggingKeyMacro( KeyHookStruct keyHookStr, int keyEvent )
		{
			if( CheckMode( ModeKind.CreateLog ) ) {
				Logger.WriteKeyEventInfo( keyHookStr, ( KeyHookEvent ) keyEvent );
			}
		}

		private static void HookSetting( UserInputHook hook )
		{
			hook.MouseHook = LoggingMouseMacro;
			hook.KeyHook = LoggingKeyMacro;

			hook.RegisterKeyHook();
			hook.RegisterMouseHook();
		}

		private static bool CheckMode( ModeKind mode )
		{
			var currentMode = AppEnvironment.GetInstance().Mode;
			return ( currentMode & mode ) == mode;
		}
	}
}
