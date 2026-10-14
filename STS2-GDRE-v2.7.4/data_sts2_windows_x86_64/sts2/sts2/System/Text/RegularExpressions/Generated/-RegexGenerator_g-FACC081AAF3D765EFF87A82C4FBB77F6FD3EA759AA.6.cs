using System;
using System.CodeDom.Compiler;
using System.Runtime.CompilerServices;

namespace System.Text.RegularExpressions.Generated
{
	// Token: 0x02000012 RID: 18
	[GeneratedCode("System.Text.RegularExpressions.Generator", "9.0.12.31616")]
	[SkipLocalsInit]
	internal sealed class <RegexGenerator_g>FACC081AAF3D765EFF87A82C4FBB77F6FD3EA759AA2D03D993988F88E97CC0B5B__NonSpaceWhitespaceCharacters_5 : Regex
	{
		// Token: 0x0600009C RID: 156 RVA: 0x000064A0 File Offset: 0x000046A0
		private <RegexGenerator_g>FACC081AAF3D765EFF87A82C4FBB77F6FD3EA759AA2D03D993988F88E97CC0B5B__NonSpaceWhitespaceCharacters_5()
		{
			this.pattern = "[\\t\\r\\n]";
			this.roptions = RegexOptions.None;
			Regex.ValidateMatchTimeout(<RegexGenerator_g>FACC081AAF3D765EFF87A82C4FBB77F6FD3EA759AA2D03D993988F88E97CC0B5B__Utilities.s_defaultTimeout);
			this.internalMatchTimeout = <RegexGenerator_g>FACC081AAF3D765EFF87A82C4FBB77F6FD3EA759AA2D03D993988F88E97CC0B5B__Utilities.s_defaultTimeout;
			this.factory = new <RegexGenerator_g>FACC081AAF3D765EFF87A82C4FBB77F6FD3EA759AA2D03D993988F88E97CC0B5B__NonSpaceWhitespaceCharacters_5.RunnerFactory();
			this.capsize = 1;
		}

		// Token: 0x0400003C RID: 60
		[Nullable(1)]
		internal static readonly <RegexGenerator_g>FACC081AAF3D765EFF87A82C4FBB77F6FD3EA759AA2D03D993988F88E97CC0B5B__NonSpaceWhitespaceCharacters_5 Instance = new <RegexGenerator_g>FACC081AAF3D765EFF87A82C4FBB77F6FD3EA759AA2D03D993988F88E97CC0B5B__NonSpaceWhitespaceCharacters_5();

		// Token: 0x02000DD0 RID: 3536
		private sealed class RunnerFactory : RegexRunnerFactory
		{
			// Token: 0x06008D08 RID: 36104 RVA: 0x002EF8B4 File Offset: 0x002EDAB4
			[NullableContext(1)]
			protected override RegexRunner CreateInstance()
			{
				return new <RegexGenerator_g>FACC081AAF3D765EFF87A82C4FBB77F6FD3EA759AA2D03D993988F88E97CC0B5B__NonSpaceWhitespaceCharacters_5.RunnerFactory.Runner();
			}

			// Token: 0x02002591 RID: 9617
			private sealed class Runner : RegexRunner
			{
				// Token: 0x0600C686 RID: 50822 RVA: 0x0044F91C File Offset: 0x0044DB1C
				protected override void Scan(ReadOnlySpan<char> inputSpan)
				{
					if (this.TryFindNextPossibleStartingPosition(inputSpan))
					{
						int runtextpos = this.runtextpos;
						int num = (this.runtextpos = runtextpos + 1);
						base.Capture(0, runtextpos, num);
					}
				}

				// Token: 0x0600C687 RID: 50823 RVA: 0x0044F950 File Offset: 0x0044DB50
				private bool TryFindNextPossibleStartingPosition(ReadOnlySpan<char> inputSpan)
				{
					int runtextpos = this.runtextpos;
					if (runtextpos < inputSpan.Length)
					{
						int num = inputSpan.Slice(runtextpos).IndexOfAny('\t', '\n', '\r');
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
