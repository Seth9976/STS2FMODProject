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
using MegaCrit.Sts2.Core.Nodes.GodotExtensions;
using MegaCrit.Sts2.Core.Platform;

namespace MegaCrit.Sts2.Core.Nodes.Screens.MainMenu
{
	// Token: 0x02000302 RID: 770
	[ScriptPath("res://src/Core/Nodes/Screens/MainMenu/NJoinFriendButton.cs")]
	public class NJoinFriendButton : NButton
	{
		// Token: 0x17000739 RID: 1849
		// (get) Token: 0x06002A2F RID: 10799 RVA: 0x00126E94 File Offset: 0x00125094
		// (set) Token: 0x06002A30 RID: 10800 RVA: 0x00126E9C File Offset: 0x0012509C
		public ulong PlayerId { get; private set; }

		// Token: 0x06002A31 RID: 10801 RVA: 0x00126EA8 File Offset: 0x001250A8
		[NullableContext(1)]
		public static NJoinFriendButton Create(ulong playerId)
		{
			PackedScene scene = PreloadManager.Cache.GetScene(NJoinFriendButton.scenePath);
			NJoinFriendButton njoinFriendButton = scene.Instantiate<NJoinFriendButton>(0L);
			njoinFriendButton.PlayerId = playerId;
			return njoinFriendButton;
		}

		// Token: 0x06002A32 RID: 10802 RVA: 0x00126ED8 File Offset: 0x001250D8
		public override void _Ready()
		{
			this.ConnectSignals();
			MegaRichTextLabel node = base.GetNode<MegaRichTextLabel>("%Text");
			NinePatchRect node2 = base.GetNode<NinePatchRect>("Image");
			node.Text = "[center]" + PlatformUtil.GetPlayerName(PlatformUtil.PrimaryPlatform, this.PlayerId) + "[/center]";
			this._hsv = (ShaderMaterial)node2.Material;
		}

		// Token: 0x06002A33 RID: 10803 RVA: 0x00126F43 File Offset: 0x00125143
		protected override void OnPress()
		{
			base.OnPress();
			base.Scale = NJoinFriendButton._pressScale;
		}

		// Token: 0x06002A34 RID: 10804 RVA: 0x00126F56 File Offset: 0x00125156
		protected override void OnRelease()
		{
			base.OnRelease();
			base.Scale = Vector2.One;
		}

		// Token: 0x06002A35 RID: 10805 RVA: 0x00126F69 File Offset: 0x00125169
		protected override void OnFocus()
		{
			base.OnFocus();
			Tween tween = this._tween;
			if (tween != null)
			{
				tween.Kill();
			}
			base.Scale = NJoinFriendButton._hoverScale;
			this._hsv.SetShaderParameter(NJoinFriendButton._v, 1.2f);
		}

		// Token: 0x06002A36 RID: 10806 RVA: 0x00126FA8 File Offset: 0x001251A8
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

		// Token: 0x06002A37 RID: 10807 RVA: 0x0012705E File Offset: 0x0012525E
		private void UpdateShaderParam(float newV)
		{
			this._hsv.SetShaderParameter(NJoinFriendButton._v, newV);
		}

		/// <summary>
		/// Get the method information for all the methods declared in this class.
		/// This method is used by Godot to register the available methods in the editor.
		/// Do not call this method.
		/// </summary>
		// Token: 0x06002A38 RID: 10808 RVA: 0x00127078 File Offset: 0x00125278
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal new static List<MethodInfo> GetGodotMethodList()
		{
			return new List<MethodInfo>(7)
			{
				new MethodInfo(NJoinFriendButton.MethodName.Create, new PropertyInfo(24L, "", 0L, "", 6L, new StringName("Control"), false), 33L, new List<PropertyInfo>
				{
					new PropertyInfo(2L, "playerId", 0L, "", 6L, false)
				}, null),
				new MethodInfo(NJoinFriendButton.MethodName._Ready, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NJoinFriendButton.MethodName.OnPress, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NJoinFriendButton.MethodName.OnRelease, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NJoinFriendButton.MethodName.OnFocus, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NJoinFriendButton.MethodName.OnUnfocus, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NJoinFriendButton.MethodName.UpdateShaderParam, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, new List<PropertyInfo>
				{
					new PropertyInfo(3L, "newV", 0L, "", 6L, false)
				}, null)
			};
		}

		/// <inheritdoc />
		// Token: 0x06002A39 RID: 10809 RVA: 0x0012722C File Offset: 0x0012542C
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool InvokeGodotClassMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
		{
			if ((ref method) == NJoinFriendButton.MethodName.Create && args.Count == 1)
			{
				NJoinFriendButton njoinFriendButton = NJoinFriendButton.Create(VariantUtils.ConvertTo<ulong>(args[0]));
				ret = VariantUtils.CreateFrom<NJoinFriendButton>(ref njoinFriendButton);
				return true;
			}
			if ((ref method) == NJoinFriendButton.MethodName._Ready && args.Count == 0)
			{
				this._Ready();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NJoinFriendButton.MethodName.OnPress && args.Count == 0)
			{
				this.OnPress();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NJoinFriendButton.MethodName.OnRelease && args.Count == 0)
			{
				this.OnRelease();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NJoinFriendButton.MethodName.OnFocus && args.Count == 0)
			{
				this.OnFocus();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NJoinFriendButton.MethodName.OnUnfocus && args.Count == 0)
			{
				this.OnUnfocus();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NJoinFriendButton.MethodName.UpdateShaderParam && args.Count == 1)
			{
				this.UpdateShaderParam(VariantUtils.ConvertTo<float>(args[0]));
				ret = default(godot_variant);
				return true;
			}
			return base.InvokeGodotClassMethod(in method, args, out ret);
		}

		// Token: 0x06002A3A RID: 10810 RVA: 0x00127368 File Offset: 0x00125568
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal static bool InvokeGodotClassStaticMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
		{
			if ((ref method) == NJoinFriendButton.MethodName.Create && args.Count == 1)
			{
				NJoinFriendButton njoinFriendButton = NJoinFriendButton.Create(VariantUtils.ConvertTo<ulong>(args[0]));
				ret = VariantUtils.CreateFrom<NJoinFriendButton>(ref njoinFriendButton);
				return true;
			}
			ret = default(godot_variant);
			return false;
		}

		/// <inheritdoc />
		// Token: 0x06002A3B RID: 10811 RVA: 0x001273B8 File Offset: 0x001255B8
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool HasGodotClassMethod(in godot_string_name method)
		{
			return (ref method) == NJoinFriendButton.MethodName.Create || (ref method) == NJoinFriendButton.MethodName._Ready || (ref method) == NJoinFriendButton.MethodName.OnPress || (ref method) == NJoinFriendButton.MethodName.OnRelease || (ref method) == NJoinFriendButton.MethodName.OnFocus || (ref method) == NJoinFriendButton.MethodName.OnUnfocus || (ref method) == NJoinFriendButton.MethodName.UpdateShaderParam || base.HasGodotClassMethod(in method);
		}

		/// <inheritdoc />
		// Token: 0x06002A3C RID: 10812 RVA: 0x00127438 File Offset: 0x00125638
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool SetGodotClassPropertyValue(in godot_string_name name, in godot_variant value)
		{
			if ((ref name) == NJoinFriendButton.PropertyName.PlayerId)
			{
				this.PlayerId = VariantUtils.ConvertTo<ulong>(ref value);
				return true;
			}
			if ((ref name) == NJoinFriendButton.PropertyName._tween)
			{
				this._tween = VariantUtils.ConvertTo<Tween>(ref value);
				return true;
			}
			if ((ref name) == NJoinFriendButton.PropertyName._hsv)
			{
				this._hsv = VariantUtils.ConvertTo<ShaderMaterial>(ref value);
				return true;
			}
			return base.SetGodotClassPropertyValue(in name, in value);
		}

		/// <inheritdoc />
		// Token: 0x06002A3D RID: 10813 RVA: 0x001274A0 File Offset: 0x001256A0
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
		{
			if ((ref name) == NJoinFriendButton.PropertyName.PlayerId)
			{
				ulong playerId = this.PlayerId;
				value = VariantUtils.CreateFrom<ulong>(ref playerId);
				return true;
			}
			if ((ref name) == NJoinFriendButton.PropertyName._tween)
			{
				value = VariantUtils.CreateFrom<Tween>(ref this._tween);
				return true;
			}
			if ((ref name) == NJoinFriendButton.PropertyName._hsv)
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
		// Token: 0x06002A3E RID: 10814 RVA: 0x00127518 File Offset: 0x00125718
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal new static List<PropertyInfo> GetGodotPropertyList()
		{
			return new List<PropertyInfo>
			{
				new PropertyInfo(24L, NJoinFriendButton.PropertyName._tween, 0L, "", 4096L, false),
				new PropertyInfo(24L, NJoinFriendButton.PropertyName._hsv, 0L, "", 4096L, false),
				new PropertyInfo(2L, NJoinFriendButton.PropertyName.PlayerId, 0L, "", 4096L, false)
			};
		}

		/// <inheritdoc />
		// Token: 0x06002A3F RID: 10815 RVA: 0x00127590 File Offset: 0x00125790
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void SaveGodotObjectData(GodotSerializationInfo info)
		{
			base.SaveGodotObjectData(info);
			StringName playerId = NJoinFriendButton.PropertyName.PlayerId;
			ulong playerId2 = this.PlayerId;
			info.AddProperty(playerId, Variant.From<ulong>(ref playerId2));
			info.AddProperty(NJoinFriendButton.PropertyName._tween, Variant.From<Tween>(ref this._tween));
			info.AddProperty(NJoinFriendButton.PropertyName._hsv, Variant.From<ShaderMaterial>(ref this._hsv));
		}

		/// <inheritdoc />
		// Token: 0x06002A40 RID: 10816 RVA: 0x001275EC File Offset: 0x001257EC
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void RestoreGodotObjectData(GodotSerializationInfo info)
		{
			base.RestoreGodotObjectData(info);
			Variant variant;
			if (info.TryGetProperty(NJoinFriendButton.PropertyName.PlayerId, ref variant))
			{
				this.PlayerId = variant.As<ulong>();
			}
			Variant variant2;
			if (info.TryGetProperty(NJoinFriendButton.PropertyName._tween, ref variant2))
			{
				this._tween = variant2.As<Tween>();
			}
			Variant variant3;
			if (info.TryGetProperty(NJoinFriendButton.PropertyName._hsv, ref variant3))
			{
				this._hsv = variant3.As<ShaderMaterial>();
			}
		}

		// Token: 0x04000FC0 RID: 4032
		[Nullable(1)]
		private static readonly StringName _v = new StringName("v");

		// Token: 0x04000FC1 RID: 4033
		[Nullable(1)]
		public static readonly string scenePath = SceneHelper.GetScenePath("ui/multiplayer/join_friend_button");

		// Token: 0x04000FC2 RID: 4034
		private static readonly Vector2 _hoverScale = Vector2.One * 1.01f;

		// Token: 0x04000FC3 RID: 4035
		private static readonly Vector2 _pressScale = Vector2.One * 0.99f;

		// Token: 0x04000FC4 RID: 4036
		private const float _defaultV = 0.9f;

		// Token: 0x04000FC5 RID: 4037
		private const float _hoverV = 1.2f;

		// Token: 0x04000FC6 RID: 4038
		[Nullable(2)]
		private Tween _tween;

		// Token: 0x04000FC7 RID: 4039
		[Nullable(1)]
		private ShaderMaterial _hsv;

		/// <summary>
		/// Cached StringNames for the methods contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x020014EE RID: 5358
		public new class MethodName : NButton.MethodName
		{
			/// <summary>
			/// Cached name for the 'Create' method.
			/// </summary>
			// Token: 0x04004FBD RID: 20413
			public static readonly StringName Create = "Create";

			/// <summary>
			/// Cached name for the '_Ready' method.
			/// </summary>
			// Token: 0x04004FBE RID: 20414
			public new static readonly StringName _Ready = "_Ready";

			/// <summary>
			/// Cached name for the 'OnPress' method.
			/// </summary>
			// Token: 0x04004FBF RID: 20415
			public new static readonly StringName OnPress = "OnPress";

			/// <summary>
			/// Cached name for the 'OnRelease' method.
			/// </summary>
			// Token: 0x04004FC0 RID: 20416
			public new static readonly StringName OnRelease = "OnRelease";

			/// <summary>
			/// Cached name for the 'OnFocus' method.
			/// </summary>
			// Token: 0x04004FC1 RID: 20417
			public new static readonly StringName OnFocus = "OnFocus";

			/// <summary>
			/// Cached name for the 'OnUnfocus' method.
			/// </summary>
			// Token: 0x04004FC2 RID: 20418
			public new static readonly StringName OnUnfocus = "OnUnfocus";

			/// <summary>
			/// Cached name for the 'UpdateShaderParam' method.
			/// </summary>
			// Token: 0x04004FC3 RID: 20419
			public static readonly StringName UpdateShaderParam = "UpdateShaderParam";
		}

		/// <summary>
		/// Cached StringNames for the properties and fields contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x020014EF RID: 5359
		public new class PropertyName : NButton.PropertyName
		{
			/// <summary>
			/// Cached name for the 'PlayerId' property.
			/// </summary>
			// Token: 0x04004FC4 RID: 20420
			public static readonly StringName PlayerId = "PlayerId";

			/// <summary>
			/// Cached name for the '_tween' field.
			/// </summary>
			// Token: 0x04004FC5 RID: 20421
			public static readonly StringName _tween = "_tween";

			/// <summary>
			/// Cached name for the '_hsv' field.
			/// </summary>
			// Token: 0x04004FC6 RID: 20422
			public static readonly StringName _hsv = "_hsv";
		}

		/// <summary>
		/// Cached StringNames for the signals contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x020014F0 RID: 5360
		public new class SignalName : NButton.SignalName
		{
		}
	}
}
