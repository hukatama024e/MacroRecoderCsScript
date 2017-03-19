using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Input;

namespace UserInputMacro
{
	class Logger
	{
		private static readonly string DATE_FORMAT = "yyyy/MM/dd HH:mm:ss.fff";
		private static readonly string INPUT_LOG_NAME = "input_log.txt";
		private static readonly string ERROR_LOG_NAME = "error_log.txt";

		private static readonly int COORDINATE_MAX = 65535;

		public static void WriteMouseInputInfo( MouseInput[] mouseInput )
		{
			foreach( var singleInput in mouseInput ) {
				int x = GetRelativeCoodinateX( singleInput.coordinateX );
				int y = GetRelativeCoodinateY( singleInput.coordinateY );

				var labeledData = new Dictionary<string, string>
				{
					{ "Date",       GetDateLog()                       },
					{ "LogKind",    "MouseInput"                       },
					{ "X",          x.ToString()                       },
					{ "Y",          y.ToString()                       },
					{ "MouseData",  singleInput.mouseData.ToString()   },
					{ "Flags",      singleInput.flags.ToString()       }
				};

				AppendInputLog( labeledData );
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

				AppendInputLog( labeledData );
			}
		}

		public static void WriteMouseEventInfo( MouseHookStruct mouseHookStr, MouseHookEvent mouseEvent )
		{
			var labeledData = new Dictionary<string, string>
			{
				{ "Date",       GetDateLog()                              },
				{ "LogKind",    "MouseEvent"                              },
				{ "X",          mouseHookStr.coordinatePoint.x.ToString() },
				{ "Y",          mouseHookStr.coordinatePoint.y.ToString() },
				{ "MouseData",  mouseHookStr.mouseData.ToString()         },
				{ "Flags",      mouseHookStr.flags.ToString()             },
				{ "Event",      mouseEvent.ToString()                     }
			};

			AppendInputLog( labeledData );
		}

		public static void WriteKeyEventInfo( KeyHookStruct keyHookStr, KeyHookEvent keyEvent )
		{
			var labeledData = new Dictionary<string, string>
			{
				{ "Date",       GetDateLog()                                                      },
				{ "LogKind",    "KeyEvent"                                                        },
				{ "Key",        KeyInterop.KeyFromVirtualKey( keyHookStr.virtualKey ).ToString()  },
				{ "Flags",      keyHookStr.flags.ToString()                                       },
				{ "Event",      keyEvent.ToString()                                               }
			};

			AppendInputLog( labeledData );
		}

		public static void WriteUserCustom( Dictionary<string, string> userCustomDic )
		{
			var labeledData = new Dictionary<string, string>
			{
				{ "Date",       GetDateLog() },
				{ "LogKind",    "UserCustom" },
			};

			labeledData = labeledData.Concat( userCustomDic ).ToDictionary( dic => dic.Key, dic => dic.Value );
			AppendInputLog( labeledData );
		}

		public static void WriteErrorLog( Exception ex )
		{
			var labeledData = new Dictionary<string, string>
			{
				{ "Date",       GetDateLog()  },
				{ "Message",    ex.Message    },
				{ "Source",     ex.Source     },
				{ "StackTrace", ex.StackTrace },
			};

			AppendErrorLog( labeledData );
		}

		private static string GetDateLog()
		{
			return DateTime.Now.ToString( DATE_FORMAT );
		}

		private static void AppendInputLog( Dictionary<string, string> labeledData )
		{
			File.AppendAllText( INPUT_LOG_NAME, CreateLtsvLog( labeledData ) + Environment.NewLine );
		}

		private static void AppendErrorLog( Dictionary<string, string> labeledData )
		{
			File.AppendAllText( ERROR_LOG_NAME, CreateLtsvLog( labeledData ) + Environment.NewLine );
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

		private static int GetRelativeCoodinateX( int coordX )
		{
			return ( int ) ( coordX * ( SystemParameters.PrimaryScreenWidth / CommonUtil.GetDpiWidth() ) / COORDINATE_MAX );
		}

		private static int GetRelativeCoodinateY( int coordY )
		{
			return ( int ) ( coordY * ( SystemParameters.PrimaryScreenHeight / CommonUtil.GetDpiHeight() ) / COORDINATE_MAX );
		}
	}
}
