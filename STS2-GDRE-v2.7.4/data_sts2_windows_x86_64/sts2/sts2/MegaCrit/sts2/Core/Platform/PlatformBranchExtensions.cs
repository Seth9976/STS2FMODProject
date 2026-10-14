using System;
using System.Runtime.CompilerServices;

namespace MegaCrit.Sts2.Core.Platform
{
	// Token: 0x02000156 RID: 342
	[NullableContext(1)]
	[Nullable(0)]
	public static class PlatformBranchExtensions
	{
		/// <summary>
		/// Prefer over ToString as this gives the actual branch name as displayed in Steam or other platform
		/// </summary>
		// Token: 0x0600107B RID: 4219 RVA: 0x00062230 File Offset: 0x00060430
		public static string ToName(this PlatformBranch branch)
		{
			string text;
			switch (branch)
			{
			case PlatformBranch.None:
				text = "none";
				break;
			case PlatformBranch.Production:
				text = "public";
				break;
			case PlatformBranch.PublicBeta:
				text = "public-beta";
				break;
			case PlatformBranch.PrivateBeta:
				text = "private-beta";
				break;
			case PlatformBranch.DevTest:
				text = "dev-test";
				break;
			default:
				<PrivateImplementationDetails>.ThrowSwitchExpressionException(branch);
				break;
			}
			return text;
		}

		// Token: 0x0600107C RID: 4220 RVA: 0x00062290 File Offset: 0x00060490
		public static PlatformBranch? FromName(string name)
		{
			if (name == "dev-test")
			{
				return new PlatformBranch?(PlatformBranch.DevTest);
			}
			if (name == "private-beta")
			{
				return new PlatformBranch?(PlatformBranch.PrivateBeta);
			}
			if (name == "public-beta")
			{
				return new PlatformBranch?(PlatformBranch.PublicBeta);
			}
			if (name == "public")
			{
				return new PlatformBranch?(PlatformBranch.Production);
			}
			return null;
		}
	}
}
