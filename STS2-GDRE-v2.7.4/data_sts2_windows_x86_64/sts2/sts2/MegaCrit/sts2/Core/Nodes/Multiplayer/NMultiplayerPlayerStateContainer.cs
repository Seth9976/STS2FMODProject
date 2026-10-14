using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using Godot;
using Godot.Bridge;
using Godot.NativeInterop;
using MegaCrit.Sts2.addons.mega_text;
using MegaCrit.Sts2.Core.Context;
using MegaCrit.Sts2.Core.Debug;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.Nodes.GodotExtensions;
using MegaCrit.Sts2.Core.Nodes.Relics;
using MegaCrit.Sts2.Core.Nodes.Screens.ScreenContext;
using MegaCrit.Sts2.Core.Nodes.Vfx;
using MegaCrit.Sts2.Core.Runs;

namespace MegaCrit.Sts2.Core.Nodes.Multiplayer
{
	/// <summary>
	/// Displays a list of all the players in the session.
	/// </summary>
	// Token: 0x02000391 RID: 913
	[ScriptPath("res://src/Core/Nodes/Multiplayer/NMultiplayerPlayerStateContainer.cs")]
	public class NMultiplayerPlayerStateContainer : Control
	{
		// Token: 0x1700085A RID: 2138
		// (get) Token: 0x06003692 RID: 13970 RVA: 0x001999C8 File Offset: 0x00197BC8
		[Nullable(2)]
		public NMultiplayerPlayerState FirstPlayerState
		{
			[NullableContext(2)]
			get
			{
				return base.GetChild<NMultiplayerPlayerState>(0, false);
			}
		}

		// Token: 0x06003693 RID: 13971 RVA: 0x001999D2 File Offset: 0x00197BD2
		public override void _EnterTree()
		{
			ActiveScreenContext.Instance.Updated += this.UpdateNavigation;
		}

		// Token: 0x06003694 RID: 13972 RVA: 0x001999EA File Offset: 0x00197BEA
		public override void _ExitTree()
		{
			ActiveScreenContext.Instance.Updated -= this.UpdateNavigation;
		}

		// Token: 0x06003695 RID: 13973 RVA: 0x00199A04 File Offset: 0x00197C04
		[NullableContext(1)]
		public override void _Input(InputEvent inputEvent)
		{
			if (inputEvent.IsActionReleased(DebugHotkey.hideMpHealthBars, false))
			{
				base.Visible = !base.Visible;
				NGame.Instance.AddChildSafely(NFullscreenTextVfx.Create((!base.Visible) ? "Hide MP Health bars" : "Show MP Health bars"));
			}
		}

		// Token: 0x06003696 RID: 13974 RVA: 0x00199A54 File Offset: 0x00197C54
		[NullableContext(1)]
		public void Initialize(RunState runState)
		{
			this._runState = runState;
			if (this._runState.Players.Count <= 1)
			{
				return;
			}
			Player me = LocalContext.GetMe(this._runState);
			NMultiplayerPlayerState nmultiplayerPlayerState = NMultiplayerPlayerState.Create(me);
			this.AddChildSafely(nmultiplayerPlayerState);
			this._nodes.Add(nmultiplayerPlayerState);
			foreach (Player player in this._runState.Players.Except<Player>(new <>z__ReadOnlySingleElementList<Player>(me)))
			{
				NMultiplayerPlayerState nmultiplayerPlayerState2 = NMultiplayerPlayerState.Create(player);
				this.AddChildSafely(nmultiplayerPlayerState2);
				this._nodes.Add(nmultiplayerPlayerState2);
			}
			this.UpdatePosition();
			NRun.Instance.GlobalUi.RelicInventory.Connect(NRelicInventory.SignalName.RelicsChanged, Callable.From(new Action(this.UpdatePositionAfterOneFrame)), 0U);
			base.GetViewport().Connect(Viewport.SignalName.SizeChanged, Callable.From(new Action(this.UpdatePositionAfterOneFrame)), 0U);
			for (int i = 0; i < base.GetChildCount(false); i++)
			{
				Control hitbox = base.GetChild<NMultiplayerPlayerState>(i, false).Hitbox;
				hitbox.FocusNeighborLeft = hitbox.GetPath();
				hitbox.FocusNeighborTop = ((i > 0) ? base.GetChild<NMultiplayerPlayerState>(i - 1, false).Hitbox.GetPath() : null);
				hitbox.FocusNeighborBottom = ((i < base.GetChildCount(false) - 1) ? base.GetChild<NMultiplayerPlayerState>(i + 1, false).Hitbox.GetPath() : null);
			}
		}

		// Token: 0x06003697 RID: 13975 RVA: 0x00199BE4 File Offset: 0x00197DE4
		private void UpdateNavigation()
		{
			Control activeScreenProxy = NRun.Instance.GlobalUi.TopBar.ActiveScreenProxy;
			NodePath nodePath = (activeScreenProxy.IsValid() ? activeScreenProxy.GetPath() : null);
			for (int i = 0; i < base.GetChildCount(false); i++)
			{
				Control hitbox = base.GetChild<NMultiplayerPlayerState>(i, false).Hitbox;
				hitbox.FocusNeighborTop = ((i > 0) ? base.GetChild<NMultiplayerPlayerState>(i - 1, false).Hitbox.GetPath() : null);
				hitbox.FocusNeighborBottom = ((i < base.GetChildCount(false) - 1) ? base.GetChild<NMultiplayerPlayerState>(i + 1, false).Hitbox.GetPath() : nodePath);
			}
		}

		/// <summary>
		/// Lock navigation so you cannot navigate away from the player states
		/// Important for controller navigation while potion targeting an ally
		/// </summary>
		// Token: 0x06003698 RID: 13976 RVA: 0x00199C84 File Offset: 0x00197E84
		public void LockNavigation()
		{
			for (int i = 0; i < base.GetChildCount(false); i++)
			{
				Control hitbox = base.GetChild<NMultiplayerPlayerState>(i, false).Hitbox;
				hitbox.FocusNeighborTop = ((i > 0) ? base.GetChild<NMultiplayerPlayerState>(i - 1, false).Hitbox.GetPath() : hitbox.GetPath());
				hitbox.FocusNeighborBottom = ((i < base.GetChildCount(false) - 1) ? base.GetChild<NMultiplayerPlayerState>(i + 1, false).Hitbox.GetPath() : hitbox.GetPath());
				hitbox.FocusNeighborLeft = hitbox.GetPath();
				hitbox.FocusNeighborRight = hitbox.GetPath();
			}
		}

		// Token: 0x06003699 RID: 13977 RVA: 0x00199D20 File Offset: 0x00197F20
		public void UnlockNavigation()
		{
			this.UpdateNavigation();
		}

		// Token: 0x0600369A RID: 13978 RVA: 0x00199D28 File Offset: 0x00197F28
		private void UpdatePositionAfterOneFrame()
		{
			TaskHelper.RunSafely(this.UpdatePositionAfterOneFrameAsync());
		}

		// Token: 0x0600369B RID: 13979 RVA: 0x00199D38 File Offset: 0x00197F38
		[NullableContext(1)]
		private async Task UpdatePositionAfterOneFrameAsync()
		{
			await this.AwaitProcessFrame(default(CancellationToken));
			this.UpdatePosition();
		}

		// Token: 0x0600369C RID: 13980 RVA: 0x00199D7B File Offset: 0x00197F7B
		private void UpdatePosition()
		{
			base.Position = this.GetTargetPosition();
		}

		// Token: 0x0600369D RID: 13981 RVA: 0x00199D8C File Offset: 0x00197F8C
		private Vector2 GetTargetPosition()
		{
			NRelicInventory relicInventory = NRun.Instance.GlobalUi.RelicInventory;
			int lineCount = relicInventory.GetLineCount();
			Vector2 vector;
			if (lineCount == 0 || relicInventory.GetChildCount(false) == 0)
			{
				vector = relicInventory.GetDefaultPosition();
			}
			else
			{
				float y = relicInventory.GetChild<Control>(0, false).Size.Y;
				float num = (float)relicInventory.GetThemeConstant(ThemeConstants.FlowContainer.VSeparation, "FlowContainer");
				vector = relicInventory.GetDefaultPosition() + (float)lineCount * (y + num) * Vector2.Down;
			}
			if (this._hidden)
			{
				vector.X = -base.Size.X;
			}
			return vector;
		}

		// Token: 0x0600369E RID: 13982 RVA: 0x00199E2C File Offset: 0x0019802C
		[NullableContext(1)]
		public void HighlightPlayer(Player player)
		{
			NMultiplayerPlayerState nmultiplayerPlayerState = this._nodes.FirstOrDefault<NMultiplayerPlayerState>((NMultiplayerPlayerState n) => n.Player == player);
			if (nmultiplayerPlayerState != null)
			{
				nmultiplayerPlayerState.OnCreatureHovered();
			}
		}

		// Token: 0x0600369F RID: 13983 RVA: 0x00199E68 File Offset: 0x00198068
		[NullableContext(1)]
		public void UnhighlightPlayer(Player player)
		{
			NMultiplayerPlayerState nmultiplayerPlayerState = this._nodes.FirstOrDefault<NMultiplayerPlayerState>((NMultiplayerPlayerState n) => n.Player == player);
			if (nmultiplayerPlayerState != null)
			{
				nmultiplayerPlayerState.OnCreatureUnhovered();
			}
		}

		// Token: 0x060036A0 RID: 13984 RVA: 0x00199EA4 File Offset: 0x001980A4
		[NullableContext(1)]
		public void FlashPlayerReady(Player player)
		{
			NMultiplayerPlayerState nmultiplayerPlayerState = this._nodes.FirstOrDefault<NMultiplayerPlayerState>((NMultiplayerPlayerState n) => n.Player == player);
			if (nmultiplayerPlayerState != null)
			{
				nmultiplayerPlayerState.FlashPlayerReady();
			}
		}

		// Token: 0x060036A1 RID: 13985 RVA: 0x00199EE0 File Offset: 0x001980E0
		public void AnimHide()
		{
			this._hidden = true;
			Tween tween = this._tween;
			if (tween != null)
			{
				tween.Kill();
			}
			this._tween = base.CreateTween();
			this._tween.TweenProperty(this, "position", this.GetTargetPosition(), 0.20000000298023224).SetTrans(4L).SetEase(2L);
			this._tween.TweenProperty(this, "modulate:a", 0f, 0.20000000298023224).SetTrans(4L).SetEase(2L);
		}

		// Token: 0x060036A2 RID: 13986 RVA: 0x00199F84 File Offset: 0x00198184
		public void AnimShow()
		{
			this._hidden = false;
			Tween tween = this._tween;
			if (tween != null)
			{
				tween.Kill();
			}
			this._tween = base.CreateTween();
			this._tween.TweenProperty(this, "position", this.GetTargetPosition(), 0.25).SetTrans(4L).SetEase(1L);
			this._tween.TweenProperty(this, "modulate:a", 1f, 0.15000000596046448).SetTrans(4L).SetEase(0L);
		}

		// Token: 0x060036A3 RID: 13987 RVA: 0x0019A028 File Offset: 0x00198228
		public void ShowImmediately()
		{
			Tween tween = this._tween;
			if (tween != null)
			{
				tween.Kill();
			}
			this._hidden = false;
			base.Position = this.GetTargetPosition();
			Color modulate = base.Modulate;
			modulate.A = 1f;
			base.Modulate = modulate;
		}

		// Token: 0x060036A4 RID: 13988 RVA: 0x0019A074 File Offset: 0x00198274
		public void HideImmediately()
		{
			Tween tween = this._tween;
			if (tween != null)
			{
				tween.Kill();
			}
			this._hidden = true;
			base.Position = this.GetTargetPosition();
			Color modulate = base.Modulate;
			modulate.A = 0f;
			base.Modulate = modulate;
		}

		/// <summary>
		/// Get the method information for all the methods declared in this class.
		/// This method is used by Godot to register the available methods in the editor.
		/// Do not call this method.
		/// </summary>
		// Token: 0x060036A5 RID: 13989 RVA: 0x0019A0C0 File Offset: 0x001982C0
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal static List<MethodInfo> GetGodotMethodList()
		{
			return new List<MethodInfo>(13)
			{
				new MethodInfo(NMultiplayerPlayerStateContainer.MethodName._EnterTree, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NMultiplayerPlayerStateContainer.MethodName._ExitTree, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NMultiplayerPlayerStateContainer.MethodName._Input, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, new List<PropertyInfo>
				{
					new PropertyInfo(24L, "inputEvent", 0L, "", 6L, new StringName("InputEvent"), false)
				}, null),
				new MethodInfo(NMultiplayerPlayerStateContainer.MethodName.UpdateNavigation, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NMultiplayerPlayerStateContainer.MethodName.LockNavigation, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NMultiplayerPlayerStateContainer.MethodName.UnlockNavigation, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NMultiplayerPlayerStateContainer.MethodName.UpdatePositionAfterOneFrame, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NMultiplayerPlayerStateContainer.MethodName.UpdatePosition, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NMultiplayerPlayerStateContainer.MethodName.GetTargetPosition, new PropertyInfo(5L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NMultiplayerPlayerStateContainer.MethodName.AnimHide, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NMultiplayerPlayerStateContainer.MethodName.AnimShow, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NMultiplayerPlayerStateContainer.MethodName.ShowImmediately, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NMultiplayerPlayerStateContainer.MethodName.HideImmediately, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null)
			};
		}

		/// <inheritdoc />
		// Token: 0x060036A6 RID: 13990 RVA: 0x0019A36C File Offset: 0x0019856C
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool InvokeGodotClassMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
		{
			if ((ref method) == NMultiplayerPlayerStateContainer.MethodName._EnterTree && args.Count == 0)
			{
				this._EnterTree();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NMultiplayerPlayerStateContainer.MethodName._ExitTree && args.Count == 0)
			{
				this._ExitTree();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NMultiplayerPlayerStateContainer.MethodName._Input && args.Count == 1)
			{
				this._Input(VariantUtils.ConvertTo<InputEvent>(args[0]));
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NMultiplayerPlayerStateContainer.MethodName.UpdateNavigation && args.Count == 0)
			{
				this.UpdateNavigation();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NMultiplayerPlayerStateContainer.MethodName.LockNavigation && args.Count == 0)
			{
				this.LockNavigation();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NMultiplayerPlayerStateContainer.MethodName.UnlockNavigation && args.Count == 0)
			{
				this.UnlockNavigation();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NMultiplayerPlayerStateContainer.MethodName.UpdatePositionAfterOneFrame && args.Count == 0)
			{
				this.UpdatePositionAfterOneFrame();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NMultiplayerPlayerStateContainer.MethodName.UpdatePosition && args.Count == 0)
			{
				this.UpdatePosition();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NMultiplayerPlayerStateContainer.MethodName.GetTargetPosition && args.Count == 0)
			{
				Vector2 targetPosition = this.GetTargetPosition();
				ret = VariantUtils.CreateFrom<Vector2>(ref targetPosition);
				return true;
			}
			if ((ref method) == NMultiplayerPlayerStateContainer.MethodName.AnimHide && args.Count == 0)
			{
				this.AnimHide();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NMultiplayerPlayerStateContainer.MethodName.AnimShow && args.Count == 0)
			{
				this.AnimShow();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NMultiplayerPlayerStateContainer.MethodName.ShowImmediately && args.Count == 0)
			{
				this.ShowImmediately();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NMultiplayerPlayerStateContainer.MethodName.HideImmediately && args.Count == 0)
			{
				this.HideImmediately();
				ret = default(godot_variant);
				return true;
			}
			return base.InvokeGodotClassMethod(ref method, args, ref ret);
		}

		/// <inheritdoc />
		// Token: 0x060036A7 RID: 13991 RVA: 0x0019A578 File Offset: 0x00198778
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool HasGodotClassMethod(in godot_string_name method)
		{
			return (ref method) == NMultiplayerPlayerStateContainer.MethodName._EnterTree || (ref method) == NMultiplayerPlayerStateContainer.MethodName._ExitTree || (ref method) == NMultiplayerPlayerStateContainer.MethodName._Input || (ref method) == NMultiplayerPlayerStateContainer.MethodName.UpdateNavigation || (ref method) == NMultiplayerPlayerStateContainer.MethodName.LockNavigation || (ref method) == NMultiplayerPlayerStateContainer.MethodName.UnlockNavigation || (ref method) == NMultiplayerPlayerStateContainer.MethodName.UpdatePositionAfterOneFrame || (ref method) == NMultiplayerPlayerStateContainer.MethodName.UpdatePosition || (ref method) == NMultiplayerPlayerStateContainer.MethodName.GetTargetPosition || (ref method) == NMultiplayerPlayerStateContainer.MethodName.AnimHide || (ref method) == NMultiplayerPlayerStateContainer.MethodName.AnimShow || (ref method) == NMultiplayerPlayerStateContainer.MethodName.ShowImmediately || (ref method) == NMultiplayerPlayerStateContainer.MethodName.HideImmediately || base.HasGodotClassMethod(ref method);
		}

		/// <inheritdoc />
		// Token: 0x060036A8 RID: 13992 RVA: 0x0019A64F File Offset: 0x0019884F
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool SetGodotClassPropertyValue(in godot_string_name name, in godot_variant value)
		{
			if ((ref name) == NMultiplayerPlayerStateContainer.PropertyName._tween)
			{
				this._tween = VariantUtils.ConvertTo<Tween>(ref value);
				return true;
			}
			if ((ref name) == NMultiplayerPlayerStateContainer.PropertyName._hidden)
			{
				this._hidden = VariantUtils.ConvertTo<bool>(ref value);
				return true;
			}
			return base.SetGodotClassPropertyValue(ref name, ref value);
		}

		/// <inheritdoc />
		// Token: 0x060036A9 RID: 13993 RVA: 0x0019A690 File Offset: 0x00198890
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
		{
			if ((ref name) == NMultiplayerPlayerStateContainer.PropertyName.FirstPlayerState)
			{
				NMultiplayerPlayerState firstPlayerState = this.FirstPlayerState;
				value = VariantUtils.CreateFrom<NMultiplayerPlayerState>(ref firstPlayerState);
				return true;
			}
			if ((ref name) == NMultiplayerPlayerStateContainer.PropertyName._tween)
			{
				value = VariantUtils.CreateFrom<Tween>(ref this._tween);
				return true;
			}
			if ((ref name) == NMultiplayerPlayerStateContainer.PropertyName._hidden)
			{
				value = VariantUtils.CreateFrom<bool>(ref this._hidden);
				return true;
			}
			return base.GetGodotClassPropertyValue(ref name, ref value);
		}

		/// <summary>
		/// Get the property information for all the properties declared in this class.
		/// This method is used by Godot to register the available properties in the editor.
		/// Do not call this method.
		/// </summary>
		// Token: 0x060036AA RID: 13994 RVA: 0x0019A708 File Offset: 0x00198908
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal static List<PropertyInfo> GetGodotPropertyList()
		{
			return new List<PropertyInfo>
			{
				new PropertyInfo(24L, NMultiplayerPlayerStateContainer.PropertyName.FirstPlayerState, 0L, "", 4096L, false),
				new PropertyInfo(24L, NMultiplayerPlayerStateContainer.PropertyName._tween, 0L, "", 4096L, false),
				new PropertyInfo(1L, NMultiplayerPlayerStateContainer.PropertyName._hidden, 0L, "", 4096L, false)
			};
		}

		/// <inheritdoc />
		// Token: 0x060036AB RID: 13995 RVA: 0x0019A77E File Offset: 0x0019897E
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void SaveGodotObjectData(GodotSerializationInfo info)
		{
			base.SaveGodotObjectData(info);
			info.AddProperty(NMultiplayerPlayerStateContainer.PropertyName._tween, Variant.From<Tween>(ref this._tween));
			info.AddProperty(NMultiplayerPlayerStateContainer.PropertyName._hidden, Variant.From<bool>(ref this._hidden));
		}

		/// <inheritdoc />
		// Token: 0x060036AC RID: 13996 RVA: 0x0019A7B4 File Offset: 0x001989B4
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void RestoreGodotObjectData(GodotSerializationInfo info)
		{
			base.RestoreGodotObjectData(info);
			Variant variant;
			if (info.TryGetProperty(NMultiplayerPlayerStateContainer.PropertyName._tween, ref variant))
			{
				this._tween = variant.As<Tween>();
			}
			Variant variant2;
			if (info.TryGetProperty(NMultiplayerPlayerStateContainer.PropertyName._hidden, ref variant2))
			{
				this._hidden = variant2.As<bool>();
			}
		}

		// Token: 0x04001528 RID: 5416
		[Nullable(1)]
		private IRunState _runState;

		// Token: 0x04001529 RID: 5417
		[Nullable(1)]
		private readonly List<NMultiplayerPlayerState> _nodes = new List<NMultiplayerPlayerState>();

		// Token: 0x0400152A RID: 5418
		[Nullable(2)]
		private Tween _tween;

		// Token: 0x0400152B RID: 5419
		private bool _hidden;

		/// <summary>
		/// Cached StringNames for the methods contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x02001732 RID: 5938
		public class MethodName : Control.MethodName
		{
			/// <summary>
			/// Cached name for the '_EnterTree' method.
			/// </summary>
			// Token: 0x04005A98 RID: 23192
			public static readonly StringName _EnterTree = "_EnterTree";

			/// <summary>
			/// Cached name for the '_ExitTree' method.
			/// </summary>
			// Token: 0x04005A99 RID: 23193
			public static readonly StringName _ExitTree = "_ExitTree";

			/// <summary>
			/// Cached name for the '_Input' method.
			/// </summary>
			// Token: 0x04005A9A RID: 23194
			public static readonly StringName _Input = "_Input";

			/// <summary>
			/// Cached name for the 'UpdateNavigation' method.
			/// </summary>
			// Token: 0x04005A9B RID: 23195
			public static readonly StringName UpdateNavigation = "UpdateNavigation";

			/// <summary>
			/// Cached name for the 'LockNavigation' method.
			/// </summary>
			// Token: 0x04005A9C RID: 23196
			public static readonly StringName LockNavigation = "LockNavigation";

			/// <summary>
			/// Cached name for the 'UnlockNavigation' method.
			/// </summary>
			// Token: 0x04005A9D RID: 23197
			public static readonly StringName UnlockNavigation = "UnlockNavigation";

			/// <summary>
			/// Cached name for the 'UpdatePositionAfterOneFrame' method.
			/// </summary>
			// Token: 0x04005A9E RID: 23198
			public static readonly StringName UpdatePositionAfterOneFrame = "UpdatePositionAfterOneFrame";

			/// <summary>
			/// Cached name for the 'UpdatePosition' method.
			/// </summary>
			// Token: 0x04005A9F RID: 23199
			public static readonly StringName UpdatePosition = "UpdatePosition";

			/// <summary>
			/// Cached name for the 'GetTargetPosition' method.
			/// </summary>
			// Token: 0x04005AA0 RID: 23200
			public static readonly StringName GetTargetPosition = "GetTargetPosition";

			/// <summary>
			/// Cached name for the 'AnimHide' method.
			/// </summary>
			// Token: 0x04005AA1 RID: 23201
			public static readonly StringName AnimHide = "AnimHide";

			/// <summary>
			/// Cached name for the 'AnimShow' method.
			/// </summary>
			// Token: 0x04005AA2 RID: 23202
			public static readonly StringName AnimShow = "AnimShow";

			/// <summary>
			/// Cached name for the 'ShowImmediately' method.
			/// </summary>
			// Token: 0x04005AA3 RID: 23203
			public static readonly StringName ShowImmediately = "ShowImmediately";

			/// <summary>
			/// Cached name for the 'HideImmediately' method.
			/// </summary>
			// Token: 0x04005AA4 RID: 23204
			public static readonly StringName HideImmediately = "HideImmediately";
		}

		/// <summary>
		/// Cached StringNames for the properties and fields contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x02001733 RID: 5939
		public class PropertyName : Control.PropertyName
		{
			/// <summary>
			/// Cached name for the 'FirstPlayerState' property.
			/// </summary>
			// Token: 0x04005AA5 RID: 23205
			public static readonly StringName FirstPlayerState = "FirstPlayerState";

			/// <summary>
			/// Cached name for the '_tween' field.
			/// </summary>
			// Token: 0x04005AA6 RID: 23206
			public static readonly StringName _tween = "_tween";

			/// <summary>
			/// Cached name for the '_hidden' field.
			/// </summary>
			// Token: 0x04005AA7 RID: 23207
			public static readonly StringName _hidden = "_hidden";
		}

		/// <summary>
		/// Cached StringNames for the signals contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x02001734 RID: 5940
		public class SignalName : Control.SignalName
		{
		}
	}
}
