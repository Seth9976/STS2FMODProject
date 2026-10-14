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
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.Nodes.Cards;
using MegaCrit.Sts2.Core.Nodes.Combat;
using MegaCrit.Sts2.Core.Nodes.GodotExtensions;
using MegaCrit.Sts2.Core.Nodes.Rooms;
using MegaCrit.Sts2.Core.Nodes.Vfx.Utilities;
using MegaCrit.Sts2.Core.TestSupport;

namespace MegaCrit.Sts2.Core.Nodes.Vfx
{
	// Token: 0x0200018D RID: 397
	[NullableContext(1)]
	[Nullable(0)]
	[ScriptPath("res://src/Core/Nodes/Vfx/NCardFlyPowerVfx.cs")]
	public class NCardFlyPowerVfx : Node2D
	{
		// Token: 0x170005F4 RID: 1524
		// (get) Token: 0x06001347 RID: 4935 RVA: 0x00073B10 File Offset: 0x00071D10
		// (set) Token: 0x06001348 RID: 4936 RVA: 0x00073B18 File Offset: 0x00071D18
		public NCard CardNode { get; private set; }

		// Token: 0x170005F5 RID: 1525
		// (get) Token: 0x06001349 RID: 4937 RVA: 0x00073B21 File Offset: 0x00071D21
		public static IEnumerable<string> AssetPaths
		{
			get
			{
				return new <>z__ReadOnlySingleElementList<string>(NCardFlyPowerVfx._scenePath);
			}
		}

		// Token: 0x0600134A RID: 4938 RVA: 0x00073B30 File Offset: 0x00071D30
		[return: Nullable(2)]
		public static NCardFlyPowerVfx Create(NCard card)
		{
			if (TestMode.IsOn)
			{
				return null;
			}
			NCardFlyPowerVfx ncardFlyPowerVfx = PreloadManager.Cache.GetScene(NCardFlyPowerVfx._scenePath).Instantiate<NCardFlyPowerVfx>(0L);
			ncardFlyPowerVfx.CardNode = card;
			return ncardFlyPowerVfx;
		}

		// Token: 0x0600134B RID: 4939 RVA: 0x00073B68 File Offset: 0x00071D68
		public override void _Ready()
		{
			base.GlobalPosition = this.CardNode.GlobalPosition;
			Player owner = this.CardNode.Model.Owner;
			this._cardOwnerNode = NCombatRoom.Instance.GetCreatureNode(owner.Creature);
			this._vfx = NCardTrailVfx.Create(this.CardNode, owner.Character.TrailPath);
			if (this._vfx != null)
			{
				this.AddChildSafely(this._vfx);
			}
			Vector2 vfxSpawnPosition = this._cardOwnerNode.VfxSpawnPosition;
			Vector2 vector = vfxSpawnPosition - base.GlobalPosition;
			this._swooshPath = base.GetNode<Path2D>("SwooshPath");
			this._swooshPath.Curve.SetPointPosition(this._swooshPath.Curve.PointCount - 1, vector);
		}

		// Token: 0x0600134C RID: 4940 RVA: 0x00073C2F File Offset: 0x00071E2F
		public override void _ExitTree()
		{
			base._ExitTree();
			Tween scaleTween = this._scaleTween;
			if (scaleTween != null)
			{
				scaleTween.Kill();
			}
			this._cancelToken.Cancel();
		}

		// Token: 0x0600134D RID: 4941 RVA: 0x00073C53 File Offset: 0x00071E53
		public float GetDuration()
		{
			return this.GetDurationInternal() + 0.05f;
		}

		// Token: 0x0600134E RID: 4942 RVA: 0x00073C61 File Offset: 0x00071E61
		private float GetDurationInternal()
		{
			return this._swooshPath.Curve.GetBakedLength() / 3000f;
		}

		// Token: 0x0600134F RID: 4943 RVA: 0x00073C7C File Offset: 0x00071E7C
		public async Task PlayAnim()
		{
			SfxCmd.Play("event:/sfx/ui/cards/card_movement_B_power", 1f);
			this._scaleTween = base.CreateTween();
			this._scaleTween.TweenProperty(this.CardNode, "scale", Vector2.One * 0.1f, 0.30000001192092896);
			float length = this._swooshPath.Curve.GetBakedLength();
			double timeAccumulator = 0.0;
			float duration = this.GetDurationInternal();
			while (timeAccumulator < (double)duration)
			{
				await this.AwaitProcessFrame(default(CancellationToken));
				if (this._cancelToken.IsCancellationRequested)
				{
					break;
				}
				double processDeltaTime = base.GetProcessDeltaTime();
				timeAccumulator += processDeltaTime;
				float num = (float)(timeAccumulator / (double)duration);
				float num2 = Ease.QuadIn(num);
				Transform2D transform2D = this._swooshPath.Curve.SampleBakedWithRotation(num2 * length, false);
				this.CardNode.GlobalPosition = base.GlobalPosition + transform2D.Origin;
				float num3 = transform2D.Rotation - this.CardNode.Rotation;
				float num4 = Mathf.Lerp(3.1415927f, 157.07964f, num);
				this.CardNode.Rotation += (float)Mathf.Sign(num3) * Mathf.Min(Mathf.Abs(num3), (float)((double)num4 * processDeltaTime));
				if (num >= 0.9f && !this._scalingOut)
				{
					this._scalingOut = true;
					Tween scaleTween = this._scaleTween;
					if (scaleTween != null)
					{
						scaleTween.Kill();
					}
					this._scaleTween = base.CreateTween();
					this._scaleTween.TweenProperty(this.CardNode, "scale", Vector2.Zero, (double)((float)((double)duration - timeAccumulator)));
				}
			}
			NGame.Instance.ScreenShake(ShakeStrength.Medium, ShakeDuration.Short, -1f);
			if (this._vfx != null)
			{
				await this._vfx.FadeOut();
			}
			this.CardNode.QueueFreeSafely();
			this.QueueFreeSafely();
		}

		/// <summary>
		/// Get the method information for all the methods declared in this class.
		/// This method is used by Godot to register the available methods in the editor.
		/// Do not call this method.
		/// </summary>
		// Token: 0x06001350 RID: 4944 RVA: 0x00073CC0 File Offset: 0x00071EC0
		[NullableContext(0)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal static List<MethodInfo> GetGodotMethodList()
		{
			return new List<MethodInfo>(5)
			{
				new MethodInfo(NCardFlyPowerVfx.MethodName.Create, new PropertyInfo(24L, "", 0L, "", 6L, new StringName("Node2D"), false), 33L, new List<PropertyInfo>
				{
					new PropertyInfo(24L, "card", 0L, "", 6L, new StringName("Control"), false)
				}, null),
				new MethodInfo(NCardFlyPowerVfx.MethodName._Ready, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NCardFlyPowerVfx.MethodName._ExitTree, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NCardFlyPowerVfx.MethodName.GetDuration, new PropertyInfo(3L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NCardFlyPowerVfx.MethodName.GetDurationInternal, new PropertyInfo(3L, "", 0L, "", 6L, false), 1L, null, null)
			};
		}

		/// <inheritdoc />
		// Token: 0x06001351 RID: 4945 RVA: 0x00073DFC File Offset: 0x00071FFC
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool InvokeGodotClassMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
		{
			if ((ref method) == NCardFlyPowerVfx.MethodName.Create && args.Count == 1)
			{
				NCardFlyPowerVfx ncardFlyPowerVfx = NCardFlyPowerVfx.Create(VariantUtils.ConvertTo<NCard>(args[0]));
				ret = VariantUtils.CreateFrom<NCardFlyPowerVfx>(ref ncardFlyPowerVfx);
				return true;
			}
			if ((ref method) == NCardFlyPowerVfx.MethodName._Ready && args.Count == 0)
			{
				this._Ready();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NCardFlyPowerVfx.MethodName._ExitTree && args.Count == 0)
			{
				this._ExitTree();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NCardFlyPowerVfx.MethodName.GetDuration && args.Count == 0)
			{
				float duration = this.GetDuration();
				ret = VariantUtils.CreateFrom<float>(ref duration);
				return true;
			}
			if ((ref method) == NCardFlyPowerVfx.MethodName.GetDurationInternal && args.Count == 0)
			{
				float durationInternal = this.GetDurationInternal();
				ret = VariantUtils.CreateFrom<float>(ref durationInternal);
				return true;
			}
			return base.InvokeGodotClassMethod(ref method, args, ref ret);
		}

		// Token: 0x06001352 RID: 4946 RVA: 0x00073EF0 File Offset: 0x000720F0
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal static bool InvokeGodotClassStaticMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
		{
			if ((ref method) == NCardFlyPowerVfx.MethodName.Create && args.Count == 1)
			{
				NCardFlyPowerVfx ncardFlyPowerVfx = NCardFlyPowerVfx.Create(VariantUtils.ConvertTo<NCard>(args[0]));
				ret = VariantUtils.CreateFrom<NCardFlyPowerVfx>(ref ncardFlyPowerVfx);
				return true;
			}
			ret = default(godot_variant);
			return false;
		}

		/// <inheritdoc />
		// Token: 0x06001353 RID: 4947 RVA: 0x00073F40 File Offset: 0x00072140
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool HasGodotClassMethod(in godot_string_name method)
		{
			return (ref method) == NCardFlyPowerVfx.MethodName.Create || (ref method) == NCardFlyPowerVfx.MethodName._Ready || (ref method) == NCardFlyPowerVfx.MethodName._ExitTree || (ref method) == NCardFlyPowerVfx.MethodName.GetDuration || (ref method) == NCardFlyPowerVfx.MethodName.GetDurationInternal || base.HasGodotClassMethod(ref method);
		}

		/// <inheritdoc />
		// Token: 0x06001354 RID: 4948 RVA: 0x00073FA0 File Offset: 0x000721A0
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool SetGodotClassPropertyValue(in godot_string_name name, in godot_variant value)
		{
			if ((ref name) == NCardFlyPowerVfx.PropertyName.CardNode)
			{
				this.CardNode = VariantUtils.ConvertTo<NCard>(ref value);
				return true;
			}
			if ((ref name) == NCardFlyPowerVfx.PropertyName._cardOwnerNode)
			{
				this._cardOwnerNode = VariantUtils.ConvertTo<NCreature>(ref value);
				return true;
			}
			if ((ref name) == NCardFlyPowerVfx.PropertyName._vfx)
			{
				this._vfx = VariantUtils.ConvertTo<NCardTrailVfx>(ref value);
				return true;
			}
			if ((ref name) == NCardFlyPowerVfx.PropertyName._swooshPath)
			{
				this._swooshPath = VariantUtils.ConvertTo<Path2D>(ref value);
				return true;
			}
			if ((ref name) == NCardFlyPowerVfx.PropertyName._scaleTween)
			{
				this._scaleTween = VariantUtils.ConvertTo<Tween>(ref value);
				return true;
			}
			if ((ref name) == NCardFlyPowerVfx.PropertyName._scalingOut)
			{
				this._scalingOut = VariantUtils.ConvertTo<bool>(ref value);
				return true;
			}
			return base.SetGodotClassPropertyValue(ref name, ref value);
		}

		/// <inheritdoc />
		// Token: 0x06001355 RID: 4949 RVA: 0x00074058 File Offset: 0x00072258
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
		{
			if ((ref name) == NCardFlyPowerVfx.PropertyName.CardNode)
			{
				NCard cardNode = this.CardNode;
				value = VariantUtils.CreateFrom<NCard>(ref cardNode);
				return true;
			}
			if ((ref name) == NCardFlyPowerVfx.PropertyName._cardOwnerNode)
			{
				value = VariantUtils.CreateFrom<NCreature>(ref this._cardOwnerNode);
				return true;
			}
			if ((ref name) == NCardFlyPowerVfx.PropertyName._vfx)
			{
				value = VariantUtils.CreateFrom<NCardTrailVfx>(ref this._vfx);
				return true;
			}
			if ((ref name) == NCardFlyPowerVfx.PropertyName._swooshPath)
			{
				value = VariantUtils.CreateFrom<Path2D>(ref this._swooshPath);
				return true;
			}
			if ((ref name) == NCardFlyPowerVfx.PropertyName._scaleTween)
			{
				value = VariantUtils.CreateFrom<Tween>(ref this._scaleTween);
				return true;
			}
			if ((ref name) == NCardFlyPowerVfx.PropertyName._scalingOut)
			{
				value = VariantUtils.CreateFrom<bool>(ref this._scalingOut);
				return true;
			}
			return base.GetGodotClassPropertyValue(ref name, ref value);
		}

		/// <summary>
		/// Get the property information for all the properties declared in this class.
		/// This method is used by Godot to register the available properties in the editor.
		/// Do not call this method.
		/// </summary>
		// Token: 0x06001356 RID: 4950 RVA: 0x00074130 File Offset: 0x00072330
		[NullableContext(0)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal static List<PropertyInfo> GetGodotPropertyList()
		{
			return new List<PropertyInfo>
			{
				new PropertyInfo(24L, NCardFlyPowerVfx.PropertyName.CardNode, 0L, "", 4096L, false),
				new PropertyInfo(24L, NCardFlyPowerVfx.PropertyName._cardOwnerNode, 0L, "", 4096L, false),
				new PropertyInfo(24L, NCardFlyPowerVfx.PropertyName._vfx, 0L, "", 4096L, false),
				new PropertyInfo(24L, NCardFlyPowerVfx.PropertyName._swooshPath, 0L, "", 4096L, false),
				new PropertyInfo(24L, NCardFlyPowerVfx.PropertyName._scaleTween, 0L, "", 4096L, false),
				new PropertyInfo(1L, NCardFlyPowerVfx.PropertyName._scalingOut, 0L, "", 4096L, false)
			};
		}

		/// <inheritdoc />
		// Token: 0x06001357 RID: 4951 RVA: 0x0007420C File Offset: 0x0007240C
		[NullableContext(0)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void SaveGodotObjectData(GodotSerializationInfo info)
		{
			base.SaveGodotObjectData(info);
			StringName cardNode = NCardFlyPowerVfx.PropertyName.CardNode;
			NCard cardNode2 = this.CardNode;
			info.AddProperty(cardNode, Variant.From<NCard>(ref cardNode2));
			info.AddProperty(NCardFlyPowerVfx.PropertyName._cardOwnerNode, Variant.From<NCreature>(ref this._cardOwnerNode));
			info.AddProperty(NCardFlyPowerVfx.PropertyName._vfx, Variant.From<NCardTrailVfx>(ref this._vfx));
			info.AddProperty(NCardFlyPowerVfx.PropertyName._swooshPath, Variant.From<Path2D>(ref this._swooshPath));
			info.AddProperty(NCardFlyPowerVfx.PropertyName._scaleTween, Variant.From<Tween>(ref this._scaleTween));
			info.AddProperty(NCardFlyPowerVfx.PropertyName._scalingOut, Variant.From<bool>(ref this._scalingOut));
		}

		/// <inheritdoc />
		// Token: 0x06001358 RID: 4952 RVA: 0x000742A8 File Offset: 0x000724A8
		[NullableContext(0)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void RestoreGodotObjectData(GodotSerializationInfo info)
		{
			base.RestoreGodotObjectData(info);
			Variant variant;
			if (info.TryGetProperty(NCardFlyPowerVfx.PropertyName.CardNode, ref variant))
			{
				this.CardNode = variant.As<NCard>();
			}
			Variant variant2;
			if (info.TryGetProperty(NCardFlyPowerVfx.PropertyName._cardOwnerNode, ref variant2))
			{
				this._cardOwnerNode = variant2.As<NCreature>();
			}
			Variant variant3;
			if (info.TryGetProperty(NCardFlyPowerVfx.PropertyName._vfx, ref variant3))
			{
				this._vfx = variant3.As<NCardTrailVfx>();
			}
			Variant variant4;
			if (info.TryGetProperty(NCardFlyPowerVfx.PropertyName._swooshPath, ref variant4))
			{
				this._swooshPath = variant4.As<Path2D>();
			}
			Variant variant5;
			if (info.TryGetProperty(NCardFlyPowerVfx.PropertyName._scaleTween, ref variant5))
			{
				this._scaleTween = variant5.As<Tween>();
			}
			Variant variant6;
			if (info.TryGetProperty(NCardFlyPowerVfx.PropertyName._scalingOut, ref variant6))
			{
				this._scalingOut = variant6.As<bool>();
			}
		}

		// Token: 0x040006D4 RID: 1748
		private const float _speed = 3000f;

		// Token: 0x040006D5 RID: 1749
		private const float _scaleOutProportion = 0.9f;

		// Token: 0x040006D6 RID: 1750
		private const float _initialRotationSpeed = 3.1415927f;

		// Token: 0x040006D7 RID: 1751
		private const float _maxRotationSpeed = 157.07964f;

		// Token: 0x040006D9 RID: 1753
		private NCreature _cardOwnerNode;

		// Token: 0x040006DA RID: 1754
		[Nullable(2)]
		private NCardTrailVfx _vfx;

		// Token: 0x040006DB RID: 1755
		private Path2D _swooshPath;

		// Token: 0x040006DC RID: 1756
		[Nullable(2)]
		private Tween _scaleTween;

		// Token: 0x040006DD RID: 1757
		private bool _scalingOut;

		// Token: 0x040006DE RID: 1758
		private readonly CancellationTokenSource _cancelToken = new CancellationTokenSource();

		// Token: 0x040006DF RID: 1759
		private static readonly string _scenePath = SceneHelper.GetScenePath("vfx/vfx_card_power_fly");

		/// <summary>
		/// Cached StringNames for the methods contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x02000FBD RID: 4029
		[NullableContext(0)]
		public class MethodName : Node2D.MethodName
		{
			/// <summary>
			/// Cached name for the 'Create' method.
			/// </summary>
			// Token: 0x04003EA2 RID: 16034
			public static readonly StringName Create = "Create";

			/// <summary>
			/// Cached name for the '_Ready' method.
			/// </summary>
			// Token: 0x04003EA3 RID: 16035
			public static readonly StringName _Ready = "_Ready";

			/// <summary>
			/// Cached name for the '_ExitTree' method.
			/// </summary>
			// Token: 0x04003EA4 RID: 16036
			public static readonly StringName _ExitTree = "_ExitTree";

			/// <summary>
			/// Cached name for the 'GetDuration' method.
			/// </summary>
			// Token: 0x04003EA5 RID: 16037
			public static readonly StringName GetDuration = "GetDuration";

			/// <summary>
			/// Cached name for the 'GetDurationInternal' method.
			/// </summary>
			// Token: 0x04003EA6 RID: 16038
			public static readonly StringName GetDurationInternal = "GetDurationInternal";
		}

		/// <summary>
		/// Cached StringNames for the properties and fields contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x02000FBE RID: 4030
		[NullableContext(0)]
		public class PropertyName : Node2D.PropertyName
		{
			/// <summary>
			/// Cached name for the 'CardNode' property.
			/// </summary>
			// Token: 0x04003EA7 RID: 16039
			public static readonly StringName CardNode = "CardNode";

			/// <summary>
			/// Cached name for the '_cardOwnerNode' field.
			/// </summary>
			// Token: 0x04003EA8 RID: 16040
			public static readonly StringName _cardOwnerNode = "_cardOwnerNode";

			/// <summary>
			/// Cached name for the '_vfx' field.
			/// </summary>
			// Token: 0x04003EA9 RID: 16041
			public static readonly StringName _vfx = "_vfx";

			/// <summary>
			/// Cached name for the '_swooshPath' field.
			/// </summary>
			// Token: 0x04003EAA RID: 16042
			public static readonly StringName _swooshPath = "_swooshPath";

			/// <summary>
			/// Cached name for the '_scaleTween' field.
			/// </summary>
			// Token: 0x04003EAB RID: 16043
			public static readonly StringName _scaleTween = "_scaleTween";

			/// <summary>
			/// Cached name for the '_scalingOut' field.
			/// </summary>
			// Token: 0x04003EAC RID: 16044
			public static readonly StringName _scalingOut = "_scalingOut";
		}

		/// <summary>
		/// Cached StringNames for the signals contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x02000FBF RID: 4031
		[NullableContext(0)]
		public class SignalName : Node2D.SignalName
		{
		}
	}
}
