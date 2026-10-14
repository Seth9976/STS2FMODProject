using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Godot;
using Godot.Bridge;
using Godot.NativeInterop;
using MegaCrit.Sts2.Core.Bindings.MegaSpine;
using MegaCrit.Sts2.Core.Helpers;

namespace MegaCrit.Sts2.Core.Nodes.Vfx
{
	// Token: 0x020001CA RID: 458
	[ScriptPath("res://src/Core/Nodes/Vfx/NLaserVfx.cs")]
	public class NLaserVfx : Node2D
	{
		// Token: 0x06001702 RID: 5890 RVA: 0x0008EBC8 File Offset: 0x0008CDC8
		public override void _Ready()
		{
			this._animNode = base.GetNode<Node2D>("SpineSprite");
			this._animController = new MegaSprite(this._animNode);
			this._targetingBone = base.GetNode<Node2D>("SpineSprite/TargetingBone");
			this.RunWhenSpineReady(this._animController, delegate(MegaAnimationState animState)
			{
				animState.SetAnimation("animation", true, 0);
			});
			this._animNode.Visible = false;
		}

		// Token: 0x06001703 RID: 5891 RVA: 0x0008EC50 File Offset: 0x0008CE50
		public void ExtendLaser(Vector2 targetPos)
		{
			this._animNode.Visible = true;
			this._animController.GetAnimationState().SetAnimation("animation", true, 0);
			this._targetingBone.GlobalPosition = base.GlobalPosition;
			Tween tween = base.CreateTween();
			tween.TweenProperty(this._targetingBone, "position", targetPos, 0.15000000596046448).SetTrans(5L).SetEase(1L);
			tween.Chain().TweenProperty(this._animNode, "modulate", Colors.Red, 0.20000000298023224);
		}

		// Token: 0x06001704 RID: 5892 RVA: 0x0008ECFC File Offset: 0x0008CEFC
		public void RetractLaser()
		{
			Tween tween = base.CreateTween();
			tween.TweenProperty(this._targetingBone, "position", base.Position, 0.15000000596046448).SetTrans(5L).SetEase(0L);
			tween.Chain().TweenProperty(this._animNode, "visible", false, 0.0);
		}

		// Token: 0x06001705 RID: 5893 RVA: 0x0008ED74 File Offset: 0x0008CF74
		public void ResetLaser()
		{
			this._targetingBone.Position = base.Position;
		}

		// Token: 0x06001706 RID: 5894 RVA: 0x0008ED87 File Offset: 0x0008CF87
		private void SetLaserColor(Color color)
		{
			((ShaderMaterial)this._animController.GetAdditiveMaterial()).SetShaderParameter(NLaserVfx._color, color);
		}

		/// <summary>
		/// Get the method information for all the methods declared in this class.
		/// This method is used by Godot to register the available methods in the editor.
		/// Do not call this method.
		/// </summary>
		// Token: 0x06001707 RID: 5895 RVA: 0x0008EDAC File Offset: 0x0008CFAC
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal static List<MethodInfo> GetGodotMethodList()
		{
			return new List<MethodInfo>(5)
			{
				new MethodInfo(NLaserVfx.MethodName._Ready, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NLaserVfx.MethodName.ExtendLaser, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, new List<PropertyInfo>
				{
					new PropertyInfo(5L, "targetPos", 0L, "", 6L, false)
				}, null),
				new MethodInfo(NLaserVfx.MethodName.RetractLaser, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NLaserVfx.MethodName.ResetLaser, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NLaserVfx.MethodName.SetLaserColor, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, new List<PropertyInfo>
				{
					new PropertyInfo(20L, "color", 0L, "", 6L, false)
				}, null)
			};
		}

		/// <inheritdoc />
		// Token: 0x06001708 RID: 5896 RVA: 0x0008EEF8 File Offset: 0x0008D0F8
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool InvokeGodotClassMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
		{
			if ((ref method) == NLaserVfx.MethodName._Ready && args.Count == 0)
			{
				this._Ready();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NLaserVfx.MethodName.ExtendLaser && args.Count == 1)
			{
				this.ExtendLaser(VariantUtils.ConvertTo<Vector2>(args[0]));
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NLaserVfx.MethodName.RetractLaser && args.Count == 0)
			{
				this.RetractLaser();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NLaserVfx.MethodName.ResetLaser && args.Count == 0)
			{
				this.ResetLaser();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NLaserVfx.MethodName.SetLaserColor && args.Count == 1)
			{
				this.SetLaserColor(VariantUtils.ConvertTo<Color>(args[0]));
				ret = default(godot_variant);
				return true;
			}
			return base.InvokeGodotClassMethod(ref method, args, ref ret);
		}

		/// <inheritdoc />
		// Token: 0x06001709 RID: 5897 RVA: 0x0008EFE4 File Offset: 0x0008D1E4
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool HasGodotClassMethod(in godot_string_name method)
		{
			return (ref method) == NLaserVfx.MethodName._Ready || (ref method) == NLaserVfx.MethodName.ExtendLaser || (ref method) == NLaserVfx.MethodName.RetractLaser || (ref method) == NLaserVfx.MethodName.ResetLaser || (ref method) == NLaserVfx.MethodName.SetLaserColor || base.HasGodotClassMethod(ref method);
		}

		/// <inheritdoc />
		// Token: 0x0600170A RID: 5898 RVA: 0x0008F043 File Offset: 0x0008D243
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool SetGodotClassPropertyValue(in godot_string_name name, in godot_variant value)
		{
			if ((ref name) == NLaserVfx.PropertyName._animNode)
			{
				this._animNode = VariantUtils.ConvertTo<Node2D>(ref value);
				return true;
			}
			if ((ref name) == NLaserVfx.PropertyName._targetingBone)
			{
				this._targetingBone = VariantUtils.ConvertTo<Node2D>(ref value);
				return true;
			}
			return base.SetGodotClassPropertyValue(ref name, ref value);
		}

		/// <inheritdoc />
		// Token: 0x0600170B RID: 5899 RVA: 0x0008F084 File Offset: 0x0008D284
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
		{
			if ((ref name) == NLaserVfx.PropertyName._animNode)
			{
				value = VariantUtils.CreateFrom<Node2D>(ref this._animNode);
				return true;
			}
			if ((ref name) == NLaserVfx.PropertyName._targetingBone)
			{
				value = VariantUtils.CreateFrom<Node2D>(ref this._targetingBone);
				return true;
			}
			return base.GetGodotClassPropertyValue(ref name, ref value);
		}

		/// <summary>
		/// Get the property information for all the properties declared in this class.
		/// This method is used by Godot to register the available properties in the editor.
		/// Do not call this method.
		/// </summary>
		// Token: 0x0600170C RID: 5900 RVA: 0x0008F0DC File Offset: 0x0008D2DC
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal static List<PropertyInfo> GetGodotPropertyList()
		{
			return new List<PropertyInfo>
			{
				new PropertyInfo(24L, NLaserVfx.PropertyName._animNode, 0L, "", 4096L, false),
				new PropertyInfo(24L, NLaserVfx.PropertyName._targetingBone, 0L, "", 4096L, false)
			};
		}

		/// <inheritdoc />
		// Token: 0x0600170D RID: 5901 RVA: 0x0008F132 File Offset: 0x0008D332
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void SaveGodotObjectData(GodotSerializationInfo info)
		{
			base.SaveGodotObjectData(info);
			info.AddProperty(NLaserVfx.PropertyName._animNode, Variant.From<Node2D>(ref this._animNode));
			info.AddProperty(NLaserVfx.PropertyName._targetingBone, Variant.From<Node2D>(ref this._targetingBone));
		}

		/// <inheritdoc />
		// Token: 0x0600170E RID: 5902 RVA: 0x0008F168 File Offset: 0x0008D368
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void RestoreGodotObjectData(GodotSerializationInfo info)
		{
			base.RestoreGodotObjectData(info);
			Variant variant;
			if (info.TryGetProperty(NLaserVfx.PropertyName._animNode, ref variant))
			{
				this._animNode = variant.As<Node2D>();
			}
			Variant variant2;
			if (info.TryGetProperty(NLaserVfx.PropertyName._targetingBone, ref variant2))
			{
				this._targetingBone = variant2.As<Node2D>();
			}
		}

		// Token: 0x0400083F RID: 2111
		[Nullable(1)]
		private static readonly StringName _color = new StringName("Color");

		// Token: 0x04000840 RID: 2112
		[Nullable(1)]
		private Node2D _animNode;

		// Token: 0x04000841 RID: 2113
		[Nullable(1)]
		private MegaSprite _animController;

		// Token: 0x04000842 RID: 2114
		[Nullable(1)]
		private Node2D _targetingBone;

		/// <summary>
		/// Cached StringNames for the methods contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x020010A0 RID: 4256
		public class MethodName : Node2D.MethodName
		{
			/// <summary>
			/// Cached name for the '_Ready' method.
			/// </summary>
			// Token: 0x04004163 RID: 16739
			public static readonly StringName _Ready = "_Ready";

			/// <summary>
			/// Cached name for the 'ExtendLaser' method.
			/// </summary>
			// Token: 0x04004164 RID: 16740
			public static readonly StringName ExtendLaser = "ExtendLaser";

			/// <summary>
			/// Cached name for the 'RetractLaser' method.
			/// </summary>
			// Token: 0x04004165 RID: 16741
			public static readonly StringName RetractLaser = "RetractLaser";

			/// <summary>
			/// Cached name for the 'ResetLaser' method.
			/// </summary>
			// Token: 0x04004166 RID: 16742
			public static readonly StringName ResetLaser = "ResetLaser";

			/// <summary>
			/// Cached name for the 'SetLaserColor' method.
			/// </summary>
			// Token: 0x04004167 RID: 16743
			public static readonly StringName SetLaserColor = "SetLaserColor";
		}

		/// <summary>
		/// Cached StringNames for the properties and fields contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x020010A1 RID: 4257
		public class PropertyName : Node2D.PropertyName
		{
			/// <summary>
			/// Cached name for the '_animNode' field.
			/// </summary>
			// Token: 0x04004168 RID: 16744
			public static readonly StringName _animNode = "_animNode";

			/// <summary>
			/// Cached name for the '_targetingBone' field.
			/// </summary>
			// Token: 0x04004169 RID: 16745
			public static readonly StringName _targetingBone = "_targetingBone";
		}

		/// <summary>
		/// Cached StringNames for the signals contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x020010A2 RID: 4258
		public class SignalName : Node2D.SignalName
		{
		}
	}
}
