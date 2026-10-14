using System;
using System.Runtime.CompilerServices;

namespace MegaCrit.Sts2.Core.Saves
{
	// Token: 0x02000093 RID: 147
	[NullableContext(1)]
	[Nullable(0)]
	public static class CorruptFileHandler
	{
		// Token: 0x060004DB RID: 1243 RVA: 0x00016B4C File Offset: 0x00014D4C
		public static string GenerateCorruptFilePath(string originalPath, ReadSaveStatus status)
		{
			int num = originalPath.LastIndexOf('/');
			if (num == -1)
			{
				num = originalPath.LastIndexOf('\\');
			}
			string text = ((num >= 0) ? originalPath.Substring(0, num) : "");
			string text2 = ((num >= 0) ? originalPath.Substring(num + 1) : originalPath);
			int num2 = text2.LastIndexOf('.');
			string text3 = ((num2 >= 0) ? text2.Substring(0, num2) : text2);
			string text4 = ((num2 >= 0) ? text2.Substring(num2) : "");
			long num3 = DateTimeOffset.UtcNow.ToUnixTimeSeconds();
			string reasonChar = CorruptFileHandler.GetReasonChar(status);
			DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(10, 3);
			defaultInterpolatedStringHandler.AppendFormatted(text3);
			defaultInterpolatedStringHandler.AppendLiteral(".");
			defaultInterpolatedStringHandler.AppendFormatted<long>(num3);
			defaultInterpolatedStringHandler.AppendLiteral(".");
			defaultInterpolatedStringHandler.AppendFormatted(reasonChar);
			defaultInterpolatedStringHandler.AppendLiteral(".corrupt");
			string text5 = defaultInterpolatedStringHandler.ToStringAndClear();
			if (string.IsNullOrEmpty(text))
			{
				return text5;
			}
			return text + "/" + text5;
		}

		// Token: 0x060004DC RID: 1244 RVA: 0x00016C44 File Offset: 0x00014E44
		private static string GetReasonChar(ReadSaveStatus status)
		{
			switch (status)
			{
			case ReadSaveStatus.JsonParseError:
				return "JSN";
			case ReadSaveStatus.FileEmpty:
				return "EMP";
			case ReadSaveStatus.MigrationFailed:
				return "MIG";
			case ReadSaveStatus.MissingSchemaVersion:
				return "SCH";
			case ReadSaveStatus.FutureVersion:
				return "FUT";
			case ReadSaveStatus.VersionTooOld:
				return "OLD";
			case ReadSaveStatus.FileAccessError:
				return "ACC";
			case ReadSaveStatus.Unrecoverable:
				return "UNR";
			case ReadSaveStatus.ValidationFailed:
				return "VAL";
			}
			return "UNK";
		}
	}
}
