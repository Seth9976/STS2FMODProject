using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using SmartFormat.Core.Extensions;
using SmartFormat.Core.Parsing;

namespace MegaCrit.Sts2.Core.Localization.Formatters
{
	// Token: 0x02000BCC RID: 3020
	[NullableContext(1)]
	[Nullable(0)]
	public class ShowIfUpgradedFormatter : IFormatter
	{
		// Token: 0x170022BE RID: 8894
		// (get) Token: 0x06007DC4 RID: 32196 RVA: 0x002ACB70 File Offset: 0x002AAD70
		// (set) Token: 0x06007DC5 RID: 32197 RVA: 0x002ACB77 File Offset: 0x002AAD77
		public string Name
		{
			get
			{
				return "show";
			}
			set
			{
				throw new NotSupportedException("Setting the 'Names' property is not supported.");
			}
		}

		// Token: 0x170022BF RID: 8895
		// (get) Token: 0x06007DC6 RID: 32198 RVA: 0x002ACB83 File Offset: 0x002AAD83
		// (set) Token: 0x06007DC7 RID: 32199 RVA: 0x002ACB8B File Offset: 0x002AAD8B
		public bool CanAutoDetect { get; set; }

		// Token: 0x06007DC8 RID: 32200 RVA: 0x002ACB94 File Offset: 0x002AAD94
		public bool TryEvaluateFormat(IFormattingInfo formattingInfo)
		{
			IfUpgradedVar ifUpgradedVar = formattingInfo.CurrentValue as IfUpgradedVar;
			if (ifUpgradedVar == null)
			{
				return false;
			}
			Format format = formattingInfo.Format;
			IList<Format> list = ((format != null) ? format.Split('|') : null);
			if (list == null)
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(58, 1);
				defaultInterpolatedStringHandler.AppendLiteral("Format expression must contain at least 1 option. format=");
				defaultInterpolatedStringHandler.AppendFormatted<Format>(formattingInfo.Format);
				defaultInterpolatedStringHandler.AppendLiteral(".");
				throw new LocException(defaultInterpolatedStringHandler.ToStringAndClear());
			}
			if (list.Count > 2)
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(78, 2);
				defaultInterpolatedStringHandler.AppendLiteral("Format expression cannot contain more than 2 options. num_of_options=");
				defaultInterpolatedStringHandler.AppendFormatted<int>(list.Count);
				defaultInterpolatedStringHandler.AppendLiteral(" format=");
				defaultInterpolatedStringHandler.AppendFormatted<Format>(formattingInfo.Format);
				defaultInterpolatedStringHandler.AppendLiteral(".");
				throw new LocException(defaultInterpolatedStringHandler.ToStringAndClear());
			}
			Format format2 = list[0];
			Format format3 = ((list.Count > 1) ? list[1] : null);
			switch (ifUpgradedVar.upgradeDisplay)
			{
			case UpgradeDisplay.Normal:
				formattingInfo.FormatAsChild(format3, formattingInfo.CurrentValue);
				break;
			case UpgradeDisplay.Upgraded:
				formattingInfo.FormatAsChild(format2, formattingInfo.CurrentValue);
				break;
			case UpgradeDisplay.UpgradePreview:
				formattingInfo.Write("[green]");
				formattingInfo.FormatAsChild(format2, formattingInfo.CurrentValue);
				formattingInfo.Write("[/green]");
				break;
			default:
			{
				string text = "upgradeDisplay";
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(18, 1);
				defaultInterpolatedStringHandler.AppendLiteral("Unexpected value: ");
				defaultInterpolatedStringHandler.AppendFormatted<UpgradeDisplay>(ifUpgradedVar.upgradeDisplay);
				throw new ArgumentOutOfRangeException(text, defaultInterpolatedStringHandler.ToStringAndClear());
			}
			}
			return true;
		}
	}
}
