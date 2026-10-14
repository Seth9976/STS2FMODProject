using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using Godot;
using Godot.Bridge;
using Godot.NativeInterop;
using MegaCrit.Sts2.Core.Bindings.MegaSpine;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Helpers;

namespace MegaCrit.Sts2.Core.Nodes.Vfx.Backgrounds
{
	/// <summary>
	/// Manages the <see cref="T:MegaCrit.Sts2.Core.Models.Encounters.KaiserCrabBoss" />'s animations, since its 2 creatures (each of its claws) that share one big
	/// Spine file.
	/// </summary>
	// Token: 0x0200023F RID: 575
	[GlobalClass]
	[ScriptPath("res://src/Core/Nodes/Vfx/Backgrounds/NKaiserCrabBossBackground.cs")]
	public class NKaiserCrabBossBackground : Node2D
	{
		// Token: 0x06001D47 RID: 7495 RVA: 0x000BED88 File Offset: 0x000BCF88
		public override void _ExitTree()
		{
			CancellationTokenSource cts = this._cts;
			if (cts == null)
			{
				return;
			}
			cts.Cancel();
		}

		// Token: 0x06001D48 RID: 7496 RVA: 0x000BED9C File Offset: 0x000BCF9C
		public override void _Ready()
		{
			Node2D node = base.GetNode<Node2D>("%Visuals");
			this._animController = new MegaSprite(node);
			this._leftArm = base.GetNode<Node2D>("%ArmBoneL");
			this._rightArm = base.GetNode<Node2D>("%ArmBoneR");
			this.RunWhenSpineReady(this._animController, delegate(MegaAnimationState state)
			{
				state.SetAnimation("right/idle_loop", true, 2);
				state.SetAnimation("body/idle_loop", true, 0);
				state.SetAnimation("left/idle_loop", true, 1);
			});
			base.SetVisible(false);
		}

		// Token: 0x06001D49 RID: 7497 RVA: 0x000BEE2C File Offset: 0x000BD02C
		[NullableContext(1)]
		public async Task PlayAttackAnim(NKaiserCrabBossBackground.ArmSide side, string animation, float duration)
		{
			this._cts = new CancellationTokenSource();
			string text = side.ToString().ToLowerInvariant();
			MegaAnimationState animationState = this._animController.GetAnimationState();
			animationState.SetAnimation(text + "/" + animation, false, (side == NKaiserCrabBossBackground.ArmSide.Left) ? 1 : 2);
			animationState.SetAnimation("reactions/attack_" + text, false, 3);
			this.AddEmptyReactionAnimation(animationState);
			animationState.AddAnimation(text + "/idle_loop", 0f, true, (side == NKaiserCrabBossBackground.ArmSide.Left) ? 1 : 2);
			await Cmd.Wait(duration, this._cts.Token, false);
		}

		// Token: 0x06001D4A RID: 7498 RVA: 0x000BEE88 File Offset: 0x000BD088
		public void PlayHurtAnim(NKaiserCrabBossBackground.ArmSide side)
		{
			string text = side.ToString().ToLowerInvariant();
			MegaAnimationState animationState = this._animController.GetAnimationState();
			animationState.SetAnimation("reactions/hurt_" + text, false, 3);
			this.AddEmptyReactionAnimation(animationState);
			if (side == NKaiserCrabBossBackground.ArmSide.Left)
			{
				animationState.SetAnimation(text + "/hurt", false, 1);
				animationState.AddAnimation(text + "/idle_loop", 0f, true, 1);
				return;
			}
			switch (this._rightArmState)
			{
			case NKaiserCrabBossBackground.RightArmState.Default:
				animationState.SetAnimation(text + "/hurt", false, 2);
				animationState.AddAnimation("right/idle_loop", 0f, true, 2);
				return;
			case NKaiserCrabBossBackground.RightArmState.Charging:
				animationState.SetAnimation(text + "/hurt_charged", false, 2);
				animationState.AddAnimation("right/charged_loop", 0f, true, 2);
				return;
			case NKaiserCrabBossBackground.RightArmState.Resting:
				animationState.SetAnimation(text + "/hurt_resting", false, 2);
				animationState.AddAnimation("right/rest_loop", 0f, true, 2);
				return;
			default:
				return;
			}
		}

		// Token: 0x06001D4B RID: 7499 RVA: 0x000BEF88 File Offset: 0x000BD188
		public void PlayArmDeathAnim(NKaiserCrabBossBackground.ArmSide side)
		{
			string text = side.ToString().ToLowerInvariant();
			MegaAnimationState animationState = this._animController.GetAnimationState();
			animationState.SetAnimation("reactions/hurt_" + text, false, 3);
			this.AddEmptyReactionAnimation(animationState);
			if (side == NKaiserCrabBossBackground.ArmSide.Left)
			{
				animationState.SetAnimation(text + "/die", false, 1);
				return;
			}
			NKaiserCrabBossBackground.RightArmState rightArmState = this._rightArmState;
			if (rightArmState <= NKaiserCrabBossBackground.RightArmState.Charging)
			{
				animationState.SetAnimation("right/die", false, 2);
				return;
			}
			if (rightArmState != NKaiserCrabBossBackground.RightArmState.Resting)
			{
				return;
			}
			animationState.SetAnimation("right/die_resting", false, 2);
		}

		// Token: 0x06001D4C RID: 7500 RVA: 0x000BF010 File Offset: 0x000BD210
		[NullableContext(1)]
		public async Task PlayRightSideChargeUpAnim(float duration)
		{
			this._cts = new CancellationTokenSource();
			this._rightArmState = NKaiserCrabBossBackground.RightArmState.Charging;
			MegaAnimationState animationState = this._animController.GetAnimationState();
			animationState.SetAnimation("right/charge_up", false, 2);
			animationState.AddAnimation("right/charged_loop", 0f, true, 2);
			animationState.SetAnimation("reactions/attack_right", false, 3);
			this.AddEmptyReactionAnimation(animationState);
			await Cmd.Wait(duration, this._cts.Token, false);
		}

		// Token: 0x06001D4D RID: 7501 RVA: 0x000BF05C File Offset: 0x000BD25C
		[NullableContext(1)]
		public async Task PlayRightSideHeavy(float duration)
		{
			this._cts = new CancellationTokenSource();
			this._rightArmState = NKaiserCrabBossBackground.RightArmState.Resting;
			MegaAnimationState animationState = this._animController.GetAnimationState();
			animationState.SetAnimation("right/attack_heavy", false, 2);
			animationState.AddAnimation("right/rest_loop", 0f, true, 2);
			animationState.SetAnimation("reactions/attack_right", false, 3);
			this.AddEmptyReactionAnimation(animationState);
			await Cmd.Wait(duration, this._cts.Token, false);
		}

		// Token: 0x06001D4E RID: 7502 RVA: 0x000BF0A8 File Offset: 0x000BD2A8
		[NullableContext(1)]
		public async Task PlayRightRecharge(float duration)
		{
			this._cts = new CancellationTokenSource();
			this._rightArmState = NKaiserCrabBossBackground.RightArmState.Default;
			MegaAnimationState animationState = this._animController.GetAnimationState();
			animationState.SetAnimation("right/wake_up", false, 2);
			animationState.AddAnimation("right/idle_loop", 0f, true, 2);
			await Cmd.Wait(duration, this._cts.Token, false);
		}

		// Token: 0x06001D4F RID: 7503 RVA: 0x000BF0F4 File Offset: 0x000BD2F4
		public void PlayBodyDeathAnim()
		{
			MegaAnimationState animationState = this._animController.GetAnimationState();
			animationState.SetAnimation("body/die", false, 0);
		}

		/// <summary>
		/// Resets the reaction track animationafter its current animation is finished
		/// returning control of the rig to other animation tracks
		/// </summary>
		// Token: 0x06001D50 RID: 7504 RVA: 0x000BF11A File Offset: 0x000BD31A
		[NullableContext(1)]
		private void AddEmptyReactionAnimation(MegaAnimationState state)
		{
			state.AddEmptyAnimation(3);
		}

		/// <summary>
		/// Get the method information for all the methods declared in this class.
		/// This method is used by Godot to register the available methods in the editor.
		/// Do not call this method.
		/// </summary>
		// Token: 0x06001D51 RID: 7505 RVA: 0x000BF124 File Offset: 0x000BD324
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal static List<MethodInfo> GetGodotMethodList()
		{
			return new List<MethodInfo>(5)
			{
				new MethodInfo(NKaiserCrabBossBackground.MethodName._ExitTree, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NKaiserCrabBossBackground.MethodName._Ready, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NKaiserCrabBossBackground.MethodName.PlayHurtAnim, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, new List<PropertyInfo>
				{
					new PropertyInfo(2L, "side", 0L, "", 6L, false)
				}, null),
				new MethodInfo(NKaiserCrabBossBackground.MethodName.PlayArmDeathAnim, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, new List<PropertyInfo>
				{
					new PropertyInfo(2L, "side", 0L, "", 6L, false)
				}, null),
				new MethodInfo(NKaiserCrabBossBackground.MethodName.PlayBodyDeathAnim, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null)
			};
		}

		/// <inheritdoc />
		// Token: 0x06001D52 RID: 7506 RVA: 0x000BF270 File Offset: 0x000BD470
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool InvokeGodotClassMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
		{
			if ((ref method) == NKaiserCrabBossBackground.MethodName._ExitTree && args.Count == 0)
			{
				this._ExitTree();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NKaiserCrabBossBackground.MethodName._Ready && args.Count == 0)
			{
				this._Ready();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NKaiserCrabBossBackground.MethodName.PlayHurtAnim && args.Count == 1)
			{
				this.PlayHurtAnim(VariantUtils.ConvertTo<NKaiserCrabBossBackground.ArmSide>(args[0]));
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NKaiserCrabBossBackground.MethodName.PlayArmDeathAnim && args.Count == 1)
			{
				this.PlayArmDeathAnim(VariantUtils.ConvertTo<NKaiserCrabBossBackground.ArmSide>(args[0]));
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NKaiserCrabBossBackground.MethodName.PlayBodyDeathAnim && args.Count == 0)
			{
				this.PlayBodyDeathAnim();
				ret = default(godot_variant);
				return true;
			}
			return base.InvokeGodotClassMethod(ref method, args, ref ret);
		}

		/// <inheritdoc />
		// Token: 0x06001D53 RID: 7507 RVA: 0x000BF35C File Offset: 0x000BD55C
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool HasGodotClassMethod(in godot_string_name method)
		{
			return (ref method) == NKaiserCrabBossBackground.MethodName._ExitTree || (ref method) == NKaiserCrabBossBackground.MethodName._Ready || (ref method) == NKaiserCrabBossBackground.MethodName.PlayHurtAnim || (ref method) == NKaiserCrabBossBackground.MethodName.PlayArmDeathAnim || (ref method) == NKaiserCrabBossBackground.MethodName.PlayBodyDeathAnim || base.HasGodotClassMethod(ref method);
		}

		/// <inheritdoc />
		// Token: 0x06001D54 RID: 7508 RVA: 0x000BF3BC File Offset: 0x000BD5BC
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool SetGodotClassPropertyValue(in godot_string_name name, in godot_variant value)
		{
			if ((ref name) == NKaiserCrabBossBackground.PropertyName._leftArm)
			{
				this._leftArm = VariantUtils.ConvertTo<Node2D>(ref value);
				return true;
			}
			if ((ref name) == NKaiserCrabBossBackground.PropertyName._rightArm)
			{
				this._rightArm = VariantUtils.ConvertTo<Node2D>(ref value);
				return true;
			}
			if ((ref name) == NKaiserCrabBossBackground.PropertyName._rightArmState)
			{
				this._rightArmState = VariantUtils.ConvertTo<NKaiserCrabBossBackground.RightArmState>(ref value);
				return true;
			}
			return base.SetGodotClassPropertyValue(ref name, ref value);
		}

		/// <inheritdoc />
		// Token: 0x06001D55 RID: 7509 RVA: 0x000BF424 File Offset: 0x000BD624
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
		{
			if ((ref name) == NKaiserCrabBossBackground.PropertyName._leftArm)
			{
				value = VariantUtils.CreateFrom<Node2D>(ref this._leftArm);
				return true;
			}
			if ((ref name) == NKaiserCrabBossBackground.PropertyName._rightArm)
			{
				value = VariantUtils.CreateFrom<Node2D>(ref this._rightArm);
				return true;
			}
			if ((ref name) == NKaiserCrabBossBackground.PropertyName._rightArmState)
			{
				value = VariantUtils.CreateFrom<NKaiserCrabBossBackground.RightArmState>(ref this._rightArmState);
				return true;
			}
			return base.GetGodotClassPropertyValue(ref name, ref value);
		}

		/// <summary>
		/// Get the property information for all the properties declared in this class.
		/// This method is used by Godot to register the available properties in the editor.
		/// Do not call this method.
		/// </summary>
		// Token: 0x06001D56 RID: 7510 RVA: 0x000BF49C File Offset: 0x000BD69C
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal static List<PropertyInfo> GetGodotPropertyList()
		{
			return new List<PropertyInfo>
			{
				new PropertyInfo(24L, NKaiserCrabBossBackground.PropertyName._leftArm, 0L, "", 4096L, false),
				new PropertyInfo(24L, NKaiserCrabBossBackground.PropertyName._rightArm, 0L, "", 4096L, false),
				new PropertyInfo(2L, NKaiserCrabBossBackground.PropertyName._rightArmState, 0L, "", 4096L, false)
			};
		}

		/// <inheritdoc />
		// Token: 0x06001D57 RID: 7511 RVA: 0x000BF514 File Offset: 0x000BD714
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void SaveGodotObjectData(GodotSerializationInfo info)
		{
			base.SaveGodotObjectData(info);
			info.AddProperty(NKaiserCrabBossBackground.PropertyName._leftArm, Variant.From<Node2D>(ref this._leftArm));
			info.AddProperty(NKaiserCrabBossBackground.PropertyName._rightArm, Variant.From<Node2D>(ref this._rightArm));
			info.AddProperty(NKaiserCrabBossBackground.PropertyName._rightArmState, Variant.From<NKaiserCrabBossBackground.RightArmState>(ref this._rightArmState));
		}

		/// <inheritdoc />
		// Token: 0x06001D58 RID: 7512 RVA: 0x000BF56C File Offset: 0x000BD76C
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void RestoreGodotObjectData(GodotSerializationInfo info)
		{
			base.RestoreGodotObjectData(info);
			Variant variant;
			if (info.TryGetProperty(NKaiserCrabBossBackground.PropertyName._leftArm, ref variant))
			{
				this._leftArm = variant.As<Node2D>();
			}
			Variant variant2;
			if (info.TryGetProperty(NKaiserCrabBossBackground.PropertyName._rightArm, ref variant2))
			{
				this._rightArm = variant2.As<Node2D>();
			}
			Variant variant3;
			if (info.TryGetProperty(NKaiserCrabBossBackground.PropertyName._rightArmState, ref variant3))
			{
				this._rightArmState = variant3.As<NKaiserCrabBossBackground.RightArmState>();
			}
		}

		// Token: 0x04000B4A RID: 2890
		private const int _bodyTrack = 0;

		// Token: 0x04000B4B RID: 2891
		private const int _leftArmTrack = 1;

		// Token: 0x04000B4C RID: 2892
		private const int _rightArmTrack = 2;

		// Token: 0x04000B4D RID: 2893
		private const int _reactionTrack = 3;

		// Token: 0x04000B4E RID: 2894
		[Nullable(1)]
		private MegaSprite _animController;

		// Token: 0x04000B4F RID: 2895
		[Nullable(1)]
		private Node2D _leftArm;

		// Token: 0x04000B50 RID: 2896
		[Nullable(1)]
		private Node2D _rightArm;

		// Token: 0x04000B51 RID: 2897
		private NKaiserCrabBossBackground.RightArmState _rightArmState;

		// Token: 0x04000B52 RID: 2898
		[Nullable(2)]
		private CancellationTokenSource _cts;

		// Token: 0x0200122B RID: 4651
		private enum RightArmState
		{
			// Token: 0x04004658 RID: 18008
			Default,
			// Token: 0x04004659 RID: 18009
			Charging,
			// Token: 0x0400465A RID: 18010
			Resting
		}

		// Token: 0x0200122C RID: 4652
		public enum ArmSide
		{
			// Token: 0x0400465C RID: 18012
			Left,
			// Token: 0x0400465D RID: 18013
			Right
		}

		/// <summary>
		/// Cached StringNames for the methods contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x0200122D RID: 4653
		public class MethodName : Node2D.MethodName
		{
			/// <summary>
			/// Cached name for the '_ExitTree' method.
			/// </summary>
			// Token: 0x0400465E RID: 18014
			public static readonly StringName _ExitTree = "_ExitTree";

			/// <summary>
			/// Cached name for the '_Ready' method.
			/// </summary>
			// Token: 0x0400465F RID: 18015
			public static readonly StringName _Ready = "_Ready";

			/// <summary>
			/// Cached name for the 'PlayHurtAnim' method.
			/// </summary>
			// Token: 0x04004660 RID: 18016
			public static readonly StringName PlayHurtAnim = "PlayHurtAnim";

			/// <summary>
			/// Cached name for the 'PlayArmDeathAnim' method.
			/// </summary>
			// Token: 0x04004661 RID: 18017
			public static readonly StringName PlayArmDeathAnim = "PlayArmDeathAnim";

			/// <summary>
			/// Cached name for the 'PlayBodyDeathAnim' method.
			/// </summary>
			// Token: 0x04004662 RID: 18018
			public static readonly StringName PlayBodyDeathAnim = "PlayBodyDeathAnim";
		}

		/// <summary>
		/// Cached StringNames for the properties and fields contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x0200122E RID: 4654
		public class PropertyName : Node2D.PropertyName
		{
			/// <summary>
			/// Cached name for the '_leftArm' field.
			/// </summary>
			// Token: 0x04004663 RID: 18019
			public static readonly StringName _leftArm = "_leftArm";

			/// <summary>
			/// Cached name for the '_rightArm' field.
			/// </summary>
			// Token: 0x04004664 RID: 18020
			public static readonly StringName _rightArm = "_rightArm";

			/// <summary>
			/// Cached name for the '_rightArmState' field.
			/// </summary>
			// Token: 0x04004665 RID: 18021
			public static readonly StringName _rightArmState = "_rightArmState";
		}

		/// <summary>
		/// Cached StringNames for the signals contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x0200122F RID: 4655
		public class SignalName : Node2D.SignalName
		{
		}
	}
}
