using System;
using System.Globalization;
using System.Runtime.CompilerServices;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.TextEffects;
using SmartFormat.Core.Extensions;

namespace MegaCrit.Sts2.Core.Localization.DynamicVars
{
	// Token: 0x02000BE1 RID: 3041
	[NullableContext(2)]
	[Nullable(0)]
	public class DynamicVar : IConvertible
	{
		// Token: 0x170022C8 RID: 8904
		// (get) Token: 0x06007E0A RID: 32266 RVA: 0x002AD7B6 File Offset: 0x002AB9B6
		[Nullable(1)]
		public string Name
		{
			[NullableContext(1)]
			get;
		}

		/// <summary>
		/// The base value of this DynamicVar.
		/// This represents the actual value of the variable before any updates from external sources.
		/// This is the value that should be used when performing calculations that will modify the game's state.
		/// Other values (like PreviewValue) are for display only, to inform the player about what the value will be after
		/// external modifications.
		/// </summary>
		// Token: 0x170022C9 RID: 8905
		// (get) Token: 0x06007E0B RID: 32267 RVA: 0x002AD7BE File Offset: 0x002AB9BE
		// (set) Token: 0x06007E0C RID: 32268 RVA: 0x002AD7C6 File Offset: 0x002AB9C6
		public decimal BaseValue
		{
			get
			{
				return this._baseValue;
			}
			set
			{
				this._baseValue = Math.Min(value, 999999999m);
				this.ResetToBase();
			}
		}

		/// <summary>
		/// The value of this DynamicVar after being modified by a <see cref="T:MegaCrit.Sts2.Core.Models.EnchantmentModel" />, but before any other
		/// hooks have been run.
		/// The vast majority of the time, this will be the same as BaseValue.
		/// Only CardModels with enchantments will have a different EnchantedValue than BaseValue (and even then, only for
		/// enchantments that modify the variable in question).
		/// This exists because, in the eyes of the player, a card's enchantment is a "part of the card", so we need to be
		/// able to display the enchanted value without coloring it like it's being buffed by an effect.
		///
		/// NOTE: If we ever add another effect that needs to be displayed as "part of the card", we should rename this to
		/// be more generic.
		/// </summary>
		// Token: 0x170022CA RID: 8906
		// (get) Token: 0x06007E0D RID: 32269 RVA: 0x002AD7E4 File Offset: 0x002AB9E4
		// (set) Token: 0x06007E0E RID: 32270 RVA: 0x002AD7EC File Offset: 0x002AB9EC
		public decimal EnchantedValue
		{
			get
			{
				return this._enchantedValue;
			}
			set
			{
				this._enchantedValue = Math.Min(value, 999999999m);
			}
		}

		/// <summary>
		/// The value that should be displayed to the player when viewing something that uses this DynamicVar.
		/// This will often be the same as BaseValue, but it is modified in places like
		/// <see cref="M:MegaCrit.Sts2.Core.Models.CardModel.UpdateDynamicVarPreview(MegaCrit.Sts2.Core.Entities.Cards.CardPreviewMode,MegaCrit.Sts2.Core.Entities.Creatures.Creature,MegaCrit.Sts2.Core.Localization.DynamicVars.DynamicVarSet)" /> to show the player the final value after all modifications.
		/// Do NOT use this value for performing calculations that will modify the game's state. It's for display only.
		/// </summary>
		// Token: 0x170022CB RID: 8907
		// (get) Token: 0x06007E0F RID: 32271 RVA: 0x002AD804 File Offset: 0x002ABA04
		// (set) Token: 0x06007E10 RID: 32272 RVA: 0x002AD80C File Offset: 0x002ABA0C
		public decimal PreviewValue
		{
			get
			{
				return this._previewValue;
			}
			set
			{
				this._previewValue = Math.Min(value, 999999999m);
			}
		}

		/// <summary>
		/// Was this DynamicVar just recently upgraded?
		/// This is mainly used to show upgrade preview values in green.
		/// This should be cleared after the upgrade is complete.
		/// </summary>
		// Token: 0x170022CC RID: 8908
		// (get) Token: 0x06007E11 RID: 32273 RVA: 0x002AD824 File Offset: 0x002ABA24
		// (set) Token: 0x06007E12 RID: 32274 RVA: 0x002AD82C File Offset: 0x002ABA2C
		public bool WasJustUpgraded { get; protected set; }

		// Token: 0x170022CD RID: 8909
		// (get) Token: 0x06007E13 RID: 32275 RVA: 0x002AD835 File Offset: 0x002ABA35
		public int IntValue
		{
			get
			{
				return (int)this.BaseValue;
			}
		}

		// Token: 0x06007E14 RID: 32276 RVA: 0x002AD842 File Offset: 0x002ABA42
		[NullableContext(1)]
		public DynamicVar(string name, decimal baseValue)
		{
			this.Name = name;
			this.BaseValue = baseValue;
			this.ResetToBase();
		}

		/// <summary>
		/// Reset all values to the base value.
		/// </summary>
		// Token: 0x06007E15 RID: 32277 RVA: 0x002AD85E File Offset: 0x002ABA5E
		public void ResetToBase()
		{
			this.EnchantedValue = this.BaseValue;
			this.PreviewValue = this.BaseValue;
		}

		/// <summary>
		/// Set the model that "owns" this DynamicVar.
		/// </summary>
		// Token: 0x06007E16 RID: 32278 RVA: 0x002AD878 File Offset: 0x002ABA78
		[NullableContext(1)]
		public virtual void SetOwner(AbstractModel owner)
		{
			this._owner = owner;
		}

		/// <summary>
		/// Update this var's preview value based on hooks (i.e. damage, block, and powers).
		/// This is so powers and relic modifications to these values can be reflected in card descriptions.
		/// This should only be called on DynamicVars owned by CardModels.
		/// </summary>
		/// <param name="card">
		/// Card that this var is being displayed on. Usually matches <see cref="F:MegaCrit.Sts2.Core.Localization.DynamicVars.DynamicVar._owner" />, but for DynamicVars on
		/// enchantments, this is the card that the enchantment is on.
		/// </param>
		/// <param name="previewMode">The mode that this preview is being shown in.</param>
		/// <param name="target">Creature who this card is targeting. Null if there is no target</param>
		/// <param name="runGlobalHooks">
		/// Whether the global hooks (defined in <see cref="T:MegaCrit.Sts2.Core.Hooks.Hook" />) should be run.
		/// If false, only <see cref="T:MegaCrit.Sts2.Core.Models.EnchantmentModel" /> hooks will be run.
		/// </param>
		// Token: 0x06007E17 RID: 32279 RVA: 0x002AD881 File Offset: 0x002ABA81
		[NullableContext(1)]
		public virtual void UpdateCardPreview(CardModel card, CardPreviewMode previewMode, [Nullable(2)] Creature target, bool runGlobalHooks)
		{
		}

		/// <summary>
		/// Increase the base value by the specified amount, and mark it as recently upgraded.
		/// </summary>
		// Token: 0x06007E18 RID: 32280 RVA: 0x002AD883 File Offset: 0x002ABA83
		public void UpgradeValueBy(decimal addend)
		{
			this.BaseValue += addend;
			this.WasJustUpgraded = true;
		}

		/// <summary>
		/// "Finalize" a recent upgrade, so it is no longer considered recently upgraded.
		/// </summary>
		// Token: 0x06007E19 RID: 32281 RVA: 0x002AD89E File Offset: 0x002ABA9E
		public void FinalizeUpgrade()
		{
			this.WasJustUpgraded = false;
		}

		// Token: 0x06007E1A RID: 32282 RVA: 0x002AD8A8 File Offset: 0x002ABAA8
		[NullableContext(1)]
		public DynamicVar Clone()
		{
			DynamicVar dynamicVar = (DynamicVar)base.MemberwiseClone();
			dynamicVar.ResetToBase();
			return dynamicVar;
		}

		/// <summary>
		/// Get a highlighted version of this variable's preview value.
		/// Normally, if the preview value is higher than the base value, color it green. If lower, red. If equal, default.
		/// Sometimes, we do the inverse (green for lower, red for higher), for cases where a lower value is good.
		/// </summary>
		/// <param name="inverse">Whether or not to flip the highlight logic.</param>
		/// <returns>Highlighted preview value.</returns>
		// Token: 0x06007E1B RID: 32283 RVA: 0x002AD8C8 File Offset: 0x002ABAC8
		[NullableContext(1)]
		public string ToHighlightedString(bool inverse)
		{
			int num = (int)this.PreviewValue;
			int num2 = (int)this.EnchantedValue;
			int num3;
			if (this.WasJustUpgraded)
			{
				num3 = 1;
			}
			else if (inverse)
			{
				num3 = num2.CompareTo(num);
			}
			else
			{
				num3 = num.CompareTo(num2);
			}
			return StsTextUtilities.HighlightChangeText(num.ToString(CultureInfo.InvariantCulture), num3);
		}

		// Token: 0x06007E1C RID: 32284 RVA: 0x002AD924 File Offset: 0x002ABB24
		[NullableContext(1)]
		public override string ToString()
		{
			return this.IntValue.ToString();
		}

		// Token: 0x06007E1D RID: 32285 RVA: 0x002AD93F File Offset: 0x002ABB3F
		[NullableContext(1)]
		public object GetSourceValue(ISelectorInfo selector)
		{
			return this.GetBaseValueForIConvertible();
		}

		// Token: 0x06007E1E RID: 32286 RVA: 0x002AD94C File Offset: 0x002ABB4C
		public TypeCode GetTypeCode()
		{
			return TypeCode.Object;
		}

		// Token: 0x06007E1F RID: 32287 RVA: 0x002AD950 File Offset: 0x002ABB50
		public bool ToBoolean(IFormatProvider provider)
		{
			DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(26, 1);
			defaultInterpolatedStringHandler.AppendLiteral("Cannot convert ");
			defaultInterpolatedStringHandler.AppendFormatted<decimal>(this.BaseValue);
			defaultInterpolatedStringHandler.AppendLiteral(" to Boolean");
			throw new InvalidCastException(defaultInterpolatedStringHandler.ToStringAndClear());
		}

		// Token: 0x06007E20 RID: 32288 RVA: 0x002AD998 File Offset: 0x002ABB98
		public byte ToByte(IFormatProvider provider)
		{
			return Convert.ToByte(this.GetBaseValueForIConvertible(), provider);
		}

		// Token: 0x06007E21 RID: 32289 RVA: 0x002AD9AC File Offset: 0x002ABBAC
		public char ToChar(IFormatProvider provider)
		{
			DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(23, 1);
			defaultInterpolatedStringHandler.AppendLiteral("Cannot convert ");
			defaultInterpolatedStringHandler.AppendFormatted<decimal>(this.BaseValue);
			defaultInterpolatedStringHandler.AppendLiteral(" to Char");
			throw new InvalidCastException(defaultInterpolatedStringHandler.ToStringAndClear());
		}

		// Token: 0x06007E22 RID: 32290 RVA: 0x002AD9F4 File Offset: 0x002ABBF4
		public DateTime ToDateTime(IFormatProvider provider)
		{
			DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(27, 1);
			defaultInterpolatedStringHandler.AppendLiteral("Cannot convert ");
			defaultInterpolatedStringHandler.AppendFormatted<decimal>(this.BaseValue);
			defaultInterpolatedStringHandler.AppendLiteral(" to DateTime");
			throw new InvalidCastException(defaultInterpolatedStringHandler.ToStringAndClear());
		}

		// Token: 0x06007E23 RID: 32291 RVA: 0x002ADA3C File Offset: 0x002ABC3C
		public decimal ToDecimal(IFormatProvider provider)
		{
			return this.GetBaseValueForIConvertible();
		}

		// Token: 0x06007E24 RID: 32292 RVA: 0x002ADA44 File Offset: 0x002ABC44
		public double ToDouble(IFormatProvider provider)
		{
			return Convert.ToDouble(this.GetBaseValueForIConvertible(), provider);
		}

		// Token: 0x06007E25 RID: 32293 RVA: 0x002ADA57 File Offset: 0x002ABC57
		public short ToInt16(IFormatProvider provider)
		{
			return Convert.ToInt16(this.GetBaseValueForIConvertible(), provider);
		}

		// Token: 0x06007E26 RID: 32294 RVA: 0x002ADA6A File Offset: 0x002ABC6A
		public int ToInt32(IFormatProvider provider)
		{
			return Convert.ToInt32(this.GetBaseValueForIConvertible(), provider);
		}

		// Token: 0x06007E27 RID: 32295 RVA: 0x002ADA7D File Offset: 0x002ABC7D
		public long ToInt64(IFormatProvider provider)
		{
			return Convert.ToInt64(this.GetBaseValueForIConvertible(), provider);
		}

		// Token: 0x06007E28 RID: 32296 RVA: 0x002ADA90 File Offset: 0x002ABC90
		public sbyte ToSByte(IFormatProvider provider)
		{
			return Convert.ToSByte(this.GetBaseValueForIConvertible(), provider);
		}

		// Token: 0x06007E29 RID: 32297 RVA: 0x002ADAA3 File Offset: 0x002ABCA3
		public float ToSingle(IFormatProvider provider)
		{
			return Convert.ToSingle(this.GetBaseValueForIConvertible(), provider);
		}

		// Token: 0x06007E2A RID: 32298 RVA: 0x002ADAB8 File Offset: 0x002ABCB8
		[NullableContext(1)]
		public string ToString([Nullable(2)] IFormatProvider provider)
		{
			return this.GetBaseValueForIConvertible().ToString(provider);
		}

		// Token: 0x06007E2B RID: 32299 RVA: 0x002ADAD4 File Offset: 0x002ABCD4
		[NullableContext(1)]
		public object ToType(Type conversionType, [Nullable(2)] IFormatProvider provider)
		{
			return Convert.ChangeType(this.GetBaseValueForIConvertible(), conversionType, provider);
		}

		// Token: 0x06007E2C RID: 32300 RVA: 0x002ADAE8 File Offset: 0x002ABCE8
		public ushort ToUInt16(IFormatProvider provider)
		{
			return Convert.ToUInt16(this.GetBaseValueForIConvertible(), provider);
		}

		// Token: 0x06007E2D RID: 32301 RVA: 0x002ADAFB File Offset: 0x002ABCFB
		public uint ToUInt32(IFormatProvider provider)
		{
			return Convert.ToUInt32(this.GetBaseValueForIConvertible(), provider);
		}

		// Token: 0x06007E2E RID: 32302 RVA: 0x002ADB0E File Offset: 0x002ABD0E
		public ulong ToUInt64(IFormatProvider provider)
		{
			return Convert.ToUInt64(this.GetBaseValueForIConvertible(), provider);
		}

		// Token: 0x06007E2F RID: 32303 RVA: 0x002ADB21 File Offset: 0x002ABD21
		protected virtual decimal GetBaseValueForIConvertible()
		{
			return this.BaseValue;
		}

		/// <summary>
		/// The model that "owns" this DynamicVar.
		/// </summary>
		// Token: 0x04002A2C RID: 10796
		protected AbstractModel _owner;

		// Token: 0x04002A2D RID: 10797
		private decimal _baseValue;

		// Token: 0x04002A2E RID: 10798
		private decimal _enchantedValue;

		// Token: 0x04002A2F RID: 10799
		private decimal _previewValue;
	}
}
