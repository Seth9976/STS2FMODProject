using System;
using System.Runtime.CompilerServices;
using Godot;
using Godot.Collections;

namespace MegaCrit.Sts2.Core.Helpers
{
	// Token: 0x02000C03 RID: 3075
	[NullableContext(1)]
	[Nullable(0)]
	public static class CommandLineHelper
	{
		// Token: 0x06007F7D RID: 32637 RVA: 0x002B48C8 File Offset: 0x002B2AC8
		static CommandLineHelper()
		{
			string[] cmdlineArgs = OS.GetCmdlineArgs();
			for (int i = 0; i < cmdlineArgs.Length; i++)
			{
				string text = cmdlineArgs[i].TrimStart('-');
				string text2 = text;
				string text3 = null;
				int num = text.IndexOf('=');
				if (num > 0)
				{
					text2 = text.Substring(0, num);
					text3 = text.Substring(num + 1);
				}
				else if (i + 1 < cmdlineArgs.Length && !cmdlineArgs[i + 1].StartsWith('-') && !cmdlineArgs[i + 1].StartsWith('+'))
				{
					text3 = cmdlineArgs[i + 1];
					i++;
				}
				CommandLineHelper._args[text2] = text3;
			}
		}

		// Token: 0x06007F7E RID: 32638 RVA: 0x002B4966 File Offset: 0x002B2B66
		public static bool HasArg(string key)
		{
			return CommandLineHelper._args.ContainsKey(key);
		}

		// Token: 0x06007F7F RID: 32639 RVA: 0x002B4973 File Offset: 0x002B2B73
		public static bool TryGetValue(string key, [Nullable(2)] out string value)
		{
			return CommandLineHelper._args.TryGetValue(key, ref value);
		}

		// Token: 0x06007F80 RID: 32640 RVA: 0x002B4984 File Offset: 0x002B2B84
		[return: Nullable(2)]
		public static string GetValue(string key)
		{
			string text;
			if (!CommandLineHelper.TryGetValue(key, out text))
			{
				return null;
			}
			return text;
		}

		// Token: 0x04002A7E RID: 10878
		[Nullable(new byte[] { 1, 1, 2 })]
		private static readonly Dictionary<string, string> _args = new Dictionary<string, string>();
	}
}
