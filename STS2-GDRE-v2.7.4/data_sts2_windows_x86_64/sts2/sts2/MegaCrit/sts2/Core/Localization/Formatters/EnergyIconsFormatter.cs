using System;
using System.Linq;
using System.Runtime.CompilerServices;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Logging;
using MegaCrit.Sts2.Core.Runs;
using SmartFormat.Core.Extensions;

namespace MegaCrit.Sts2.Core.Localization.Formatters
{
	// Token: 0x02000BC6 RID: 3014
	[NullableContext(1)]
	[Nullable(0)]
	public class EnergyIconsFormatter : IFormatter
	{
		// Token: 0x170022B1 RID: 8881
		// (get) Token: 0x06007D9F RID: 32159 RVA: 0x002AC684 File Offset: 0x002AA884
		// (set) Token: 0x06007DA0 RID: 32160 RVA: 0x002AC68B File Offset: 0x002AA88B
		public string Name
		{
			get
			{
				return "energyIcons";
			}
			set
			{
				throw new NotImplementedException();
			}
		}

		// Token: 0x170022B2 RID: 8882
		// (get) Token: 0x06007DA1 RID: 32161 RVA: 0x002AC692 File Offset: 0x002AA892
		// (set) Token: 0x06007DA2 RID: 32162 RVA: 0x002AC69A File Offset: 0x002AA89A
		public bool CanAutoDetect { get; set; }

		// Token: 0x06007DA3 RID: 32163 RVA: 0x002AC6A4 File Offset: 0x002AA8A4
		public bool TryEvaluateFormat(IFormattingInfo formattingInfo)
		{
			string text = null;
			object currentValue = formattingInfo.CurrentValue;
			EnergyVar energyVar = currentValue as EnergyVar;
			int num2;
			if (energyVar == null)
			{
				CalculatedVar calculatedVar = currentValue as CalculatedVar;
				if (calculatedVar == null)
				{
					if (currentValue is decimal)
					{
						decimal num = (decimal)currentValue;
						num2 = (int)num;
					}
					else if (currentValue is int)
					{
						int num3 = (int)currentValue;
						num2 = num3;
					}
					else
					{
						string text2 = currentValue as string;
						if (text2 == null)
						{
							DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(22, 2);
							defaultInterpolatedStringHandler.AppendLiteral("Unknown value='");
							defaultInterpolatedStringHandler.AppendFormatted<object>(formattingInfo.CurrentValue);
							defaultInterpolatedStringHandler.AppendLiteral("' type=");
							object currentValue2 = formattingInfo.CurrentValue;
							defaultInterpolatedStringHandler.AppendFormatted<Type>((currentValue2 != null) ? currentValue2.GetType() : null);
							throw new LocException(defaultInterpolatedStringHandler.ToStringAndClear());
						}
						if (!int.TryParse(formattingInfo.FormatterOptions, out num2))
						{
							return false;
						}
						text = text2;
					}
				}
				else
				{
					num2 = Convert.ToInt32(calculatedVar.Calculate(null));
				}
			}
			else
			{
				num2 = Convert.ToInt32(energyVar.PreviewValue);
				if (!string.IsNullOrEmpty(energyVar.ColorPrefix))
				{
					text = energyVar.ColorPrefix;
				}
			}
			if (string.IsNullOrEmpty(text) || text == "colorless")
			{
				text = RunManager.Instance.GetLocalCharacterEnergyIconPrefix();
			}
			if (text == null)
			{
				Log.Warn("No energy prefix found for EnergyIconsFormatter! Using colorless as a fallback.", 2);
				if (text == null)
				{
					text = "colorless";
				}
			}
			string text3 = "[img]res://images/packed/sprite_fonts/" + text + "_energy_icon.png[/img]";
			string text4;
			if (num2 > 0 && num2 < 4)
			{
				text4 = string.Concat(Enumerable.Repeat<string>(text3, num2));
			}
			else
			{
				DynamicVar dynamicVar = formattingInfo.CurrentValue as DynamicVar;
				if (dynamicVar != null)
				{
					text4 = dynamicVar.ToHighlightedString(false) + text3;
				}
				else
				{
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(0, 2);
					defaultInterpolatedStringHandler.AppendFormatted<int>(num2);
					defaultInterpolatedStringHandler.AppendFormatted(text3);
					text4 = defaultInterpolatedStringHandler.ToStringAndClear();
				}
			}
			formattingInfo.Write(text4);
			return true;
		}

		// Token: 0x040029FE RID: 10750
		private const string _fallbackIconPrefix = "colorless";
	}
}
