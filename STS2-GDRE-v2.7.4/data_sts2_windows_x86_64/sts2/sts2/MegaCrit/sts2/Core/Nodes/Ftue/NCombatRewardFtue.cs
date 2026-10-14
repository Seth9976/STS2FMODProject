using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Godot;
using Godot.Bridge;
using Godot.NativeInterop;
using MegaCrit.Sts2.addons.mega_text;
using MegaCrit.Sts2.Core.Assets;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.Localization;
using MegaCrit.Sts2.Core.Nodes.GodotExtensions;
using MegaCrit.Sts2.Core.TestSupport;

namespace MegaCrit.Sts2.Core.Nodes.Ftue
{
	/// <summary>
	/// Ftue which appears when a player presses Proceed with items remaining in the Combat Reward screen.
	/// Auto-disables after you Proceed without forgetting.
	/// </summary>
	// Token: 0x020003AB RID: 939
	[NullableContext(1)]
	[Nullable(0)]
	[ScriptPath("res://src/Core/Nodes/Ftue/NCombatRewardFtue.cs")]
	public class NCombatRewardFtue : NFtue
	{
		// Token: 0x06003887 RID: 14471 RVA: 0x001AAB74 File Offset: 0x001A8D74
		public override void _Ready()
		{
			this._header = base.GetNode<MegaLabel>("FtuePopup/Header");
			this._header.SetTextAutoSize(new LocString("ftues", "REWARDS_FTUE_TITLE").GetFormattedText());
			this._description = base.GetNode<MegaRichTextLabel>("FtuePopup/DescriptionContainer/Description");
			this._description.Text = new LocString("ftues", "REWARDS_FTUE_DESCRIPTION").GetFormattedText();
			this._confirmButton = base.GetNode<NButton>("FtuePopup/FtueConfirmButton");
			this._confirmButton.Connect(NClickableControl.SignalName.Released, Callable.From<NButton>(new Action<NButton>(this.CloseFtue)), 0U);
			this._defaultZIndex = this._rewardsContainer.ZIndex;
			Control rewardsContainer = this._rewardsContainer;
			int zindex = rewardsContainer.ZIndex;
			rewardsContainer.ZIndex = zindex + 1;
		}

		// Token: 0x06003888 RID: 14472 RVA: 0x001AAC4C File Offset: 0x001A8E4C
		[return: Nullable(2)]
		public static NCombatRewardFtue Create(Control rewardsContainer)
		{
			if (TestMode.IsOn)
			{
				return null;
			}
			NCombatRewardFtue ncombatRewardFtue = PreloadManager.Cache.GetScene(NCombatRewardFtue._scenePath).Instantiate<NCombatRewardFtue>(0L);
			ncombatRewardFtue._rewardsContainer = rewardsContainer;
			return ncombatRewardFtue;
		}

		// Token: 0x06003889 RID: 14473 RVA: 0x001AAC81 File Offset: 0x001A8E81
		private void CloseFtue(NButton _)
		{
			this._rewardsContainer.ZIndex = this._defaultZIndex;
			base.CloseFtue();
		}

		// Token: 0x0600388A RID: 14474 RVA: 0x001AAC9C File Offset: 0x001A8E9C
		public async Task WaitForPlayerToConfirm()
		{
			await this._confirmButton.AwaitSignal<NClickableControl>(NClickableControl.SignalName.Released, this);
		}

		/// <summary>
		/// Get the method information for all the methods declared in this class.
		/// This method is used by Godot to register the available methods in the editor.
		/// Do not call this method.
		/// </summary>
		// Token: 0x0600388B RID: 14475 RVA: 0x001AACE0 File Offset: 0x001A8EE0
		[NullableContext(0)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal new static List<MethodInfo> GetGodotMethodList()
		{
			return new List<MethodInfo>(3)
			{
				new MethodInfo(NCombatRewardFtue.MethodName._Ready, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NCombatRewardFtue.MethodName.Create, new PropertyInfo(24L, "", 0L, "", 6L, new StringName("Control"), false), 33L, new List<PropertyInfo>
				{
					new PropertyInfo(24L, "rewardsContainer", 0L, "", 6L, new StringName("Control"), false)
				}, null),
				new MethodInfo(NCombatRewardFtue.MethodName.CloseFtue, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, new List<PropertyInfo>
				{
					new PropertyInfo(24L, "_", 0L, "", 6L, new StringName("Control"), false)
				}, null)
			};
		}

		/// <inheritdoc />
		// Token: 0x0600388C RID: 14476 RVA: 0x001AADF0 File Offset: 0x001A8FF0
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool InvokeGodotClassMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
		{
			if ((ref method) == NCombatRewardFtue.MethodName._Ready && args.Count == 0)
			{
				this._Ready();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NCombatRewardFtue.MethodName.Create && args.Count == 1)
			{
				NCombatRewardFtue ncombatRewardFtue = NCombatRewardFtue.Create(VariantUtils.ConvertTo<Control>(args[0]));
				ret = VariantUtils.CreateFrom<NCombatRewardFtue>(ref ncombatRewardFtue);
				return true;
			}
			if ((ref method) == NCombatRewardFtue.MethodName.CloseFtue && args.Count == 1)
			{
				this.CloseFtue(VariantUtils.ConvertTo<NButton>(args[0]));
				ret = default(godot_variant);
				return true;
			}
			return base.InvokeGodotClassMethod(in method, args, out ret);
		}

		// Token: 0x0600388D RID: 14477 RVA: 0x001AAE98 File Offset: 0x001A9098
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal static bool InvokeGodotClassStaticMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
		{
			if ((ref method) == NCombatRewardFtue.MethodName.Create && args.Count == 1)
			{
				NCombatRewardFtue ncombatRewardFtue = NCombatRewardFtue.Create(VariantUtils.ConvertTo<Control>(args[0]));
				ret = VariantUtils.CreateFrom<NCombatRewardFtue>(ref ncombatRewardFtue);
				return true;
			}
			ret = default(godot_variant);
			return false;
		}

		/// <inheritdoc />
		// Token: 0x0600388E RID: 14478 RVA: 0x001AAEE6 File Offset: 0x001A90E6
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool HasGodotClassMethod(in godot_string_name method)
		{
			return (ref method) == NCombatRewardFtue.MethodName._Ready || (ref method) == NCombatRewardFtue.MethodName.Create || (ref method) == NCombatRewardFtue.MethodName.CloseFtue || base.HasGodotClassMethod(in method);
		}

		/// <inheritdoc />
		// Token: 0x0600388F RID: 14479 RVA: 0x001AAF1C File Offset: 0x001A911C
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool SetGodotClassPropertyValue(in godot_string_name name, in godot_variant value)
		{
			if ((ref name) == NCombatRewardFtue.PropertyName._confirmButton)
			{
				this._confirmButton = VariantUtils.ConvertTo<NButton>(ref value);
				return true;
			}
			if ((ref name) == NCombatRewardFtue.PropertyName._header)
			{
				this._header = VariantUtils.ConvertTo<MegaLabel>(ref value);
				return true;
			}
			if ((ref name) == NCombatRewardFtue.PropertyName._description)
			{
				this._description = VariantUtils.ConvertTo<MegaRichTextLabel>(ref value);
				return true;
			}
			if ((ref name) == NCombatRewardFtue.PropertyName._rewardsContainer)
			{
				this._rewardsContainer = VariantUtils.ConvertTo<Control>(ref value);
				return true;
			}
			if ((ref name) == NCombatRewardFtue.PropertyName._defaultZIndex)
			{
				this._defaultZIndex = VariantUtils.ConvertTo<int>(ref value);
				return true;
			}
			return base.SetGodotClassPropertyValue(ref name, ref value);
		}

		/// <inheritdoc />
		// Token: 0x06003890 RID: 14480 RVA: 0x001AAFB8 File Offset: 0x001A91B8
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
		{
			if ((ref name) == NCombatRewardFtue.PropertyName._confirmButton)
			{
				value = VariantUtils.CreateFrom<NButton>(ref this._confirmButton);
				return true;
			}
			if ((ref name) == NCombatRewardFtue.PropertyName._header)
			{
				value = VariantUtils.CreateFrom<MegaLabel>(ref this._header);
				return true;
			}
			if ((ref name) == NCombatRewardFtue.PropertyName._description)
			{
				value = VariantUtils.CreateFrom<MegaRichTextLabel>(ref this._description);
				return true;
			}
			if ((ref name) == NCombatRewardFtue.PropertyName._rewardsContainer)
			{
				value = VariantUtils.CreateFrom<Control>(ref this._rewardsContainer);
				return true;
			}
			if ((ref name) == NCombatRewardFtue.PropertyName._defaultZIndex)
			{
				value = VariantUtils.CreateFrom<int>(ref this._defaultZIndex);
				return true;
			}
			return base.GetGodotClassPropertyValue(in name, out value);
		}

		/// <summary>
		/// Get the property information for all the properties declared in this class.
		/// This method is used by Godot to register the available properties in the editor.
		/// Do not call this method.
		/// </summary>
		// Token: 0x06003891 RID: 14481 RVA: 0x001AB070 File Offset: 0x001A9270
		[NullableContext(0)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal new static List<PropertyInfo> GetGodotPropertyList()
		{
			return new List<PropertyInfo>
			{
				new PropertyInfo(24L, NCombatRewardFtue.PropertyName._confirmButton, 0L, "", 4096L, false),
				new PropertyInfo(24L, NCombatRewardFtue.PropertyName._header, 0L, "", 4096L, false),
				new PropertyInfo(24L, NCombatRewardFtue.PropertyName._description, 0L, "", 4096L, false),
				new PropertyInfo(24L, NCombatRewardFtue.PropertyName._rewardsContainer, 0L, "", 4096L, false),
				new PropertyInfo(2L, NCombatRewardFtue.PropertyName._defaultZIndex, 0L, "", 4096L, false)
			};
		}

		/// <inheritdoc />
		// Token: 0x06003892 RID: 14482 RVA: 0x001AB128 File Offset: 0x001A9328
		[NullableContext(0)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void SaveGodotObjectData(GodotSerializationInfo info)
		{
			base.SaveGodotObjectData(info);
			info.AddProperty(NCombatRewardFtue.PropertyName._confirmButton, Variant.From<NButton>(ref this._confirmButton));
			info.AddProperty(NCombatRewardFtue.PropertyName._header, Variant.From<MegaLabel>(ref this._header));
			info.AddProperty(NCombatRewardFtue.PropertyName._description, Variant.From<MegaRichTextLabel>(ref this._description));
			info.AddProperty(NCombatRewardFtue.PropertyName._rewardsContainer, Variant.From<Control>(ref this._rewardsContainer));
			info.AddProperty(NCombatRewardFtue.PropertyName._defaultZIndex, Variant.From<int>(ref this._defaultZIndex));
		}

		/// <inheritdoc />
		// Token: 0x06003893 RID: 14483 RVA: 0x001AB1AC File Offset: 0x001A93AC
		[NullableContext(0)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void RestoreGodotObjectData(GodotSerializationInfo info)
		{
			base.RestoreGodotObjectData(info);
			Variant variant;
			if (info.TryGetProperty(NCombatRewardFtue.PropertyName._confirmButton, ref variant))
			{
				this._confirmButton = variant.As<NButton>();
			}
			Variant variant2;
			if (info.TryGetProperty(NCombatRewardFtue.PropertyName._header, ref variant2))
			{
				this._header = variant2.As<MegaLabel>();
			}
			Variant variant3;
			if (info.TryGetProperty(NCombatRewardFtue.PropertyName._description, ref variant3))
			{
				this._description = variant3.As<MegaRichTextLabel>();
			}
			Variant variant4;
			if (info.TryGetProperty(NCombatRewardFtue.PropertyName._rewardsContainer, ref variant4))
			{
				this._rewardsContainer = variant4.As<Control>();
			}
			Variant variant5;
			if (info.TryGetProperty(NCombatRewardFtue.PropertyName._defaultZIndex, ref variant5))
			{
				this._defaultZIndex = variant5.As<int>();
			}
		}

		// Token: 0x040015E2 RID: 5602
		public const string id = "combat_reward_ftue";

		// Token: 0x040015E3 RID: 5603
		private static readonly string _scenePath = SceneHelper.GetScenePath("ftue/combat_reward_ftue");

		// Token: 0x040015E4 RID: 5604
		private NButton _confirmButton;

		// Token: 0x040015E5 RID: 5605
		private MegaLabel _header;

		// Token: 0x040015E6 RID: 5606
		private MegaRichTextLabel _description;

		// Token: 0x040015E7 RID: 5607
		private Control _rewardsContainer;

		// Token: 0x040015E8 RID: 5608
		private int _defaultZIndex;

		/// <summary>
		/// Cached StringNames for the methods contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x0200179B RID: 6043
		[NullableContext(0)]
		public new class MethodName : NFtue.MethodName
		{
			/// <summary>
			/// Cached name for the '_Ready' method.
			/// </summary>
			// Token: 0x04005C04 RID: 23556
			public static readonly StringName _Ready = "_Ready";

			/// <summary>
			/// Cached name for the 'Create' method.
			/// </summary>
			// Token: 0x04005C05 RID: 23557
			public static readonly StringName Create = "Create";

			/// <summary>
			/// Cached name for the 'CloseFtue' method.
			/// </summary>
			// Token: 0x04005C06 RID: 23558
			public new static readonly StringName CloseFtue = "CloseFtue";
		}

		/// <summary>
		/// Cached StringNames for the properties and fields contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x0200179C RID: 6044
		[NullableContext(0)]
		public new class PropertyName : NFtue.PropertyName
		{
			/// <summary>
			/// Cached name for the '_confirmButton' field.
			/// </summary>
			// Token: 0x04005C07 RID: 23559
			public static readonly StringName _confirmButton = "_confirmButton";

			/// <summary>
			/// Cached name for the '_header' field.
			/// </summary>
			// Token: 0x04005C08 RID: 23560
			public static readonly StringName _header = "_header";

			/// <summary>
			/// Cached name for the '_description' field.
			/// </summary>
			// Token: 0x04005C09 RID: 23561
			public static readonly StringName _description = "_description";

			/// <summary>
			/// Cached name for the '_rewardsContainer' field.
			/// </summary>
			// Token: 0x04005C0A RID: 23562
			public static readonly StringName _rewardsContainer = "_rewardsContainer";

			/// <summary>
			/// Cached name for the '_defaultZIndex' field.
			/// </summary>
			// Token: 0x04005C0B RID: 23563
			public static readonly StringName _defaultZIndex = "_defaultZIndex";
		}

		/// <summary>
		/// Cached StringNames for the signals contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x0200179D RID: 6045
		[NullableContext(0)]
		public new class SignalName : NFtue.SignalName
		{
		}
	}
}
