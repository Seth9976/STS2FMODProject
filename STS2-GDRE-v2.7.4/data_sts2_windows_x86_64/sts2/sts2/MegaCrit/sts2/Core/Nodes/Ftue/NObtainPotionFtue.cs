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
using MegaCrit.Sts2.Core.Nodes.Potions;
using MegaCrit.Sts2.Core.TestSupport;

namespace MegaCrit.Sts2.Core.Nodes.Ftue
{
	// Token: 0x020003B1 RID: 945
	[ScriptPath("res://src/Core/Nodes/Ftue/NObtainPotionFtue.cs")]
	public class NObtainPotionFtue : NFtue
	{
		// Token: 0x060038E0 RID: 14560 RVA: 0x001AE274 File Offset: 0x001AC474
		public override void _Ready()
		{
			this._header = base.GetNode<MegaLabel>("FtuePopup/Header");
			this._header.SetTextAutoSize(new LocString("ftues", "POTION_FTUE_TITLE").GetFormattedText());
			this._description = base.GetNode<MegaRichTextLabel>("FtuePopup/DescriptionContainer/Description");
			this._description.Text = new LocString("ftues", "POTION_FTUE_DESCRIPTION").GetFormattedText();
			this._confirmButton = base.GetNode<NButton>("FtuePopup/FtueConfirmButton");
			this._confirmButton.Connect(NClickableControl.SignalName.Released, Callable.From<NButton>(new Action<NButton>(this.CloseFtue)), 0U);
			this._defaultZIndex = NRun.Instance.GlobalUi.TopBar.PotionContainer.ZIndex;
			NPotionContainer potionContainer = NRun.Instance.GlobalUi.TopBar.PotionContainer;
			int zindex = potionContainer.ZIndex;
			potionContainer.ZIndex = zindex + 1;
		}

		// Token: 0x060038E1 RID: 14561 RVA: 0x001AE366 File Offset: 0x001AC566
		[NullableContext(2)]
		public static NObtainPotionFtue Create()
		{
			if (TestMode.IsOn)
			{
				return null;
			}
			return PreloadManager.Cache.GetScene(NObtainPotionFtue._scenePath).Instantiate<NObtainPotionFtue>(0L);
		}

		// Token: 0x060038E2 RID: 14562 RVA: 0x001AE387 File Offset: 0x001AC587
		[NullableContext(1)]
		private void CloseFtue(NButton _)
		{
			NRun.Instance.GlobalUi.TopBar.PotionContainer.ZIndex = this._defaultZIndex;
			base.CloseFtue();
		}

		/// <summary>
		/// Get the method information for all the methods declared in this class.
		/// This method is used by Godot to register the available methods in the editor.
		/// Do not call this method.
		/// </summary>
		// Token: 0x060038E3 RID: 14563 RVA: 0x001AE3B0 File Offset: 0x001AC5B0
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal new static List<MethodInfo> GetGodotMethodList()
		{
			return new List<MethodInfo>(3)
			{
				new MethodInfo(NObtainPotionFtue.MethodName._Ready, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NObtainPotionFtue.MethodName.Create, new PropertyInfo(24L, "", 0L, "", 6L, new StringName("Control"), false), 33L, null, null),
				new MethodInfo(NObtainPotionFtue.MethodName.CloseFtue, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, new List<PropertyInfo>
				{
					new PropertyInfo(24L, "_", 0L, "", 6L, new StringName("Control"), false)
				}, null)
			};
		}

		/// <inheritdoc />
		// Token: 0x060038E4 RID: 14564 RVA: 0x001AE490 File Offset: 0x001AC690
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool InvokeGodotClassMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
		{
			if ((ref method) == NObtainPotionFtue.MethodName._Ready && args.Count == 0)
			{
				this._Ready();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NObtainPotionFtue.MethodName.Create && args.Count == 0)
			{
				NObtainPotionFtue nobtainPotionFtue = NObtainPotionFtue.Create();
				ret = VariantUtils.CreateFrom<NObtainPotionFtue>(ref nobtainPotionFtue);
				return true;
			}
			if ((ref method) == NObtainPotionFtue.MethodName.CloseFtue && args.Count == 1)
			{
				this.CloseFtue(VariantUtils.ConvertTo<NButton>(args[0]));
				ret = default(godot_variant);
				return true;
			}
			return base.InvokeGodotClassMethod(in method, args, out ret);
		}

		// Token: 0x060038E5 RID: 14565 RVA: 0x001AE52C File Offset: 0x001AC72C
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal static bool InvokeGodotClassStaticMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
		{
			if ((ref method) == NObtainPotionFtue.MethodName.Create && args.Count == 0)
			{
				NObtainPotionFtue nobtainPotionFtue = NObtainPotionFtue.Create();
				ret = VariantUtils.CreateFrom<NObtainPotionFtue>(ref nobtainPotionFtue);
				return true;
			}
			ret = default(godot_variant);
			return false;
		}

		/// <inheritdoc />
		// Token: 0x060038E6 RID: 14566 RVA: 0x001AE56C File Offset: 0x001AC76C
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool HasGodotClassMethod(in godot_string_name method)
		{
			return (ref method) == NObtainPotionFtue.MethodName._Ready || (ref method) == NObtainPotionFtue.MethodName.Create || (ref method) == NObtainPotionFtue.MethodName.CloseFtue || base.HasGodotClassMethod(in method);
		}

		/// <inheritdoc />
		// Token: 0x060038E7 RID: 14567 RVA: 0x001AE5A4 File Offset: 0x001AC7A4
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool SetGodotClassPropertyValue(in godot_string_name name, in godot_variant value)
		{
			if ((ref name) == NObtainPotionFtue.PropertyName._confirmButton)
			{
				this._confirmButton = VariantUtils.ConvertTo<NButton>(ref value);
				return true;
			}
			if ((ref name) == NObtainPotionFtue.PropertyName._header)
			{
				this._header = VariantUtils.ConvertTo<MegaLabel>(ref value);
				return true;
			}
			if ((ref name) == NObtainPotionFtue.PropertyName._description)
			{
				this._description = VariantUtils.ConvertTo<MegaRichTextLabel>(ref value);
				return true;
			}
			if ((ref name) == NObtainPotionFtue.PropertyName._defaultZIndex)
			{
				this._defaultZIndex = VariantUtils.ConvertTo<int>(ref value);
				return true;
			}
			return base.SetGodotClassPropertyValue(ref name, ref value);
		}

		/// <inheritdoc />
		// Token: 0x060038E8 RID: 14568 RVA: 0x001AE628 File Offset: 0x001AC828
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
		{
			if ((ref name) == NObtainPotionFtue.PropertyName._confirmButton)
			{
				value = VariantUtils.CreateFrom<NButton>(ref this._confirmButton);
				return true;
			}
			if ((ref name) == NObtainPotionFtue.PropertyName._header)
			{
				value = VariantUtils.CreateFrom<MegaLabel>(ref this._header);
				return true;
			}
			if ((ref name) == NObtainPotionFtue.PropertyName._description)
			{
				value = VariantUtils.CreateFrom<MegaRichTextLabel>(ref this._description);
				return true;
			}
			if ((ref name) == NObtainPotionFtue.PropertyName._defaultZIndex)
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
		// Token: 0x060038E9 RID: 14569 RVA: 0x001AE6C0 File Offset: 0x001AC8C0
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal new static List<PropertyInfo> GetGodotPropertyList()
		{
			return new List<PropertyInfo>
			{
				new PropertyInfo(24L, NObtainPotionFtue.PropertyName._confirmButton, 0L, "", 4096L, false),
				new PropertyInfo(24L, NObtainPotionFtue.PropertyName._header, 0L, "", 4096L, false),
				new PropertyInfo(24L, NObtainPotionFtue.PropertyName._description, 0L, "", 4096L, false),
				new PropertyInfo(2L, NObtainPotionFtue.PropertyName._defaultZIndex, 0L, "", 4096L, false)
			};
		}

		/// <inheritdoc />
		// Token: 0x060038EA RID: 14570 RVA: 0x001AE758 File Offset: 0x001AC958
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void SaveGodotObjectData(GodotSerializationInfo info)
		{
			base.SaveGodotObjectData(info);
			info.AddProperty(NObtainPotionFtue.PropertyName._confirmButton, Variant.From<NButton>(ref this._confirmButton));
			info.AddProperty(NObtainPotionFtue.PropertyName._header, Variant.From<MegaLabel>(ref this._header));
			info.AddProperty(NObtainPotionFtue.PropertyName._description, Variant.From<MegaRichTextLabel>(ref this._description));
			info.AddProperty(NObtainPotionFtue.PropertyName._defaultZIndex, Variant.From<int>(ref this._defaultZIndex));
		}

		/// <inheritdoc />
		// Token: 0x060038EB RID: 14571 RVA: 0x001AE7C4 File Offset: 0x001AC9C4
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void RestoreGodotObjectData(GodotSerializationInfo info)
		{
			base.RestoreGodotObjectData(info);
			Variant variant;
			if (info.TryGetProperty(NObtainPotionFtue.PropertyName._confirmButton, ref variant))
			{
				this._confirmButton = variant.As<NButton>();
			}
			Variant variant2;
			if (info.TryGetProperty(NObtainPotionFtue.PropertyName._header, ref variant2))
			{
				this._header = variant2.As<MegaLabel>();
			}
			Variant variant3;
			if (info.TryGetProperty(NObtainPotionFtue.PropertyName._description, ref variant3))
			{
				this._description = variant3.As<MegaRichTextLabel>();
			}
			Variant variant4;
			if (info.TryGetProperty(NObtainPotionFtue.PropertyName._defaultZIndex, ref variant4))
			{
				this._defaultZIndex = variant4.As<int>();
			}
		}

		// Token: 0x04001611 RID: 5649
		[Nullable(1)]
		public const string id = "obtain_potion_ftue";

		// Token: 0x04001612 RID: 5650
		[Nullable(1)]
		private static readonly string _scenePath = SceneHelper.GetScenePath("ftue/obtain_potion_ftue");

		// Token: 0x04001613 RID: 5651
		[Nullable(1)]
		private NButton _confirmButton;

		// Token: 0x04001614 RID: 5652
		[Nullable(1)]
		private MegaLabel _header;

		// Token: 0x04001615 RID: 5653
		[Nullable(1)]
		private MegaRichTextLabel _description;

		// Token: 0x04001616 RID: 5654
		private int _defaultZIndex;

		/// <summary>
		/// Cached StringNames for the methods contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x020017AF RID: 6063
		public new class MethodName : NFtue.MethodName
		{
			/// <summary>
			/// Cached name for the '_Ready' method.
			/// </summary>
			// Token: 0x04005C48 RID: 23624
			public static readonly StringName _Ready = "_Ready";

			/// <summary>
			/// Cached name for the 'Create' method.
			/// </summary>
			// Token: 0x04005C49 RID: 23625
			public static readonly StringName Create = "Create";

			/// <summary>
			/// Cached name for the 'CloseFtue' method.
			/// </summary>
			// Token: 0x04005C4A RID: 23626
			public new static readonly StringName CloseFtue = "CloseFtue";
		}

		/// <summary>
		/// Cached StringNames for the properties and fields contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x020017B0 RID: 6064
		public new class PropertyName : NFtue.PropertyName
		{
			/// <summary>
			/// Cached name for the '_confirmButton' field.
			/// </summary>
			// Token: 0x04005C4B RID: 23627
			public static readonly StringName _confirmButton = "_confirmButton";

			/// <summary>
			/// Cached name for the '_header' field.
			/// </summary>
			// Token: 0x04005C4C RID: 23628
			public static readonly StringName _header = "_header";

			/// <summary>
			/// Cached name for the '_description' field.
			/// </summary>
			// Token: 0x04005C4D RID: 23629
			public static readonly StringName _description = "_description";

			/// <summary>
			/// Cached name for the '_defaultZIndex' field.
			/// </summary>
			// Token: 0x04005C4E RID: 23630
			public static readonly StringName _defaultZIndex = "_defaultZIndex";
		}

		/// <summary>
		/// Cached StringNames for the signals contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x020017B1 RID: 6065
		public new class SignalName : NFtue.SignalName
		{
		}
	}
}
