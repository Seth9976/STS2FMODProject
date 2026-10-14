using System;
using System.CodeDom.Compiler;
using System.Runtime.CompilerServices;

namespace System.Text.RegularExpressions.Generated
{
	// Token: 0x0200000F RID: 15
	[GeneratedCode("System.Text.RegularExpressions.Generator", "9.0.12.31616")]
	[SkipLocalsInit]
	internal sealed class <RegexGenerator_g>FACC081AAF3D765EFF87A82C4FBB77F6FD3EA759AA2D03D993988F88E97CC0B5B__WhitespaceRegex_2 : Regex
	{
		// Token: 0x06000096 RID: 150 RVA: 0x00006398 File Offset: 0x00004598
		private <RegexGenerator_g>FACC081AAF3D765EFF87A82C4FBB77F6FD3EA759AA2D03D993988F88E97CC0B5B__WhitespaceRegex_2()
		{
			this.pattern = "\\s+";
			this.roptions = RegexOptions.None;
			Regex.ValidateMatchTimeout(<RegexGenerator_g>FACC081AAF3D765EFF87A82C4FBB77F6FD3EA759AA2D03D993988F88E97CC0B5B__Utilities.s_defaultTimeout);
			this.internalMatchTimeout = <RegexGenerator_g>FACC081AAF3D765EFF87A82C4FBB77F6FD3EA759AA2D03D993988F88E97CC0B5B__Utilities.s_defaultTimeout;
			this.factory = new <RegexGenerator_g>FACC081AAF3D765EFF87A82C4FBB77F6FD3EA759AA2D03D993988F88E97CC0B5B__WhitespaceRegex_2.RunnerFactory();
			this.capsize = 1;
		}

		// Token: 0x04000039 RID: 57
		[Nullable(1)]
		internal static readonly <RegexGenerator_g>FACC081AAF3D765EFF87A82C4FBB77F6FD3EA759AA2D03D993988F88E97CC0B5B__WhitespaceRegex_2 Instance = new <RegexGenerator_g>FACC081AAF3D765EFF87A82C4FBB77F6FD3EA759AA2D03D993988F88E97CC0B5B__WhitespaceRegex_2();

		// Token: 0x02000DCD RID: 3533
		private sealed class RunnerFactory : RegexRunnerFactory
		{
			// Token: 0x06008D02 RID: 36098 RVA: 0x002EF887 File Offset: 0x002EDA87
			[NullableContext(1)]
			protected override RegexRunner CreateInstance()
			{
				return new <RegexGenerator_g>FACC081AAF3D765EFF87A82C4FBB77F6FD3EA759AA2D03D993988F88E97CC0B5B__WhitespaceRegex_2.RunnerFactory.Runner();
			}

			// Token: 0x0200258E RID: 9614
			private sealed class Runner : RegexRunner
			{
				// Token: 0x0600C67B RID: 50811 RVA: 0x0044F5AF File Offset: 0x0044D7AF
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

				// Token: 0x0600C67C RID: 50812 RVA: 0x0044F5F0 File Offset: 0x0044D7F0
				private bool TryFindNextPossibleStartingPosition(ReadOnlySpan<char> inputSpan)
				{
					int runtextpos = this.runtextpos;
					if (runtextpos < inputSpan.Length)
					{
						int num = inputSpan.Slice(runtextpos).IndexOfAny<char>(<RegexGenerator_g>FACC081AAF3D765EFF87A82C4FBB77F6FD3EA759AA2D03D993988F88E97CC0B5B__Utilities.s_whitespace);
						if (num >= 0)
						{
							this.runtextpos = runtextpos + num;
							return true;
						}
					}
					this.runtextpos = inputSpan.Length;
					return false;
				}

				// Token: 0x0600C67D RID: 50813 RVA: 0x0044F640 File Offset: 0x0044D840
				private unsafe bool TryMatchAtCurrentPosition(ReadOnlySpan<char> inputSpan)
				{
					int num = this.runtextpos;
					int num2 = num;
					ReadOnlySpan<char> readOnlySpan = inputSpan.Slice(num);
					int num3 = 0;
					while (num3 < readOnlySpan.Length && char.IsWhiteSpace((char)(*readOnlySpan[num3])))
					{
						num3++;
					}
					if (num3 == 0)
					{
						return false;
					}
					readOnlySpan = readOnlySpan.Slice(num3);
					num += num3;
					this.runtextpos = num;
					base.Capture(0, num2, num);
					return true;
				}
			}
		}
	}
}
