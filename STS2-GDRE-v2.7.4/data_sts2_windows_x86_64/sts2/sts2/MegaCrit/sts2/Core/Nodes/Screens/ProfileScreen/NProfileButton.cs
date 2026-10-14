using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Runtime.CompilerServices;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Threading;
using System.Threading.Tasks;
using Godot;
using Godot.Bridge;
using Godot.NativeInterop;
using MegaCrit.Sts2.addons.mega_text;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.Localization;
using MegaCrit.Sts2.Core.Nodes.GodotExtensions;
using MegaCrit.Sts2.Core.Saves;

namespace MegaCrit.Sts2.Core.Nodes.Screens.ProfileScreen
{
	/// <summary>
	/// Button in the profile screen which allows the player to select a profile.
	/// </summary>
	// Token: 0x020002DA RID: 730
	[NullableContext(1)]
	[Nullable(0)]
	[ScriptPath("res://src/Core/Nodes/Screens/ProfileScreen/NProfileButton.cs")]
	public class NProfileButton : NButton
	{
		// Token: 0x170006E5 RID: 1765
		// (get) Token: 0x060026F1 RID: 9969 RVA: 0x00109AF4 File Offset: 0x00107CF4
		public static IEnumerable<string> AssetPaths
		{
			get
			{
				return NProfileIcon.AssetPaths;
			}
		}

		// Token: 0x060026F2 RID: 9970 RVA: 0x00109AFC File Offset: 0x00107CFC
		public override void _Ready()
		{
			this.ConnectSignals();
			this._background = base.GetNode<TextureRect>("%Background");
			this._hsv = (ShaderMaterial)this._background.Material;
			this._title = base.GetNode<MegaRichTextLabel>("%Title");
			this._description = base.GetNode<MegaRichTextLabel>("%Info");
			this._profileIcon = base.GetNode<NProfileIcon>("%ProfileIcon");
			this._currentProfileIndicator = base.GetNode<Control>("%CurrentProfileIndicator");
		}

		// Token: 0x060026F3 RID: 9971 RVA: 0x00109B94 File Offset: 0x00107D94
		public void Initialize(NProfileScreen profileScreen, int profileId)
		{
			this._profileScreen = profileScreen;
			this._profileId = profileId;
			LocString locString = new LocString("main_menu_ui", "PROFILE_SCREEN.BUTTON.title");
			locString.Add("Id", profileId);
			this._title.Text = locString.GetFormattedText();
			GodotFileIo godotFileIo = new GodotFileIo(UserDataPathProvider.GetProfileScopedPath(profileId, "saves", null, null));
			this._profileIcon.SetProfileId(profileId);
			this._currentProfileIndicator.Visible = SaveManager.Instance.CurrentProfileId == profileId;
			string text = "progress.save";
			int? forceShowProfileAsDeleted = NProfileScreen.forceShowProfileAsDeleted;
			if (((forceShowProfileAsDeleted.GetValueOrDefault() == profileId) & (forceShowProfileAsDeleted != null)) || !godotFileIo.FileExists(text))
			{
				LocString locString2 = new LocString("main_menu_ui", "PROFILE_SCREEN.BUTTON.empty");
				this._description.Text = locString2.GetFormattedText();
				return;
			}
			LocString locString3 = new LocString("main_menu_ui", "PROFILE_SCREEN.BUTTON.description");
			if (SaveManager.Instance.CurrentProfileId == profileId)
			{
				locString3.Add("Playtime", TimeFormatting.Format((float)SaveManager.Instance.Progress.TotalPlaytime));
			}
			else
			{
				string text2 = godotFileIo.ReadFile(text);
				JsonObject jsonObject = JsonSerializer.Deserialize<JsonObject>(text2, JsonSerializationUtility.GetTypeInfo<JsonObject>());
				JsonNode jsonNode;
				if (jsonObject.TryGetPropertyValue("total_playtime", out jsonNode))
				{
					JsonValue jsonValue = jsonNode as JsonValue;
					long num;
					if (jsonValue != null && jsonValue.TryGetValue<long>(out num))
					{
						locString3.Add("Playtime", TimeFormatting.Format((float)num));
						goto IL_0187;
					}
				}
				locString3.Add("Playtime", "???");
			}
			IL_0187:
			DateTimeOffset dateTimeOffset = godotFileIo.GetLastModifiedTime(text);
			string text3 = "current_run.save";
			if (godotFileIo.FileExists(text3))
			{
				DateTimeOffset lastModifiedTime = godotFileIo.GetLastModifiedTime(text3);
				dateTimeOffset = ((dateTimeOffset > lastModifiedTime) ? dateTimeOffset : lastModifiedTime);
			}
			string text4 = "current_run_mp.save";
			if (godotFileIo.FileExists(text4))
			{
				DateTimeOffset lastModifiedTime2 = godotFileIo.GetLastModifiedTime(text4);
				dateTimeOffset = ((dateTimeOffset > lastModifiedTime2) ? dateTimeOffset : lastModifiedTime2);
			}
			DateTimeFormatInfo dateTimeFormat = LocManager.Instance.CultureInfo.DateTimeFormat;
			DateTime dateTime = TimeZoneInfo.ConvertTimeFromUtc(dateTimeOffset.UtcDateTime, TimeZoneInfo.Local);
			LocString locString4 = new LocString("main_menu_ui", "PROFILE_SCREEN.BUTTON.dateFormat");
			string text5 = dateTime.ToString(locString4.GetFormattedText(), dateTimeFormat);
			locString3.Add("LastUpdatedTime", text5);
			this._description.Text = locString3.GetFormattedText();
		}

		// Token: 0x060026F4 RID: 9972 RVA: 0x00109DF4 File Offset: 0x00107FF4
		protected override void OnRelease()
		{
			if (SaveManager.Instance.CurrentProfileId == this._profileId)
			{
				NGame.Instance.MainMenu.SubmenuStack.Pop();
				return;
			}
			TaskHelper.RunSafely(this.SwitchToThisProfile());
		}

		// Token: 0x060026F5 RID: 9973 RVA: 0x00109E2C File Offset: 0x0010802C
		private async Task SwitchToThisProfile()
		{
			NProfileScreen profileScreen = this._profileScreen;
			if (profileScreen != null)
			{
				profileScreen.ShowLoading();
			}
			await this.AwaitProcessFrame(default(CancellationToken));
			await this.AwaitProcessFrame(default(CancellationToken));
			SaveManager.Instance.SwitchProfileId(this._profileId);
			ReadSaveResult<PrefsSave> readSaveResult = SaveManager.Instance.InitPrefsData();
			ReadSaveResult<SerializableProgress> readSaveResult2 = SaveManager.Instance.InitProgressData();
			NGame.Instance.ReloadMainMenu();
			NGame.Instance.CheckShowSaveFileError(readSaveResult2, readSaveResult, new ReadSaveResult<SettingsSave>(new SettingsSave()));
		}

		// Token: 0x060026F6 RID: 9974 RVA: 0x00109E70 File Offset: 0x00108070
		protected override void OnFocus()
		{
			base.OnFocus();
			Tween tween = this._tween;
			if (tween != null)
			{
				tween.Kill();
			}
			this._tween = base.CreateTween().SetParallel(true);
			this._tween.TweenProperty(this, "scale", Vector2.One * 1.03f, 0.05);
			this._tween.TweenMethod(Callable.From<float>(new Action<float>(this.UpdateShaderV)), this._hsv.GetShaderParameter(NProfileButton._v), 1.3f, 0.05);
		}

		// Token: 0x060026F7 RID: 9975 RVA: 0x00109F1C File Offset: 0x0010811C
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
			this._tween.TweenMethod(Callable.From<float>(new Action<float>(this.UpdateShaderV)), this._hsv.GetShaderParameter(NProfileButton._v), 1f, 0.5).SetEase(1L).SetTrans(5L);
		}

		// Token: 0x060026F8 RID: 9976 RVA: 0x00109FD8 File Offset: 0x001081D8
		private void UpdateShaderV(float value)
		{
			this._hsv.SetShaderParameter(NProfileButton._v, value);
		}

		/// <summary>
		/// Get the method information for all the methods declared in this class.
		/// This method is used by Godot to register the available methods in the editor.
		/// Do not call this method.
		/// </summary>
		// Token: 0x060026F9 RID: 9977 RVA: 0x00109FF0 File Offset: 0x001081F0
		[NullableContext(0)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal new static List<MethodInfo> GetGodotMethodList()
		{
			return new List<MethodInfo>(6)
			{
				new MethodInfo(NProfileButton.MethodName._Ready, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NProfileButton.MethodName.Initialize, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, new List<PropertyInfo>
				{
					new PropertyInfo(24L, "profileScreen", 0L, "", 6L, new StringName("Control"), false),
					new PropertyInfo(2L, "profileId", 0L, "", 6L, false)
				}, null),
				new MethodInfo(NProfileButton.MethodName.OnRelease, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NProfileButton.MethodName.OnFocus, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NProfileButton.MethodName.OnUnfocus, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NProfileButton.MethodName.UpdateShaderV, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, new List<PropertyInfo>
				{
					new PropertyInfo(3L, "value", 0L, "", 6L, false)
				}, null)
			};
		}

		/// <inheritdoc />
		// Token: 0x060026FA RID: 9978 RVA: 0x0010A198 File Offset: 0x00108398
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool InvokeGodotClassMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
		{
			if ((ref method) == NProfileButton.MethodName._Ready && args.Count == 0)
			{
				this._Ready();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NProfileButton.MethodName.Initialize && args.Count == 2)
			{
				this.Initialize(VariantUtils.ConvertTo<NProfileScreen>(args[0]), VariantUtils.ConvertTo<int>(args[1]));
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NProfileButton.MethodName.OnRelease && args.Count == 0)
			{
				this.OnRelease();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NProfileButton.MethodName.OnFocus && args.Count == 0)
			{
				this.OnFocus();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NProfileButton.MethodName.OnUnfocus && args.Count == 0)
			{
				this.OnUnfocus();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NProfileButton.MethodName.UpdateShaderV && args.Count == 1)
			{
				this.UpdateShaderV(VariantUtils.ConvertTo<float>(args[0]));
				ret = default(godot_variant);
				return true;
			}
			return base.InvokeGodotClassMethod(in method, args, out ret);
		}

		/// <inheritdoc />
		// Token: 0x060026FB RID: 9979 RVA: 0x0010A2B8 File Offset: 0x001084B8
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool HasGodotClassMethod(in godot_string_name method)
		{
			return (ref method) == NProfileButton.MethodName._Ready || (ref method) == NProfileButton.MethodName.Initialize || (ref method) == NProfileButton.MethodName.OnRelease || (ref method) == NProfileButton.MethodName.OnFocus || (ref method) == NProfileButton.MethodName.OnUnfocus || (ref method) == NProfileButton.MethodName.UpdateShaderV || base.HasGodotClassMethod(in method);
		}

		/// <inheritdoc />
		// Token: 0x060026FC RID: 9980 RVA: 0x0010A328 File Offset: 0x00108528
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool SetGodotClassPropertyValue(in godot_string_name name, in godot_variant value)
		{
			if ((ref name) == NProfileButton.PropertyName._title)
			{
				this._title = VariantUtils.ConvertTo<MegaRichTextLabel>(ref value);
				return true;
			}
			if ((ref name) == NProfileButton.PropertyName._description)
			{
				this._description = VariantUtils.ConvertTo<MegaRichTextLabel>(ref value);
				return true;
			}
			if ((ref name) == NProfileButton.PropertyName._currentProfileIndicator)
			{
				this._currentProfileIndicator = VariantUtils.ConvertTo<Control>(ref value);
				return true;
			}
			if ((ref name) == NProfileButton.PropertyName._profileIcon)
			{
				this._profileIcon = VariantUtils.ConvertTo<NProfileIcon>(ref value);
				return true;
			}
			if ((ref name) == NProfileButton.PropertyName._deleteButton)
			{
				this._deleteButton = VariantUtils.ConvertTo<NDeleteProfileButton>(ref value);
				return true;
			}
			if ((ref name) == NProfileButton.PropertyName._background)
			{
				this._background = VariantUtils.ConvertTo<TextureRect>(ref value);
				return true;
			}
			if ((ref name) == NProfileButton.PropertyName._hsv)
			{
				this._hsv = VariantUtils.ConvertTo<ShaderMaterial>(ref value);
				return true;
			}
			if ((ref name) == NProfileButton.PropertyName._profileScreen)
			{
				this._profileScreen = VariantUtils.ConvertTo<NProfileScreen>(ref value);
				return true;
			}
			if ((ref name) == NProfileButton.PropertyName._tween)
			{
				this._tween = VariantUtils.ConvertTo<Tween>(ref value);
				return true;
			}
			if ((ref name) == NProfileButton.PropertyName._profileId)
			{
				this._profileId = VariantUtils.ConvertTo<int>(ref value);
				return true;
			}
			return base.SetGodotClassPropertyValue(in name, in value);
		}

		/// <inheritdoc />
		// Token: 0x060026FD RID: 9981 RVA: 0x0010A44C File Offset: 0x0010864C
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
		{
			if ((ref name) == NProfileButton.PropertyName._title)
			{
				value = VariantUtils.CreateFrom<MegaRichTextLabel>(ref this._title);
				return true;
			}
			if ((ref name) == NProfileButton.PropertyName._description)
			{
				value = VariantUtils.CreateFrom<MegaRichTextLabel>(ref this._description);
				return true;
			}
			if ((ref name) == NProfileButton.PropertyName._currentProfileIndicator)
			{
				value = VariantUtils.CreateFrom<Control>(ref this._currentProfileIndicator);
				return true;
			}
			if ((ref name) == NProfileButton.PropertyName._profileIcon)
			{
				value = VariantUtils.CreateFrom<NProfileIcon>(ref this._profileIcon);
				return true;
			}
			if ((ref name) == NProfileButton.PropertyName._deleteButton)
			{
				value = VariantUtils.CreateFrom<NDeleteProfileButton>(ref this._deleteButton);
				return true;
			}
			if ((ref name) == NProfileButton.PropertyName._background)
			{
				value = VariantUtils.CreateFrom<TextureRect>(ref this._background);
				return true;
			}
			if ((ref name) == NProfileButton.PropertyName._hsv)
			{
				value = VariantUtils.CreateFrom<ShaderMaterial>(ref this._hsv);
				return true;
			}
			if ((ref name) == NProfileButton.PropertyName._profileScreen)
			{
				value = VariantUtils.CreateFrom<NProfileScreen>(ref this._profileScreen);
				return true;
			}
			if ((ref name) == NProfileButton.PropertyName._tween)
			{
				value = VariantUtils.CreateFrom<Tween>(ref this._tween);
				return true;
			}
			if ((ref name) == NProfileButton.PropertyName._profileId)
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
		// Token: 0x060026FE RID: 9982 RVA: 0x0010A5A4 File Offset: 0x001087A4
		[NullableContext(0)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal new static List<PropertyInfo> GetGodotPropertyList()
		{
			return new List<PropertyInfo>
			{
				new PropertyInfo(24L, NProfileButton.PropertyName._title, 0L, "", 4096L, false),
				new PropertyInfo(24L, NProfileButton.PropertyName._description, 0L, "", 4096L, false),
				new PropertyInfo(24L, NProfileButton.PropertyName._currentProfileIndicator, 0L, "", 4096L, false),
				new PropertyInfo(24L, NProfileButton.PropertyName._profileIcon, 0L, "", 4096L, false),
				new PropertyInfo(24L, NProfileButton.PropertyName._deleteButton, 0L, "", 4096L, false),
				new PropertyInfo(24L, NProfileButton.PropertyName._background, 0L, "", 4096L, false),
				new PropertyInfo(24L, NProfileButton.PropertyName._hsv, 0L, "", 4096L, false),
				new PropertyInfo(24L, NProfileButton.PropertyName._profileScreen, 0L, "", 4096L, false),
				new PropertyInfo(24L, NProfileButton.PropertyName._tween, 0L, "", 4096L, false),
				new PropertyInfo(2L, NProfileButton.PropertyName._profileId, 0L, "", 4096L, false)
			};
		}

		/// <inheritdoc />
		// Token: 0x060026FF RID: 9983 RVA: 0x0010A704 File Offset: 0x00108904
		[NullableContext(0)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void SaveGodotObjectData(GodotSerializationInfo info)
		{
			base.SaveGodotObjectData(info);
			info.AddProperty(NProfileButton.PropertyName._title, Variant.From<MegaRichTextLabel>(ref this._title));
			info.AddProperty(NProfileButton.PropertyName._description, Variant.From<MegaRichTextLabel>(ref this._description));
			info.AddProperty(NProfileButton.PropertyName._currentProfileIndicator, Variant.From<Control>(ref this._currentProfileIndicator));
			info.AddProperty(NProfileButton.PropertyName._profileIcon, Variant.From<NProfileIcon>(ref this._profileIcon));
			info.AddProperty(NProfileButton.PropertyName._deleteButton, Variant.From<NDeleteProfileButton>(ref this._deleteButton));
			info.AddProperty(NProfileButton.PropertyName._background, Variant.From<TextureRect>(ref this._background));
			info.AddProperty(NProfileButton.PropertyName._hsv, Variant.From<ShaderMaterial>(ref this._hsv));
			info.AddProperty(NProfileButton.PropertyName._profileScreen, Variant.From<NProfileScreen>(ref this._profileScreen));
			info.AddProperty(NProfileButton.PropertyName._tween, Variant.From<Tween>(ref this._tween));
			info.AddProperty(NProfileButton.PropertyName._profileId, Variant.From<int>(ref this._profileId));
		}

		/// <inheritdoc />
		// Token: 0x06002700 RID: 9984 RVA: 0x0010A7F4 File Offset: 0x001089F4
		[NullableContext(0)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void RestoreGodotObjectData(GodotSerializationInfo info)
		{
			base.RestoreGodotObjectData(info);
			Variant variant;
			if (info.TryGetProperty(NProfileButton.PropertyName._title, ref variant))
			{
				this._title = variant.As<MegaRichTextLabel>();
			}
			Variant variant2;
			if (info.TryGetProperty(NProfileButton.PropertyName._description, ref variant2))
			{
				this._description = variant2.As<MegaRichTextLabel>();
			}
			Variant variant3;
			if (info.TryGetProperty(NProfileButton.PropertyName._currentProfileIndicator, ref variant3))
			{
				this._currentProfileIndicator = variant3.As<Control>();
			}
			Variant variant4;
			if (info.TryGetProperty(NProfileButton.PropertyName._profileIcon, ref variant4))
			{
				this._profileIcon = variant4.As<NProfileIcon>();
			}
			Variant variant5;
			if (info.TryGetProperty(NProfileButton.PropertyName._deleteButton, ref variant5))
			{
				this._deleteButton = variant5.As<NDeleteProfileButton>();
			}
			Variant variant6;
			if (info.TryGetProperty(NProfileButton.PropertyName._background, ref variant6))
			{
				this._background = variant6.As<TextureRect>();
			}
			Variant variant7;
			if (info.TryGetProperty(NProfileButton.PropertyName._hsv, ref variant7))
			{
				this._hsv = variant7.As<ShaderMaterial>();
			}
			Variant variant8;
			if (info.TryGetProperty(NProfileButton.PropertyName._profileScreen, ref variant8))
			{
				this._profileScreen = variant8.As<NProfileScreen>();
			}
			Variant variant9;
			if (info.TryGetProperty(NProfileButton.PropertyName._tween, ref variant9))
			{
				this._tween = variant9.As<Tween>();
			}
			Variant variant10;
			if (info.TryGetProperty(NProfileButton.PropertyName._profileId, ref variant10))
			{
				this._profileId = variant10.As<int>();
			}
		}

		// Token: 0x04000E80 RID: 3712
		private static readonly StringName _v = new StringName("v");

		// Token: 0x04000E81 RID: 3713
		private MegaRichTextLabel _title;

		// Token: 0x04000E82 RID: 3714
		private MegaRichTextLabel _description;

		// Token: 0x04000E83 RID: 3715
		private Control _currentProfileIndicator;

		// Token: 0x04000E84 RID: 3716
		private NProfileIcon _profileIcon;

		// Token: 0x04000E85 RID: 3717
		private NDeleteProfileButton _deleteButton;

		// Token: 0x04000E86 RID: 3718
		private TextureRect _background;

		// Token: 0x04000E87 RID: 3719
		private ShaderMaterial _hsv;

		// Token: 0x04000E88 RID: 3720
		[Nullable(2)]
		private NProfileScreen _profileScreen;

		// Token: 0x04000E89 RID: 3721
		[Nullable(2)]
		private Tween _tween;

		// Token: 0x04000E8A RID: 3722
		private int _profileId;

		/// <summary>
		/// Cached StringNames for the methods contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x0200145E RID: 5214
		[NullableContext(0)]
		public new class MethodName : NButton.MethodName
		{
			/// <summary>
			/// Cached name for the '_Ready' method.
			/// </summary>
			// Token: 0x04004D55 RID: 19797
			public new static readonly StringName _Ready = "_Ready";

			/// <summary>
			/// Cached name for the 'Initialize' method.
			/// </summary>
			// Token: 0x04004D56 RID: 19798
			public static readonly StringName Initialize = "Initialize";

			/// <summary>
			/// Cached name for the 'OnRelease' method.
			/// </summary>
			// Token: 0x04004D57 RID: 19799
			public new static readonly StringName OnRelease = "OnRelease";

			/// <summary>
			/// Cached name for the 'OnFocus' method.
			/// </summary>
			// Token: 0x04004D58 RID: 19800
			public new static readonly StringName OnFocus = "OnFocus";

			/// <summary>
			/// Cached name for the 'OnUnfocus' method.
			/// </summary>
			// Token: 0x04004D59 RID: 19801
			public new static readonly StringName OnUnfocus = "OnUnfocus";

			/// <summary>
			/// Cached name for the 'UpdateShaderV' method.
			/// </summary>
			// Token: 0x04004D5A RID: 19802
			public static readonly StringName UpdateShaderV = "UpdateShaderV";
		}

		/// <summary>
		/// Cached StringNames for the properties and fields contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x0200145F RID: 5215
		[NullableContext(0)]
		public new class PropertyName : NButton.PropertyName
		{
			/// <summary>
			/// Cached name for the '_title' field.
			/// </summary>
			// Token: 0x04004D5B RID: 19803
			public static readonly StringName _title = "_title";

			/// <summary>
			/// Cached name for the '_description' field.
			/// </summary>
			// Token: 0x04004D5C RID: 19804
			public static readonly StringName _description = "_description";

			/// <summary>
			/// Cached name for the '_currentProfileIndicator' field.
			/// </summary>
			// Token: 0x04004D5D RID: 19805
			public static readonly StringName _currentProfileIndicator = "_currentProfileIndicator";

			/// <summary>
			/// Cached name for the '_profileIcon' field.
			/// </summary>
			// Token: 0x04004D5E RID: 19806
			public static readonly StringName _profileIcon = "_profileIcon";

			/// <summary>
			/// Cached name for the '_deleteButton' field.
			/// </summary>
			// Token: 0x04004D5F RID: 19807
			public static readonly StringName _deleteButton = "_deleteButton";

			/// <summary>
			/// Cached name for the '_background' field.
			/// </summary>
			// Token: 0x04004D60 RID: 19808
			public static readonly StringName _background = "_background";

			/// <summary>
			/// Cached name for the '_hsv' field.
			/// </summary>
			// Token: 0x04004D61 RID: 19809
			public static readonly StringName _hsv = "_hsv";

			/// <summary>
			/// Cached name for the '_profileScreen' field.
			/// </summary>
			// Token: 0x04004D62 RID: 19810
			public static readonly StringName _profileScreen = "_profileScreen";

			/// <summary>
			/// Cached name for the '_tween' field.
			/// </summary>
			// Token: 0x04004D63 RID: 19811
			public static readonly StringName _tween = "_tween";

			/// <summary>
			/// Cached name for the '_profileId' field.
			/// </summary>
			// Token: 0x04004D64 RID: 19812
			public static readonly StringName _profileId = "_profileId";
		}

		/// <summary>
		/// Cached StringNames for the signals contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x02001460 RID: 5216
		[NullableContext(0)]
		public new class SignalName : NButton.SignalName
		{
		}
	}
}
