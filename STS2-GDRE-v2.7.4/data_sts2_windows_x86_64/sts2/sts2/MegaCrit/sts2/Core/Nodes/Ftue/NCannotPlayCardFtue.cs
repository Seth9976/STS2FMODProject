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
using MegaCrit.Sts2.Core.Nodes.Combat;
using MegaCrit.Sts2.Core.Nodes.GodotExtensions;
using MegaCrit.Sts2.Core.Nodes.Rooms;
using MegaCrit.Sts2.Core.TestSupport;

namespace MegaCrit.Sts2.Core.Nodes.Ftue
{
	// Token: 0x020003A9 RID: 937
	[ScriptPath("res://src/Core/Nodes/Ftue/NCannotPlayCardFtue.cs")]
	public class NCannotPlayCardFtue : NFtue
	{
		// Token: 0x0600386A RID: 14442 RVA: 0x001A9F80 File Offset: 0x001A8180
		public override void _Ready()
		{
			this._header = base.GetNode<MegaLabel>("FtuePopup/Header");
			this._header.SetTextAutoSize(new LocString("ftues", "CANNOT_PLAY_CARD_FTUE_TITLE").GetFormattedText());
			this._description = base.GetNode<MegaRichTextLabel>("FtuePopup/DescriptionContainer/Description");
			this._description.Text = new LocString("ftues", "CANNOT_PLAY_CARD_FTUE_DESCRIPTION").GetFormattedText();
			this._sneakyHitbox = base.GetNode<Control>("SneakyHitbox");
			this._sneakyHitbox.Connect(NClickableControl.SignalName.Released, Callable.From<NButton>(new Action<NButton>(this.CloseFtueAndEndTurn)), 0U);
			this._confirmButton = base.GetNode<NButton>("FtuePopup/FtueConfirmButton");
			this._confirmButton.Connect(NClickableControl.SignalName.Released, Callable.From<NButton>(new Action<NButton>(this.CloseFtue)), 0U);
			NEndTurnButton endTurnButton = NCombatRoom.Instance.Ui.EndTurnButton;
			endTurnButton.ZIndex = 1;
			this._sneakyHitbox.GlobalPosition = endTurnButton.GlobalPosition;
		}

		// Token: 0x0600386B RID: 14443 RVA: 0x001AA091 File Offset: 0x001A8291
		[NullableContext(2)]
		public static NCannotPlayCardFtue Create()
		{
			if (TestMode.IsOn)
			{
				return null;
			}
			return PreloadManager.Cache.GetScene(NCannotPlayCardFtue._scenePath).Instantiate<NCannotPlayCardFtue>(0L);
		}

		// Token: 0x0600386C RID: 14444 RVA: 0x001AA0B2 File Offset: 0x001A82B2
		[NullableContext(1)]
		private void CloseFtueAndEndTurn(NButton _)
		{
			NCombatRoom.Instance.Ui.EndTurnButton.SecretEndTurnLogicViaFtue();
			this.CloseFtue(_);
		}

		// Token: 0x0600386D RID: 14445 RVA: 0x001AA0CF File Offset: 0x001A82CF
		[NullableContext(1)]
		private void CloseFtue(NButton _)
		{
			NCombatRoom.Instance.Ui.EndTurnButton.ZIndex = 0;
			base.CloseFtue();
		}

		/// <summary>
		/// Get the method information for all the methods declared in this class.
		/// This method is used by Godot to register the available methods in the editor.
		/// Do not call this method.
		/// </summary>
		// Token: 0x0600386E RID: 14446 RVA: 0x001AA0EC File Offset: 0x001A82EC
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal new static List<MethodInfo> GetGodotMethodList()
		{
			return new List<MethodInfo>(4)
			{
				new MethodInfo(NCannotPlayCardFtue.MethodName._Ready, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NCannotPlayCardFtue.MethodName.Create, new PropertyInfo(24L, "", 0L, "", 6L, new StringName("Control"), false), 33L, null, null),
				new MethodInfo(NCannotPlayCardFtue.MethodName.CloseFtueAndEndTurn, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, new List<PropertyInfo>
				{
					new PropertyInfo(24L, "_", 0L, "", 6L, new StringName("Control"), false)
				}, null),
				new MethodInfo(NCannotPlayCardFtue.MethodName.CloseFtue, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, new List<PropertyInfo>
				{
					new PropertyInfo(24L, "_", 0L, "", 6L, new StringName("Control"), false)
				}, null)
			};
		}

		/// <inheritdoc />
		// Token: 0x0600386F RID: 14447 RVA: 0x001AA22C File Offset: 0x001A842C
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool InvokeGodotClassMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
		{
			if ((ref method) == NCannotPlayCardFtue.MethodName._Ready && args.Count == 0)
			{
				this._Ready();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NCannotPlayCardFtue.MethodName.Create && args.Count == 0)
			{
				NCannotPlayCardFtue ncannotPlayCardFtue = NCannotPlayCardFtue.Create();
				ret = VariantUtils.CreateFrom<NCannotPlayCardFtue>(ref ncannotPlayCardFtue);
				return true;
			}
			if ((ref method) == NCannotPlayCardFtue.MethodName.CloseFtueAndEndTurn && args.Count == 1)
			{
				this.CloseFtueAndEndTurn(VariantUtils.ConvertTo<NButton>(args[0]));
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NCannotPlayCardFtue.MethodName.CloseFtue && args.Count == 1)
			{
				this.CloseFtue(VariantUtils.ConvertTo<NButton>(args[0]));
				ret = default(godot_variant);
				return true;
			}
			return base.InvokeGodotClassMethod(in method, args, out ret);
		}

		// Token: 0x06003870 RID: 14448 RVA: 0x001AA2F8 File Offset: 0x001A84F8
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal static bool InvokeGodotClassStaticMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
		{
			if ((ref method) == NCannotPlayCardFtue.MethodName.Create && args.Count == 0)
			{
				NCannotPlayCardFtue ncannotPlayCardFtue = NCannotPlayCardFtue.Create();
				ret = VariantUtils.CreateFrom<NCannotPlayCardFtue>(ref ncannotPlayCardFtue);
				return true;
			}
			ret = default(godot_variant);
			return false;
		}

		/// <inheritdoc />
		// Token: 0x06003871 RID: 14449 RVA: 0x001AA338 File Offset: 0x001A8538
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool HasGodotClassMethod(in godot_string_name method)
		{
			return (ref method) == NCannotPlayCardFtue.MethodName._Ready || (ref method) == NCannotPlayCardFtue.MethodName.Create || (ref method) == NCannotPlayCardFtue.MethodName.CloseFtueAndEndTurn || (ref method) == NCannotPlayCardFtue.MethodName.CloseFtue || base.HasGodotClassMethod(in method);
		}

		/// <inheritdoc />
		// Token: 0x06003872 RID: 14450 RVA: 0x001AA388 File Offset: 0x001A8588
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool SetGodotClassPropertyValue(in godot_string_name name, in godot_variant value)
		{
			if ((ref name) == NCannotPlayCardFtue.PropertyName._confirmButton)
			{
				this._confirmButton = VariantUtils.ConvertTo<NButton>(ref value);
				return true;
			}
			if ((ref name) == NCannotPlayCardFtue.PropertyName._header)
			{
				this._header = VariantUtils.ConvertTo<MegaLabel>(ref value);
				return true;
			}
			if ((ref name) == NCannotPlayCardFtue.PropertyName._description)
			{
				this._description = VariantUtils.ConvertTo<MegaRichTextLabel>(ref value);
				return true;
			}
			if ((ref name) == NCannotPlayCardFtue.PropertyName._sneakyHitbox)
			{
				this._sneakyHitbox = VariantUtils.ConvertTo<Control>(ref value);
				return true;
			}
			return base.SetGodotClassPropertyValue(ref name, ref value);
		}

		/// <inheritdoc />
		// Token: 0x06003873 RID: 14451 RVA: 0x001AA40C File Offset: 0x001A860C
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
		{
			if ((ref name) == NCannotPlayCardFtue.PropertyName._confirmButton)
			{
				value = VariantUtils.CreateFrom<NButton>(ref this._confirmButton);
				return true;
			}
			if ((ref name) == NCannotPlayCardFtue.PropertyName._header)
			{
				value = VariantUtils.CreateFrom<MegaLabel>(ref this._header);
				return true;
			}
			if ((ref name) == NCannotPlayCardFtue.PropertyName._description)
			{
				value = VariantUtils.CreateFrom<MegaRichTextLabel>(ref this._description);
				return true;
			}
			if ((ref name) == NCannotPlayCardFtue.PropertyName._sneakyHitbox)
			{
				value = VariantUtils.CreateFrom<Control>(ref this._sneakyHitbox);
				return true;
			}
			return base.GetGodotClassPropertyValue(in name, out value);
		}

		/// <summary>
		/// Get the property information for all the properties declared in this class.
		/// This method is used by Godot to register the available properties in the editor.
		/// Do not call this method.
		/// </summary>
		// Token: 0x06003874 RID: 14452 RVA: 0x001AA4A4 File Offset: 0x001A86A4
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal new static List<PropertyInfo> GetGodotPropertyList()
		{
			return new List<PropertyInfo>
			{
				new PropertyInfo(24L, NCannotPlayCardFtue.PropertyName._confirmButton, 0L, "", 4096L, false),
				new PropertyInfo(24L, NCannotPlayCardFtue.PropertyName._header, 0L, "", 4096L, false),
				new PropertyInfo(24L, NCannotPlayCardFtue.PropertyName._description, 0L, "", 4096L, false),
				new PropertyInfo(24L, NCannotPlayCardFtue.PropertyName._sneakyHitbox, 0L, "", 4096L, false)
			};
		}

		/// <inheritdoc />
		// Token: 0x06003875 RID: 14453 RVA: 0x001AA53C File Offset: 0x001A873C
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void SaveGodotObjectData(GodotSerializationInfo info)
		{
			base.SaveGodotObjectData(info);
			info.AddProperty(NCannotPlayCardFtue.PropertyName._confirmButton, Variant.From<NButton>(ref this._confirmButton));
			info.AddProperty(NCannotPlayCardFtue.PropertyName._header, Variant.From<MegaLabel>(ref this._header));
			info.AddProperty(NCannotPlayCardFtue.PropertyName._description, Variant.From<MegaRichTextLabel>(ref this._description));
			info.AddProperty(NCannotPlayCardFtue.PropertyName._sneakyHitbox, Variant.From<Control>(ref this._sneakyHitbox));
		}

		/// <inheritdoc />
		// Token: 0x06003876 RID: 14454 RVA: 0x001AA5A8 File Offset: 0x001A87A8
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void RestoreGodotObjectData(GodotSerializationInfo info)
		{
			base.RestoreGodotObjectData(info);
			Variant variant;
			if (info.TryGetProperty(NCannotPlayCardFtue.PropertyName._confirmButton, ref variant))
			{
				this._confirmButton = variant.As<NButton>();
			}
			Variant variant2;
			if (info.TryGetProperty(NCannotPlayCardFtue.PropertyName._header, ref variant2))
			{
				this._header = variant2.As<MegaLabel>();
			}
			Variant variant3;
			if (info.TryGetProperty(NCannotPlayCardFtue.PropertyName._description, ref variant3))
			{
				this._description = variant3.As<MegaRichTextLabel>();
			}
			Variant variant4;
			if (info.TryGetProperty(NCannotPlayCardFtue.PropertyName._sneakyHitbox, ref variant4))
			{
				this._sneakyHitbox = variant4.As<Control>();
			}
		}

		// Token: 0x040015D7 RID: 5591
		[Nullable(1)]
		public const string id = "cannot_play_card_ftue";

		// Token: 0x040015D8 RID: 5592
		[Nullable(1)]
		private static readonly string _scenePath = SceneHelper.GetScenePath("ftue/cannot_play_card_ftue");

		// Token: 0x040015D9 RID: 5593
		[Nullable(1)]
		private NButton _confirmButton;

		// Token: 0x040015DA RID: 5594
		[Nullable(1)]
		private MegaLabel _header;

		// Token: 0x040015DB RID: 5595
		[Nullable(1)]
		private MegaRichTextLabel _description;

		// Token: 0x040015DC RID: 5596
		[Nullable(1)]
		private Control _sneakyHitbox;

		/// <summary>
		/// Cached StringNames for the methods contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x02001795 RID: 6037
		public new class MethodName : NFtue.MethodName
		{
			/// <summary>
			/// Cached name for the '_Ready' method.
			/// </summary>
			// Token: 0x04005BF6 RID: 23542
			public static readonly StringName _Ready = "_Ready";

			/// <summary>
			/// Cached name for the 'Create' method.
			/// </summary>
			// Token: 0x04005BF7 RID: 23543
			public static readonly StringName Create = "Create";

			/// <summary>
			/// Cached name for the 'CloseFtueAndEndTurn' method.
			/// </summary>
			// Token: 0x04005BF8 RID: 23544
			public static readonly StringName CloseFtueAndEndTurn = "CloseFtueAndEndTurn";

			/// <summary>
			/// Cached name for the 'CloseFtue' method.
			/// </summary>
			// Token: 0x04005BF9 RID: 23545
			public new static readonly StringName CloseFtue = "CloseFtue";
		}

		/// <summary>
		/// Cached StringNames for the properties and fields contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x02001796 RID: 6038
		public new class PropertyName : NFtue.PropertyName
		{
			/// <summary>
			/// Cached name for the '_confirmButton' field.
			/// </summary>
			// Token: 0x04005BFA RID: 23546
			public static readonly StringName _confirmButton = "_confirmButton";

			/// <summary>
			/// Cached name for the '_header' field.
			/// </summary>
			// Token: 0x04005BFB RID: 23547
			public static readonly StringName _header = "_header";

			/// <summary>
			/// Cached name for the '_description' field.
			/// </summary>
			// Token: 0x04005BFC RID: 23548
			public static readonly StringName _description = "_description";

			/// <summary>
			/// Cached name for the '_sneakyHitbox' field.
			/// </summary>
			// Token: 0x04005BFD RID: 23549
			public static readonly StringName _sneakyHitbox = "_sneakyHitbox";
		}

		/// <summary>
		/// Cached StringNames for the signals contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x02001797 RID: 6039
		public new class SignalName : NFtue.SignalName
		{
		}
	}
}
