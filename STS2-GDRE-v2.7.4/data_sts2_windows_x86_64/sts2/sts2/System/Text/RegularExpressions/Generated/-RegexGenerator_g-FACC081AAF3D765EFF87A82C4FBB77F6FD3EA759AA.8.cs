using System;
using System.CodeDom.Compiler;
using System.Runtime.CompilerServices;

namespace System.Text.RegularExpressions.Generated
{
	// Token: 0x02000014 RID: 20
	[GeneratedCode("System.Text.RegularExpressions.Generator", "9.0.12.31616")]
	[SkipLocalsInit]
	internal sealed class <RegexGenerator_g>FACC081AAF3D765EFF87A82C4FBB77F6FD3EA759AA2D03D993988F88E97CC0B5B__HtmlTags_7 : Regex
	{
		// Token: 0x060000A0 RID: 160 RVA: 0x00006550 File Offset: 0x00004750
		private <RegexGenerator_g>FACC081AAF3D765EFF87A82C4FBB77F6FD3EA759AA2D03D993988F88E97CC0B5B__HtmlTags_7()
		{
			this.pattern = "<.*?>";
			this.roptions = RegexOptions.None;
			Regex.ValidateMatchTimeout(<RegexGenerator_g>FACC081AAF3D765EFF87A82C4FBB77F6FD3EA759AA2D03D993988F88E97CC0B5B__Utilities.s_defaultTimeout);
			this.internalMatchTimeout = <RegexGenerator_g>FACC081AAF3D765EFF87A82C4FBB77F6FD3EA759AA2D03D993988F88E97CC0B5B__Utilities.s_defaultTimeout;
			this.factory = new <RegexGenerator_g>FACC081AAF3D765EFF87A82C4FBB77F6FD3EA759AA2D03D993988F88E97CC0B5B__HtmlTags_7.RunnerFactory();
			this.capsize = 1;
		}

		// Token: 0x0400003E RID: 62
		[Nullable(1)]
		internal static readonly <RegexGenerator_g>FACC081AAF3D765EFF87A82C4FBB77F6FD3EA759AA2D03D993988F88E97CC0B5B__HtmlTags_7 Instance = new <RegexGenerator_g>FACC081AAF3D765EFF87A82C4FBB77F6FD3EA759AA2D03D993988F88E97CC0B5B__HtmlTags_7();

		// Token: 0x02000DD2 RID: 3538
		private sealed class RunnerFactory : RegexRunnerFactory
		{
			// Token: 0x06008D0C RID: 36108 RVA: 0x002EF8D2 File Offset: 0x002EDAD2
			[NullableContext(1)]
			protected override RegexRunner CreateInstance()
			{
				return new <RegexGenerator_g>FACC081AAF3D765EFF87A82C4FBB77F6FD3EA759AA2D03D993988F88E97CC0B5B__HtmlTags_7.RunnerFactory.Runner();
			}

			// Token: 0x02002593 RID: 9619
			private sealed class Runner : RegexRunner
			{
				// Token: 0x0600C68D RID: 50829 RVA: 0x0044FAE5 File Offset: 0x0044DCE5
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

				// Token: 0x0600C68E RID: 50830 RVA: 0x0044FB28 File Offset: 0x0044DD28
				private bool TryFindNextPossibleStartingPosition(ReadOnlySpan<char> inputSpan)
				{
					int runtextpos = this.runtextpos;
					if (runtextpos <= inputSpan.Length - 2)
					{
						int num = inputSpan.Slice(runtextpos).IndexOf('<');
						if (num >= 0)
						{
							this.runtextpos = runtextpos + num;
							return true;
						}
					}
					this.runtextpos = inputSpan.Length;
					return false;
				}

				// Token: 0x0600C68F RID: 50831 RVA: 0x0044FB78 File Offset: 0x0044DD78
				private unsafe bool TryMatchAtCurrentPosition(ReadOnlySpan<char> inputSpan)
				{
					int num = this.runtextpos;
					int num2 = num;
					ReadOnlySpan<char> readOnlySpan = inputSpan.Slice(num);
					if (readOnlySpan.IsEmpty || *readOnlySpan[0] != 60)
					{
						return false;
					}
					num++;
					readOnlySpan = inputSpan.Slice(num);
					int num3 = num;
					while (readOnlySpan.IsEmpty || *readOnlySpan[0] != 62)
					{
						if (<RegexGenerator_g>FACC081AAF3D765EFF87A82C4FBB77F6FD3EA759AA2D03D993988F88E97CC0B5B__Utilities.s_hasTimeout)
						{
							base.CheckTimeout();
						}
						num = num3;
						readOnlySpan = inputSpan.Slice(num);
						if (readOnlySpan.IsEmpty || *readOnlySpan[0] == 10)
						{
							return false;
						}
						num++;
						readOnlySpan = inputSpan.Slice(num);
						num3 = readOnlySpan.IndexOfAny('\n', '>');
						if (num3 >= readOnlySpan.Length || *readOnlySpan[num3] == 10)
						{
							return false;
						}
						num += num3;
						readOnlySpan = inputSpan.Slice(num);
						num3 = num;
					}
					num++;
					this.runtextpos = num;
					base.Capture(0, num2, num);
					return true;
				}
			}
		}
	}
}
