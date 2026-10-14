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
using MegaCrit.Sts2.Core.Nodes.Combat;
using MegaCrit.Sts2.Core.Timeline;

namespace MegaCrit.Sts2.Core.Nodes.Screens.Timeline.UnlockScreens
{
	/// <summary>
	/// Unlock screen which appears after unlocking any new character
	/// </summary>
	// Token: 0x0200026A RID: 618
	[NullableContext(1)]
	[Nullable(0)]
	[ScriptPath("res://src/Core/Nodes/Screens/Timeline/UnlockScreens/NUnlockCharacterScreen.cs")]
	public class NUnlockCharacterScreen : NUnlockScreen
	{
		// Token: 0x17000690 RID: 1680
		// (get) Token: 0x0600208A RID: 8330 RVA: 0x000DC95C File Offset: 0x000DAB5C
		public static IEnumerable<string> AssetPaths
		{
			get
			{
				return new <>z__ReadOnlySingleElementList<string>(NUnlockCharacterScreen._scenePath);
			}
		}

		// Token: 0x0600208B RID: 8331 RVA: 0x000DC968 File Offset: 0x000DAB68
		public static NUnlockCharacterScreen Create(EpochModel epoch, CharacterModel character)
		{
			NUnlockCharacterScreen nunlockCharacterScreen = PreloadManager.Cache.GetScene(NUnlockCharacterScreen._scenePath).Instantiate<NUnlockCharacterScreen>(0L);
			nunlockCharacterScreen._character = character;
			nunlockCharacterScreen._epoch = epoch;
			return nunlockCharacterScreen;
		}

		// Token: 0x0600208C RID: 8332 RVA: 0x000DC99C File Offset: 0x000DAB9C
		public override void _Ready()
		{
			base.ConnectSignals();
			this._topLabel = base.GetNode<MegaRichTextLabel>("%TopLabel");
			this._bottomLabel = base.GetNode<MegaRichTextLabel>("%BottomLabel");
			this._spineAnchor = base.GetNode<Control>("%SpineAnchor");
			this._rareGlow = base.GetNode<GpuParticles2D>("%RareGlow");
			this._topLabel.Text = new LocString("epochs", this._epoch.Id + ".unlock").GetFormattedText();
			this._bottomLabel.Text = new LocString("epochs", this._epoch.Id + ".unlockText").GetFormattedText();
			this._topLabel.Modulate = StsColors.transparentBlack;
			this._bottomLabel.Modulate = StsColors.transparentBlack;
			this._spineAnchor.Modulate = StsColors.transparentBlack;
			this._creatureVisuals = this._character.CreateVisuals();
			this._spineAnchor.AddChildSafely(this._creatureVisuals);
		}

		// Token: 0x0600208D RID: 8333 RVA: 0x000DCAB8 File Offset: 0x000DACB8
		public override void Open()
		{
			base.Open();
			SfxCmd.Play("event:/sfx/ui/timeline/ui_timeline_unlock", 1f);
			this._tween = base.CreateTween().SetParallel(true);
			this._tween.TweenProperty(this._spineAnchor, "modulate", Colors.White, 0.25).SetDelay(0.25);
			this._tween.TweenProperty(this._topLabel, "modulate", Colors.White, 1.0).SetDelay(1.0);
			this._tween.TweenProperty(this._bottomLabel, "modulate", Colors.White, 1.0).SetDelay(1.5);
			this._tween.TweenProperty(this._rareGlow, "modulate:a", 1f, 0.5).SetDelay(1.0);
			this._creatureVisuals.SpineAnimation.AddAnimation("idle_loop", 0f, true, 0);
			this._creatureVisuals.SpineAnimation.AddAnimation("attack", 0.5f, false, 0);
			this._creatureVisuals.SpineAnimation.AddAnimation("idle_loop", 0f, true, 0);
		}

		// Token: 0x0600208E RID: 8334 RVA: 0x000DCC40 File Offset: 0x000DAE40
		protected override void OnScreenPreClose()
		{
			Tween tween = this._tween;
			if (tween != null)
			{
				tween.Kill();
			}
			this._tween = base.CreateTween();
			this._tween.TweenProperty(this._rareGlow, "modulate:a", 0f, 0.5);
		}

		// Token: 0x0600208F RID: 8335 RVA: 0x000DCC99 File Offset: 0x000DAE99
		protected override void OnScreenClose()
		{
			NTimelineScreen.Instance.EnableInput();
		}

		/// <summary>
		/// Get the method information for all the methods declared in this class.
		/// This method is used by Godot to register the available methods in the editor.
		/// Do not call this method.
		/// </summary>
		// Token: 0x06002090 RID: 8336 RVA: 0x000DCCA8 File Offset: 0x000DAEA8
		[NullableContext(0)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal new static List<MethodInfo> GetGodotMethodList()
		{
			return new List<MethodInfo>(4)
			{
				new MethodInfo(NUnlockCharacterScreen.MethodName._Ready, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NUnlockCharacterScreen.MethodName.Open, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NUnlockCharacterScreen.MethodName.OnScreenPreClose, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NUnlockCharacterScreen.MethodName.OnScreenClose, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null)
			};
		}

		/// <inheritdoc />
		// Token: 0x06002091 RID: 8337 RVA: 0x000DCD7C File Offset: 0x000DAF7C
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool InvokeGodotClassMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
		{
			if ((ref method) == NUnlockCharacterScreen.MethodName._Ready && args.Count == 0)
			{
				this._Ready();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NUnlockCharacterScreen.MethodName.Open && args.Count == 0)
			{
				this.Open();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NUnlockCharacterScreen.MethodName.OnScreenPreClose && args.Count == 0)
			{
				this.OnScreenPreClose();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NUnlockCharacterScreen.MethodName.OnScreenClose && args.Count == 0)
			{
				this.OnScreenClose();
				ret = default(godot_variant);
				return true;
			}
			return base.InvokeGodotClassMethod(in method, args, out ret);
		}

		/// <inheritdoc />
		// Token: 0x06002092 RID: 8338 RVA: 0x000DCE28 File Offset: 0x000DB028
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool HasGodotClassMethod(in godot_string_name method)
		{
			return (ref method) == NUnlockCharacterScreen.MethodName._Ready || (ref method) == NUnlockCharacterScreen.MethodName.Open || (ref method) == NUnlockCharacterScreen.MethodName.OnScreenPreClose || (ref method) == NUnlockCharacterScreen.MethodName.OnScreenClose || base.HasGodotClassMethod(in method);
		}

		/// <inheritdoc />
		// Token: 0x06002093 RID: 8339 RVA: 0x000DCE78 File Offset: 0x000DB078
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool SetGodotClassPropertyValue(in godot_string_name name, in godot_variant value)
		{
			if ((ref name) == NUnlockCharacterScreen.PropertyName._topLabel)
			{
				this._topLabel = VariantUtils.ConvertTo<MegaRichTextLabel>(ref value);
				return true;
			}
			if ((ref name) == NUnlockCharacterScreen.PropertyName._bottomLabel)
			{
				this._bottomLabel = VariantUtils.ConvertTo<MegaRichTextLabel>(ref value);
				return true;
			}
			if ((ref name) == NUnlockCharacterScreen.PropertyName._spineAnchor)
			{
				this._spineAnchor = VariantUtils.ConvertTo<Control>(ref value);
				return true;
			}
			if ((ref name) == NUnlockCharacterScreen.PropertyName._creatureVisuals)
			{
				this._creatureVisuals = VariantUtils.ConvertTo<NCreatureVisuals>(ref value);
				return true;
			}
			if ((ref name) == NUnlockCharacterScreen.PropertyName._rareGlow)
			{
				this._rareGlow = VariantUtils.ConvertTo<GpuParticles2D>(ref value);
				return true;
			}
			if ((ref name) == NUnlockCharacterScreen.PropertyName._tween)
			{
				this._tween = VariantUtils.ConvertTo<Tween>(ref value);
				return true;
			}
			return base.SetGodotClassPropertyValue(in name, in value);
		}

		/// <inheritdoc />
		// Token: 0x06002094 RID: 8340 RVA: 0x000DCF30 File Offset: 0x000DB130
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
		{
			if ((ref name) == NUnlockCharacterScreen.PropertyName._topLabel)
			{
				value = VariantUtils.CreateFrom<MegaRichTextLabel>(ref this._topLabel);
				return true;
			}
			if ((ref name) == NUnlockCharacterScreen.PropertyName._bottomLabel)
			{
				value = VariantUtils.CreateFrom<MegaRichTextLabel>(ref this._bottomLabel);
				return true;
			}
			if ((ref name) == NUnlockCharacterScreen.PropertyName._spineAnchor)
			{
				value = VariantUtils.CreateFrom<Control>(ref this._spineAnchor);
				return true;
			}
			if ((ref name) == NUnlockCharacterScreen.PropertyName._creatureVisuals)
			{
				value = VariantUtils.CreateFrom<NCreatureVisuals>(ref this._creatureVisuals);
				return true;
			}
			if ((ref name) == NUnlockCharacterScreen.PropertyName._rareGlow)
			{
				value = VariantUtils.CreateFrom<GpuParticles2D>(ref this._rareGlow);
				return true;
			}
			if ((ref name) == NUnlockCharacterScreen.PropertyName._tween)
			{
				value = VariantUtils.CreateFrom<Tween>(ref this._tween);
				return true;
			}
			return base.GetGodotClassPropertyValue(in name, out value);
		}

		/// <summary>
		/// Get the property information for all the properties declared in this class.
		/// This method is used by Godot to register the available properties in the editor.
		/// Do not call this method.
		/// </summary>
		// Token: 0x06002095 RID: 8341 RVA: 0x000DD008 File Offset: 0x000DB208
		[NullableContext(0)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal new static List<PropertyInfo> GetGodotPropertyList()
		{
			return new List<PropertyInfo>
			{
				new PropertyInfo(24L, NUnlockCharacterScreen.PropertyName._topLabel, 0L, "", 4096L, false),
				new PropertyInfo(24L, NUnlockCharacterScreen.PropertyName._bottomLabel, 0L, "", 4096L, false),
				new PropertyInfo(24L, NUnlockCharacterScreen.PropertyName._spineAnchor, 0L, "", 4096L, false),
				new PropertyInfo(24L, NUnlockCharacterScreen.PropertyName._creatureVisuals, 0L, "", 4096L, false),
				new PropertyInfo(24L, NUnlockCharacterScreen.PropertyName._rareGlow, 0L, "", 4096L, false),
				new PropertyInfo(24L, NUnlockCharacterScreen.PropertyName._tween, 0L, "", 4096L, false)
			};
		}

		/// <inheritdoc />
		// Token: 0x06002096 RID: 8342 RVA: 0x000DD0E4 File Offset: 0x000DB2E4
		[NullableContext(0)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void SaveGodotObjectData(GodotSerializationInfo info)
		{
			base.SaveGodotObjectData(info);
			info.AddProperty(NUnlockCharacterScreen.PropertyName._topLabel, Variant.From<MegaRichTextLabel>(ref this._topLabel));
			info.AddProperty(NUnlockCharacterScreen.PropertyName._bottomLabel, Variant.From<MegaRichTextLabel>(ref this._bottomLabel));
			info.AddProperty(NUnlockCharacterScreen.PropertyName._spineAnchor, Variant.From<Control>(ref this._spineAnchor));
			info.AddProperty(NUnlockCharacterScreen.PropertyName._creatureVisuals, Variant.From<NCreatureVisuals>(ref this._creatureVisuals));
			info.AddProperty(NUnlockCharacterScreen.PropertyName._rareGlow, Variant.From<GpuParticles2D>(ref this._rareGlow));
			info.AddProperty(NUnlockCharacterScreen.PropertyName._tween, Variant.From<Tween>(ref this._tween));
		}

		/// <inheritdoc />
		// Token: 0x06002097 RID: 8343 RVA: 0x000DD17C File Offset: 0x000DB37C
		[NullableContext(0)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void RestoreGodotObjectData(GodotSerializationInfo info)
		{
			base.RestoreGodotObjectData(info);
			Variant variant;
			if (info.TryGetProperty(NUnlockCharacterScreen.PropertyName._topLabel, ref variant))
			{
				this._topLabel = variant.As<MegaRichTextLabel>();
			}
			Variant variant2;
			if (info.TryGetProperty(NUnlockCharacterScreen.PropertyName._bottomLabel, ref variant2))
			{
				this._bottomLabel = variant2.As<MegaRichTextLabel>();
			}
			Variant variant3;
			if (info.TryGetProperty(NUnlockCharacterScreen.PropertyName._spineAnchor, ref variant3))
			{
				this._spineAnchor = variant3.As<Control>();
			}
			Variant variant4;
			if (info.TryGetProperty(NUnlockCharacterScreen.PropertyName._creatureVisuals, ref variant4))
			{
				this._creatureVisuals = variant4.As<NCreatureVisuals>();
			}
			Variant variant5;
			if (info.TryGetProperty(NUnlockCharacterScreen.PropertyName._rareGlow, ref variant5))
			{
				this._rareGlow = variant5.As<GpuParticles2D>();
			}
			Variant variant6;
			if (info.TryGetProperty(NUnlockCharacterScreen.PropertyName._tween, ref variant6))
			{
				this._tween = variant6.As<Tween>();
			}
		}

		// Token: 0x04000CB0 RID: 3248
		private static readonly string _scenePath = SceneHelper.GetScenePath("timeline_screen/unlock_character_screen");

		// Token: 0x04000CB1 RID: 3249
		private MegaRichTextLabel _topLabel;

		// Token: 0x04000CB2 RID: 3250
		private MegaRichTextLabel _bottomLabel;

		// Token: 0x04000CB3 RID: 3251
		private Control _spineAnchor;

		// Token: 0x04000CB4 RID: 3252
		private NCreatureVisuals _creatureVisuals;

		// Token: 0x04000CB5 RID: 3253
		private GpuParticles2D _rareGlow;

		// Token: 0x04000CB6 RID: 3254
		private EpochModel _epoch;

		// Token: 0x04000CB7 RID: 3255
		private CharacterModel _character;

		// Token: 0x04000CB8 RID: 3256
		[Nullable(2)]
		private Tween _tween;

		/// <summary>
		/// Cached StringNames for the methods contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x020012DC RID: 4828
		[NullableContext(0)]
		public new class MethodName : NUnlockScreen.MethodName
		{
			/// <summary>
			/// Cached name for the '_Ready' method.
			/// </summary>
			// Token: 0x04004983 RID: 18819
			public new static readonly StringName _Ready = "_Ready";

			/// <summary>
			/// Cached name for the 'Open' method.
			/// </summary>
			// Token: 0x04004984 RID: 18820
			public new static readonly StringName Open = "Open";

			/// <summary>
			/// Cached name for the 'OnScreenPreClose' method.
			/// </summary>
			// Token: 0x04004985 RID: 18821
			public new static readonly StringName OnScreenPreClose = "OnScreenPreClose";

			/// <summary>
			/// Cached name for the 'OnScreenClose' method.
			/// </summary>
			// Token: 0x04004986 RID: 18822
			public new static readonly StringName OnScreenClose = "OnScreenClose";
		}

		/// <summary>
		/// Cached StringNames for the properties and fields contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x020012DD RID: 4829
		[NullableContext(0)]
		public new class PropertyName : NUnlockScreen.PropertyName
		{
			/// <summary>
			/// Cached name for the '_topLabel' field.
			/// </summary>
			// Token: 0x04004987 RID: 18823
			public static readonly StringName _topLabel = "_topLabel";

			/// <summary>
			/// Cached name for the '_bottomLabel' field.
			/// </summary>
			// Token: 0x04004988 RID: 18824
			public static readonly StringName _bottomLabel = "_bottomLabel";

			/// <summary>
			/// Cached name for the '_spineAnchor' field.
			/// </summary>
			// Token: 0x04004989 RID: 18825
			public static readonly StringName _spineAnchor = "_spineAnchor";

			/// <summary>
			/// Cached name for the '_creatureVisuals' field.
			/// </summary>
			// Token: 0x0400498A RID: 18826
			public static readonly StringName _creatureVisuals = "_creatureVisuals";

			/// <summary>
			/// Cached name for the '_rareGlow' field.
			/// </summary>
			// Token: 0x0400498B RID: 18827
			public static readonly StringName _rareGlow = "_rareGlow";

			/// <summary>
			/// Cached name for the '_tween' field.
			/// </summary>
			// Token: 0x0400498C RID: 18828
			public new static readonly StringName _tween = "_tween";
		}

		/// <summary>
		/// Cached StringNames for the signals contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x020012DE RID: 4830
		[NullableContext(0)]
		public new class SignalName : NUnlockScreen.SignalName
		{
		}
	}
}
