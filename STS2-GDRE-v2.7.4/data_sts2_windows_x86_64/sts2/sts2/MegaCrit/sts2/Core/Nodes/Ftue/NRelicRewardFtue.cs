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
using MegaCrit.Sts2.Core.Logging;
using MegaCrit.Sts2.Core.Nodes.GodotExtensions;
using MegaCrit.Sts2.Core.TestSupport;

namespace MegaCrit.Sts2.Core.Nodes.Ftue
{
	// Token: 0x020003B3 RID: 947
	[NullableContext(1)]
	[Nullable(0)]
	[ScriptPath("res://src/Core/Nodes/Ftue/NRelicRewardFtue.cs")]
	public class NRelicRewardFtue : NFtue
	{
		// Token: 0x060038FC RID: 14588 RVA: 0x001AEFB8 File Offset: 0x001AD1B8
		public override void _Ready()
		{
			this._header = base.GetNode<MegaLabel>("FtuePopup/Header");
			this._header.SetTextAutoSize(new LocString("ftues", "RELIC_FTUE_TITLE").GetFormattedText());
			this._description = base.GetNode<MegaRichTextLabel>("FtuePopup/DescriptionContainer/Description");
			this._description.Text = new LocString("ftues", "RELIC_FTUE_DESCRIPTION").GetFormattedText();
			this._confirmButton = base.GetNode<NButton>("FtuePopup/FtueConfirmButton");
			this._confirmButton.Connect(NClickableControl.SignalName.Released, Callable.From<NButton>(new Action<NButton>(this.CloseFtue)), 0U);
			this._arrow = base.GetNode<Control>("%Arrow");
			this._arrow.GlobalPosition = this._relicReward.GlobalPosition + new Vector2(this._relicReward.Size.X * 0.5f, this._relicReward.Size.Y);
			DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(2, 3);
			defaultInterpolatedStringHandler.AppendFormatted<Vector2>(this._relicReward.GlobalPosition);
			defaultInterpolatedStringHandler.AppendLiteral(" ");
			defaultInterpolatedStringHandler.AppendFormatted<Vector2>(this._relicReward.Size);
			defaultInterpolatedStringHandler.AppendLiteral(" ");
			defaultInterpolatedStringHandler.AppendFormatted<Vector2>(this._arrow.GlobalPosition);
			Log.Info(defaultInterpolatedStringHandler.ToStringAndClear(), 2);
			this._defaultZIndex = this._relicReward.ZIndex;
			Control relicReward = this._relicReward;
			int zindex = relicReward.ZIndex;
			relicReward.ZIndex = zindex + 1;
		}

		// Token: 0x060038FD RID: 14589 RVA: 0x001AF150 File Offset: 0x001AD350
		[return: Nullable(2)]
		public static NRelicRewardFtue Create(Control relicReward)
		{
			if (TestMode.IsOn)
			{
				return null;
			}
			NRelicRewardFtue nrelicRewardFtue = PreloadManager.Cache.GetScene(NRelicRewardFtue._scenePath).Instantiate<NRelicRewardFtue>(0L);
			nrelicRewardFtue._relicReward = relicReward;
			return nrelicRewardFtue;
		}

		// Token: 0x060038FE RID: 14590 RVA: 0x001AF185 File Offset: 0x001AD385
		private void CloseFtue(NButton _)
		{
			this._relicReward.ZIndex = this._defaultZIndex;
			base.CloseFtue();
		}

		/// <summary>
		/// Get the method information for all the methods declared in this class.
		/// This method is used by Godot to register the available methods in the editor.
		/// Do not call this method.
		/// </summary>
		// Token: 0x060038FF RID: 14591 RVA: 0x001AF1A0 File Offset: 0x001AD3A0
		[NullableContext(0)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal new static List<MethodInfo> GetGodotMethodList()
		{
			return new List<MethodInfo>(3)
			{
				new MethodInfo(NRelicRewardFtue.MethodName._Ready, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NRelicRewardFtue.MethodName.Create, new PropertyInfo(24L, "", 0L, "", 6L, new StringName("Control"), false), 33L, new List<PropertyInfo>
				{
					new PropertyInfo(24L, "relicReward", 0L, "", 6L, new StringName("Control"), false)
				}, null),
				new MethodInfo(NRelicRewardFtue.MethodName.CloseFtue, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, new List<PropertyInfo>
				{
					new PropertyInfo(24L, "_", 0L, "", 6L, new StringName("Control"), false)
				}, null)
			};
		}

		/// <inheritdoc />
		// Token: 0x06003900 RID: 14592 RVA: 0x001AF2B0 File Offset: 0x001AD4B0
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool InvokeGodotClassMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
		{
			if ((ref method) == NRelicRewardFtue.MethodName._Ready && args.Count == 0)
			{
				this._Ready();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NRelicRewardFtue.MethodName.Create && args.Count == 1)
			{
				NRelicRewardFtue nrelicRewardFtue = NRelicRewardFtue.Create(VariantUtils.ConvertTo<Control>(args[0]));
				ret = VariantUtils.CreateFrom<NRelicRewardFtue>(ref nrelicRewardFtue);
				return true;
			}
			if ((ref method) == NRelicRewardFtue.MethodName.CloseFtue && args.Count == 1)
			{
				this.CloseFtue(VariantUtils.ConvertTo<NButton>(args[0]));
				ret = default(godot_variant);
				return true;
			}
			return base.InvokeGodotClassMethod(in method, args, out ret);
		}

		// Token: 0x06003901 RID: 14593 RVA: 0x001AF358 File Offset: 0x001AD558
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal static bool InvokeGodotClassStaticMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
		{
			if ((ref method) == NRelicRewardFtue.MethodName.Create && args.Count == 1)
			{
				NRelicRewardFtue nrelicRewardFtue = NRelicRewardFtue.Create(VariantUtils.ConvertTo<Control>(args[0]));
				ret = VariantUtils.CreateFrom<NRelicRewardFtue>(ref nrelicRewardFtue);
				return true;
			}
			ret = default(godot_variant);
			return false;
		}

		/// <inheritdoc />
		// Token: 0x06003902 RID: 14594 RVA: 0x001AF3A6 File Offset: 0x001AD5A6
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool HasGodotClassMethod(in godot_string_name method)
		{
			return (ref method) == NRelicRewardFtue.MethodName._Ready || (ref method) == NRelicRewardFtue.MethodName.Create || (ref method) == NRelicRewardFtue.MethodName.CloseFtue || base.HasGodotClassMethod(in method);
		}

		/// <inheritdoc />
		// Token: 0x06003903 RID: 14595 RVA: 0x001AF3DC File Offset: 0x001AD5DC
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool SetGodotClassPropertyValue(in godot_string_name name, in godot_variant value)
		{
			if ((ref name) == NRelicRewardFtue.PropertyName._confirmButton)
			{
				this._confirmButton = VariantUtils.ConvertTo<NButton>(ref value);
				return true;
			}
			if ((ref name) == NRelicRewardFtue.PropertyName._header)
			{
				this._header = VariantUtils.ConvertTo<MegaLabel>(ref value);
				return true;
			}
			if ((ref name) == NRelicRewardFtue.PropertyName._description)
			{
				this._description = VariantUtils.ConvertTo<MegaRichTextLabel>(ref value);
				return true;
			}
			if ((ref name) == NRelicRewardFtue.PropertyName._arrow)
			{
				this._arrow = VariantUtils.ConvertTo<Control>(ref value);
				return true;
			}
			if ((ref name) == NRelicRewardFtue.PropertyName._defaultZIndex)
			{
				this._defaultZIndex = VariantUtils.ConvertTo<int>(ref value);
				return true;
			}
			if ((ref name) == NRelicRewardFtue.PropertyName._relicReward)
			{
				this._relicReward = VariantUtils.ConvertTo<Control>(ref value);
				return true;
			}
			return base.SetGodotClassPropertyValue(ref name, ref value);
		}

		/// <inheritdoc />
		// Token: 0x06003904 RID: 14596 RVA: 0x001AF494 File Offset: 0x001AD694
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
		{
			if ((ref name) == NRelicRewardFtue.PropertyName._confirmButton)
			{
				value = VariantUtils.CreateFrom<NButton>(ref this._confirmButton);
				return true;
			}
			if ((ref name) == NRelicRewardFtue.PropertyName._header)
			{
				value = VariantUtils.CreateFrom<MegaLabel>(ref this._header);
				return true;
			}
			if ((ref name) == NRelicRewardFtue.PropertyName._description)
			{
				value = VariantUtils.CreateFrom<MegaRichTextLabel>(ref this._description);
				return true;
			}
			if ((ref name) == NRelicRewardFtue.PropertyName._arrow)
			{
				value = VariantUtils.CreateFrom<Control>(ref this._arrow);
				return true;
			}
			if ((ref name) == NRelicRewardFtue.PropertyName._defaultZIndex)
			{
				value = VariantUtils.CreateFrom<int>(ref this._defaultZIndex);
				return true;
			}
			if ((ref name) == NRelicRewardFtue.PropertyName._relicReward)
			{
				value = VariantUtils.CreateFrom<Control>(ref this._relicReward);
				return true;
			}
			return base.GetGodotClassPropertyValue(in name, out value);
		}

		/// <summary>
		/// Get the property information for all the properties declared in this class.
		/// This method is used by Godot to register the available properties in the editor.
		/// Do not call this method.
		/// </summary>
		// Token: 0x06003905 RID: 14597 RVA: 0x001AF56C File Offset: 0x001AD76C
		[NullableContext(0)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal new static List<PropertyInfo> GetGodotPropertyList()
		{
			return new List<PropertyInfo>
			{
				new PropertyInfo(24L, NRelicRewardFtue.PropertyName._confirmButton, 0L, "", 4096L, false),
				new PropertyInfo(24L, NRelicRewardFtue.PropertyName._header, 0L, "", 4096L, false),
				new PropertyInfo(24L, NRelicRewardFtue.PropertyName._description, 0L, "", 4096L, false),
				new PropertyInfo(24L, NRelicRewardFtue.PropertyName._arrow, 0L, "", 4096L, false),
				new PropertyInfo(2L, NRelicRewardFtue.PropertyName._defaultZIndex, 0L, "", 4096L, false),
				new PropertyInfo(24L, NRelicRewardFtue.PropertyName._relicReward, 0L, "", 4096L, false)
			};
		}

		/// <inheritdoc />
		// Token: 0x06003906 RID: 14598 RVA: 0x001AF648 File Offset: 0x001AD848
		[NullableContext(0)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void SaveGodotObjectData(GodotSerializationInfo info)
		{
			base.SaveGodotObjectData(info);
			info.AddProperty(NRelicRewardFtue.PropertyName._confirmButton, Variant.From<NButton>(ref this._confirmButton));
			info.AddProperty(NRelicRewardFtue.PropertyName._header, Variant.From<MegaLabel>(ref this._header));
			info.AddProperty(NRelicRewardFtue.PropertyName._description, Variant.From<MegaRichTextLabel>(ref this._description));
			info.AddProperty(NRelicRewardFtue.PropertyName._arrow, Variant.From<Control>(ref this._arrow));
			info.AddProperty(NRelicRewardFtue.PropertyName._defaultZIndex, Variant.From<int>(ref this._defaultZIndex));
			info.AddProperty(NRelicRewardFtue.PropertyName._relicReward, Variant.From<Control>(ref this._relicReward));
		}

		/// <inheritdoc />
		// Token: 0x06003907 RID: 14599 RVA: 0x001AF6E0 File Offset: 0x001AD8E0
		[NullableContext(0)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void RestoreGodotObjectData(GodotSerializationInfo info)
		{
			base.RestoreGodotObjectData(info);
			Variant variant;
			if (info.TryGetProperty(NRelicRewardFtue.PropertyName._confirmButton, ref variant))
			{
				this._confirmButton = variant.As<NButton>();
			}
			Variant variant2;
			if (info.TryGetProperty(NRelicRewardFtue.PropertyName._header, ref variant2))
			{
				this._header = variant2.As<MegaLabel>();
			}
			Variant variant3;
			if (info.TryGetProperty(NRelicRewardFtue.PropertyName._description, ref variant3))
			{
				this._description = variant3.As<MegaRichTextLabel>();
			}
			Variant variant4;
			if (info.TryGetProperty(NRelicRewardFtue.PropertyName._arrow, ref variant4))
			{
				this._arrow = variant4.As<Control>();
			}
			Variant variant5;
			if (info.TryGetProperty(NRelicRewardFtue.PropertyName._defaultZIndex, ref variant5))
			{
				this._defaultZIndex = variant5.As<int>();
			}
			Variant variant6;
			if (info.TryGetProperty(NRelicRewardFtue.PropertyName._relicReward, ref variant6))
			{
				this._relicReward = variant6.As<Control>();
			}
		}

		// Token: 0x0400161F RID: 5663
		public const string id = "obtain_relic_ftue";

		// Token: 0x04001620 RID: 5664
		private static readonly string _scenePath = SceneHelper.GetScenePath("ftue/obtain_relic_ftue");

		// Token: 0x04001621 RID: 5665
		private NButton _confirmButton;

		// Token: 0x04001622 RID: 5666
		private MegaLabel _header;

		// Token: 0x04001623 RID: 5667
		private MegaRichTextLabel _description;

		// Token: 0x04001624 RID: 5668
		private Control _arrow;

		// Token: 0x04001625 RID: 5669
		private int _defaultZIndex;

		// Token: 0x04001626 RID: 5670
		private Control _relicReward;

		/// <summary>
		/// Cached StringNames for the methods contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x020017B5 RID: 6069
		[NullableContext(0)]
		public new class MethodName : NFtue.MethodName
		{
			/// <summary>
			/// Cached name for the '_Ready' method.
			/// </summary>
			// Token: 0x04005C58 RID: 23640
			public static readonly StringName _Ready = "_Ready";

			/// <summary>
			/// Cached name for the 'Create' method.
			/// </summary>
			// Token: 0x04005C59 RID: 23641
			public static readonly StringName Create = "Create";

			/// <summary>
			/// Cached name for the 'CloseFtue' method.
			/// </summary>
			// Token: 0x04005C5A RID: 23642
			public new static readonly StringName CloseFtue = "CloseFtue";
		}

		/// <summary>
		/// Cached StringNames for the properties and fields contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x020017B6 RID: 6070
		[NullableContext(0)]
		public new class PropertyName : NFtue.PropertyName
		{
			/// <summary>
			/// Cached name for the '_confirmButton' field.
			/// </summary>
			// Token: 0x04005C5B RID: 23643
			public static readonly StringName _confirmButton = "_confirmButton";

			/// <summary>
			/// Cached name for the '_header' field.
			/// </summary>
			// Token: 0x04005C5C RID: 23644
			public static readonly StringName _header = "_header";

			/// <summary>
			/// Cached name for the '_description' field.
			/// </summary>
			// Token: 0x04005C5D RID: 23645
			public static readonly StringName _description = "_description";

			/// <summary>
			/// Cached name for the '_arrow' field.
			/// </summary>
			// Token: 0x04005C5E RID: 23646
			public static readonly StringName _arrow = "_arrow";

			/// <summary>
			/// Cached name for the '_defaultZIndex' field.
			/// </summary>
			// Token: 0x04005C5F RID: 23647
			public static readonly StringName _defaultZIndex = "_defaultZIndex";

			/// <summary>
			/// Cached name for the '_relicReward' field.
			/// </summary>
			// Token: 0x04005C60 RID: 23648
			public static readonly StringName _relicReward = "_relicReward";
		}

		/// <summary>
		/// Cached StringNames for the signals contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x020017B7 RID: 6071
		[NullableContext(0)]
		public new class SignalName : NFtue.SignalName
		{
		}
	}
}
