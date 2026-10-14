using System;
using System.Buffers;
using System.CodeDom.Compiler;
using System.Globalization;
using System.Runtime.CompilerServices;

namespace System.Text.RegularExpressions.Generated
{
	// Token: 0x02000015 RID: 21
	[NullableContext(1)]
	[Nullable(0)]
	[GeneratedCode("System.Text.RegularExpressions.Generator", "9.0.12.31616")]
	internal static class <RegexGenerator_g>FACC081AAF3D765EFF87A82C4FBB77F6FD3EA759AA2D03D993988F88E97CC0B5B__Utilities
	{
		// Token: 0x060000A2 RID: 162 RVA: 0x000065A8 File Offset: 0x000047A8
		[NullableContext(0)]
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		internal unsafe static bool IsBoundary(ReadOnlySpan<char> inputSpan, int index)
		{
			int num = index - 1;
			return (num < inputSpan.Length && <RegexGenerator_g>FACC081AAF3D765EFF87A82C4FBB77F6FD3EA759AA2D03D993988F88E97CC0B5B__Utilities.<IsBoundary>g__IsBoundaryWordChar|2_0((char)(*inputSpan[num]))) != (index < inputSpan.Length && <RegexGenerator_g>FACC081AAF3D765EFF87A82C4FBB77F6FD3EA759AA2D03D993988F88E97CC0B5B__Utilities.<IsBoundary>g__IsBoundaryWordChar|2_0((char)(*inputSpan[index])));
		}

		// Token: 0x060000A3 RID: 163 RVA: 0x000065F4 File Offset: 0x000047F4
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		internal unsafe static bool IsWordChar(char ch)
		{
			ReadOnlySpan<byte> readOnlySpan = new ReadOnlySpan<byte>((void*)(&<PrivateImplementationDetails>.3C17DA885F7916F14BECDDD5A559E146795A03758C28EA455EC8E9E803B90531), 16);
			int num = (int)(ch >> 3);
			if (num >= readOnlySpan.Length)
			{
				return (262463 & (1 << (int)CharUnicodeInfo.GetUnicodeCategory(ch))) != 0;
			}
			return ((int)(*readOnlySpan[num]) & (1 << (int)(ch & '\a'))) != 0;
		}

		// Token: 0x060000A4 RID: 164 RVA: 0x00006648 File Offset: 0x00004848
		// Note: this type is marked as 'beforefieldinit'.
		static <RegexGenerator_g>FACC081AAF3D765EFF87A82C4FBB77F6FD3EA759AA2D03D993988F88E97CC0B5B__Utilities()
		{
			object data = AppContext.GetData("REGEX_DEFAULT_MATCH_TIMEOUT");
			TimeSpan timeSpan2;
			if (data is TimeSpan)
			{
				TimeSpan timeSpan = (TimeSpan)data;
				timeSpan2 = timeSpan;
			}
			else
			{
				timeSpan2 = Regex.InfiniteMatchTimeout;
			}
			<RegexGenerator_g>FACC081AAF3D765EFF87A82C4FBB77F6FD3EA759AA2D03D993988F88E97CC0B5B__Utilities.s_defaultTimeout = timeSpan2;
			<RegexGenerator_g>FACC081AAF3D765EFF87A82C4FBB77F6FD3EA759AA2D03D993988F88E97CC0B5B__Utilities.s_hasTimeout = <RegexGenerator_g>FACC081AAF3D765EFF87A82C4FBB77F6FD3EA759AA2D03D993988F88E97CC0B5B__Utilities.s_defaultTimeout != Regex.InfiniteMatchTimeout;
			<RegexGenerator_g>FACC081AAF3D765EFF87A82C4FBB77F6FD3EA759AA2D03D993988F88E97CC0B5B__Utilities.s_asciiLettersAndDigits = SearchValues.Create("0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz");
			<RegexGenerator_g>FACC081AAF3D765EFF87A82C4FBB77F6FD3EA759AA2D03D993988F88E97CC0B5B__Utilities.s_ascii_FF03FEFFFF8700000000 = SearchValues.Create("0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZ_");
			string text = "76561";
			<RegexGenerator_g>FACC081AAF3D765EFF87A82C4FBB77F6FD3EA759AA2D03D993988F88E97CC0B5B__Utilities.s_indexOfString_76561_Ordinal = SearchValues.Create(new ReadOnlySpan<string>(ref text), StringComparison.Ordinal);
			<RegexGenerator_g>FACC081AAF3D765EFF87A82C4FBB77F6FD3EA759AA2D03D993988F88E97CC0B5B__Utilities.s_whitespace = SearchValues.Create("\t\n\v\f\r \u0085\u00a0\u1680\u2000\u2001\u2002\u2003\u2004\u2005\u2006\u2007\u2008\u2009\u200a\u2028\u2029\u202f\u205f\u3000");
		}

		// Token: 0x060000A5 RID: 165 RVA: 0x000066E6 File Offset: 0x000048E6
		[CompilerGenerated]
		internal static bool <IsBoundary>g__IsBoundaryWordChar|2_0(char ch)
		{
			return <RegexGenerator_g>FACC081AAF3D765EFF87A82C4FBB77F6FD3EA759AA2D03D993988F88E97CC0B5B__Utilities.IsWordChar(ch) || ((ch == '\u200c') | (ch == '\u200d'));
		}

		// Token: 0x0400003F RID: 63
		internal static readonly TimeSpan s_defaultTimeout;

		// Token: 0x04000040 RID: 64
		internal static readonly bool s_hasTimeout;

		// Token: 0x04000041 RID: 65
		internal static readonly SearchValues<char> s_asciiLettersAndDigits;

		// Token: 0x04000042 RID: 66
		internal static readonly SearchValues<char> s_ascii_FF03FEFFFF8700000000;

		// Token: 0x04000043 RID: 67
		internal static readonly SearchValues<string> s_indexOfString_76561_Ordinal;

		// Token: 0x04000044 RID: 68
		internal static readonly SearchValues<char> s_whitespace;
	}
}
