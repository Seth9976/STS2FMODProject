using System;
using System.CodeDom.Compiler;
using System.Runtime.CompilerServices;

namespace System.Text.RegularExpressions.Generated
{
	// Token: 0x0200000D RID: 13
	[GeneratedCode("System.Text.RegularExpressions.Generator", "9.0.12.31616")]
	[SkipLocalsInit]
	internal sealed class <RegexGenerator_g>FACC081AAF3D765EFF87A82C4FBB77F6FD3EA759AA2D03D993988F88E97CC0B5B__CamelCaseRegex_0 : Regex
	{
		// Token: 0x06000092 RID: 146 RVA: 0x000062E8 File Offset: 0x000044E8
		private <RegexGenerator_g>FACC081AAF3D765EFF87A82C4FBB77F6FD3EA759AA2D03D993988F88E97CC0B5B__CamelCaseRegex_0()
		{
			this.pattern = "([A-Za-z0-9]|\\G(?!^))([A-Z])";
			this.roptions = RegexOptions.None;
			Regex.ValidateMatchTimeout(<RegexGenerator_g>FACC081AAF3D765EFF87A82C4FBB77F6FD3EA759AA2D03D993988F88E97CC0B5B__Utilities.s_defaultTimeout);
			this.internalMatchTimeout = <RegexGenerator_g>FACC081AAF3D765EFF87A82C4FBB77F6FD3EA759AA2D03D993988F88E97CC0B5B__Utilities.s_defaultTimeout;
			this.factory = new <RegexGenerator_g>FACC081AAF3D765EFF87A82C4FBB77F6FD3EA759AA2D03D993988F88E97CC0B5B__CamelCaseRegex_0.RunnerFactory();
			this.capsize = 3;
		}

		// Token: 0x04000037 RID: 55
		[Nullable(1)]
		internal static readonly <RegexGenerator_g>FACC081AAF3D765EFF87A82C4FBB77F6FD3EA759AA2D03D993988F88E97CC0B5B__CamelCaseRegex_0 Instance = new <RegexGenerator_g>FACC081AAF3D765EFF87A82C4FBB77F6FD3EA759AA2D03D993988F88E97CC0B5B__CamelCaseRegex_0();

		// Token: 0x02000DCB RID: 3531
		private sealed class RunnerFactory : RegexRunnerFactory
		{
			// Token: 0x06008CFE RID: 36094 RVA: 0x002EF869 File Offset: 0x002EDA69
			[NullableContext(1)]
			protected override RegexRunner CreateInstance()
			{
				return new <RegexGenerator_g>FACC081AAF3D765EFF87A82C4FBB77F6FD3EA759AA2D03D993988F88E97CC0B5B__CamelCaseRegex_0.RunnerFactory.Runner();
			}

			// Token: 0x0200258C RID: 9612
			private sealed class Runner : RegexRunner
			{
				// Token: 0x0600C671 RID: 50801 RVA: 0x0044F1D8 File Offset: 0x0044D3D8
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

				// Token: 0x0600C672 RID: 50802 RVA: 0x0044F218 File Offset: 0x0044D418
				private bool TryFindNextPossibleStartingPosition(ReadOnlySpan<char> inputSpan)
				{
					int runtextpos = this.runtextpos;
					if (runtextpos < inputSpan.Length)
					{
						int num = inputSpan.Slice(runtextpos).IndexOfAny<char>(<RegexGenerator_g>FACC081AAF3D765EFF87A82C4FBB77F6FD3EA759AA2D03D993988F88E97CC0B5B__Utilities.s_asciiLettersAndDigits);
						if (num >= 0)
						{
							this.runtextpos = runtextpos + num;
							return true;
						}
					}
					this.runtextpos = inputSpan.Length;
					return false;
				}

				// Token: 0x0600C673 RID: 50803 RVA: 0x0044F268 File Offset: 0x0044D468
				private unsafe bool TryMatchAtCurrentPosition(ReadOnlySpan<char> inputSpan)
				{
					int num = this.runtextpos;
					int num2 = num;
					ReadOnlySpan<char> readOnlySpan = inputSpan.Slice(num);
					int num3 = num;
					int num4 = num;
					int num5 = base.Crawlpos();
					if (readOnlySpan.IsEmpty || !char.IsAsciiLetterOrDigit((char)(*readOnlySpan[0])))
					{
						goto IL_0058;
					}
					int num6 = 0;
					num++;
					readOnlySpan = inputSpan.Slice(num);
					int num7;
					do
					{
						IL_00C8:
						base.Capture(1, num3, num);
						num7 = num;
						if (!readOnlySpan.IsEmpty && char.IsAsciiLetterUpper((char)(*readOnlySpan[0])))
						{
							goto Block_10;
						}
						if (<RegexGenerator_g>FACC081AAF3D765EFF87A82C4FBB77F6FD3EA759AA2D03D993988F88E97CC0B5B__Utilities.s_hasTimeout)
						{
							base.CheckTimeout();
						}
						if (num6 == 0)
						{
							goto IL_0058;
						}
					}
					while (num6 != 1);
					this.<TryMatchAtCurrentPosition>g__UncaptureUntil|2_0(0);
					return false;
					Block_10:
					num++;
					readOnlySpan = inputSpan.Slice(num);
					base.Capture(2, num7, num);
					this.runtextpos = num;
					base.Capture(0, num2, num);
					return true;
					IL_0058:
					num = num4;
					readOnlySpan = inputSpan.Slice(num);
					this.<TryMatchAtCurrentPosition>g__UncaptureUntil|2_0(num5);
					if (num != this.runtextstart)
					{
						this.<TryMatchAtCurrentPosition>g__UncaptureUntil|2_0(0);
						return false;
					}
					int num8 = num;
					if (<RegexGenerator_g>FACC081AAF3D765EFF87A82C4FBB77F6FD3EA759AA2D03D993988F88E97CC0B5B__Utilities.s_hasTimeout)
					{
						base.CheckTimeout();
					}
					if (num == 0)
					{
						this.<TryMatchAtCurrentPosition>g__UncaptureUntil|2_0(0);
						return false;
					}
					num = num8;
					readOnlySpan = inputSpan.Slice(num);
					num6 = 1;
					goto IL_00C8;
				}

				// Token: 0x0600C675 RID: 50805 RVA: 0x0044F394 File Offset: 0x0044D594
				[CompilerGenerated]
				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				private void <TryMatchAtCurrentPosition>g__UncaptureUntil|2_0(int capturePosition)
				{
					while (base.Crawlpos() > capturePosition)
					{
						base.Uncapture();
					}
				}
			}
		}
	}
}
