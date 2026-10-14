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
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Nodes.GodotExtensions;

namespace MegaCrit.Sts2.Core.Nodes.Screens.Bestiary
{
	/// <summary>
	/// The buttons which are in the Action list for the monsters in the Bestiary.
	/// Click on these buttons to play the associated animation: Attack, Hurt, Die, etc.
	/// </summary>
	// Token: 0x02000366 RID: 870
	[ScriptPath("res://src/Core/Nodes/Screens/Bestiary/NBestiaryMoveButton.cs")]
	public class NBestiaryMoveButton : NButton
	{
		// Token: 0x170007E4 RID: 2020
		// (get) Token: 0x060032A0 RID: 12960 RVA: 0x00179100 File Offset: 0x00177300
		[Nullable(2)]
		protected override string ClickedSfx
		{
			[NullableContext(2)]
			get
			{
				return null;
			}
		}

		// Token: 0x170007E5 RID: 2021
		// (get) Token: 0x060032A1 RID: 12961 RVA: 0x00179103 File Offset: 0x00177303
		[Nullable(2)]
		protected override string HoveredSfx
		{
			[NullableContext(2)]
			get
			{
				return null;
			}
		}

		// Token: 0x170007E6 RID: 2022
		// (get) Token: 0x060032A2 RID: 12962 RVA: 0x00179106 File Offset: 0x00177306
		// (set) Token: 0x060032A3 RID: 12963 RVA: 0x0017910E File Offset: 0x0017730E
		public BestiaryMonsterMove Move { get; private set; }

		// Token: 0x170007E7 RID: 2023
		// (get) Token: 0x060032A4 RID: 12964 RVA: 0x00179117 File Offset: 0x00177317
		[Nullable(1)]
		protected override string[] Hotkeys
		{
			[NullableContext(1)]
			get
			{
				return this._hotkeys;
			}
		}

		// Token: 0x060032A5 RID: 12965 RVA: 0x00179120 File Offset: 0x00177320
		public override void _Ready()
		{
			this.ConnectSignals();
			this._label = base.GetNode<MegaLabel>("%Label");
			this._buttonAnimator = base.GetNode<Control>("%ButtonAnimator");
			this._label.SetTextAutoSize(this.Move.displayName);
			this._label.PivotOffset = new Vector2(0f, this._label.Size.Y * 0.5f);
			this._buttonAnimator.PivotOffset = new Vector2(0f, this._buttonAnimator.Size.Y * 0.5f);
		}

		// Token: 0x060032A6 RID: 12966 RVA: 0x001791CC File Offset: 0x001773CC
		[NullableContext(1)]
		public static NBestiaryMoveButton Create(BestiaryMonsterMove move, StringName setHotkey)
		{
			NBestiaryMoveButton nbestiaryMoveButton = PreloadManager.Cache.GetAsset<PackedScene>(NBestiaryMoveButton._scenePath).Instantiate<NBestiaryMoveButton>(0L);
			nbestiaryMoveButton._hotkeys = new string[] { setHotkey };
			nbestiaryMoveButton.Move = move;
			return nbestiaryMoveButton;
		}

		// Token: 0x060032A7 RID: 12967 RVA: 0x00179210 File Offset: 0x00177410
		protected override void OnRelease()
		{
			base.OnRelease();
			this._label.Modulate = StsColors.green;
			Tween clickTween = this._clickTween;
			if (clickTween != null)
			{
				clickTween.Kill();
			}
			this._clickTween = base.CreateTween().SetParallel(true);
			this._clickTween.TweenProperty(this._buttonAnimator, "scale", Vector2.One, 0.3).SetEase(1L).SetTrans(9L);
			this._clickTween.TweenProperty(this._label, "modulate", StsColors.cream, 0.2);
		}

		// Token: 0x060032A8 RID: 12968 RVA: 0x001792C4 File Offset: 0x001774C4
		protected override void OnPress()
		{
			base.OnPress();
			Tween clickTween = this._clickTween;
			if (clickTween != null)
			{
				clickTween.Kill();
			}
			this._clickTween = base.CreateTween().SetParallel(true);
			this._clickTween.TweenProperty(this._buttonAnimator, "scale", Vector2.One * 0.9f, 0.2).SetEase(1L).SetTrans(5L);
			this._clickTween.TweenProperty(this._label, "modulate", StsColors.lightGray, 0.05);
		}

		// Token: 0x060032A9 RID: 12969 RVA: 0x00179370 File Offset: 0x00177570
		protected override void OnFocus()
		{
			base.OnFocus();
			Tween tween = this._tween;
			if (tween != null)
			{
				tween.Kill();
			}
			this._tween = base.CreateTween().SetParallel(true);
			this._tween.TweenProperty(this._label, "scale", Vector2.One * 1.05f, 0.05);
		}

		// Token: 0x060032AA RID: 12970 RVA: 0x001793E0 File Offset: 0x001775E0
		protected override void OnUnfocus()
		{
			Tween tween = this._tween;
			if (tween != null)
			{
				tween.Kill();
			}
			this._tween = base.CreateTween().SetParallel(true);
			this._tween.TweenProperty(this._label, "scale", Vector2.One, 0.25).SetEase(1L).SetTrans(7L);
			Tween clickTween = this._clickTween;
			if (clickTween != null)
			{
				clickTween.Kill();
			}
			this._clickTween = base.CreateTween().SetParallel(true);
			this._clickTween.TweenProperty(this._buttonAnimator, "scale", Vector2.One, 0.1).SetEase(1L).SetTrans(5L);
			this._clickTween.TweenProperty(this._label, "modulate", StsColors.cream, 0.1);
		}

		/// <summary>
		/// Get the method information for all the methods declared in this class.
		/// This method is used by Godot to register the available methods in the editor.
		/// Do not call this method.
		/// </summary>
		// Token: 0x060032AB RID: 12971 RVA: 0x001794DC File Offset: 0x001776DC
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal new static List<MethodInfo> GetGodotMethodList()
		{
			return new List<MethodInfo>(5)
			{
				new MethodInfo(NBestiaryMoveButton.MethodName._Ready, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NBestiaryMoveButton.MethodName.OnRelease, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NBestiaryMoveButton.MethodName.OnPress, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NBestiaryMoveButton.MethodName.OnFocus, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NBestiaryMoveButton.MethodName.OnUnfocus, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null)
			};
		}

		/// <inheritdoc />
		// Token: 0x060032AC RID: 12972 RVA: 0x001795DC File Offset: 0x001777DC
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool InvokeGodotClassMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
		{
			if ((ref method) == NBestiaryMoveButton.MethodName._Ready && args.Count == 0)
			{
				this._Ready();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NBestiaryMoveButton.MethodName.OnRelease && args.Count == 0)
			{
				this.OnRelease();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NBestiaryMoveButton.MethodName.OnPress && args.Count == 0)
			{
				this.OnPress();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NBestiaryMoveButton.MethodName.OnFocus && args.Count == 0)
			{
				this.OnFocus();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NBestiaryMoveButton.MethodName.OnUnfocus && args.Count == 0)
			{
				this.OnUnfocus();
				ret = default(godot_variant);
				return true;
			}
			return base.InvokeGodotClassMethod(in method, args, out ret);
		}

		/// <inheritdoc />
		// Token: 0x060032AD RID: 12973 RVA: 0x001796AC File Offset: 0x001778AC
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool HasGodotClassMethod(in godot_string_name method)
		{
			return (ref method) == NBestiaryMoveButton.MethodName._Ready || (ref method) == NBestiaryMoveButton.MethodName.OnRelease || (ref method) == NBestiaryMoveButton.MethodName.OnPress || (ref method) == NBestiaryMoveButton.MethodName.OnFocus || (ref method) == NBestiaryMoveButton.MethodName.OnUnfocus || base.HasGodotClassMethod(in method);
		}

		/// <inheritdoc />
		// Token: 0x060032AE RID: 12974 RVA: 0x0017970C File Offset: 0x0017790C
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool SetGodotClassPropertyValue(in godot_string_name name, in godot_variant value)
		{
			if ((ref name) == NBestiaryMoveButton.PropertyName._buttonAnimator)
			{
				this._buttonAnimator = VariantUtils.ConvertTo<Control>(ref value);
				return true;
			}
			if ((ref name) == NBestiaryMoveButton.PropertyName._label)
			{
				this._label = VariantUtils.ConvertTo<MegaLabel>(ref value);
				return true;
			}
			if ((ref name) == NBestiaryMoveButton.PropertyName._tween)
			{
				this._tween = VariantUtils.ConvertTo<Tween>(ref value);
				return true;
			}
			if ((ref name) == NBestiaryMoveButton.PropertyName._clickTween)
			{
				this._clickTween = VariantUtils.ConvertTo<Tween>(ref value);
				return true;
			}
			if ((ref name) == NBestiaryMoveButton.PropertyName._hotkeys)
			{
				this._hotkeys = VariantUtils.ConvertTo<string[]>(ref value);
				return true;
			}
			return base.SetGodotClassPropertyValue(in name, in value);
		}

		/// <inheritdoc />
		// Token: 0x060032AF RID: 12975 RVA: 0x001797A8 File Offset: 0x001779A8
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
		{
			if ((ref name) == NBestiaryMoveButton.PropertyName.ClickedSfx)
			{
				string text = this.ClickedSfx;
				value = VariantUtils.CreateFrom<string>(ref text);
				return true;
			}
			if ((ref name) == NBestiaryMoveButton.PropertyName.HoveredSfx)
			{
				string text = this.HoveredSfx;
				value = VariantUtils.CreateFrom<string>(ref text);
				return true;
			}
			if ((ref name) == NBestiaryMoveButton.PropertyName.Hotkeys)
			{
				string[] hotkeys = this.Hotkeys;
				value = VariantUtils.CreateFrom<string[]>(ref hotkeys);
				return true;
			}
			if ((ref name) == NBestiaryMoveButton.PropertyName._buttonAnimator)
			{
				value = VariantUtils.CreateFrom<Control>(ref this._buttonAnimator);
				return true;
			}
			if ((ref name) == NBestiaryMoveButton.PropertyName._label)
			{
				value = VariantUtils.CreateFrom<MegaLabel>(ref this._label);
				return true;
			}
			if ((ref name) == NBestiaryMoveButton.PropertyName._tween)
			{
				value = VariantUtils.CreateFrom<Tween>(ref this._tween);
				return true;
			}
			if ((ref name) == NBestiaryMoveButton.PropertyName._clickTween)
			{
				value = VariantUtils.CreateFrom<Tween>(ref this._clickTween);
				return true;
			}
			if ((ref name) == NBestiaryMoveButton.PropertyName._hotkeys)
			{
				value = VariantUtils.CreateFrom<string[]>(ref this._hotkeys);
				return true;
			}
			return base.GetGodotClassPropertyValue(in name, out value);
		}

		/// <summary>
		/// Get the property information for all the properties declared in this class.
		/// This method is used by Godot to register the available properties in the editor.
		/// Do not call this method.
		/// </summary>
		// Token: 0x060032B0 RID: 12976 RVA: 0x001798C8 File Offset: 0x00177AC8
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal new static List<PropertyInfo> GetGodotPropertyList()
		{
			return new List<PropertyInfo>
			{
				new PropertyInfo(4L, NBestiaryMoveButton.PropertyName.ClickedSfx, 0L, "", 4096L, false),
				new PropertyInfo(4L, NBestiaryMoveButton.PropertyName.HoveredSfx, 0L, "", 4096L, false),
				new PropertyInfo(24L, NBestiaryMoveButton.PropertyName._buttonAnimator, 0L, "", 4096L, false),
				new PropertyInfo(24L, NBestiaryMoveButton.PropertyName._label, 0L, "", 4096L, false),
				new PropertyInfo(24L, NBestiaryMoveButton.PropertyName._tween, 0L, "", 4096L, false),
				new PropertyInfo(24L, NBestiaryMoveButton.PropertyName._clickTween, 0L, "", 4096L, false),
				new PropertyInfo(34L, NBestiaryMoveButton.PropertyName._hotkeys, 0L, "", 4096L, false),
				new PropertyInfo(34L, NBestiaryMoveButton.PropertyName.Hotkeys, 0L, "", 4096L, false)
			};
		}

		/// <inheritdoc />
		// Token: 0x060032B1 RID: 12977 RVA: 0x001799E4 File Offset: 0x00177BE4
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void SaveGodotObjectData(GodotSerializationInfo info)
		{
			base.SaveGodotObjectData(info);
			info.AddProperty(NBestiaryMoveButton.PropertyName._buttonAnimator, Variant.From<Control>(ref this._buttonAnimator));
			info.AddProperty(NBestiaryMoveButton.PropertyName._label, Variant.From<MegaLabel>(ref this._label));
			info.AddProperty(NBestiaryMoveButton.PropertyName._tween, Variant.From<Tween>(ref this._tween));
			info.AddProperty(NBestiaryMoveButton.PropertyName._clickTween, Variant.From<Tween>(ref this._clickTween));
			info.AddProperty(NBestiaryMoveButton.PropertyName._hotkeys, Variant.From<string[]>(ref this._hotkeys));
		}

		/// <inheritdoc />
		// Token: 0x060032B2 RID: 12978 RVA: 0x00179A68 File Offset: 0x00177C68
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void RestoreGodotObjectData(GodotSerializationInfo info)
		{
			base.RestoreGodotObjectData(info);
			Variant variant;
			if (info.TryGetProperty(NBestiaryMoveButton.PropertyName._buttonAnimator, ref variant))
			{
				this._buttonAnimator = variant.As<Control>();
			}
			Variant variant2;
			if (info.TryGetProperty(NBestiaryMoveButton.PropertyName._label, ref variant2))
			{
				this._label = variant2.As<MegaLabel>();
			}
			Variant variant3;
			if (info.TryGetProperty(NBestiaryMoveButton.PropertyName._tween, ref variant3))
			{
				this._tween = variant3.As<Tween>();
			}
			Variant variant4;
			if (info.TryGetProperty(NBestiaryMoveButton.PropertyName._clickTween, ref variant4))
			{
				this._clickTween = variant4.As<Tween>();
			}
			Variant variant5;
			if (info.TryGetProperty(NBestiaryMoveButton.PropertyName._hotkeys, ref variant5))
			{
				this._hotkeys = variant5.As<string[]>();
			}
		}

		// Token: 0x040013B2 RID: 5042
		[Nullable(1)]
		private static readonly string _scenePath = SceneHelper.GetScenePath("screens/bestiary/bestiary_move_button");

		// Token: 0x040013B4 RID: 5044
		[Nullable(1)]
		private Control _buttonAnimator;

		// Token: 0x040013B5 RID: 5045
		[Nullable(1)]
		private MegaLabel _label;

		// Token: 0x040013B6 RID: 5046
		[Nullable(2)]
		private Tween _tween;

		// Token: 0x040013B7 RID: 5047
		[Nullable(2)]
		private Tween _clickTween;

		// Token: 0x040013B8 RID: 5048
		[Nullable(1)]
		private string[] _hotkeys;

		/// <summary>
		/// Cached StringNames for the methods contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x02001674 RID: 5748
		public new class MethodName : NButton.MethodName
		{
			/// <summary>
			/// Cached name for the '_Ready' method.
			/// </summary>
			// Token: 0x04005782 RID: 22402
			public new static readonly StringName _Ready = "_Ready";

			/// <summary>
			/// Cached name for the 'OnRelease' method.
			/// </summary>
			// Token: 0x04005783 RID: 22403
			public new static readonly StringName OnRelease = "OnRelease";

			/// <summary>
			/// Cached name for the 'OnPress' method.
			/// </summary>
			// Token: 0x04005784 RID: 22404
			public new static readonly StringName OnPress = "OnPress";

			/// <summary>
			/// Cached name for the 'OnFocus' method.
			/// </summary>
			// Token: 0x04005785 RID: 22405
			public new static readonly StringName OnFocus = "OnFocus";

			/// <summary>
			/// Cached name for the 'OnUnfocus' method.
			/// </summary>
			// Token: 0x04005786 RID: 22406
			public new static readonly StringName OnUnfocus = "OnUnfocus";
		}

		/// <summary>
		/// Cached StringNames for the properties and fields contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x02001675 RID: 5749
		public new class PropertyName : NButton.PropertyName
		{
			/// <summary>
			/// Cached name for the 'ClickedSfx' property.
			/// </summary>
			// Token: 0x04005787 RID: 22407
			public new static readonly StringName ClickedSfx = "ClickedSfx";

			/// <summary>
			/// Cached name for the 'HoveredSfx' property.
			/// </summary>
			// Token: 0x04005788 RID: 22408
			public new static readonly StringName HoveredSfx = "HoveredSfx";

			/// <summary>
			/// Cached name for the 'Hotkeys' property.
			/// </summary>
			// Token: 0x04005789 RID: 22409
			public new static readonly StringName Hotkeys = "Hotkeys";

			/// <summary>
			/// Cached name for the '_buttonAnimator' field.
			/// </summary>
			// Token: 0x0400578A RID: 22410
			public static readonly StringName _buttonAnimator = "_buttonAnimator";

			/// <summary>
			/// Cached name for the '_label' field.
			/// </summary>
			// Token: 0x0400578B RID: 22411
			public static readonly StringName _label = "_label";

			/// <summary>
			/// Cached name for the '_tween' field.
			/// </summary>
			// Token: 0x0400578C RID: 22412
			public static readonly StringName _tween = "_tween";

			/// <summary>
			/// Cached name for the '_clickTween' field.
			/// </summary>
			// Token: 0x0400578D RID: 22413
			public static readonly StringName _clickTween = "_clickTween";

			/// <summary>
			/// Cached name for the '_hotkeys' field.
			/// </summary>
			// Token: 0x0400578E RID: 22414
			public static readonly StringName _hotkeys = "_hotkeys";
		}

		/// <summary>
		/// Cached StringNames for the signals contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x02001676 RID: 5750
		public new class SignalName : NButton.SignalName
		{
		}
	}
}
