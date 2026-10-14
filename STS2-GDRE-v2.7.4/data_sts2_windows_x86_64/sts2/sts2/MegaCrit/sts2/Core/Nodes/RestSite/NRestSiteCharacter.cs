using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using Godot;
using Godot.Bridge;
using Godot.NativeInterop;
using MegaCrit.Sts2.Core.Assets;
using MegaCrit.Sts2.Core.Bindings.MegaSpine;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Entities.RestSite;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.Models.Characters;
using MegaCrit.Sts2.Core.Nodes.Combat;
using MegaCrit.Sts2.Core.Nodes.GodotExtensions;
using MegaCrit.Sts2.Core.Nodes.Vfx;
using MegaCrit.Sts2.Core.Nodes.Vfx.Utilities;
using MegaCrit.Sts2.Core.Random;

namespace MegaCrit.Sts2.Core.Nodes.RestSite
{
	// Token: 0x02000375 RID: 885
	[NullableContext(1)]
	[Nullable(0)]
	[ScriptPath("res://src/Core/Nodes/RestSite/NRestSiteCharacter.cs")]
	public class NRestSiteCharacter : Node2D
	{
		// Token: 0x17000823 RID: 2083
		// (get) Token: 0x0600342E RID: 13358 RVA: 0x00184E16 File Offset: 0x00183016
		// (set) Token: 0x0600342F RID: 13359 RVA: 0x00184E1E File Offset: 0x0018301E
		public Control Hitbox { get; private set; }

		// Token: 0x17000824 RID: 2084
		// (get) Token: 0x06003430 RID: 13360 RVA: 0x00184E27 File Offset: 0x00183027
		// (set) Token: 0x06003431 RID: 13361 RVA: 0x00184E2F File Offset: 0x0018302F
		public Player Player { get; private set; }

		// Token: 0x06003432 RID: 13362 RVA: 0x00184E38 File Offset: 0x00183038
		public static NRestSiteCharacter Create(Player player, int characterIndex)
		{
			NRestSiteCharacter nrestSiteCharacter = PreloadManager.Cache.GetScene(player.Character.RestSiteAnimPath).Instantiate<NRestSiteCharacter>(0L);
			nrestSiteCharacter.Player = player;
			nrestSiteCharacter._characterIndex = characterIndex;
			return nrestSiteCharacter;
		}

		// Token: 0x06003433 RID: 13363 RVA: 0x00184E71 File Offset: 0x00183071
		public override void _EnterTree()
		{
			this._cts = new CancellationTokenSource();
		}

		// Token: 0x06003434 RID: 13364 RVA: 0x00184E7E File Offset: 0x0018307E
		public override void _ExitTree()
		{
			CancellationTokenSource thoughtBubbleGoAwayCancellation = this._thoughtBubbleGoAwayCancellation;
			if (thoughtBubbleGoAwayCancellation != null)
			{
				thoughtBubbleGoAwayCancellation.Cancel();
			}
			this._cts.Cancel();
		}

		// Token: 0x06003435 RID: 13365 RVA: 0x00184E9C File Offset: 0x0018309C
		public override void _Ready()
		{
			this._controlRoot = base.GetNode<Control>("ControlRoot");
			this.Hitbox = base.GetNode<Control>("%Hitbox");
			this._selectionReticle = base.GetNode<NSelectionReticle>("%SelectionReticle");
			this._leftThoughtAnchor = base.GetNode<Control>("%ThoughtBubbleLeft");
			this._rightThoughtAnchor = base.GetNode<Control>("%ThoughtBubbleRight");
			string text;
			switch (this.Player.RunState.CurrentActIndex)
			{
			case 0:
				text = "overgrowth_loop";
				break;
			case 1:
				text = "hive_loop";
				break;
			case 2:
				text = "glory_loop";
				break;
			default:
				throw new InvalidOperationException("Unexpected act");
			}
			string animName = text;
			Action<MegaAnimationState> <>9__0;
			foreach (Node2D node2D in this.GetChildSpineNodes())
			{
				MegaSprite megaSprite = new MegaSprite(node2D);
				MegaSprite megaSprite2 = megaSprite;
				Action<MegaAnimationState> action;
				if ((action = <>9__0) == null)
				{
					action = (<>9__0 = delegate(MegaAnimationState animState)
					{
						animState.SetAnimation(animName, true, 0);
						using (MegaTrackEntry current = animState.GetCurrent(0))
						{
							if (current != null)
							{
								current.SetTrackTime(current.GetAnimationEnd() * Rng.Chaotic.NextFloat(1f));
							}
						}
					});
				}
				this.RunWhenSpineReady(megaSprite2, action);
			}
			if (this.Player.Character is Necrobinder)
			{
				Sprite2D node = base.GetNode<Sprite2D>("%NecroFire");
				Sprite2D node2 = base.GetNode<Sprite2D>("%OstyFire");
				this.RandomizeFire((ShaderMaterial)node.Material);
				this.RandomizeFire((ShaderMaterial)node2.Material);
				if (this._characterIndex >= 2)
				{
					Node2D node3 = base.GetNode<Node2D>("Osty");
					Node2D node4 = base.GetNode<Node2D>("OstyRightAnchor");
					node3.Position = node4.Position;
					this.MoveChildSafely(node3, 0);
				}
			}
			this.Hitbox.Connect(Control.SignalName.FocusEntered, Callable.From(new Action(this.OnFocus)), 0U);
			this.Hitbox.Connect(Control.SignalName.FocusExited, Callable.From(new Action(this.OnUnfocus)), 0U);
			this.Hitbox.Connect(Control.SignalName.MouseEntered, Callable.From(new Action(this.OnFocus)), 0U);
			this.Hitbox.Connect(Control.SignalName.MouseExited, Callable.From(new Action(this.OnUnfocus)), 0U);
		}

		// Token: 0x06003436 RID: 13366 RVA: 0x0018510C File Offset: 0x0018330C
		private void RandomizeFire(ShaderMaterial mat)
		{
			mat.SetShaderParameter(NRestSiteCharacter._globalOffset, new Vector2(Rng.Chaotic.NextFloat(-50f, 50f), Rng.Chaotic.NextFloat(-50f, 50f)));
			mat.SetShaderParameter(NRestSiteCharacter._noise1Panning, mat.GetShaderParameter(NRestSiteCharacter._noise1Panning).AsVector2() + new Vector2(Rng.Chaotic.NextFloat(-0.1f, 0.1f), Rng.Chaotic.NextFloat(-0.1f, 0.1f)));
			mat.SetShaderParameter(NRestSiteCharacter._noise2Panning, mat.GetShaderParameter(NRestSiteCharacter._noise2Panning).AsVector2() + new Vector2(Rng.Chaotic.NextFloat(-0.1f, 0.1f), Rng.Chaotic.NextFloat(-0.1f, 0.1f)));
		}

		// Token: 0x06003437 RID: 13367 RVA: 0x00185200 File Offset: 0x00183400
		private void OnFocus()
		{
			if (NTargetManager.Instance.IsInSelection && NTargetManager.Instance.AllowedToTargetNode(this))
			{
				NTargetManager.Instance.OnNodeHovered(this);
				this._selectionReticle.OnSelect();
				NRun.Instance.GlobalUi.MultiplayerPlayerContainer.HighlightPlayer(this.Player);
			}
		}

		// Token: 0x06003438 RID: 13368 RVA: 0x00185256 File Offset: 0x00183456
		private void OnUnfocus()
		{
			if (NTargetManager.Instance.IsInSelection && NTargetManager.Instance.AllowedToTargetNode(this))
			{
				NTargetManager.Instance.OnNodeUnhovered(this);
			}
			this.Deselect();
		}

		// Token: 0x06003439 RID: 13369 RVA: 0x00185282 File Offset: 0x00183482
		public void Deselect()
		{
			if (this._selectionReticle.IsSelected)
			{
				this._selectionReticle.OnDeselect();
			}
			NRun.Instance.GlobalUi.MultiplayerPlayerContainer.UnhighlightPlayer(this.Player);
		}

		// Token: 0x0600343A RID: 13370 RVA: 0x001852B8 File Offset: 0x001834B8
		public void FlipX()
		{
			Vector2 vector;
			foreach (Node2D node2D in this.GetChildSpineNodes())
			{
				Node2D node2D2 = node2D;
				vector = node2D.Scale;
				vector.X = -node2D.Scale.X;
				node2D2.Scale = vector;
				Node2D node2D3 = node2D;
				vector = node2D.Position;
				vector.X = -node2D.Position.X;
				node2D3.Position = vector;
			}
			Control controlRoot = this._controlRoot;
			vector = this._controlRoot.Scale;
			vector.X = -this._controlRoot.Scale.X;
			controlRoot.Scale = vector;
		}

		// Token: 0x0600343B RID: 13371 RVA: 0x00185370 File Offset: 0x00183570
		public void HideFlameGlow()
		{
			foreach (Node2D node2D in this.GetChildSpineNodes())
			{
				MegaSprite megaSprite = new MegaSprite(node2D);
				if (megaSprite.HasAnimation("_tracks/light_off"))
				{
					megaSprite.GetAnimationState().SetAnimation("_tracks/light_off", true, 1);
				}
			}
		}

		// Token: 0x0600343C RID: 13372 RVA: 0x001853E4 File Offset: 0x001835E4
		[NullableContext(2)]
		public void ShowHoveredRestSiteOption(RestSiteOption option)
		{
			this._hoveredRestSiteOption = option;
			this.RefreshThoughtBubbleVfx();
		}

		// Token: 0x0600343D RID: 13373 RVA: 0x001853F3 File Offset: 0x001835F3
		[NullableContext(2)]
		public void SetSelectingRestSiteOption(RestSiteOption option)
		{
			this._selectingRestSiteOption = option;
			this.RefreshThoughtBubbleVfx();
		}

		// Token: 0x0600343E RID: 13374 RVA: 0x00185404 File Offset: 0x00183604
		private void RefreshThoughtBubbleVfx()
		{
			if (this._selectedOptionConfirmation != null)
			{
				return;
			}
			RestSiteOption restSiteOption = this._selectingRestSiteOption ?? this._hoveredRestSiteOption;
			if (this._restSiteOptionInThoughtBubble == restSiteOption)
			{
				return;
			}
			this._restSiteOptionInThoughtBubble = restSiteOption;
			if (restSiteOption == null)
			{
				TaskHelper.RunSafely(this.RemoveThoughtBubbleAfterDelay());
				return;
			}
			CancellationTokenSource thoughtBubbleGoAwayCancellation = this._thoughtBubbleGoAwayCancellation;
			if (thoughtBubbleGoAwayCancellation != null)
			{
				thoughtBubbleGoAwayCancellation.Cancel();
			}
			if (this._thoughtBubbleVfx == null)
			{
				int characterIndex = this._characterIndex;
				bool flag = characterIndex == 0 || characterIndex == 3;
				bool flag2 = flag;
				this._thoughtBubbleVfx = NThoughtBubbleVfx.Create(restSiteOption.Icon, flag2 ? DialogueSide.Right : DialogueSide.Left, null);
				ShaderMaterial shaderMaterial = (ShaderMaterial)this._thoughtBubbleVfx.GetNode<TextureRect>("%Image").Material;
				shaderMaterial.SetShaderParameter(NRestSiteCharacter._s, 0.145f);
				shaderMaterial.SetShaderParameter(NRestSiteCharacter._v, 0.85f);
				this.AddChildSafely(this._thoughtBubbleVfx);
				this._thoughtBubbleVfx.GlobalPosition = this.GetRestSiteOptionAnchor().GlobalPosition;
				return;
			}
			this._thoughtBubbleVfx.SetTexture(restSiteOption.Icon);
		}

		// Token: 0x0600343F RID: 13375 RVA: 0x0018552C File Offset: 0x0018372C
		public void ShowSelectedRestSiteOption(RestSiteOption option)
		{
			if (this._thoughtBubbleVfx != null)
			{
				TaskHelper.RunSafely(this._thoughtBubbleVfx.GoAway());
			}
			this._thoughtBubbleVfx = null;
			this._selectedOptionConfirmation = PreloadManager.Cache.GetScene(NRestSiteCharacter._multiplayerConfirmationScenePath).Instantiate<Control>(0L);
			this._selectedOptionConfirmation.GetNode<TextureRect>("%Icon").Texture = option.Icon;
			this.AddChildSafely(this._selectedOptionConfirmation);
			int characterIndex = this._characterIndex;
			bool flag = characterIndex == 0 || characterIndex == 3;
			bool flag2 = flag;
			this._selectedOptionConfirmation.GlobalPosition = this.GetRestSiteOptionAnchor().GlobalPosition;
			this._selectedOptionConfirmation.Position += NRestSiteCharacter._multiplayerConfirmationOffset + (flag2 ? NRestSiteCharacter._multiplayerConfirmationFlipOffset : Vector2.Zero);
		}

		// Token: 0x06003440 RID: 13376 RVA: 0x001855FC File Offset: 0x001837FC
		public void Shake()
		{
			TaskHelper.RunSafely(this.DoShake());
		}

		// Token: 0x06003441 RID: 13377 RVA: 0x0018560C File Offset: 0x0018380C
		private async Task DoShake()
		{
			ScreenPunchInstance shake = new ScreenPunchInstance(15f, 0.4, 0f);
			Vector2 originalPosition = base.Position;
			while (!shake.IsDone)
			{
				float num = await this.AwaitProcessFrame(this._cts.Token);
				float num2 = num;
				Vector2 vector = shake.Update((double)num2);
				base.Position = originalPosition + vector;
			}
			base.Position = originalPosition;
		}

		// Token: 0x06003442 RID: 13378 RVA: 0x0018564F File Offset: 0x0018384F
		private Control GetRestSiteOptionAnchor()
		{
			if (this._characterIndex < 2)
			{
				return this._leftThoughtAnchor;
			}
			return this._rightThoughtAnchor;
		}

		// Token: 0x06003443 RID: 13379 RVA: 0x00185668 File Offset: 0x00183868
		private async Task RemoveThoughtBubbleAfterDelay()
		{
			this._thoughtBubbleGoAwayCancellation = new CancellationTokenSource();
			await Cmd.Wait(0.5f, this._thoughtBubbleGoAwayCancellation.Token, false);
			if (!this._thoughtBubbleGoAwayCancellation.IsCancellationRequested)
			{
				if (this._thoughtBubbleVfx != null)
				{
					TaskHelper.RunSafely(this._thoughtBubbleVfx.GoAway());
				}
				this._thoughtBubbleVfx = null;
			}
		}

		// Token: 0x06003444 RID: 13380 RVA: 0x001856AB File Offset: 0x001838AB
		private IEnumerable<Node2D> GetChildSpineNodes()
		{
			NRestSiteCharacter.<GetChildSpineNodes>d__46 <GetChildSpineNodes>d__ = new NRestSiteCharacter.<GetChildSpineNodes>d__46(-2);
			<GetChildSpineNodes>d__.<>4__this = this;
			return <GetChildSpineNodes>d__;
		}

		/// <summary>
		/// Get the method information for all the methods declared in this class.
		/// This method is used by Godot to register the available methods in the editor.
		/// Do not call this method.
		/// </summary>
		// Token: 0x06003445 RID: 13381 RVA: 0x001856BC File Offset: 0x001838BC
		[NullableContext(0)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal static List<MethodInfo> GetGodotMethodList()
		{
			return new List<MethodInfo>(12)
			{
				new MethodInfo(NRestSiteCharacter.MethodName._EnterTree, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NRestSiteCharacter.MethodName._ExitTree, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NRestSiteCharacter.MethodName._Ready, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NRestSiteCharacter.MethodName.RandomizeFire, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, new List<PropertyInfo>
				{
					new PropertyInfo(24L, "mat", 0L, "", 6L, new StringName("ShaderMaterial"), false)
				}, null),
				new MethodInfo(NRestSiteCharacter.MethodName.OnFocus, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NRestSiteCharacter.MethodName.OnUnfocus, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NRestSiteCharacter.MethodName.Deselect, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NRestSiteCharacter.MethodName.FlipX, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NRestSiteCharacter.MethodName.HideFlameGlow, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NRestSiteCharacter.MethodName.RefreshThoughtBubbleVfx, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NRestSiteCharacter.MethodName.Shake, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NRestSiteCharacter.MethodName.GetRestSiteOptionAnchor, new PropertyInfo(24L, "", 0L, "", 6L, new StringName("Control"), false), 1L, null, null)
			};
		}

		/// <inheritdoc />
		// Token: 0x06003446 RID: 13382 RVA: 0x00185944 File Offset: 0x00183B44
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool InvokeGodotClassMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
		{
			if ((ref method) == NRestSiteCharacter.MethodName._EnterTree && args.Count == 0)
			{
				this._EnterTree();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NRestSiteCharacter.MethodName._ExitTree && args.Count == 0)
			{
				this._ExitTree();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NRestSiteCharacter.MethodName._Ready && args.Count == 0)
			{
				this._Ready();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NRestSiteCharacter.MethodName.RandomizeFire && args.Count == 1)
			{
				this.RandomizeFire(VariantUtils.ConvertTo<ShaderMaterial>(args[0]));
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NRestSiteCharacter.MethodName.OnFocus && args.Count == 0)
			{
				this.OnFocus();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NRestSiteCharacter.MethodName.OnUnfocus && args.Count == 0)
			{
				this.OnUnfocus();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NRestSiteCharacter.MethodName.Deselect && args.Count == 0)
			{
				this.Deselect();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NRestSiteCharacter.MethodName.FlipX && args.Count == 0)
			{
				this.FlipX();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NRestSiteCharacter.MethodName.HideFlameGlow && args.Count == 0)
			{
				this.HideFlameGlow();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NRestSiteCharacter.MethodName.RefreshThoughtBubbleVfx && args.Count == 0)
			{
				this.RefreshThoughtBubbleVfx();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NRestSiteCharacter.MethodName.Shake && args.Count == 0)
			{
				this.Shake();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NRestSiteCharacter.MethodName.GetRestSiteOptionAnchor && args.Count == 0)
			{
				Control restSiteOptionAnchor = this.GetRestSiteOptionAnchor();
				ret = VariantUtils.CreateFrom<Control>(ref restSiteOptionAnchor);
				return true;
			}
			return base.InvokeGodotClassMethod(ref method, args, ref ret);
		}

		/// <inheritdoc />
		// Token: 0x06003447 RID: 13383 RVA: 0x00185B2C File Offset: 0x00183D2C
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool HasGodotClassMethod(in godot_string_name method)
		{
			return (ref method) == NRestSiteCharacter.MethodName._EnterTree || (ref method) == NRestSiteCharacter.MethodName._ExitTree || (ref method) == NRestSiteCharacter.MethodName._Ready || (ref method) == NRestSiteCharacter.MethodName.RandomizeFire || (ref method) == NRestSiteCharacter.MethodName.OnFocus || (ref method) == NRestSiteCharacter.MethodName.OnUnfocus || (ref method) == NRestSiteCharacter.MethodName.Deselect || (ref method) == NRestSiteCharacter.MethodName.FlipX || (ref method) == NRestSiteCharacter.MethodName.HideFlameGlow || (ref method) == NRestSiteCharacter.MethodName.RefreshThoughtBubbleVfx || (ref method) == NRestSiteCharacter.MethodName.Shake || (ref method) == NRestSiteCharacter.MethodName.GetRestSiteOptionAnchor || base.HasGodotClassMethod(ref method);
		}

		/// <inheritdoc />
		// Token: 0x06003448 RID: 13384 RVA: 0x00185BF4 File Offset: 0x00183DF4
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool SetGodotClassPropertyValue(in godot_string_name name, in godot_variant value)
		{
			if ((ref name) == NRestSiteCharacter.PropertyName.Hitbox)
			{
				this.Hitbox = VariantUtils.ConvertTo<Control>(ref value);
				return true;
			}
			if ((ref name) == NRestSiteCharacter.PropertyName._controlRoot)
			{
				this._controlRoot = VariantUtils.ConvertTo<Control>(ref value);
				return true;
			}
			if ((ref name) == NRestSiteCharacter.PropertyName._selectionReticle)
			{
				this._selectionReticle = VariantUtils.ConvertTo<NSelectionReticle>(ref value);
				return true;
			}
			if ((ref name) == NRestSiteCharacter.PropertyName._leftThoughtAnchor)
			{
				this._leftThoughtAnchor = VariantUtils.ConvertTo<Control>(ref value);
				return true;
			}
			if ((ref name) == NRestSiteCharacter.PropertyName._rightThoughtAnchor)
			{
				this._rightThoughtAnchor = VariantUtils.ConvertTo<Control>(ref value);
				return true;
			}
			if ((ref name) == NRestSiteCharacter.PropertyName._characterIndex)
			{
				this._characterIndex = VariantUtils.ConvertTo<int>(ref value);
				return true;
			}
			if ((ref name) == NRestSiteCharacter.PropertyName._thoughtBubbleVfx)
			{
				this._thoughtBubbleVfx = VariantUtils.ConvertTo<NThoughtBubbleVfx>(ref value);
				return true;
			}
			if ((ref name) == NRestSiteCharacter.PropertyName._selectedOptionConfirmation)
			{
				this._selectedOptionConfirmation = VariantUtils.ConvertTo<Control>(ref value);
				return true;
			}
			return base.SetGodotClassPropertyValue(ref name, ref value);
		}

		/// <inheritdoc />
		// Token: 0x06003449 RID: 13385 RVA: 0x00185CE4 File Offset: 0x00183EE4
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
		{
			if ((ref name) == NRestSiteCharacter.PropertyName.Hitbox)
			{
				Control hitbox = this.Hitbox;
				value = VariantUtils.CreateFrom<Control>(ref hitbox);
				return true;
			}
			if ((ref name) == NRestSiteCharacter.PropertyName._controlRoot)
			{
				value = VariantUtils.CreateFrom<Control>(ref this._controlRoot);
				return true;
			}
			if ((ref name) == NRestSiteCharacter.PropertyName._selectionReticle)
			{
				value = VariantUtils.CreateFrom<NSelectionReticle>(ref this._selectionReticle);
				return true;
			}
			if ((ref name) == NRestSiteCharacter.PropertyName._leftThoughtAnchor)
			{
				value = VariantUtils.CreateFrom<Control>(ref this._leftThoughtAnchor);
				return true;
			}
			if ((ref name) == NRestSiteCharacter.PropertyName._rightThoughtAnchor)
			{
				value = VariantUtils.CreateFrom<Control>(ref this._rightThoughtAnchor);
				return true;
			}
			if ((ref name) == NRestSiteCharacter.PropertyName._characterIndex)
			{
				value = VariantUtils.CreateFrom<int>(ref this._characterIndex);
				return true;
			}
			if ((ref name) == NRestSiteCharacter.PropertyName._thoughtBubbleVfx)
			{
				value = VariantUtils.CreateFrom<NThoughtBubbleVfx>(ref this._thoughtBubbleVfx);
				return true;
			}
			if ((ref name) == NRestSiteCharacter.PropertyName._selectedOptionConfirmation)
			{
				value = VariantUtils.CreateFrom<Control>(ref this._selectedOptionConfirmation);
				return true;
			}
			return base.GetGodotClassPropertyValue(ref name, ref value);
		}

		/// <summary>
		/// Get the property information for all the properties declared in this class.
		/// This method is used by Godot to register the available properties in the editor.
		/// Do not call this method.
		/// </summary>
		// Token: 0x0600344A RID: 13386 RVA: 0x00185DFC File Offset: 0x00183FFC
		[NullableContext(0)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal static List<PropertyInfo> GetGodotPropertyList()
		{
			return new List<PropertyInfo>
			{
				new PropertyInfo(24L, NRestSiteCharacter.PropertyName._controlRoot, 0L, "", 4096L, false),
				new PropertyInfo(24L, NRestSiteCharacter.PropertyName.Hitbox, 0L, "", 4096L, false),
				new PropertyInfo(24L, NRestSiteCharacter.PropertyName._selectionReticle, 0L, "", 4096L, false),
				new PropertyInfo(24L, NRestSiteCharacter.PropertyName._leftThoughtAnchor, 0L, "", 4096L, false),
				new PropertyInfo(24L, NRestSiteCharacter.PropertyName._rightThoughtAnchor, 0L, "", 4096L, false),
				new PropertyInfo(2L, NRestSiteCharacter.PropertyName._characterIndex, 0L, "", 4096L, false),
				new PropertyInfo(24L, NRestSiteCharacter.PropertyName._thoughtBubbleVfx, 0L, "", 4096L, false),
				new PropertyInfo(24L, NRestSiteCharacter.PropertyName._selectedOptionConfirmation, 0L, "", 4096L, false)
			};
		}

		/// <inheritdoc />
		// Token: 0x0600344B RID: 13387 RVA: 0x00185F18 File Offset: 0x00184118
		[NullableContext(0)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void SaveGodotObjectData(GodotSerializationInfo info)
		{
			base.SaveGodotObjectData(info);
			StringName hitbox = NRestSiteCharacter.PropertyName.Hitbox;
			Control hitbox2 = this.Hitbox;
			info.AddProperty(hitbox, Variant.From<Control>(ref hitbox2));
			info.AddProperty(NRestSiteCharacter.PropertyName._controlRoot, Variant.From<Control>(ref this._controlRoot));
			info.AddProperty(NRestSiteCharacter.PropertyName._selectionReticle, Variant.From<NSelectionReticle>(ref this._selectionReticle));
			info.AddProperty(NRestSiteCharacter.PropertyName._leftThoughtAnchor, Variant.From<Control>(ref this._leftThoughtAnchor));
			info.AddProperty(NRestSiteCharacter.PropertyName._rightThoughtAnchor, Variant.From<Control>(ref this._rightThoughtAnchor));
			info.AddProperty(NRestSiteCharacter.PropertyName._characterIndex, Variant.From<int>(ref this._characterIndex));
			info.AddProperty(NRestSiteCharacter.PropertyName._thoughtBubbleVfx, Variant.From<NThoughtBubbleVfx>(ref this._thoughtBubbleVfx));
			info.AddProperty(NRestSiteCharacter.PropertyName._selectedOptionConfirmation, Variant.From<Control>(ref this._selectedOptionConfirmation));
		}

		/// <inheritdoc />
		// Token: 0x0600344C RID: 13388 RVA: 0x00185FE0 File Offset: 0x001841E0
		[NullableContext(0)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void RestoreGodotObjectData(GodotSerializationInfo info)
		{
			base.RestoreGodotObjectData(info);
			Variant variant;
			if (info.TryGetProperty(NRestSiteCharacter.PropertyName.Hitbox, ref variant))
			{
				this.Hitbox = variant.As<Control>();
			}
			Variant variant2;
			if (info.TryGetProperty(NRestSiteCharacter.PropertyName._controlRoot, ref variant2))
			{
				this._controlRoot = variant2.As<Control>();
			}
			Variant variant3;
			if (info.TryGetProperty(NRestSiteCharacter.PropertyName._selectionReticle, ref variant3))
			{
				this._selectionReticle = variant3.As<NSelectionReticle>();
			}
			Variant variant4;
			if (info.TryGetProperty(NRestSiteCharacter.PropertyName._leftThoughtAnchor, ref variant4))
			{
				this._leftThoughtAnchor = variant4.As<Control>();
			}
			Variant variant5;
			if (info.TryGetProperty(NRestSiteCharacter.PropertyName._rightThoughtAnchor, ref variant5))
			{
				this._rightThoughtAnchor = variant5.As<Control>();
			}
			Variant variant6;
			if (info.TryGetProperty(NRestSiteCharacter.PropertyName._characterIndex, ref variant6))
			{
				this._characterIndex = variant6.As<int>();
			}
			Variant variant7;
			if (info.TryGetProperty(NRestSiteCharacter.PropertyName._thoughtBubbleVfx, ref variant7))
			{
				this._thoughtBubbleVfx = variant7.As<NThoughtBubbleVfx>();
			}
			Variant variant8;
			if (info.TryGetProperty(NRestSiteCharacter.PropertyName._selectedOptionConfirmation, ref variant8))
			{
				this._selectedOptionConfirmation = variant8.As<Control>();
			}
		}

		// Token: 0x0400143D RID: 5181
		private static readonly StringName _v = new StringName("v");

		// Token: 0x0400143E RID: 5182
		private static readonly StringName _s = new StringName("s");

		// Token: 0x0400143F RID: 5183
		private static readonly StringName _noise2Panning = new StringName("Noise2Panning");

		// Token: 0x04001440 RID: 5184
		private static readonly StringName _noise1Panning = new StringName("Noise1Panning");

		// Token: 0x04001441 RID: 5185
		private static readonly StringName _globalOffset = new StringName("GlobalOffset");

		// Token: 0x04001442 RID: 5186
		private static readonly Vector2 _multiplayerConfirmationOffset = new Vector2(-25f, -123f);

		// Token: 0x04001443 RID: 5187
		private static readonly Vector2 _multiplayerConfirmationFlipOffset = new Vector2(-155f, 0f);

		// Token: 0x04001444 RID: 5188
		private static readonly string _multiplayerConfirmationScenePath = SceneHelper.GetScenePath("rest_site/rest_site_multiplayer_confirmation");

		// Token: 0x04001445 RID: 5189
		private Control _controlRoot;

		// Token: 0x04001447 RID: 5191
		private NSelectionReticle _selectionReticle;

		// Token: 0x04001448 RID: 5192
		private Control _leftThoughtAnchor;

		// Token: 0x04001449 RID: 5193
		private Control _rightThoughtAnchor;

		// Token: 0x0400144A RID: 5194
		private int _characterIndex;

		// Token: 0x0400144B RID: 5195
		[Nullable(2)]
		private NThoughtBubbleVfx _thoughtBubbleVfx;

		// Token: 0x0400144C RID: 5196
		[Nullable(2)]
		private CancellationTokenSource _thoughtBubbleGoAwayCancellation;

		// Token: 0x0400144D RID: 5197
		private CancellationTokenSource _cts = new CancellationTokenSource();

		// Token: 0x0400144E RID: 5198
		[Nullable(2)]
		private Control _selectedOptionConfirmation;

		// Token: 0x0400144F RID: 5199
		[Nullable(2)]
		private RestSiteOption _hoveredRestSiteOption;

		// Token: 0x04001450 RID: 5200
		[Nullable(2)]
		private RestSiteOption _selectingRestSiteOption;

		// Token: 0x04001451 RID: 5201
		[Nullable(2)]
		private RestSiteOption _restSiteOptionInThoughtBubble;

		/// <summary>
		/// Cached StringNames for the methods contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x020016BD RID: 5821
		[NullableContext(0)]
		public class MethodName : Node2D.MethodName
		{
			/// <summary>
			/// Cached name for the '_EnterTree' method.
			/// </summary>
			// Token: 0x040058A1 RID: 22689
			public static readonly StringName _EnterTree = "_EnterTree";

			/// <summary>
			/// Cached name for the '_ExitTree' method.
			/// </summary>
			// Token: 0x040058A2 RID: 22690
			public static readonly StringName _ExitTree = "_ExitTree";

			/// <summary>
			/// Cached name for the '_Ready' method.
			/// </summary>
			// Token: 0x040058A3 RID: 22691
			public static readonly StringName _Ready = "_Ready";

			/// <summary>
			/// Cached name for the 'RandomizeFire' method.
			/// </summary>
			// Token: 0x040058A4 RID: 22692
			public static readonly StringName RandomizeFire = "RandomizeFire";

			/// <summary>
			/// Cached name for the 'OnFocus' method.
			/// </summary>
			// Token: 0x040058A5 RID: 22693
			public static readonly StringName OnFocus = "OnFocus";

			/// <summary>
			/// Cached name for the 'OnUnfocus' method.
			/// </summary>
			// Token: 0x040058A6 RID: 22694
			public static readonly StringName OnUnfocus = "OnUnfocus";

			/// <summary>
			/// Cached name for the 'Deselect' method.
			/// </summary>
			// Token: 0x040058A7 RID: 22695
			public static readonly StringName Deselect = "Deselect";

			/// <summary>
			/// Cached name for the 'FlipX' method.
			/// </summary>
			// Token: 0x040058A8 RID: 22696
			public static readonly StringName FlipX = "FlipX";

			/// <summary>
			/// Cached name for the 'HideFlameGlow' method.
			/// </summary>
			// Token: 0x040058A9 RID: 22697
			public static readonly StringName HideFlameGlow = "HideFlameGlow";

			/// <summary>
			/// Cached name for the 'RefreshThoughtBubbleVfx' method.
			/// </summary>
			// Token: 0x040058AA RID: 22698
			public static readonly StringName RefreshThoughtBubbleVfx = "RefreshThoughtBubbleVfx";

			/// <summary>
			/// Cached name for the 'Shake' method.
			/// </summary>
			// Token: 0x040058AB RID: 22699
			public static readonly StringName Shake = "Shake";

			/// <summary>
			/// Cached name for the 'GetRestSiteOptionAnchor' method.
			/// </summary>
			// Token: 0x040058AC RID: 22700
			public static readonly StringName GetRestSiteOptionAnchor = "GetRestSiteOptionAnchor";
		}

		/// <summary>
		/// Cached StringNames for the properties and fields contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x020016BE RID: 5822
		[NullableContext(0)]
		public class PropertyName : Node2D.PropertyName
		{
			/// <summary>
			/// Cached name for the 'Hitbox' property.
			/// </summary>
			// Token: 0x040058AD RID: 22701
			public static readonly StringName Hitbox = "Hitbox";

			/// <summary>
			/// Cached name for the '_controlRoot' field.
			/// </summary>
			// Token: 0x040058AE RID: 22702
			public static readonly StringName _controlRoot = "_controlRoot";

			/// <summary>
			/// Cached name for the '_selectionReticle' field.
			/// </summary>
			// Token: 0x040058AF RID: 22703
			public static readonly StringName _selectionReticle = "_selectionReticle";

			/// <summary>
			/// Cached name for the '_leftThoughtAnchor' field.
			/// </summary>
			// Token: 0x040058B0 RID: 22704
			public static readonly StringName _leftThoughtAnchor = "_leftThoughtAnchor";

			/// <summary>
			/// Cached name for the '_rightThoughtAnchor' field.
			/// </summary>
			// Token: 0x040058B1 RID: 22705
			public static readonly StringName _rightThoughtAnchor = "_rightThoughtAnchor";

			/// <summary>
			/// Cached name for the '_characterIndex' field.
			/// </summary>
			// Token: 0x040058B2 RID: 22706
			public static readonly StringName _characterIndex = "_characterIndex";

			/// <summary>
			/// Cached name for the '_thoughtBubbleVfx' field.
			/// </summary>
			// Token: 0x040058B3 RID: 22707
			public static readonly StringName _thoughtBubbleVfx = "_thoughtBubbleVfx";

			/// <summary>
			/// Cached name for the '_selectedOptionConfirmation' field.
			/// </summary>
			// Token: 0x040058B4 RID: 22708
			public static readonly StringName _selectedOptionConfirmation = "_selectedOptionConfirmation";
		}

		/// <summary>
		/// Cached StringNames for the signals contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x020016BF RID: 5823
		[NullableContext(0)]
		public class SignalName : Node2D.SignalName
		{
		}
	}
}
