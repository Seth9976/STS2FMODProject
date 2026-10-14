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
using MegaCrit.Sts2.Core.Nodes.Potions;
using MegaCrit.Sts2.Core.Nodes.Screens.ScreenContext;

namespace MegaCrit.Sts2.Core.Nodes.Screens.Timeline.UnlockScreens
{
	/// <summary>
	/// Unlock screen which reveals which potions you unlocked after slotting an Epoch.
	/// We want to let players know that these potions show up on future runs and
	/// allow them to inspect each potions.
	/// </summary>
	// Token: 0x0200026D RID: 621
	[ScriptPath("res://src/Core/Nodes/Screens/Timeline/UnlockScreens/NUnlockPotionsScreen.cs")]
	public class NUnlockPotionsScreen : NUnlockScreen
	{
		// Token: 0x17000692 RID: 1682
		// (get) Token: 0x060020B8 RID: 8376 RVA: 0x000DDE81 File Offset: 0x000DC081
		[Nullable(1)]
		public static IEnumerable<string> AssetPaths
		{
			[NullableContext(1)]
			get
			{
				return new <>z__ReadOnlySingleElementList<string>(NUnlockPotionsScreen._scenePath);
			}
		}

		// Token: 0x060020B9 RID: 8377 RVA: 0x000DDE8D File Offset: 0x000DC08D
		[NullableContext(1)]
		public static NUnlockPotionsScreen Create()
		{
			return PreloadManager.Cache.GetScene(NUnlockPotionsScreen._scenePath).Instantiate<NUnlockPotionsScreen>(0L);
		}

		// Token: 0x060020BA RID: 8378 RVA: 0x000DDEA8 File Offset: 0x000DC0A8
		public override void _Ready()
		{
			base.ConnectSignals();
			this._banner = base.GetNode<NCommonBanner>("%Banner");
			this._banner.label.SetTextAutoSize(new LocString("timeline", "UNLOCK_POTIONS_BANNER").GetRawText());
			this._banner.AnimateIn();
			this._potionRow = base.GetNode<Control>("%PotionRow");
			LocString locString = new LocString("timeline", "UNLOCK_POTIONS");
			base.GetNode<MegaRichTextLabel>("%ExplanationText").Text = "[center]" + locString.GetFormattedText() + "[/center]";
		}

		// Token: 0x060020BB RID: 8379 RVA: 0x000DDF50 File Offset: 0x000DC150
		public override void Open()
		{
			base.Open();
			SfxCmd.Play("event:/sfx/ui/timeline/ui_timeline_unlock", 1f);
			this._potionTween = base.CreateTween().SetParallel(true);
			int num = -1;
			foreach (PotionModel potionModel in this._potions)
			{
				NPotionHolder npotionHolder = NPotionHolder.Create(false);
				this._potionRow.AddChildSafely(npotionHolder);
				npotionHolder.Position = base.Position;
				npotionHolder.Modulate = StsColors.transparentBlack;
				npotionHolder.Scale = NUnlockPotionsScreen._potionScale;
				NPotion npotion = NPotion.Create(potionModel.ToMutable());
				npotionHolder.AddPotion(npotion);
				npotion.Position = Vector2.Zero;
				this._potionTween.TweenProperty(npotionHolder, "position", base.Position + Vector2.Right * 350f * (float)num, 0.5).SetEase(1L).SetTrans(5L);
				this._potionTween.TweenProperty(npotionHolder, "modulate", Colors.White, 1.0).SetEase(1L).SetTrans(7L);
				num++;
			}
			ActiveScreenContext.Instance.FocusOnDefaultControl();
		}

		// Token: 0x060020BC RID: 8380 RVA: 0x000DE0B8 File Offset: 0x000DC2B8
		[NullableContext(1)]
		public void SetPotions(IReadOnlyList<PotionModel> potions)
		{
			this._potions = potions;
		}

		// Token: 0x060020BD RID: 8381 RVA: 0x000DE0C1 File Offset: 0x000DC2C1
		protected override void OnScreenClose()
		{
			NTimelineScreen.Instance.EnableInput();
		}

		// Token: 0x17000693 RID: 1683
		// (get) Token: 0x060020BE RID: 8382 RVA: 0x000DE0CD File Offset: 0x000DC2CD
		[Nullable(2)]
		public override Control DefaultFocusedControl
		{
			[NullableContext(2)]
			get
			{
				if (this._potionRow.GetChildCount(false) != 0)
				{
					return this._potionRow.GetChild<Control>(this._potionRow.GetChildCount(false) / 2, false);
				}
				return null;
			}
		}

		/// <summary>
		/// Get the method information for all the methods declared in this class.
		/// This method is used by Godot to register the available methods in the editor.
		/// Do not call this method.
		/// </summary>
		// Token: 0x060020BF RID: 8383 RVA: 0x000DE0FC File Offset: 0x000DC2FC
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal new static List<MethodInfo> GetGodotMethodList()
		{
			return new List<MethodInfo>(4)
			{
				new MethodInfo(NUnlockPotionsScreen.MethodName.Create, new PropertyInfo(24L, "", 0L, "", 6L, new StringName("Control"), false), 33L, null, null),
				new MethodInfo(NUnlockPotionsScreen.MethodName._Ready, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NUnlockPotionsScreen.MethodName.Open, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NUnlockPotionsScreen.MethodName.OnScreenClose, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null)
			};
		}

		/// <inheritdoc />
		// Token: 0x060020C0 RID: 8384 RVA: 0x000DE1DC File Offset: 0x000DC3DC
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool InvokeGodotClassMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
		{
			if ((ref method) == NUnlockPotionsScreen.MethodName.Create && args.Count == 0)
			{
				NUnlockPotionsScreen nunlockPotionsScreen = NUnlockPotionsScreen.Create();
				ret = VariantUtils.CreateFrom<NUnlockPotionsScreen>(ref nunlockPotionsScreen);
				return true;
			}
			if ((ref method) == NUnlockPotionsScreen.MethodName._Ready && args.Count == 0)
			{
				this._Ready();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NUnlockPotionsScreen.MethodName.Open && args.Count == 0)
			{
				this.Open();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NUnlockPotionsScreen.MethodName.OnScreenClose && args.Count == 0)
			{
				this.OnScreenClose();
				ret = default(godot_variant);
				return true;
			}
			return base.InvokeGodotClassMethod(in method, args, out ret);
		}

		// Token: 0x060020C1 RID: 8385 RVA: 0x000DE28C File Offset: 0x000DC48C
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal static bool InvokeGodotClassStaticMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
		{
			if ((ref method) == NUnlockPotionsScreen.MethodName.Create && args.Count == 0)
			{
				NUnlockPotionsScreen nunlockPotionsScreen = NUnlockPotionsScreen.Create();
				ret = VariantUtils.CreateFrom<NUnlockPotionsScreen>(ref nunlockPotionsScreen);
				return true;
			}
			ret = default(godot_variant);
			return false;
		}

		/// <inheritdoc />
		// Token: 0x060020C2 RID: 8386 RVA: 0x000DE2CC File Offset: 0x000DC4CC
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool HasGodotClassMethod(in godot_string_name method)
		{
			return (ref method) == NUnlockPotionsScreen.MethodName.Create || (ref method) == NUnlockPotionsScreen.MethodName._Ready || (ref method) == NUnlockPotionsScreen.MethodName.Open || (ref method) == NUnlockPotionsScreen.MethodName.OnScreenClose || base.HasGodotClassMethod(in method);
		}

		/// <inheritdoc />
		// Token: 0x060020C3 RID: 8387 RVA: 0x000DE31C File Offset: 0x000DC51C
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool SetGodotClassPropertyValue(in godot_string_name name, in godot_variant value)
		{
			if ((ref name) == NUnlockPotionsScreen.PropertyName._potionRow)
			{
				this._potionRow = VariantUtils.ConvertTo<Control>(ref value);
				return true;
			}
			if ((ref name) == NUnlockPotionsScreen.PropertyName._banner)
			{
				this._banner = VariantUtils.ConvertTo<NCommonBanner>(ref value);
				return true;
			}
			if ((ref name) == NUnlockPotionsScreen.PropertyName._potionTween)
			{
				this._potionTween = VariantUtils.ConvertTo<Tween>(ref value);
				return true;
			}
			return base.SetGodotClassPropertyValue(in name, in value);
		}

		/// <inheritdoc />
		// Token: 0x060020C4 RID: 8388 RVA: 0x000DE384 File Offset: 0x000DC584
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
		{
			if ((ref name) == NUnlockPotionsScreen.PropertyName.DefaultFocusedControl)
			{
				Control defaultFocusedControl = this.DefaultFocusedControl;
				value = VariantUtils.CreateFrom<Control>(ref defaultFocusedControl);
				return true;
			}
			if ((ref name) == NUnlockPotionsScreen.PropertyName._potionRow)
			{
				value = VariantUtils.CreateFrom<Control>(ref this._potionRow);
				return true;
			}
			if ((ref name) == NUnlockPotionsScreen.PropertyName._banner)
			{
				value = VariantUtils.CreateFrom<NCommonBanner>(ref this._banner);
				return true;
			}
			if ((ref name) == NUnlockPotionsScreen.PropertyName._potionTween)
			{
				value = VariantUtils.CreateFrom<Tween>(ref this._potionTween);
				return true;
			}
			return base.GetGodotClassPropertyValue(in name, out value);
		}

		/// <summary>
		/// Get the property information for all the properties declared in this class.
		/// This method is used by Godot to register the available properties in the editor.
		/// Do not call this method.
		/// </summary>
		// Token: 0x060020C5 RID: 8389 RVA: 0x000DE41C File Offset: 0x000DC61C
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal new static List<PropertyInfo> GetGodotPropertyList()
		{
			return new List<PropertyInfo>
			{
				new PropertyInfo(24L, NUnlockPotionsScreen.PropertyName._potionRow, 0L, "", 4096L, false),
				new PropertyInfo(24L, NUnlockPotionsScreen.PropertyName._banner, 0L, "", 4096L, false),
				new PropertyInfo(24L, NUnlockPotionsScreen.PropertyName._potionTween, 0L, "", 4096L, false),
				new PropertyInfo(24L, NUnlockPotionsScreen.PropertyName.DefaultFocusedControl, 0L, "", 4096L, false)
			};
		}

		/// <inheritdoc />
		// Token: 0x060020C6 RID: 8390 RVA: 0x000DE4B4 File Offset: 0x000DC6B4
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void SaveGodotObjectData(GodotSerializationInfo info)
		{
			base.SaveGodotObjectData(info);
			info.AddProperty(NUnlockPotionsScreen.PropertyName._potionRow, Variant.From<Control>(ref this._potionRow));
			info.AddProperty(NUnlockPotionsScreen.PropertyName._banner, Variant.From<NCommonBanner>(ref this._banner));
			info.AddProperty(NUnlockPotionsScreen.PropertyName._potionTween, Variant.From<Tween>(ref this._potionTween));
		}

		/// <inheritdoc />
		// Token: 0x060020C7 RID: 8391 RVA: 0x000DE50C File Offset: 0x000DC70C
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void RestoreGodotObjectData(GodotSerializationInfo info)
		{
			base.RestoreGodotObjectData(info);
			Variant variant;
			if (info.TryGetProperty(NUnlockPotionsScreen.PropertyName._potionRow, ref variant))
			{
				this._potionRow = variant.As<Control>();
			}
			Variant variant2;
			if (info.TryGetProperty(NUnlockPotionsScreen.PropertyName._banner, ref variant2))
			{
				this._banner = variant2.As<NCommonBanner>();
			}
			Variant variant3;
			if (info.TryGetProperty(NUnlockPotionsScreen.PropertyName._potionTween, ref variant3))
			{
				this._potionTween = variant3.As<Tween>();
			}
		}

		// Token: 0x04000CC1 RID: 3265
		[Nullable(1)]
		private static readonly string _scenePath = SceneHelper.GetScenePath("timeline_screen/unlock_potions_screen");

		// Token: 0x04000CC2 RID: 3266
		[Nullable(1)]
		private Control _potionRow;

		// Token: 0x04000CC3 RID: 3267
		[Nullable(1)]
		private NCommonBanner _banner;

		// Token: 0x04000CC4 RID: 3268
		[Nullable(1)]
		private IReadOnlyList<PotionModel> _potions;

		// Token: 0x04000CC5 RID: 3269
		[Nullable(2)]
		private Tween _potionTween;

		// Token: 0x04000CC6 RID: 3270
		private const float _potionXOffset = 350f;

		// Token: 0x04000CC7 RID: 3271
		private static readonly Vector2 _potionScale = Vector2.One * 3f;

		/// <summary>
		/// Cached StringNames for the methods contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x020012E5 RID: 4837
		public new class MethodName : NUnlockScreen.MethodName
		{
			/// <summary>
			/// Cached name for the 'Create' method.
			/// </summary>
			// Token: 0x0400499A RID: 18842
			public static readonly StringName Create = "Create";

			/// <summary>
			/// Cached name for the '_Ready' method.
			/// </summary>
			// Token: 0x0400499B RID: 18843
			public new static readonly StringName _Ready = "_Ready";

			/// <summary>
			/// Cached name for the 'Open' method.
			/// </summary>
			// Token: 0x0400499C RID: 18844
			public new static readonly StringName Open = "Open";

			/// <summary>
			/// Cached name for the 'OnScreenClose' method.
			/// </summary>
			// Token: 0x0400499D RID: 18845
			public new static readonly StringName OnScreenClose = "OnScreenClose";
		}

		/// <summary>
		/// Cached StringNames for the properties and fields contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x020012E6 RID: 4838
		public new class PropertyName : NUnlockScreen.PropertyName
		{
			/// <summary>
			/// Cached name for the 'DefaultFocusedControl' property.
			/// </summary>
			// Token: 0x0400499E RID: 18846
			public new static readonly StringName DefaultFocusedControl = "DefaultFocusedControl";

			/// <summary>
			/// Cached name for the '_potionRow' field.
			/// </summary>
			// Token: 0x0400499F RID: 18847
			public static readonly StringName _potionRow = "_potionRow";

			/// <summary>
			/// Cached name for the '_banner' field.
			/// </summary>
			// Token: 0x040049A0 RID: 18848
			public static readonly StringName _banner = "_banner";

			/// <summary>
			/// Cached name for the '_potionTween' field.
			/// </summary>
			// Token: 0x040049A1 RID: 18849
			public static readonly StringName _potionTween = "_potionTween";
		}

		/// <summary>
		/// Cached StringNames for the signals contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x020012E7 RID: 4839
		public new class SignalName : NUnlockScreen.SignalName
		{
		}
	}
}
