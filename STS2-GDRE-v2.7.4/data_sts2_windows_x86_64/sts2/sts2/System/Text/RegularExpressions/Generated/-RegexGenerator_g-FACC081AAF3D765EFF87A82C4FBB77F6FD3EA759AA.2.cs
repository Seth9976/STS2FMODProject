using System;
using System.CodeDom.Compiler;
using System.Runtime.CompilerServices;

namespace System.Text.RegularExpressions.Generated
{
	// Token: 0x0200000E RID: 14
	[GeneratedCode("System.Text.RegularExpressions.Generator", "9.0.12.31616")]
	[SkipLocalsInit]
	internal sealed class <RegexGenerator_g>FACC081AAF3D765EFF87A82C4FBB77F6FD3EA759AA2D03D993988F88E97CC0B5B__SnakeCaseRegex_1 : Regex
	{
		// Token: 0x06000094 RID: 148 RVA: 0x00006340 File Offset: 0x00004540
		private <RegexGenerator_g>FACC081AAF3D765EFF87A82C4FBB77F6FD3EA759AA2D03D993988F88E97CC0B5B__SnakeCaseRegex_1()
		{
			this.pattern = "(.*?)_([a-zA-Z0-9])";
			this.roptions = RegexOptions.None;
			Regex.ValidateMatchTimeout(<RegexGenerator_g>FACC081AAF3D765EFF87A82C4FBB77F6FD3EA759AA2D03D993988F88E97CC0B5B__Utilities.s_defaultTimeout);
			this.internalMatchTimeout = <RegexGenerator_g>FACC081AAF3D765EFF87A82C4FBB77F6FD3EA759AA2D03D993988F88E97CC0B5B__Utilities.s_defaultTimeout;
			this.factory = new <RegexGenerator_g>FACC081AAF3D765EFF87A82C4FBB77F6FD3EA759AA2D03D993988F88E97CC0B5B__SnakeCaseRegex_1.RunnerFactory();
			this.capsize = 3;
		}

		// Token: 0x04000038 RID: 56
		[Nullable(1)]
		internal static readonly <RegexGenerator_g>FACC081AAF3D765EFF87A82C4FBB77F6FD3EA759AA2D03D993988F88E97CC0B5B__SnakeCaseRegex_1 Instance = new <RegexGenerator_g>FACC081AAF3D765EFF87A82C4FBB77F6FD3EA759AA2D03D993988F88E97CC0B5B__SnakeCaseRegex_1();

		// Token: 0x02000DCC RID: 3532
		private sealed class RunnerFactory : RegexRunnerFactory
		{
			// Token: 0x06008D00 RID: 36096 RVA: 0x002EF878 File Offset: 0x002EDA78
			[NullableContext(1)]
			protected override RegexRunner CreateInstance()
			{
				return new <RegexGenerator_g>FACC081AAF3D765EFF87A82C4FBB77F6FD3EA759AA2D03D993988F88E97CC0B5B__SnakeCaseRegex_1.RunnerFactory.Runner();
			}

			// Token: 0x0200258D RID: 9613
			private sealed class Runner : RegexRunner
			{
				// Token: 0x0600C676 RID: 50806 RVA: 0x0044F3A7 File Offset: 0x0044D5A7
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

				// Token: 0x0600C677 RID: 50807 RVA: 0x0044F3E8 File Offset: 0x0044D5E8
				private unsafe bool TryFindNextPossibleStartingPosition(ReadOnlySpan<char> inputSpan)
				{
					int runtextpos = this.runtextpos;
					if (runtextpos <= inputSpan.Length - 2)
					{
						ReadOnlySpan<char> readOnlySpan = inputSpan.Slice(runtextpos);
						for (int i = 0; i < readOnlySpan.Length; i++)
						{
							if (*readOnlySpan[i] != 10)
							{
								this.runtextpos = runtextpos + i;
								return true;
							}
						}
					}
					this.runtextpos = inputSpan.Length;
					return false;
				}

				// Token: 0x0600C678 RID: 50808 RVA: 0x0044F44C File Offset: 0x0044D64C
				private unsafe bool TryMatchAtCurrentPosition(ReadOnlySpan<char> inputSpan)
				{
					int num = this.runtextpos;
					int num2 = num;
					ReadOnlySpan<char> readOnlySpan = inputSpan.Slice(num);
					int num3 = num;
					int num4 = num;
					int num6;
					for (;;)
					{
						int num5 = base.Crawlpos();
						base.Capture(1, num3, num);
						if (!readOnlySpan.IsEmpty && *readOnlySpan[0] == 95)
						{
							num++;
							readOnlySpan = inputSpan.Slice(num);
							num6 = num;
							if (!readOnlySpan.IsEmpty && char.IsAsciiLetterOrDigit((char)(*readOnlySpan[0])))
							{
								goto Block_7;
							}
						}
						this.<TryMatchAtCurrentPosition>g__UncaptureUntil|2_0(num5);
						if (<RegexGenerator_g>FACC081AAF3D765EFF87A82C4FBB77F6FD3EA759AA2D03D993988F88E97CC0B5B__Utilities.s_hasTimeout)
						{
							base.CheckTimeout();
						}
						num = num4;
						readOnlySpan = inputSpan.Slice(num);
						if (readOnlySpan.IsEmpty || *readOnlySpan[0] == 10)
						{
							break;
						}
						num++;
						readOnlySpan = inputSpan.Slice(num);
						num4 = readOnlySpan.IndexOfAny('\n', '_');
						if (num4 >= readOnlySpan.Length || *readOnlySpan[num4] == 10)
						{
							goto IL_009C;
						}
						num += num4;
						readOnlySpan = inputSpan.Slice(num);
						num4 = num;
					}
					this.<TryMatchAtCurrentPosition>g__UncaptureUntil|2_0(0);
					return false;
					IL_009C:
					this.<TryMatchAtCurrentPosition>g__UncaptureUntil|2_0(0);
					return false;
					Block_7:
					num++;
					readOnlySpan = inputSpan.Slice(num);
					base.Capture(2, num6, num);
					this.runtextpos = num;
					base.Capture(0, num2, num);
					return true;
				}

				// Token: 0x0600C67A RID: 50810 RVA: 0x0044F59C File Offset: 0x0044D79C
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
