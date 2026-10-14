using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using Godot;
using Godot.Bridge;
using Godot.NativeInterop;
using MegaCrit.Sts2.addons.mega_text;
using MegaCrit.Sts2.Core.Assets;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization;
using MegaCrit.Sts2.Core.Nodes.HoverTips;
using MegaCrit.Sts2.Core.Nodes.Vfx.Utilities;
using MegaCrit.Sts2.Core.TestSupport;

namespace MegaCrit.Sts2.Core.Nodes.Combat
{
	// Token: 0x02000401 RID: 1025
	[NullableContext(1)]
	[Nullable(0)]
	[ScriptPath("res://src/Core/Nodes/Combat/NEnergyCounter.cs")]
	public class NEnergyCounter : Control
	{
		// Token: 0x17000935 RID: 2357
		// (get) Token: 0x06003FCE RID: 16334 RVA: 0x001E9F01 File Offset: 0x001E8101
		public static IEnumerable<string> AssetPaths
		{
			get
			{
				return new <>z__ReadOnlySingleElementList<string>("res://materials/ui/energy_orb_dark.tres");
			}
		}

		// Token: 0x06003FCF RID: 16335 RVA: 0x001E9F10 File Offset: 0x001E8110
		[return: Nullable(2)]
		public static NEnergyCounter Create(Player player)
		{
			if (TestMode.IsOn)
			{
				return null;
			}
			NEnergyCounter nenergyCounter = PreloadManager.Cache.GetScene(player.Character.EnergyCounterPath).Instantiate<NEnergyCounter>(0L);
			nenergyCounter._player = player;
			return nenergyCounter;
		}

		// Token: 0x06003FD0 RID: 16336 RVA: 0x001E9F4C File Offset: 0x001E814C
		public override void _Ready()
		{
			this._label = base.GetNode<MegaLabel>("Label");
			this._layers = base.GetNode<Control>("%Layers");
			this._rotationLayers = base.GetNode<Control>("%RotationLayers");
			this._backVfx = base.GetNode<NParticlesContainer>("%EnergyVfxBack");
			this._frontVfx = base.GetNode<NParticlesContainer>("%EnergyVfxFront");
			LocString locString = new LocString("static_hover_tips", "ENERGY_COUNT.description");
			locString.Add("energyPrefix", EnergyIconHelper.GetPrefix(this._player.Character.CardPool));
			this._hoverTip = new HoverTip(new LocString("static_hover_tips", "ENERGY_COUNT.title"), locString, null);
			base.Connect(Control.SignalName.MouseEntered, Callable.From(new Action(this.OnHovered)), 0U);
			base.Connect(Control.SignalName.MouseExited, Callable.From(new Action(this.OnUnhovered)), 0U);
			this.RefreshLabel();
		}

		// Token: 0x06003FD1 RID: 16337 RVA: 0x001EA055 File Offset: 0x001E8255
		public override void _EnterTree()
		{
			base._EnterTree();
			CombatManager.Instance.StateTracker.CombatStateChanged += this.OnCombatStateChanged;
			this._player.PlayerCombatState.EnergyChanged += this.OnEnergyChanged;
		}

		// Token: 0x06003FD2 RID: 16338 RVA: 0x001EA094 File Offset: 0x001E8294
		public override void _ExitTree()
		{
			base._ExitTree();
			CombatManager.Instance.StateTracker.CombatStateChanged -= this.OnCombatStateChanged;
			this._player.PlayerCombatState.EnergyChanged -= this.OnEnergyChanged;
		}

		// Token: 0x06003FD3 RID: 16339 RVA: 0x001EA0D4 File Offset: 0x001E82D4
		private void OnHovered()
		{
			NHoverTipSet nhoverTipSet = NHoverTipSet.CreateAndShow(this, this._hoverTip, HoverTipAlignment.None);
			if (nhoverTipSet != null)
			{
				nhoverTipSet.SetGlobalPosition(base.GlobalPosition + new Vector2(-70f, -200f), false);
			}
		}

		// Token: 0x06003FD4 RID: 16340 RVA: 0x001EA118 File Offset: 0x001E8318
		private void OnUnhovered()
		{
			NHoverTipSet.Remove(this);
		}

		// Token: 0x06003FD5 RID: 16341 RVA: 0x001EA120 File Offset: 0x001E8320
		private void OnCombatStateChanged(CombatState combatState)
		{
			this.RefreshLabel();
		}

		// Token: 0x06003FD6 RID: 16342 RVA: 0x001EA128 File Offset: 0x001E8328
		private void RefreshLabel()
		{
			PlayerCombatState playerCombatState = this._player.PlayerCombatState;
			MegaLabel label = this._label;
			DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(1, 2);
			defaultInterpolatedStringHandler.AppendFormatted<int>(playerCombatState.Energy);
			defaultInterpolatedStringHandler.AppendLiteral("/");
			defaultInterpolatedStringHandler.AppendFormatted<int>(playerCombatState.MaxEnergy);
			label.SetTextAutoSize(defaultInterpolatedStringHandler.ToStringAndClear());
			this._label.AddThemeColorOverride(ThemeConstants.Label.FontColor, (playerCombatState.Energy == 0) ? StsColors.red : StsColors.cream);
			this._label.AddThemeColorOverride(ThemeConstants.Label.FontOutlineColor, (playerCombatState.Energy == 0) ? StsColors.unplayableEnergyCostOutline : this.OutlineColor);
			Material material = ((playerCombatState.Energy == 0) ? PreloadManager.Cache.GetMaterial("res://materials/ui/energy_orb_dark.tres") : null);
			foreach (Control control in this._layers.GetChildren(false).OfType<Control>())
			{
				control.Material = material;
			}
			foreach (Control control2 in this._rotationLayers.GetChildren(false).OfType<Control>())
			{
				control2.Material = material;
			}
			this._layers.Modulate = ((playerCombatState.Energy == 0) ? Colors.DarkGray : Colors.White);
		}

		// Token: 0x06003FD7 RID: 16343 RVA: 0x001EA2A0 File Offset: 0x001E84A0
		private void OnEnergyChanged(int oldEnergy, int newEnergy)
		{
			if (oldEnergy >= newEnergy)
			{
				return;
			}
			NParticlesContainer backVfx = this._backVfx;
			if (backVfx != null)
			{
				backVfx.Restart();
			}
			NParticlesContainer frontVfx = this._frontVfx;
			if (frontVfx == null)
			{
				return;
			}
			frontVfx.Restart();
		}

		// Token: 0x17000936 RID: 2358
		// (get) Token: 0x06003FD8 RID: 16344 RVA: 0x001EA2C8 File Offset: 0x001E84C8
		private Color OutlineColor
		{
			get
			{
				return this._player.Character.EnergyLabelOutlineColor;
			}
		}

		// Token: 0x06003FD9 RID: 16345 RVA: 0x001EA2DC File Offset: 0x001E84DC
		public override void _Process(double delta)
		{
			float num = ((this._player.PlayerCombatState.Energy == 0) ? 5f : 30f);
			for (int i = 0; i < this._rotationLayers.GetChildCount(false); i++)
			{
				this._rotationLayers.GetChild<Control>(i, false).RotationDegrees += (float)delta * num * (float)(i + 1);
			}
		}

		// Token: 0x06003FDA RID: 16346 RVA: 0x001EA344 File Offset: 0x001E8544
		public void AnimIn()
		{
			Tween animOutTween = this._animOutTween;
			if (animOutTween != null)
			{
				animOutTween.Kill();
			}
			this._animInTween = base.CreateTween();
			base.Position = NEnergyCounter._hidePosition;
			this._animInTween.TweenProperty(this, "position", NEnergyCounter._showPosition, 0.6000000238418579).SetEase(1L).SetTrans(5L);
		}

		// Token: 0x06003FDB RID: 16347 RVA: 0x001EA3B4 File Offset: 0x001E85B4
		public void AnimOut()
		{
			Tween animInTween = this._animInTween;
			if (animInTween != null)
			{
				animInTween.Kill();
			}
			this._animOutTween = base.CreateTween();
			base.Position = NEnergyCounter._showPosition;
			this._animOutTween.TweenProperty(this, "position", NEnergyCounter._hidePosition, 0.6000000238418579).SetEase(0L).SetTrans(10L);
		}

		/// <summary>
		/// Get the method information for all the methods declared in this class.
		/// This method is used by Godot to register the available methods in the editor.
		/// Do not call this method.
		/// </summary>
		// Token: 0x06003FDC RID: 16348 RVA: 0x001EA424 File Offset: 0x001E8624
		[NullableContext(0)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal static List<MethodInfo> GetGodotMethodList()
		{
			return new List<MethodInfo>(10)
			{
				new MethodInfo(NEnergyCounter.MethodName._Ready, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NEnergyCounter.MethodName._EnterTree, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NEnergyCounter.MethodName._ExitTree, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NEnergyCounter.MethodName.OnHovered, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NEnergyCounter.MethodName.OnUnhovered, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NEnergyCounter.MethodName.RefreshLabel, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NEnergyCounter.MethodName.OnEnergyChanged, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, new List<PropertyInfo>
				{
					new PropertyInfo(2L, "oldEnergy", 0L, "", 6L, false),
					new PropertyInfo(2L, "newEnergy", 0L, "", 6L, false)
				}, null),
				new MethodInfo(NEnergyCounter.MethodName._Process, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, new List<PropertyInfo>
				{
					new PropertyInfo(3L, "delta", 0L, "", 6L, false)
				}, null),
				new MethodInfo(NEnergyCounter.MethodName.AnimIn, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NEnergyCounter.MethodName.AnimOut, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null)
			};
		}

		/// <inheritdoc />
		// Token: 0x06003FDD RID: 16349 RVA: 0x001EA67C File Offset: 0x001E887C
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool InvokeGodotClassMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
		{
			if ((ref method) == NEnergyCounter.MethodName._Ready && args.Count == 0)
			{
				this._Ready();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NEnergyCounter.MethodName._EnterTree && args.Count == 0)
			{
				this._EnterTree();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NEnergyCounter.MethodName._ExitTree && args.Count == 0)
			{
				this._ExitTree();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NEnergyCounter.MethodName.OnHovered && args.Count == 0)
			{
				this.OnHovered();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NEnergyCounter.MethodName.OnUnhovered && args.Count == 0)
			{
				this.OnUnhovered();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NEnergyCounter.MethodName.RefreshLabel && args.Count == 0)
			{
				this.RefreshLabel();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NEnergyCounter.MethodName.OnEnergyChanged && args.Count == 2)
			{
				this.OnEnergyChanged(VariantUtils.ConvertTo<int>(args[0]), VariantUtils.ConvertTo<int>(args[1]));
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NEnergyCounter.MethodName._Process && args.Count == 1)
			{
				this._Process(VariantUtils.ConvertTo<double>(args[0]));
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NEnergyCounter.MethodName.AnimIn && args.Count == 0)
			{
				this.AnimIn();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NEnergyCounter.MethodName.AnimOut && args.Count == 0)
			{
				this.AnimOut();
				ret = default(godot_variant);
				return true;
			}
			return base.InvokeGodotClassMethod(ref method, args, ref ret);
		}

		/// <inheritdoc />
		// Token: 0x06003FDE RID: 16350 RVA: 0x001EA830 File Offset: 0x001E8A30
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool HasGodotClassMethod(in godot_string_name method)
		{
			return (ref method) == NEnergyCounter.MethodName._Ready || (ref method) == NEnergyCounter.MethodName._EnterTree || (ref method) == NEnergyCounter.MethodName._ExitTree || (ref method) == NEnergyCounter.MethodName.OnHovered || (ref method) == NEnergyCounter.MethodName.OnUnhovered || (ref method) == NEnergyCounter.MethodName.RefreshLabel || (ref method) == NEnergyCounter.MethodName.OnEnergyChanged || (ref method) == NEnergyCounter.MethodName._Process || (ref method) == NEnergyCounter.MethodName.AnimIn || (ref method) == NEnergyCounter.MethodName.AnimOut || base.HasGodotClassMethod(ref method);
		}

		/// <inheritdoc />
		// Token: 0x06003FDF RID: 16351 RVA: 0x001EA8DC File Offset: 0x001E8ADC
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool SetGodotClassPropertyValue(in godot_string_name name, in godot_variant value)
		{
			if ((ref name) == NEnergyCounter.PropertyName._label)
			{
				this._label = VariantUtils.ConvertTo<MegaLabel>(ref value);
				return true;
			}
			if ((ref name) == NEnergyCounter.PropertyName._layers)
			{
				this._layers = VariantUtils.ConvertTo<Control>(ref value);
				return true;
			}
			if ((ref name) == NEnergyCounter.PropertyName._rotationLayers)
			{
				this._rotationLayers = VariantUtils.ConvertTo<Control>(ref value);
				return true;
			}
			if ((ref name) == NEnergyCounter.PropertyName._backVfx)
			{
				this._backVfx = VariantUtils.ConvertTo<NParticlesContainer>(ref value);
				return true;
			}
			if ((ref name) == NEnergyCounter.PropertyName._frontVfx)
			{
				this._frontVfx = VariantUtils.ConvertTo<NParticlesContainer>(ref value);
				return true;
			}
			if ((ref name) == NEnergyCounter.PropertyName._animInTween)
			{
				this._animInTween = VariantUtils.ConvertTo<Tween>(ref value);
				return true;
			}
			if ((ref name) == NEnergyCounter.PropertyName._animOutTween)
			{
				this._animOutTween = VariantUtils.ConvertTo<Tween>(ref value);
				return true;
			}
			return base.SetGodotClassPropertyValue(ref name, ref value);
		}

		/// <inheritdoc />
		// Token: 0x06003FE0 RID: 16352 RVA: 0x001EA9B0 File Offset: 0x001E8BB0
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
		{
			if ((ref name) == NEnergyCounter.PropertyName.OutlineColor)
			{
				Color outlineColor = this.OutlineColor;
				value = VariantUtils.CreateFrom<Color>(ref outlineColor);
				return true;
			}
			if ((ref name) == NEnergyCounter.PropertyName._label)
			{
				value = VariantUtils.CreateFrom<MegaLabel>(ref this._label);
				return true;
			}
			if ((ref name) == NEnergyCounter.PropertyName._layers)
			{
				value = VariantUtils.CreateFrom<Control>(ref this._layers);
				return true;
			}
			if ((ref name) == NEnergyCounter.PropertyName._rotationLayers)
			{
				value = VariantUtils.CreateFrom<Control>(ref this._rotationLayers);
				return true;
			}
			if ((ref name) == NEnergyCounter.PropertyName._backVfx)
			{
				value = VariantUtils.CreateFrom<NParticlesContainer>(ref this._backVfx);
				return true;
			}
			if ((ref name) == NEnergyCounter.PropertyName._frontVfx)
			{
				value = VariantUtils.CreateFrom<NParticlesContainer>(ref this._frontVfx);
				return true;
			}
			if ((ref name) == NEnergyCounter.PropertyName._animInTween)
			{
				value = VariantUtils.CreateFrom<Tween>(ref this._animInTween);
				return true;
			}
			if ((ref name) == NEnergyCounter.PropertyName._animOutTween)
			{
				value = VariantUtils.CreateFrom<Tween>(ref this._animOutTween);
				return true;
			}
			return base.GetGodotClassPropertyValue(ref name, ref value);
		}

		/// <summary>
		/// Get the property information for all the properties declared in this class.
		/// This method is used by Godot to register the available properties in the editor.
		/// Do not call this method.
		/// </summary>
		// Token: 0x06003FE1 RID: 16353 RVA: 0x001EAAC8 File Offset: 0x001E8CC8
		[NullableContext(0)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal static List<PropertyInfo> GetGodotPropertyList()
		{
			return new List<PropertyInfo>
			{
				new PropertyInfo(24L, NEnergyCounter.PropertyName._label, 0L, "", 4096L, false),
				new PropertyInfo(24L, NEnergyCounter.PropertyName._layers, 0L, "", 4096L, false),
				new PropertyInfo(24L, NEnergyCounter.PropertyName._rotationLayers, 0L, "", 4096L, false),
				new PropertyInfo(24L, NEnergyCounter.PropertyName._backVfx, 0L, "", 4096L, false),
				new PropertyInfo(24L, NEnergyCounter.PropertyName._frontVfx, 0L, "", 4096L, false),
				new PropertyInfo(24L, NEnergyCounter.PropertyName._animInTween, 0L, "", 4096L, false),
				new PropertyInfo(24L, NEnergyCounter.PropertyName._animOutTween, 0L, "", 4096L, false),
				new PropertyInfo(20L, NEnergyCounter.PropertyName.OutlineColor, 0L, "", 4096L, false)
			};
		}

		/// <inheritdoc />
		// Token: 0x06003FE2 RID: 16354 RVA: 0x001EABE4 File Offset: 0x001E8DE4
		[NullableContext(0)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void SaveGodotObjectData(GodotSerializationInfo info)
		{
			base.SaveGodotObjectData(info);
			info.AddProperty(NEnergyCounter.PropertyName._label, Variant.From<MegaLabel>(ref this._label));
			info.AddProperty(NEnergyCounter.PropertyName._layers, Variant.From<Control>(ref this._layers));
			info.AddProperty(NEnergyCounter.PropertyName._rotationLayers, Variant.From<Control>(ref this._rotationLayers));
			info.AddProperty(NEnergyCounter.PropertyName._backVfx, Variant.From<NParticlesContainer>(ref this._backVfx));
			info.AddProperty(NEnergyCounter.PropertyName._frontVfx, Variant.From<NParticlesContainer>(ref this._frontVfx));
			info.AddProperty(NEnergyCounter.PropertyName._animInTween, Variant.From<Tween>(ref this._animInTween));
			info.AddProperty(NEnergyCounter.PropertyName._animOutTween, Variant.From<Tween>(ref this._animOutTween));
		}

		/// <inheritdoc />
		// Token: 0x06003FE3 RID: 16355 RVA: 0x001EAC94 File Offset: 0x001E8E94
		[NullableContext(0)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void RestoreGodotObjectData(GodotSerializationInfo info)
		{
			base.RestoreGodotObjectData(info);
			Variant variant;
			if (info.TryGetProperty(NEnergyCounter.PropertyName._label, ref variant))
			{
				this._label = variant.As<MegaLabel>();
			}
			Variant variant2;
			if (info.TryGetProperty(NEnergyCounter.PropertyName._layers, ref variant2))
			{
				this._layers = variant2.As<Control>();
			}
			Variant variant3;
			if (info.TryGetProperty(NEnergyCounter.PropertyName._rotationLayers, ref variant3))
			{
				this._rotationLayers = variant3.As<Control>();
			}
			Variant variant4;
			if (info.TryGetProperty(NEnergyCounter.PropertyName._backVfx, ref variant4))
			{
				this._backVfx = variant4.As<NParticlesContainer>();
			}
			Variant variant5;
			if (info.TryGetProperty(NEnergyCounter.PropertyName._frontVfx, ref variant5))
			{
				this._frontVfx = variant5.As<NParticlesContainer>();
			}
			Variant variant6;
			if (info.TryGetProperty(NEnergyCounter.PropertyName._animInTween, ref variant6))
			{
				this._animInTween = variant6.As<Tween>();
			}
			Variant variant7;
			if (info.TryGetProperty(NEnergyCounter.PropertyName._animOutTween, ref variant7))
			{
				this._animOutTween = variant7.As<Tween>();
			}
		}

		// Token: 0x040018BA RID: 6330
		private const string _darkenedMatPath = "res://materials/ui/energy_orb_dark.tres";

		// Token: 0x040018BB RID: 6331
		private Player _player;

		// Token: 0x040018BC RID: 6332
		private MegaLabel _label;

		// Token: 0x040018BD RID: 6333
		private Control _layers;

		// Token: 0x040018BE RID: 6334
		private Control _rotationLayers;

		// Token: 0x040018BF RID: 6335
		[Nullable(2)]
		private NParticlesContainer _backVfx;

		// Token: 0x040018C0 RID: 6336
		[Nullable(2)]
		private NParticlesContainer _frontVfx;

		// Token: 0x040018C1 RID: 6337
		private HoverTip _hoverTip;

		// Token: 0x040018C2 RID: 6338
		[Nullable(2)]
		private Tween _animInTween;

		// Token: 0x040018C3 RID: 6339
		[Nullable(2)]
		private Tween _animOutTween;

		// Token: 0x040018C4 RID: 6340
		private const float _animDuration = 0.6f;

		// Token: 0x040018C5 RID: 6341
		private static readonly Vector2 _showPosition = Vector2.Zero;

		// Token: 0x040018C6 RID: 6342
		private static readonly Vector2 _hidePosition = new Vector2(-480f, 128f);

		/// <summary>
		/// Cached StringNames for the methods contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x020018E8 RID: 6376
		[NullableContext(0)]
		public class MethodName : Control.MethodName
		{
			/// <summary>
			/// Cached name for the '_Ready' method.
			/// </summary>
			// Token: 0x040061A0 RID: 24992
			public static readonly StringName _Ready = "_Ready";

			/// <summary>
			/// Cached name for the '_EnterTree' method.
			/// </summary>
			// Token: 0x040061A1 RID: 24993
			public static readonly StringName _EnterTree = "_EnterTree";

			/// <summary>
			/// Cached name for the '_ExitTree' method.
			/// </summary>
			// Token: 0x040061A2 RID: 24994
			public static readonly StringName _ExitTree = "_ExitTree";

			/// <summary>
			/// Cached name for the 'OnHovered' method.
			/// </summary>
			// Token: 0x040061A3 RID: 24995
			public static readonly StringName OnHovered = "OnHovered";

			/// <summary>
			/// Cached name for the 'OnUnhovered' method.
			/// </summary>
			// Token: 0x040061A4 RID: 24996
			public static readonly StringName OnUnhovered = "OnUnhovered";

			/// <summary>
			/// Cached name for the 'RefreshLabel' method.
			/// </summary>
			// Token: 0x040061A5 RID: 24997
			public static readonly StringName RefreshLabel = "RefreshLabel";

			/// <summary>
			/// Cached name for the 'OnEnergyChanged' method.
			/// </summary>
			// Token: 0x040061A6 RID: 24998
			public static readonly StringName OnEnergyChanged = "OnEnergyChanged";

			/// <summary>
			/// Cached name for the '_Process' method.
			/// </summary>
			// Token: 0x040061A7 RID: 24999
			public static readonly StringName _Process = "_Process";

			/// <summary>
			/// Cached name for the 'AnimIn' method.
			/// </summary>
			// Token: 0x040061A8 RID: 25000
			public static readonly StringName AnimIn = "AnimIn";

			/// <summary>
			/// Cached name for the 'AnimOut' method.
			/// </summary>
			// Token: 0x040061A9 RID: 25001
			public static readonly StringName AnimOut = "AnimOut";
		}

		/// <summary>
		/// Cached StringNames for the properties and fields contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x020018E9 RID: 6377
		[NullableContext(0)]
		public class PropertyName : Control.PropertyName
		{
			/// <summary>
			/// Cached name for the 'OutlineColor' property.
			/// </summary>
			// Token: 0x040061AA RID: 25002
			public static readonly StringName OutlineColor = "OutlineColor";

			/// <summary>
			/// Cached name for the '_label' field.
			/// </summary>
			// Token: 0x040061AB RID: 25003
			public static readonly StringName _label = "_label";

			/// <summary>
			/// Cached name for the '_layers' field.
			/// </summary>
			// Token: 0x040061AC RID: 25004
			public static readonly StringName _layers = "_layers";

			/// <summary>
			/// Cached name for the '_rotationLayers' field.
			/// </summary>
			// Token: 0x040061AD RID: 25005
			public static readonly StringName _rotationLayers = "_rotationLayers";

			/// <summary>
			/// Cached name for the '_backVfx' field.
			/// </summary>
			// Token: 0x040061AE RID: 25006
			public static readonly StringName _backVfx = "_backVfx";

			/// <summary>
			/// Cached name for the '_frontVfx' field.
			/// </summary>
			// Token: 0x040061AF RID: 25007
			public static readonly StringName _frontVfx = "_frontVfx";

			/// <summary>
			/// Cached name for the '_animInTween' field.
			/// </summary>
			// Token: 0x040061B0 RID: 25008
			public static readonly StringName _animInTween = "_animInTween";

			/// <summary>
			/// Cached name for the '_animOutTween' field.
			/// </summary>
			// Token: 0x040061B1 RID: 25009
			public static readonly StringName _animOutTween = "_animOutTween";
		}

		/// <summary>
		/// Cached StringNames for the signals contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x020018EA RID: 6378
		[NullableContext(0)]
		public class SignalName : Control.SignalName
		{
		}
	}
}
