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
using MegaCrit.Sts2.Core.Nodes.Screens.MainMenu;
using MegaCrit.Sts2.Core.Nodes.Screens.ScreenContext;
using MegaCrit.Sts2.Core.Saves;
using MegaCrit.Sts2.Core.TestSupport;

namespace MegaCrit.Sts2.Core.Nodes.Screens.ProfileScreen
{
	/// <summary>
	/// Screen where the player can change profiles.
	/// </summary>
	// Token: 0x020002DC RID: 732
	[ScriptPath("res://src/Core/Nodes/Screens/ProfileScreen/NProfileScreen.cs")]
	public class NProfileScreen : NSubmenu
	{
		// Token: 0x170006E7 RID: 1767
		// (get) Token: 0x0600270F RID: 9999 RVA: 0x0010ABE8 File Offset: 0x00108DE8
		[Nullable(1)]
		public static string[] AssetPaths
		{
			[NullableContext(1)]
			get
			{
				List<string> list = new List<string>();
				list.Add(NProfileScreen._scenePath);
				list.AddRange(NProfileButton.AssetPaths);
				return list.ToArray();
			}
		}

		// Token: 0x170006E8 RID: 1768
		// (get) Token: 0x06002710 RID: 10000 RVA: 0x0010AC0A File Offset: 0x00108E0A
		[Nullable(1)]
		protected override Control InitialFocusedControl
		{
			[NullableContext(1)]
			get
			{
				return this._profileButtons[SaveManager.Instance.CurrentProfileId - 1];
			}
		}

		// Token: 0x06002711 RID: 10001 RVA: 0x0010AC23 File Offset: 0x00108E23
		[NullableContext(2)]
		public static NProfileScreen Create()
		{
			if (TestMode.IsOn)
			{
				return null;
			}
			return PreloadManager.Cache.GetScene(NProfileScreen._scenePath).Instantiate<NProfileScreen>(0L);
		}

		// Token: 0x06002712 RID: 10002 RVA: 0x0010AC44 File Offset: 0x00108E44
		public override void _Ready()
		{
			this.ConnectSignals();
			this._loadingOverlay = base.GetNode<Control>("%LoadingOverlay");
			base.GetNode<MegaLabel>("%ChooseProfileMessage").SetTextAutoSize(new LocString("main_menu_ui", "PROFILE_SCREEN.BUTTON.chooseProfileMessage").GetFormattedText());
			this._profileButtons.Add(base.GetNode<NProfileButton>("%ProfileButton1"));
			this._profileButtons.Add(base.GetNode<NProfileButton>("%ProfileButton2"));
			this._profileButtons.Add(base.GetNode<NProfileButton>("%ProfileButton3"));
			this._deleteButtons.Add(base.GetNode<NDeleteProfileButton>("%DeleteProfileButton1"));
			this._deleteButtons.Add(base.GetNode<NDeleteProfileButton>("%DeleteProfileButton2"));
			this._deleteButtons.Add(base.GetNode<NDeleteProfileButton>("%DeleteProfileButton3"));
			if (this._profileButtons.Count != 3)
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(109, 2);
				defaultInterpolatedStringHandler.AppendLiteral("There are ");
				defaultInterpolatedStringHandler.AppendFormatted<int>(this._profileButtons.Count);
				defaultInterpolatedStringHandler.AppendLiteral(" profile buttons, but max profile count in ProfileSaveManager is ");
				defaultInterpolatedStringHandler.AppendFormatted<int>(3);
				defaultInterpolatedStringHandler.AppendLiteral("! This might result in subtle bugs");
				Log.Error(defaultInterpolatedStringHandler.ToStringAndClear(), 2);
			}
			for (int i = 0; i < this._profileButtons.Count; i++)
			{
				this._profileButtons[i].FocusNeighborTop = this._profileButtons[i].GetPath();
				this._profileButtons[i].FocusNeighborBottom = this._deleteButtons[i].GetPath();
				Control control = this._profileButtons[i];
				NodePath nodePath;
				if (i <= 0)
				{
					List<NProfileButton> profileButtons = this._profileButtons;
					nodePath = profileButtons[profileButtons.Count - 1].GetPath();
				}
				else
				{
					nodePath = this._profileButtons[i - 1].GetPath();
				}
				control.FocusNeighborLeft = nodePath;
				this._profileButtons[i].FocusNeighborRight = ((i < this._profileButtons.Count - 1) ? this._profileButtons[i + 1].GetPath() : this._profileButtons[0].GetPath());
				this._deleteButtons[i].FocusNeighborTop = this._profileButtons[i].GetPath();
				this._deleteButtons[i].FocusNeighborBottom = this._deleteButtons[i].GetPath();
				Control control2 = this._deleteButtons[i];
				NodePath nodePath2;
				if (i <= 0)
				{
					List<NDeleteProfileButton> deleteButtons = this._deleteButtons;
					nodePath2 = deleteButtons[deleteButtons.Count - 1].GetPath();
				}
				else
				{
					nodePath2 = this._deleteButtons[i - 1].GetPath();
				}
				control2.FocusNeighborLeft = nodePath2;
				this._deleteButtons[i].FocusNeighborRight = ((i < this._deleteButtons.Count - 1) ? this._deleteButtons[i + 1].GetPath() : this._deleteButtons[0].GetPath());
			}
		}

		// Token: 0x06002713 RID: 10003 RVA: 0x0010AF51 File Offset: 0x00109151
		public override void OnSubmenuOpened()
		{
			this.Refresh();
		}

		// Token: 0x06002714 RID: 10004 RVA: 0x0010AF59 File Offset: 0x00109159
		public void ShowLoading()
		{
			this._loadingOverlay.Visible = true;
		}

		// Token: 0x06002715 RID: 10005 RVA: 0x0010AF68 File Offset: 0x00109168
		public void Refresh()
		{
			for (int i = 0; i < this._profileButtons.Count; i++)
			{
				this._profileButtons[i].Initialize(this, i + 1);
			}
			for (int j = 0; j < this._deleteButtons.Count; j++)
			{
				this._deleteButtons[j].Initialize(this, j + 1);
			}
			NProfileScreen.forceShowProfileAsDeleted = null;
			ActiveScreenContext.Instance.Update();
		}

		/// <summary>
		/// Get the method information for all the methods declared in this class.
		/// This method is used by Godot to register the available methods in the editor.
		/// Do not call this method.
		/// </summary>
		// Token: 0x06002716 RID: 10006 RVA: 0x0010AFE0 File Offset: 0x001091E0
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal new static List<MethodInfo> GetGodotMethodList()
		{
			return new List<MethodInfo>(5)
			{
				new MethodInfo(NProfileScreen.MethodName.Create, new PropertyInfo(24L, "", 0L, "", 6L, new StringName("Control"), false), 33L, null, null),
				new MethodInfo(NProfileScreen.MethodName._Ready, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NProfileScreen.MethodName.OnSubmenuOpened, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NProfileScreen.MethodName.ShowLoading, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NProfileScreen.MethodName.Refresh, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null)
			};
		}

		/// <inheritdoc />
		// Token: 0x06002717 RID: 10007 RVA: 0x0010B0EC File Offset: 0x001092EC
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool InvokeGodotClassMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
		{
			if ((ref method) == NProfileScreen.MethodName.Create && args.Count == 0)
			{
				NProfileScreen nprofileScreen = NProfileScreen.Create();
				ret = VariantUtils.CreateFrom<NProfileScreen>(ref nprofileScreen);
				return true;
			}
			if ((ref method) == NProfileScreen.MethodName._Ready && args.Count == 0)
			{
				this._Ready();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NProfileScreen.MethodName.OnSubmenuOpened && args.Count == 0)
			{
				this.OnSubmenuOpened();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NProfileScreen.MethodName.ShowLoading && args.Count == 0)
			{
				this.ShowLoading();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NProfileScreen.MethodName.Refresh && args.Count == 0)
			{
				this.Refresh();
				ret = default(godot_variant);
				return true;
			}
			return base.InvokeGodotClassMethod(in method, args, out ret);
		}

		// Token: 0x06002718 RID: 10008 RVA: 0x0010B1C4 File Offset: 0x001093C4
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal static bool InvokeGodotClassStaticMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
		{
			if ((ref method) == NProfileScreen.MethodName.Create && args.Count == 0)
			{
				NProfileScreen nprofileScreen = NProfileScreen.Create();
				ret = VariantUtils.CreateFrom<NProfileScreen>(ref nprofileScreen);
				return true;
			}
			ret = default(godot_variant);
			return false;
		}

		/// <inheritdoc />
		// Token: 0x06002719 RID: 10009 RVA: 0x0010B204 File Offset: 0x00109404
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool HasGodotClassMethod(in godot_string_name method)
		{
			return (ref method) == NProfileScreen.MethodName.Create || (ref method) == NProfileScreen.MethodName._Ready || (ref method) == NProfileScreen.MethodName.OnSubmenuOpened || (ref method) == NProfileScreen.MethodName.ShowLoading || (ref method) == NProfileScreen.MethodName.Refresh || base.HasGodotClassMethod(in method);
		}

		/// <inheritdoc />
		// Token: 0x0600271A RID: 10010 RVA: 0x0010B263 File Offset: 0x00109463
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool SetGodotClassPropertyValue(in godot_string_name name, in godot_variant value)
		{
			if ((ref name) == NProfileScreen.PropertyName._loadingOverlay)
			{
				this._loadingOverlay = VariantUtils.ConvertTo<Control>(ref value);
				return true;
			}
			return base.SetGodotClassPropertyValue(in name, in value);
		}

		/// <inheritdoc />
		// Token: 0x0600271B RID: 10011 RVA: 0x0010B288 File Offset: 0x00109488
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
		{
			if ((ref name) == NProfileScreen.PropertyName.InitialFocusedControl)
			{
				Control initialFocusedControl = this.InitialFocusedControl;
				value = VariantUtils.CreateFrom<Control>(ref initialFocusedControl);
				return true;
			}
			if ((ref name) == NProfileScreen.PropertyName._loadingOverlay)
			{
				value = VariantUtils.CreateFrom<Control>(ref this._loadingOverlay);
				return true;
			}
			return base.GetGodotClassPropertyValue(in name, out value);
		}

		/// <summary>
		/// Get the property information for all the properties declared in this class.
		/// This method is used by Godot to register the available properties in the editor.
		/// Do not call this method.
		/// </summary>
		// Token: 0x0600271C RID: 10012 RVA: 0x0010B2E0 File Offset: 0x001094E0
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal new static List<PropertyInfo> GetGodotPropertyList()
		{
			return new List<PropertyInfo>
			{
				new PropertyInfo(24L, NProfileScreen.PropertyName._loadingOverlay, 0L, "", 4096L, false),
				new PropertyInfo(24L, NProfileScreen.PropertyName.InitialFocusedControl, 0L, "", 4096L, false)
			};
		}

		/// <inheritdoc />
		// Token: 0x0600271D RID: 10013 RVA: 0x0010B336 File Offset: 0x00109536
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void SaveGodotObjectData(GodotSerializationInfo info)
		{
			base.SaveGodotObjectData(info);
			info.AddProperty(NProfileScreen.PropertyName._loadingOverlay, Variant.From<Control>(ref this._loadingOverlay));
		}

		/// <inheritdoc />
		// Token: 0x0600271E RID: 10014 RVA: 0x0010B358 File Offset: 0x00109558
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void RestoreGodotObjectData(GodotSerializationInfo info)
		{
			base.RestoreGodotObjectData(info);
			Variant variant;
			if (info.TryGetProperty(NProfileScreen.PropertyName._loadingOverlay, ref variant))
			{
				this._loadingOverlay = variant.As<Control>();
			}
		}

		// Token: 0x04000E8C RID: 3724
		public static int? forceShowProfileAsDeleted;

		// Token: 0x04000E8D RID: 3725
		[Nullable(1)]
		private static readonly string _scenePath = SceneHelper.GetScenePath("screens/profiles/profile_screen");

		// Token: 0x04000E8E RID: 3726
		[Nullable(1)]
		private Control _loadingOverlay;

		// Token: 0x04000E8F RID: 3727
		[Nullable(1)]
		private readonly List<NProfileButton> _profileButtons = new List<NProfileButton>();

		// Token: 0x04000E90 RID: 3728
		[Nullable(1)]
		private readonly List<NDeleteProfileButton> _deleteButtons = new List<NDeleteProfileButton>();

		/// <summary>
		/// Cached StringNames for the methods contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x02001465 RID: 5221
		public new class MethodName : NSubmenu.MethodName
		{
			/// <summary>
			/// Cached name for the 'Create' method.
			/// </summary>
			// Token: 0x04004D6C RID: 19820
			public static readonly StringName Create = "Create";

			/// <summary>
			/// Cached name for the '_Ready' method.
			/// </summary>
			// Token: 0x04004D6D RID: 19821
			public new static readonly StringName _Ready = "_Ready";

			/// <summary>
			/// Cached name for the 'OnSubmenuOpened' method.
			/// </summary>
			// Token: 0x04004D6E RID: 19822
			public new static readonly StringName OnSubmenuOpened = "OnSubmenuOpened";

			/// <summary>
			/// Cached name for the 'ShowLoading' method.
			/// </summary>
			// Token: 0x04004D6F RID: 19823
			public static readonly StringName ShowLoading = "ShowLoading";

			/// <summary>
			/// Cached name for the 'Refresh' method.
			/// </summary>
			// Token: 0x04004D70 RID: 19824
			public static readonly StringName Refresh = "Refresh";
		}

		/// <summary>
		/// Cached StringNames for the properties and fields contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x02001466 RID: 5222
		public new class PropertyName : NSubmenu.PropertyName
		{
			/// <summary>
			/// Cached name for the 'InitialFocusedControl' property.
			/// </summary>
			// Token: 0x04004D71 RID: 19825
			public new static readonly StringName InitialFocusedControl = "InitialFocusedControl";

			/// <summary>
			/// Cached name for the '_loadingOverlay' field.
			/// </summary>
			// Token: 0x04004D72 RID: 19826
			public static readonly StringName _loadingOverlay = "_loadingOverlay";
		}

		/// <summary>
		/// Cached StringNames for the signals contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x02001467 RID: 5223
		public new class SignalName : NSubmenu.SignalName
		{
		}
	}
}
