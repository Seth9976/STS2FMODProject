using System;
using System.CodeDom.Compiler;
using System.Runtime.CompilerServices;

namespace System.Text.RegularExpressions.Generated
{
	// Token: 0x02000011 RID: 17
	[GeneratedCode("System.Text.RegularExpressions.Generator", "9.0.12.31616")]
	[SkipLocalsInit]
	internal sealed class <RegexGenerator_g>FACC081AAF3D765EFF87A82C4FBB77F6FD3EA759AA2D03D993988F88E97CC0B5B__SteamIdRegex_4 : Regex
	{
		// Token: 0x0600009A RID: 154 RVA: 0x00006448 File Offset: 0x00004648
		private <RegexGenerator_g>FACC081AAF3D765EFF87A82C4FBB77F6FD3EA759AA2D03D993988F88E97CC0B5B__SteamIdRegex_4()
		{
			this.pattern = "\\b76561\\d{12}\\b";
			this.roptions = RegexOptions.None;
			Regex.ValidateMatchTimeout(<RegexGenerator_g>FACC081AAF3D765EFF87A82C4FBB77F6FD3EA759AA2D03D993988F88E97CC0B5B__Utilities.s_defaultTimeout);
			this.internalMatchTimeout = <RegexGenerator_g>FACC081AAF3D765EFF87A82C4FBB77F6FD3EA759AA2D03D993988F88E97CC0B5B__Utilities.s_defaultTimeout;
			this.factory = new <RegexGenerator_g>FACC081AAF3D765EFF87A82C4FBB77F6FD3EA759AA2D03D993988F88E97CC0B5B__SteamIdRegex_4.RunnerFactory();
			this.capsize = 1;
		}

		// Token: 0x0400003B RID: 59
		[Nullable(1)]
		internal static readonly <RegexGenerator_g>FACC081AAF3D765EFF87A82C4FBB77F6FD3EA759AA2D03D993988F88E97CC0B5B__SteamIdRegex_4 Instance = new <RegexGenerator_g>FACC081AAF3D765EFF87A82C4FBB77F6FD3EA759AA2D03D993988F88E97CC0B5B__SteamIdRegex_4();

		// Token: 0x02000DCF RID: 3535
		private sealed class RunnerFactory : RegexRunnerFactory
		{
			// Token: 0x06008D06 RID: 36102 RVA: 0x002EF8A5 File Offset: 0x002EDAA5
			[NullableContext(1)]
			protected override RegexRunner CreateInstance()
			{
				return new <RegexGenerator_g>FACC081AAF3D765EFF87A82C4FBB77F6FD3EA759AA2D03D993988F88E97CC0B5B__SteamIdRegex_4.RunnerFactory.Runner();
			}

			// Token: 0x02002590 RID: 9616
			private sealed class Runner : RegexRunner
			{
				// Token: 0x0600C682 RID: 50818 RVA: 0x0044F736 File Offset: 0x0044D936
				protected override void Scan(ReadOnlySpan<char> inputSpan)
				{
					while (this.TryFindNextPossibleStartingPosition(inputSpan) && !this.TryMatchAtCurrentPosition(inputSpan) && this.runtextpos != inputSpan.Length)
					{
						this.runtextpos++;
						if (<RegexGenerator_g>FACC081AAF3D765EFF87A82C4FBB77F6FD3EA759AA2D03D993988F88E97CC0B5B__Utilities.s_hasTimeout)
						{
							base.CheckTimeout();
						}
					}
				}

				// Token: 0x0600C683 RID: 50819 RVA: 0x0044F778 File Offset: 0x0044D978
				private bool TryFindNextPossibleStartingPosition(ReadOnlySpan<char> inputSpan)
				{
					int runtextpos = this.runtextpos;
					if (runtextpos <= inputSpan.Length - 17)
					{
						int num = inputSpan.Slice(runtextpos).IndexOfAny(<RegexGenerator_g>FACC081AAF3D765EFF87A82C4FBB77F6FD3EA759AA2D03D993988F88E97CC0B5B__Utilities.s_indexOfString_76561_Ordinal);
						if (num >= 0)
						{
							this.runtextpos = runtextpos + num;
							return true;
						}
					}
					this.runtextpos = inputSpan.Length;
					return false;
				}

				// Token: 0x0600C684 RID: 50820 RVA: 0x0044F7CC File Offset: 0x0044D9CC
				private unsafe bool TryMatchAtCurrentPosition(ReadOnlySpan<char> inputSpan)
				{
					int num = this.runtextpos;
					int num2 = num;
					ReadOnlySpan<char> readOnlySpan = inputSpan.Slice(num);
					if (!<RegexGenerator_g>FACC081AAF3D765EFF87A82C4FBB77F6FD3EA759AA2D03D993988F88E97CC0B5B__Utilities.IsBoundary(inputSpan, num))
					{
						return false;
					}
					if (readOnlySpan.Length < 17 || !readOnlySpan.StartsWith("76561", StringComparison.OrdinalIgnoreCase) || !char.IsDigit((char)(*readOnlySpan[5])) || !char.IsDigit((char)(*readOnlySpan[6])) || !char.IsDigit((char)(*readOnlySpan[7])) || !char.IsDigit((char)(*readOnlySpan[8])) || !char.IsDigit((char)(*readOnlySpan[9])) || !char.IsDigit((char)(*readOnlySpan[10])) || !char.IsDigit((char)(*readOnlySpan[11])) || !char.IsDigit((char)(*readOnlySpan[12])) || !char.IsDigit((char)(*readOnlySpan[13])) || !char.IsDigit((char)(*readOnlySpan[14])) || !char.IsDigit((char)(*readOnlySpan[15])) || !char.IsDigit((char)(*readOnlySpan[16])))
					{
						return false;
					}
					if (!<RegexGenerator_g>FACC081AAF3D765EFF87A82C4FBB77F6FD3EA759AA2D03D993988F88E97CC0B5B__Utilities.IsBoundary(inputSpan, num + 17))
					{
						return false;
					}
					num += 17;
					this.runtextpos = num;
					base.Capture(0, num2, num);
					return true;
				}
			}
		}
	}
}
