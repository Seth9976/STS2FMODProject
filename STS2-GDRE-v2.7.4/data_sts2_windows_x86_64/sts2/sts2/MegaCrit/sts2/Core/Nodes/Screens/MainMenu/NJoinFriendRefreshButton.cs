using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Godot;
using Godot.Bridge;
using Godot.NativeInterop;
using MegaCrit.Sts2.addons.mega_text;
using MegaCrit.Sts2.Core.ControllerInput;
using MegaCrit.Sts2.Core.Localization;
using MegaCrit.Sts2.Core.Nodes.GodotExtensions;

namespace MegaCrit.Sts2.Core.Nodes.Screens.MainMenu
{
	// Token: 0x02000303 RID: 771
	[ScriptPath("res://src/Core/Nodes/Screens/MainMenu/NJoinFriendRefreshButton.cs")]
	public class NJoinFriendRefreshButton : NButton
	{
		// Token: 0x1700073A RID: 1850
		// (get) Token: 0x06002A43 RID: 10819 RVA: 0x001276AF File Offset: 0x001258AF
		[Nullable(1)]
		protected override string[] Hotkeys
		{
			[NullableContext(1)]
			get
			{
				return new string[] { MegaInput.accept };
			}
		}

		// Token: 0x1700073B RID: 1851
		// (get) Token: 0x06002A44 RID: 10820 RVA: 0x001276C4 File Offset: 0x001258C4
		// (set) Token: 0x06002A45 RID: 10821 RVA: 0x001276CC File Offset: 0x001258CC
		public ulong PlayerId { get; private set; }

		// Token: 0x06002A46 RID: 10822 RVA: 0x001276D8 File Offset: 0x001258D8
		public override void _Ready()
		{
			this.ConnectSignals();
			MegaLabel node = base.GetNode<MegaLabel>("Label");
			node.SetTextAutoSize(new LocString("main_menu_ui", "JOIN_FRIENDS_MENU.refresh").GetFormattedText());
			this._hsv = (ShaderMaterial)base.Material;
		}

		// Token: 0x06002A47 RID: 10823 RVA: 0x00127727 File Offset: 0x00125927
		protected override void OnPress()
		{
			base.OnPress();
			base.Scale = NJoinFriendRefreshButton._pressScale;
		}

		// Token: 0x06002A48 RID: 10824 RVA: 0x0012773A File Offset: 0x0012593A
		protected override void OnRelease()
		{
			base.OnRelease();
			base.Scale = Vector2.One;
		}

		// Token: 0x06002A49 RID: 10825 RVA: 0x0012774D File Offset: 0x0012594D
		protected override void OnFocus()
		{
			base.OnFocus();
			Tween tween = this._tween;
			if (tween != null)
			{
				tween.Kill();
			}
			base.Scale = NJoinFriendRefreshButton._hoverScale;
			this._hsv.SetShaderParameter(NJoinFriendRefreshButton._v, 1.2f);
		}

		// Token: 0x06002A4A RID: 10826 RVA: 0x0012778C File Offset: 0x0012598C
		protected override void OnUnfocus()
		{
			base.OnUnfocus();
			Tween tween = this._tween;
			if (tween != null)
			{
				tween.Kill();
			}
			this._tween = base.CreateTween().SetParallel(true);
			this._tween.TweenProperty(this, "scale", Vector2.One, 0.5).SetEase(1L).SetTrans(5L);
			this._tween.TweenMethod(Callable.From<float>(new Action<float>(this.UpdateShaderParam)), 1.2f, 0.9f, 0.5).SetEase(1L).SetTrans(5L);
		}

		// Token: 0x06002A4B RID: 10827 RVA: 0x00127842 File Offset: 0x00125A42
		private void UpdateShaderParam(float newV)
		{
			this._hsv.SetShaderParameter(NJoinFriendRefreshButton._v, newV);
		}

		/// <summary>
		/// Get the method information for all the methods declared in this class.
		/// This method is used by Godot to register the available methods in the editor.
		/// Do not call this method.
		/// </summary>
		// Token: 0x06002A4C RID: 10828 RVA: 0x0012785C File Offset: 0x00125A5C
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal new static List<MethodInfo> GetGodotMethodList()
		{
			return new List<MethodInfo>(6)
			{
				new MethodInfo(NJoinFriendRefreshButton.MethodName._Ready, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NJoinFriendRefreshButton.MethodName.OnPress, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NJoinFriendRefreshButton.MethodName.OnRelease, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NJoinFriendRefreshButton.MethodName.OnFocus, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NJoinFriendRefreshButton.MethodName.OnUnfocus, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NJoinFriendRefreshButton.MethodName.UpdateShaderParam, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, new List<PropertyInfo>
				{
					new PropertyInfo(3L, "newV", 0L, "", 6L, false)
				}, null)
			};
		}

		/// <inheritdoc />
		// Token: 0x06002A4D RID: 10829 RVA: 0x001279B0 File Offset: 0x00125BB0
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool InvokeGodotClassMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
		{
			if ((ref method) == NJoinFriendRefreshButton.MethodName._Ready && args.Count == 0)
			{
				this._Ready();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NJoinFriendRefreshButton.MethodName.OnPress && args.Count == 0)
			{
				this.OnPress();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NJoinFriendRefreshButton.MethodName.OnRelease && args.Count == 0)
			{
				this.OnRelease();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NJoinFriendRefreshButton.MethodName.OnFocus && args.Count == 0)
			{
				this.OnFocus();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NJoinFriendRefreshButton.MethodName.OnUnfocus && args.Count == 0)
			{
				this.OnUnfocus();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NJoinFriendRefreshButton.MethodName.UpdateShaderParam && args.Count == 1)
			{
				this.UpdateShaderParam(VariantUtils.ConvertTo<float>(args[0]));
				ret = default(godot_variant);
				return true;
			}
			return base.InvokeGodotClassMethod(in method, args, out ret);
		}

		/// <inheritdoc />
		// Token: 0x06002A4E RID: 10830 RVA: 0x00127AB4 File Offset: 0x00125CB4
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool HasGodotClassMethod(in godot_string_name method)
		{
			return (ref method) == NJoinFriendRefreshButton.MethodName._Ready || (ref method) == NJoinFriendRefreshButton.MethodName.OnPress || (ref method) == NJoinFriendRefreshButton.MethodName.OnRelease || (ref method) == NJoinFriendRefreshButton.MethodName.OnFocus || (ref method) == NJoinFriendRefreshButton.MethodName.OnUnfocus || (ref method) == NJoinFriendRefreshButton.MethodName.UpdateShaderParam || base.HasGodotClassMethod(in method);
		}

		/// <inheritdoc />
		// Token: 0x06002A4F RID: 10831 RVA: 0x00127B24 File Offset: 0x00125D24
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool SetGodotClassPropertyValue(in godot_string_name name, in godot_variant value)
		{
			if ((ref name) == NJoinFriendRefreshButton.PropertyName.PlayerId)
			{
				this.PlayerId = VariantUtils.ConvertTo<ulong>(ref value);
				return true;
			}
			if ((ref name) == NJoinFriendRefreshButton.PropertyName._tween)
			{
				this._tween = VariantUtils.ConvertTo<Tween>(ref value);
				return true;
			}
			if ((ref name) == NJoinFriendRefreshButton.PropertyName._hsv)
			{
				this._hsv = VariantUtils.ConvertTo<ShaderMaterial>(ref value);
				return true;
			}
			return base.SetGodotClassPropertyValue(in name, in value);
		}

		/// <inheritdoc />
		// Token: 0x06002A50 RID: 10832 RVA: 0x00127B8C File Offset: 0x00125D8C
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
		{
			if ((ref name) == NJoinFriendRefreshButton.PropertyName.Hotkeys)
			{
				string[] hotkeys = this.Hotkeys;
				value = VariantUtils.CreateFrom<string[]>(ref hotkeys);
				return true;
			}
			if ((ref name) == NJoinFriendRefreshButton.PropertyName.PlayerId)
			{
				ulong playerId = this.PlayerId;
				value = VariantUtils.CreateFrom<ulong>(ref playerId);
				return true;
			}
			if ((ref name) == NJoinFriendRefreshButton.PropertyName._tween)
			{
				value = VariantUtils.CreateFrom<Tween>(ref this._tween);
				return true;
			}
			if ((ref name) == NJoinFriendRefreshButton.PropertyName._hsv)
			{
				value = VariantUtils.CreateFrom<ShaderMaterial>(ref this._hsv);
				return true;
			}
			return base.GetGodotClassPropertyValue(in name, out value);
		}

		/// <summary>
		/// Get the property information for all the properties declared in this class.
		/// This method is used by Godot to register the available properties in the editor.
		/// Do not call this method.
		/// </summary>
		// Token: 0x06002A51 RID: 10833 RVA: 0x00127C28 File Offset: 0x00125E28
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal new static List<PropertyInfo> GetGodotPropertyList()
		{
			return new List<PropertyInfo>
			{
				new PropertyInfo(24L, NJoinFriendRefreshButton.PropertyName._tween, 0L, "", 4096L, false),
				new PropertyInfo(24L, NJoinFriendRefreshButton.PropertyName._hsv, 0L, "", 4096L, false),
				new PropertyInfo(34L, NJoinFriendRefreshButton.PropertyName.Hotkeys, 0L, "", 4096L, false),
				new PropertyInfo(2L, NJoinFriendRefreshButton.PropertyName.PlayerId, 0L, "", 4096L, false)
			};
		}

		/// <inheritdoc />
		// Token: 0x06002A52 RID: 10834 RVA: 0x00127CC0 File Offset: 0x00125EC0
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void SaveGodotObjectData(GodotSerializationInfo info)
		{
			base.SaveGodotObjectData(info);
			StringName playerId = NJoinFriendRefreshButton.PropertyName.PlayerId;
			ulong playerId2 = this.PlayerId;
			info.AddProperty(playerId, Variant.From<ulong>(ref playerId2));
			info.AddProperty(NJoinFriendRefreshButton.PropertyName._tween, Variant.From<Tween>(ref this._tween));
			info.AddProperty(NJoinFriendRefreshButton.PropertyName._hsv, Variant.From<ShaderMaterial>(ref this._hsv));
		}

		/// <inheritdoc />
		// Token: 0x06002A53 RID: 10835 RVA: 0x00127D1C File Offset: 0x00125F1C
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void RestoreGodotObjectData(GodotSerializationInfo info)
		{
			base.RestoreGodotObjectData(info);
			Variant variant;
			if (info.TryGetProperty(NJoinFriendRefreshButton.PropertyName.PlayerId, ref variant))
			{
				this.PlayerId = variant.As<ulong>();
			}
			Variant variant2;
			if (info.TryGetProperty(NJoinFriendRefreshButton.PropertyName._tween, ref variant2))
			{
				this._tween = variant2.As<Tween>();
			}
			Variant variant3;
			if (info.TryGetProperty(NJoinFriendRefreshButton.PropertyName._hsv, ref variant3))
			{
				this._hsv = variant3.As<ShaderMaterial>();
			}
		}

		// Token: 0x04000FC9 RID: 4041
		[Nullable(1)]
		private static readonly StringName _v = new StringName("v");

		// Token: 0x04000FCA RID: 4042
		private static readonly Vector2 _hoverScale = Vector2.One * 1.01f;

		// Token: 0x04000FCB RID: 4043
		private static readonly Vector2 _pressScale = Vector2.One * 0.99f;

		// Token: 0x04000FCC RID: 4044
		private const float _defaultV = 0.9f;

		// Token: 0x04000FCD RID: 4045
		private const float _hoverV = 1.2f;

		// Token: 0x04000FCE RID: 4046
		[Nullable(2)]
		private Tween _tween;

		// Token: 0x04000FCF RID: 4047
		[Nullable(1)]
		private ShaderMaterial _hsv;

		/// <summary>
		/// Cached StringNames for the methods contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x020014F1 RID: 5361
		public new class MethodName : NButton.MethodName
		{
			/// <summary>
			/// Cached name for the '_Ready' method.
			/// </summary>
			// Token: 0x04004FC7 RID: 20423
			public new static readonly StringName _Ready = "_Ready";

			/// <summary>
			/// Cached name for the 'OnPress' method.
			/// </summary>
			// Token: 0x04004FC8 RID: 20424
			public new static readonly StringName OnPress = "OnPress";

			/// <summary>
			/// Cached name for the 'OnRelease' method.
			/// </summary>
			// Token: 0x04004FC9 RID: 20425
			public new static readonly StringName OnRelease = "OnRelease";

			/// <summary>
			/// Cached name for the 'OnFocus' method.
			/// </summary>
			// Token: 0x04004FCA RID: 20426
			public new static readonly StringName OnFocus = "OnFocus";

			/// <summary>
			/// Cached name for the 'OnUnfocus' method.
			/// </summary>
			// Token: 0x04004FCB RID: 20427
			public new static readonly StringName OnUnfocus = "OnUnfocus";

			/// <summary>
			/// Cached name for the 'UpdateShaderParam' method.
			/// </summary>
			// Token: 0x04004FCC RID: 20428
			public static readonly StringName UpdateShaderParam = "UpdateShaderParam";
		}

		/// <summary>
		/// Cached StringNames for the properties and fields contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x020014F2 RID: 5362
		public new class PropertyName : NButton.PropertyName
		{
			/// <summary>
			/// Cached name for the 'Hotkeys' property.
			/// </summary>
			// Token: 0x04004FCD RID: 20429
			public new static readonly StringName Hotkeys = "Hotkeys";

			/// <summary>
			/// Cached name for the 'PlayerId' property.
			/// </summary>
			// Token: 0x04004FCE RID: 20430
			public static readonly StringName PlayerId = "PlayerId";

			/// <summary>
			/// Cached name for the '_tween' field.
			/// </summary>
			// Token: 0x04004FCF RID: 20431
			public static readonly StringName _tween = "_tween";

			/// <summary>
			/// Cached name for the '_hsv' field.
			/// </summary>
			// Token: 0x04004FD0 RID: 20432
			public static readonly StringName _hsv = "_hsv";
		}

		/// <summary>
		/// Cached StringNames for the signals contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x020014F3 RID: 5363
		public new class SignalName : NButton.SignalName
		{
		}
	}
}
