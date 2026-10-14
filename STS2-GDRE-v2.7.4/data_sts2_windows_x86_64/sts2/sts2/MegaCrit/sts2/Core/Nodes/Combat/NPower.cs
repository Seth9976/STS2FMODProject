using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Godot;
using Godot.Bridge;
using Godot.NativeInterop;
using MegaCrit.Sts2.addons.mega_text;
using MegaCrit.Sts2.Core.Assets;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Nodes.Rooms;

namespace MegaCrit.Sts2.Core.Nodes.Combat
{
	// Token: 0x0200040A RID: 1034
	[NullableContext(1)]
	[Nullable(0)]
	[ScriptPath("res://src/Core/Nodes/Combat/NPower.cs")]
	public class NPower : Control
	{
		// Token: 0x17000951 RID: 2385
		// (get) Token: 0x060040F4 RID: 16628 RVA: 0x001F6009 File Offset: 0x001F4209
		// (set) Token: 0x060040F5 RID: 16629 RVA: 0x001F6011 File Offset: 0x001F4211
		public NPowerContainer Container { get; set; }

		// Token: 0x17000952 RID: 2386
		// (get) Token: 0x060040F6 RID: 16630 RVA: 0x001F601A File Offset: 0x001F421A
		private static string ScenePath
		{
			get
			{
				return SceneHelper.GetScenePath("combat/power");
			}
		}

		// Token: 0x17000953 RID: 2387
		// (get) Token: 0x060040F7 RID: 16631 RVA: 0x001F6026 File Offset: 0x001F4226
		public static IEnumerable<string> AssetPaths
		{
			get
			{
				return new <>z__ReadOnlySingleElementList<string>(NPower.ScenePath);
			}
		}

		// Token: 0x17000954 RID: 2388
		// (get) Token: 0x060040F8 RID: 16632 RVA: 0x001F6032 File Offset: 0x001F4232
		// (set) Token: 0x060040F9 RID: 16633 RVA: 0x001F6049 File Offset: 0x001F4249
		public PowerModel Model
		{
			get
			{
				PowerModel model = this._model;
				if (model == null)
				{
					throw new InvalidOperationException("Model was accessed before it was set.");
				}
				return model;
			}
			set
			{
				if (this._model != null)
				{
					this.UnsubscribeFromModelEvents();
				}
				value.AssertMutable();
				this._model = value;
				if (this._model != null && base.IsInsideTree())
				{
					this.SubscribeToModelEvents();
				}
				this.Reload();
			}
		}

		// Token: 0x060040FA RID: 16634 RVA: 0x001F6084 File Offset: 0x001F4284
		public static NPower Create(PowerModel power)
		{
			NPower npower = PreloadManager.Cache.GetScene(NPower.ScenePath).Instantiate<NPower>(0L);
			npower.Model = power;
			return npower;
		}

		// Token: 0x060040FB RID: 16635 RVA: 0x001F60B0 File Offset: 0x001F42B0
		public override void _Ready()
		{
			this._icon = base.GetNode<TextureRect>("%Icon");
			this._amountLabel = base.GetNode<MegaLabel>("%AmountLabel");
			this._powerFlash = base.GetNode<CpuParticles2D>("%PowerFlash");
			base.Connect(Control.SignalName.MouseEntered, Callable.From(new Action(this.OnHovered)), 0U);
			base.Connect(Control.SignalName.MouseExited, Callable.From(new Action(this.OnUnhovered)), 0U);
			this.Reload();
			Tween animInTween = this._animInTween;
			if (animInTween != null)
			{
				animInTween.Kill();
			}
			this._animInTween = base.CreateTween().SetParallel(true);
			this._animInTween.TweenProperty(this._icon, "position:y", 0f, 0.5).SetEase(1L).SetTrans(10L)
				.From(-24f);
			this._animInTween.TweenProperty(this, "modulate:a", 1f, 0.5).SetEase(1L).SetTrans(5L);
		}

		// Token: 0x060040FC RID: 16636 RVA: 0x001F61E9 File Offset: 0x001F43E9
		public override void _EnterTree()
		{
			this.SubscribeToModelEvents();
		}

		// Token: 0x060040FD RID: 16637 RVA: 0x001F61F1 File Offset: 0x001F43F1
		public override void _ExitTree()
		{
			this.UnsubscribeFromModelEvents();
		}

		// Token: 0x060040FE RID: 16638 RVA: 0x001F61FC File Offset: 0x001F43FC
		private void Reload()
		{
			if (!base.IsNodeReady())
			{
				return;
			}
			TextureRect icon = this._icon;
			PowerModel model = this._model;
			icon.Texture = ((model != null) ? model.Icon : null);
			CpuParticles2D powerFlash = this._powerFlash;
			PowerModel model2 = this._model;
			powerFlash.Texture = ((model2 != null) ? model2.BigIcon : null);
			this.RefreshAmount();
		}

		// Token: 0x060040FF RID: 16639 RVA: 0x001F6254 File Offset: 0x001F4454
		private void OnPulsingStarted()
		{
			ShaderMaterial shaderMaterial = (ShaderMaterial)this._icon.Material;
			shaderMaterial.SetShaderParameter(NPower._pulse, 1);
		}

		// Token: 0x06004100 RID: 16640 RVA: 0x001F6284 File Offset: 0x001F4484
		private void OnPulsingStopped()
		{
			ShaderMaterial shaderMaterial = (ShaderMaterial)this._icon.Material;
			shaderMaterial.SetShaderParameter(NPower._pulse, 0);
		}

		// Token: 0x06004101 RID: 16641 RVA: 0x001F62B4 File Offset: 0x001F44B4
		private void RefreshAmount()
		{
			if (this._model != null)
			{
				this._amountLabel.AddThemeColorOverride(ThemeConstants.Label.FontColor, this.Model.AmountLabelColor);
				this._amountLabel.SetTextAutoSize((this.Model.StackType == PowerStackType.Counter) ? this.Model.DisplayAmount.ToString() : string.Empty);
				return;
			}
			this._amountLabel.SetTextAutoSize(string.Empty);
		}

		// Token: 0x06004102 RID: 16642 RVA: 0x001F6328 File Offset: 0x001F4528
		private void OnDisplayAmountChanged()
		{
			this.FlashPower();
			this.RefreshAmount();
		}

		// Token: 0x06004103 RID: 16643 RVA: 0x001F6336 File Offset: 0x001F4536
		private void OnPowerFlashed(PowerModel _)
		{
			this.FlashPower();
		}

		// Token: 0x06004104 RID: 16644 RVA: 0x001F633E File Offset: 0x001F453E
		private void FlashPower()
		{
			this._powerFlash.Emitting = true;
		}

		// Token: 0x06004105 RID: 16645 RVA: 0x001F634C File Offset: 0x001F454C
		private void OnHovered()
		{
			NCombatRoom instance = NCombatRoom.Instance;
			if (instance != null)
			{
				NCreature creatureNode = instance.GetCreatureNode(this.Model.Owner);
				if (creatureNode != null)
				{
					creatureNode.ShowHoverTips(this.Model.HoverTips);
				}
			}
			this._icon.Scale = Vector2.One * 1.1f;
			CombatManager.Instance.StateTracker.CombatStateChanged += this.ShowPowerHoverTips;
		}

		// Token: 0x06004106 RID: 16646 RVA: 0x001F63C0 File Offset: 0x001F45C0
		private void OnUnhovered()
		{
			NCombatRoom instance = NCombatRoom.Instance;
			if (instance != null)
			{
				NCreature creatureNode = instance.GetCreatureNode(this.Model.Owner);
				if (creatureNode != null)
				{
					creatureNode.HideHoverTips();
				}
			}
			this._icon.Scale = Vector2.One * 1f;
			CombatManager.Instance.StateTracker.CombatStateChanged -= this.ShowPowerHoverTips;
		}

		// Token: 0x06004107 RID: 16647 RVA: 0x001F6428 File Offset: 0x001F4628
		private void ShowPowerHoverTips(CombatState _)
		{
			NCombatRoom instance = NCombatRoom.Instance;
			if (instance == null)
			{
				return;
			}
			NCreature creatureNode = instance.GetCreatureNode(this.Model.Owner);
			if (creatureNode == null)
			{
				return;
			}
			creatureNode.ShowHoverTips(this.Model.HoverTips);
		}

		// Token: 0x06004108 RID: 16648 RVA: 0x001F645C File Offset: 0x001F465C
		private void SubscribeToModelEvents()
		{
			if (this._model == null)
			{
				return;
			}
			this.Model.DisplayAmountChanged += this.OnDisplayAmountChanged;
			this.Model.Flashed += this.OnPowerFlashed;
			this.Model.Removed += this.OnPowerRemoved;
			this.Model.Owner.Died += this.OnOwnerDied;
			this.Model.Owner.Revived += this.OnOwnerRevived;
			this.Model.PulsingStarted += this.OnPulsingStarted;
			this.Model.PulsingStopped += this.OnPulsingStopped;
		}

		// Token: 0x06004109 RID: 16649 RVA: 0x001F6520 File Offset: 0x001F4720
		private void UnsubscribeFromModelEvents()
		{
			if (this._model == null)
			{
				return;
			}
			this.Model.DisplayAmountChanged -= this.OnDisplayAmountChanged;
			this.Model.Flashed -= this.OnPowerFlashed;
			this.Model.Removed -= this.OnPowerRemoved;
			this.Model.Owner.Died -= this.OnOwnerDied;
			this.Model.Owner.Revived -= this.OnOwnerRevived;
			this.Model.PulsingStarted -= this.OnPulsingStarted;
			this.Model.PulsingStopped -= this.OnPulsingStopped;
		}

		// Token: 0x0600410A RID: 16650 RVA: 0x001F65E1 File Offset: 0x001F47E1
		private void OnPowerRemoved()
		{
			this.UnsubscribeFromModelEvents();
		}

		// Token: 0x0600410B RID: 16651 RVA: 0x001F65E9 File Offset: 0x001F47E9
		private void OnOwnerDied(Creature _)
		{
			if (!GodotObject.IsInstanceValid(this))
			{
				return;
			}
			if (!this.Model.ShouldPowerBeRemovedAfterOwnerDeath())
			{
				return;
			}
			base.MouseFilter = 2L;
		}

		// Token: 0x0600410C RID: 16652 RVA: 0x001F660A File Offset: 0x001F480A
		private void OnOwnerRevived(Creature _)
		{
			base.MouseFilter = 0L;
		}

		/// <summary>
		/// Get the method information for all the methods declared in this class.
		/// This method is used by Godot to register the available methods in the editor.
		/// Do not call this method.
		/// </summary>
		// Token: 0x0600410D RID: 16653 RVA: 0x001F6614 File Offset: 0x001F4814
		[NullableContext(0)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal static List<MethodInfo> GetGodotMethodList()
		{
			return new List<MethodInfo>(14)
			{
				new MethodInfo(NPower.MethodName._Ready, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NPower.MethodName._EnterTree, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NPower.MethodName._ExitTree, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NPower.MethodName.Reload, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NPower.MethodName.OnPulsingStarted, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NPower.MethodName.OnPulsingStopped, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NPower.MethodName.RefreshAmount, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NPower.MethodName.OnDisplayAmountChanged, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NPower.MethodName.FlashPower, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NPower.MethodName.OnHovered, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NPower.MethodName.OnUnhovered, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NPower.MethodName.SubscribeToModelEvents, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NPower.MethodName.UnsubscribeFromModelEvents, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NPower.MethodName.OnPowerRemoved, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null)
			};
		}

		/// <inheritdoc />
		// Token: 0x0600410E RID: 16654 RVA: 0x001F68BC File Offset: 0x001F4ABC
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool InvokeGodotClassMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
		{
			if ((ref method) == NPower.MethodName._Ready && args.Count == 0)
			{
				this._Ready();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NPower.MethodName._EnterTree && args.Count == 0)
			{
				this._EnterTree();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NPower.MethodName._ExitTree && args.Count == 0)
			{
				this._ExitTree();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NPower.MethodName.Reload && args.Count == 0)
			{
				this.Reload();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NPower.MethodName.OnPulsingStarted && args.Count == 0)
			{
				this.OnPulsingStarted();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NPower.MethodName.OnPulsingStopped && args.Count == 0)
			{
				this.OnPulsingStopped();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NPower.MethodName.RefreshAmount && args.Count == 0)
			{
				this.RefreshAmount();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NPower.MethodName.OnDisplayAmountChanged && args.Count == 0)
			{
				this.OnDisplayAmountChanged();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NPower.MethodName.FlashPower && args.Count == 0)
			{
				this.FlashPower();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NPower.MethodName.OnHovered && args.Count == 0)
			{
				this.OnHovered();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NPower.MethodName.OnUnhovered && args.Count == 0)
			{
				this.OnUnhovered();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NPower.MethodName.SubscribeToModelEvents && args.Count == 0)
			{
				this.SubscribeToModelEvents();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NPower.MethodName.UnsubscribeFromModelEvents && args.Count == 0)
			{
				this.UnsubscribeFromModelEvents();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NPower.MethodName.OnPowerRemoved && args.Count == 0)
			{
				this.OnPowerRemoved();
				ret = default(godot_variant);
				return true;
			}
			return base.InvokeGodotClassMethod(ref method, args, ref ret);
		}

		/// <inheritdoc />
		// Token: 0x0600410F RID: 16655 RVA: 0x001F6AD8 File Offset: 0x001F4CD8
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool HasGodotClassMethod(in godot_string_name method)
		{
			return (ref method) == NPower.MethodName._Ready || (ref method) == NPower.MethodName._EnterTree || (ref method) == NPower.MethodName._ExitTree || (ref method) == NPower.MethodName.Reload || (ref method) == NPower.MethodName.OnPulsingStarted || (ref method) == NPower.MethodName.OnPulsingStopped || (ref method) == NPower.MethodName.RefreshAmount || (ref method) == NPower.MethodName.OnDisplayAmountChanged || (ref method) == NPower.MethodName.FlashPower || (ref method) == NPower.MethodName.OnHovered || (ref method) == NPower.MethodName.OnUnhovered || (ref method) == NPower.MethodName.SubscribeToModelEvents || (ref method) == NPower.MethodName.UnsubscribeFromModelEvents || (ref method) == NPower.MethodName.OnPowerRemoved || base.HasGodotClassMethod(ref method);
		}

		/// <inheritdoc />
		// Token: 0x06004110 RID: 16656 RVA: 0x001F6BC0 File Offset: 0x001F4DC0
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool SetGodotClassPropertyValue(in godot_string_name name, in godot_variant value)
		{
			if ((ref name) == NPower.PropertyName.Container)
			{
				this.Container = VariantUtils.ConvertTo<NPowerContainer>(ref value);
				return true;
			}
			if ((ref name) == NPower.PropertyName._icon)
			{
				this._icon = VariantUtils.ConvertTo<TextureRect>(ref value);
				return true;
			}
			if ((ref name) == NPower.PropertyName._amountLabel)
			{
				this._amountLabel = VariantUtils.ConvertTo<MegaLabel>(ref value);
				return true;
			}
			if ((ref name) == NPower.PropertyName._powerFlash)
			{
				this._powerFlash = VariantUtils.ConvertTo<CpuParticles2D>(ref value);
				return true;
			}
			if ((ref name) == NPower.PropertyName._animInTween)
			{
				this._animInTween = VariantUtils.ConvertTo<Tween>(ref value);
				return true;
			}
			return base.SetGodotClassPropertyValue(ref name, ref value);
		}

		/// <inheritdoc />
		// Token: 0x06004111 RID: 16657 RVA: 0x001F6C5C File Offset: 0x001F4E5C
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
		{
			if ((ref name) == NPower.PropertyName.Container)
			{
				NPowerContainer container = this.Container;
				value = VariantUtils.CreateFrom<NPowerContainer>(ref container);
				return true;
			}
			if ((ref name) == NPower.PropertyName._icon)
			{
				value = VariantUtils.CreateFrom<TextureRect>(ref this._icon);
				return true;
			}
			if ((ref name) == NPower.PropertyName._amountLabel)
			{
				value = VariantUtils.CreateFrom<MegaLabel>(ref this._amountLabel);
				return true;
			}
			if ((ref name) == NPower.PropertyName._powerFlash)
			{
				value = VariantUtils.CreateFrom<CpuParticles2D>(ref this._powerFlash);
				return true;
			}
			if ((ref name) == NPower.PropertyName._animInTween)
			{
				value = VariantUtils.CreateFrom<Tween>(ref this._animInTween);
				return true;
			}
			return base.GetGodotClassPropertyValue(ref name, ref value);
		}

		/// <summary>
		/// Get the property information for all the properties declared in this class.
		/// This method is used by Godot to register the available properties in the editor.
		/// Do not call this method.
		/// </summary>
		// Token: 0x06004112 RID: 16658 RVA: 0x001F6D14 File Offset: 0x001F4F14
		[NullableContext(0)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal static List<PropertyInfo> GetGodotPropertyList()
		{
			return new List<PropertyInfo>
			{
				new PropertyInfo(24L, NPower.PropertyName._icon, 0L, "", 4096L, false),
				new PropertyInfo(24L, NPower.PropertyName._amountLabel, 0L, "", 4096L, false),
				new PropertyInfo(24L, NPower.PropertyName._powerFlash, 0L, "", 4096L, false),
				new PropertyInfo(24L, NPower.PropertyName._animInTween, 0L, "", 4096L, false),
				new PropertyInfo(24L, NPower.PropertyName.Container, 0L, "", 4096L, false)
			};
		}

		/// <inheritdoc />
		// Token: 0x06004113 RID: 16659 RVA: 0x001F6DD0 File Offset: 0x001F4FD0
		[NullableContext(0)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void SaveGodotObjectData(GodotSerializationInfo info)
		{
			base.SaveGodotObjectData(info);
			StringName container = NPower.PropertyName.Container;
			NPowerContainer container2 = this.Container;
			info.AddProperty(container, Variant.From<NPowerContainer>(ref container2));
			info.AddProperty(NPower.PropertyName._icon, Variant.From<TextureRect>(ref this._icon));
			info.AddProperty(NPower.PropertyName._amountLabel, Variant.From<MegaLabel>(ref this._amountLabel));
			info.AddProperty(NPower.PropertyName._powerFlash, Variant.From<CpuParticles2D>(ref this._powerFlash));
			info.AddProperty(NPower.PropertyName._animInTween, Variant.From<Tween>(ref this._animInTween));
		}

		/// <inheritdoc />
		// Token: 0x06004114 RID: 16660 RVA: 0x001F6E58 File Offset: 0x001F5058
		[NullableContext(0)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void RestoreGodotObjectData(GodotSerializationInfo info)
		{
			base.RestoreGodotObjectData(info);
			Variant variant;
			if (info.TryGetProperty(NPower.PropertyName.Container, ref variant))
			{
				this.Container = variant.As<NPowerContainer>();
			}
			Variant variant2;
			if (info.TryGetProperty(NPower.PropertyName._icon, ref variant2))
			{
				this._icon = variant2.As<TextureRect>();
			}
			Variant variant3;
			if (info.TryGetProperty(NPower.PropertyName._amountLabel, ref variant3))
			{
				this._amountLabel = variant3.As<MegaLabel>();
			}
			Variant variant4;
			if (info.TryGetProperty(NPower.PropertyName._powerFlash, ref variant4))
			{
				this._powerFlash = variant4.As<CpuParticles2D>();
			}
			Variant variant5;
			if (info.TryGetProperty(NPower.PropertyName._animInTween, ref variant5))
			{
				this._animInTween = variant5.As<Tween>();
			}
		}

		// Token: 0x04001946 RID: 6470
		private static readonly StringName _pulse = new StringName("pulse");

		// Token: 0x04001947 RID: 6471
		[Nullable(2)]
		private PowerModel _model;

		// Token: 0x04001948 RID: 6472
		private TextureRect _icon;

		// Token: 0x04001949 RID: 6473
		private MegaLabel _amountLabel;

		// Token: 0x0400194A RID: 6474
		private CpuParticles2D _powerFlash;

		// Token: 0x0400194B RID: 6475
		[Nullable(2)]
		private Tween _animInTween;

		/// <summary>
		/// Cached StringNames for the methods contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x02001916 RID: 6422
		[NullableContext(0)]
		public class MethodName : Control.MethodName
		{
			/// <summary>
			/// Cached name for the '_Ready' method.
			/// </summary>
			// Token: 0x040062BD RID: 25277
			public static readonly StringName _Ready = "_Ready";

			/// <summary>
			/// Cached name for the '_EnterTree' method.
			/// </summary>
			// Token: 0x040062BE RID: 25278
			public static readonly StringName _EnterTree = "_EnterTree";

			/// <summary>
			/// Cached name for the '_ExitTree' method.
			/// </summary>
			// Token: 0x040062BF RID: 25279
			public static readonly StringName _ExitTree = "_ExitTree";

			/// <summary>
			/// Cached name for the 'Reload' method.
			/// </summary>
			// Token: 0x040062C0 RID: 25280
			public static readonly StringName Reload = "Reload";

			/// <summary>
			/// Cached name for the 'OnPulsingStarted' method.
			/// </summary>
			// Token: 0x040062C1 RID: 25281
			public static readonly StringName OnPulsingStarted = "OnPulsingStarted";

			/// <summary>
			/// Cached name for the 'OnPulsingStopped' method.
			/// </summary>
			// Token: 0x040062C2 RID: 25282
			public static readonly StringName OnPulsingStopped = "OnPulsingStopped";

			/// <summary>
			/// Cached name for the 'RefreshAmount' method.
			/// </summary>
			// Token: 0x040062C3 RID: 25283
			public static readonly StringName RefreshAmount = "RefreshAmount";

			/// <summary>
			/// Cached name for the 'OnDisplayAmountChanged' method.
			/// </summary>
			// Token: 0x040062C4 RID: 25284
			public static readonly StringName OnDisplayAmountChanged = "OnDisplayAmountChanged";

			/// <summary>
			/// Cached name for the 'FlashPower' method.
			/// </summary>
			// Token: 0x040062C5 RID: 25285
			public static readonly StringName FlashPower = "FlashPower";

			/// <summary>
			/// Cached name for the 'OnHovered' method.
			/// </summary>
			// Token: 0x040062C6 RID: 25286
			public static readonly StringName OnHovered = "OnHovered";

			/// <summary>
			/// Cached name for the 'OnUnhovered' method.
			/// </summary>
			// Token: 0x040062C7 RID: 25287
			public static readonly StringName OnUnhovered = "OnUnhovered";

			/// <summary>
			/// Cached name for the 'SubscribeToModelEvents' method.
			/// </summary>
			// Token: 0x040062C8 RID: 25288
			public static readonly StringName SubscribeToModelEvents = "SubscribeToModelEvents";

			/// <summary>
			/// Cached name for the 'UnsubscribeFromModelEvents' method.
			/// </summary>
			// Token: 0x040062C9 RID: 25289
			public static readonly StringName UnsubscribeFromModelEvents = "UnsubscribeFromModelEvents";

			/// <summary>
			/// Cached name for the 'OnPowerRemoved' method.
			/// </summary>
			// Token: 0x040062CA RID: 25290
			public static readonly StringName OnPowerRemoved = "OnPowerRemoved";
		}

		/// <summary>
		/// Cached StringNames for the properties and fields contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x02001917 RID: 6423
		[NullableContext(0)]
		public class PropertyName : Control.PropertyName
		{
			/// <summary>
			/// Cached name for the 'Container' property.
			/// </summary>
			// Token: 0x040062CB RID: 25291
			public static readonly StringName Container = "Container";

			/// <summary>
			/// Cached name for the '_icon' field.
			/// </summary>
			// Token: 0x040062CC RID: 25292
			public static readonly StringName _icon = "_icon";

			/// <summary>
			/// Cached name for the '_amountLabel' field.
			/// </summary>
			// Token: 0x040062CD RID: 25293
			public static readonly StringName _amountLabel = "_amountLabel";

			/// <summary>
			/// Cached name for the '_powerFlash' field.
			/// </summary>
			// Token: 0x040062CE RID: 25294
			public static readonly StringName _powerFlash = "_powerFlash";

			/// <summary>
			/// Cached name for the '_animInTween' field.
			/// </summary>
			// Token: 0x040062CF RID: 25295
			public static readonly StringName _animInTween = "_animInTween";
		}

		/// <summary>
		/// Cached StringNames for the signals contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x02001918 RID: 6424
		[NullableContext(0)]
		public class SignalName : Control.SignalName
		{
		}
	}
}
