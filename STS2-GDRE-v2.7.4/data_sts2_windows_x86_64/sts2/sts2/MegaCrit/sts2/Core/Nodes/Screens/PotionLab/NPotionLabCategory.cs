using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using Godot;
using Godot.Bridge;
using Godot.NativeInterop;
using MegaCrit.Sts2.addons.mega_text;
using MegaCrit.Sts2.Core.Entities.Potions;
using MegaCrit.Sts2.Core.Entities.UI;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.Localization;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Unlocks;

namespace MegaCrit.Sts2.Core.Nodes.Screens.PotionLab
{
	// Token: 0x020002DF RID: 735
	[ScriptPath("res://src/Core/Nodes/Screens/PotionLab/NPotionLabCategory.cs")]
	public class NPotionLabCategory : VBoxContainer
	{
		// Token: 0x0600274D RID: 10061 RVA: 0x0010C5CC File Offset: 0x0010A7CC
		public override void _Ready()
		{
			this._headerLabel = base.GetNode<MegaRichTextLabel>("Header");
			this._potionContainer = base.GetNode<GridContainer>("%PotionsContainer");
		}

		/// <summary>
		/// Set the potions to be displayed.
		/// </summary>
		/// <param name="potionRarity">The potion rarities to be displayed.</param>
		/// <param name="header">The header that will be displayed at the top of the category.</param>
		/// <param name="seenPotions">The potions that have been seen by the local player.</param>
		/// <param name="unlockState">The unlock state of the local player.</param>
		/// <param name="allUnlockedPotions">All the potions that the local player has unlocked.</param>
		/// <param name="secondRarity">An optional second rarity to display additionally. Only used in one specific category</param>
		// Token: 0x0600274E RID: 10062 RVA: 0x0010C5FC File Offset: 0x0010A7FC
		[NullableContext(1)]
		public void LoadPotions(PotionRarity potionRarity, LocString header, HashSet<PotionModel> seenPotions, UnlockState unlockState, HashSet<PotionModel> allUnlockedPotions, PotionRarity? secondRarity = null)
		{
			this._headerLabel.Text = header.GetFormattedText();
			IEnumerable<PotionModel> enumerable = ModelDb.AllPotions.Where<PotionModel>(delegate(PotionModel relic)
			{
				if (relic.Rarity != potionRarity)
				{
					PotionRarity rarity = relic.Rarity;
					PotionRarity? secondRarity2 = secondRarity;
					return (rarity == secondRarity2.GetValueOrDefault()) & (secondRarity2 != null);
				}
				return true;
			});
			List<PotionModel> list = new List<PotionModel>();
			List<PotionModel> list2 = new List<PotionModel>();
			foreach (PotionPoolModel potionPoolModel in ModelDb.AllCharacterPotionPools)
			{
				foreach (PotionModel potionModel in enumerable)
				{
					if (potionPoolModel.AllPotionIds.Contains(potionModel.Id))
					{
						list.Add(potionModel);
					}
				}
			}
			foreach (PotionModel potionModel2 in enumerable)
			{
				if (!list.Contains(potionModel2))
				{
					list2.Add(potionModel2);
				}
			}
			list2.Sort((PotionModel p1, PotionModel p2) => LocManager.Instance.StringComparer.Compare(p1.Title.GetFormattedText(), p2.Title.GetFormattedText()));
			foreach (PotionModel potionModel3 in list2.Concat<PotionModel>(list))
			{
				ModelVisibility modelVisibility = ((!allUnlockedPotions.Contains(potionModel3)) ? ModelVisibility.Locked : ((!seenPotions.Contains(potionModel3)) ? ModelVisibility.NotSeen : ModelVisibility.Visible));
				NLabPotionHolder nlabPotionHolder = NLabPotionHolder.Create(potionModel3.ToMutable(), modelVisibility);
				this._potionContainer.AddChildSafely(nlabPotionHolder);
			}
		}

		// Token: 0x0600274F RID: 10063 RVA: 0x0010C7C8 File Offset: 0x0010A9C8
		public void ClearPotions()
		{
			foreach (Node node in this._potionContainer.GetChildren(false))
			{
				node.QueueFreeSafely();
			}
		}

		// Token: 0x170006F1 RID: 1777
		// (get) Token: 0x06002750 RID: 10064 RVA: 0x0010C81C File Offset: 0x0010AA1C
		[Nullable(2)]
		public Control DefaultFocusedControl
		{
			[NullableContext(2)]
			get
			{
				if (this._potionContainer.GetChildCount(false) <= 0)
				{
					return null;
				}
				return this._potionContainer.GetChild<Control>(0, false);
			}
		}

		// Token: 0x06002751 RID: 10065 RVA: 0x0010C83C File Offset: 0x0010AA3C
		[NullableContext(1)]
		public List<IReadOnlyList<Control>> GetGridItems()
		{
			List<IReadOnlyList<Control>> list = new List<IReadOnlyList<Control>>();
			for (int i = 0; i < this._potionContainer.GetChildren(false).Count; i += this._potionContainer.Columns)
			{
				list.Add(this._potionContainer.GetChildren(false).OfType<Control>().Skip<Control>(i)
					.Take<Control>(this._potionContainer.Columns)
					.ToList<Control>());
			}
			return list;
		}

		/// <summary>
		/// Get the method information for all the methods declared in this class.
		/// This method is used by Godot to register the available methods in the editor.
		/// Do not call this method.
		/// </summary>
		// Token: 0x06002752 RID: 10066 RVA: 0x0010C8A8 File Offset: 0x0010AAA8
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal static List<MethodInfo> GetGodotMethodList()
		{
			return new List<MethodInfo>(2)
			{
				new MethodInfo(NPotionLabCategory.MethodName._Ready, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NPotionLabCategory.MethodName.ClearPotions, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null)
			};
		}

		/// <inheritdoc />
		// Token: 0x06002753 RID: 10067 RVA: 0x0010C91C File Offset: 0x0010AB1C
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool InvokeGodotClassMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
		{
			if ((ref method) == NPotionLabCategory.MethodName._Ready && args.Count == 0)
			{
				this._Ready();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NPotionLabCategory.MethodName.ClearPotions && args.Count == 0)
			{
				this.ClearPotions();
				ret = default(godot_variant);
				return true;
			}
			return base.InvokeGodotClassMethod(ref method, args, ref ret);
		}

		/// <inheritdoc />
		// Token: 0x06002754 RID: 10068 RVA: 0x0010C97C File Offset: 0x0010AB7C
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool HasGodotClassMethod(in godot_string_name method)
		{
			return (ref method) == NPotionLabCategory.MethodName._Ready || (ref method) == NPotionLabCategory.MethodName.ClearPotions || base.HasGodotClassMethod(ref method);
		}

		/// <inheritdoc />
		// Token: 0x06002755 RID: 10069 RVA: 0x0010C9A3 File Offset: 0x0010ABA3
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool SetGodotClassPropertyValue(in godot_string_name name, in godot_variant value)
		{
			if ((ref name) == NPotionLabCategory.PropertyName._headerLabel)
			{
				this._headerLabel = VariantUtils.ConvertTo<MegaRichTextLabel>(ref value);
				return true;
			}
			if ((ref name) == NPotionLabCategory.PropertyName._potionContainer)
			{
				this._potionContainer = VariantUtils.ConvertTo<GridContainer>(ref value);
				return true;
			}
			return base.SetGodotClassPropertyValue(ref name, ref value);
		}

		/// <inheritdoc />
		// Token: 0x06002756 RID: 10070 RVA: 0x0010C9E4 File Offset: 0x0010ABE4
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
		{
			if ((ref name) == NPotionLabCategory.PropertyName.DefaultFocusedControl)
			{
				Control defaultFocusedControl = this.DefaultFocusedControl;
				value = VariantUtils.CreateFrom<Control>(ref defaultFocusedControl);
				return true;
			}
			if ((ref name) == NPotionLabCategory.PropertyName._headerLabel)
			{
				value = VariantUtils.CreateFrom<MegaRichTextLabel>(ref this._headerLabel);
				return true;
			}
			if ((ref name) == NPotionLabCategory.PropertyName._potionContainer)
			{
				value = VariantUtils.CreateFrom<GridContainer>(ref this._potionContainer);
				return true;
			}
			return base.GetGodotClassPropertyValue(ref name, ref value);
		}

		/// <summary>
		/// Get the property information for all the properties declared in this class.
		/// This method is used by Godot to register the available properties in the editor.
		/// Do not call this method.
		/// </summary>
		// Token: 0x06002757 RID: 10071 RVA: 0x0010CA5C File Offset: 0x0010AC5C
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal static List<PropertyInfo> GetGodotPropertyList()
		{
			return new List<PropertyInfo>
			{
				new PropertyInfo(24L, NPotionLabCategory.PropertyName._headerLabel, 0L, "", 4096L, false),
				new PropertyInfo(24L, NPotionLabCategory.PropertyName._potionContainer, 0L, "", 4096L, false),
				new PropertyInfo(24L, NPotionLabCategory.PropertyName.DefaultFocusedControl, 0L, "", 4096L, false)
			};
		}

		/// <inheritdoc />
		// Token: 0x06002758 RID: 10072 RVA: 0x0010CAD3 File Offset: 0x0010ACD3
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void SaveGodotObjectData(GodotSerializationInfo info)
		{
			base.SaveGodotObjectData(info);
			info.AddProperty(NPotionLabCategory.PropertyName._headerLabel, Variant.From<MegaRichTextLabel>(ref this._headerLabel));
			info.AddProperty(NPotionLabCategory.PropertyName._potionContainer, Variant.From<GridContainer>(ref this._potionContainer));
		}

		/// <inheritdoc />
		// Token: 0x06002759 RID: 10073 RVA: 0x0010CB08 File Offset: 0x0010AD08
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void RestoreGodotObjectData(GodotSerializationInfo info)
		{
			base.RestoreGodotObjectData(info);
			Variant variant;
			if (info.TryGetProperty(NPotionLabCategory.PropertyName._headerLabel, ref variant))
			{
				this._headerLabel = variant.As<MegaRichTextLabel>();
			}
			Variant variant2;
			if (info.TryGetProperty(NPotionLabCategory.PropertyName._potionContainer, ref variant2))
			{
				this._potionContainer = variant2.As<GridContainer>();
			}
		}

		// Token: 0x04000EA1 RID: 3745
		[Nullable(1)]
		private MegaRichTextLabel _headerLabel;

		// Token: 0x04000EA2 RID: 3746
		[Nullable(1)]
		private GridContainer _potionContainer;

		/// <summary>
		/// Cached StringNames for the methods contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x02001471 RID: 5233
		public class MethodName : VBoxContainer.MethodName
		{
			/// <summary>
			/// Cached name for the '_Ready' method.
			/// </summary>
			// Token: 0x04004D92 RID: 19858
			public static readonly StringName _Ready = "_Ready";

			/// <summary>
			/// Cached name for the 'ClearPotions' method.
			/// </summary>
			// Token: 0x04004D93 RID: 19859
			public static readonly StringName ClearPotions = "ClearPotions";
		}

		/// <summary>
		/// Cached StringNames for the properties and fields contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x02001472 RID: 5234
		public class PropertyName : VBoxContainer.PropertyName
		{
			/// <summary>
			/// Cached name for the 'DefaultFocusedControl' property.
			/// </summary>
			// Token: 0x04004D94 RID: 19860
			public static readonly StringName DefaultFocusedControl = "DefaultFocusedControl";

			/// <summary>
			/// Cached name for the '_headerLabel' field.
			/// </summary>
			// Token: 0x04004D95 RID: 19861
			public static readonly StringName _headerLabel = "_headerLabel";

			/// <summary>
			/// Cached name for the '_potionContainer' field.
			/// </summary>
			// Token: 0x04004D96 RID: 19862
			public static readonly StringName _potionContainer = "_potionContainer";
		}

		/// <summary>
		/// Cached StringNames for the signals contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x02001473 RID: 5235
		public class SignalName : VBoxContainer.SignalName
		{
		}
	}
}
