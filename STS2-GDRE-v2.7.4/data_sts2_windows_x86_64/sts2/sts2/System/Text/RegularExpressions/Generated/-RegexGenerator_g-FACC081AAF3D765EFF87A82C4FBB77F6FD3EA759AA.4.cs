using System;
using System.CodeDom.Compiler;
using System.Runtime.CompilerServices;

namespace System.Text.RegularExpressions.Generated
{
	// Token: 0x02000010 RID: 16
	[GeneratedCode("System.Text.RegularExpressions.Generator", "9.0.12.31616")]
	[SkipLocalsInit]
	internal sealed class <RegexGenerator_g>FACC081AAF3D765EFF87A82C4FBB77F6FD3EA759AA2D03D993988F88E97CC0B5B__SpecialCharRegex_3 : Regex
	{
		// Token: 0x06000098 RID: 152 RVA: 0x000063F0 File Offset: 0x000045F0
		private <RegexGenerator_g>FACC081AAF3D765EFF87A82C4FBB77F6FD3EA759AA2D03D993988F88E97CC0B5B__SpecialCharRegex_3()
		{
			this.pattern = "[^A-Z0-9_]";
			this.roptions = RegexOptions.None;
			Regex.ValidateMatchTimeout(<RegexGenerator_g>FACC081AAF3D765EFF87A82C4FBB77F6FD3EA759AA2D03D993988F88E97CC0B5B__Utilities.s_defaultTimeout);
			this.internalMatchTimeout = <RegexGenerator_g>FACC081AAF3D765EFF87A82C4FBB77F6FD3EA759AA2D03D993988F88E97CC0B5B__Utilities.s_defaultTimeout;
			this.factory = new <RegexGenerator_g>FACC081AAF3D765EFF87A82C4FBB77F6FD3EA759AA2D03D993988F88E97CC0B5B__SpecialCharRegex_3.RunnerFactory();
			this.capsize = 1;
		}

		// Token: 0x0400003A RID: 58
		[Nullable(1)]
		internal static readonly <RegexGenerator_g>FACC081AAF3D765EFF87A82C4FBB77F6FD3EA759AA2D03D993988F88E97CC0B5B__SpecialCharRegex_3 Instance = new <RegexGenerator_g>FACC081AAF3D765EFF87A82C4FBB77F6FD3EA759AA2D03D993988F88E97CC0B5B__SpecialCharRegex_3();

		// Token: 0x02000DCE RID: 3534
		private sealed class RunnerFactory : RegexRunnerFactory
		{
			// Token: 0x06008D04 RID: 36100 RVA: 0x002EF896 File Offset: 0x002EDA96
			[NullableContext(1)]
			protected override RegexRunner CreateInstance()
			{
				return new <RegexGenerator_g>FACC081AAF3D765EFF87A82C4FBB77F6FD3EA759AA2D03D993988F88E97CC0B5B__SpecialCharRegex_3.RunnerFactory.Runner();
			}

			// Token: 0x0200258F RID: 9615
			private sealed class Runner : RegexRunner
			{
				// Token: 0x0600C67F RID: 50815 RVA: 0x0044F6AC File Offset: 0x0044D8AC
				protected override void Scan(ReadOnlySpan<char> inputSpan)
				{
					if (this.TryFindNextPossibleStartingPosition(inputSpan))
					{
						int runtextpos = this.runtextpos;
						int num = (this.runtextpos = runtextpos + 1);
						base.Capture(0, runtextpos, num);
					}
				}

				// Token: 0x0600C680 RID: 50816 RVA: 0x0044F6E0 File Offset: 0x0044D8E0
				private bool TryFindNextPossibleStartingPosition(ReadOnlySpan<char> inputSpan)
				{
					int runtextpos = this.runtextpos;
					if (runtextpos < inputSpan.Length)
					{
						int num = inputSpan.Slice(runtextpos).IndexOfAnyExcept<char>(<RegexGenerator_g>FACC081AAF3D765EFF87A82C4FBB77F6FD3EA759AA2D03D993988F88E97CC0B5B__Utilities.s_ascii_FF03FEFFFF8700000000);
						if (num >= 0)
						{
							this.runtextpos = runtextpos + num;
							return true;
						}
					}
					this.runtextpos = inputSpan.Length;
					return false;
				}
			}
		}
	}
}
