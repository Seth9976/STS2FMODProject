using System;
using System.CodeDom.Compiler;
using System.Runtime.CompilerServices;

namespace System.Text.RegularExpressions.Generated
{
	// Token: 0x02000013 RID: 19
	[GeneratedCode("System.Text.RegularExpressions.Generator", "9.0.12.31616")]
	[SkipLocalsInit]
	internal sealed class <RegexGenerator_g>FACC081AAF3D765EFF87A82C4FBB77F6FD3EA759AA2D03D993988F88E97CC0B5B__ConsecutiveSpaces_6 : Regex
	{
		// Token: 0x0600009E RID: 158 RVA: 0x000064F8 File Offset: 0x000046F8
		private <RegexGenerator_g>FACC081AAF3D765EFF87A82C4FBB77F6FD3EA759AA2D03D993988F88E97CC0B5B__ConsecutiveSpaces_6()
		{
			this.pattern = "\\s{2,}";
			this.roptions = RegexOptions.None;
			Regex.ValidateMatchTimeout(<RegexGenerator_g>FACC081AAF3D765EFF87A82C4FBB77F6FD3EA759AA2D03D993988F88E97CC0B5B__Utilities.s_defaultTimeout);
			this.internalMatchTimeout = <RegexGenerator_g>FACC081AAF3D765EFF87A82C4FBB77F6FD3EA759AA2D03D993988F88E97CC0B5B__Utilities.s_defaultTimeout;
			this.factory = new <RegexGenerator_g>FACC081AAF3D765EFF87A82C4FBB77F6FD3EA759AA2D03D993988F88E97CC0B5B__ConsecutiveSpaces_6.RunnerFactory();
			this.capsize = 1;
		}

		// Token: 0x0400003D RID: 61
		[Nullable(1)]
		internal static readonly <RegexGenerator_g>FACC081AAF3D765EFF87A82C4FBB77F6FD3EA759AA2D03D993988F88E97CC0B5B__ConsecutiveSpaces_6 Instance = new <RegexGenerator_g>FACC081AAF3D765EFF87A82C4FBB77F6FD3EA759AA2D03D993988F88E97CC0B5B__ConsecutiveSpaces_6();

		// Token: 0x02000DD1 RID: 3537
		private sealed class RunnerFactory : RegexRunnerFactory
		{
			// Token: 0x06008D0A RID: 36106 RVA: 0x002EF8C3 File Offset: 0x002EDAC3
			[NullableContext(1)]
			protected override RegexRunner CreateInstance()
			{
				return new <RegexGenerator_g>FACC081AAF3D765EFF87A82C4FBB77F6FD3EA759AA2D03D993988F88E97CC0B5B__ConsecutiveSpaces_6.RunnerFactory.Runner();
			}

			// Token: 0x02002592 RID: 9618
			private sealed class Runner : RegexRunner
			{
				// Token: 0x0600C689 RID: 50825 RVA: 0x0044F9A7 File Offset: 0x0044DBA7
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

				// Token: 0x0600C68A RID: 50826 RVA: 0x0044F9E8 File Offset: 0x0044DBE8
				private unsafe bool TryFindNextPossibleStartingPosition(ReadOnlySpan<char> inputSpan)
				{
					int runtextpos = this.runtextpos;
					if (runtextpos <= inputSpan.Length - 2)
					{
						ReadOnlySpan<char> readOnlySpan = inputSpan.Slice(runtextpos);
						for (int i = 0; i < readOnlySpan.Length - 1; i++)
						{
							int num = readOnlySpan.Slice(i).IndexOfAny<char>(<RegexGenerator_g>FACC081AAF3D765EFF87A82C4FBB77F6FD3EA759AA2D03D993988F88E97CC0B5B__Utilities.s_whitespace);
							if (num < 0)
							{
								break;
							}
							i += num;
							if (i + 1 >= readOnlySpan.Length)
							{
								break;
							}
							if (char.IsWhiteSpace((char)(*readOnlySpan[i + 1])))
							{
								this.runtextpos = runtextpos + i;
								return true;
							}
						}
					}
					this.runtextpos = inputSpan.Length;
					return false;
				}

				// Token: 0x0600C68B RID: 50827 RVA: 0x0044FA78 File Offset: 0x0044DC78
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
					if (num3 < 2)
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
