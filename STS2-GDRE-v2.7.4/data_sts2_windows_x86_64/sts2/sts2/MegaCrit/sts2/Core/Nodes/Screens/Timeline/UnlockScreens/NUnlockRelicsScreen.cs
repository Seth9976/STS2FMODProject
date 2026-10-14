using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Godot;
using Godot.Bridge;
using Godot.NativeInterop;
using MegaCrit.Sts2.addons.mega_text;
using MegaCrit.Sts2.Core.Assets;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.Localization;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Nodes.CommonUi;
using MegaCrit.Sts2.Core.Nodes.Relics;
using MegaCrit.Sts2.Core.Nodes.Screens.ScreenContext;

namespace MegaCrit.Sts2.Core.Nodes.Screens.Timeline.UnlockScreens
{
	/// <summary>
	/// Unlock screen which reveals which relics you unlocked after slotting an Epoch.
	/// We want to let players know that these relics show up on future runs and
	/// allow them to inspect each relic.
	/// </summary>
	// Token: 0x0200026E RID: 622
	[ScriptPath("res://src/Core/Nodes/Screens/Timeline/UnlockScreens/NUnlockRelicsScreen.cs")]
	public class NUnlockRelicsScreen : NUnlockScreen
	{
		// Token: 0x17000694 RID: 1684
		// (get) Token: 0x060020CA RID: 8394 RVA: 0x000DE5A1 File Offset: 0x000DC7A1
		[Nullable(1)]
		public static IEnumerable<string> AssetPaths
		{
			[NullableContext(1)]
			get
			{
				return new <>z__ReadOnlySingleElementList<string>(NUnlockRelicsScreen._scenePath);
			}
		}

		// Token: 0x060020CB RID: 8395 RVA: 0x000DE5AD File Offset: 0x000DC7AD
		[NullableContext(1)]
		public static NUnlockRelicsScreen Create()
		{
			return PreloadManager.Cache.GetScene(NUnlockRelicsScreen._scenePath).Instantiate<NUnlockRelicsScreen>(0L);
		}

		// Token: 0x060020CC RID: 8396 RVA: 0x000DE5C8 File Offset: 0x000DC7C8
		public override void _Ready()
		{
			base.ConnectSignals();
			this._banner = base.GetNode<NCommonBanner>("%Banner");
			this._banner.label.SetTextAutoSize(new LocString("timeline", "UNLOCK_RELICS_BANNER").GetRawText());
			this._banner.AnimateIn();
			this._relicRow = base.GetNode<Control>("%RelicRow");
			LocString locString = new LocString("timeline", "UNLOCK_RELICS");
			base.GetNode<MegaRichTextLabel>("%ExplanationText").Text = "[center]" + locString.GetFormattedText() + "[/center]";
		}

		// Token: 0x060020CD RID: 8397 RVA: 0x000DE670 File Offset: 0x000DC870
		public override void Open()
		{
			base.Open();
			SfxCmd.Play("event:/sfx/ui/timeline/ui_timeline_unlock", 1f);
			Vector2 vector = Vector2.Left * (float)(this._relics.Count - 1) * 350f * 0.5f;
			this._relicTween = base.CreateTween().SetParallel(true);
			int num = 0;
			foreach (RelicModel relicModel in this._relics)
			{
				NRelicBasicHolder nrelicBasicHolder = NRelicBasicHolder.Create(relicModel);
				this._relicRow.AddChildSafely(nrelicBasicHolder);
				nrelicBasicHolder.Modulate = StsColors.transparentBlack;
				nrelicBasicHolder.Scale = NUnlockRelicsScreen._relicScale;
				this._relicTween.TweenProperty(nrelicBasicHolder, "position", vector + Vector2.Right * 350f * (float)num, 0.5).SetEase(1L).SetTrans(5L);
				this._relicTween.TweenProperty(nrelicBasicHolder, "modulate", Colors.White, 1.0).SetEase(1L).SetTrans(7L);
				num++;
			}
			ActiveScreenContext.Instance.FocusOnDefaultControl();
		}

		// Token: 0x060020CE RID: 8398 RVA: 0x000DE7D8 File Offset: 0x000DC9D8
		[NullableContext(1)]
		public void SetRelics(IReadOnlyList<RelicModel> relics)
		{
			this._relics = relics;
		}

		// Token: 0x060020CF RID: 8399 RVA: 0x000DE7E1 File Offset: 0x000DC9E1
		protected override void OnScreenClose()
		{
			NTimelineScreen.Instance.EnableInput();
		}

		// Token: 0x17000695 RID: 1685
		// (get) Token: 0x060020D0 RID: 8400 RVA: 0x000DE7ED File Offset: 0x000DC9ED
		[Nullable(2)]
		public override Control DefaultFocusedControl
		{
			[NullableContext(2)]
			get
			{
				if (this._relicRow.GetChildCount(false) != 0)
				{
					return this._relicRow.GetChild<Control>(this._relicRow.GetChildCount(false) / 2, false);
				}
				return null;
			}
		}

		/// <summary>
		/// Get the method information for all the methods declared in this class.
		/// This method is used by Godot to register the available methods in the editor.
		/// Do not call this method.
		/// </summary>
		// Token: 0x060020D1 RID: 8401 RVA: 0x000DE81C File Offset: 0x000DCA1C
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal new static List<MethodInfo> GetGodotMethodList()
		{
			return new List<MethodInfo>(4)
			{
				new MethodInfo(NUnlockRelicsScreen.MethodName.Create, new PropertyInfo(24L, "", 0L, "", 6L, new StringName("Control"), false), 33L, null, null),
				new MethodInfo(NUnlockRelicsScreen.MethodName._Ready, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NUnlockRelicsScreen.MethodName.Open, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NUnlockRelicsScreen.MethodName.OnScreenClose, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null)
			};
		}

		/// <inheritdoc />
		// Token: 0x060020D2 RID: 8402 RVA: 0x000DE8FC File Offset: 0x000DCAFC
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool InvokeGodotClassMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
		{
			if ((ref method) == NUnlockRelicsScreen.MethodName.Create && args.Count == 0)
			{
				NUnlockRelicsScreen nunlockRelicsScreen = NUnlockRelicsScreen.Create();
				ret = VariantUtils.CreateFrom<NUnlockRelicsScreen>(ref nunlockRelicsScreen);
				return true;
			}
			if ((ref method) == NUnlockRelicsScreen.MethodName._Ready && args.Count == 0)
			{
				this._Ready();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NUnlockRelicsScreen.MethodName.Open && args.Count == 0)
			{
				this.Open();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NUnlockRelicsScreen.MethodName.OnScreenClose && args.Count == 0)
			{
				this.OnScreenClose();
				ret = default(godot_variant);
				return true;
			}
			return base.InvokeGodotClassMethod(in method, args, out ret);
		}

		// Token: 0x060020D3 RID: 8403 RVA: 0x000DE9AC File Offset: 0x000DCBAC
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal static bool InvokeGodotClassStaticMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
		{
			if ((ref method) == NUnlockRelicsScreen.MethodName.Create && args.Count == 0)
			{
				NUnlockRelicsScreen nunlockRelicsScreen = NUnlockRelicsScreen.Create();
				ret = VariantUtils.CreateFrom<NUnlockRelicsScreen>(ref nunlockRelicsScreen);
				return true;
			}
			ret = default(godot_variant);
			return false;
		}

		/// <inheritdoc />
		// Token: 0x060020D4 RID: 8404 RVA: 0x000DE9EC File Offset: 0x000DCBEC
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool HasGodotClassMethod(in godot_string_name method)
		{
			return (ref method) == NUnlockRelicsScreen.MethodName.Create || (ref method) == NUnlockRelicsScreen.MethodName._Ready || (ref method) == NUnlockRelicsScreen.MethodName.Open || (ref method) == NUnlockRelicsScreen.MethodName.OnScreenClose || base.HasGodotClassMethod(in method);
		}

		/// <inheritdoc />
		// Token: 0x060020D5 RID: 8405 RVA: 0x000DEA3C File Offset: 0x000DCC3C
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool SetGodotClassPropertyValue(in godot_string_name name, in godot_variant value)
		{
			if ((ref name) == NUnlockRelicsScreen.PropertyName._relicRow)
			{
				this._relicRow = VariantUtils.ConvertTo<Control>(ref value);
				return true;
			}
			if ((ref name) == NUnlockRelicsScreen.PropertyName._banner)
			{
				this._banner = VariantUtils.ConvertTo<NCommonBanner>(ref value);
				return true;
			}
			if ((ref name) == NUnlockRelicsScreen.PropertyName._relicTween)
			{
				this._relicTween = VariantUtils.ConvertTo<Tween>(ref value);
				return true;
			}
			return base.SetGodotClassPropertyValue(in name, in value);
		}

		/// <inheritdoc />
		// Token: 0x060020D6 RID: 8406 RVA: 0x000DEAA4 File Offset: 0x000DCCA4
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
		{
			if ((ref name) == NUnlockRelicsScreen.PropertyName.DefaultFocusedControl)
			{
				Control defaultFocusedControl = this.DefaultFocusedControl;
				value = VariantUtils.CreateFrom<Control>(ref defaultFocusedControl);
				return true;
			}
			if ((ref name) == NUnlockRelicsScreen.PropertyName._relicRow)
			{
				value = VariantUtils.CreateFrom<Control>(ref this._relicRow);
				return true;
			}
			if ((ref name) == NUnlockRelicsScreen.PropertyName._banner)
			{
				value = VariantUtils.CreateFrom<NCommonBanner>(ref this._banner);
				return true;
			}
			if ((ref name) == NUnlockRelicsScreen.PropertyName._relicTween)
			{
				value = VariantUtils.CreateFrom<Tween>(ref this._relicTween);
				return true;
			}
			return base.GetGodotClassPropertyValue(in name, out value);
		}

		/// <summary>
		/// Get the property information for all the properties declared in this class.
		/// This method is used by Godot to register the available properties in the editor.
		/// Do not call this method.
		/// </summary>
		// Token: 0x060020D7 RID: 8407 RVA: 0x000DEB3C File Offset: 0x000DCD3C
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal new static List<PropertyInfo> GetGodotPropertyList()
		{
			return new List<PropertyInfo>
			{
				new PropertyInfo(24L, NUnlockRelicsScreen.PropertyName._relicRow, 0L, "", 4096L, false),
				new PropertyInfo(24L, NUnlockRelicsScreen.PropertyName._banner, 0L, "", 4096L, false),
				new PropertyInfo(24L, NUnlockRelicsScreen.PropertyName._relicTween, 0L, "", 4096L, false),
				new PropertyInfo(24L, NUnlockRelicsScreen.PropertyName.DefaultFocusedControl, 0L, "", 4096L, false)
			};
		}

		/// <inheritdoc />
		// Token: 0x060020D8 RID: 8408 RVA: 0x000DEBD4 File Offset: 0x000DCDD4
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void SaveGodotObjectData(GodotSerializationInfo info)
		{
			base.SaveGodotObjectData(info);
			info.AddProperty(NUnlockRelicsScreen.PropertyName._relicRow, Variant.From<Control>(ref this._relicRow));
			info.AddProperty(NUnlockRelicsScreen.PropertyName._banner, Variant.From<NCommonBanner>(ref this._banner));
			info.AddProperty(NUnlockRelicsScreen.PropertyName._relicTween, Variant.From<Tween>(ref this._relicTween));
		}

		/// <inheritdoc />
		// Token: 0x060020D9 RID: 8409 RVA: 0x000DEC2C File Offset: 0x000DCE2C
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void RestoreGodotObjectData(GodotSerializationInfo info)
		{
			base.RestoreGodotObjectData(info);
			Variant variant;
			if (info.TryGetProperty(NUnlockRelicsScreen.PropertyName._relicRow, ref variant))
			{
				this._relicRow = variant.As<Control>();
			}
			Variant variant2;
			if (info.TryGetProperty(NUnlockRelicsScreen.PropertyName._banner, ref variant2))
			{
				this._banner = variant2.As<NCommonBanner>();
			}
			Variant variant3;
			if (info.TryGetProperty(NUnlockRelicsScreen.PropertyName._relicTween, ref variant3))
			{
				this._relicTween = variant3.As<Tween>();
			}
		}

		// Token: 0x04000CC8 RID: 3272
		[Nullable(1)]
		private static readonly string _scenePath = SceneHelper.GetScenePath("timeline_screen/unlock_relics_screen");

		// Token: 0x04000CC9 RID: 3273
		[Nullable(1)]
		private Control _relicRow;

		// Token: 0x04000CCA RID: 3274
		[Nullable(1)]
		private NCommonBanner _banner;

		// Token: 0x04000CCB RID: 3275
		[Nullable(1)]
		private IReadOnlyList<RelicModel> _relics;

		// Token: 0x04000CCC RID: 3276
		[Nullable(2)]
		private Tween _relicTween;

		// Token: 0x04000CCD RID: 3277
		private const float _relicXOffset = 350f;

		// Token: 0x04000CCE RID: 3278
		private static readonly Vector2 _relicScale = Vector2.One * 3f;

		/// <summary>
		/// Cached StringNames for the methods contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x020012E8 RID: 4840
		public new class MethodName : NUnlockScreen.MethodName
		{
			/// <summary>
			/// Cached name for the 'Create' method.
			/// </summary>
			// Token: 0x040049A2 RID: 18850
			public static readonly StringName Create = "Create";

			/// <summary>
			/// Cached name for the '_Ready' method.
			/// </summary>
			// Token: 0x040049A3 RID: 18851
			public new static readonly StringName _Ready = "_Ready";

			/// <summary>
			/// Cached name for the 'Open' method.
			/// </summary>
			// Token: 0x040049A4 RID: 18852
			public new static readonly StringName Open = "Open";

			/// <summary>
			/// Cached name for the 'OnScreenClose' method.
			/// </summary>
			// Token: 0x040049A5 RID: 18853
			public new static readonly StringName OnScreenClose = "OnScreenClose";
		}

		/// <summary>
		/// Cached StringNames for the properties and fields contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x020012E9 RID: 4841
		public new class PropertyName : NUnlockScreen.PropertyName
		{
			/// <summary>
			/// Cached name for the 'DefaultFocusedControl' property.
			/// </summary>
			// Token: 0x040049A6 RID: 18854
			public new static readonly StringName DefaultFocusedControl = "DefaultFocusedControl";

			/// <summary>
			/// Cached name for the '_relicRow' field.
			/// </summary>
			// Token: 0x040049A7 RID: 18855
			public static readonly StringName _relicRow = "_relicRow";

			/// <summary>
			/// Cached name for the '_banner' field.
			/// </summary>
			// Token: 0x040049A8 RID: 18856
			public static readonly StringName _banner = "_banner";

			/// <summary>
			/// Cached name for the '_relicTween' field.
			/// </summary>
			// Token: 0x040049A9 RID: 18857
			public static readonly StringName _relicTween = "_relicTween";
		}

		/// <summary>
		/// Cached StringNames for the signals contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x020012EA RID: 4842
		public new class SignalName : NUnlockScreen.SignalName
		{
		}
	}
}
