using System;
using System.IO;
using System.IO.Compression;
using System.Runtime.CompilerServices;
using System.Text;
using MegaCrit.Sts2.Core.Logging;
using Sentry;

namespace MegaCrit.Sts2.Core.Debug
{
	// Token: 0x02000D17 RID: 3351
	[NullableContext(1)]
	[Nullable(0)]
	public static class SentryExtension
	{
		// Token: 0x06008734 RID: 34612 RVA: 0x002D76DC File Offset: 0x002D58DC
		public static void AddCompressedAttachment(this Scope scope, string text, string fileName)
		{
			byte[] array = SentryExtension.GzipCompress(text);
			if (array.Length <= 102400)
			{
				scope.AddAttachment(array, fileName, 0, null);
				return;
			}
			DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(50, 3);
			defaultInterpolatedStringHandler.AppendLiteral("Skipping Sentry attachment ");
			defaultInterpolatedStringHandler.AppendFormatted(fileName);
			defaultInterpolatedStringHandler.AppendLiteral(": ");
			defaultInterpolatedStringHandler.AppendFormatted<int>(array.Length / 1024);
			defaultInterpolatedStringHandler.AppendLiteral(" KB exceeds ");
			defaultInterpolatedStringHandler.AppendFormatted<int>(100);
			defaultInterpolatedStringHandler.AppendLiteral(" KB limit");
			Log.Warn(defaultInterpolatedStringHandler.ToStringAndClear(), 2);
		}

		// Token: 0x06008735 RID: 34613 RVA: 0x002D7770 File Offset: 0x002D5970
		private static byte[] GzipCompress(string text)
		{
			byte[] bytes = Encoding.UTF8.GetBytes(text);
			byte[] array;
			using (MemoryStream memoryStream = new MemoryStream())
			{
				using (GZipStream gzipStream = new GZipStream(memoryStream, CompressionLevel.Optimal, true))
				{
					gzipStream.Write(bytes, 0, bytes.Length);
				}
				array = memoryStream.ToArray();
			}
			return array;
		}
	}
}
