using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;
using Godot;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.Localization;
using MegaCrit.Sts2.Core.Models;

namespace MegaCrit.Sts2.Core.HoverTips
{
	// Token: 0x02000BF8 RID: 3064
	[NullableContext(1)]
	[Nullable(0)]
	public struct HoverTip : IHoverTip, IEquatable<HoverTip>
	{
		// Token: 0x170022F6 RID: 8950
		// (get) Token: 0x06007E98 RID: 32408 RVA: 0x002AE4C4 File Offset: 0x002AC6C4
		// (set) Token: 0x06007E99 RID: 32409 RVA: 0x002AE4CC File Offset: 0x002AC6CC
		[Nullable(2)]
		public string Title
		{
			[NullableContext(2)]
			readonly get;
			[NullableContext(2)]
			private set;
		}

		// Token: 0x170022F7 RID: 8951
		// (get) Token: 0x06007E9A RID: 32410 RVA: 0x002AE4D5 File Offset: 0x002AC6D5
		// (set) Token: 0x06007E9B RID: 32411 RVA: 0x002AE4DD File Offset: 0x002AC6DD
		[Nullable(2)]
		public Texture2D Icon
		{
			[NullableContext(2)]
			readonly get;
			[NullableContext(2)]
			private set;
		}

		// Token: 0x170022F8 RID: 8952
		// (get) Token: 0x06007E9C RID: 32412 RVA: 0x002AE4E6 File Offset: 0x002AC6E6
		// (set) Token: 0x06007E9D RID: 32413 RVA: 0x002AE4EE File Offset: 0x002AC6EE
		public string Description { readonly get; private set; }

		// Token: 0x170022F9 RID: 8953
		// (get) Token: 0x06007E9E RID: 32414 RVA: 0x002AE4F7 File Offset: 0x002AC6F7
		// (set) Token: 0x06007E9F RID: 32415 RVA: 0x002AE4FF File Offset: 0x002AC6FF
		public string Id { readonly get; set; }

		// Token: 0x170022FA RID: 8954
		// (get) Token: 0x06007EA0 RID: 32416 RVA: 0x002AE508 File Offset: 0x002AC708
		// (set) Token: 0x06007EA1 RID: 32417 RVA: 0x002AE510 File Offset: 0x002AC710
		public bool IsSmart { readonly get; set; }

		// Token: 0x170022FB RID: 8955
		// (get) Token: 0x06007EA2 RID: 32418 RVA: 0x002AE519 File Offset: 0x002AC719
		// (set) Token: 0x06007EA3 RID: 32419 RVA: 0x002AE521 File Offset: 0x002AC721
		public bool IsDebuff { readonly get; set; }

		// Token: 0x170022FC RID: 8956
		// (get) Token: 0x06007EA4 RID: 32420 RVA: 0x002AE52A File Offset: 0x002AC72A
		// (set) Token: 0x06007EA5 RID: 32421 RVA: 0x002AE532 File Offset: 0x002AC732
		public bool IsInstanced { readonly get; set; }

		// Token: 0x170022FD RID: 8957
		// (get) Token: 0x06007EA6 RID: 32422 RVA: 0x002AE53B File Offset: 0x002AC73B
		// (set) Token: 0x06007EA7 RID: 32423 RVA: 0x002AE543 File Offset: 0x002AC743
		[Nullable(2)]
		public AbstractModel CanonicalModel
		{
			[NullableContext(2)]
			readonly get;
			[NullableContext(2)]
			private set;
		}

		/// <summary>
		/// Defines whether or now we want to word wrap the description text on the hover tip node
		/// or if we want the text to stretch the HoverTip horizontally. This is useful for situations
		/// like our map point entry hover tips in the run history screen, that can be pretty long.
		/// </summary>
		// Token: 0x170022FE RID: 8958
		// (get) Token: 0x06007EA8 RID: 32424 RVA: 0x002AE54C File Offset: 0x002AC74C
		// (set) Token: 0x06007EA9 RID: 32425 RVA: 0x002AE554 File Offset: 0x002AC754
		public bool ShouldOverrideTextOverflow { readonly get; set; }

		// Token: 0x06007EAA RID: 32426 RVA: 0x002AE560 File Offset: 0x002AC760
		public HoverTip(LocString description, [Nullable(2)] Texture2D icon = null)
		{
			this.Title = null;
			this.IsSmart = false;
			this.IsDebuff = false;
			this.IsInstanced = false;
			this.CanonicalModel = null;
			this.ShouldOverrideTextOverflow = false;
			this.Id = "LocString with Description=" + description.LocTable + "." + description.LocEntryKey;
			this.Description = description.GetFormattedText();
			this.Icon = icon;
		}

		// Token: 0x06007EAB RID: 32427 RVA: 0x002AE5CC File Offset: 0x002AC7CC
		public HoverTip(LocString title, LocString description, [Nullable(2)] Texture2D icon = null)
		{
			this.IsSmart = false;
			this.IsDebuff = false;
			this.IsInstanced = false;
			this.CanonicalModel = null;
			this.ShouldOverrideTextOverflow = false;
			DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(40, 4);
			defaultInterpolatedStringHandler.AppendLiteral("LocString with Title=");
			defaultInterpolatedStringHandler.AppendFormatted(title.LocTable);
			defaultInterpolatedStringHandler.AppendLiteral(".");
			defaultInterpolatedStringHandler.AppendFormatted(title.LocEntryKey);
			defaultInterpolatedStringHandler.AppendLiteral(" and Description=");
			defaultInterpolatedStringHandler.AppendFormatted(description.LocTable);
			defaultInterpolatedStringHandler.AppendLiteral(".");
			defaultInterpolatedStringHandler.AppendFormatted(description.LocEntryKey);
			this.Id = defaultInterpolatedStringHandler.ToStringAndClear();
			this.Title = title.GetFormattedText();
			this.Description = description.GetFormattedText();
			this.Icon = icon;
		}

		// Token: 0x06007EAC RID: 32428 RVA: 0x002AE698 File Offset: 0x002AC898
		public HoverTip(LocString title, string description, [Nullable(2)] Texture2D icon = null)
		{
			this.IsSmart = false;
			this.IsDebuff = false;
			this.IsInstanced = false;
			this.CanonicalModel = null;
			this.ShouldOverrideTextOverflow = false;
			this.Id = "LocString with Title=" + title.LocTable + "." + title.LocEntryKey;
			this.Title = title.GetFormattedText();
			this.Description = description;
			this.Icon = icon;
		}

		// Token: 0x06007EAD RID: 32429 RVA: 0x002AE704 File Offset: 0x002AC904
		public HoverTip(AfflictionModel affliction, LocString description)
		{
			this.IsSmart = false;
			this.IsInstanced = false;
			this.CanonicalModel = null;
			this.ShouldOverrideTextOverflow = false;
			this.Id = affliction.Id.ToString();
			this.Title = affliction.Title.GetFormattedText();
			this.Description = description.GetFormattedText();
			this.Icon = null;
			this.IsDebuff = true;
		}

		// Token: 0x06007EAE RID: 32430 RVA: 0x002AE76C File Offset: 0x002AC96C
		public HoverTip(OrbModel orb, LocString description)
		{
			this.IsSmart = false;
			this.IsDebuff = false;
			this.IsInstanced = false;
			this.CanonicalModel = null;
			this.ShouldOverrideTextOverflow = false;
			this.Id = orb.Id.ToString();
			this.Title = orb.Title.GetFormattedText();
			this.Description = description.GetFormattedText();
			this.Icon = orb.Icon;
		}

		/// <summary>
		/// Create a HoverTip from a PowerModel.
		/// </summary>
		/// <param name="power">Power to create the HoverTip for.</param>
		/// <param name="description">string to use in the description field.</param>
		/// <param name="isSmart">
		///     Whether or not the power description is "smart".
		///     For example, when the Strength HoverTip is displayed on the Body Slam card, it should be "dumb" because it's
		///     just referring to the concept of Strength, rather than a specific amount.
		///     On the flip side, when the Strength HoverTip is displayed when hovering over a creature with 3 Strength, it
		///     should be "smart" because it's referring to an instance of 3 Strength.
		/// </param>
		// Token: 0x06007EAF RID: 32431 RVA: 0x002AE7D8 File Offset: 0x002AC9D8
		public HoverTip(PowerModel power, string description, bool isSmart)
		{
			this.CanonicalModel = null;
			this.ShouldOverrideTextOverflow = false;
			this.Id = power.Id.ToString();
			this.Title = power.Title.GetFormattedText();
			this.Description = description;
			this.Icon = power.Icon;
			this.IsDebuff = power.Type == PowerType.Debuff;
			this.IsInstanced = power.InstanceType > PowerInstanceType.None;
			this.IsSmart = isSmart;
		}

		// Token: 0x06007EB0 RID: 32432 RVA: 0x002AE84D File Offset: 0x002ACA4D
		public void SetCanonicalModel(AbstractModel model)
		{
			model.AssertCanonical();
			this.CanonicalModel = model;
		}

		/// <summary>
		/// We prefer to align HoverTips to be on the right-hand side of the hovered element. However, if we can't fit the HoverTip we left align.
		/// </summary>
		// Token: 0x06007EB1 RID: 32433 RVA: 0x002AE85C File Offset: 0x002ACA5C
		public static HoverTipAlignment GetHoverTipAlignment(Node2D node, float threshold = 0.75f)
		{
			if (node.GlobalPosition.X <= node.GetViewport().GetVisibleRect().Size.X * threshold)
			{
				return HoverTipAlignment.Right;
			}
			return HoverTipAlignment.Left;
		}

		// Token: 0x06007EB2 RID: 32434 RVA: 0x002AE894 File Offset: 0x002ACA94
		public static HoverTipAlignment GetHoverTipAlignment(Control node, float threshold = 0.75f)
		{
			if (node.GlobalPosition.X <= node.GetViewport().GetVisibleRect().Size.X * threshold)
			{
				return HoverTipAlignment.Right;
			}
			return HoverTipAlignment.Left;
		}

		// Token: 0x06007EB3 RID: 32435 RVA: 0x002AE8CC File Offset: 0x002ACACC
		[NullableContext(0)]
		[CompilerGenerated]
		public override readonly string ToString()
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append("HoverTip");
			stringBuilder.Append(" { ");
			if (this.PrintMembers(stringBuilder))
			{
				stringBuilder.Append(' ');
			}
			stringBuilder.Append('}');
			return stringBuilder.ToString();
		}

		// Token: 0x06007EB4 RID: 32436 RVA: 0x002AE918 File Offset: 0x002ACB18
		[NullableContext(0)]
		[CompilerGenerated]
		private readonly bool PrintMembers(StringBuilder builder)
		{
			builder.Append("Title = ");
			builder.Append(this.Title);
			builder.Append(", Icon = ");
			builder.Append(this.Icon);
			builder.Append(", Description = ");
			builder.Append(this.Description);
			builder.Append(", Id = ");
			builder.Append(this.Id);
			builder.Append(", IsSmart = ");
			builder.Append(this.IsSmart.ToString());
			builder.Append(", IsDebuff = ");
			builder.Append(this.IsDebuff.ToString());
			builder.Append(", IsInstanced = ");
			builder.Append(this.IsInstanced.ToString());
			builder.Append(", CanonicalModel = ");
			builder.Append(this.CanonicalModel);
			builder.Append(", ShouldOverrideTextOverflow = ");
			builder.Append(this.ShouldOverrideTextOverflow.ToString());
			return true;
		}

		// Token: 0x06007EB5 RID: 32437 RVA: 0x002AEA3F File Offset: 0x002ACC3F
		[CompilerGenerated]
		public static bool operator !=(HoverTip left, HoverTip right)
		{
			return !(left == right);
		}

		// Token: 0x06007EB6 RID: 32438 RVA: 0x002AEA4B File Offset: 0x002ACC4B
		[CompilerGenerated]
		public static bool operator ==(HoverTip left, HoverTip right)
		{
			return left.Equals(right);
		}

		// Token: 0x06007EB7 RID: 32439 RVA: 0x002AEA58 File Offset: 0x002ACC58
		[CompilerGenerated]
		public override readonly int GetHashCode()
		{
			return (((((((EqualityComparer<string>.Default.GetHashCode(this.<Title>k__BackingField) * -1521134295 + EqualityComparer<Texture2D>.Default.GetHashCode(this.<Icon>k__BackingField)) * -1521134295 + EqualityComparer<string>.Default.GetHashCode(this.<Description>k__BackingField)) * -1521134295 + EqualityComparer<string>.Default.GetHashCode(this.<Id>k__BackingField)) * -1521134295 + EqualityComparer<bool>.Default.GetHashCode(this.<IsSmart>k__BackingField)) * -1521134295 + EqualityComparer<bool>.Default.GetHashCode(this.<IsDebuff>k__BackingField)) * -1521134295 + EqualityComparer<bool>.Default.GetHashCode(this.<IsInstanced>k__BackingField)) * -1521134295 + EqualityComparer<AbstractModel>.Default.GetHashCode(this.<CanonicalModel>k__BackingField)) * -1521134295 + EqualityComparer<bool>.Default.GetHashCode(this.<ShouldOverrideTextOverflow>k__BackingField);
		}

		// Token: 0x06007EB8 RID: 32440 RVA: 0x002AEB2D File Offset: 0x002ACD2D
		[NullableContext(0)]
		[CompilerGenerated]
		public override readonly bool Equals(object obj)
		{
			return obj is HoverTip && this.Equals((HoverTip)obj);
		}

		// Token: 0x06007EB9 RID: 32441 RVA: 0x002AEB48 File Offset: 0x002ACD48
		[CompilerGenerated]
		public readonly bool Equals(HoverTip other)
		{
			return EqualityComparer<string>.Default.Equals(this.<Title>k__BackingField, other.<Title>k__BackingField) && EqualityComparer<Texture2D>.Default.Equals(this.<Icon>k__BackingField, other.<Icon>k__BackingField) && EqualityComparer<string>.Default.Equals(this.<Description>k__BackingField, other.<Description>k__BackingField) && EqualityComparer<string>.Default.Equals(this.<Id>k__BackingField, other.<Id>k__BackingField) && EqualityComparer<bool>.Default.Equals(this.<IsSmart>k__BackingField, other.<IsSmart>k__BackingField) && EqualityComparer<bool>.Default.Equals(this.<IsDebuff>k__BackingField, other.<IsDebuff>k__BackingField) && EqualityComparer<bool>.Default.Equals(this.<IsInstanced>k__BackingField, other.<IsInstanced>k__BackingField) && EqualityComparer<AbstractModel>.Default.Equals(this.<CanonicalModel>k__BackingField, other.<CanonicalModel>k__BackingField) && EqualityComparer<bool>.Default.Equals(this.<ShouldOverrideTextOverflow>k__BackingField, other.<ShouldOverrideTextOverflow>k__BackingField);
		}
	}
}
