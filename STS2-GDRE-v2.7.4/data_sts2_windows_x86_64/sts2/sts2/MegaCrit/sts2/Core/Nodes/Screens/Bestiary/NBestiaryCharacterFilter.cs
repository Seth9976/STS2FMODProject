using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Godot;
using Godot.Bridge;
using Godot.NativeInterop;
using MegaCrit.Sts2.Core.Assets;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.Localization;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Nodes.Combat;
using MegaCrit.Sts2.Core.Nodes.CommonUi;
using MegaCrit.Sts2.Core.Nodes.GodotExtensions;

namespace MegaCrit.Sts2.Core.Nodes.Screens.Bestiary
{
	/// <summary>
	/// The current filter the player is viewing for the Bestiary.
	/// Unlike the other tickboxes, this one is a radio-button style.
	/// Clicking on a pool filter will deselect the others. If this filter
	/// is already active, then you can't click on it.
	/// </summary>
	// Token: 0x0200035E RID: 862
	[ScriptPath("res://src/Core/Nodes/Screens/Bestiary/NBestiaryCharacterFilter.cs")]
	public class NBestiaryCharacterFilter : NButton
	{
		// Token: 0x170007D2 RID: 2002
		// (get) Token: 0x06003209 RID: 12809 RVA: 0x00175B24 File Offset: 0x00173D24
		public int Total
		{
			get
			{
				return this.kills + this.deaths;
			}
		}

		// Token: 0x170007D3 RID: 2003
		// (get) Token: 0x0600320A RID: 12810 RVA: 0x00175B33 File Offset: 0x00173D33
		private double WinRateValue
		{
			get
			{
				if (this.Total <= 0)
				{
					return 0.0;
				}
				return (double)this.kills / (double)this.Total * 100.0;
			}
		}

		// Token: 0x170007D4 RID: 2004
		// (get) Token: 0x0600320B RID: 12811 RVA: 0x00175B64 File Offset: 0x00173D64
		[Nullable(1)]
		public string WinRate
		{
			[NullableContext(1)]
			get
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler;
				if (this.WinRateValue % 1.0 != 0.0)
				{
					defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(0, 1);
					defaultInterpolatedStringHandler.AppendFormatted<double>(this.WinRateValue, "F1");
					return defaultInterpolatedStringHandler.ToStringAndClear();
				}
				defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(0, 1);
				defaultInterpolatedStringHandler.AppendFormatted<double>(this.WinRateValue, "F0");
				return defaultInterpolatedStringHandler.ToStringAndClear();
			}
		}

		// Token: 0x170007D5 RID: 2005
		// (get) Token: 0x0600320C RID: 12812 RVA: 0x00175BD1 File Offset: 0x00173DD1
		[Nullable(1)]
		public string BestiarySeenQuote
		{
			[NullableContext(1)]
			get
			{
				if (this.character != null)
				{
					return this.character.BestiarySeenQuote.GetFormattedText();
				}
				return string.Empty;
			}
		}

		// Token: 0x170007D6 RID: 2006
		// (get) Token: 0x0600320D RID: 12813 RVA: 0x00175BF1 File Offset: 0x00173DF1
		[Nullable(2)]
		public LocString BestiaryKillQuote
		{
			[NullableContext(2)]
			get
			{
				CharacterModel characterModel = this.character;
				if (characterModel == null)
				{
					return null;
				}
				return characterModel.BestiaryKillQuote;
			}
		}

		// Token: 0x170007D7 RID: 2007
		// (get) Token: 0x0600320E RID: 12814 RVA: 0x00175C04 File Offset: 0x00173E04
		// (set) Token: 0x0600320F RID: 12815 RVA: 0x00175C0C File Offset: 0x00173E0C
		public bool IsSelected
		{
			get
			{
				return this._isSelected;
			}
			set
			{
				this._isSelected = value;
				this.OnToggle();
			}
		}

		// Token: 0x170007D8 RID: 2008
		// (get) Token: 0x06003210 RID: 12816 RVA: 0x00175C1B File Offset: 0x00173E1B
		// (set) Token: 0x06003211 RID: 12817 RVA: 0x00175C23 File Offset: 0x00173E23
		public bool IsLocked
		{
			get
			{
				return this._isLocked;
			}
			set
			{
				this._isLocked = value;
				this.SetLockedState();
			}
		}

		/// <summary>
		/// If a null character is passed, then this filter is set to "All Characters"
		/// </summary>
		// Token: 0x06003212 RID: 12818 RVA: 0x00175C34 File Offset: 0x00173E34
		[NullableContext(1)]
		public static NBestiaryCharacterFilter Create([Nullable(2)] CharacterModel character)
		{
			NBestiaryCharacterFilter nbestiaryCharacterFilter = PreloadManager.Cache.GetAsset<PackedScene>(NBestiaryCharacterFilter._scenePath).Instantiate<NBestiaryCharacterFilter>(0L);
			nbestiaryCharacterFilter.character = character;
			return nbestiaryCharacterFilter;
		}

		// Token: 0x06003213 RID: 12819 RVA: 0x00175C60 File Offset: 0x00173E60
		public override void _Ready()
		{
			this.ConnectSignals();
			this._image = base.GetNode<TextureRect>("%Image");
			if (this.character != null)
			{
				this._image.Texture = this.character.IconTexture;
				base.GetNode<TextureRect>("%Shadow").Texture = this.character.IconTexture;
			}
			this._controllerSelectionReticle = base.GetNode<NSelectionReticle>("%SelectionReticle");
			this._hsv = (ShaderMaterial)this._image.GetMaterial();
		}

		// Token: 0x06003214 RID: 12820 RVA: 0x00175CF4 File Offset: 0x00173EF4
		private void OnToggle()
		{
			Tween tween = this._tween;
			if (tween != null)
			{
				tween.Kill();
			}
			this._hsv.SetShaderParameter(NBestiaryCharacterFilter._s, this._isSelected ? 1.1f : 0.5f);
			this._hsv.SetShaderParameter(NBestiaryCharacterFilter._v, this._isSelected ? 1.1f : 0.75f);
			if (!this._isSelected)
			{
				this._tween = base.CreateTween().SetParallel(true);
				this._tween.TweenProperty(this._image, "scale", NBestiaryCharacterFilter._disabledScale, 0.3).SetEase(1L).SetTrans(5L);
				return;
			}
			this._tween = base.CreateTween().SetParallel(true);
			this._tween.TweenProperty(this._image, "scale", NBestiaryCharacterFilter._enabledScale, 0.2).SetEase(1L).SetTrans(10L);
		}

		// Token: 0x06003215 RID: 12821 RVA: 0x00175E0C File Offset: 0x0017400C
		private void SetLockedState()
		{
			this._image.SelfModulate = (this._isLocked ? new Color(1f, 1f, 1f, 0.2f) : Colors.White);
		}

		// Token: 0x06003216 RID: 12822 RVA: 0x00175E44 File Offset: 0x00174044
		protected override void OnRelease()
		{
			if (this._isSelected || this._isLocked)
			{
				return;
			}
			base.OnRelease();
			this.IsSelected = !this.IsSelected;
			base.EmitSignal(NBestiaryCharacterFilter.SignalName.Toggled, new Variant[] { this });
		}

		// Token: 0x06003217 RID: 12823 RVA: 0x00175E98 File Offset: 0x00174098
		protected override void OnFocus()
		{
			if (this._isSelected || this._isLocked)
			{
				return;
			}
			base.OnFocus();
			Tween tween = this._tween;
			if (tween != null)
			{
				tween.Kill();
			}
			this._tween = base.CreateTween().SetParallel(true);
			this._tween.TweenProperty(this._image, "scale", (this._isSelected ? NBestiaryCharacterFilter._enabledScale : NBestiaryCharacterFilter._disabledScale) * 1.2f, 0.05);
			if (NControllerManager.Instance.IsUsingController)
			{
				this._controllerSelectionReticle.OnSelect();
			}
		}

		// Token: 0x06003218 RID: 12824 RVA: 0x00175F40 File Offset: 0x00174140
		protected override void OnUnfocus()
		{
			if (this._isSelected || this._isLocked)
			{
				return;
			}
			base.OnUnfocus();
			Tween tween = this._tween;
			if (tween != null)
			{
				tween.Kill();
			}
			this._tween = base.CreateTween().SetParallel(true);
			this._tween.TweenProperty(this._image, "scale", this._isSelected ? NBestiaryCharacterFilter._enabledScale : NBestiaryCharacterFilter._disabledScale, 0.3);
			this._controllerSelectionReticle.OnDeselect();
		}

		// Token: 0x06003219 RID: 12825 RVA: 0x00175FD0 File Offset: 0x001741D0
		protected override void OnPress()
		{
			if (this._isSelected || this._isLocked)
			{
				return;
			}
			base.OnPress();
			Tween tween = this._tween;
			if (tween != null)
			{
				tween.Kill();
			}
			this._tween = base.CreateTween().SetParallel(true);
			this._tween.TweenProperty(this._image, "scale", (this._isSelected ? NBestiaryCharacterFilter._enabledScale : NBestiaryCharacterFilter._disabledScale) * 0.8f, 0.3).SetEase(1L).SetTrans(5L);
		}

		// Token: 0x0600321A RID: 12826 RVA: 0x0017606D File Offset: 0x0017426D
		public void Deselect()
		{
			this.IsSelected = false;
		}

		/// <summary>
		/// Get the method information for all the methods declared in this class.
		/// This method is used by Godot to register the available methods in the editor.
		/// Do not call this method.
		/// </summary>
		// Token: 0x0600321B RID: 12827 RVA: 0x00176078 File Offset: 0x00174278
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal new static List<MethodInfo> GetGodotMethodList()
		{
			return new List<MethodInfo>(8)
			{
				new MethodInfo(NBestiaryCharacterFilter.MethodName._Ready, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NBestiaryCharacterFilter.MethodName.OnToggle, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NBestiaryCharacterFilter.MethodName.SetLockedState, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NBestiaryCharacterFilter.MethodName.OnRelease, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NBestiaryCharacterFilter.MethodName.OnFocus, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NBestiaryCharacterFilter.MethodName.OnUnfocus, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NBestiaryCharacterFilter.MethodName.OnPress, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NBestiaryCharacterFilter.MethodName.Deselect, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null)
			};
		}

		/// <inheritdoc />
		// Token: 0x0600321C RID: 12828 RVA: 0x00176208 File Offset: 0x00174408
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool InvokeGodotClassMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
		{
			if ((ref method) == NBestiaryCharacterFilter.MethodName._Ready && args.Count == 0)
			{
				this._Ready();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NBestiaryCharacterFilter.MethodName.OnToggle && args.Count == 0)
			{
				this.OnToggle();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NBestiaryCharacterFilter.MethodName.SetLockedState && args.Count == 0)
			{
				this.SetLockedState();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NBestiaryCharacterFilter.MethodName.OnRelease && args.Count == 0)
			{
				this.OnRelease();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NBestiaryCharacterFilter.MethodName.OnFocus && args.Count == 0)
			{
				this.OnFocus();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NBestiaryCharacterFilter.MethodName.OnUnfocus && args.Count == 0)
			{
				this.OnUnfocus();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NBestiaryCharacterFilter.MethodName.OnPress && args.Count == 0)
			{
				this.OnPress();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NBestiaryCharacterFilter.MethodName.Deselect && args.Count == 0)
			{
				this.Deselect();
				ret = default(godot_variant);
				return true;
			}
			return base.InvokeGodotClassMethod(in method, args, out ret);
		}

		/// <inheritdoc />
		// Token: 0x0600321D RID: 12829 RVA: 0x00176348 File Offset: 0x00174548
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool HasGodotClassMethod(in godot_string_name method)
		{
			return (ref method) == NBestiaryCharacterFilter.MethodName._Ready || (ref method) == NBestiaryCharacterFilter.MethodName.OnToggle || (ref method) == NBestiaryCharacterFilter.MethodName.SetLockedState || (ref method) == NBestiaryCharacterFilter.MethodName.OnRelease || (ref method) == NBestiaryCharacterFilter.MethodName.OnFocus || (ref method) == NBestiaryCharacterFilter.MethodName.OnUnfocus || (ref method) == NBestiaryCharacterFilter.MethodName.OnPress || (ref method) == NBestiaryCharacterFilter.MethodName.Deselect || base.HasGodotClassMethod(in method);
		}

		/// <inheritdoc />
		// Token: 0x0600321E RID: 12830 RVA: 0x001763D4 File Offset: 0x001745D4
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool SetGodotClassPropertyValue(in godot_string_name name, in godot_variant value)
		{
			if ((ref name) == NBestiaryCharacterFilter.PropertyName.IsSelected)
			{
				this.IsSelected = VariantUtils.ConvertTo<bool>(ref value);
				return true;
			}
			if ((ref name) == NBestiaryCharacterFilter.PropertyName.IsLocked)
			{
				this.IsLocked = VariantUtils.ConvertTo<bool>(ref value);
				return true;
			}
			if ((ref name) == NBestiaryCharacterFilter.PropertyName._isSelected)
			{
				this._isSelected = VariantUtils.ConvertTo<bool>(ref value);
				return true;
			}
			if ((ref name) == NBestiaryCharacterFilter.PropertyName._isLocked)
			{
				this._isLocked = VariantUtils.ConvertTo<bool>(ref value);
				return true;
			}
			if ((ref name) == NBestiaryCharacterFilter.PropertyName._image)
			{
				this._image = VariantUtils.ConvertTo<TextureRect>(ref value);
				return true;
			}
			if ((ref name) == NBestiaryCharacterFilter.PropertyName._hsv)
			{
				this._hsv = VariantUtils.ConvertTo<ShaderMaterial>(ref value);
				return true;
			}
			if ((ref name) == NBestiaryCharacterFilter.PropertyName._controllerSelectionReticle)
			{
				this._controllerSelectionReticle = VariantUtils.ConvertTo<NSelectionReticle>(ref value);
				return true;
			}
			if ((ref name) == NBestiaryCharacterFilter.PropertyName._tween)
			{
				this._tween = VariantUtils.ConvertTo<Tween>(ref value);
				return true;
			}
			if ((ref name) == NBestiaryCharacterFilter.PropertyName.kills)
			{
				this.kills = VariantUtils.ConvertTo<int>(ref value);
				return true;
			}
			if ((ref name) == NBestiaryCharacterFilter.PropertyName.deaths)
			{
				this.deaths = VariantUtils.ConvertTo<int>(ref value);
				return true;
			}
			return base.SetGodotClassPropertyValue(in name, in value);
		}

		/// <inheritdoc />
		// Token: 0x0600321F RID: 12831 RVA: 0x001764F8 File Offset: 0x001746F8
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
		{
			if ((ref name) == NBestiaryCharacterFilter.PropertyName.Total)
			{
				int total = this.Total;
				value = VariantUtils.CreateFrom<int>(ref total);
				return true;
			}
			if ((ref name) == NBestiaryCharacterFilter.PropertyName.WinRateValue)
			{
				double winRateValue = this.WinRateValue;
				value = VariantUtils.CreateFrom<double>(ref winRateValue);
				return true;
			}
			if ((ref name) == NBestiaryCharacterFilter.PropertyName.WinRate)
			{
				string text = this.WinRate;
				value = VariantUtils.CreateFrom<string>(ref text);
				return true;
			}
			if ((ref name) == NBestiaryCharacterFilter.PropertyName.BestiarySeenQuote)
			{
				string text = this.BestiarySeenQuote;
				value = VariantUtils.CreateFrom<string>(ref text);
				return true;
			}
			if ((ref name) == NBestiaryCharacterFilter.PropertyName.IsSelected)
			{
				bool flag = this.IsSelected;
				value = VariantUtils.CreateFrom<bool>(ref flag);
				return true;
			}
			if ((ref name) == NBestiaryCharacterFilter.PropertyName.IsLocked)
			{
				bool flag = this.IsLocked;
				value = VariantUtils.CreateFrom<bool>(ref flag);
				return true;
			}
			if ((ref name) == NBestiaryCharacterFilter.PropertyName._isSelected)
			{
				value = VariantUtils.CreateFrom<bool>(ref this._isSelected);
				return true;
			}
			if ((ref name) == NBestiaryCharacterFilter.PropertyName._isLocked)
			{
				value = VariantUtils.CreateFrom<bool>(ref this._isLocked);
				return true;
			}
			if ((ref name) == NBestiaryCharacterFilter.PropertyName._image)
			{
				value = VariantUtils.CreateFrom<TextureRect>(ref this._image);
				return true;
			}
			if ((ref name) == NBestiaryCharacterFilter.PropertyName._hsv)
			{
				value = VariantUtils.CreateFrom<ShaderMaterial>(ref this._hsv);
				return true;
			}
			if ((ref name) == NBestiaryCharacterFilter.PropertyName._controllerSelectionReticle)
			{
				value = VariantUtils.CreateFrom<NSelectionReticle>(ref this._controllerSelectionReticle);
				return true;
			}
			if ((ref name) == NBestiaryCharacterFilter.PropertyName._tween)
			{
				value = VariantUtils.CreateFrom<Tween>(ref this._tween);
				return true;
			}
			if ((ref name) == NBestiaryCharacterFilter.PropertyName.kills)
			{
				value = VariantUtils.CreateFrom<int>(ref this.kills);
				return true;
			}
			if ((ref name) == NBestiaryCharacterFilter.PropertyName.deaths)
			{
				value = VariantUtils.CreateFrom<int>(ref this.deaths);
				return true;
			}
			return base.GetGodotClassPropertyValue(in name, out value);
		}

		/// <summary>
		/// Get the property information for all the properties declared in this class.
		/// This method is used by Godot to register the available properties in the editor.
		/// Do not call this method.
		/// </summary>
		// Token: 0x06003220 RID: 12832 RVA: 0x001766E0 File Offset: 0x001748E0
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal new static List<PropertyInfo> GetGodotPropertyList()
		{
			return new List<PropertyInfo>
			{
				new PropertyInfo(1L, NBestiaryCharacterFilter.PropertyName._isSelected, 0L, "", 4096L, false),
				new PropertyInfo(1L, NBestiaryCharacterFilter.PropertyName._isLocked, 0L, "", 4096L, false),
				new PropertyInfo(24L, NBestiaryCharacterFilter.PropertyName._image, 0L, "", 4096L, false),
				new PropertyInfo(24L, NBestiaryCharacterFilter.PropertyName._hsv, 0L, "", 4096L, false),
				new PropertyInfo(24L, NBestiaryCharacterFilter.PropertyName._controllerSelectionReticle, 0L, "", 4096L, false),
				new PropertyInfo(24L, NBestiaryCharacterFilter.PropertyName._tween, 0L, "", 4096L, false),
				new PropertyInfo(2L, NBestiaryCharacterFilter.PropertyName.kills, 0L, "", 4096L, false),
				new PropertyInfo(2L, NBestiaryCharacterFilter.PropertyName.deaths, 0L, "", 4096L, false),
				new PropertyInfo(2L, NBestiaryCharacterFilter.PropertyName.Total, 0L, "", 4096L, false),
				new PropertyInfo(3L, NBestiaryCharacterFilter.PropertyName.WinRateValue, 0L, "", 4096L, false),
				new PropertyInfo(4L, NBestiaryCharacterFilter.PropertyName.WinRate, 0L, "", 4096L, false),
				new PropertyInfo(4L, NBestiaryCharacterFilter.PropertyName.BestiarySeenQuote, 0L, "", 4096L, false),
				new PropertyInfo(1L, NBestiaryCharacterFilter.PropertyName.IsSelected, 0L, "", 4096L, false),
				new PropertyInfo(1L, NBestiaryCharacterFilter.PropertyName.IsLocked, 0L, "", 4096L, false)
			};
		}

		/// <inheritdoc />
		// Token: 0x06003221 RID: 12833 RVA: 0x001768B8 File Offset: 0x00174AB8
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void SaveGodotObjectData(GodotSerializationInfo info)
		{
			base.SaveGodotObjectData(info);
			StringName isSelected = NBestiaryCharacterFilter.PropertyName.IsSelected;
			bool flag = this.IsSelected;
			info.AddProperty(isSelected, Variant.From<bool>(ref flag));
			StringName isLocked = NBestiaryCharacterFilter.PropertyName.IsLocked;
			flag = this.IsLocked;
			info.AddProperty(isLocked, Variant.From<bool>(ref flag));
			info.AddProperty(NBestiaryCharacterFilter.PropertyName._isSelected, Variant.From<bool>(ref this._isSelected));
			info.AddProperty(NBestiaryCharacterFilter.PropertyName._isLocked, Variant.From<bool>(ref this._isLocked));
			info.AddProperty(NBestiaryCharacterFilter.PropertyName._image, Variant.From<TextureRect>(ref this._image));
			info.AddProperty(NBestiaryCharacterFilter.PropertyName._hsv, Variant.From<ShaderMaterial>(ref this._hsv));
			info.AddProperty(NBestiaryCharacterFilter.PropertyName._controllerSelectionReticle, Variant.From<NSelectionReticle>(ref this._controllerSelectionReticle));
			info.AddProperty(NBestiaryCharacterFilter.PropertyName._tween, Variant.From<Tween>(ref this._tween));
			info.AddProperty(NBestiaryCharacterFilter.PropertyName.kills, Variant.From<int>(ref this.kills));
			info.AddProperty(NBestiaryCharacterFilter.PropertyName.deaths, Variant.From<int>(ref this.deaths));
			info.AddSignalEventDelegate(NBestiaryCharacterFilter.SignalName.Toggled, this.backing_Toggled);
		}

		/// <inheritdoc />
		// Token: 0x06003222 RID: 12834 RVA: 0x001769C0 File Offset: 0x00174BC0
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void RestoreGodotObjectData(GodotSerializationInfo info)
		{
			base.RestoreGodotObjectData(info);
			Variant variant;
			if (info.TryGetProperty(NBestiaryCharacterFilter.PropertyName.IsSelected, ref variant))
			{
				this.IsSelected = variant.As<bool>();
			}
			Variant variant2;
			if (info.TryGetProperty(NBestiaryCharacterFilter.PropertyName.IsLocked, ref variant2))
			{
				this.IsLocked = variant2.As<bool>();
			}
			Variant variant3;
			if (info.TryGetProperty(NBestiaryCharacterFilter.PropertyName._isSelected, ref variant3))
			{
				this._isSelected = variant3.As<bool>();
			}
			Variant variant4;
			if (info.TryGetProperty(NBestiaryCharacterFilter.PropertyName._isLocked, ref variant4))
			{
				this._isLocked = variant4.As<bool>();
			}
			Variant variant5;
			if (info.TryGetProperty(NBestiaryCharacterFilter.PropertyName._image, ref variant5))
			{
				this._image = variant5.As<TextureRect>();
			}
			Variant variant6;
			if (info.TryGetProperty(NBestiaryCharacterFilter.PropertyName._hsv, ref variant6))
			{
				this._hsv = variant6.As<ShaderMaterial>();
			}
			Variant variant7;
			if (info.TryGetProperty(NBestiaryCharacterFilter.PropertyName._controllerSelectionReticle, ref variant7))
			{
				this._controllerSelectionReticle = variant7.As<NSelectionReticle>();
			}
			Variant variant8;
			if (info.TryGetProperty(NBestiaryCharacterFilter.PropertyName._tween, ref variant8))
			{
				this._tween = variant8.As<Tween>();
			}
			Variant variant9;
			if (info.TryGetProperty(NBestiaryCharacterFilter.PropertyName.kills, ref variant9))
			{
				this.kills = variant9.As<int>();
			}
			Variant variant10;
			if (info.TryGetProperty(NBestiaryCharacterFilter.PropertyName.deaths, ref variant10))
			{
				this.deaths = variant10.As<int>();
			}
			NBestiaryCharacterFilter.ToggledEventHandler toggledEventHandler;
			if (info.TryGetSignalEventDelegate<NBestiaryCharacterFilter.ToggledEventHandler>(NBestiaryCharacterFilter.SignalName.Toggled, ref toggledEventHandler))
			{
				this.backing_Toggled = toggledEventHandler;
			}
		}

		/// <summary>
		/// Get the signal information for all the signals declared in this class.
		/// This method is used by Godot to register the available signals in the editor.
		/// Do not call this method.
		/// </summary>
		// Token: 0x06003223 RID: 12835 RVA: 0x00176B04 File Offset: 0x00174D04
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal new static List<MethodInfo> GetGodotSignalList()
		{
			return new List<MethodInfo>(1)
			{
				new MethodInfo(NBestiaryCharacterFilter.SignalName.Toggled, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, new List<PropertyInfo>
				{
					new PropertyInfo(24L, "filter", 0L, "", 6L, new StringName("Control"), false)
				}, null)
			};
		}

		/// <inheritdoc cref="T:MegaCrit.Sts2.Core.Nodes.Screens.Bestiary.NBestiaryCharacterFilter.ToggledEventHandler" />
		// Token: 0x1400002C RID: 44
		// (add) Token: 0x06003224 RID: 12836 RVA: 0x00176B78 File Offset: 0x00174D78
		// (remove) Token: 0x06003225 RID: 12837 RVA: 0x00176B91 File Offset: 0x00174D91
		public event NBestiaryCharacterFilter.ToggledEventHandler Toggled
		{
			add
			{
				this.backing_Toggled = (NBestiaryCharacterFilter.ToggledEventHandler)Delegate.Combine(this.backing_Toggled, value);
			}
			remove
			{
				this.backing_Toggled = (NBestiaryCharacterFilter.ToggledEventHandler)Delegate.Remove(this.backing_Toggled, value);
			}
		}

		// Token: 0x06003226 RID: 12838 RVA: 0x00176BAA File Offset: 0x00174DAA
		protected void EmitSignalToggled(NBestiaryCharacterFilter filter)
		{
			base.EmitSignal(NBestiaryCharacterFilter.SignalName.Toggled, new Variant[] { filter });
		}

		/// <inheritdoc />
		// Token: 0x06003227 RID: 12839 RVA: 0x00176BCB File Offset: 0x00174DCB
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void RaiseGodotClassSignalCallbacks(in godot_string_name signal, NativeVariantPtrArgs args)
		{
			if (!((ref signal) == NBestiaryCharacterFilter.SignalName.Toggled) || args.Count != 1)
			{
				base.RaiseGodotClassSignalCallbacks(in signal, args);
				return;
			}
			NBestiaryCharacterFilter.ToggledEventHandler toggledEventHandler = this.backing_Toggled;
			if (toggledEventHandler == null)
			{
				return;
			}
			toggledEventHandler(VariantUtils.ConvertTo<NBestiaryCharacterFilter>(args[0]));
		}

		/// <inheritdoc />
		// Token: 0x06003228 RID: 12840 RVA: 0x00176C0A File Offset: 0x00174E0A
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool HasGodotClassSignal(in godot_string_name signal)
		{
			return (ref signal) == NBestiaryCharacterFilter.SignalName.Toggled || base.HasGodotClassSignal(in signal);
		}

		// Token: 0x0400138A RID: 5002
		[Nullable(1)]
		private static readonly StringName _v = new StringName("v");

		// Token: 0x0400138B RID: 5003
		[Nullable(1)]
		private static readonly StringName _s = new StringName("s");

		// Token: 0x0400138C RID: 5004
		[Nullable(1)]
		private static readonly string _scenePath = SceneHelper.GetScenePath("screens/bestiary/bestiary_character_filter");

		// Token: 0x0400138D RID: 5005
		private bool _isSelected;

		// Token: 0x0400138E RID: 5006
		private bool _isLocked;

		// Token: 0x0400138F RID: 5007
		[Nullable(2)]
		public CharacterModel character;

		// Token: 0x04001390 RID: 5008
		[Nullable(1)]
		private TextureRect _image;

		// Token: 0x04001391 RID: 5009
		[Nullable(1)]
		private ShaderMaterial _hsv;

		// Token: 0x04001392 RID: 5010
		[Nullable(1)]
		private NSelectionReticle _controllerSelectionReticle;

		// Token: 0x04001393 RID: 5011
		[Nullable(2)]
		private Tween _tween;

		// Token: 0x04001394 RID: 5012
		private const float _focusedMultiplier = 1.2f;

		// Token: 0x04001395 RID: 5013
		private const float _pressDownMultiplier = 0.8f;

		// Token: 0x04001396 RID: 5014
		private static readonly Vector2 _enabledScale = Vector2.One * 1.2f;

		// Token: 0x04001397 RID: 5015
		private static readonly Vector2 _disabledScale = Vector2.One * 0.95f;

		// Token: 0x04001398 RID: 5016
		public int kills;

		// Token: 0x04001399 RID: 5017
		public int deaths;

		// Token: 0x0400139A RID: 5018
		private NBestiaryCharacterFilter.ToggledEventHandler backing_Toggled;

		// Token: 0x02001658 RID: 5720
		// (Invoke) Token: 0x0600A49B RID: 42139
		[Signal]
		public delegate void ToggledEventHandler(NBestiaryCharacterFilter filter);

		/// <summary>
		/// Cached StringNames for the methods contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x02001659 RID: 5721
		public new class MethodName : NButton.MethodName
		{
			/// <summary>
			/// Cached name for the '_Ready' method.
			/// </summary>
			// Token: 0x04005733 RID: 22323
			public new static readonly StringName _Ready = "_Ready";

			/// <summary>
			/// Cached name for the 'OnToggle' method.
			/// </summary>
			// Token: 0x04005734 RID: 22324
			public static readonly StringName OnToggle = "OnToggle";

			/// <summary>
			/// Cached name for the 'SetLockedState' method.
			/// </summary>
			// Token: 0x04005735 RID: 22325
			public static readonly StringName SetLockedState = "SetLockedState";

			/// <summary>
			/// Cached name for the 'OnRelease' method.
			/// </summary>
			// Token: 0x04005736 RID: 22326
			public new static readonly StringName OnRelease = "OnRelease";

			/// <summary>
			/// Cached name for the 'OnFocus' method.
			/// </summary>
			// Token: 0x04005737 RID: 22327
			public new static readonly StringName OnFocus = "OnFocus";

			/// <summary>
			/// Cached name for the 'OnUnfocus' method.
			/// </summary>
			// Token: 0x04005738 RID: 22328
			public new static readonly StringName OnUnfocus = "OnUnfocus";

			/// <summary>
			/// Cached name for the 'OnPress' method.
			/// </summary>
			// Token: 0x04005739 RID: 22329
			public new static readonly StringName OnPress = "OnPress";

			/// <summary>
			/// Cached name for the 'Deselect' method.
			/// </summary>
			// Token: 0x0400573A RID: 22330
			public static readonly StringName Deselect = "Deselect";
		}

		/// <summary>
		/// Cached StringNames for the properties and fields contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x0200165A RID: 5722
		public new class PropertyName : NButton.PropertyName
		{
			/// <summary>
			/// Cached name for the 'Total' property.
			/// </summary>
			// Token: 0x0400573B RID: 22331
			public static readonly StringName Total = "Total";

			/// <summary>
			/// Cached name for the 'WinRateValue' property.
			/// </summary>
			// Token: 0x0400573C RID: 22332
			public static readonly StringName WinRateValue = "WinRateValue";

			/// <summary>
			/// Cached name for the 'WinRate' property.
			/// </summary>
			// Token: 0x0400573D RID: 22333
			public static readonly StringName WinRate = "WinRate";

			/// <summary>
			/// Cached name for the 'BestiarySeenQuote' property.
			/// </summary>
			// Token: 0x0400573E RID: 22334
			public static readonly StringName BestiarySeenQuote = "BestiarySeenQuote";

			/// <summary>
			/// Cached name for the 'IsSelected' property.
			/// </summary>
			// Token: 0x0400573F RID: 22335
			public static readonly StringName IsSelected = "IsSelected";

			/// <summary>
			/// Cached name for the 'IsLocked' property.
			/// </summary>
			// Token: 0x04005740 RID: 22336
			public static readonly StringName IsLocked = "IsLocked";

			/// <summary>
			/// Cached name for the '_isSelected' field.
			/// </summary>
			// Token: 0x04005741 RID: 22337
			public static readonly StringName _isSelected = "_isSelected";

			/// <summary>
			/// Cached name for the '_isLocked' field.
			/// </summary>
			// Token: 0x04005742 RID: 22338
			public static readonly StringName _isLocked = "_isLocked";

			/// <summary>
			/// Cached name for the '_image' field.
			/// </summary>
			// Token: 0x04005743 RID: 22339
			public static readonly StringName _image = "_image";

			/// <summary>
			/// Cached name for the '_hsv' field.
			/// </summary>
			// Token: 0x04005744 RID: 22340
			public static readonly StringName _hsv = "_hsv";

			/// <summary>
			/// Cached name for the '_controllerSelectionReticle' field.
			/// </summary>
			// Token: 0x04005745 RID: 22341
			public static readonly StringName _controllerSelectionReticle = "_controllerSelectionReticle";

			/// <summary>
			/// Cached name for the '_tween' field.
			/// </summary>
			// Token: 0x04005746 RID: 22342
			public static readonly StringName _tween = "_tween";

			/// <summary>
			/// Cached name for the 'kills' field.
			/// </summary>
			// Token: 0x04005747 RID: 22343
			public static readonly StringName kills = "kills";

			/// <summary>
			/// Cached name for the 'deaths' field.
			/// </summary>
			// Token: 0x04005748 RID: 22344
			public static readonly StringName deaths = "deaths";
		}

		/// <summary>
		/// Cached StringNames for the signals contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x0200165B RID: 5723
		public new class SignalName : NButton.SignalName
		{
			/// <summary>
			/// Cached name for the 'Toggled' signal.
			/// </summary>
			// Token: 0x04005749 RID: 22345
			public static readonly StringName Toggled = "Toggled";
		}
	}
}
