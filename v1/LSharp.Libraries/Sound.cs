using System;

using System.Runtime.InteropServices;

namespace LSharp.Libraries
{
	/// <summary>
	/// Summary description for Sound.
	/// </summary>
	public class Sound
	{

		[DllImport("Winmm.dll")]
		static extern int sndPlaySound(string lpszSound,  int fuSound);


																						
		public static void Play(string filename) 
		{
			 sndPlaySound(filename,0);
		}


		
	}
}
