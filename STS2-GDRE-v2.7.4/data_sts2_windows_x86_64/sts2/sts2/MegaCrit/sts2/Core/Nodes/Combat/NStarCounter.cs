using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Godot;
using Godot.Bridge;
using Godot.NativeInterop;
using MegaCrit.Sts2.addons.mega_text;
using MegaCrit.Sts2.Core.Assets;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization;
using MegaCrit.Sts2.Core.Nodes.HoverTips;

namespace MegaCrit.Sts2.Core.Nodes.Combat
{
	// Token: 0x0200040F RID: 1039
	[NullableContext(1)]
	[Nullable(0)]
	[ScriptPath("res://src/Core/Nodes/Combat/NStarCounter.cs")]
	public class NStarCounter : Control
	{
		// Token: 0x17000958 RID: 2392
		// (get) Token: 0x06004158 RID: 16728 RVA: 0x001F8B57 File Offset: 0x001F6D57
		public static IEnumerable<string> AssetPaths
		{
			get
			{
				return new <>z__ReadOnlySingleElementList<string>(NStarCounter._starGainVfxPath);
			}
		}

		// Token: 0x06004159 RID: 16729 RVA: 0x001F8B64 File Offset: 0x001F6D64
		public override void _Ready()
		{
			this._label = base.GetNode<MegaRichTextLabel>("%CountLabel");
			this._rotationLayers = base.GetNode<Control>("%RotationLayers");
			this._icon = base.GetNode<Control>("Icon");
			this._hsv = (ShaderMaterial)this._icon.Material;
			LocString locString = new LocString("static_hover_tips", "STAR_COUNT.description");
			locString.Add("singleStarIcon", "[img]res://images/packed/sprite_fonts/star_icon.png[/img]");
			this._hoverTip = new HoverTip(new LocString("static_hover_tips", "STAR_COUNT.title"), locString, null);
			base.Connect(Control.SignalName.MouseEntered, Callable.From(new Action(this.OnHovered)), 0U);
			base.Connect(Control.SignalName.MouseExited, Callable.From(new Action(this.OnUnhovered)), 0U);
			base.Visible = false;
		}

		// Token: 0x0600415A RID: 16730 RVA: 0x001F8C48 File Offset: 0x001F6E48
		public override void _EnterTree()
		{
			base._EnterTree();
			this.ConnectStarsChangedSignal();
		}

		// Token: 0x0600415B RID: 16731 RVA: 0x001F8C56 File Offset: 0x001F6E56
		public override void _ExitTree()
		{
			base._ExitTree();
			if (this._player != null && this._isListeningToCombatState)
			{
				this._player.PlayerCombatState.StarsChanged -= this.OnStarsChanged;
				this._isListeningToCombatState = false;
			}
		}

		// Token: 0x0600415C RID: 16732 RVA: 0x001F8C91 File Offset: 0x001F6E91
		private void ConnectStarsChangedSignal()
		{
			if (this._player != null && !this._isListeningToCombatState)
			{
				this._player.PlayerCombatState.StarsChanged += this.OnStarsChanged;
				this._isListeningToCombatState = true;
			}
		}

		// Token: 0x0600415D RID: 16733 RVA: 0x001F8CC6 File Offset: 0x001F6EC6
		public void Initialize(Player player)
		{
			this._player = player;
			this.ConnectStarsChangedSignal();
			this.RefreshVisibility();
		}

		// Token: 0x0600415E RID: 16734 RVA: 0x001F8CDC File Offset: 0x001F6EDC
		private void OnHovered()
		{
			NHoverTipSet nhoverTipSet = NHoverTipSet.CreateAndShow(this, this._hoverTip, HoverTipAlignment.None);
			if (nhoverTipSet != null)
			{
				nhoverTipSet.SetGlobalPosition(base.GlobalPosition + new Vector2(-34f, -300f), false);
			}
		}

		// Token: 0x0600415F RID: 16735 RVA: 0x001F8D20 File Offset: 0x001F6F20
		private void OnUnhovered()
		{
			NHoverTipSet.Remove(this);
		}

		// Token: 0x06004160 RID: 16736 RVA: 0x001F8D28 File Offset: 0x001F6F28
		private void OnStarsChanged(int oldStars, int newStars)
		{
			this.UpdateStarCount(oldStars, newStars);
			this.RefreshVisibility();
		}

		// Token: 0x06004161 RID: 16737 RVA: 0x001F8D38 File Offset: 0x001F6F38
		public override void _Process(double delta)
		{
			if (this._player == null)
			{
				return;
			}
			float num = ((this._player.PlayerCombatState.Stars == 0) ? 5f : 30f);
			for (int i = 0; i < this._rotationLayers.GetChildCount(false); i++)
			{
				this._rotationLayers.GetChild<Control>(i, false).RotationDegrees += (float)delta * num * (float)(i + 1);
			}
			this._lerpingStarCount = MathHelper.SmoothDamp(this._lerpingStarCount, (float)this._player.PlayerCombatState.Stars, ref this._velocity, 0.1f, (float)delta, float.PositiveInfinity);
			this.SetStarCountText(Mathf.RoundToInt(this._lerpingStarCount));
		}

		// Token: 0x06004162 RID: 16738 RVA: 0x001F8DEC File Offset: 0x001F6FEC
		private void UpdateStarCount(int oldCount, int newCount)
		{
			if (newCount < oldCount)
			{
				Tween hsvTween = this._hsvTween;
				if (hsvTween != null)
				{
					hsvTween.Kill();
				}
				this._hsv.SetShaderParameter(NStarCounter._v, 1f);
				this._lerpingStarCount = (float)newCount;
				this.SetStarCountText(newCount);
				return;
			}
			if (newCount > oldCount)
			{
				Tween hsvTween2 = this._hsvTween;
				if (hsvTween2 != null)
				{
					hsvTween2.Kill();
				}
				this._hsvTween = base.CreateTween();
				this._hsvTween.TweenMethod(Callable.From<float>(new Action<float>(this.UpdateShaderV)), 2f, 1f, 0.20000000298023224);
				Node2D node2D = PreloadManager.Cache.GetAsset<PackedScene>(NStarCounter._starGainVfxPath).Instantiate<Node2D>(0L);
				this.AddChildSafely(node2D);
				this.MoveChildSafely(node2D, 0);
				node2D.Position = base.Size / 2f;
			}
		}

		// Token: 0x06004163 RID: 16739 RVA: 0x001F8ED4 File Offset: 0x001F70D4
		private void SetStarCountText(int stars)
		{
			if (this._displayedStarCount != stars)
			{
				this._displayedStarCount = stars;
				this._label.AddThemeColorOverride(ThemeConstants.Label.FontColor, (stars == 0) ? StsColors.red : StsColors.cream);
				MegaRichTextLabel label = this._label;
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(17, 1);
				defaultInterpolatedStringHandler.AppendLiteral("[center]");
				defaultInterpolatedStringHandler.AppendFormatted<int>(stars);
				defaultInterpolatedStringHandler.AppendLiteral("[/center]");
				label.Text = defaultInterpolatedStringHandler.ToStringAndClear();
				if (stars == 0)
				{
					this._hsv.SetShaderParameter(NStarCounter._s, 0.5f);
					this._hsv.SetShaderParameter(NStarCounter._v, 0.85f);
					return;
				}
				this._hsv.SetShaderParameter(NStarCounter._s, 1f);
				this._hsv.SetShaderParameter(NStarCounter._v, 1f);
			}
		}

		// Token: 0x06004164 RID: 16740 RVA: 0x001F8FBB File Offset: 0x001F71BB
		private void UpdateShaderV(float value)
		{
			this._hsv.SetShaderParameter(NStarCounter._v, value);
		}

		// Token: 0x06004165 RID: 16741 RVA: 0x001F8FD4 File Offset: 0x001F71D4
		private void RefreshVisibility()
		{
			if (this._player == null)
			{
				base.Visible = false;
				return;
			}
			int stars = this._player.PlayerCombatState.Stars;
			base.Visible = base.Visible || this._player.Character.ShouldAlwaysShowStarCounter || stars > 0;
		}

		/// <summary>
		/// Get the method information for all the methods declared in this class.
		/// This method is used by Godot to register the available methods in the editor.
		/// Do not call this method.
		/// </summary>
		// Token: 0x06004166 RID: 16742 RVA: 0x001F902C File Offset: 0x001F722C
		[NullableContext(0)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal static List<MethodInfo> GetGodotMethodList()
		{
			return new List<MethodInfo>(12)
			{
				new MethodInfo(NStarCounter.MethodName._Ready, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NStarCounter.MethodName._EnterTree, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NStarCounter.MethodName._ExitTree, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NStarCounter.MethodName.ConnectStarsChangedSignal, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NStarCounter.MethodName.OnHovered, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NStarCounter.MethodName.OnUnhovered, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NStarCounter.MethodName.OnStarsChanged, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, new List<PropertyInfo>
				{
					new PropertyInfo(2L, "oldStars", 0L, "", 6L, false),
					new PropertyInfo(2L, "newStars", 0L, "", 6L, false)
				}, null),
				new MethodInfo(NStarCounter.MethodName._Process, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, new List<PropertyInfo>
				{
					new PropertyInfo(3L, "delta", 0L, "", 6L, false)
				}, null),
				new MethodInfo(NStarCounter.MethodName.UpdateStarCount, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, new List<PropertyInfo>
				{
					new PropertyInfo(2L, "oldCount", 0L, "", 6L, false),
					new PropertyInfo(2L, "newCount", 0L, "", 6L, false)
				}, null),
				new MethodInfo(NStarCounter.MethodName.SetStarCountText, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, new List<PropertyInfo>
				{
					new PropertyInfo(2L, "stars", 0L, "", 6L, false)
				}, null),
				new MethodInfo(NStarCounter.MethodName.UpdateShaderV, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, new List<PropertyInfo>
				{
					new PropertyInfo(3L, "value", 0L, "", 6L, false)
				}, null),
				new MethodInfo(NStarCounter.MethodName.RefreshVisibility, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null)
			};
		}

		/// <inheritdoc />
		// Token: 0x06004167 RID: 16743 RVA: 0x001F9374 File Offset: 0x001F7574
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool InvokeGodotClassMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
		{
			if ((ref method) == NStarCounter.MethodName._Ready && args.Count == 0)
			{
				this._Ready();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NStarCounter.MethodName._EnterTree && args.Count == 0)
			{
				this._EnterTree();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NStarCounter.MethodName._ExitTree && args.Count == 0)
			{
				this._ExitTree();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NStarCounter.MethodName.ConnectStarsChangedSignal && args.Count == 0)
			{
				this.ConnectStarsChangedSignal();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NStarCounter.MethodName.OnHovered && args.Count == 0)
			{
				this.OnHovered();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NStarCounter.MethodName.OnUnhovered && args.Count == 0)
			{
				this.OnUnhovered();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NStarCounter.MethodName.OnStarsChanged && args.Count == 2)
			{
				this.OnStarsChanged(VariantUtils.ConvertTo<int>(args[0]), VariantUtils.ConvertTo<int>(args[1]));
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NStarCounter.MethodName._Process && args.Count == 1)
			{
				this._Process(VariantUtils.ConvertTo<double>(args[0]));
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NStarCounter.MethodName.UpdateStarCount && args.Count == 2)
			{
				this.UpdateStarCount(VariantUtils.ConvertTo<int>(args[0]), VariantUtils.ConvertTo<int>(args[1]));
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NStarCounter.MethodName.SetStarCountText && args.Count == 1)
			{
				this.SetStarCountText(VariantUtils.ConvertTo<int>(args[0]));
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NStarCounter.MethodName.UpdateShaderV && args.Count == 1)
			{
				this.UpdateShaderV(VariantUtils.ConvertTo<float>(args[0]));
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NStarCounter.MethodName.RefreshVisibility && args.Count == 0)
			{
				this.RefreshVisibility();
				ret = default(godot_variant);
				return true;
			}
			return base.InvokeGodotClassMethod(ref method, args, ref ret);
		}

		/// <inheritdoc />
		// Token: 0x06004168 RID: 16744 RVA: 0x001F95A8 File Offset: 0x001F77A8
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool HasGodotClassMethod(in godot_string_name method)
		{
			return (ref method) == NStarCounter.MethodName._Ready || (ref method) == NStarCounter.MethodName._EnterTree || (ref method) == NStarCounter.MethodName._ExitTree || (ref method) == NStarCounter.MethodName.ConnectStarsChangedSignal || (ref method) == NStarCounter.MethodName.OnHovered || (ref method) == NStarCounter.MethodName.OnUnhovered || (ref method) == NStarCounter.MethodName.OnStarsChanged || (ref method) == NStarCounter.MethodName._Process || (ref method) == NStarCounter.MethodName.UpdateStarCount || (ref method) == NStarCounter.MethodName.SetStarCountText || (ref method) == NStarCounter.MethodName.UpdateShaderV || (ref method) == NStarCounter.MethodName.RefreshVisibility || base.HasGodotClassMethod(ref method);
		}

		/// <inheritdoc />
		// Token: 0x06004169 RID: 16745 RVA: 0x001F9670 File Offset: 0x001F7870
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool SetGodotClassPropertyValue(in godot_string_name name, in godot_variant value)
		{
			if ((ref name) == NStarCounter.PropertyName._label)
			{
				this._label = VariantUtils.ConvertTo<MegaRichTextLabel>(ref value);
				return true;
			}
			if ((ref name) == NStarCounter.PropertyName._rotationLayers)
			{
				this._rotationLayers = VariantUtils.ConvertTo<Control>(ref value);
				return true;
			}
			if ((ref name) == NStarCounter.PropertyName._icon)
			{
				this._icon = VariantUtils.ConvertTo<Control>(ref value);
				return true;
			}
			if ((ref name) == NStarCounter.PropertyName._hsv)
			{
				this._hsv = VariantUtils.ConvertTo<ShaderMaterial>(ref value);
				return true;
			}
			if ((ref name) == NStarCounter.PropertyName._lerpingStarCount)
			{
				this._lerpingStarCount = VariantUtils.ConvertTo<float>(ref value);
				return true;
			}
			if ((ref name) == NStarCounter.PropertyName._velocity)
			{
				this._velocity = VariantUtils.ConvertTo<float>(ref value);
				return true;
			}
			if ((ref name) == NStarCounter.PropertyName._displayedStarCount)
			{
				this._displayedStarCount = VariantUtils.ConvertTo<int>(ref value);
				return true;
			}
			if ((ref name) == NStarCounter.PropertyName._hsvTween)
			{
				this._hsvTween = VariantUtils.ConvertTo<Tween>(ref value);
				return true;
			}
			if ((ref name) == NStarCounter.PropertyName._isListeningToCombatState)
			{
				this._isListeningToCombatState = VariantUtils.ConvertTo<bool>(ref value);
				return true;
			}
			return base.SetGodotClassPropertyValue(ref name, ref value);
		}

		/// <inheritdoc />
		// Token: 0x0600416A RID: 16746 RVA: 0x001F9778 File Offset: 0x001F7978
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
		{
			if ((ref name) == NStarCounter.PropertyName._label)
			{
				value = VariantUtils.CreateFrom<MegaRichTextLabel>(ref this._label);
				return true;
			}
			if ((ref name) == NStarCounter.PropertyName._rotationLayers)
			{
				value = VariantUtils.CreateFrom<Control>(ref this._rotationLayers);
				return true;
			}
			if ((ref name) == NStarCounter.PropertyName._icon)
			{
				value = VariantUtils.CreateFrom<Control>(ref this._icon);
				return true;
			}
			if ((ref name) == NStarCounter.PropertyName._hsv)
			{
				value = VariantUtils.CreateFrom<ShaderMaterial>(ref this._hsv);
				return true;
			}
			if ((ref name) == NStarCounter.PropertyName._lerpingStarCount)
			{
				value = VariantUtils.CreateFrom<float>(ref this._lerpingStarCount);
				return true;
			}
			if ((ref name) == NStarCounter.PropertyName._velocity)
			{
				value = VariantUtils.CreateFrom<float>(ref this._velocity);
				return true;
			}
			if ((ref name) == NStarCounter.PropertyName._displayedStarCount)
			{
				value = VariantUtils.CreateFrom<int>(ref this._displayedStarCount);
				return true;
			}
			if ((ref name) == NStarCounter.PropertyName._hsvTween)
			{
				value = VariantUtils.CreateFrom<Tween>(ref this._hsvTween);
				return true;
			}
			if ((ref name) == NStarCounter.PropertyName._isListeningToCombatState)
			{
				value = VariantUtils.CreateFrom<bool>(ref this._isListeningToCombatState);
				return true;
			}
			return base.GetGodotClassPropertyValue(ref name, ref value);
		}

		/// <summary>
		/// Get the property information for all the properties declared in this class.
		/// This method is used by Godot to register the available properties in the editor.
		/// Do not call this method.
		/// </summary>
		// Token: 0x0600416B RID: 16747 RVA: 0x001F98B0 File Offset: 0x001F7AB0
		[NullableContext(0)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal static List<PropertyInfo> GetGodotPropertyList()
		{
			return new List<PropertyInfo>
			{
				new PropertyInfo(24L, NStarCounter.PropertyName._label, 0L, "", 4096L, false),
				new PropertyInfo(24L, NStarCounter.PropertyName._rotationLayers, 0L, "", 4096L, false),
				new PropertyInfo(24L, NStarCounter.PropertyName._icon, 0L, "", 4096L, false),
				new PropertyInfo(24L, NStarCounter.PropertyName._hsv, 0L, "", 4096L, false),
				new PropertyInfo(3L, NStarCounter.PropertyName._lerpingStarCount, 0L, "", 4096L, false),
				new PropertyInfo(3L, NStarCounter.PropertyName._velocity, 0L, "", 4096L, false),
				new PropertyInfo(2L, NStarCounter.PropertyName._displayedStarCount, 0L, "", 4096L, false),
				new PropertyInfo(24L, NStarCounter.PropertyName._hsvTween, 0L, "", 4096L, false),
				new PropertyInfo(1L, NStarCounter.PropertyName._isListeningToCombatState, 0L, "", 4096L, false)
			};
		}

		/// <inheritdoc />
		// Token: 0x0600416C RID: 16748 RVA: 0x001F99EC File Offset: 0x001F7BEC
		[NullableContext(0)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void SaveGodotObjectData(GodotSerializationInfo info)
		{
			base.SaveGodotObjectData(info);
			info.AddProperty(NStarCounter.PropertyName._label, Variant.From<MegaRichTextLabel>(ref this._label));
			info.AddProperty(NStarCounter.PropertyName._rotationLayers, Variant.From<Control>(ref this._rotationLayers));
			info.AddProperty(NStarCounter.PropertyName._icon, Variant.From<Control>(ref this._icon));
			info.AddProperty(NStarCounter.PropertyName._hsv, Variant.From<ShaderMaterial>(ref this._hsv));
			info.AddProperty(NStarCounter.PropertyName._lerpingStarCount, Variant.From<float>(ref this._lerpingStarCount));
			info.AddProperty(NStarCounter.PropertyName._velocity, Variant.From<float>(ref this._velocity));
			info.AddProperty(NStarCounter.PropertyName._displayedStarCount, Variant.From<int>(ref this._displayedStarCount));
			info.AddProperty(NStarCounter.PropertyName._hsvTween, Variant.From<Tween>(ref this._hsvTween));
			info.AddProperty(NStarCounter.PropertyName._isListeningToCombatState, Variant.From<bool>(ref this._isListeningToCombatState));
		}

		/// <inheritdoc />
		// Token: 0x0600416D RID: 16749 RVA: 0x001F9AC8 File Offset: 0x001F7CC8
		[NullableContext(0)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void RestoreGodotObjectData(GodotSerializationInfo info)
		{
			base.RestoreGodotObjectData(info);
			Variant variant;
			if (info.TryGetProperty(NStarCounter.PropertyName._label, ref variant))
			{
				this._label = variant.As<MegaRichTextLabel>();
			}
			Variant variant2;
			if (info.TryGetProperty(NStarCounter.PropertyName._rotationLayers, ref variant2))
			{
				this._rotationLayers = variant2.As<Control>();
			}
			Variant variant3;
			if (info.TryGetProperty(NStarCounter.PropertyName._icon, ref variant3))
			{
				this._icon = variant3.As<Control>();
			}
			Variant variant4;
			if (info.TryGetProperty(NStarCounter.PropertyName._hsv, ref variant4))
			{
				this._hsv = variant4.As<ShaderMaterial>();
			}
			Variant variant5;
			if (info.TryGetProperty(NStarCounter.PropertyName._lerpingStarCount, ref variant5))
			{
				this._lerpingStarCount = variant5.As<float>();
			}
			Variant variant6;
			if (info.TryGetProperty(NStarCounter.PropertyName._velocity, ref variant6))
			{
				this._velocity = variant6.As<float>();
			}
			Variant variant7;
			if (info.TryGetProperty(NStarCounter.PropertyName._displayedStarCount, ref variant7))
			{
				this._displayedStarCount = variant7.As<int>();
			}
			Variant variant8;
			if (info.TryGetProperty(NStarCounter.PropertyName._hsvTween, ref variant8))
			{
				this._hsvTween = variant8.As<Tween>();
			}
			Variant variant9;
			if (info.TryGetProperty(NStarCounter.PropertyName._isListeningToCombatState, ref variant9))
			{
				this._isListeningToCombatState = variant9.As<bool>();
			}
		}

		// Token: 0x0400195E RID: 6494
		private static readonly StringName _v = new StringName("v");

		// Token: 0x0400195F RID: 6495
		private static readonly StringName _s = new StringName("s");

		// Token: 0x04001960 RID: 6496
		private static readonly string _starGainVfxPath = SceneHelper.GetScenePath("vfx/star_gain_vfx");

		// Token: 0x04001961 RID: 6497
		[Nullable(2)]
		private Player _player;

		// Token: 0x04001962 RID: 6498
		private MegaRichTextLabel _label;

		// Token: 0x04001963 RID: 6499
		private Control _rotationLayers;

		// Token: 0x04001964 RID: 6500
		private Control _icon;

		// Token: 0x04001965 RID: 6501
		private ShaderMaterial _hsv;

		// Token: 0x04001966 RID: 6502
		private float _lerpingStarCount;

		// Token: 0x04001967 RID: 6503
		private float _velocity;

		// Token: 0x04001968 RID: 6504
		private int _displayedStarCount;

		// Token: 0x04001969 RID: 6505
		[Nullable(2)]
		private Tween _hsvTween;

		// Token: 0x0400196A RID: 6506
		private bool _isListeningToCombatState;

		// Token: 0x0400196B RID: 6507
		private HoverTip _hoverTip;

		/// <summary>
		/// Cached StringNames for the methods contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x02001927 RID: 6439
		[NullableContext(0)]
		public class MethodName : Control.MethodName
		{
			/// <summary>
			/// Cached name for the '_Ready' method.
			/// </summary>
			// Token: 0x040062EF RID: 25327
			public static readonly StringName _Ready = "_Ready";

			/// <summary>
			/// Cached name for the '_EnterTree' method.
			/// </summary>
			// Token: 0x040062F0 RID: 25328
			public static readonly StringName _EnterTree = "_EnterTree";

			/// <summary>
			/// Cached name for the '_ExitTree' method.
			/// </summary>
			// Token: 0x040062F1 RID: 25329
			public static readonly StringName _ExitTree = "_ExitTree";

			/// <summary>
			/// Cached name for the 'ConnectStarsChangedSignal' method.
			/// </summary>
			// Token: 0x040062F2 RID: 25330
			public static readonly StringName ConnectStarsChangedSignal = "ConnectStarsChangedSignal";

			/// <summary>
			/// Cached name for the 'OnHovered' method.
			/// </summary>
			// Token: 0x040062F3 RID: 25331
			public static readonly StringName OnHovered = "OnHovered";

			/// <summary>
			/// Cached name for the 'OnUnhovered' method.
			/// </summary>
			// Token: 0x040062F4 RID: 25332
			public static readonly StringName OnUnhovered = "OnUnhovered";

			/// <summary>
			/// Cached name for the 'OnStarsChanged' method.
			/// </summary>
			// Token: 0x040062F5 RID: 25333
			public static readonly StringName OnStarsChanged = "OnStarsChanged";

			/// <summary>
			/// Cached name for the '_Process' method.
			/// </summary>
			// Token: 0x040062F6 RID: 25334
			public static readonly StringName _Process = "_Process";

			/// <summary>
			/// Cached name for the 'UpdateStarCount' method.
			/// </summary>
			// Token: 0x040062F7 RID: 25335
			public static readonly StringName UpdateStarCount = "UpdateStarCount";

			/// <summary>
			/// Cached name for the 'SetStarCountText' method.
			/// </summary>
			// Token: 0x040062F8 RID: 25336
			public static readonly StringName SetStarCountText = "SetStarCountText";

			/// <summary>
			/// Cached name for the 'UpdateShaderV' method.
			/// </summary>
			// Token: 0x040062F9 RID: 25337
			public static readonly StringName UpdateShaderV = "UpdateShaderV";

			/// <summary>
			/// Cached name for the 'RefreshVisibility' method.
			/// </summary>
			// Token: 0x040062FA RID: 25338
			public static readonly StringName RefreshVisibility = "RefreshVisibility";
		}

		/// <summary>
		/// Cached StringNames for the properties and fields contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x02001928 RID: 6440
		[NullableContext(0)]
		public class PropertyName : Control.PropertyName
		{
			/// <summary>
			/// Cached name for the '_label' field.
			/// </summary>
			// Token: 0x040062FB RID: 25339
			public static readonly StringName _label = "_label";

			/// <summary>
			/// Cached name for the '_rotationLayers' field.
			/// </summary>
			// Token: 0x040062FC RID: 25340
			public static readonly StringName _rotationLayers = "_rotationLayers";

			/// <summary>
			/// Cached name for the '_icon' field.
			/// </summary>
			// Token: 0x040062FD RID: 25341
			public static readonly StringName _icon = "_icon";

			/// <summary>
			/// Cached name for the '_hsv' field.
			/// </summary>
			// Token: 0x040062FE RID: 25342
			public static readonly StringName _hsv = "_hsv";

			/// <summary>
			/// Cached name for the '_lerpingStarCount' field.
			/// </summary>
			// Token: 0x040062FF RID: 25343
			public static readonly StringName _lerpingStarCount = "_lerpingStarCount";

			/// <summary>
			/// Cached name for the '_velocity' field.
			/// </summary>
			// Token: 0x04006300 RID: 25344
			public static readonly StringName _velocity = "_velocity";

			/// <summary>
			/// Cached name for the '_displayedStarCount' field.
			/// </summary>
			// Token: 0x04006301 RID: 25345
			public static readonly StringName _displayedStarCount = "_displayedStarCount";

			/// <summary>
			/// Cached name for the '_hsvTween' field.
			/// </summary>
			// Token: 0x04006302 RID: 25346
			public static readonly StringName _hsvTween = "_hsvTween";

			/// <summary>
			/// Cached name for the '_isListeningToCombatState' field.
			/// </summary>
			// Token: 0x04006303 RID: 25347
			public static readonly StringName _isListeningToCombatState = "_isListeningToCombatState";
		}

		/// <summary>
		/// Cached StringNames for the signals contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x02001929 RID: 6441
		[NullableContext(0)]
		public class SignalName : Control.SignalName
		{
		}
	}
}
