using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Godot;
using Godot.Bridge;
using Godot.NativeInterop;
using MegaCrit.Sts2.Core.Bindings.MegaSpine;

namespace MegaCrit.Sts2.Core.Nodes.Vfx
{
	// Token: 0x020001A2 RID: 418
	[GlobalClass]
	[ScriptPath("res://src/Core/Nodes/Vfx/NEntomancerVfx.cs")]
	public class NEntomancerVfx : Node
	{
		// Token: 0x060014AB RID: 5291 RVA: 0x0007E03C File Offset: 0x0007C23C
		public override void _Ready()
		{
			this._parent = base.GetParent<Node2D>();
			this._animController = new MegaSprite(this._parent);
			this._animController.ConnectAnimationEvent(Callable.From<GodotObject, GodotObject, GodotObject, GodotObject>(new Action<GodotObject, GodotObject, GodotObject, GodotObject>(this.OnAnimationEvent)));
			this._swarmParticles = this._parent.GetNode<GpuParticles2D>("SwarmParticles");
			this._attackingBugParticles = this._parent.GetNode<GpuParticles2D>("SwarmParticles/AttackingBugParticles");
			this._swarmTargetNode = this._parent.GetNode<Node2D>("SwarmTargetNode");
			this._basePosition = this._swarmParticles.Position;
			this._attackingBugParticles.Emitting = false;
		}

		// Token: 0x060014AC RID: 5292 RVA: 0x0007E0F8 File Offset: 0x0007C2F8
		private void LaunchSwarm()
		{
			this._swarming = true;
			this._swarmTween = base.CreateTween();
			this._attackingBugParticles.Emitting = true;
			this._swarmTween.SetEase(1L).SetTrans(7L);
			this._swarmTween.TweenProperty(this._swarmParticles, "position", this._swarmTargetNode.Position, 1.0);
			this._swarmTween.SetEase(2L).SetTrans(7L);
			this._swarmTween.TweenProperty(this._swarmParticles, "position", this._basePosition, 1.5).SetDelay(0.0);
			this._swarmTween.TweenCallback(Callable.From(new Action(this.CompleteSwarmAttack))).SetDelay(0.009999999776482582);
		}

		// Token: 0x060014AD RID: 5293 RVA: 0x0007E1EC File Offset: 0x0007C3EC
		private void CompleteSwarmAttack()
		{
			this._attackingBugParticles.Emitting = false;
			this._swarming = false;
		}

		// Token: 0x060014AE RID: 5294 RVA: 0x0007E201 File Offset: 0x0007C401
		private void CancelSwarmAttack()
		{
			if (this._swarming)
			{
				this._swarmTween.Kill();
				this._swarmParticles.Position = this._basePosition;
				this.CompleteSwarmAttack();
			}
		}

		// Token: 0x060014AF RID: 5295 RVA: 0x0007E22D File Offset: 0x0007C42D
		private void TurnOffSwarm()
		{
			this._swarmParticles.Emitting = false;
		}

		// Token: 0x060014B0 RID: 5296 RVA: 0x0007E23C File Offset: 0x0007C43C
		[NullableContext(1)]
		private void OnAnimationEvent(GodotObject _, GodotObject __, GodotObject ___, GodotObject spineEvent)
		{
			string eventName = new MegaEvent(spineEvent).GetData().GetEventName();
			if (eventName == "launch_swarm")
			{
				this.LaunchSwarm();
				return;
			}
			if (!(eventName == "turn_off_swarm"))
			{
				return;
			}
			this.TurnOffSwarm();
		}

		// Token: 0x060014B1 RID: 5297 RVA: 0x0007E288 File Offset: 0x0007C488
		public override void _ExitTree()
		{
			Tween swarmTween = this._swarmTween;
			if (swarmTween == null)
			{
				return;
			}
			swarmTween.Kill();
		}

		/// <summary>
		/// Get the method information for all the methods declared in this class.
		/// This method is used by Godot to register the available methods in the editor.
		/// Do not call this method.
		/// </summary>
		// Token: 0x060014B2 RID: 5298 RVA: 0x0007E29C File Offset: 0x0007C49C
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal static List<MethodInfo> GetGodotMethodList()
		{
			return new List<MethodInfo>(7)
			{
				new MethodInfo(NEntomancerVfx.MethodName._Ready, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NEntomancerVfx.MethodName.LaunchSwarm, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NEntomancerVfx.MethodName.CompleteSwarmAttack, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NEntomancerVfx.MethodName.CancelSwarmAttack, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NEntomancerVfx.MethodName.TurnOffSwarm, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NEntomancerVfx.MethodName.OnAnimationEvent, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, new List<PropertyInfo>
				{
					new PropertyInfo(24L, "_", 0L, "", 6L, new StringName("Object"), false),
					new PropertyInfo(24L, "__", 0L, "", 6L, new StringName("Object"), false),
					new PropertyInfo(24L, "___", 0L, "", 6L, new StringName("Object"), false),
					new PropertyInfo(24L, "spineEvent", 0L, "", 6L, new StringName("Object"), false)
				}, null),
				new MethodInfo(NEntomancerVfx.MethodName._ExitTree, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null)
			};
		}

		/// <inheritdoc />
		// Token: 0x060014B3 RID: 5299 RVA: 0x0007E4B0 File Offset: 0x0007C6B0
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool InvokeGodotClassMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
		{
			if ((ref method) == NEntomancerVfx.MethodName._Ready && args.Count == 0)
			{
				this._Ready();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NEntomancerVfx.MethodName.LaunchSwarm && args.Count == 0)
			{
				this.LaunchSwarm();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NEntomancerVfx.MethodName.CompleteSwarmAttack && args.Count == 0)
			{
				this.CompleteSwarmAttack();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NEntomancerVfx.MethodName.CancelSwarmAttack && args.Count == 0)
			{
				this.CancelSwarmAttack();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NEntomancerVfx.MethodName.TurnOffSwarm && args.Count == 0)
			{
				this.TurnOffSwarm();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NEntomancerVfx.MethodName.OnAnimationEvent && args.Count == 4)
			{
				this.OnAnimationEvent(VariantUtils.ConvertTo<GodotObject>(args[0]), VariantUtils.ConvertTo<GodotObject>(args[1]), VariantUtils.ConvertTo<GodotObject>(args[2]), VariantUtils.ConvertTo<GodotObject>(args[3]));
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NEntomancerVfx.MethodName._ExitTree && args.Count == 0)
			{
				this._ExitTree();
				ret = default(godot_variant);
				return true;
			}
			return base.InvokeGodotClassMethod(ref method, args, ref ret);
		}

		/// <inheritdoc />
		// Token: 0x060014B4 RID: 5300 RVA: 0x0007E600 File Offset: 0x0007C800
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool HasGodotClassMethod(in godot_string_name method)
		{
			return (ref method) == NEntomancerVfx.MethodName._Ready || (ref method) == NEntomancerVfx.MethodName.LaunchSwarm || (ref method) == NEntomancerVfx.MethodName.CompleteSwarmAttack || (ref method) == NEntomancerVfx.MethodName.CancelSwarmAttack || (ref method) == NEntomancerVfx.MethodName.TurnOffSwarm || (ref method) == NEntomancerVfx.MethodName.OnAnimationEvent || (ref method) == NEntomancerVfx.MethodName._ExitTree || base.HasGodotClassMethod(ref method);
		}

		/// <inheritdoc />
		// Token: 0x060014B5 RID: 5301 RVA: 0x0007E680 File Offset: 0x0007C880
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool SetGodotClassPropertyValue(in godot_string_name name, in godot_variant value)
		{
			if ((ref name) == NEntomancerVfx.PropertyName._swarmParticles)
			{
				this._swarmParticles = VariantUtils.ConvertTo<GpuParticles2D>(ref value);
				return true;
			}
			if ((ref name) == NEntomancerVfx.PropertyName._attackingBugParticles)
			{
				this._attackingBugParticles = VariantUtils.ConvertTo<GpuParticles2D>(ref value);
				return true;
			}
			if ((ref name) == NEntomancerVfx.PropertyName._swarmTargetNode)
			{
				this._swarmTargetNode = VariantUtils.ConvertTo<Node2D>(ref value);
				return true;
			}
			if ((ref name) == NEntomancerVfx.PropertyName._basePosition)
			{
				this._basePosition = VariantUtils.ConvertTo<Vector2>(ref value);
				return true;
			}
			if ((ref name) == NEntomancerVfx.PropertyName._swarmTween)
			{
				this._swarmTween = VariantUtils.ConvertTo<Tween>(ref value);
				return true;
			}
			if ((ref name) == NEntomancerVfx.PropertyName._swarming)
			{
				this._swarming = VariantUtils.ConvertTo<bool>(ref value);
				return true;
			}
			if ((ref name) == NEntomancerVfx.PropertyName._parent)
			{
				this._parent = VariantUtils.ConvertTo<Node2D>(ref value);
				return true;
			}
			return base.SetGodotClassPropertyValue(ref name, ref value);
		}

		/// <inheritdoc />
		// Token: 0x060014B6 RID: 5302 RVA: 0x0007E754 File Offset: 0x0007C954
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
		{
			if ((ref name) == NEntomancerVfx.PropertyName._swarmParticles)
			{
				value = VariantUtils.CreateFrom<GpuParticles2D>(ref this._swarmParticles);
				return true;
			}
			if ((ref name) == NEntomancerVfx.PropertyName._attackingBugParticles)
			{
				value = VariantUtils.CreateFrom<GpuParticles2D>(ref this._attackingBugParticles);
				return true;
			}
			if ((ref name) == NEntomancerVfx.PropertyName._swarmTargetNode)
			{
				value = VariantUtils.CreateFrom<Node2D>(ref this._swarmTargetNode);
				return true;
			}
			if ((ref name) == NEntomancerVfx.PropertyName._basePosition)
			{
				value = VariantUtils.CreateFrom<Vector2>(ref this._basePosition);
				return true;
			}
			if ((ref name) == NEntomancerVfx.PropertyName._swarmTween)
			{
				value = VariantUtils.CreateFrom<Tween>(ref this._swarmTween);
				return true;
			}
			if ((ref name) == NEntomancerVfx.PropertyName._swarming)
			{
				value = VariantUtils.CreateFrom<bool>(ref this._swarming);
				return true;
			}
			if ((ref name) == NEntomancerVfx.PropertyName._parent)
			{
				value = VariantUtils.CreateFrom<Node2D>(ref this._parent);
				return true;
			}
			return base.GetGodotClassPropertyValue(ref name, ref value);
		}

		/// <summary>
		/// Get the property information for all the properties declared in this class.
		/// This method is used by Godot to register the available properties in the editor.
		/// Do not call this method.
		/// </summary>
		// Token: 0x060014B7 RID: 5303 RVA: 0x0007E84C File Offset: 0x0007CA4C
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal static List<PropertyInfo> GetGodotPropertyList()
		{
			return new List<PropertyInfo>
			{
				new PropertyInfo(24L, NEntomancerVfx.PropertyName._swarmParticles, 0L, "", 4096L, false),
				new PropertyInfo(24L, NEntomancerVfx.PropertyName._attackingBugParticles, 0L, "", 4096L, false),
				new PropertyInfo(24L, NEntomancerVfx.PropertyName._swarmTargetNode, 0L, "", 4096L, false),
				new PropertyInfo(5L, NEntomancerVfx.PropertyName._basePosition, 0L, "", 4096L, false),
				new PropertyInfo(24L, NEntomancerVfx.PropertyName._swarmTween, 0L, "", 4096L, false),
				new PropertyInfo(1L, NEntomancerVfx.PropertyName._swarming, 0L, "", 4096L, false),
				new PropertyInfo(24L, NEntomancerVfx.PropertyName._parent, 0L, "", 4096L, false)
			};
		}

		/// <inheritdoc />
		// Token: 0x060014B8 RID: 5304 RVA: 0x0007E948 File Offset: 0x0007CB48
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void SaveGodotObjectData(GodotSerializationInfo info)
		{
			base.SaveGodotObjectData(info);
			info.AddProperty(NEntomancerVfx.PropertyName._swarmParticles, Variant.From<GpuParticles2D>(ref this._swarmParticles));
			info.AddProperty(NEntomancerVfx.PropertyName._attackingBugParticles, Variant.From<GpuParticles2D>(ref this._attackingBugParticles));
			info.AddProperty(NEntomancerVfx.PropertyName._swarmTargetNode, Variant.From<Node2D>(ref this._swarmTargetNode));
			info.AddProperty(NEntomancerVfx.PropertyName._basePosition, Variant.From<Vector2>(ref this._basePosition));
			info.AddProperty(NEntomancerVfx.PropertyName._swarmTween, Variant.From<Tween>(ref this._swarmTween));
			info.AddProperty(NEntomancerVfx.PropertyName._swarming, Variant.From<bool>(ref this._swarming));
			info.AddProperty(NEntomancerVfx.PropertyName._parent, Variant.From<Node2D>(ref this._parent));
		}

		/// <inheritdoc />
		// Token: 0x060014B9 RID: 5305 RVA: 0x0007E9F8 File Offset: 0x0007CBF8
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void RestoreGodotObjectData(GodotSerializationInfo info)
		{
			base.RestoreGodotObjectData(info);
			Variant variant;
			if (info.TryGetProperty(NEntomancerVfx.PropertyName._swarmParticles, ref variant))
			{
				this._swarmParticles = variant.As<GpuParticles2D>();
			}
			Variant variant2;
			if (info.TryGetProperty(NEntomancerVfx.PropertyName._attackingBugParticles, ref variant2))
			{
				this._attackingBugParticles = variant2.As<GpuParticles2D>();
			}
			Variant variant3;
			if (info.TryGetProperty(NEntomancerVfx.PropertyName._swarmTargetNode, ref variant3))
			{
				this._swarmTargetNode = variant3.As<Node2D>();
			}
			Variant variant4;
			if (info.TryGetProperty(NEntomancerVfx.PropertyName._basePosition, ref variant4))
			{
				this._basePosition = variant4.As<Vector2>();
			}
			Variant variant5;
			if (info.TryGetProperty(NEntomancerVfx.PropertyName._swarmTween, ref variant5))
			{
				this._swarmTween = variant5.As<Tween>();
			}
			Variant variant6;
			if (info.TryGetProperty(NEntomancerVfx.PropertyName._swarming, ref variant6))
			{
				this._swarming = variant6.As<bool>();
			}
			Variant variant7;
			if (info.TryGetProperty(NEntomancerVfx.PropertyName._parent, ref variant7))
			{
				this._parent = variant7.As<Node2D>();
			}
		}

		// Token: 0x0400076C RID: 1900
		[Nullable(1)]
		private GpuParticles2D _swarmParticles;

		// Token: 0x0400076D RID: 1901
		[Nullable(1)]
		private GpuParticles2D _attackingBugParticles;

		// Token: 0x0400076E RID: 1902
		[Nullable(1)]
		private Node2D _swarmTargetNode;

		// Token: 0x0400076F RID: 1903
		private Vector2 _basePosition;

		// Token: 0x04000770 RID: 1904
		[Nullable(2)]
		private Tween _swarmTween;

		// Token: 0x04000771 RID: 1905
		private bool _swarming;

		// Token: 0x04000772 RID: 1906
		[Nullable(1)]
		private Node2D _parent;

		// Token: 0x04000773 RID: 1907
		[Nullable(1)]
		private MegaSprite _animController;

		/// <summary>
		/// Cached StringNames for the methods contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x02001012 RID: 4114
		public class MethodName : Node.MethodName
		{
			/// <summary>
			/// Cached name for the '_Ready' method.
			/// </summary>
			// Token: 0x04003FD3 RID: 16339
			public static readonly StringName _Ready = "_Ready";

			/// <summary>
			/// Cached name for the 'LaunchSwarm' method.
			/// </summary>
			// Token: 0x04003FD4 RID: 16340
			public static readonly StringName LaunchSwarm = "LaunchSwarm";

			/// <summary>
			/// Cached name for the 'CompleteSwarmAttack' method.
			/// </summary>
			// Token: 0x04003FD5 RID: 16341
			public static readonly StringName CompleteSwarmAttack = "CompleteSwarmAttack";

			/// <summary>
			/// Cached name for the 'CancelSwarmAttack' method.
			/// </summary>
			// Token: 0x04003FD6 RID: 16342
			public static readonly StringName CancelSwarmAttack = "CancelSwarmAttack";

			/// <summary>
			/// Cached name for the 'TurnOffSwarm' method.
			/// </summary>
			// Token: 0x04003FD7 RID: 16343
			public static readonly StringName TurnOffSwarm = "TurnOffSwarm";

			/// <summary>
			/// Cached name for the 'OnAnimationEvent' method.
			/// </summary>
			// Token: 0x04003FD8 RID: 16344
			public static readonly StringName OnAnimationEvent = "OnAnimationEvent";

			/// <summary>
			/// Cached name for the '_ExitTree' method.
			/// </summary>
			// Token: 0x04003FD9 RID: 16345
			public static readonly StringName _ExitTree = "_ExitTree";
		}

		/// <summary>
		/// Cached StringNames for the properties and fields contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x02001013 RID: 4115
		public class PropertyName : Node.PropertyName
		{
			/// <summary>
			/// Cached name for the '_swarmParticles' field.
			/// </summary>
			// Token: 0x04003FDA RID: 16346
			public static readonly StringName _swarmParticles = "_swarmParticles";

			/// <summary>
			/// Cached name for the '_attackingBugParticles' field.
			/// </summary>
			// Token: 0x04003FDB RID: 16347
			public static readonly StringName _attackingBugParticles = "_attackingBugParticles";

			/// <summary>
			/// Cached name for the '_swarmTargetNode' field.
			/// </summary>
			// Token: 0x04003FDC RID: 16348
			public static readonly StringName _swarmTargetNode = "_swarmTargetNode";

			/// <summary>
			/// Cached name for the '_basePosition' field.
			/// </summary>
			// Token: 0x04003FDD RID: 16349
			public static readonly StringName _basePosition = "_basePosition";

			/// <summary>
			/// Cached name for the '_swarmTween' field.
			/// </summary>
			// Token: 0x04003FDE RID: 16350
			public static readonly StringName _swarmTween = "_swarmTween";

			/// <summary>
			/// Cached name for the '_swarming' field.
			/// </summary>
			// Token: 0x04003FDF RID: 16351
			public static readonly StringName _swarming = "_swarming";

			/// <summary>
			/// Cached name for the '_parent' field.
			/// </summary>
			// Token: 0x04003FE0 RID: 16352
			public static readonly StringName _parent = "_parent";
		}

		/// <summary>
		/// Cached StringNames for the signals contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x02001014 RID: 4116
		public class SignalName : Node.SignalName
		{
		}
	}
}
