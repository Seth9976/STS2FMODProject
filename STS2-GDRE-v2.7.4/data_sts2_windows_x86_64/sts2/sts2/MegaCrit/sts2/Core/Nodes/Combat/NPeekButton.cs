using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using Godot;
using Godot.Bridge;
using Godot.NativeInterop;
using MegaCrit.Sts2.Core.Audio.Debug;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.ControllerInput;
using MegaCrit.Sts2.Core.Nodes.GodotExtensions;
using MegaCrit.Sts2.Core.Nodes.Rooms;
using MegaCrit.Sts2.Core.Nodes.Screens.Capstones;
using MegaCrit.Sts2.Core.Nodes.Screens.Overlays;

namespace MegaCrit.Sts2.Core.Nodes.Combat
{
	// Token: 0x02000406 RID: 1030
	[ScriptPath("res://src/Core/Nodes/Combat/NPeekButton.cs")]
	public class NPeekButton : NButton
	{
		// Token: 0x1700093E RID: 2366
		// (get) Token: 0x0600404E RID: 16462 RVA: 0x001EF566 File Offset: 0x001ED766
		[Nullable(1)]
		protected override string[] Hotkeys
		{
			[NullableContext(1)]
			get
			{
				return new string[] { MegaInput.peek };
			}
		}

		// Token: 0x1700093F RID: 2367
		// (get) Token: 0x0600404F RID: 16463 RVA: 0x001EF57B File Offset: 0x001ED77B
		// (set) Token: 0x06004050 RID: 16464 RVA: 0x001EF583 File Offset: 0x001ED783
		public bool IsPeeking { get; private set; }

		// Token: 0x17000940 RID: 2368
		// (get) Token: 0x06004051 RID: 16465 RVA: 0x001EF58C File Offset: 0x001ED78C
		// (set) Token: 0x06004052 RID: 16466 RVA: 0x001EF594 File Offset: 0x001ED794
		[Nullable(1)]
		public Marker2D CurrentCardMarker
		{
			[NullableContext(1)]
			get;
			[NullableContext(1)]
			private set;
		}

		// Token: 0x06004053 RID: 16467 RVA: 0x001EF5A0 File Offset: 0x001ED7A0
		public override void _Ready()
		{
			this.ConnectSignals();
			this._flash = base.GetNode<TextureRect>("%Flash");
			this._visuals = base.GetNode<Control>("%Visuals");
			this.CurrentCardMarker = base.GetNode<Marker2D>("%CurrentCardMarker");
			if (NCombatRoom.Instance != null)
			{
				if (NCombatRoom.Instance.IsNodeReady())
				{
					this.OnCombatRoomReady();
				}
				else
				{
					NCombatRoom.Instance.Connect(Node.SignalName.Ready, Callable.From(new Action(this.OnCombatRoomReady)), 0U);
				}
			}
			if (!CombatManager.Instance.IsInProgress)
			{
				base.Disable();
			}
			Node node = base.GetParent();
			while (node != null)
			{
				IOverlayScreen overlayScreen = node as IOverlayScreen;
				if (overlayScreen != null)
				{
					this._overlayScreenParent = overlayScreen;
					NOverlayStack instance = NOverlayStack.Instance;
					if (instance != null)
					{
						instance.Connect(NOverlayStack.SignalName.Changed, Callable.From(new Action(this.OnOverlayStackChanged)), 0U);
					}
					NCapstoneContainer instance2 = NCapstoneContainer.Instance;
					if (instance2 == null)
					{
						return;
					}
					instance2.Connect(NCapstoneContainer.SignalName.Changed, Callable.From(new Action(this.OnOverlayStackChanged)), 0U);
					return;
				}
				else
				{
					node = node.GetParent();
				}
			}
		}

		// Token: 0x06004054 RID: 16468 RVA: 0x001EF6B8 File Offset: 0x001ED8B8
		protected override void OnEnable()
		{
			base.OnEnable();
			base.Visible = true;
		}

		// Token: 0x06004055 RID: 16469 RVA: 0x001EF6C7 File Offset: 0x001ED8C7
		protected override void OnDisable()
		{
			base.OnDisable();
			base.Visible = false;
		}

		/// <summary>
		/// This method handles re-hiding the backstop if the peek button is inside an overlay screen and another screen
		/// appeared above it and then was removed.
		/// </summary>
		// Token: 0x06004056 RID: 16470 RVA: 0x001EF6D8 File Offset: 0x001ED8D8
		private void OnOverlayStackChanged()
		{
			if (!this.IsPeeking)
			{
				return;
			}
			if (this._overlayScreenParent == null)
			{
				return;
			}
			NCapstoneContainer instance = NCapstoneContainer.Instance;
			if (((instance != null) ? instance.CurrentCapstoneScreen : null) != null)
			{
				return;
			}
			IOverlayScreen overlayScreenParent = this._overlayScreenParent;
			NOverlayStack instance2 = NOverlayStack.Instance;
			if (overlayScreenParent != ((instance2 != null) ? instance2.Peek() : null))
			{
				return;
			}
			NOverlayStack.Instance.HideBackstop();
		}

		// Token: 0x06004057 RID: 16471 RVA: 0x001EF730 File Offset: 0x001ED930
		public void Wiggle()
		{
			this._flash.Visible = true;
			CanvasItem flash = this._flash;
			Color modulate = this._flash.Modulate;
			modulate.A = 0f;
			flash.Modulate = modulate;
			Tween wiggleTween = this._wiggleTween;
			if (wiggleTween != null)
			{
				wiggleTween.Kill();
			}
			this._wiggleTween = base.CreateTween();
			this._visuals.RotationDegrees = 0f;
			this._wiggleTween.TweenMethod(Callable.From<float>(delegate(float t)
			{
				this._visuals.RotationDegrees = 10f * Mathf.Sin(t * 3f) * Mathf.Sin(t * 0.5f);
			}), 0f, 6.2831855f, 0.5);
			this._wiggleTween.Parallel().TweenMethod(Callable.From<float>(delegate(float t)
			{
				this._visuals.Scale = Vector2.One + Vector2.One * 0.15f * Mathf.Sin(t) * Mathf.Sin(t * 0.5f);
			}), 0f, 3.1415927f, 0.25);
			this._wiggleTween.Parallel().TweenProperty(this._flash, "modulate:a", 1f, 0.1);
			this._wiggleTween.Chain().TweenProperty(this._flash, "modulate:a", 0f, 0.3).SetTrans(4L)
				.SetEase(1L);
			NDebugAudioManager.Instance.Play("deny.mp3", 0.5f, PitchVariance.Medium);
		}

		// Token: 0x06004058 RID: 16472 RVA: 0x001EF89F File Offset: 0x001EDA9F
		[NullableContext(1)]
		public void AddTargets(params Control[] targets)
		{
			this._targets.AddRange(targets);
		}

		// Token: 0x06004059 RID: 16473 RVA: 0x001EF8B0 File Offset: 0x001EDAB0
		public void SetPeeking(bool isPeeking)
		{
			if (this.IsPeeking == isPeeking)
			{
				return;
			}
			this.IsPeeking = isPeeking;
			if (NOverlayStack.Instance.ScreenCount > 0)
			{
				if (this.IsPeeking)
				{
					NOverlayStack.Instance.HideBackstop();
				}
				else
				{
					NOverlayStack.Instance.ShowBackstop();
				}
			}
			if (this.IsPeeking)
			{
				using (IEnumerator<Control> enumerator = this._targets.Where<Control>((Control t) => t.Visible).GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						Control control = enumerator.Current;
						this._hiddenTargets.Add(control);
						control.Visible = false;
					}
					goto IL_00E5;
				}
			}
			foreach (Control control2 in this._hiddenTargets)
			{
				control2.Visible = true;
			}
			this._hiddenTargets.Clear();
			IL_00E5:
			((ShaderMaterial)this._visuals.Material).SetShaderParameter(NPeekButton._pulseStrength, (this.IsPeeking > false) ? 1 : 0);
			base.EmitSignal(NPeekButton.SignalName.Toggled, new Variant[] { this });
		}

		// Token: 0x0600405A RID: 16474 RVA: 0x001EFA08 File Offset: 0x001EDC08
		protected override void OnRelease()
		{
			this.SetPeeking(!this.IsPeeking);
			Tween hoverTween = this._hoverTween;
			if (hoverTween != null)
			{
				hoverTween.Kill();
			}
			this._hoverTween = base.CreateTween();
			this._hoverTween.TweenProperty(this._visuals, "scale", Vector2.One, 0.15);
		}

		// Token: 0x0600405B RID: 16475 RVA: 0x001EFA70 File Offset: 0x001EDC70
		protected override void OnPress()
		{
			base.OnPress();
			Tween hoverTween = this._hoverTween;
			if (hoverTween != null)
			{
				hoverTween.Kill();
			}
			this._hoverTween = base.CreateTween();
			this._hoverTween.TweenProperty(this._visuals, "scale", Vector2.One * 0.95f, 0.05);
		}

		// Token: 0x0600405C RID: 16476 RVA: 0x001EFADC File Offset: 0x001EDCDC
		protected override void OnFocus()
		{
			base.OnFocus();
			Tween hoverTween = this._hoverTween;
			if (hoverTween != null)
			{
				hoverTween.Kill();
			}
			this._hoverTween = base.CreateTween();
			this._hoverTween.TweenProperty(this._visuals, "scale", Vector2.One * 1.1f, 0.05);
		}

		// Token: 0x0600405D RID: 16477 RVA: 0x001EFB48 File Offset: 0x001EDD48
		protected override void OnUnfocus()
		{
			base.OnUnfocus();
			Tween hoverTween = this._hoverTween;
			if (hoverTween != null)
			{
				hoverTween.Kill();
			}
			this._hoverTween = base.CreateTween();
			this._hoverTween.TweenProperty(this._visuals, "scale", Vector2.One, 0.15);
		}

		// Token: 0x0600405E RID: 16478 RVA: 0x001EFBA7 File Offset: 0x001EDDA7
		private void OnCombatRoomReady()
		{
			NCombatRoom.Instance.Ui.OnPeekButtonReady(this);
		}

		/// <summary>
		/// Get the method information for all the methods declared in this class.
		/// This method is used by Godot to register the available methods in the editor.
		/// Do not call this method.
		/// </summary>
		// Token: 0x0600405F RID: 16479 RVA: 0x001EFBBC File Offset: 0x001EDDBC
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal new static List<MethodInfo> GetGodotMethodList()
		{
			return new List<MethodInfo>(12)
			{
				new MethodInfo(NPeekButton.MethodName._Ready, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NPeekButton.MethodName.OnEnable, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NPeekButton.MethodName.OnDisable, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NPeekButton.MethodName.OnOverlayStackChanged, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NPeekButton.MethodName.Wiggle, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NPeekButton.MethodName.AddTargets, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, new List<PropertyInfo>
				{
					new PropertyInfo(28L, "targets", 0L, "", 6L, false)
				}, null),
				new MethodInfo(NPeekButton.MethodName.SetPeeking, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, new List<PropertyInfo>
				{
					new PropertyInfo(1L, "isPeeking", 0L, "", 6L, false)
				}, null),
				new MethodInfo(NPeekButton.MethodName.OnRelease, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NPeekButton.MethodName.OnPress, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NPeekButton.MethodName.OnFocus, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NPeekButton.MethodName.OnUnfocus, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NPeekButton.MethodName.OnCombatRoomReady, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null)
			};
		}

		/// <inheritdoc />
		// Token: 0x06004060 RID: 16480 RVA: 0x001EFE54 File Offset: 0x001EE054
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool InvokeGodotClassMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
		{
			if ((ref method) == NPeekButton.MethodName._Ready && args.Count == 0)
			{
				this._Ready();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NPeekButton.MethodName.OnEnable && args.Count == 0)
			{
				this.OnEnable();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NPeekButton.MethodName.OnDisable && args.Count == 0)
			{
				this.OnDisable();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NPeekButton.MethodName.OnOverlayStackChanged && args.Count == 0)
			{
				this.OnOverlayStackChanged();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NPeekButton.MethodName.Wiggle && args.Count == 0)
			{
				this.Wiggle();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NPeekButton.MethodName.AddTargets && args.Count == 1)
			{
				this.AddTargets(VariantUtils.ConvertToSystemArrayOfGodotObject<Control>(args[0]));
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NPeekButton.MethodName.SetPeeking && args.Count == 1)
			{
				this.SetPeeking(VariantUtils.ConvertTo<bool>(args[0]));
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NPeekButton.MethodName.OnRelease && args.Count == 0)
			{
				this.OnRelease();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NPeekButton.MethodName.OnPress && args.Count == 0)
			{
				this.OnPress();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NPeekButton.MethodName.OnFocus && args.Count == 0)
			{
				this.OnFocus();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NPeekButton.MethodName.OnUnfocus && args.Count == 0)
			{
				this.OnUnfocus();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NPeekButton.MethodName.OnCombatRoomReady && args.Count == 0)
			{
				this.OnCombatRoomReady();
				ret = default(godot_variant);
				return true;
			}
			return base.InvokeGodotClassMethod(in method, args, out ret);
		}

		/// <inheritdoc />
		// Token: 0x06004061 RID: 16481 RVA: 0x001F0044 File Offset: 0x001EE244
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool HasGodotClassMethod(in godot_string_name method)
		{
			return (ref method) == NPeekButton.MethodName._Ready || (ref method) == NPeekButton.MethodName.OnEnable || (ref method) == NPeekButton.MethodName.OnDisable || (ref method) == NPeekButton.MethodName.OnOverlayStackChanged || (ref method) == NPeekButton.MethodName.Wiggle || (ref method) == NPeekButton.MethodName.AddTargets || (ref method) == NPeekButton.MethodName.SetPeeking || (ref method) == NPeekButton.MethodName.OnRelease || (ref method) == NPeekButton.MethodName.OnPress || (ref method) == NPeekButton.MethodName.OnFocus || (ref method) == NPeekButton.MethodName.OnUnfocus || (ref method) == NPeekButton.MethodName.OnCombatRoomReady || base.HasGodotClassMethod(in method);
		}

		/// <inheritdoc />
		// Token: 0x06004062 RID: 16482 RVA: 0x001F010C File Offset: 0x001EE30C
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool SetGodotClassPropertyValue(in godot_string_name name, in godot_variant value)
		{
			if ((ref name) == NPeekButton.PropertyName.IsPeeking)
			{
				this.IsPeeking = VariantUtils.ConvertTo<bool>(ref value);
				return true;
			}
			if ((ref name) == NPeekButton.PropertyName.CurrentCardMarker)
			{
				this.CurrentCardMarker = VariantUtils.ConvertTo<Marker2D>(ref value);
				return true;
			}
			if ((ref name) == NPeekButton.PropertyName._flash)
			{
				this._flash = VariantUtils.ConvertTo<TextureRect>(ref value);
				return true;
			}
			if ((ref name) == NPeekButton.PropertyName._visuals)
			{
				this._visuals = VariantUtils.ConvertTo<Control>(ref value);
				return true;
			}
			if ((ref name) == NPeekButton.PropertyName._hoverTween)
			{
				this._hoverTween = VariantUtils.ConvertTo<Tween>(ref value);
				return true;
			}
			if ((ref name) == NPeekButton.PropertyName._wiggleTween)
			{
				this._wiggleTween = VariantUtils.ConvertTo<Tween>(ref value);
				return true;
			}
			return base.SetGodotClassPropertyValue(in name, in value);
		}

		/// <inheritdoc />
		// Token: 0x06004063 RID: 16483 RVA: 0x001F01C4 File Offset: 0x001EE3C4
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
		{
			if ((ref name) == NPeekButton.PropertyName.Hotkeys)
			{
				string[] hotkeys = this.Hotkeys;
				value = VariantUtils.CreateFrom<string[]>(ref hotkeys);
				return true;
			}
			if ((ref name) == NPeekButton.PropertyName.IsPeeking)
			{
				bool isPeeking = this.IsPeeking;
				value = VariantUtils.CreateFrom<bool>(ref isPeeking);
				return true;
			}
			if ((ref name) == NPeekButton.PropertyName.CurrentCardMarker)
			{
				Marker2D currentCardMarker = this.CurrentCardMarker;
				value = VariantUtils.CreateFrom<Marker2D>(ref currentCardMarker);
				return true;
			}
			if ((ref name) == NPeekButton.PropertyName._flash)
			{
				value = VariantUtils.CreateFrom<TextureRect>(ref this._flash);
				return true;
			}
			if ((ref name) == NPeekButton.PropertyName._visuals)
			{
				value = VariantUtils.CreateFrom<Control>(ref this._visuals);
				return true;
			}
			if ((ref name) == NPeekButton.PropertyName._hoverTween)
			{
				value = VariantUtils.CreateFrom<Tween>(ref this._hoverTween);
				return true;
			}
			if ((ref name) == NPeekButton.PropertyName._wiggleTween)
			{
				value = VariantUtils.CreateFrom<Tween>(ref this._wiggleTween);
				return true;
			}
			return base.GetGodotClassPropertyValue(in name, out value);
		}

		/// <summary>
		/// Get the property information for all the properties declared in this class.
		/// This method is used by Godot to register the available properties in the editor.
		/// Do not call this method.
		/// </summary>
		// Token: 0x06004064 RID: 16484 RVA: 0x001F02C4 File Offset: 0x001EE4C4
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal new static List<PropertyInfo> GetGodotPropertyList()
		{
			return new List<PropertyInfo>
			{
				new PropertyInfo(34L, NPeekButton.PropertyName.Hotkeys, 0L, "", 4096L, false),
				new PropertyInfo(1L, NPeekButton.PropertyName.IsPeeking, 0L, "", 4096L, false),
				new PropertyInfo(24L, NPeekButton.PropertyName._flash, 0L, "", 4096L, false),
				new PropertyInfo(24L, NPeekButton.PropertyName._visuals, 0L, "", 4096L, false),
				new PropertyInfo(24L, NPeekButton.PropertyName._hoverTween, 0L, "", 4096L, false),
				new PropertyInfo(24L, NPeekButton.PropertyName._wiggleTween, 0L, "", 4096L, false),
				new PropertyInfo(24L, NPeekButton.PropertyName.CurrentCardMarker, 0L, "", 4096L, false)
			};
		}

		/// <inheritdoc />
		// Token: 0x06004065 RID: 16485 RVA: 0x001F03C0 File Offset: 0x001EE5C0
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void SaveGodotObjectData(GodotSerializationInfo info)
		{
			base.SaveGodotObjectData(info);
			StringName isPeeking = NPeekButton.PropertyName.IsPeeking;
			bool isPeeking2 = this.IsPeeking;
			info.AddProperty(isPeeking, Variant.From<bool>(ref isPeeking2));
			StringName currentCardMarker = NPeekButton.PropertyName.CurrentCardMarker;
			Marker2D currentCardMarker2 = this.CurrentCardMarker;
			info.AddProperty(currentCardMarker, Variant.From<Marker2D>(ref currentCardMarker2));
			info.AddProperty(NPeekButton.PropertyName._flash, Variant.From<TextureRect>(ref this._flash));
			info.AddProperty(NPeekButton.PropertyName._visuals, Variant.From<Control>(ref this._visuals));
			info.AddProperty(NPeekButton.PropertyName._hoverTween, Variant.From<Tween>(ref this._hoverTween));
			info.AddProperty(NPeekButton.PropertyName._wiggleTween, Variant.From<Tween>(ref this._wiggleTween));
			info.AddSignalEventDelegate(NPeekButton.SignalName.Toggled, this.backing_Toggled);
		}

		/// <inheritdoc />
		// Token: 0x06004066 RID: 16486 RVA: 0x001F0470 File Offset: 0x001EE670
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void RestoreGodotObjectData(GodotSerializationInfo info)
		{
			base.RestoreGodotObjectData(info);
			Variant variant;
			if (info.TryGetProperty(NPeekButton.PropertyName.IsPeeking, ref variant))
			{
				this.IsPeeking = variant.As<bool>();
			}
			Variant variant2;
			if (info.TryGetProperty(NPeekButton.PropertyName.CurrentCardMarker, ref variant2))
			{
				this.CurrentCardMarker = variant2.As<Marker2D>();
			}
			Variant variant3;
			if (info.TryGetProperty(NPeekButton.PropertyName._flash, ref variant3))
			{
				this._flash = variant3.As<TextureRect>();
			}
			Variant variant4;
			if (info.TryGetProperty(NPeekButton.PropertyName._visuals, ref variant4))
			{
				this._visuals = variant4.As<Control>();
			}
			Variant variant5;
			if (info.TryGetProperty(NPeekButton.PropertyName._hoverTween, ref variant5))
			{
				this._hoverTween = variant5.As<Tween>();
			}
			Variant variant6;
			if (info.TryGetProperty(NPeekButton.PropertyName._wiggleTween, ref variant6))
			{
				this._wiggleTween = variant6.As<Tween>();
			}
			NPeekButton.ToggledEventHandler toggledEventHandler;
			if (info.TryGetSignalEventDelegate<NPeekButton.ToggledEventHandler>(NPeekButton.SignalName.Toggled, ref toggledEventHandler))
			{
				this.backing_Toggled = toggledEventHandler;
			}
		}

		/// <summary>
		/// Get the signal information for all the signals declared in this class.
		/// This method is used by Godot to register the available signals in the editor.
		/// Do not call this method.
		/// </summary>
		// Token: 0x06004067 RID: 16487 RVA: 0x001F0544 File Offset: 0x001EE744
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal new static List<MethodInfo> GetGodotSignalList()
		{
			return new List<MethodInfo>(1)
			{
				new MethodInfo(NPeekButton.SignalName.Toggled, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, new List<PropertyInfo>
				{
					new PropertyInfo(24L, "peekButton", 0L, "", 6L, new StringName("Control"), false)
				}, null)
			};
		}

		/// <inheritdoc cref="T:MegaCrit.Sts2.Core.Nodes.Combat.NPeekButton.ToggledEventHandler" />
		// Token: 0x1400004C RID: 76
		// (add) Token: 0x06004068 RID: 16488 RVA: 0x001F05B8 File Offset: 0x001EE7B8
		// (remove) Token: 0x06004069 RID: 16489 RVA: 0x001F05D1 File Offset: 0x001EE7D1
		public event NPeekButton.ToggledEventHandler Toggled
		{
			add
			{
				this.backing_Toggled = (NPeekButton.ToggledEventHandler)Delegate.Combine(this.backing_Toggled, value);
			}
			remove
			{
				this.backing_Toggled = (NPeekButton.ToggledEventHandler)Delegate.Remove(this.backing_Toggled, value);
			}
		}

		// Token: 0x0600406A RID: 16490 RVA: 0x001F05EA File Offset: 0x001EE7EA
		protected void EmitSignalToggled(NPeekButton peekButton)
		{
			base.EmitSignal(NPeekButton.SignalName.Toggled, new Variant[] { peekButton });
		}

		/// <inheritdoc />
		// Token: 0x0600406B RID: 16491 RVA: 0x001F060B File Offset: 0x001EE80B
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void RaiseGodotClassSignalCallbacks(in godot_string_name signal, NativeVariantPtrArgs args)
		{
			if (!((ref signal) == NPeekButton.SignalName.Toggled) || args.Count != 1)
			{
				base.RaiseGodotClassSignalCallbacks(in signal, args);
				return;
			}
			NPeekButton.ToggledEventHandler toggledEventHandler = this.backing_Toggled;
			if (toggledEventHandler == null)
			{
				return;
			}
			toggledEventHandler(VariantUtils.ConvertTo<NPeekButton>(args[0]));
		}

		/// <inheritdoc />
		// Token: 0x0600406C RID: 16492 RVA: 0x001F064A File Offset: 0x001EE84A
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool HasGodotClassSignal(in godot_string_name signal)
		{
			return (ref signal) == NPeekButton.SignalName.Toggled || base.HasGodotClassSignal(in signal);
		}

		// Token: 0x0400190A RID: 6410
		[Nullable(1)]
		private static readonly StringName _pulseStrength = new StringName("pulse_strength");

		// Token: 0x0400190B RID: 6411
		[Nullable(1)]
		private readonly List<Control> _targets = new List<Control>();

		// Token: 0x0400190C RID: 6412
		[Nullable(1)]
		private readonly List<Control> _hiddenTargets = new List<Control>();

		// Token: 0x0400190E RID: 6414
		[Nullable(1)]
		private TextureRect _flash;

		// Token: 0x0400190F RID: 6415
		[Nullable(1)]
		private Control _visuals;

		// Token: 0x04001910 RID: 6416
		[Nullable(2)]
		private IOverlayScreen _overlayScreenParent;

		// Token: 0x04001911 RID: 6417
		[Nullable(2)]
		private Tween _hoverTween;

		// Token: 0x04001912 RID: 6418
		[Nullable(2)]
		private Tween _wiggleTween;

		// Token: 0x04001914 RID: 6420
		private NPeekButton.ToggledEventHandler backing_Toggled;

		// Token: 0x020018FE RID: 6398
		// (Invoke) Token: 0x0600A9CA RID: 43466
		[Signal]
		public delegate void ToggledEventHandler(NPeekButton peekButton);

		/// <summary>
		/// Cached StringNames for the methods contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x020018FF RID: 6399
		public new class MethodName : NButton.MethodName
		{
			/// <summary>
			/// Cached name for the '_Ready' method.
			/// </summary>
			// Token: 0x04006226 RID: 25126
			public new static readonly StringName _Ready = "_Ready";

			/// <summary>
			/// Cached name for the 'OnEnable' method.
			/// </summary>
			// Token: 0x04006227 RID: 25127
			public new static readonly StringName OnEnable = "OnEnable";

			/// <summary>
			/// Cached name for the 'OnDisable' method.
			/// </summary>
			// Token: 0x04006228 RID: 25128
			public new static readonly StringName OnDisable = "OnDisable";

			/// <summary>
			/// Cached name for the 'OnOverlayStackChanged' method.
			/// </summary>
			// Token: 0x04006229 RID: 25129
			public static readonly StringName OnOverlayStackChanged = "OnOverlayStackChanged";

			/// <summary>
			/// Cached name for the 'Wiggle' method.
			/// </summary>
			// Token: 0x0400622A RID: 25130
			public static readonly StringName Wiggle = "Wiggle";

			/// <summary>
			/// Cached name for the 'AddTargets' method.
			/// </summary>
			// Token: 0x0400622B RID: 25131
			public static readonly StringName AddTargets = "AddTargets";

			/// <summary>
			/// Cached name for the 'SetPeeking' method.
			/// </summary>
			// Token: 0x0400622C RID: 25132
			public static readonly StringName SetPeeking = "SetPeeking";

			/// <summary>
			/// Cached name for the 'OnRelease' method.
			/// </summary>
			// Token: 0x0400622D RID: 25133
			public new static readonly StringName OnRelease = "OnRelease";

			/// <summary>
			/// Cached name for the 'OnPress' method.
			/// </summary>
			// Token: 0x0400622E RID: 25134
			public new static readonly StringName OnPress = "OnPress";

			/// <summary>
			/// Cached name for the 'OnFocus' method.
			/// </summary>
			// Token: 0x0400622F RID: 25135
			public new static readonly StringName OnFocus = "OnFocus";

			/// <summary>
			/// Cached name for the 'OnUnfocus' method.
			/// </summary>
			// Token: 0x04006230 RID: 25136
			public new static readonly StringName OnUnfocus = "OnUnfocus";

			/// <summary>
			/// Cached name for the 'OnCombatRoomReady' method.
			/// </summary>
			// Token: 0x04006231 RID: 25137
			public static readonly StringName OnCombatRoomReady = "OnCombatRoomReady";
		}

		/// <summary>
		/// Cached StringNames for the properties and fields contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x02001900 RID: 6400
		public new class PropertyName : NButton.PropertyName
		{
			/// <summary>
			/// Cached name for the 'Hotkeys' property.
			/// </summary>
			// Token: 0x04006232 RID: 25138
			public new static readonly StringName Hotkeys = "Hotkeys";

			/// <summary>
			/// Cached name for the 'IsPeeking' property.
			/// </summary>
			// Token: 0x04006233 RID: 25139
			public static readonly StringName IsPeeking = "IsPeeking";

			/// <summary>
			/// Cached name for the 'CurrentCardMarker' property.
			/// </summary>
			// Token: 0x04006234 RID: 25140
			public static readonly StringName CurrentCardMarker = "CurrentCardMarker";

			/// <summary>
			/// Cached name for the '_flash' field.
			/// </summary>
			// Token: 0x04006235 RID: 25141
			public static readonly StringName _flash = "_flash";

			/// <summary>
			/// Cached name for the '_visuals' field.
			/// </summary>
			// Token: 0x04006236 RID: 25142
			public static readonly StringName _visuals = "_visuals";

			/// <summary>
			/// Cached name for the '_hoverTween' field.
			/// </summary>
			// Token: 0x04006237 RID: 25143
			public static readonly StringName _hoverTween = "_hoverTween";

			/// <summary>
			/// Cached name for the '_wiggleTween' field.
			/// </summary>
			// Token: 0x04006238 RID: 25144
			public static readonly StringName _wiggleTween = "_wiggleTween";
		}

		/// <summary>
		/// Cached StringNames for the signals contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x02001901 RID: 6401
		public new class SignalName : NButton.SignalName
		{
			/// <summary>
			/// Cached name for the 'Toggled' signal.
			/// </summary>
			// Token: 0x04006239 RID: 25145
			public static readonly StringName Toggled = "Toggled";
		}
	}
}
