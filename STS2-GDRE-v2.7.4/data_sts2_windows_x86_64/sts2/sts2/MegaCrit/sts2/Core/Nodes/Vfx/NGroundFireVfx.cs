using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Godot;
using Godot.Bridge;
using Godot.NativeInterop;
using MegaCrit.Sts2.Core.Assets;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.Nodes.Combat;
using MegaCrit.Sts2.Core.Nodes.GodotExtensions;
using MegaCrit.Sts2.Core.Nodes.Rooms;
using MegaCrit.Sts2.Core.TestSupport;

namespace MegaCrit.Sts2.Core.Nodes.Vfx
{
	// Token: 0x020001B7 RID: 439
	[NullableContext(1)]
	[Nullable(0)]
	[ScriptPath("res://src/Core/Nodes/Vfx/NGroundFireVfx.cs")]
	public class NGroundFireVfx : Node2D
	{
		// Token: 0x17000610 RID: 1552
		// (get) Token: 0x060015D3 RID: 5587 RVA: 0x00085316 File Offset: 0x00083516
		public static IEnumerable<string> AssetPaths
		{
			get
			{
				return new <>z__ReadOnlySingleElementList<string>(NGroundFireVfx._scenePath);
			}
		}

		// Token: 0x060015D4 RID: 5588 RVA: 0x00085324 File Offset: 0x00083524
		[return: Nullable(2)]
		public static NGroundFireVfx Create(Creature target, VfxColor color = VfxColor.Red)
		{
			if (TestMode.IsOn)
			{
				return null;
			}
			NCreature creatureNode = NCombatRoom.Instance.GetCreatureNode(target);
			if (creatureNode == null)
			{
				return null;
			}
			NGroundFireVfx ngroundFireVfx = PreloadManager.Cache.GetScene(NGroundFireVfx._scenePath).Instantiate<NGroundFireVfx>(0L);
			ngroundFireVfx._vfxColor = color;
			ngroundFireVfx.GlobalPosition = creatureNode.GetBottomOfHitbox();
			return ngroundFireVfx;
		}

		// Token: 0x060015D5 RID: 5589 RVA: 0x00085378 File Offset: 0x00083578
		public override void _Ready()
		{
			this._mainFire = base.GetNode<Node2D>("MainFire");
			this._ember = base.GetNode<GpuParticles2D>("Ember");
			this._flameSprites = base.GetNode<GpuParticles2D>("FlameSprites");
			this.ApplyColor();
			TaskHelper.RunSafely(this.AnimateIn());
		}

		// Token: 0x060015D6 RID: 5590 RVA: 0x000853DC File Offset: 0x000835DC
		private void ApplyColor()
		{
			if (this._vfxColor == VfxColor.Red)
			{
				return;
			}
			Color white = Colors.White;
			Color white2 = Colors.White;
			Color black;
			black..ctor("541b00");
			switch (this._vfxColor)
			{
			case VfxColor.Green:
				white..ctor("2fa800");
				white2..ctor("06a000");
				black..ctor("541b00");
				goto IL_00BC;
			case VfxColor.Blue:
				white..ctor("0099cd");
				white2..ctor("00a3bf");
				black = Colors.Black;
				goto IL_00BC;
			case VfxColor.Purple:
				white..ctor("7821ff");
				white2..ctor("3f21ff");
				black..ctor("541b00");
				goto IL_00BC;
			case VfxColor.Black:
			case VfxColor.White:
				goto IL_00BC;
			}
			throw new ArgumentOutOfRangeException();
			IL_00BC:
			Node node = this._mainFire.GetNode("VfxAdditiveStepFire");
			ShaderMaterial shaderMaterial = (ShaderMaterial)node.GetNode<Node2D>("SteppedFireMix").Material;
			shaderMaterial.SetShaderParameter(NGroundFireVfx._outerColor, white);
			shaderMaterial.SetShaderParameter(NGroundFireVfx._innerColor, white2);
			shaderMaterial = (ShaderMaterial)node.GetNode<Node2D>("SteppedFireAdd").Material;
			shaderMaterial.SetShaderParameter(NGroundFireVfx._outerColor, black);
		}

		// Token: 0x060015D7 RID: 5591 RVA: 0x00085529 File Offset: 0x00083729
		public override void _ExitTree()
		{
			Tween tween = this._tween;
			if (tween == null)
			{
				return;
			}
			tween.Kill();
		}

		// Token: 0x060015D8 RID: 5592 RVA: 0x0008553C File Offset: 0x0008373C
		private async Task AnimateIn()
		{
			this._mainFire.Modulate = Colors.Transparent;
			this._mainFire.Scale = Vector2.Zero;
			this._ember.Emitting = true;
			Task emberDone = this._ember.AwaitSignal(GpuParticles2D.SignalName.Finished, this);
			this._tween = base.CreateTween().SetParallel(true);
			this._tween.TweenProperty(this._mainFire, "scale", Vector2.One * 4f, 0.5).SetEase(1L).SetTrans(10L);
			this._tween.TweenProperty(this._mainFire, "modulate:a", 0.9f, 0.5).SetEase(1L).SetTrans(7L);
			TaskAwaiter<bool> taskAwaiter = this._tween.AwaitFinished(this).GetAwaiter();
			TaskAwaiter<bool> taskAwaiter2;
			if (!taskAwaiter.IsCompleted)
			{
				await taskAwaiter;
				taskAwaiter = taskAwaiter2;
				taskAwaiter2 = default(TaskAwaiter<bool>);
			}
			if (taskAwaiter.GetResult())
			{
				this._flameSprites.Emitting = true;
				this._tween = base.CreateTween().SetParallel(true);
				this._tween.TweenProperty(this._mainFire, "modulate:a", 0f, 0.5);
				this._tween.TweenProperty(this._flameSprites, "modulate:a", 0f, 0.5);
				this._tween.TweenProperty(this._mainFire, "scale", Vector2.One * 2f, 2.0).SetEase(2L).SetTrans(7L);
				TaskAwaiter<bool> taskAwaiter3 = this._tween.AwaitFinished(this).GetAwaiter();
				if (!taskAwaiter3.IsCompleted)
				{
					await taskAwaiter3;
					taskAwaiter3 = taskAwaiter2;
					taskAwaiter2 = default(TaskAwaiter<bool>);
				}
				if (taskAwaiter3.GetResult())
				{
					this._flameSprites.Emitting = false;
					await emberDone;
					this.QueueFreeSafely();
				}
			}
		}

		/// <summary>
		/// Get the method information for all the methods declared in this class.
		/// This method is used by Godot to register the available methods in the editor.
		/// Do not call this method.
		/// </summary>
		// Token: 0x060015D9 RID: 5593 RVA: 0x00085580 File Offset: 0x00083780
		[NullableContext(0)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal static List<MethodInfo> GetGodotMethodList()
		{
			return new List<MethodInfo>(3)
			{
				new MethodInfo(NGroundFireVfx.MethodName._Ready, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NGroundFireVfx.MethodName.ApplyColor, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NGroundFireVfx.MethodName._ExitTree, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null)
			};
		}

		/// <inheritdoc />
		// Token: 0x060015DA RID: 5594 RVA: 0x00085624 File Offset: 0x00083824
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool InvokeGodotClassMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
		{
			if ((ref method) == NGroundFireVfx.MethodName._Ready && args.Count == 0)
			{
				this._Ready();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NGroundFireVfx.MethodName.ApplyColor && args.Count == 0)
			{
				this.ApplyColor();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NGroundFireVfx.MethodName._ExitTree && args.Count == 0)
			{
				this._ExitTree();
				ret = default(godot_variant);
				return true;
			}
			return base.InvokeGodotClassMethod(ref method, args, ref ret);
		}

		/// <inheritdoc />
		// Token: 0x060015DB RID: 5595 RVA: 0x000856A9 File Offset: 0x000838A9
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool HasGodotClassMethod(in godot_string_name method)
		{
			return (ref method) == NGroundFireVfx.MethodName._Ready || (ref method) == NGroundFireVfx.MethodName.ApplyColor || (ref method) == NGroundFireVfx.MethodName._ExitTree || base.HasGodotClassMethod(ref method);
		}

		/// <inheritdoc />
		// Token: 0x060015DC RID: 5596 RVA: 0x000856E0 File Offset: 0x000838E0
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool SetGodotClassPropertyValue(in godot_string_name name, in godot_variant value)
		{
			if ((ref name) == NGroundFireVfx.PropertyName._tween)
			{
				this._tween = VariantUtils.ConvertTo<Tween>(ref value);
				return true;
			}
			if ((ref name) == NGroundFireVfx.PropertyName._mainFire)
			{
				this._mainFire = VariantUtils.ConvertTo<Node2D>(ref value);
				return true;
			}
			if ((ref name) == NGroundFireVfx.PropertyName._ember)
			{
				this._ember = VariantUtils.ConvertTo<GpuParticles2D>(ref value);
				return true;
			}
			if ((ref name) == NGroundFireVfx.PropertyName._flameSprites)
			{
				this._flameSprites = VariantUtils.ConvertTo<GpuParticles2D>(ref value);
				return true;
			}
			if ((ref name) == NGroundFireVfx.PropertyName._vfxColor)
			{
				this._vfxColor = VariantUtils.ConvertTo<VfxColor>(ref value);
				return true;
			}
			return base.SetGodotClassPropertyValue(ref name, ref value);
		}

		/// <inheritdoc />
		// Token: 0x060015DD RID: 5597 RVA: 0x0008577C File Offset: 0x0008397C
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
		{
			if ((ref name) == NGroundFireVfx.PropertyName._tween)
			{
				value = VariantUtils.CreateFrom<Tween>(ref this._tween);
				return true;
			}
			if ((ref name) == NGroundFireVfx.PropertyName._mainFire)
			{
				value = VariantUtils.CreateFrom<Node2D>(ref this._mainFire);
				return true;
			}
			if ((ref name) == NGroundFireVfx.PropertyName._ember)
			{
				value = VariantUtils.CreateFrom<GpuParticles2D>(ref this._ember);
				return true;
			}
			if ((ref name) == NGroundFireVfx.PropertyName._flameSprites)
			{
				value = VariantUtils.CreateFrom<GpuParticles2D>(ref this._flameSprites);
				return true;
			}
			if ((ref name) == NGroundFireVfx.PropertyName._vfxColor)
			{
				value = VariantUtils.CreateFrom<VfxColor>(ref this._vfxColor);
				return true;
			}
			return base.GetGodotClassPropertyValue(ref name, ref value);
		}

		/// <summary>
		/// Get the property information for all the properties declared in this class.
		/// This method is used by Godot to register the available properties in the editor.
		/// Do not call this method.
		/// </summary>
		// Token: 0x060015DE RID: 5598 RVA: 0x00085834 File Offset: 0x00083A34
		[NullableContext(0)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal static List<PropertyInfo> GetGodotPropertyList()
		{
			return new List<PropertyInfo>
			{
				new PropertyInfo(24L, NGroundFireVfx.PropertyName._tween, 0L, "", 4096L, false),
				new PropertyInfo(24L, NGroundFireVfx.PropertyName._mainFire, 0L, "", 4096L, false),
				new PropertyInfo(24L, NGroundFireVfx.PropertyName._ember, 0L, "", 4096L, false),
				new PropertyInfo(24L, NGroundFireVfx.PropertyName._flameSprites, 0L, "", 4096L, false),
				new PropertyInfo(2L, NGroundFireVfx.PropertyName._vfxColor, 0L, "", 4096L, false)
			};
		}

		/// <inheritdoc />
		// Token: 0x060015DF RID: 5599 RVA: 0x000858EC File Offset: 0x00083AEC
		[NullableContext(0)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void SaveGodotObjectData(GodotSerializationInfo info)
		{
			base.SaveGodotObjectData(info);
			info.AddProperty(NGroundFireVfx.PropertyName._tween, Variant.From<Tween>(ref this._tween));
			info.AddProperty(NGroundFireVfx.PropertyName._mainFire, Variant.From<Node2D>(ref this._mainFire));
			info.AddProperty(NGroundFireVfx.PropertyName._ember, Variant.From<GpuParticles2D>(ref this._ember));
			info.AddProperty(NGroundFireVfx.PropertyName._flameSprites, Variant.From<GpuParticles2D>(ref this._flameSprites));
			info.AddProperty(NGroundFireVfx.PropertyName._vfxColor, Variant.From<VfxColor>(ref this._vfxColor));
		}

		/// <inheritdoc />
		// Token: 0x060015E0 RID: 5600 RVA: 0x00085970 File Offset: 0x00083B70
		[NullableContext(0)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void RestoreGodotObjectData(GodotSerializationInfo info)
		{
			base.RestoreGodotObjectData(info);
			Variant variant;
			if (info.TryGetProperty(NGroundFireVfx.PropertyName._tween, ref variant))
			{
				this._tween = variant.As<Tween>();
			}
			Variant variant2;
			if (info.TryGetProperty(NGroundFireVfx.PropertyName._mainFire, ref variant2))
			{
				this._mainFire = variant2.As<Node2D>();
			}
			Variant variant3;
			if (info.TryGetProperty(NGroundFireVfx.PropertyName._ember, ref variant3))
			{
				this._ember = variant3.As<GpuParticles2D>();
			}
			Variant variant4;
			if (info.TryGetProperty(NGroundFireVfx.PropertyName._flameSprites, ref variant4))
			{
				this._flameSprites = variant4.As<GpuParticles2D>();
			}
			Variant variant5;
			if (info.TryGetProperty(NGroundFireVfx.PropertyName._vfxColor, ref variant5))
			{
				this._vfxColor = variant5.As<VfxColor>();
			}
		}

		// Token: 0x040007C0 RID: 1984
		private static readonly StringName _outerColor = new StringName("OuterColor");

		// Token: 0x040007C1 RID: 1985
		private static readonly StringName _innerColor = new StringName("InnerColor");

		// Token: 0x040007C2 RID: 1986
		[Nullable(2)]
		private Tween _tween;

		// Token: 0x040007C3 RID: 1987
		private static readonly string _scenePath = SceneHelper.GetScenePath("vfx/fires/vfx_ground_fire");

		// Token: 0x040007C4 RID: 1988
		private Node2D _mainFire;

		// Token: 0x040007C5 RID: 1989
		private GpuParticles2D _ember;

		// Token: 0x040007C6 RID: 1990
		private GpuParticles2D _flameSprites;

		// Token: 0x040007C7 RID: 1991
		private VfxColor _vfxColor;

		/// <summary>
		/// Cached StringNames for the methods contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x0200105A RID: 4186
		[NullableContext(0)]
		public class MethodName : Node2D.MethodName
		{
			/// <summary>
			/// Cached name for the '_Ready' method.
			/// </summary>
			// Token: 0x04004072 RID: 16498
			public static readonly StringName _Ready = "_Ready";

			/// <summary>
			/// Cached name for the 'ApplyColor' method.
			/// </summary>
			// Token: 0x04004073 RID: 16499
			public static readonly StringName ApplyColor = "ApplyColor";

			/// <summary>
			/// Cached name for the '_ExitTree' method.
			/// </summary>
			// Token: 0x04004074 RID: 16500
			public static readonly StringName _ExitTree = "_ExitTree";
		}

		/// <summary>
		/// Cached StringNames for the properties and fields contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x0200105B RID: 4187
		[NullableContext(0)]
		public class PropertyName : Node2D.PropertyName
		{
			/// <summary>
			/// Cached name for the '_tween' field.
			/// </summary>
			// Token: 0x04004075 RID: 16501
			public static readonly StringName _tween = "_tween";

			/// <summary>
			/// Cached name for the '_mainFire' field.
			/// </summary>
			// Token: 0x04004076 RID: 16502
			public static readonly StringName _mainFire = "_mainFire";

			/// <summary>
			/// Cached name for the '_ember' field.
			/// </summary>
			// Token: 0x04004077 RID: 16503
			public static readonly StringName _ember = "_ember";

			/// <summary>
			/// Cached name for the '_flameSprites' field.
			/// </summary>
			// Token: 0x04004078 RID: 16504
			public static readonly StringName _flameSprites = "_flameSprites";

			/// <summary>
			/// Cached name for the '_vfxColor' field.
			/// </summary>
			// Token: 0x04004079 RID: 16505
			public static readonly StringName _vfxColor = "_vfxColor";
		}

		/// <summary>
		/// Cached StringNames for the signals contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x0200105C RID: 4188
		[NullableContext(0)]
		public class SignalName : Node2D.SignalName
		{
		}
	}
}
