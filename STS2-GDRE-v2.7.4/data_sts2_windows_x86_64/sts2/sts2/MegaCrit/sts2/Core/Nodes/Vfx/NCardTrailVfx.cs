using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Godot;
using Godot.Bridge;
using Godot.NativeInterop;
using MegaCrit.Sts2.Core.Assets;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.Nodes.Combat;
using MegaCrit.Sts2.Core.Nodes.GodotExtensions;
using MegaCrit.Sts2.Core.TestSupport;

namespace MegaCrit.Sts2.Core.Nodes.Vfx
{
	/// <summary>
	/// Holds both the Sprites (the card image + particle effects) and the Trails for the "card trail vfx".
	/// We need these detached so that the Trail nodes' parents don't affect the trail's positions.
	/// </summary>
	// Token: 0x02000192 RID: 402
	[ScriptPath("res://src/Core/Nodes/Vfx/NCardTrailVfx.cs")]
	public class NCardTrailVfx : Node2D
	{
		// Token: 0x060013A8 RID: 5032 RVA: 0x00076A0C File Offset: 0x00074C0C
		[NullableContext(1)]
		[return: Nullable(2)]
		public static NCardTrailVfx Create(Control card, string characterTrailPath)
		{
			if (TestMode.IsOn)
			{
				return null;
			}
			NCardTrailVfx ncardTrailVfx = PreloadManager.Cache.GetScene(characterTrailPath).Instantiate<NCardTrailVfx>(0L);
			ncardTrailVfx._nodeToFollow = card;
			return ncardTrailVfx;
		}

		// Token: 0x060013A9 RID: 5033 RVA: 0x00076A40 File Offset: 0x00074C40
		public override void _Ready()
		{
			if (NCombatUi.IsDebugHidingPlayContainer)
			{
				base.Visible = false;
			}
			this._sprites = base.GetNode<Node2D>("Sprites");
			this._sprites.Modulate = StsColors.transparentWhite;
			this._tween = base.CreateTween().SetParallel(true);
			this._tween.TweenProperty(this._sprites, "scale", Vector2.One * 0.5f, 0.5).SetEase(0L).SetTrans(7L)
				.SetDelay(0.25);
			this._tween.TweenProperty(this._nodeToFollow, "modulate:a", 0.75f, 0.5).SetEase(1L).SetTrans(7L);
			this._tween.TweenProperty(this._sprites, "modulate:a", 1f, 1.0).SetEase(1L).SetTrans(7L);
		}

		// Token: 0x060013AA RID: 5034 RVA: 0x00076B62 File Offset: 0x00074D62
		public override void _Process(double delta)
		{
			if (!this._updateSprites)
			{
				return;
			}
			base.GlobalPosition = this._nodeToFollow.GlobalPosition;
			base.Rotation = this._nodeToFollow.Rotation;
		}

		// Token: 0x060013AB RID: 5035 RVA: 0x00076B90 File Offset: 0x00074D90
		[NullableContext(1)]
		public async Task FadeOut()
		{
			this._updateSprites = false;
			this._tween = base.CreateTween().SetParallel(true);
			this._tween.TweenProperty(this, "modulate:a", 0f, 0.5);
			this.StopParticles(this._tween);
			bool flag = await this._tween.AwaitFinished(this);
			this.QueueFreeSafely();
		}

		/// <summary>
		/// Prevents the trail from emitting a ton of sparks upon reaching destination.
		/// </summary>
		// Token: 0x060013AC RID: 5036 RVA: 0x00076BD4 File Offset: 0x00074DD4
		[NullableContext(1)]
		private void StopParticles(Tween tween)
		{
			foreach (Node node in this._sprites.GetChildren(false))
			{
				CpuParticles2D cpuParticles2D = node as CpuParticles2D;
				if (cpuParticles2D != null)
				{
					tween.TweenProperty(cpuParticles2D, "amount", 1, 0.5);
				}
			}
		}

		// Token: 0x060013AD RID: 5037 RVA: 0x00076C4C File Offset: 0x00074E4C
		public override void _ExitTree()
		{
			Tween tween = this._tween;
			if (tween == null)
			{
				return;
			}
			tween.Kill();
		}

		/// <summary>
		/// Get the method information for all the methods declared in this class.
		/// This method is used by Godot to register the available methods in the editor.
		/// Do not call this method.
		/// </summary>
		// Token: 0x060013AE RID: 5038 RVA: 0x00076C60 File Offset: 0x00074E60
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal static List<MethodInfo> GetGodotMethodList()
		{
			return new List<MethodInfo>(5)
			{
				new MethodInfo(NCardTrailVfx.MethodName.Create, new PropertyInfo(24L, "", 0L, "", 6L, new StringName("Node2D"), false), 33L, new List<PropertyInfo>
				{
					new PropertyInfo(24L, "card", 0L, "", 6L, new StringName("Control"), false),
					new PropertyInfo(4L, "characterTrailPath", 0L, "", 6L, false)
				}, null),
				new MethodInfo(NCardTrailVfx.MethodName._Ready, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NCardTrailVfx.MethodName._Process, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, new List<PropertyInfo>
				{
					new PropertyInfo(3L, "delta", 0L, "", 6L, false)
				}, null),
				new MethodInfo(NCardTrailVfx.MethodName.StopParticles, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, new List<PropertyInfo>
				{
					new PropertyInfo(24L, "tween", 0L, "", 6L, new StringName("Tween"), false)
				}, null),
				new MethodInfo(NCardTrailVfx.MethodName._ExitTree, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null)
			};
		}

		/// <inheritdoc />
		// Token: 0x060013AF RID: 5039 RVA: 0x00076E14 File Offset: 0x00075014
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool InvokeGodotClassMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
		{
			if ((ref method) == NCardTrailVfx.MethodName.Create && args.Count == 2)
			{
				NCardTrailVfx ncardTrailVfx = NCardTrailVfx.Create(VariantUtils.ConvertTo<Control>(args[0]), VariantUtils.ConvertTo<string>(args[1]));
				ret = VariantUtils.CreateFrom<NCardTrailVfx>(ref ncardTrailVfx);
				return true;
			}
			if ((ref method) == NCardTrailVfx.MethodName._Ready && args.Count == 0)
			{
				this._Ready();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NCardTrailVfx.MethodName._Process && args.Count == 1)
			{
				this._Process(VariantUtils.ConvertTo<double>(args[0]));
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NCardTrailVfx.MethodName.StopParticles && args.Count == 1)
			{
				this.StopParticles(VariantUtils.ConvertTo<Tween>(args[0]));
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NCardTrailVfx.MethodName._ExitTree && args.Count == 0)
			{
				this._ExitTree();
				ret = default(godot_variant);
				return true;
			}
			return base.InvokeGodotClassMethod(ref method, args, ref ret);
		}

		// Token: 0x060013B0 RID: 5040 RVA: 0x00076F20 File Offset: 0x00075120
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal static bool InvokeGodotClassStaticMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
		{
			if ((ref method) == NCardTrailVfx.MethodName.Create && args.Count == 2)
			{
				NCardTrailVfx ncardTrailVfx = NCardTrailVfx.Create(VariantUtils.ConvertTo<Control>(args[0]), VariantUtils.ConvertTo<string>(args[1]));
				ret = VariantUtils.CreateFrom<NCardTrailVfx>(ref ncardTrailVfx);
				return true;
			}
			ret = default(godot_variant);
			return false;
		}

		/// <inheritdoc />
		// Token: 0x060013B1 RID: 5041 RVA: 0x00076F7C File Offset: 0x0007517C
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool HasGodotClassMethod(in godot_string_name method)
		{
			return (ref method) == NCardTrailVfx.MethodName.Create || (ref method) == NCardTrailVfx.MethodName._Ready || (ref method) == NCardTrailVfx.MethodName._Process || (ref method) == NCardTrailVfx.MethodName.StopParticles || (ref method) == NCardTrailVfx.MethodName._ExitTree || base.HasGodotClassMethod(ref method);
		}

		/// <inheritdoc />
		// Token: 0x060013B2 RID: 5042 RVA: 0x00076FDC File Offset: 0x000751DC
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool SetGodotClassPropertyValue(in godot_string_name name, in godot_variant value)
		{
			if ((ref name) == NCardTrailVfx.PropertyName._nodeToFollow)
			{
				this._nodeToFollow = VariantUtils.ConvertTo<Control>(ref value);
				return true;
			}
			if ((ref name) == NCardTrailVfx.PropertyName._sprites)
			{
				this._sprites = VariantUtils.ConvertTo<Node2D>(ref value);
				return true;
			}
			if ((ref name) == NCardTrailVfx.PropertyName._updateSprites)
			{
				this._updateSprites = VariantUtils.ConvertTo<bool>(ref value);
				return true;
			}
			if ((ref name) == NCardTrailVfx.PropertyName._tween)
			{
				this._tween = VariantUtils.ConvertTo<Tween>(ref value);
				return true;
			}
			return base.SetGodotClassPropertyValue(ref name, ref value);
		}

		/// <inheritdoc />
		// Token: 0x060013B3 RID: 5043 RVA: 0x00077060 File Offset: 0x00075260
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
		{
			if ((ref name) == NCardTrailVfx.PropertyName._nodeToFollow)
			{
				value = VariantUtils.CreateFrom<Control>(ref this._nodeToFollow);
				return true;
			}
			if ((ref name) == NCardTrailVfx.PropertyName._sprites)
			{
				value = VariantUtils.CreateFrom<Node2D>(ref this._sprites);
				return true;
			}
			if ((ref name) == NCardTrailVfx.PropertyName._updateSprites)
			{
				value = VariantUtils.CreateFrom<bool>(ref this._updateSprites);
				return true;
			}
			if ((ref name) == NCardTrailVfx.PropertyName._tween)
			{
				value = VariantUtils.CreateFrom<Tween>(ref this._tween);
				return true;
			}
			return base.GetGodotClassPropertyValue(ref name, ref value);
		}

		/// <summary>
		/// Get the property information for all the properties declared in this class.
		/// This method is used by Godot to register the available properties in the editor.
		/// Do not call this method.
		/// </summary>
		// Token: 0x060013B4 RID: 5044 RVA: 0x000770F8 File Offset: 0x000752F8
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal static List<PropertyInfo> GetGodotPropertyList()
		{
			return new List<PropertyInfo>
			{
				new PropertyInfo(24L, NCardTrailVfx.PropertyName._nodeToFollow, 0L, "", 4096L, false),
				new PropertyInfo(24L, NCardTrailVfx.PropertyName._sprites, 0L, "", 4096L, false),
				new PropertyInfo(1L, NCardTrailVfx.PropertyName._updateSprites, 0L, "", 4096L, false),
				new PropertyInfo(24L, NCardTrailVfx.PropertyName._tween, 0L, "", 4096L, false)
			};
		}

		/// <inheritdoc />
		// Token: 0x060013B5 RID: 5045 RVA: 0x00077190 File Offset: 0x00075390
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void SaveGodotObjectData(GodotSerializationInfo info)
		{
			base.SaveGodotObjectData(info);
			info.AddProperty(NCardTrailVfx.PropertyName._nodeToFollow, Variant.From<Control>(ref this._nodeToFollow));
			info.AddProperty(NCardTrailVfx.PropertyName._sprites, Variant.From<Node2D>(ref this._sprites));
			info.AddProperty(NCardTrailVfx.PropertyName._updateSprites, Variant.From<bool>(ref this._updateSprites));
			info.AddProperty(NCardTrailVfx.PropertyName._tween, Variant.From<Tween>(ref this._tween));
		}

		/// <inheritdoc />
		// Token: 0x060013B6 RID: 5046 RVA: 0x000771FC File Offset: 0x000753FC
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void RestoreGodotObjectData(GodotSerializationInfo info)
		{
			base.RestoreGodotObjectData(info);
			Variant variant;
			if (info.TryGetProperty(NCardTrailVfx.PropertyName._nodeToFollow, ref variant))
			{
				this._nodeToFollow = variant.As<Control>();
			}
			Variant variant2;
			if (info.TryGetProperty(NCardTrailVfx.PropertyName._sprites, ref variant2))
			{
				this._sprites = variant2.As<Node2D>();
			}
			Variant variant3;
			if (info.TryGetProperty(NCardTrailVfx.PropertyName._updateSprites, ref variant3))
			{
				this._updateSprites = variant3.As<bool>();
			}
			Variant variant4;
			if (info.TryGetProperty(NCardTrailVfx.PropertyName._tween, ref variant4))
			{
				this._tween = variant4.As<Tween>();
			}
		}

		// Token: 0x0400070B RID: 1803
		[Nullable(1)]
		private Control _nodeToFollow;

		// Token: 0x0400070C RID: 1804
		[Nullable(1)]
		private Node2D _sprites;

		// Token: 0x0400070D RID: 1805
		private bool _updateSprites = true;

		// Token: 0x0400070E RID: 1806
		[Nullable(2)]
		private Tween _tween;

		/// <summary>
		/// Cached StringNames for the methods contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x02000FD2 RID: 4050
		public class MethodName : Node2D.MethodName
		{
			/// <summary>
			/// Cached name for the 'Create' method.
			/// </summary>
			// Token: 0x04003EFD RID: 16125
			public static readonly StringName Create = "Create";

			/// <summary>
			/// Cached name for the '_Ready' method.
			/// </summary>
			// Token: 0x04003EFE RID: 16126
			public static readonly StringName _Ready = "_Ready";

			/// <summary>
			/// Cached name for the '_Process' method.
			/// </summary>
			// Token: 0x04003EFF RID: 16127
			public static readonly StringName _Process = "_Process";

			/// <summary>
			/// Cached name for the 'StopParticles' method.
			/// </summary>
			// Token: 0x04003F00 RID: 16128
			public static readonly StringName StopParticles = "StopParticles";

			/// <summary>
			/// Cached name for the '_ExitTree' method.
			/// </summary>
			// Token: 0x04003F01 RID: 16129
			public static readonly StringName _ExitTree = "_ExitTree";
		}

		/// <summary>
		/// Cached StringNames for the properties and fields contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x02000FD3 RID: 4051
		public class PropertyName : Node2D.PropertyName
		{
			/// <summary>
			/// Cached name for the '_nodeToFollow' field.
			/// </summary>
			// Token: 0x04003F02 RID: 16130
			public static readonly StringName _nodeToFollow = "_nodeToFollow";

			/// <summary>
			/// Cached name for the '_sprites' field.
			/// </summary>
			// Token: 0x04003F03 RID: 16131
			public static readonly StringName _sprites = "_sprites";

			/// <summary>
			/// Cached name for the '_updateSprites' field.
			/// </summary>
			// Token: 0x04003F04 RID: 16132
			public static readonly StringName _updateSprites = "_updateSprites";

			/// <summary>
			/// Cached name for the '_tween' field.
			/// </summary>
			// Token: 0x04003F05 RID: 16133
			public static readonly StringName _tween = "_tween";
		}

		/// <summary>
		/// Cached StringNames for the signals contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x02000FD4 RID: 4052
		public class SignalName : Node2D.SignalName
		{
		}
	}
}
