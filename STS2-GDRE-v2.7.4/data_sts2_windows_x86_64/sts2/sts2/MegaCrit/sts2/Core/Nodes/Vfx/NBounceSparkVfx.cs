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
using MegaCrit.Sts2.Core.Nodes.GodotExtensions;
using MegaCrit.Sts2.Core.Nodes.Rooms;
using MegaCrit.Sts2.Core.Random;
using MegaCrit.Sts2.Core.TestSupport;

namespace MegaCrit.Sts2.Core.Nodes.Vfx
{
	// Token: 0x0200018B RID: 395
	[ScriptPath("res://src/Core/Nodes/Vfx/NBounceSparkVfx.cs")]
	public class NBounceSparkVfx : Node2D
	{
		// Token: 0x06001325 RID: 4901 RVA: 0x00072C70 File Offset: 0x00070E70
		[NullableContext(1)]
		[return: Nullable(2)]
		public static NBounceSparkVfx Create(Creature target, VfxColor vfxColor = VfxColor.Gold)
		{
			if (TestMode.IsOn)
			{
				return null;
			}
			NBounceSparkVfx nbounceSparkVfx = PreloadManager.Cache.GetScene("res://scenes/vfx/bounce_spark_vfx.tscn").Instantiate<NBounceSparkVfx>(0L);
			nbounceSparkVfx._startPosition = NCombatRoom.Instance.GetCreatureNode(target).GetBottomOfHitbox();
			return nbounceSparkVfx;
		}

		// Token: 0x06001326 RID: 4902 RVA: 0x00072CB4 File Offset: 0x00070EB4
		public override void _Ready()
		{
			this._startPosition += new Vector2(Rng.Chaotic.NextFloat(-120f, 120f), Rng.Chaotic.NextFloat(0f, 20f));
			this._floorY = this._startPosition.Y + Rng.Chaotic.NextFloat(0f, 64f);
			base.GlobalPosition = this._startPosition;
			this._velocity = new Vector2(Rng.Chaotic.NextFloat(-400f, 400f), Rng.Chaotic.NextFloat(-800f, -300f));
			float num = Rng.Chaotic.NextFloat(0.8f, 1.2f);
			base.Scale = new Vector2(num, 2f - num) * Rng.Chaotic.NextFloat(0.1f, 0.8f);
			base.Modulate = new Color(1f, Rng.Chaotic.NextFloat(0.2f, 0.8f), Rng.Chaotic.NextFloat(0f, 0.2f), 0f);
			TaskHelper.RunSafely(this.Animate());
		}

		// Token: 0x06001327 RID: 4903 RVA: 0x00072DF0 File Offset: 0x00070FF0
		public override void _Process(double delta)
		{
			float num = (float)delta;
			base.Position += this._velocity * num;
			this._velocity += NBounceSparkVfx._gravity * num;
			base.Rotation = MathHelper.GetAngle(this._velocity);
			if (base.Position.Y > this._floorY)
			{
				base.Position = new Vector2(base.Position.X, this._floorY);
				this._velocity = new Vector2(this._velocity.X, -this._velocity.Y * 0.5f);
			}
		}

		// Token: 0x06001328 RID: 4904 RVA: 0x00072EA1 File Offset: 0x000710A1
		public override void _ExitTree()
		{
			Tween tween = this._tween;
			if (tween == null)
			{
				return;
			}
			tween.Kill();
		}

		// Token: 0x06001329 RID: 4905 RVA: 0x00072EB4 File Offset: 0x000710B4
		[NullableContext(1)]
		private async Task Animate()
		{
			this._tween = base.CreateTween().SetParallel(true);
			this._tween.TweenProperty(this, "modulate:a", 0.8f, 0.10000000149011612);
			this._tween.Chain();
			float num = Rng.Chaotic.NextFloat(0.4f, 1.5f);
			this._tween.TweenProperty(this, "modulate:a", 0f, (double)num);
			this._tween.TweenProperty(this, "scale", base.Scale * 0.5f, (double)num);
			bool flag = await this._tween.AwaitFinished(this);
			this.QueueFreeSafely();
		}

		/// <summary>
		/// Get the method information for all the methods declared in this class.
		/// This method is used by Godot to register the available methods in the editor.
		/// Do not call this method.
		/// </summary>
		// Token: 0x0600132A RID: 4906 RVA: 0x00072EF8 File Offset: 0x000710F8
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal static List<MethodInfo> GetGodotMethodList()
		{
			return new List<MethodInfo>(3)
			{
				new MethodInfo(NBounceSparkVfx.MethodName._Ready, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NBounceSparkVfx.MethodName._Process, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, new List<PropertyInfo>
				{
					new PropertyInfo(3L, "delta", 0L, "", 6L, false)
				}, null),
				new MethodInfo(NBounceSparkVfx.MethodName._ExitTree, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null)
			};
		}

		/// <inheritdoc />
		// Token: 0x0600132B RID: 4907 RVA: 0x00072FC0 File Offset: 0x000711C0
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool InvokeGodotClassMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
		{
			if ((ref method) == NBounceSparkVfx.MethodName._Ready && args.Count == 0)
			{
				this._Ready();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NBounceSparkVfx.MethodName._Process && args.Count == 1)
			{
				this._Process(VariantUtils.ConvertTo<double>(args[0]));
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NBounceSparkVfx.MethodName._ExitTree && args.Count == 0)
			{
				this._ExitTree();
				ret = default(godot_variant);
				return true;
			}
			return base.InvokeGodotClassMethod(ref method, args, ref ret);
		}

		/// <inheritdoc />
		// Token: 0x0600132C RID: 4908 RVA: 0x00073053 File Offset: 0x00071253
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool HasGodotClassMethod(in godot_string_name method)
		{
			return (ref method) == NBounceSparkVfx.MethodName._Ready || (ref method) == NBounceSparkVfx.MethodName._Process || (ref method) == NBounceSparkVfx.MethodName._ExitTree || base.HasGodotClassMethod(ref method);
		}

		/// <inheritdoc />
		// Token: 0x0600132D RID: 4909 RVA: 0x0007308C File Offset: 0x0007128C
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool SetGodotClassPropertyValue(in godot_string_name name, in godot_variant value)
		{
			if ((ref name) == NBounceSparkVfx.PropertyName._particle)
			{
				this._particle = VariantUtils.ConvertTo<Node2D>(ref value);
				return true;
			}
			if ((ref name) == NBounceSparkVfx.PropertyName._velocity)
			{
				this._velocity = VariantUtils.ConvertTo<Vector2>(ref value);
				return true;
			}
			if ((ref name) == NBounceSparkVfx.PropertyName._startPosition)
			{
				this._startPosition = VariantUtils.ConvertTo<Vector2>(ref value);
				return true;
			}
			if ((ref name) == NBounceSparkVfx.PropertyName._floorY)
			{
				this._floorY = VariantUtils.ConvertTo<float>(ref value);
				return true;
			}
			if ((ref name) == NBounceSparkVfx.PropertyName._tween)
			{
				this._tween = VariantUtils.ConvertTo<Tween>(ref value);
				return true;
			}
			return base.SetGodotClassPropertyValue(ref name, ref value);
		}

		/// <inheritdoc />
		// Token: 0x0600132E RID: 4910 RVA: 0x00073128 File Offset: 0x00071328
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
		{
			if ((ref name) == NBounceSparkVfx.PropertyName._particle)
			{
				value = VariantUtils.CreateFrom<Node2D>(ref this._particle);
				return true;
			}
			if ((ref name) == NBounceSparkVfx.PropertyName._velocity)
			{
				value = VariantUtils.CreateFrom<Vector2>(ref this._velocity);
				return true;
			}
			if ((ref name) == NBounceSparkVfx.PropertyName._startPosition)
			{
				value = VariantUtils.CreateFrom<Vector2>(ref this._startPosition);
				return true;
			}
			if ((ref name) == NBounceSparkVfx.PropertyName._floorY)
			{
				value = VariantUtils.CreateFrom<float>(ref this._floorY);
				return true;
			}
			if ((ref name) == NBounceSparkVfx.PropertyName._tween)
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
		// Token: 0x0600132F RID: 4911 RVA: 0x000731E0 File Offset: 0x000713E0
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal static List<PropertyInfo> GetGodotPropertyList()
		{
			return new List<PropertyInfo>
			{
				new PropertyInfo(24L, NBounceSparkVfx.PropertyName._particle, 34L, "Node2D", 4102L, true),
				new PropertyInfo(5L, NBounceSparkVfx.PropertyName._velocity, 0L, "", 4096L, false),
				new PropertyInfo(5L, NBounceSparkVfx.PropertyName._startPosition, 0L, "", 4096L, false),
				new PropertyInfo(3L, NBounceSparkVfx.PropertyName._floorY, 0L, "", 4096L, false),
				new PropertyInfo(24L, NBounceSparkVfx.PropertyName._tween, 0L, "", 4096L, false)
			};
		}

		/// <inheritdoc />
		// Token: 0x06001330 RID: 4912 RVA: 0x00073298 File Offset: 0x00071498
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void SaveGodotObjectData(GodotSerializationInfo info)
		{
			base.SaveGodotObjectData(info);
			info.AddProperty(NBounceSparkVfx.PropertyName._particle, Variant.From<Node2D>(ref this._particle));
			info.AddProperty(NBounceSparkVfx.PropertyName._velocity, Variant.From<Vector2>(ref this._velocity));
			info.AddProperty(NBounceSparkVfx.PropertyName._startPosition, Variant.From<Vector2>(ref this._startPosition));
			info.AddProperty(NBounceSparkVfx.PropertyName._floorY, Variant.From<float>(ref this._floorY));
			info.AddProperty(NBounceSparkVfx.PropertyName._tween, Variant.From<Tween>(ref this._tween));
		}

		/// <inheritdoc />
		// Token: 0x06001331 RID: 4913 RVA: 0x0007331C File Offset: 0x0007151C
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void RestoreGodotObjectData(GodotSerializationInfo info)
		{
			base.RestoreGodotObjectData(info);
			Variant variant;
			if (info.TryGetProperty(NBounceSparkVfx.PropertyName._particle, ref variant))
			{
				this._particle = variant.As<Node2D>();
			}
			Variant variant2;
			if (info.TryGetProperty(NBounceSparkVfx.PropertyName._velocity, ref variant2))
			{
				this._velocity = variant2.As<Vector2>();
			}
			Variant variant3;
			if (info.TryGetProperty(NBounceSparkVfx.PropertyName._startPosition, ref variant3))
			{
				this._startPosition = variant3.As<Vector2>();
			}
			Variant variant4;
			if (info.TryGetProperty(NBounceSparkVfx.PropertyName._floorY, ref variant4))
			{
				this._floorY = variant4.As<float>();
			}
			Variant variant5;
			if (info.TryGetProperty(NBounceSparkVfx.PropertyName._tween, ref variant5))
			{
				this._tween = variant5.As<Tween>();
			}
		}

		// Token: 0x040006C3 RID: 1731
		[Nullable(1)]
		private const string _scenePath = "res://scenes/vfx/bounce_spark_vfx.tscn";

		// Token: 0x040006C4 RID: 1732
		[Nullable(1)]
		[Export(/*Failed to decode CustomAttribute blob!*/)]
		private Node2D _particle;

		// Token: 0x040006C5 RID: 1733
		private Vector2 _velocity;

		// Token: 0x040006C6 RID: 1734
		private Vector2 _startPosition;

		// Token: 0x040006C7 RID: 1735
		private float _floorY;

		// Token: 0x040006C8 RID: 1736
		private const float _targetAlpha = 0.8f;

		// Token: 0x040006C9 RID: 1737
		private static readonly Vector2 _gravity = new Vector2(0f, 1500f);

		// Token: 0x040006CA RID: 1738
		[Nullable(2)]
		private Tween _tween;

		/// <summary>
		/// Cached StringNames for the methods contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x02000FB5 RID: 4021
		public class MethodName : Node2D.MethodName
		{
			/// <summary>
			/// Cached name for the '_Ready' method.
			/// </summary>
			// Token: 0x04003E89 RID: 16009
			public static readonly StringName _Ready = "_Ready";

			/// <summary>
			/// Cached name for the '_Process' method.
			/// </summary>
			// Token: 0x04003E8A RID: 16010
			public static readonly StringName _Process = "_Process";

			/// <summary>
			/// Cached name for the '_ExitTree' method.
			/// </summary>
			// Token: 0x04003E8B RID: 16011
			public static readonly StringName _ExitTree = "_ExitTree";
		}

		/// <summary>
		/// Cached StringNames for the properties and fields contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x02000FB6 RID: 4022
		public class PropertyName : Node2D.PropertyName
		{
			/// <summary>
			/// Cached name for the '_particle' field.
			/// </summary>
			// Token: 0x04003E8C RID: 16012
			public static readonly StringName _particle = "_particle";

			/// <summary>
			/// Cached name for the '_velocity' field.
			/// </summary>
			// Token: 0x04003E8D RID: 16013
			public static readonly StringName _velocity = "_velocity";

			/// <summary>
			/// Cached name for the '_startPosition' field.
			/// </summary>
			// Token: 0x04003E8E RID: 16014
			public static readonly StringName _startPosition = "_startPosition";

			/// <summary>
			/// Cached name for the '_floorY' field.
			/// </summary>
			// Token: 0x04003E8F RID: 16015
			public static readonly StringName _floorY = "_floorY";

			/// <summary>
			/// Cached name for the '_tween' field.
			/// </summary>
			// Token: 0x04003E90 RID: 16016
			public static readonly StringName _tween = "_tween";
		}

		/// <summary>
		/// Cached StringNames for the signals contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x02000FB7 RID: 4023
		public class SignalName : Node2D.SignalName
		{
		}
	}
}
