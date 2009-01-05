using System;
using System.IO;
using System.Text;

namespace LSharpSilverlightRepl
{
	public delegate void WriteDelegate(string value);

	public class SilverlightWriter : TextWriter
	{
		private WriteDelegate PageWrite;

		public SilverlightWriter(WriteDelegate write)
		{
			PageWrite = write;
		}

		public override void Write(string value)
		{
			PageWrite(value);
		}
		public override void Write(object value)
		{
			PageWrite(value.ToString());
		}
		public override void WriteLine(string value)
		{
			PageWrite(value + "\n");
		}
		public override void WriteLine(string format, params object[] arg)
		{
			PageWrite(String.Format(format, arg) + "\n");
		}
		public override void WriteLine(string format, object arg0, object arg1)
		{
			PageWrite(String.Format(format, arg0, arg1) + "\n");
		}
		public override void WriteLine()
		{
			PageWrite("\n");
		}

		public override Encoding Encoding
		{
			get { return Encoding.Unicode; }
		}
	}
}
