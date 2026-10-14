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

namespace MegaCrit.Sts2.Core.Nodes.Vfx.Cards
{
	// Token: 0x02000230 RID: 560
	[ScriptPath("res://src/Core/Nodes/Vfx/Cards/NBolasVfx.cs")]
	public class NBolasVfx : Node2D
	{
		// Token: 0x1700063F RID: 1599
		// (get) Token: 0x06001C66 RID: 7270 RVA: 0x000B81C1 File Offset: 0x000B63C1
		[Nullable(1)]
		public static IEnumerable<string> AssetPaths
		{
			[NullableContext(1)]
			get
			{
				return new <>z__ReadOnlySingleElementList<string>(NBolasVfx._scenePath);
			}
		}

		// Token: 0x06001C67 RID: 7271 RVA: 0x000B81D0 File Offset: 0x000B63D0
		[NullableContext(1)]
		[return: Nullable(2)]
		public static NBolasVfx Create(Creature owner, Creature target)
		{
			if (TestMode.IsOn)
			{
				return null;
			}
			if (target.IsDead)
			{
				return null;
			}
			NBolasVfx nbolasVfx = PreloadManager.Cache.GetScene(NBolasVfx._scenePath).Instantiate<NBolasVfx>(0L);
			nbolasVfx.GlobalPosition = NCombatRoom.Instance.GetCreatureNode(owner).VfxSpawnPosition;
			nbolasVfx._startPosition = nbolasVfx.GlobalPosition;
			nbolasVfx._endPosition = NCombatRoom.Instance.GetCreatureNode(target).VfxSpawnPosition;
			float num = Mathf.Min(nbolasVfx._startPosition.Y, nbolasVfx._endPosition.Y) - Rng.Chaotic.NextFloat(400f, 500f);
			nbolasVfx._controlPosition = new Vector2((nbolasVfx._startPosition.X + nbolasVfx._endPosition.X) * 0.5f, num);
			return nbolasVfx;
		}

		// Token: 0x06001C68 RID: 7272 RVA: 0x000B829A File Offset: 0x000B649A
		public override void _Ready()
		{
			TaskHelper.RunSafely(this.FlyBolasFly());
			this._bola2 = base.GetNode<Node2D>("Bola2");
			this._bola3 = base.GetNode<Node2D>("Bola3");
		}

		// Token: 0x06001C69 RID: 7273 RVA: 0x000B82D4 File Offset: 0x000B64D4
		public override void _ExitTree()
		{
			Tween tween = this._tween;
			if (tween == null)
			{
				return;
			}
			tween.Kill();
		}

		// Token: 0x06001C6A RID: 7274 RVA: 0x000B82E8 File Offset: 0x000B64E8
		[NullableContext(1)]
		private async Task FlyBolasFly()
		{
			this._tween = base.CreateTween().SetParallel(true);
			this._tween.TweenProperty(this, "modulate:a", 1f, 0.25).From(0f);
			this._tween.TweenMethod(Callable.From<float>(new Action<float>(this.FollowCurve)), 0f, 1f, 0.6000000238418579).SetEase(1L).SetTrans(1L);
			this._tween.Chain();
			this._tween.TweenInterval(0.15000000596046448);
			this._tween.TweenProperty(this, "modulate:a", 0f, 0.15000000596046448);
			bool flag = await this._tween.AwaitFinished(this);
			this.QueueFreeSafely();
		}

		// Token: 0x06001C6B RID: 7275 RVA: 0x000B832B File Offset: 0x000B652B
		private void FollowCurve(float progressPercent)
		{
			base.GlobalPosition = MathHelper.BezierCurve(this._startPosition, this._endPosition, this._controlPosition, progressPercent);
		}

		// Token: 0x06001C6C RID: 7276 RVA: 0x000B834C File Offset: 0x000B654C
		public override void _Process(double delta)
		{
			float num = (float)delta;
			base.Rotation += num * -this._rotationSpeed;
			this._rotationSpeed -= num * 12f;
			this._bola2.Position -= new Vector2(150f * num, 0f);
			this._bola3.Position -= new Vector2(-150f * num, 0f);
		}

		/// <summary>
		/// Get the method information for all the methods declared in this class.
		/// This method is used by Godot to register the available methods in the editor.
		/// Do not call this method.
		/// </summary>
		// Token: 0x06001C6D RID: 7277 RVA: 0x000B83D4 File Offset: 0x000B65D4
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal static List<MethodInfo> GetGodotMethodList()
		{
			return new List<MethodInfo>(4)
			{
				new MethodInfo(NBolasVfx.MethodName._Ready, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NBolasVfx.MethodName._ExitTree, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NBolasVfx.MethodName.FollowCurve, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, new List<PropertyInfo>
				{
					new PropertyInfo(3L, "progressPercent", 0L, "", 6L, false)
				}, null),
				new MethodInfo(NBolasVfx.MethodName._Process, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, new List<PropertyInfo>
				{
					new PropertyInfo(3L, "delta", 0L, "", 6L, false)
				}, null)
			};
		}

		/// <inheritdoc />
		// Token: 0x06001C6E RID: 7278 RVA: 0x000B84F0 File Offset: 0x000B66F0
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool InvokeGodotClassMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
		{
			if ((ref method) == NBolasVfx.MethodName._Ready && args.Count == 0)
			{
				this._Ready();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NBolasVfx.MethodName._ExitTree && args.Count == 0)
			{
				this._ExitTree();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NBolasVfx.MethodName.FollowCurve && args.Count == 1)
			{
				this.FollowCurve(VariantUtils.ConvertTo<float>(args[0]));
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NBolasVfx.MethodName._Process && args.Count == 1)
			{
				this._Process(VariantUtils.ConvertTo<double>(args[0]));
				ret = default(godot_variant);
				return true;
			}
			return base.InvokeGodotClassMethod(ref method, args, ref ret);
		}

		/// <inheritdoc />
		// Token: 0x06001C6F RID: 7279 RVA: 0x000B85B8 File Offset: 0x000B67B8
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool HasGodotClassMethod(in godot_string_name method)
		{
			return (ref method) == NBolasVfx.MethodName._Ready || (ref method) == NBolasVfx.MethodName._ExitTree || (ref method) == NBolasVfx.MethodName.FollowCurve || (ref method) == NBolasVfx.MethodName._Process || base.HasGodotClassMethod(ref method);
		}

		/// <inheritdoc />
		// Token: 0x06001C70 RID: 7280 RVA: 0x000B8608 File Offset: 0x000B6808
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool SetGodotClassPropertyValue(in godot_string_name name, in godot_variant value)
		{
			if ((ref name) == NBolasVfx.PropertyName._bola2)
			{
				this._bola2 = VariantUtils.ConvertTo<Node2D>(ref value);
				return true;
			}
			if ((ref name) == NBolasVfx.PropertyName._bola3)
			{
				this._bola3 = VariantUtils.ConvertTo<Node2D>(ref value);
				return true;
			}
			if ((ref name) == NBolasVfx.PropertyName._startPosition)
			{
				this._startPosition = VariantUtils.ConvertTo<Vector2>(ref value);
				return true;
			}
			if ((ref name) == NBolasVfx.PropertyName._controlPosition)
			{
				this._controlPosition = VariantUtils.ConvertTo<Vector2>(ref value);
				return true;
			}
			if ((ref name) == NBolasVfx.PropertyName._endPosition)
			{
				this._endPosition = VariantUtils.ConvertTo<Vector2>(ref value);
				return true;
			}
			if ((ref name) == NBolasVfx.PropertyName._rotationSpeed)
			{
				this._rotationSpeed = VariantUtils.ConvertTo<float>(ref value);
				return true;
			}
			if ((ref name) == NBolasVfx.PropertyName._tween)
			{
				this._tween = VariantUtils.ConvertTo<Tween>(ref value);
				return true;
			}
			return base.SetGodotClassPropertyValue(ref name, ref value);
		}

		/// <inheritdoc />
		// Token: 0x06001C71 RID: 7281 RVA: 0x000B86DC File Offset: 0x000B68DC
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
		{
			if ((ref name) == NBolasVfx.PropertyName._bola2)
			{
				value = VariantUtils.CreateFrom<Node2D>(ref this._bola2);
				return true;
			}
			if ((ref name) == NBolasVfx.PropertyName._bola3)
			{
				value = VariantUtils.CreateFrom<Node2D>(ref this._bola3);
				return true;
			}
			if ((ref name) == NBolasVfx.PropertyName._startPosition)
			{
				value = VariantUtils.CreateFrom<Vector2>(ref this._startPosition);
				return true;
			}
			if ((ref name) == NBolasVfx.PropertyName._controlPosition)
			{
				value = VariantUtils.CreateFrom<Vector2>(ref this._controlPosition);
				return true;
			}
			if ((ref name) == NBolasVfx.PropertyName._endPosition)
			{
				value = VariantUtils.CreateFrom<Vector2>(ref this._endPosition);
				return true;
			}
			if ((ref name) == NBolasVfx.PropertyName._rotationSpeed)
			{
				value = VariantUtils.CreateFrom<float>(ref this._rotationSpeed);
				return true;
			}
			if ((ref name) == NBolasVfx.PropertyName._tween)
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
		// Token: 0x06001C72 RID: 7282 RVA: 0x000B87D4 File Offset: 0x000B69D4
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal static List<PropertyInfo> GetGodotPropertyList()
		{
			return new List<PropertyInfo>
			{
				new PropertyInfo(24L, NBolasVfx.PropertyName._bola2, 0L, "", 4096L, false),
				new PropertyInfo(24L, NBolasVfx.PropertyName._bola3, 0L, "", 4096L, false),
				new PropertyInfo(5L, NBolasVfx.PropertyName._startPosition, 0L, "", 4096L, false),
				new PropertyInfo(5L, NBolasVfx.PropertyName._controlPosition, 0L, "", 4096L, false),
				new PropertyInfo(5L, NBolasVfx.PropertyName._endPosition, 0L, "", 4096L, false),
				new PropertyInfo(3L, NBolasVfx.PropertyName._rotationSpeed, 0L, "", 4096L, false),
				new PropertyInfo(24L, NBolasVfx.PropertyName._tween, 0L, "", 4096L, false)
			};
		}

		/// <inheritdoc />
		// Token: 0x06001C73 RID: 7283 RVA: 0x000B88CC File Offset: 0x000B6ACC
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void SaveGodotObjectData(GodotSerializationInfo info)
		{
			base.SaveGodotObjectData(info);
			info.AddProperty(NBolasVfx.PropertyName._bola2, Variant.From<Node2D>(ref this._bola2));
			info.AddProperty(NBolasVfx.PropertyName._bola3, Variant.From<Node2D>(ref this._bola3));
			info.AddProperty(NBolasVfx.PropertyName._startPosition, Variant.From<Vector2>(ref this._startPosition));
			info.AddProperty(NBolasVfx.PropertyName._controlPosition, Variant.From<Vector2>(ref this._controlPosition));
			info.AddProperty(NBolasVfx.PropertyName._endPosition, Variant.From<Vector2>(ref this._endPosition));
			info.AddProperty(NBolasVfx.PropertyName._rotationSpeed, Variant.From<float>(ref this._rotationSpeed));
			info.AddProperty(NBolasVfx.PropertyName._tween, Variant.From<Tween>(ref this._tween));
		}

		/// <inheritdoc />
		// Token: 0x06001C74 RID: 7284 RVA: 0x000B897C File Offset: 0x000B6B7C
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void RestoreGodotObjectData(GodotSerializationInfo info)
		{
			base.RestoreGodotObjectData(info);
			Variant variant;
			if (info.TryGetProperty(NBolasVfx.PropertyName._bola2, ref variant))
			{
				this._bola2 = variant.As<Node2D>();
			}
			Variant variant2;
			if (info.TryGetProperty(NBolasVfx.PropertyName._bola3, ref variant2))
			{
				this._bola3 = variant2.As<Node2D>();
			}
			Variant variant3;
			if (info.TryGetProperty(NBolasVfx.PropertyName._startPosition, ref variant3))
			{
				this._startPosition = variant3.As<Vector2>();
			}
			Variant variant4;
			if (info.TryGetProperty(NBolasVfx.PropertyName._controlPosition, ref variant4))
			{
				this._controlPosition = variant4.As<Vector2>();
			}
			Variant variant5;
			if (info.TryGetProperty(NBolasVfx.PropertyName._endPosition, ref variant5))
			{
				this._endPosition = variant5.As<Vector2>();
			}
			Variant variant6;
			if (info.TryGetProperty(NBolasVfx.PropertyName._rotationSpeed, ref variant6))
			{
				this._rotationSpeed = variant6.As<float>();
			}
			Variant variant7;
			if (info.TryGetProperty(NBolasVfx.PropertyName._tween, ref variant7))
			{
				this._tween = variant7.As<Tween>();
			}
		}

		// Token: 0x04000AD7 RID: 2775
		[Nullable(1)]
		private static readonly string _scenePath = SceneHelper.GetScenePath("vfx/cards/bolas_vfx");

		// Token: 0x04000AD8 RID: 2776
		[Nullable(1)]
		private Node2D _bola2;

		// Token: 0x04000AD9 RID: 2777
		[Nullable(1)]
		private Node2D _bola3;

		// Token: 0x04000ADA RID: 2778
		private Vector2 _startPosition;

		// Token: 0x04000ADB RID: 2779
		private Vector2 _controlPosition;

		// Token: 0x04000ADC RID: 2780
		private Vector2 _endPosition;

		// Token: 0x04000ADD RID: 2781
		private float _rotationSpeed = 30f;

		// Token: 0x04000ADE RID: 2782
		[Nullable(2)]
		private Tween _tween;

		/// <summary>
		/// Cached StringNames for the methods contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x020011EB RID: 4587
		public class MethodName : Node2D.MethodName
		{
			/// <summary>
			/// Cached name for the '_Ready' method.
			/// </summary>
			// Token: 0x04004581 RID: 17793
			public static readonly StringName _Ready = "_Ready";

			/// <summary>
			/// Cached name for the '_ExitTree' method.
			/// </summary>
			// Token: 0x04004582 RID: 17794
			public static readonly StringName _ExitTree = "_ExitTree";

			/// <summary>
			/// Cached name for the 'FollowCurve' method.
			/// </summary>
			// Token: 0x04004583 RID: 17795
			public static readonly StringName FollowCurve = "FollowCurve";

			/// <summary>
			/// Cached name for the '_Process' method.
			/// </summary>
			// Token: 0x04004584 RID: 17796
			public static readonly StringName _Process = "_Process";
		}

		/// <summary>
		/// Cached StringNames for the properties and fields contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x020011EC RID: 4588
		public class PropertyName : Node2D.PropertyName
		{
			/// <summary>
			/// Cached name for the '_bola2' field.
			/// </summary>
			// Token: 0x04004585 RID: 17797
			public static readonly StringName _bola2 = "_bola2";

			/// <summary>
			/// Cached name for the '_bola3' field.
			/// </summary>
			// Token: 0x04004586 RID: 17798
			public static readonly StringName _bola3 = "_bola3";

			/// <summary>
			/// Cached name for the '_startPosition' field.
			/// </summary>
			// Token: 0x04004587 RID: 17799
			public static readonly StringName _startPosition = "_startPosition";

			/// <summary>
			/// Cached name for the '_controlPosition' field.
			/// </summary>
			// Token: 0x04004588 RID: 17800
			public static readonly StringName _controlPosition = "_controlPosition";

			/// <summary>
			/// Cached name for the '_endPosition' field.
			/// </summary>
			// Token: 0x04004589 RID: 17801
			public static readonly StringName _endPosition = "_endPosition";

			/// <summary>
			/// Cached name for the '_rotationSpeed' field.
			/// </summary>
			// Token: 0x0400458A RID: 17802
			public static readonly StringName _rotationSpeed = "_rotationSpeed";

			/// <summary>
			/// Cached name for the '_tween' field.
			/// </summary>
			// Token: 0x0400458B RID: 17803
			public static readonly StringName _tween = "_tween";
		}

		/// <summary>
		/// Cached StringNames for the signals contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x020011ED RID: 4589
		public class SignalName : Node2D.SignalName
		{
		}
	}
}
