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
using MegaCrit.Sts2.Core.Nodes.Combat;
using MegaCrit.Sts2.Core.Nodes.GodotExtensions;
using MegaCrit.Sts2.Core.Nodes.Rooms;
using MegaCrit.Sts2.Core.TestSupport;

namespace MegaCrit.Sts2.Core.Nodes.Ftue
{
	// Token: 0x020003B5 RID: 949
	[ScriptPath("res://src/Core/Nodes/Ftue/NShuffleFtue.cs")]
	public class NShuffleFtue : NFtue
	{
		// Token: 0x06003918 RID: 14616 RVA: 0x001AFEF8 File Offset: 0x001AE0F8
		public override void _Ready()
		{
			this._header = base.GetNode<MegaLabel>("FtuePopup/Header");
			this._header.SetTextAutoSize(new LocString("ftues", "SHUFFLE_FTUE_TITLE").GetFormattedText());
			this._description = base.GetNode<MegaRichTextLabel>("FtuePopup/DescriptionContainer/Description");
			this._description.Text = new LocString("ftues", "SHUFFLE_FTUE_DESCRIPTION").GetFormattedText();
			this._confirmButton = base.GetNode<NButton>("FtuePopup/FtueConfirmButton");
			this._confirmButton.Connect(NClickableControl.SignalName.Released, Callable.From<NButton>(new Action<NButton>(this.CloseFtue)), 0U);
			NCombatUi ui = NCombatRoom.Instance.Ui;
			this._defaultZIndex = ui.DrawPile.ZIndex;
			NDrawPileButton drawPile = ui.DrawPile;
			int num = drawPile.ZIndex;
			drawPile.ZIndex = num + 1;
			NDiscardPileButton discardPile = ui.DiscardPile;
			num = discardPile.ZIndex;
			discardPile.ZIndex = num + 1;
		}

		// Token: 0x06003919 RID: 14617 RVA: 0x001AFFEE File Offset: 0x001AE1EE
		[NullableContext(2)]
		public static NShuffleFtue Create()
		{
			if (TestMode.IsOn)
			{
				return null;
			}
			return PreloadManager.Cache.GetScene(NShuffleFtue._scenePath).Instantiate<NShuffleFtue>(0L);
		}

		// Token: 0x0600391A RID: 14618 RVA: 0x001B0010 File Offset: 0x001AE210
		[NullableContext(1)]
		public async Task WaitForPlayerToConfirm()
		{
			await this._confirmButton.AwaitSignal<NClickableControl>(NClickableControl.SignalName.Released, this);
		}

		// Token: 0x0600391B RID: 14619 RVA: 0x001B0054 File Offset: 0x001AE254
		[NullableContext(1)]
		private void CloseFtue(NButton _)
		{
			NCombatUi ui = NCombatRoom.Instance.Ui;
			ui.DrawPile.ZIndex = this._defaultZIndex;
			ui.DiscardPile.ZIndex = this._defaultZIndex;
			base.CloseFtue();
		}

		/// <summary>
		/// Get the method information for all the methods declared in this class.
		/// This method is used by Godot to register the available methods in the editor.
		/// Do not call this method.
		/// </summary>
		// Token: 0x0600391C RID: 14620 RVA: 0x001B0094 File Offset: 0x001AE294
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal new static List<MethodInfo> GetGodotMethodList()
		{
			return new List<MethodInfo>(3)
			{
				new MethodInfo(NShuffleFtue.MethodName._Ready, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NShuffleFtue.MethodName.Create, new PropertyInfo(24L, "", 0L, "", 6L, new StringName("Control"), false), 33L, null, null),
				new MethodInfo(NShuffleFtue.MethodName.CloseFtue, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, new List<PropertyInfo>
				{
					new PropertyInfo(24L, "_", 0L, "", 6L, new StringName("Control"), false)
				}, null)
			};
		}

		/// <inheritdoc />
		// Token: 0x0600391D RID: 14621 RVA: 0x001B0174 File Offset: 0x001AE374
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool InvokeGodotClassMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
		{
			if ((ref method) == NShuffleFtue.MethodName._Ready && args.Count == 0)
			{
				this._Ready();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NShuffleFtue.MethodName.Create && args.Count == 0)
			{
				NShuffleFtue nshuffleFtue = NShuffleFtue.Create();
				ret = VariantUtils.CreateFrom<NShuffleFtue>(ref nshuffleFtue);
				return true;
			}
			if ((ref method) == NShuffleFtue.MethodName.CloseFtue && args.Count == 1)
			{
				this.CloseFtue(VariantUtils.ConvertTo<NButton>(args[0]));
				ret = default(godot_variant);
				return true;
			}
			return base.InvokeGodotClassMethod(in method, args, out ret);
		}

		// Token: 0x0600391E RID: 14622 RVA: 0x001B0210 File Offset: 0x001AE410
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal static bool InvokeGodotClassStaticMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
		{
			if ((ref method) == NShuffleFtue.MethodName.Create && args.Count == 0)
			{
				NShuffleFtue nshuffleFtue = NShuffleFtue.Create();
				ret = VariantUtils.CreateFrom<NShuffleFtue>(ref nshuffleFtue);
				return true;
			}
			ret = default(godot_variant);
			return false;
		}

		/// <inheritdoc />
		// Token: 0x0600391F RID: 14623 RVA: 0x001B0250 File Offset: 0x001AE450
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool HasGodotClassMethod(in godot_string_name method)
		{
			return (ref method) == NShuffleFtue.MethodName._Ready || (ref method) == NShuffleFtue.MethodName.Create || (ref method) == NShuffleFtue.MethodName.CloseFtue || base.HasGodotClassMethod(in method);
		}

		/// <inheritdoc />
		// Token: 0x06003920 RID: 14624 RVA: 0x001B0288 File Offset: 0x001AE488
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool SetGodotClassPropertyValue(in godot_string_name name, in godot_variant value)
		{
			if ((ref name) == NShuffleFtue.PropertyName._confirmButton)
			{
				this._confirmButton = VariantUtils.ConvertTo<NButton>(ref value);
				return true;
			}
			if ((ref name) == NShuffleFtue.PropertyName._header)
			{
				this._header = VariantUtils.ConvertTo<MegaLabel>(ref value);
				return true;
			}
			if ((ref name) == NShuffleFtue.PropertyName._description)
			{
				this._description = VariantUtils.ConvertTo<MegaRichTextLabel>(ref value);
				return true;
			}
			if ((ref name) == NShuffleFtue.PropertyName._defaultZIndex)
			{
				this._defaultZIndex = VariantUtils.ConvertTo<int>(ref value);
				return true;
			}
			return base.SetGodotClassPropertyValue(ref name, ref value);
		}

		/// <inheritdoc />
		// Token: 0x06003921 RID: 14625 RVA: 0x001B030C File Offset: 0x001AE50C
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
		{
			if ((ref name) == NShuffleFtue.PropertyName._confirmButton)
			{
				value = VariantUtils.CreateFrom<NButton>(ref this._confirmButton);
				return true;
			}
			if ((ref name) == NShuffleFtue.PropertyName._header)
			{
				value = VariantUtils.CreateFrom<MegaLabel>(ref this._header);
				return true;
			}
			if ((ref name) == NShuffleFtue.PropertyName._description)
			{
				value = VariantUtils.CreateFrom<MegaRichTextLabel>(ref this._description);
				return true;
			}
			if ((ref name) == NShuffleFtue.PropertyName._defaultZIndex)
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
		// Token: 0x06003922 RID: 14626 RVA: 0x001B03A4 File Offset: 0x001AE5A4
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal new static List<PropertyInfo> GetGodotPropertyList()
		{
			return new List<PropertyInfo>
			{
				new PropertyInfo(24L, NShuffleFtue.PropertyName._confirmButton, 0L, "", 4096L, false),
				new PropertyInfo(24L, NShuffleFtue.PropertyName._header, 0L, "", 4096L, false),
				new PropertyInfo(24L, NShuffleFtue.PropertyName._description, 0L, "", 4096L, false),
				new PropertyInfo(2L, NShuffleFtue.PropertyName._defaultZIndex, 0L, "", 4096L, false)
			};
		}

		/// <inheritdoc />
		// Token: 0x06003923 RID: 14627 RVA: 0x001B043C File Offset: 0x001AE63C
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void SaveGodotObjectData(GodotSerializationInfo info)
		{
			base.SaveGodotObjectData(info);
			info.AddProperty(NShuffleFtue.PropertyName._confirmButton, Variant.From<NButton>(ref this._confirmButton));
			info.AddProperty(NShuffleFtue.PropertyName._header, Variant.From<MegaLabel>(ref this._header));
			info.AddProperty(NShuffleFtue.PropertyName._description, Variant.From<MegaRichTextLabel>(ref this._description));
			info.AddProperty(NShuffleFtue.PropertyName._defaultZIndex, Variant.From<int>(ref this._defaultZIndex));
		}

		/// <inheritdoc />
		// Token: 0x06003924 RID: 14628 RVA: 0x001B04A8 File Offset: 0x001AE6A8
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void RestoreGodotObjectData(GodotSerializationInfo info)
		{
			base.RestoreGodotObjectData(info);
			Variant variant;
			if (info.TryGetProperty(NShuffleFtue.PropertyName._confirmButton, ref variant))
			{
				this._confirmButton = variant.As<NButton>();
			}
			Variant variant2;
			if (info.TryGetProperty(NShuffleFtue.PropertyName._header, ref variant2))
			{
				this._header = variant2.As<MegaLabel>();
			}
			Variant variant3;
			if (info.TryGetProperty(NShuffleFtue.PropertyName._description, ref variant3))
			{
				this._description = variant3.As<MegaRichTextLabel>();
			}
			Variant variant4;
			if (info.TryGetProperty(NShuffleFtue.PropertyName._defaultZIndex, ref variant4))
			{
				this._defaultZIndex = variant4.As<int>();
			}
		}

		// Token: 0x0400162F RID: 5679
		[Nullable(1)]
		public const string id = "shuffle_ftue";

		// Token: 0x04001630 RID: 5680
		[Nullable(1)]
		private static readonly string _scenePath = SceneHelper.GetScenePath("ftue/shuffle_ftue");

		// Token: 0x04001631 RID: 5681
		[Nullable(1)]
		private NButton _confirmButton;

		// Token: 0x04001632 RID: 5682
		[Nullable(1)]
		private MegaLabel _header;

		// Token: 0x04001633 RID: 5683
		[Nullable(1)]
		private MegaRichTextLabel _description;

		// Token: 0x04001634 RID: 5684
		private int _defaultZIndex;

		/// <summary>
		/// Cached StringNames for the methods contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x020017BB RID: 6075
		public new class MethodName : NFtue.MethodName
		{
			/// <summary>
			/// Cached name for the '_Ready' method.
			/// </summary>
			// Token: 0x04005C6A RID: 23658
			public static readonly StringName _Ready = "_Ready";

			/// <summary>
			/// Cached name for the 'Create' method.
			/// </summary>
			// Token: 0x04005C6B RID: 23659
			public static readonly StringName Create = "Create";

			/// <summary>
			/// Cached name for the 'CloseFtue' method.
			/// </summary>
			// Token: 0x04005C6C RID: 23660
			public new static readonly StringName CloseFtue = "CloseFtue";
		}

		/// <summary>
		/// Cached StringNames for the properties and fields contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x020017BC RID: 6076
		public new class PropertyName : NFtue.PropertyName
		{
			/// <summary>
			/// Cached name for the '_confirmButton' field.
			/// </summary>
			// Token: 0x04005C6D RID: 23661
			public static readonly StringName _confirmButton = "_confirmButton";

			/// <summary>
			/// Cached name for the '_header' field.
			/// </summary>
			// Token: 0x04005C6E RID: 23662
			public static readonly StringName _header = "_header";

			/// <summary>
			/// Cached name for the '_description' field.
			/// </summary>
			// Token: 0x04005C6F RID: 23663
			public static readonly StringName _description = "_description";

			/// <summary>
			/// Cached name for the '_defaultZIndex' field.
			/// </summary>
			// Token: 0x04005C70 RID: 23664
			public static readonly StringName _defaultZIndex = "_defaultZIndex";
		}

		/// <summary>
		/// Cached StringNames for the signals contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x020017BD RID: 6077
		public new class SignalName : NFtue.SignalName
		{
		}
	}
}
