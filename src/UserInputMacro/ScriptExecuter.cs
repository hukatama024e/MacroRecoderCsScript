using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis.Scripting;
using Microsoft.CodeAnalysis.CSharp.Scripting;

namespace UserInputMacro
{
	class ScriptExecuter
	{
		public static async Task ExecuteAsync( string scriptPath )
		{
			var script = CSharpScript.Create( File.ReadAllText( scriptPath ), ScriptOptions.Default, typeof( MacroScript ) );
			await script.RunAsync( new MacroScript() );
		}
	}
}
