using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Godot;
using Godot.Bridge;
using Godot.NativeInterop;
using MegaCrit.Sts2.addons.mega_text;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Nodes.Combat;
using MegaCrit.Sts2.Core.Nodes.CommonUi;
using MegaCrit.Sts2.Core.Nodes.GodotExtensions;
using MegaCrit.Sts2.Core.Nodes.HoverTips;
using MegaCrit.Sts2.Core.Platform;
using MegaCrit.Sts2.Core.Runs;
using MegaCrit.Sts2.Core.Saves;

namespace MegaCrit.Sts2.Core.Nodes.Screens.RunHistoryScreen
{
	// Token: 0x020002D5 RID: 725
	[NullableContext(1)]
	[Nullable(0)]
	[ScriptPath("res://src/Core/Nodes/Screens/RunHistoryScreen/NRunHistoryPlayerIcon.cs")]
	public class NRunHistoryPlayerIcon : NButton
	{
		// Token: 0x170006D9 RID: 1753
		// (get) Token: 0x06002688 RID: 9864 RVA: 0x00105FC0 File Offset: 0x001041C0
		// (set) Token: 0x06002689 RID: 9865 RVA: 0x00105FC8 File Offset: 0x001041C8
		public RunHistoryPlayer Player { get; private set; }

		// Token: 0x0600268A RID: 9866 RVA: 0x00105FD4 File Offset: 0x001041D4
		public override void _Ready()
		{
			this.ConnectSignals();
			this._achievementLock = base.GetNode<Control>("%AchievementLock");
			this._ascensionIcon = base.GetNode<Control>("%AscensionIcon");
			this._ascensionLabel = base.GetNode<MegaLabel>("%AscensionLabel");
			this._selectionReticle = base.GetNode<NSelectionReticle>("%SelectionReticle");
			this._icon = base.GetNode<TextureRect>("%Icon");
			this._hsv = (ShaderMaterial)this._icon.GetMaterial();
			this.Deselect();
		}

		// Token: 0x0600268B RID: 9867 RVA: 0x00106074 File Offset: 0x00104274
		public void LoadRun(RunHistoryPlayer player, RunHistory history)
		{
			this.Player = player;
			CharacterModel characterModel = SaveUtil.CharacterOrDeprecated(player.Character);
			this._icon.Texture = characterModel.IconTexture;
			LocString locString = new LocString("ascension", "PORTRAIT_TITLE");
			locString.Add("character", characterModel.Title);
			locString.Add("ascension", history.Ascension);
			LocString locString2 = new LocString("ascension", "PORTRAIT_DESCRIPTION");
			List<string> list = new List<string>();
			for (int i = 1; i <= history.Ascension; i++)
			{
				list.Add(AscensionHelper.GetTitle(i).GetFormattedText());
			}
			locString2.Add("ascensions", list);
			this._achievementLock.Visible = history.GameMode.AreAchievementsAndEpochsLocked();
			this._ascensionIcon.Visible = false;
			this._ascensionLabel.SetTextAutoSize((history.Ascension > 0) ? history.Ascension.ToString() : string.Empty);
			LocString locString3 = new LocString("run_history", "PLAYER_HOVER");
			if (history.Players.Count > 1)
			{
				locString3.Add("PlayerName", PlatformUtil.GetPlayerName(history.PlatformType, player.Id));
				locString3.Add("CharacterName", characterModel.Title.GetFormattedText());
			}
			else
			{
				locString3.Add("PlayerName", characterModel.Title.GetFormattedText());
				locString3.Add("CharacterName", string.Empty);
			}
			if (history.Ascension > 0 || history.GameMode.AreAchievementsAndEpochsLocked())
			{
				this._hoverTips.Add(AscensionHelper.GetHoverTip(characterModel, history.Ascension, history.GameMode.AreAchievementsAndEpochsLocked()));
				return;
			}
			this._hoverTips.Add(new HoverTip(locString3, null));
		}

		// Token: 0x0600268C RID: 9868 RVA: 0x00106244 File Offset: 0x00104444
		public void Select()
		{
			this._hsv.SetShaderParameter(NRunHistoryPlayerIcon._s, 1f);
			this._hsv.SetShaderParameter(NRunHistoryPlayerIcon._v, 1f);
			this._ascensionIcon.Visible = this._ascensionLabel.Text != string.Empty;
			Tween tween = this._tween;
			if (tween != null)
			{
				tween.Kill();
			}
			this._tween = base.CreateTween().SetParallel(true);
			this._tween.TweenProperty(this._icon, "scale", NRunHistoryPlayerIcon._enabledScale, 0.05);
		}

		// Token: 0x0600268D RID: 9869 RVA: 0x001062F8 File Offset: 0x001044F8
		public void Deselect()
		{
			this._hsv.SetShaderParameter(NRunHistoryPlayerIcon._s, 0.3f);
			this._hsv.SetShaderParameter(NRunHistoryPlayerIcon._v, 0.55f);
			this._ascensionIcon.Visible = false;
			Tween tween = this._tween;
			if (tween != null)
			{
				tween.Kill();
			}
			this._tween = base.CreateTween().SetParallel(true);
			this._tween.TweenProperty(this._icon, "scale", NRunHistoryPlayerIcon._disabledScale, 0.05);
		}

		// Token: 0x0600268E RID: 9870 RVA: 0x00106398 File Offset: 0x00104598
		protected override void OnFocus()
		{
			NHoverTipSet nhoverTipSet = NHoverTipSet.CreateAndShow(this, this._hoverTips, HoverTipAlignment.None);
			if (nhoverTipSet != null)
			{
				nhoverTipSet.SetGlobalPosition(base.GlobalPosition + new Vector2(0f, base.Size.Y + 20f), false);
			}
			if (NControllerManager.Instance.IsUsingController)
			{
				this._selectionReticle.OnSelect();
			}
		}

		// Token: 0x0600268F RID: 9871 RVA: 0x001063FA File Offset: 0x001045FA
		protected override void OnUnfocus()
		{
			this._selectionReticle.OnDeselect();
			NHoverTipSet.Remove(this);
		}

		/// <summary>
		/// Get the method information for all the methods declared in this class.
		/// This method is used by Godot to register the available methods in the editor.
		/// Do not call this method.
		/// </summary>
		// Token: 0x06002690 RID: 9872 RVA: 0x00106410 File Offset: 0x00104610
		[NullableContext(0)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal new static List<MethodInfo> GetGodotMethodList()
		{
			return new List<MethodInfo>(5)
			{
				new MethodInfo(NRunHistoryPlayerIcon.MethodName._Ready, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NRunHistoryPlayerIcon.MethodName.Select, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NRunHistoryPlayerIcon.MethodName.Deselect, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NRunHistoryPlayerIcon.MethodName.OnFocus, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NRunHistoryPlayerIcon.MethodName.OnUnfocus, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null)
			};
		}

		/// <inheritdoc />
		// Token: 0x06002691 RID: 9873 RVA: 0x00106510 File Offset: 0x00104710
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool InvokeGodotClassMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
		{
			if ((ref method) == NRunHistoryPlayerIcon.MethodName._Ready && args.Count == 0)
			{
				this._Ready();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NRunHistoryPlayerIcon.MethodName.Select && args.Count == 0)
			{
				this.Select();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NRunHistoryPlayerIcon.MethodName.Deselect && args.Count == 0)
			{
				this.Deselect();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NRunHistoryPlayerIcon.MethodName.OnFocus && args.Count == 0)
			{
				this.OnFocus();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NRunHistoryPlayerIcon.MethodName.OnUnfocus && args.Count == 0)
			{
				this.OnUnfocus();
				ret = default(godot_variant);
				return true;
			}
			return base.InvokeGodotClassMethod(in method, args, out ret);
		}

		/// <inheritdoc />
		// Token: 0x06002692 RID: 9874 RVA: 0x001065E0 File Offset: 0x001047E0
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool HasGodotClassMethod(in godot_string_name method)
		{
			return (ref method) == NRunHistoryPlayerIcon.MethodName._Ready || (ref method) == NRunHistoryPlayerIcon.MethodName.Select || (ref method) == NRunHistoryPlayerIcon.MethodName.Deselect || (ref method) == NRunHistoryPlayerIcon.MethodName.OnFocus || (ref method) == NRunHistoryPlayerIcon.MethodName.OnUnfocus || base.HasGodotClassMethod(in method);
		}

		/// <inheritdoc />
		// Token: 0x06002693 RID: 9875 RVA: 0x00106640 File Offset: 0x00104840
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool SetGodotClassPropertyValue(in godot_string_name name, in godot_variant value)
		{
			if ((ref name) == NRunHistoryPlayerIcon.PropertyName._achievementLock)
			{
				this._achievementLock = VariantUtils.ConvertTo<Control>(ref value);
				return true;
			}
			if ((ref name) == NRunHistoryPlayerIcon.PropertyName._ascensionIcon)
			{
				this._ascensionIcon = VariantUtils.ConvertTo<Control>(ref value);
				return true;
			}
			if ((ref name) == NRunHistoryPlayerIcon.PropertyName._ascensionLabel)
			{
				this._ascensionLabel = VariantUtils.ConvertTo<MegaLabel>(ref value);
				return true;
			}
			if ((ref name) == NRunHistoryPlayerIcon.PropertyName._selectionReticle)
			{
				this._selectionReticle = VariantUtils.ConvertTo<NSelectionReticle>(ref value);
				return true;
			}
			if ((ref name) == NRunHistoryPlayerIcon.PropertyName._hsv)
			{
				this._hsv = VariantUtils.ConvertTo<ShaderMaterial>(ref value);
				return true;
			}
			if ((ref name) == NRunHistoryPlayerIcon.PropertyName._icon)
			{
				this._icon = VariantUtils.ConvertTo<TextureRect>(ref value);
				return true;
			}
			if ((ref name) == NRunHistoryPlayerIcon.PropertyName._tween)
			{
				this._tween = VariantUtils.ConvertTo<Tween>(ref value);
				return true;
			}
			return base.SetGodotClassPropertyValue(in name, in value);
		}

		/// <inheritdoc />
		// Token: 0x06002694 RID: 9876 RVA: 0x00106714 File Offset: 0x00104914
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
		{
			if ((ref name) == NRunHistoryPlayerIcon.PropertyName._achievementLock)
			{
				value = VariantUtils.CreateFrom<Control>(ref this._achievementLock);
				return true;
			}
			if ((ref name) == NRunHistoryPlayerIcon.PropertyName._ascensionIcon)
			{
				value = VariantUtils.CreateFrom<Control>(ref this._ascensionIcon);
				return true;
			}
			if ((ref name) == NRunHistoryPlayerIcon.PropertyName._ascensionLabel)
			{
				value = VariantUtils.CreateFrom<MegaLabel>(ref this._ascensionLabel);
				return true;
			}
			if ((ref name) == NRunHistoryPlayerIcon.PropertyName._selectionReticle)
			{
				value = VariantUtils.CreateFrom<NSelectionReticle>(ref this._selectionReticle);
				return true;
			}
			if ((ref name) == NRunHistoryPlayerIcon.PropertyName._hsv)
			{
				value = VariantUtils.CreateFrom<ShaderMaterial>(ref this._hsv);
				return true;
			}
			if ((ref name) == NRunHistoryPlayerIcon.PropertyName._icon)
			{
				value = VariantUtils.CreateFrom<TextureRect>(ref this._icon);
				return true;
			}
			if ((ref name) == NRunHistoryPlayerIcon.PropertyName._tween)
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
		// Token: 0x06002695 RID: 9877 RVA: 0x0010680C File Offset: 0x00104A0C
		[NullableContext(0)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal new static List<PropertyInfo> GetGodotPropertyList()
		{
			return new List<PropertyInfo>
			{
				new PropertyInfo(24L, NRunHistoryPlayerIcon.PropertyName._achievementLock, 0L, "", 4096L, false),
				new PropertyInfo(24L, NRunHistoryPlayerIcon.PropertyName._ascensionIcon, 0L, "", 4096L, false),
				new PropertyInfo(24L, NRunHistoryPlayerIcon.PropertyName._ascensionLabel, 0L, "", 4096L, false),
				new PropertyInfo(24L, NRunHistoryPlayerIcon.PropertyName._selectionReticle, 0L, "", 4096L, false),
				new PropertyInfo(24L, NRunHistoryPlayerIcon.PropertyName._hsv, 0L, "", 4096L, false),
				new PropertyInfo(24L, NRunHistoryPlayerIcon.PropertyName._icon, 0L, "", 4096L, false),
				new PropertyInfo(24L, NRunHistoryPlayerIcon.PropertyName._tween, 0L, "", 4096L, false)
			};
		}

		/// <inheritdoc />
		// Token: 0x06002696 RID: 9878 RVA: 0x00106908 File Offset: 0x00104B08
		[NullableContext(0)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void SaveGodotObjectData(GodotSerializationInfo info)
		{
			base.SaveGodotObjectData(info);
			info.AddProperty(NRunHistoryPlayerIcon.PropertyName._achievementLock, Variant.From<Control>(ref this._achievementLock));
			info.AddProperty(NRunHistoryPlayerIcon.PropertyName._ascensionIcon, Variant.From<Control>(ref this._ascensionIcon));
			info.AddProperty(NRunHistoryPlayerIcon.PropertyName._ascensionLabel, Variant.From<MegaLabel>(ref this._ascensionLabel));
			info.AddProperty(NRunHistoryPlayerIcon.PropertyName._selectionReticle, Variant.From<NSelectionReticle>(ref this._selectionReticle));
			info.AddProperty(NRunHistoryPlayerIcon.PropertyName._hsv, Variant.From<ShaderMaterial>(ref this._hsv));
			info.AddProperty(NRunHistoryPlayerIcon.PropertyName._icon, Variant.From<TextureRect>(ref this._icon));
			info.AddProperty(NRunHistoryPlayerIcon.PropertyName._tween, Variant.From<Tween>(ref this._tween));
		}

		/// <inheritdoc />
		// Token: 0x06002697 RID: 9879 RVA: 0x001069B8 File Offset: 0x00104BB8
		[NullableContext(0)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void RestoreGodotObjectData(GodotSerializationInfo info)
		{
			base.RestoreGodotObjectData(info);
			Variant variant;
			if (info.TryGetProperty(NRunHistoryPlayerIcon.PropertyName._achievementLock, ref variant))
			{
				this._achievementLock = variant.As<Control>();
			}
			Variant variant2;
			if (info.TryGetProperty(NRunHistoryPlayerIcon.PropertyName._ascensionIcon, ref variant2))
			{
				this._ascensionIcon = variant2.As<Control>();
			}
			Variant variant3;
			if (info.TryGetProperty(NRunHistoryPlayerIcon.PropertyName._ascensionLabel, ref variant3))
			{
				this._ascensionLabel = variant3.As<MegaLabel>();
			}
			Variant variant4;
			if (info.TryGetProperty(NRunHistoryPlayerIcon.PropertyName._selectionReticle, ref variant4))
			{
				this._selectionReticle = variant4.As<NSelectionReticle>();
			}
			Variant variant5;
			if (info.TryGetProperty(NRunHistoryPlayerIcon.PropertyName._hsv, ref variant5))
			{
				this._hsv = variant5.As<ShaderMaterial>();
			}
			Variant variant6;
			if (info.TryGetProperty(NRunHistoryPlayerIcon.PropertyName._icon, ref variant6))
			{
				this._icon = variant6.As<TextureRect>();
			}
			Variant variant7;
			if (info.TryGetProperty(NRunHistoryPlayerIcon.PropertyName._tween, ref variant7))
			{
				this._tween = variant7.As<Tween>();
			}
		}

		// Token: 0x04000E4C RID: 3660
		private static readonly StringName _v = new StringName("v");

		// Token: 0x04000E4D RID: 3661
		private static readonly StringName _s = new StringName("s");

		// Token: 0x04000E4E RID: 3662
		public static readonly string scenePath = SceneHelper.GetScenePath("screens/run_history_screen/run_history_player_icon");

		// Token: 0x04000E4F RID: 3663
		private readonly List<IHoverTip> _hoverTips = new List<IHoverTip>();

		// Token: 0x04000E50 RID: 3664
		private Control _achievementLock;

		// Token: 0x04000E51 RID: 3665
		private Control _ascensionIcon;

		// Token: 0x04000E52 RID: 3666
		private MegaLabel _ascensionLabel;

		// Token: 0x04000E53 RID: 3667
		private NSelectionReticle _selectionReticle;

		// Token: 0x04000E54 RID: 3668
		private ShaderMaterial _hsv;

		// Token: 0x04000E55 RID: 3669
		private TextureRect _icon;

		// Token: 0x04000E57 RID: 3671
		[Nullable(2)]
		private Tween _tween;

		// Token: 0x04000E58 RID: 3672
		private static readonly Vector2 _enabledScale = Vector2.One * 1.1f;

		// Token: 0x04000E59 RID: 3673
		private static readonly Vector2 _disabledScale = Vector2.One * 0.95f;

		/// <summary>
		/// Cached StringNames for the methods contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x02001449 RID: 5193
		[NullableContext(0)]
		public new class MethodName : NButton.MethodName
		{
			/// <summary>
			/// Cached name for the '_Ready' method.
			/// </summary>
			// Token: 0x04004CFC RID: 19708
			public new static readonly StringName _Ready = "_Ready";

			/// <summary>
			/// Cached name for the 'Select' method.
			/// </summary>
			// Token: 0x04004CFD RID: 19709
			public static readonly StringName Select = "Select";

			/// <summary>
			/// Cached name for the 'Deselect' method.
			/// </summary>
			// Token: 0x04004CFE RID: 19710
			public static readonly StringName Deselect = "Deselect";

			/// <summary>
			/// Cached name for the 'OnFocus' method.
			/// </summary>
			// Token: 0x04004CFF RID: 19711
			public new static readonly StringName OnFocus = "OnFocus";

			/// <summary>
			/// Cached name for the 'OnUnfocus' method.
			/// </summary>
			// Token: 0x04004D00 RID: 19712
			public new static readonly StringName OnUnfocus = "OnUnfocus";
		}

		/// <summary>
		/// Cached StringNames for the properties and fields contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x0200144A RID: 5194
		[NullableContext(0)]
		public new class PropertyName : NButton.PropertyName
		{
			/// <summary>
			/// Cached name for the '_achievementLock' field.
			/// </summary>
			// Token: 0x04004D01 RID: 19713
			public static readonly StringName _achievementLock = "_achievementLock";

			/// <summary>
			/// Cached name for the '_ascensionIcon' field.
			/// </summary>
			// Token: 0x04004D02 RID: 19714
			public static readonly StringName _ascensionIcon = "_ascensionIcon";

			/// <summary>
			/// Cached name for the '_ascensionLabel' field.
			/// </summary>
			// Token: 0x04004D03 RID: 19715
			public static readonly StringName _ascensionLabel = "_ascensionLabel";

			/// <summary>
			/// Cached name for the '_selectionReticle' field.
			/// </summary>
			// Token: 0x04004D04 RID: 19716
			public static readonly StringName _selectionReticle = "_selectionReticle";

			/// <summary>
			/// Cached name for the '_hsv' field.
			/// </summary>
			// Token: 0x04004D05 RID: 19717
			public static readonly StringName _hsv = "_hsv";

			/// <summary>
			/// Cached name for the '_icon' field.
			/// </summary>
			// Token: 0x04004D06 RID: 19718
			public static readonly StringName _icon = "_icon";

			/// <summary>
			/// Cached name for the '_tween' field.
			/// </summary>
			// Token: 0x04004D07 RID: 19719
			public static readonly StringName _tween = "_tween";
		}

		/// <summary>
		/// Cached StringNames for the signals contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x0200144B RID: 5195
		[NullableContext(0)]
		public new class SignalName : NButton.SignalName
		{
		}
	}
}
