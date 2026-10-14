using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Godot;
using Godot.Bridge;
using Godot.NativeInterop;
using MegaCrit.Sts2.addons.mega_text;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.Localization;
using MegaCrit.Sts2.Core.Logging;
using MegaCrit.Sts2.Core.Nodes.CommonUi;
using MegaCrit.Sts2.Core.Nodes.GodotExtensions;
using MegaCrit.Sts2.Core.Nodes.Multiplayer;
using MegaCrit.Sts2.Core.Saves;

namespace MegaCrit.Sts2.Core.Nodes.Screens.ProfileScreen
{
	// Token: 0x020002D9 RID: 729
	[NullableContext(1)]
	[Nullable(0)]
	[ScriptPath("res://src/Core/Nodes/Screens/ProfileScreen/NDeleteProfileButton.cs")]
	public class NDeleteProfileButton : NButton
	{
		// Token: 0x060026E0 RID: 9952 RVA: 0x0010904C File Offset: 0x0010724C
		public override void _Ready()
		{
			this.ConnectSignals();
			this._label = base.GetNode<MegaLabel>("%MegaLabel");
			this._label.SetTextAutoSize(NDeleteProfileButton._buttonMesssage.GetFormattedText());
			this._icon = base.GetNode<TextureRect>("Icon");
			this._hsv = (ShaderMaterial)this._icon.Material;
		}

		// Token: 0x060026E1 RID: 9953 RVA: 0x001090B8 File Offset: 0x001072B8
		public void Initialize(NProfileScreen profileScreen, int profileId)
		{
			this._profileScreen = profileScreen;
			this._profileId = profileId;
			int? forceShowProfileAsDeleted = NProfileScreen.forceShowProfileAsDeleted;
			bool flag = !((forceShowProfileAsDeleted.GetValueOrDefault() == profileId) & (forceShowProfileAsDeleted != null)) && FileAccess.FileExists(UserDataPathProvider.GetProfileScopedPath(profileId, "saves/progress.save", null, null));
			base.Visible = flag;
		}

		// Token: 0x060026E2 RID: 9954 RVA: 0x0010911D File Offset: 0x0010731D
		protected override void OnRelease()
		{
			TaskHelper.RunSafely(this.ConfirmDeletion());
		}

		// Token: 0x060026E3 RID: 9955 RVA: 0x0010912C File Offset: 0x0010732C
		private async Task ConfirmDeletion()
		{
			NGenericPopup ngenericPopup = NGenericPopup.Create();
			NModalContainer.Instance.Add(ngenericPopup, true);
			NDeleteProfileButton._title.Add("Id", this._profileId);
			NDeleteProfileButton._description.Add("Id", this._profileId);
			bool flag = await ngenericPopup.WaitForConfirmation(NDeleteProfileButton._description, NDeleteProfileButton._title, new LocString("main_menu_ui", "PROFILE_SCREEN.DELETE_CONFIRM_POPUP.cancel"), new LocString("main_menu_ui", "PROFILE_SCREEN.DELETE_CONFIRM_POPUP.delete"));
			bool flag2 = flag;
			if (flag2)
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(49, 1);
				defaultInterpolatedStringHandler.AppendLiteral("Player clicked yes on confirm deletion popup for ");
				defaultInterpolatedStringHandler.AppendFormatted<int>(this._profileId);
				Log.Info(defaultInterpolatedStringHandler.ToStringAndClear(), 2);
				SaveManager.Instance.DeleteProfile(this._profileId);
				NProfileScreen.forceShowProfileAsDeleted = new int?(this._profileId);
				SaveManager.Instance.InitProgressData();
				SaveManager.Instance.InitPrefsData();
				if (this._profileId == SaveManager.Instance.CurrentProfileId)
				{
					NGame.Instance.ReloadMainMenu();
					Callable.From(new Action(NGame.Instance.MainMenu.OpenProfileScreen)).CallDeferred(Array.Empty<Variant>());
				}
				else
				{
					this._profileScreen.Refresh();
				}
			}
		}

		// Token: 0x060026E4 RID: 9956 RVA: 0x00109170 File Offset: 0x00107370
		protected override void OnFocus()
		{
			base.OnFocus();
			Tween tween = this._tween;
			if (tween != null)
			{
				tween.Kill();
			}
			this._tween = base.CreateTween().SetParallel(true);
			this._tween.TweenProperty(this, "scale", Vector2.One * 1.1f, 0.05);
			this._tween.TweenMethod(Callable.From<float>(new Action<float>(this.UpdateShaderV)), this._hsv.GetShaderParameter(NDeleteProfileButton._v), 1.4f, 0.05);
			this._tween.TweenProperty(this._label, "modulate:a", 1f, 0.2);
			this._tween.TweenProperty(this._label, "position:y", 78f, 0.2).SetEase(1L).SetTrans(7L)
				.From(48f);
		}

		// Token: 0x060026E5 RID: 9957 RVA: 0x00109298 File Offset: 0x00107498
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
			this._tween.TweenMethod(Callable.From<float>(new Action<float>(this.UpdateShaderV)), this._hsv.GetShaderParameter(NDeleteProfileButton._v), 1f, 0.5).SetEase(1L).SetTrans(5L);
			this._tween.TweenProperty(this._label, "modulate:a", 0f, 0.05);
		}

		// Token: 0x060026E6 RID: 9958 RVA: 0x00109383 File Offset: 0x00107583
		private void UpdateShaderV(float value)
		{
			this._hsv.SetShaderParameter(NDeleteProfileButton._v, value);
		}

		/// <summary>
		/// Get the method information for all the methods declared in this class.
		/// This method is used by Godot to register the available methods in the editor.
		/// Do not call this method.
		/// </summary>
		// Token: 0x060026E7 RID: 9959 RVA: 0x0010939C File Offset: 0x0010759C
		[NullableContext(0)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal new static List<MethodInfo> GetGodotMethodList()
		{
			return new List<MethodInfo>(6)
			{
				new MethodInfo(NDeleteProfileButton.MethodName._Ready, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NDeleteProfileButton.MethodName.Initialize, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, new List<PropertyInfo>
				{
					new PropertyInfo(24L, "profileScreen", 0L, "", 6L, new StringName("Control"), false),
					new PropertyInfo(2L, "profileId", 0L, "", 6L, false)
				}, null),
				new MethodInfo(NDeleteProfileButton.MethodName.OnRelease, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NDeleteProfileButton.MethodName.OnFocus, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NDeleteProfileButton.MethodName.OnUnfocus, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NDeleteProfileButton.MethodName.UpdateShaderV, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, new List<PropertyInfo>
				{
					new PropertyInfo(3L, "value", 0L, "", 6L, false)
				}, null)
			};
		}

		/// <inheritdoc />
		// Token: 0x060026E8 RID: 9960 RVA: 0x00109544 File Offset: 0x00107744
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool InvokeGodotClassMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
		{
			if ((ref method) == NDeleteProfileButton.MethodName._Ready && args.Count == 0)
			{
				this._Ready();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NDeleteProfileButton.MethodName.Initialize && args.Count == 2)
			{
				this.Initialize(VariantUtils.ConvertTo<NProfileScreen>(args[0]), VariantUtils.ConvertTo<int>(args[1]));
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NDeleteProfileButton.MethodName.OnRelease && args.Count == 0)
			{
				this.OnRelease();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NDeleteProfileButton.MethodName.OnFocus && args.Count == 0)
			{
				this.OnFocus();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NDeleteProfileButton.MethodName.OnUnfocus && args.Count == 0)
			{
				this.OnUnfocus();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NDeleteProfileButton.MethodName.UpdateShaderV && args.Count == 1)
			{
				this.UpdateShaderV(VariantUtils.ConvertTo<float>(args[0]));
				ret = default(godot_variant);
				return true;
			}
			return base.InvokeGodotClassMethod(in method, args, out ret);
		}

		/// <inheritdoc />
		// Token: 0x060026E9 RID: 9961 RVA: 0x00109664 File Offset: 0x00107864
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool HasGodotClassMethod(in godot_string_name method)
		{
			return (ref method) == NDeleteProfileButton.MethodName._Ready || (ref method) == NDeleteProfileButton.MethodName.Initialize || (ref method) == NDeleteProfileButton.MethodName.OnRelease || (ref method) == NDeleteProfileButton.MethodName.OnFocus || (ref method) == NDeleteProfileButton.MethodName.OnUnfocus || (ref method) == NDeleteProfileButton.MethodName.UpdateShaderV || base.HasGodotClassMethod(in method);
		}

		/// <inheritdoc />
		// Token: 0x060026EA RID: 9962 RVA: 0x001096D4 File Offset: 0x001078D4
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool SetGodotClassPropertyValue(in godot_string_name name, in godot_variant value)
		{
			if ((ref name) == NDeleteProfileButton.PropertyName._icon)
			{
				this._icon = VariantUtils.ConvertTo<TextureRect>(ref value);
				return true;
			}
			if ((ref name) == NDeleteProfileButton.PropertyName._label)
			{
				this._label = VariantUtils.ConvertTo<MegaLabel>(ref value);
				return true;
			}
			if ((ref name) == NDeleteProfileButton.PropertyName._hsv)
			{
				this._hsv = VariantUtils.ConvertTo<ShaderMaterial>(ref value);
				return true;
			}
			if ((ref name) == NDeleteProfileButton.PropertyName._tween)
			{
				this._tween = VariantUtils.ConvertTo<Tween>(ref value);
				return true;
			}
			if ((ref name) == NDeleteProfileButton.PropertyName._profileScreen)
			{
				this._profileScreen = VariantUtils.ConvertTo<NProfileScreen>(ref value);
				return true;
			}
			if ((ref name) == NDeleteProfileButton.PropertyName._profileId)
			{
				this._profileId = VariantUtils.ConvertTo<int>(ref value);
				return true;
			}
			return base.SetGodotClassPropertyValue(in name, in value);
		}

		/// <inheritdoc />
		// Token: 0x060026EB RID: 9963 RVA: 0x0010978C File Offset: 0x0010798C
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
		{
			if ((ref name) == NDeleteProfileButton.PropertyName._icon)
			{
				value = VariantUtils.CreateFrom<TextureRect>(ref this._icon);
				return true;
			}
			if ((ref name) == NDeleteProfileButton.PropertyName._label)
			{
				value = VariantUtils.CreateFrom<MegaLabel>(ref this._label);
				return true;
			}
			if ((ref name) == NDeleteProfileButton.PropertyName._hsv)
			{
				value = VariantUtils.CreateFrom<ShaderMaterial>(ref this._hsv);
				return true;
			}
			if ((ref name) == NDeleteProfileButton.PropertyName._tween)
			{
				value = VariantUtils.CreateFrom<Tween>(ref this._tween);
				return true;
			}
			if ((ref name) == NDeleteProfileButton.PropertyName._profileScreen)
			{
				value = VariantUtils.CreateFrom<NProfileScreen>(ref this._profileScreen);
				return true;
			}
			if ((ref name) == NDeleteProfileButton.PropertyName._profileId)
			{
				value = VariantUtils.CreateFrom<int>(ref this._profileId);
				return true;
			}
			return base.GetGodotClassPropertyValue(in name, out value);
		}

		/// <summary>
		/// Get the property information for all the properties declared in this class.
		/// This method is used by Godot to register the available properties in the editor.
		/// Do not call this method.
		/// </summary>
		// Token: 0x060026EC RID: 9964 RVA: 0x00109864 File Offset: 0x00107A64
		[NullableContext(0)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal new static List<PropertyInfo> GetGodotPropertyList()
		{
			return new List<PropertyInfo>
			{
				new PropertyInfo(24L, NDeleteProfileButton.PropertyName._icon, 0L, "", 4096L, false),
				new PropertyInfo(24L, NDeleteProfileButton.PropertyName._label, 0L, "", 4096L, false),
				new PropertyInfo(24L, NDeleteProfileButton.PropertyName._hsv, 0L, "", 4096L, false),
				new PropertyInfo(24L, NDeleteProfileButton.PropertyName._tween, 0L, "", 4096L, false),
				new PropertyInfo(24L, NDeleteProfileButton.PropertyName._profileScreen, 0L, "", 4096L, false),
				new PropertyInfo(2L, NDeleteProfileButton.PropertyName._profileId, 0L, "", 4096L, false)
			};
		}

		/// <inheritdoc />
		// Token: 0x060026ED RID: 9965 RVA: 0x00109940 File Offset: 0x00107B40
		[NullableContext(0)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void SaveGodotObjectData(GodotSerializationInfo info)
		{
			base.SaveGodotObjectData(info);
			info.AddProperty(NDeleteProfileButton.PropertyName._icon, Variant.From<TextureRect>(ref this._icon));
			info.AddProperty(NDeleteProfileButton.PropertyName._label, Variant.From<MegaLabel>(ref this._label));
			info.AddProperty(NDeleteProfileButton.PropertyName._hsv, Variant.From<ShaderMaterial>(ref this._hsv));
			info.AddProperty(NDeleteProfileButton.PropertyName._tween, Variant.From<Tween>(ref this._tween));
			info.AddProperty(NDeleteProfileButton.PropertyName._profileScreen, Variant.From<NProfileScreen>(ref this._profileScreen));
			info.AddProperty(NDeleteProfileButton.PropertyName._profileId, Variant.From<int>(ref this._profileId));
		}

		/// <inheritdoc />
		// Token: 0x060026EE RID: 9966 RVA: 0x001099D8 File Offset: 0x00107BD8
		[NullableContext(0)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void RestoreGodotObjectData(GodotSerializationInfo info)
		{
			base.RestoreGodotObjectData(info);
			Variant variant;
			if (info.TryGetProperty(NDeleteProfileButton.PropertyName._icon, ref variant))
			{
				this._icon = variant.As<TextureRect>();
			}
			Variant variant2;
			if (info.TryGetProperty(NDeleteProfileButton.PropertyName._label, ref variant2))
			{
				this._label = variant2.As<MegaLabel>();
			}
			Variant variant3;
			if (info.TryGetProperty(NDeleteProfileButton.PropertyName._hsv, ref variant3))
			{
				this._hsv = variant3.As<ShaderMaterial>();
			}
			Variant variant4;
			if (info.TryGetProperty(NDeleteProfileButton.PropertyName._tween, ref variant4))
			{
				this._tween = variant4.As<Tween>();
			}
			Variant variant5;
			if (info.TryGetProperty(NDeleteProfileButton.PropertyName._profileScreen, ref variant5))
			{
				this._profileScreen = variant5.As<NProfileScreen>();
			}
			Variant variant6;
			if (info.TryGetProperty(NDeleteProfileButton.PropertyName._profileId, ref variant6))
			{
				this._profileId = variant6.As<int>();
			}
		}

		// Token: 0x04000E76 RID: 3702
		private static readonly StringName _v = new StringName("v");

		// Token: 0x04000E77 RID: 3703
		private static readonly LocString _title = new LocString("main_menu_ui", "PROFILE_SCREEN.DELETE_CONFIRM_POPUP.title");

		// Token: 0x04000E78 RID: 3704
		private static readonly LocString _description = new LocString("main_menu_ui", "PROFILE_SCREEN.DELETE_CONFIRM_POPUP.description");

		// Token: 0x04000E79 RID: 3705
		private static readonly LocString _buttonMesssage = new LocString("main_menu_ui", "PROFILE_SCREEN.DELETE_BUTTON.label");

		// Token: 0x04000E7A RID: 3706
		private TextureRect _icon;

		// Token: 0x04000E7B RID: 3707
		private MegaLabel _label;

		// Token: 0x04000E7C RID: 3708
		private ShaderMaterial _hsv;

		// Token: 0x04000E7D RID: 3709
		[Nullable(2)]
		private Tween _tween;

		// Token: 0x04000E7E RID: 3710
		private NProfileScreen _profileScreen;

		// Token: 0x04000E7F RID: 3711
		private int _profileId;

		/// <summary>
		/// Cached StringNames for the methods contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x0200145A RID: 5210
		[NullableContext(0)]
		public new class MethodName : NButton.MethodName
		{
			/// <summary>
			/// Cached name for the '_Ready' method.
			/// </summary>
			// Token: 0x04004D45 RID: 19781
			public new static readonly StringName _Ready = "_Ready";

			/// <summary>
			/// Cached name for the 'Initialize' method.
			/// </summary>
			// Token: 0x04004D46 RID: 19782
			public static readonly StringName Initialize = "Initialize";

			/// <summary>
			/// Cached name for the 'OnRelease' method.
			/// </summary>
			// Token: 0x04004D47 RID: 19783
			public new static readonly StringName OnRelease = "OnRelease";

			/// <summary>
			/// Cached name for the 'OnFocus' method.
			/// </summary>
			// Token: 0x04004D48 RID: 19784
			public new static readonly StringName OnFocus = "OnFocus";

			/// <summary>
			/// Cached name for the 'OnUnfocus' method.
			/// </summary>
			// Token: 0x04004D49 RID: 19785
			public new static readonly StringName OnUnfocus = "OnUnfocus";

			/// <summary>
			/// Cached name for the 'UpdateShaderV' method.
			/// </summary>
			// Token: 0x04004D4A RID: 19786
			public static readonly StringName UpdateShaderV = "UpdateShaderV";
		}

		/// <summary>
		/// Cached StringNames for the properties and fields contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x0200145B RID: 5211
		[NullableContext(0)]
		public new class PropertyName : NButton.PropertyName
		{
			/// <summary>
			/// Cached name for the '_icon' field.
			/// </summary>
			// Token: 0x04004D4B RID: 19787
			public static readonly StringName _icon = "_icon";

			/// <summary>
			/// Cached name for the '_label' field.
			/// </summary>
			// Token: 0x04004D4C RID: 19788
			public static readonly StringName _label = "_label";

			/// <summary>
			/// Cached name for the '_hsv' field.
			/// </summary>
			// Token: 0x04004D4D RID: 19789
			public static readonly StringName _hsv = "_hsv";

			/// <summary>
			/// Cached name for the '_tween' field.
			/// </summary>
			// Token: 0x04004D4E RID: 19790
			public static readonly StringName _tween = "_tween";

			/// <summary>
			/// Cached name for the '_profileScreen' field.
			/// </summary>
			// Token: 0x04004D4F RID: 19791
			public static readonly StringName _profileScreen = "_profileScreen";

			/// <summary>
			/// Cached name for the '_profileId' field.
			/// </summary>
			// Token: 0x04004D50 RID: 19792
			public static readonly StringName _profileId = "_profileId";
		}

		/// <summary>
		/// Cached StringNames for the signals contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x0200145C RID: 5212
		[NullableContext(0)]
		public new class SignalName : NButton.SignalName
		{
		}
	}
}
