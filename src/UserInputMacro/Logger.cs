using System;
using System.IO;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;

namespace UserInputMacro
{
	class Logger
	{
		private static readonly string DATE_FORMAT = "yyyy/MM/dd HH:mm:ss.fff";
		private static readonly string FILE_NAME = "Log.txt";

		public static void WriteMouseInputInfo( MouseInput[] mouseInput )
		{
			foreach( var singleInput in mouseInput ) {
				var labeledData = new Dictionary<string, string>
				{
					{ "Date",       GetDateLog()                       },
					{ "LogKind",    "MouseInput"                       },
					{ "X",          singleInput.coordinateX.ToString() },
					{ "Y",          singleInput.coordinateY.ToString() },
					{ "MouseData",  singleInput.mouseData.ToString()   },
					{ "Flags",      singleInput.flags.ToString()       }
				};

				AppendLog( labeledData );
			}
		}

		public static void WriteKeyInputInfo( KeyInput[] keyInput )
		{
			foreach( var singleInput in keyInput ) {
				var labeledData = new Dictionary<string, string>
				{
					{ "Date",       GetDateLog()			                                          },
					{ "LogKind",    "KeyInput"						                                  },
					{ "Key",        KeyInterop.KeyFromVirtualKey( singleInput.virtualKey ).ToString() },
					{ "Flags",      singleInput.flags.ToString()			                          }
				};

				AppendLog( labeledData );
			}
		}

		private static string GetDateLog()
		{
			return DateTime.Now.ToString( DATE_FORMAT );
		}

		private static void AppendLog( Dictionary<string, string> labeledData )
		{
			File.AppendAllText( FILE_NAME, CreateLtsvLog( labeledData ) + Environment.NewLine );
		}

		private static string CreateLtsvLog( Dictionary<string, string> labeledData )
		{
			var ltsvLog = new StringBuilder();

			foreach( var singleData in labeledData ) {
				ltsvLog.Append( singleData.Key + ":" + singleData.Value + "\t" );
			}

			// delete last tub character
			ltsvLog.Remove( ltsvLog.Length - 1, 1 );

			return ltsvLog.ToString();
		}
	}
}
