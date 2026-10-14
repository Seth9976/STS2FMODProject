using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using Godot;
using Godot.Bridge;
using Godot.NativeInterop;
using MegaCrit.Sts2.addons.mega_text;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.Logging;
using MegaCrit.Sts2.Core.Nodes.CommonUi;
using MegaCrit.Sts2.Core.Platform;
using MegaCrit.Sts2.Core.Saves;

namespace MegaCrit.Sts2.Core.Nodes.Screens.Settings
{
	/// <summary>
	/// The resolution dropdown in the OptionsScreen.
	/// </summary>
	// Token: 0x020002AF RID: 687
	[ScriptPath("res://src/Core/Nodes/Screens/Settings/NResolutionDropdown.cs")]
	public class NResolutionDropdown : NSettingsDropdown
	{
		// Token: 0x170006BF RID: 1727
		// (get) Token: 0x06002457 RID: 9303 RVA: 0x000F66D2 File Offset: 0x000F48D2
		// (set) Token: 0x06002458 RID: 9304 RVA: 0x000F66D9 File Offset: 0x000F48D9
		[Nullable(2)]
		public static NResolutionDropdown Instance
		{
			[NullableContext(2)]
			get;
			[NullableContext(2)]
			private set;
		}

		// Token: 0x06002459 RID: 9305 RVA: 0x000F66E1 File Offset: 0x000F48E1
		public override void _EnterTree()
		{
			NResolutionDropdown.Instance = this;
		}

		// Token: 0x0600245A RID: 9306 RVA: 0x000F66EC File Offset: 0x000F48EC
		public override void _Ready()
		{
			this.ConnectSignals();
			this._arrow = base.GetNode<Control>("%Arrow");
			NGame.Instance.Connect(NGame.SignalName.WindowChange, Callable.From<bool>(new Action<bool>(this.OnWindowChange)), 0U);
			this.RefreshEnabled();
			this.RefreshCurrentlySelectedResolution();
			this.PopulateDropdownItems();
		}

		// Token: 0x0600245B RID: 9307 RVA: 0x000F674C File Offset: 0x000F494C
		public void RefreshCurrentlySelectedResolution()
		{
			if (base.IsEnabled)
			{
				NResolutionDropdown._currentResolution = DisplayServer.WindowGetSize(0);
				MegaLabel currentOptionLabel = this._currentOptionLabel;
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(3, 2);
				defaultInterpolatedStringHandler.AppendFormatted<int>(NResolutionDropdown._currentResolution.X);
				defaultInterpolatedStringHandler.AppendLiteral(" x ");
				defaultInterpolatedStringHandler.AppendFormatted<int>(NResolutionDropdown._currentResolution.Y);
				currentOptionLabel.SetTextAutoSize(defaultInterpolatedStringHandler.ToStringAndClear());
			}
		}

		/// <summary>
		/// A separate function as we repopulate the list of resolutions whenever the player
		/// changes the Display they wish to play the game on.
		/// </summary>
		// Token: 0x0600245C RID: 9308 RVA: 0x000F67B8 File Offset: 0x000F49B8
		public void PopulateDropdownItems()
		{
			base.ClearDropdownItems();
			Vector2I vector2I = DisplayServer.ScreenGetSize(SaveManager.Instance.SettingsSave.TargetDisplay);
			foreach (Vector2I vector2I2 in NResolutionDropdown.GetResolutionWhiteList())
			{
				if (NResolutionDropdown.DoesResolutionFit(vector2I2, vector2I))
				{
					NResolutionDropdownItem nresolutionDropdownItem = this._dropdownItemScene.Instantiate<NResolutionDropdownItem>(0L);
					this._dropdownItems.AddChildSafely(nresolutionDropdownItem);
					nresolutionDropdownItem.Connect(NDropdownItem.SignalName.Selected, Callable.From<NDropdownItem>(new Action<NDropdownItem>(this.OnDropdownItemSelected)), 0U);
					nresolutionDropdownItem.Init(vector2I2);
				}
			}
			this._dropdownItems.GetParent<NDropdownContainer>().RefreshLayout();
		}

		// Token: 0x0600245D RID: 9309 RVA: 0x000F6878 File Offset: 0x000F4A78
		private void OnWindowChange(bool isAutoAspectRatio)
		{
			this.RefreshEnabled();
			this.RefreshCurrentlySelectedResolution();
		}

		// Token: 0x0600245E RID: 9310 RVA: 0x000F6886 File Offset: 0x000F4A86
		private void RefreshEnabled()
		{
			if (SaveManager.Instance.SettingsSave.Fullscreen || PlatformUtil.GetSupportedWindowMode().ShouldForceFullscreen())
			{
				base.Disable();
				return;
			}
			base.Enable();
		}

		// Token: 0x0600245F RID: 9311 RVA: 0x000F68B2 File Offset: 0x000F4AB2
		protected override void OnEnable()
		{
			this._currentOptionLabel.Modulate = StsColors.gold;
			this._arrow.Visible = true;
			this.RefreshCurrentlySelectedResolution();
		}

		// Token: 0x06002460 RID: 9312 RVA: 0x000F68D6 File Offset: 0x000F4AD6
		protected override void OnDisable()
		{
			this._currentOptionLabel.SetTextAutoSize("N/A");
			this._currentOptionLabel.Modulate = StsColors.gray;
			this._arrow.Visible = false;
		}

		// Token: 0x06002461 RID: 9313 RVA: 0x000F6904 File Offset: 0x000F4B04
		[NullableContext(1)]
		private void OnDropdownItemSelected(NDropdownItem nDropdownItem)
		{
			NResolutionDropdownItem nresolutionDropdownItem = (NResolutionDropdownItem)nDropdownItem;
			if (nresolutionDropdownItem.resolution == NResolutionDropdown._currentResolution)
			{
				return;
			}
			base.CloseDropdown();
			SaveManager.Instance.SettingsSave.WindowPosition = DisplayServer.WindowGetPosition(0) - DisplayServer.ScreenGetPosition(SaveManager.Instance.SettingsSave.TargetDisplay);
			SaveManager.Instance.SettingsSave.WindowSize = nresolutionDropdownItem.resolution;
			DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(37, 1);
			defaultInterpolatedStringHandler.AppendLiteral("Setting window size to ");
			defaultInterpolatedStringHandler.AppendFormatted<Vector2I>(nresolutionDropdownItem.resolution);
			defaultInterpolatedStringHandler.AppendLiteral(" from dropdown");
			Log.Info(defaultInterpolatedStringHandler.ToStringAndClear(), 2);
			NGame.Instance.ApplyDisplaySettings();
		}

		// Token: 0x06002462 RID: 9314 RVA: 0x000F69BA File Offset: 0x000F4BBA
		private static bool DoesResolutionFit(Vector2I resolution, Vector2I boundaryResolution)
		{
			return resolution.X <= boundaryResolution.X && resolution.Y <= boundaryResolution.Y;
		}

		// Token: 0x06002463 RID: 9315 RVA: 0x000F69E0 File Offset: 0x000F4BE0
		[NullableContext(1)]
		private unsafe static List<Vector2I> GetResolutionWhiteList()
		{
			int num = 26;
			List<Vector2I> list = new List<Vector2I>(num);
			CollectionsMarshal.SetCount<Vector2I>(list, num);
			Span<Vector2I> span = CollectionsMarshal.AsSpan<Vector2I>(list);
			int num2 = 0;
			*span[num2] = new Vector2I(1024, 768);
			num2++;
			*span[num2] = new Vector2I(1152, 864);
			num2++;
			*span[num2] = new Vector2I(1280, 720);
			num2++;
			*span[num2] = new Vector2I(1280, 800);
			num2++;
			*span[num2] = new Vector2I(1280, 960);
			num2++;
			*span[num2] = new Vector2I(1366, 768);
			num2++;
			*span[num2] = new Vector2I(1400, 1050);
			num2++;
			*span[num2] = new Vector2I(1440, 900);
			num2++;
			*span[num2] = new Vector2I(1440, 1080);
			num2++;
			*span[num2] = new Vector2I(1600, 900);
			num2++;
			*span[num2] = new Vector2I(1600, 1200);
			num2++;
			*span[num2] = new Vector2I(1680, 1050);
			num2++;
			*span[num2] = new Vector2I(1856, 1392);
			num2++;
			*span[num2] = new Vector2I(1920, 1080);
			num2++;
			*span[num2] = new Vector2I(1920, 1200);
			num2++;
			*span[num2] = new Vector2I(1920, 1440);
			num2++;
			*span[num2] = new Vector2I(2048, 1536);
			num2++;
			*span[num2] = new Vector2I(2560, 1080);
			num2++;
			*span[num2] = new Vector2I(2560, 1440);
			num2++;
			*span[num2] = new Vector2I(2560, 1600);
			num2++;
			*span[num2] = new Vector2I(3200, 1800);
			num2++;
			*span[num2] = new Vector2I(3440, 1440);
			num2++;
			*span[num2] = new Vector2I(3840, 1600);
			num2++;
			*span[num2] = new Vector2I(3840, 2160);
			num2++;
			*span[num2] = new Vector2I(3840, 2400);
			num2++;
			*span[num2] = new Vector2I(7680, 4320);
			return list;
		}

		/// <summary>
		/// Get the method information for all the methods declared in this class.
		/// This method is used by Godot to register the available methods in the editor.
		/// Do not call this method.
		/// </summary>
		// Token: 0x06002464 RID: 9316 RVA: 0x000F6D44 File Offset: 0x000F4F44
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal new static List<MethodInfo> GetGodotMethodList()
		{
			return new List<MethodInfo>(10)
			{
				new MethodInfo(NResolutionDropdown.MethodName._EnterTree, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NResolutionDropdown.MethodName._Ready, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NResolutionDropdown.MethodName.RefreshCurrentlySelectedResolution, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NResolutionDropdown.MethodName.PopulateDropdownItems, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NResolutionDropdown.MethodName.OnWindowChange, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, new List<PropertyInfo>
				{
					new PropertyInfo(1L, "isAutoAspectRatio", 0L, "", 6L, false)
				}, null),
				new MethodInfo(NResolutionDropdown.MethodName.RefreshEnabled, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NResolutionDropdown.MethodName.OnEnable, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NResolutionDropdown.MethodName.OnDisable, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NResolutionDropdown.MethodName.OnDropdownItemSelected, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, new List<PropertyInfo>
				{
					new PropertyInfo(24L, "nDropdownItem", 0L, "", 6L, new StringName("Control"), false)
				}, null),
				new MethodInfo(NResolutionDropdown.MethodName.DoesResolutionFit, new PropertyInfo(1L, "", 0L, "", 6L, false), 33L, new List<PropertyInfo>
				{
					new PropertyInfo(6L, "resolution", 0L, "", 6L, false),
					new PropertyInfo(6L, "boundaryResolution", 0L, "", 6L, false)
				}, null)
			};
		}

		/// <inheritdoc />
		// Token: 0x06002465 RID: 9317 RVA: 0x000F6FCC File Offset: 0x000F51CC
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool InvokeGodotClassMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
		{
			if ((ref method) == NResolutionDropdown.MethodName._EnterTree && args.Count == 0)
			{
				this._EnterTree();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NResolutionDropdown.MethodName._Ready && args.Count == 0)
			{
				this._Ready();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NResolutionDropdown.MethodName.RefreshCurrentlySelectedResolution && args.Count == 0)
			{
				this.RefreshCurrentlySelectedResolution();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NResolutionDropdown.MethodName.PopulateDropdownItems && args.Count == 0)
			{
				this.PopulateDropdownItems();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NResolutionDropdown.MethodName.OnWindowChange && args.Count == 1)
			{
				this.OnWindowChange(VariantUtils.ConvertTo<bool>(args[0]));
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NResolutionDropdown.MethodName.RefreshEnabled && args.Count == 0)
			{
				this.RefreshEnabled();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NResolutionDropdown.MethodName.OnEnable && args.Count == 0)
			{
				this.OnEnable();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NResolutionDropdown.MethodName.OnDisable && args.Count == 0)
			{
				this.OnDisable();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NResolutionDropdown.MethodName.OnDropdownItemSelected && args.Count == 1)
			{
				this.OnDropdownItemSelected(VariantUtils.ConvertTo<NDropdownItem>(args[0]));
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NResolutionDropdown.MethodName.DoesResolutionFit && args.Count == 2)
			{
				bool flag = NResolutionDropdown.DoesResolutionFit(VariantUtils.ConvertTo<Vector2I>(args[0]), VariantUtils.ConvertTo<Vector2I>(args[1]));
				ret = VariantUtils.CreateFrom<bool>(ref flag);
				return true;
			}
			return base.InvokeGodotClassMethod(in method, args, out ret);
		}

		// Token: 0x06002466 RID: 9318 RVA: 0x000F7194 File Offset: 0x000F5394
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal static bool InvokeGodotClassStaticMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
		{
			if ((ref method) == NResolutionDropdown.MethodName.DoesResolutionFit && args.Count == 2)
			{
				bool flag = NResolutionDropdown.DoesResolutionFit(VariantUtils.ConvertTo<Vector2I>(args[0]), VariantUtils.ConvertTo<Vector2I>(args[1]));
				ret = VariantUtils.CreateFrom<bool>(ref flag);
				return true;
			}
			ret = default(godot_variant);
			return false;
		}

		/// <inheritdoc />
		// Token: 0x06002467 RID: 9319 RVA: 0x000F71F0 File Offset: 0x000F53F0
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool HasGodotClassMethod(in godot_string_name method)
		{
			return (ref method) == NResolutionDropdown.MethodName._EnterTree || (ref method) == NResolutionDropdown.MethodName._Ready || (ref method) == NResolutionDropdown.MethodName.RefreshCurrentlySelectedResolution || (ref method) == NResolutionDropdown.MethodName.PopulateDropdownItems || (ref method) == NResolutionDropdown.MethodName.OnWindowChange || (ref method) == NResolutionDropdown.MethodName.RefreshEnabled || (ref method) == NResolutionDropdown.MethodName.OnEnable || (ref method) == NResolutionDropdown.MethodName.OnDisable || (ref method) == NResolutionDropdown.MethodName.OnDropdownItemSelected || (ref method) == NResolutionDropdown.MethodName.DoesResolutionFit || base.HasGodotClassMethod(in method);
		}

		/// <inheritdoc />
		// Token: 0x06002468 RID: 9320 RVA: 0x000F729A File Offset: 0x000F549A
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool SetGodotClassPropertyValue(in godot_string_name name, in godot_variant value)
		{
			if ((ref name) == NResolutionDropdown.PropertyName._dropdownItemScene)
			{
				this._dropdownItemScene = VariantUtils.ConvertTo<PackedScene>(ref value);
				return true;
			}
			if ((ref name) == NResolutionDropdown.PropertyName._arrow)
			{
				this._arrow = VariantUtils.ConvertTo<Control>(ref value);
				return true;
			}
			return base.SetGodotClassPropertyValue(in name, in value);
		}

		/// <inheritdoc />
		// Token: 0x06002469 RID: 9321 RVA: 0x000F72DC File Offset: 0x000F54DC
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
		{
			if ((ref name) == NResolutionDropdown.PropertyName._dropdownItemScene)
			{
				value = VariantUtils.CreateFrom<PackedScene>(ref this._dropdownItemScene);
				return true;
			}
			if ((ref name) == NResolutionDropdown.PropertyName._arrow)
			{
				value = VariantUtils.CreateFrom<Control>(ref this._arrow);
				return true;
			}
			return base.GetGodotClassPropertyValue(in name, out value);
		}

		/// <summary>
		/// Get the property information for all the properties declared in this class.
		/// This method is used by Godot to register the available properties in the editor.
		/// Do not call this method.
		/// </summary>
		// Token: 0x0600246A RID: 9322 RVA: 0x000F7334 File Offset: 0x000F5534
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal new static List<PropertyInfo> GetGodotPropertyList()
		{
			return new List<PropertyInfo>
			{
				new PropertyInfo(24L, NResolutionDropdown.PropertyName._dropdownItemScene, 17L, "PackedScene", 4102L, true),
				new PropertyInfo(24L, NResolutionDropdown.PropertyName._arrow, 0L, "", 4096L, false)
			};
		}

		/// <inheritdoc />
		// Token: 0x0600246B RID: 9323 RVA: 0x000F738B File Offset: 0x000F558B
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void SaveGodotObjectData(GodotSerializationInfo info)
		{
			base.SaveGodotObjectData(info);
			info.AddProperty(NResolutionDropdown.PropertyName._dropdownItemScene, Variant.From<PackedScene>(ref this._dropdownItemScene));
			info.AddProperty(NResolutionDropdown.PropertyName._arrow, Variant.From<Control>(ref this._arrow));
		}

		/// <inheritdoc />
		// Token: 0x0600246C RID: 9324 RVA: 0x000F73C0 File Offset: 0x000F55C0
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void RestoreGodotObjectData(GodotSerializationInfo info)
		{
			base.RestoreGodotObjectData(info);
			Variant variant;
			if (info.TryGetProperty(NResolutionDropdown.PropertyName._dropdownItemScene, ref variant))
			{
				this._dropdownItemScene = variant.As<PackedScene>();
			}
			Variant variant2;
			if (info.TryGetProperty(NResolutionDropdown.PropertyName._arrow, ref variant2))
			{
				this._arrow = variant2.As<Control>();
			}
		}

		// Token: 0x04000DB4 RID: 3508
		[Nullable(1)]
		[Export(/*Failed to decode CustomAttribute blob!*/)]
		private PackedScene _dropdownItemScene;

		// Token: 0x04000DB6 RID: 3510
		[Nullable(1)]
		private Control _arrow;

		// Token: 0x04000DB7 RID: 3511
		private static Vector2I _currentResolution;

		/// <summary>
		/// Cached StringNames for the methods contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x020013C5 RID: 5061
		public new class MethodName : NSettingsDropdown.MethodName
		{
			/// <summary>
			/// Cached name for the '_EnterTree' method.
			/// </summary>
			// Token: 0x04004BB7 RID: 19383
			public static readonly StringName _EnterTree = "_EnterTree";

			/// <summary>
			/// Cached name for the '_Ready' method.
			/// </summary>
			// Token: 0x04004BB8 RID: 19384
			public new static readonly StringName _Ready = "_Ready";

			/// <summary>
			/// Cached name for the 'RefreshCurrentlySelectedResolution' method.
			/// </summary>
			// Token: 0x04004BB9 RID: 19385
			public static readonly StringName RefreshCurrentlySelectedResolution = "RefreshCurrentlySelectedResolution";

			/// <summary>
			/// Cached name for the 'PopulateDropdownItems' method.
			/// </summary>
			// Token: 0x04004BBA RID: 19386
			public static readonly StringName PopulateDropdownItems = "PopulateDropdownItems";

			/// <summary>
			/// Cached name for the 'OnWindowChange' method.
			/// </summary>
			// Token: 0x04004BBB RID: 19387
			public static readonly StringName OnWindowChange = "OnWindowChange";

			/// <summary>
			/// Cached name for the 'RefreshEnabled' method.
			/// </summary>
			// Token: 0x04004BBC RID: 19388
			public static readonly StringName RefreshEnabled = "RefreshEnabled";

			/// <summary>
			/// Cached name for the 'OnEnable' method.
			/// </summary>
			// Token: 0x04004BBD RID: 19389
			public new static readonly StringName OnEnable = "OnEnable";

			/// <summary>
			/// Cached name for the 'OnDisable' method.
			/// </summary>
			// Token: 0x04004BBE RID: 19390
			public new static readonly StringName OnDisable = "OnDisable";

			/// <summary>
			/// Cached name for the 'OnDropdownItemSelected' method.
			/// </summary>
			// Token: 0x04004BBF RID: 19391
			public static readonly StringName OnDropdownItemSelected = "OnDropdownItemSelected";

			/// <summary>
			/// Cached name for the 'DoesResolutionFit' method.
			/// </summary>
			// Token: 0x04004BC0 RID: 19392
			public static readonly StringName DoesResolutionFit = "DoesResolutionFit";
		}

		/// <summary>
		/// Cached StringNames for the properties and fields contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x020013C6 RID: 5062
		public new class PropertyName : NSettingsDropdown.PropertyName
		{
			/// <summary>
			/// Cached name for the '_dropdownItemScene' field.
			/// </summary>
			// Token: 0x04004BC1 RID: 19393
			public static readonly StringName _dropdownItemScene = "_dropdownItemScene";

			/// <summary>
			/// Cached name for the '_arrow' field.
			/// </summary>
			// Token: 0x04004BC2 RID: 19394
			public static readonly StringName _arrow = "_arrow";
		}

		/// <summary>
		/// Cached StringNames for the signals contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x020013C7 RID: 5063
		public new class SignalName : NSettingsDropdown.SignalName
		{
		}
	}
}
