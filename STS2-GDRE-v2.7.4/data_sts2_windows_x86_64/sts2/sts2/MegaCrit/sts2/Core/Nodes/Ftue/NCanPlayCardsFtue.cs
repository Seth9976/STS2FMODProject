using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Godot;
using Godot.Bridge;
using Godot.NativeInterop;
using MegaCrit.Sts2.addons.mega_text;
using MegaCrit.Sts2.Core.Assets;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.Localization;
using MegaCrit.Sts2.Core.Nodes.GodotExtensions;
using MegaCrit.Sts2.Core.Nodes.Rooms;
using MegaCrit.Sts2.Core.TestSupport;

namespace MegaCrit.Sts2.Core.Nodes.Ftue
{
	// Token: 0x020003AA RID: 938
	[ScriptPath("res://src/Core/Nodes/Ftue/NCanPlayCardsFtue.cs")]
	public class NCanPlayCardsFtue : NFtue
	{
		// Token: 0x06003879 RID: 14457 RVA: 0x001AA648 File Offset: 0x001A8848
		public override void _Ready()
		{
			this._header = base.GetNode<MegaLabel>("FtuePopup/Header");
			this._header.SetTextAutoSize(new LocString("ftues", "CAN_PLAY_CARDS_FTUE_TITLE").GetFormattedText());
			this._description = base.GetNode<MegaRichTextLabel>("FtuePopup/DescriptionContainer/Description");
			this._description.Text = new LocString("ftues", "CAN_PLAY_CARDS_FTUE_DESCRIPTION").GetFormattedText();
			this._confirmButton = base.GetNode<NButton>("FtuePopup/FtueConfirmButton");
			this._confirmButton.Connect(NClickableControl.SignalName.Released, Callable.From<NButton>(new Action<NButton>(this.CloseFtue)), 0U);
			NCombatRoom.Instance.Ui.EnergyCounterContainer.ZIndex = 1;
		}

		// Token: 0x0600387A RID: 14458 RVA: 0x001AA70D File Offset: 0x001A890D
		[NullableContext(2)]
		public static NCanPlayCardsFtue Create()
		{
			if (TestMode.IsOn)
			{
				return null;
			}
			return PreloadManager.Cache.GetScene(NCanPlayCardsFtue._scenePath).Instantiate<NCanPlayCardsFtue>(0L);
		}

		// Token: 0x0600387B RID: 14459 RVA: 0x001AA72E File Offset: 0x001A892E
		[NullableContext(1)]
		private void CloseFtue(NButton _)
		{
			NCombatRoom.Instance.Ui.EnergyCounterContainer.ZIndex = 0;
			base.CloseFtue();
		}

		/// <summary>
		/// Get the method information for all the methods declared in this class.
		/// This method is used by Godot to register the available methods in the editor.
		/// Do not call this method.
		/// </summary>
		// Token: 0x0600387C RID: 14460 RVA: 0x001AA74C File Offset: 0x001A894C
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal new static List<MethodInfo> GetGodotMethodList()
		{
			return new List<MethodInfo>(3)
			{
				new MethodInfo(NCanPlayCardsFtue.MethodName._Ready, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NCanPlayCardsFtue.MethodName.Create, new PropertyInfo(24L, "", 0L, "", 6L, new StringName("Control"), false), 33L, null, null),
				new MethodInfo(NCanPlayCardsFtue.MethodName.CloseFtue, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, new List<PropertyInfo>
				{
					new PropertyInfo(24L, "_", 0L, "", 6L, new StringName("Control"), false)
				}, null)
			};
		}

		/// <inheritdoc />
		// Token: 0x0600387D RID: 14461 RVA: 0x001AA82C File Offset: 0x001A8A2C
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool InvokeGodotClassMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
		{
			if ((ref method) == NCanPlayCardsFtue.MethodName._Ready && args.Count == 0)
			{
				this._Ready();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NCanPlayCardsFtue.MethodName.Create && args.Count == 0)
			{
				NCanPlayCardsFtue ncanPlayCardsFtue = NCanPlayCardsFtue.Create();
				ret = VariantUtils.CreateFrom<NCanPlayCardsFtue>(ref ncanPlayCardsFtue);
				return true;
			}
			if ((ref method) == NCanPlayCardsFtue.MethodName.CloseFtue && args.Count == 1)
			{
				this.CloseFtue(VariantUtils.ConvertTo<NButton>(args[0]));
				ret = default(godot_variant);
				return true;
			}
			return base.InvokeGodotClassMethod(in method, args, out ret);
		}

		// Token: 0x0600387E RID: 14462 RVA: 0x001AA8C8 File Offset: 0x001A8AC8
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal static bool InvokeGodotClassStaticMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
		{
			if ((ref method) == NCanPlayCardsFtue.MethodName.Create && args.Count == 0)
			{
				NCanPlayCardsFtue ncanPlayCardsFtue = NCanPlayCardsFtue.Create();
				ret = VariantUtils.CreateFrom<NCanPlayCardsFtue>(ref ncanPlayCardsFtue);
				return true;
			}
			ret = default(godot_variant);
			return false;
		}

		/// <inheritdoc />
		// Token: 0x0600387F RID: 14463 RVA: 0x001AA908 File Offset: 0x001A8B08
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool HasGodotClassMethod(in godot_string_name method)
		{
			return (ref method) == NCanPlayCardsFtue.MethodName._Ready || (ref method) == NCanPlayCardsFtue.MethodName.Create || (ref method) == NCanPlayCardsFtue.MethodName.CloseFtue || base.HasGodotClassMethod(in method);
		}

		/// <inheritdoc />
		// Token: 0x06003880 RID: 14464 RVA: 0x001AA940 File Offset: 0x001A8B40
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool SetGodotClassPropertyValue(in godot_string_name name, in godot_variant value)
		{
			if ((ref name) == NCanPlayCardsFtue.PropertyName._confirmButton)
			{
				this._confirmButton = VariantUtils.ConvertTo<NButton>(ref value);
				return true;
			}
			if ((ref name) == NCanPlayCardsFtue.PropertyName._header)
			{
				this._header = VariantUtils.ConvertTo<MegaLabel>(ref value);
				return true;
			}
			if ((ref name) == NCanPlayCardsFtue.PropertyName._description)
			{
				this._description = VariantUtils.ConvertTo<MegaRichTextLabel>(ref value);
				return true;
			}
			return base.SetGodotClassPropertyValue(ref name, ref value);
		}

		/// <inheritdoc />
		// Token: 0x06003881 RID: 14465 RVA: 0x001AA9A8 File Offset: 0x001A8BA8
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
		{
			if ((ref name) == NCanPlayCardsFtue.PropertyName._confirmButton)
			{
				value = VariantUtils.CreateFrom<NButton>(ref this._confirmButton);
				return true;
			}
			if ((ref name) == NCanPlayCardsFtue.PropertyName._header)
			{
				value = VariantUtils.CreateFrom<MegaLabel>(ref this._header);
				return true;
			}
			if ((ref name) == NCanPlayCardsFtue.PropertyName._description)
			{
				value = VariantUtils.CreateFrom<MegaRichTextLabel>(ref this._description);
				return true;
			}
			return base.GetGodotClassPropertyValue(in name, out value);
		}

		/// <summary>
		/// Get the property information for all the properties declared in this class.
		/// This method is used by Godot to register the available properties in the editor.
		/// Do not call this method.
		/// </summary>
		// Token: 0x06003882 RID: 14466 RVA: 0x001AAA20 File Offset: 0x001A8C20
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal new static List<PropertyInfo> GetGodotPropertyList()
		{
			return new List<PropertyInfo>
			{
				new PropertyInfo(24L, NCanPlayCardsFtue.PropertyName._confirmButton, 0L, "", 4096L, false),
				new PropertyInfo(24L, NCanPlayCardsFtue.PropertyName._header, 0L, "", 4096L, false),
				new PropertyInfo(24L, NCanPlayCardsFtue.PropertyName._description, 0L, "", 4096L, false)
			};
		}

		/// <inheritdoc />
		// Token: 0x06003883 RID: 14467 RVA: 0x001AAA98 File Offset: 0x001A8C98
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void SaveGodotObjectData(GodotSerializationInfo info)
		{
			base.SaveGodotObjectData(info);
			info.AddProperty(NCanPlayCardsFtue.PropertyName._confirmButton, Variant.From<NButton>(ref this._confirmButton));
			info.AddProperty(NCanPlayCardsFtue.PropertyName._header, Variant.From<MegaLabel>(ref this._header));
			info.AddProperty(NCanPlayCardsFtue.PropertyName._description, Variant.From<MegaRichTextLabel>(ref this._description));
		}

		/// <inheritdoc />
		// Token: 0x06003884 RID: 14468 RVA: 0x001AAAF0 File Offset: 0x001A8CF0
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void RestoreGodotObjectData(GodotSerializationInfo info)
		{
			base.RestoreGodotObjectData(info);
			Variant variant;
			if (info.TryGetProperty(NCanPlayCardsFtue.PropertyName._confirmButton, ref variant))
			{
				this._confirmButton = variant.As<NButton>();
			}
			Variant variant2;
			if (info.TryGetProperty(NCanPlayCardsFtue.PropertyName._header, ref variant2))
			{
				this._header = variant2.As<MegaLabel>();
			}
			Variant variant3;
			if (info.TryGetProperty(NCanPlayCardsFtue.PropertyName._description, ref variant3))
			{
				this._description = variant3.As<MegaRichTextLabel>();
			}
		}

		// Token: 0x040015DD RID: 5597
		[Nullable(1)]
		public const string id = "can_play_cards_ftue";

		// Token: 0x040015DE RID: 5598
		[Nullable(1)]
		private static readonly string _scenePath = SceneHelper.GetScenePath("ftue/can_play_cards_ftue");

		// Token: 0x040015DF RID: 5599
		[Nullable(1)]
		private NButton _confirmButton;

		// Token: 0x040015E0 RID: 5600
		[Nullable(1)]
		private MegaLabel _header;

		// Token: 0x040015E1 RID: 5601
		[Nullable(1)]
		private MegaRichTextLabel _description;

		/// <summary>
		/// Cached StringNames for the methods contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x02001798 RID: 6040
		public new class MethodName : NFtue.MethodName
		{
			/// <summary>
			/// Cached name for the '_Ready' method.
			/// </summary>
			// Token: 0x04005BFE RID: 23550
			public static readonly StringName _Ready = "_Ready";

			/// <summary>
			/// Cached name for the 'Create' method.
			/// </summary>
			// Token: 0x04005BFF RID: 23551
			public static readonly StringName Create = "Create";

			/// <summary>
			/// Cached name for the 'CloseFtue' method.
			/// </summary>
			// Token: 0x04005C00 RID: 23552
			public new static readonly StringName CloseFtue = "CloseFtue";
		}

		/// <summary>
		/// Cached StringNames for the properties and fields contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x02001799 RID: 6041
		public new class PropertyName : NFtue.PropertyName
		{
			/// <summary>
			/// Cached name for the '_confirmButton' field.
			/// </summary>
			// Token: 0x04005C01 RID: 23553
			public static readonly StringName _confirmButton = "_confirmButton";

			/// <summary>
			/// Cached name for the '_header' field.
			/// </summary>
			// Token: 0x04005C02 RID: 23554
			public static readonly StringName _header = "_header";

			/// <summary>
			/// Cached name for the '_description' field.
			/// </summary>
			// Token: 0x04005C03 RID: 23555
			public static readonly StringName _description = "_description";
		}

		/// <summary>
		/// Cached StringNames for the signals contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x0200179A RID: 6042
		public new class SignalName : NFtue.SignalName
		{
		}
	}
}
