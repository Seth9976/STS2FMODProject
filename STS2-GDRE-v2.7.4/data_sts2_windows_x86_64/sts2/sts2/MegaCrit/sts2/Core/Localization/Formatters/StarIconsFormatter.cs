using System;
using System.Linq;
using System.Runtime.CompilerServices;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using SmartFormat.Core.Extensions;

namespace MegaCrit.Sts2.Core.Localization.Formatters
{
	// Token: 0x02000BCD RID: 3021
	[NullableContext(1)]
	[Nullable(0)]
	public class StarIconsFormatter : IFormatter
	{
		// Token: 0x170022C0 RID: 8896
		// (get) Token: 0x06007DCA RID: 32202 RVA: 0x002ACD1E File Offset: 0x002AAF1E
		// (set) Token: 0x06007DCB RID: 32203 RVA: 0x002ACD25 File Offset: 0x002AAF25
		public string Name
		{
			get
			{
				return "starIcons";
			}
			set
			{
				throw new NotImplementedException();
			}
		}

		// Token: 0x170022C1 RID: 8897
		// (get) Token: 0x06007DCC RID: 32204 RVA: 0x002ACD2C File Offset: 0x002AAF2C
		// (set) Token: 0x06007DCD RID: 32205 RVA: 0x002ACD34 File Offset: 0x002AAF34
		public bool CanAutoDetect { get; set; }

		// Token: 0x06007DCE RID: 32206 RVA: 0x002ACD40 File Offset: 0x002AAF40
		public bool TryEvaluateFormat(IFormattingInfo formattingInfo)
		{
			object currentValue = formattingInfo.CurrentValue;
			DynamicVar dynamicVar = currentValue as DynamicVar;
			int num2;
			if (dynamicVar == null)
			{
				if (currentValue is decimal)
				{
					decimal num = (decimal)currentValue;
					num2 = (int)num;
				}
				else
				{
					if (!(currentValue is int))
					{
						DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(22, 2);
						defaultInterpolatedStringHandler.AppendLiteral("Unknown value='");
						defaultInterpolatedStringHandler.AppendFormatted<object>(formattingInfo.CurrentValue);
						defaultInterpolatedStringHandler.AppendLiteral("' type=");
						object currentValue2 = formattingInfo.CurrentValue;
						defaultInterpolatedStringHandler.AppendFormatted<Type>((currentValue2 != null) ? currentValue2.GetType() : null);
						throw new LocException(defaultInterpolatedStringHandler.ToStringAndClear());
					}
					int num3 = (int)currentValue;
					num2 = num3;
				}
			}
			else
			{
				num2 = (int)dynamicVar.PreviewValue;
			}
			int num4 = num2;
			string text = string.Concat(Enumerable.Repeat<string>("[img]res://images/packed/sprite_fonts/star_icon.png[/img]", num4));
			formattingInfo.Write(text);
			return true;
		}

		// Token: 0x04002A06 RID: 10758
		private const string _starIconPath = "res://images/packed/sprite_fonts/star_icon.png";

		// Token: 0x04002A07 RID: 10759
		public const string starIconSprite = "[img]res://images/packed/sprite_fonts/star_icon.png[/img]";
	}
}
