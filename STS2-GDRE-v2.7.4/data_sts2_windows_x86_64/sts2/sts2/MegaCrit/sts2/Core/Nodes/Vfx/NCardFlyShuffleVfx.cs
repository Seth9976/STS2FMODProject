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
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.Nodes.GodotExtensions;
using MegaCrit.Sts2.Core.Nodes.Rooms;
using MegaCrit.Sts2.Core.Random;
using MegaCrit.Sts2.Core.TestSupport;

namespace MegaCrit.Sts2.Core.Nodes.Vfx
{
	/// <summary>
	/// Another card soul vfx when we have no card.
	/// Used when the discard pile is shuffled into the draw pile.
	/// </summary>
	// Token: 0x0200018E RID: 398
	[ScriptPath("res://src/Core/Nodes/Vfx/NCardFlyShuffleVfx.cs")]
	public class NCardFlyShuffleVfx : Control
	{
		// Token: 0x170005F6 RID: 1526
		// (get) Token: 0x0600135B RID: 4955 RVA: 0x00074388 File Offset: 0x00072588
		[Nullable(1)]
		public static IEnumerable<string> AssetPaths
		{
			[NullableContext(1)]
			get
			{
				return new <>z__ReadOnlySingleElementList<string>(NCardFlyShuffleVfx._scenePath);
			}
		}

		// Token: 0x0600135C RID: 4956 RVA: 0x00074394 File Offset: 0x00072594
		[NullableContext(1)]
		[return: Nullable(2)]
		public static NCardFlyShuffleVfx Create(CardPile startPile, CardPile targetPile, string trailPath)
		{
			if (TestMode.IsOn)
			{
				return null;
			}
			SfxCmd.PlayCardSwooshSfx(targetPile, startPile);
			NCardFlyShuffleVfx ncardFlyShuffleVfx = PreloadManager.Cache.GetScene(NCardFlyShuffleVfx._scenePath).Instantiate<NCardFlyShuffleVfx>(0L);
			ncardFlyShuffleVfx._startPos = startPile.Type.GetTargetPosition(null);
			ncardFlyShuffleVfx._endPos = targetPile.Type.GetTargetPosition(null);
			ncardFlyShuffleVfx._trailPath = trailPath;
			ncardFlyShuffleVfx._targetPile = targetPile;
			return ncardFlyShuffleVfx;
		}

		// Token: 0x0600135D RID: 4957 RVA: 0x000743FC File Offset: 0x000725FC
		public override void _Ready()
		{
			this._controlPointOffset = Rng.Chaotic.NextFloat(-300f, 400f);
			this._speed = Rng.Chaotic.NextFloat(1.1f, 1.25f);
			this._accel = Rng.Chaotic.NextFloat(2f, 2.5f);
			this._arcDir = ((this._endPos.Y < 540f) ? (-500f) : (500f + this._controlPointOffset));
			this._duration = Rng.Chaotic.NextFloat(1f, 1.75f);
			this._vfx = NCardTrailVfx.Create(this, this._trailPath);
			if (this._vfx != null)
			{
				NCombatRoom.Instance.CombatVfxContainer.AddChildSafely(this._vfx);
			}
			Node parent = base.GetParent();
			parent.MoveChildSafely(this, parent.GetChildCount(false) - 1);
			TaskHelper.RunSafely(this.PlayAnim());
		}

		// Token: 0x0600135E RID: 4958 RVA: 0x000744F0 File Offset: 0x000726F0
		[NullableContext(1)]
		private async Task PlayAnim()
		{
			float time = 0f;
			while (time / this._duration <= 1f)
			{
				await this.AwaitProcessFrame(default(CancellationToken));
				if (this._cancelToken.IsCancellationRequested)
				{
					return;
				}
				float num = (float)base.GetProcessDeltaTime();
				time += this._speed * num;
				this._speed += this._accel * num;
				Vector2 vector = this._startPos + (this._endPos - this._startPos) * 0.5f;
				vector.Y -= this._arcDir;
				base.GlobalPosition = MathHelper.BezierCurve(this._startPos, this._endPos, vector, time / this._duration);
				Vector2 vector2 = MathHelper.BezierCurve(this._startPos, this._endPos, vector, (time + 0.05f) / this._duration);
				base.Rotation = (vector2 - base.GlobalPosition).Angle() + 1.5707964f;
			}
			base.GlobalPosition = this._endPos;
			this._targetPile.InvokeCardAddFinished();
			time = 0f;
			while (time / this._duration <= 1f)
			{
				await this.AwaitProcessFrame(default(CancellationToken));
				if (this._cancelToken.IsCancellationRequested)
				{
					return;
				}
				float num2 = (float)base.GetProcessDeltaTime();
				time += this._speed * num2;
				if (time / this._duration > 0.25f && !this._vfxFading)
				{
					if (this._vfx != null)
					{
						TaskHelper.RunSafely(this._vfx.FadeOut());
					}
					this._vfxFading = true;
				}
				base.Scale = Vector2.One * Mathf.Max(Mathf.Lerp(0.1f, -0.1f, time / this._duration), 0f);
			}
			this._fadeOutTween = base.CreateTween();
			this._fadeOutTween.TweenProperty(this, "modulate:a", 0f, 0.800000011920929);
			bool flag = await this._fadeOutTween.AwaitFinished(this);
			this.QueueFreeSafely();
		}

		// Token: 0x0600135F RID: 4959 RVA: 0x00074533 File Offset: 0x00072733
		public override void _ExitTree()
		{
			Tween fadeOutTween = this._fadeOutTween;
			if (fadeOutTween != null)
			{
				fadeOutTween.Kill();
			}
			this._cancelToken.Cancel();
		}

		/// <summary>
		/// Get the method information for all the methods declared in this class.
		/// This method is used by Godot to register the available methods in the editor.
		/// Do not call this method.
		/// </summary>
		// Token: 0x06001360 RID: 4960 RVA: 0x00074554 File Offset: 0x00072754
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal static List<MethodInfo> GetGodotMethodList()
		{
			return new List<MethodInfo>(2)
			{
				new MethodInfo(NCardFlyShuffleVfx.MethodName._Ready, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NCardFlyShuffleVfx.MethodName._ExitTree, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null)
			};
		}

		/// <inheritdoc />
		// Token: 0x06001361 RID: 4961 RVA: 0x000745C8 File Offset: 0x000727C8
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool InvokeGodotClassMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
		{
			if ((ref method) == NCardFlyShuffleVfx.MethodName._Ready && args.Count == 0)
			{
				this._Ready();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NCardFlyShuffleVfx.MethodName._ExitTree && args.Count == 0)
			{
				this._ExitTree();
				ret = default(godot_variant);
				return true;
			}
			return base.InvokeGodotClassMethod(ref method, args, ref ret);
		}

		/// <inheritdoc />
		// Token: 0x06001362 RID: 4962 RVA: 0x00074628 File Offset: 0x00072828
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool HasGodotClassMethod(in godot_string_name method)
		{
			return (ref method) == NCardFlyShuffleVfx.MethodName._Ready || (ref method) == NCardFlyShuffleVfx.MethodName._ExitTree || base.HasGodotClassMethod(ref method);
		}

		/// <inheritdoc />
		// Token: 0x06001363 RID: 4963 RVA: 0x00074650 File Offset: 0x00072850
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool SetGodotClassPropertyValue(in godot_string_name name, in godot_variant value)
		{
			if ((ref name) == NCardFlyShuffleVfx.PropertyName._vfx)
			{
				this._vfx = VariantUtils.ConvertTo<NCardTrailVfx>(ref value);
				return true;
			}
			if ((ref name) == NCardFlyShuffleVfx.PropertyName._fadeOutTween)
			{
				this._fadeOutTween = VariantUtils.ConvertTo<Tween>(ref value);
				return true;
			}
			if ((ref name) == NCardFlyShuffleVfx.PropertyName._vfxFading)
			{
				this._vfxFading = VariantUtils.ConvertTo<bool>(ref value);
				return true;
			}
			if ((ref name) == NCardFlyShuffleVfx.PropertyName._startPos)
			{
				this._startPos = VariantUtils.ConvertTo<Vector2>(ref value);
				return true;
			}
			if ((ref name) == NCardFlyShuffleVfx.PropertyName._endPos)
			{
				this._endPos = VariantUtils.ConvertTo<Vector2>(ref value);
				return true;
			}
			if ((ref name) == NCardFlyShuffleVfx.PropertyName._controlPointOffset)
			{
				this._controlPointOffset = VariantUtils.ConvertTo<float>(ref value);
				return true;
			}
			if ((ref name) == NCardFlyShuffleVfx.PropertyName._duration)
			{
				this._duration = VariantUtils.ConvertTo<float>(ref value);
				return true;
			}
			if ((ref name) == NCardFlyShuffleVfx.PropertyName._speed)
			{
				this._speed = VariantUtils.ConvertTo<float>(ref value);
				return true;
			}
			if ((ref name) == NCardFlyShuffleVfx.PropertyName._accel)
			{
				this._accel = VariantUtils.ConvertTo<float>(ref value);
				return true;
			}
			if ((ref name) == NCardFlyShuffleVfx.PropertyName._arcDir)
			{
				this._arcDir = VariantUtils.ConvertTo<float>(ref value);
				return true;
			}
			if ((ref name) == NCardFlyShuffleVfx.PropertyName._trailPath)
			{
				this._trailPath = VariantUtils.ConvertTo<string>(ref value);
				return true;
			}
			return base.SetGodotClassPropertyValue(ref name, ref value);
		}

		/// <inheritdoc />
		// Token: 0x06001364 RID: 4964 RVA: 0x00074790 File Offset: 0x00072990
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
		{
			if ((ref name) == NCardFlyShuffleVfx.PropertyName._vfx)
			{
				value = VariantUtils.CreateFrom<NCardTrailVfx>(ref this._vfx);
				return true;
			}
			if ((ref name) == NCardFlyShuffleVfx.PropertyName._fadeOutTween)
			{
				value = VariantUtils.CreateFrom<Tween>(ref this._fadeOutTween);
				return true;
			}
			if ((ref name) == NCardFlyShuffleVfx.PropertyName._vfxFading)
			{
				value = VariantUtils.CreateFrom<bool>(ref this._vfxFading);
				return true;
			}
			if ((ref name) == NCardFlyShuffleVfx.PropertyName._startPos)
			{
				value = VariantUtils.CreateFrom<Vector2>(ref this._startPos);
				return true;
			}
			if ((ref name) == NCardFlyShuffleVfx.PropertyName._endPos)
			{
				value = VariantUtils.CreateFrom<Vector2>(ref this._endPos);
				return true;
			}
			if ((ref name) == NCardFlyShuffleVfx.PropertyName._controlPointOffset)
			{
				value = VariantUtils.CreateFrom<float>(ref this._controlPointOffset);
				return true;
			}
			if ((ref name) == NCardFlyShuffleVfx.PropertyName._duration)
			{
				value = VariantUtils.CreateFrom<float>(ref this._duration);
				return true;
			}
			if ((ref name) == NCardFlyShuffleVfx.PropertyName._speed)
			{
				value = VariantUtils.CreateFrom<float>(ref this._speed);
				return true;
			}
			if ((ref name) == NCardFlyShuffleVfx.PropertyName._accel)
			{
				value = VariantUtils.CreateFrom<float>(ref this._accel);
				return true;
			}
			if ((ref name) == NCardFlyShuffleVfx.PropertyName._arcDir)
			{
				value = VariantUtils.CreateFrom<float>(ref this._arcDir);
				return true;
			}
			if ((ref name) == NCardFlyShuffleVfx.PropertyName._trailPath)
			{
				value = VariantUtils.CreateFrom<string>(ref this._trailPath);
				return true;
			}
			return base.GetGodotClassPropertyValue(ref name, ref value);
		}

		/// <summary>
		/// Get the property information for all the properties declared in this class.
		/// This method is used by Godot to register the available properties in the editor.
		/// Do not call this method.
		/// </summary>
		// Token: 0x06001365 RID: 4965 RVA: 0x00074908 File Offset: 0x00072B08
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal static List<PropertyInfo> GetGodotPropertyList()
		{
			return new List<PropertyInfo>
			{
				new PropertyInfo(24L, NCardFlyShuffleVfx.PropertyName._vfx, 0L, "", 4096L, false),
				new PropertyInfo(24L, NCardFlyShuffleVfx.PropertyName._fadeOutTween, 0L, "", 4096L, false),
				new PropertyInfo(1L, NCardFlyShuffleVfx.PropertyName._vfxFading, 0L, "", 4096L, false),
				new PropertyInfo(5L, NCardFlyShuffleVfx.PropertyName._startPos, 0L, "", 4096L, false),
				new PropertyInfo(5L, NCardFlyShuffleVfx.PropertyName._endPos, 0L, "", 4096L, false),
				new PropertyInfo(3L, NCardFlyShuffleVfx.PropertyName._controlPointOffset, 0L, "", 4096L, false),
				new PropertyInfo(3L, NCardFlyShuffleVfx.PropertyName._duration, 0L, "", 4096L, false),
				new PropertyInfo(3L, NCardFlyShuffleVfx.PropertyName._speed, 0L, "", 4096L, false),
				new PropertyInfo(3L, NCardFlyShuffleVfx.PropertyName._accel, 0L, "", 4096L, false),
				new PropertyInfo(3L, NCardFlyShuffleVfx.PropertyName._arcDir, 0L, "", 4096L, false),
				new PropertyInfo(4L, NCardFlyShuffleVfx.PropertyName._trailPath, 0L, "", 4096L, false)
			};
		}

		/// <inheritdoc />
		// Token: 0x06001366 RID: 4966 RVA: 0x00074A80 File Offset: 0x00072C80
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void SaveGodotObjectData(GodotSerializationInfo info)
		{
			base.SaveGodotObjectData(info);
			info.AddProperty(NCardFlyShuffleVfx.PropertyName._vfx, Variant.From<NCardTrailVfx>(ref this._vfx));
			info.AddProperty(NCardFlyShuffleVfx.PropertyName._fadeOutTween, Variant.From<Tween>(ref this._fadeOutTween));
			info.AddProperty(NCardFlyShuffleVfx.PropertyName._vfxFading, Variant.From<bool>(ref this._vfxFading));
			info.AddProperty(NCardFlyShuffleVfx.PropertyName._startPos, Variant.From<Vector2>(ref this._startPos));
			info.AddProperty(NCardFlyShuffleVfx.PropertyName._endPos, Variant.From<Vector2>(ref this._endPos));
			info.AddProperty(NCardFlyShuffleVfx.PropertyName._controlPointOffset, Variant.From<float>(ref this._controlPointOffset));
			info.AddProperty(NCardFlyShuffleVfx.PropertyName._duration, Variant.From<float>(ref this._duration));
			info.AddProperty(NCardFlyShuffleVfx.PropertyName._speed, Variant.From<float>(ref this._speed));
			info.AddProperty(NCardFlyShuffleVfx.PropertyName._accel, Variant.From<float>(ref this._accel));
			info.AddProperty(NCardFlyShuffleVfx.PropertyName._arcDir, Variant.From<float>(ref this._arcDir));
			info.AddProperty(NCardFlyShuffleVfx.PropertyName._trailPath, Variant.From<string>(ref this._trailPath));
		}

		/// <inheritdoc />
		// Token: 0x06001367 RID: 4967 RVA: 0x00074B88 File Offset: 0x00072D88
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void RestoreGodotObjectData(GodotSerializationInfo info)
		{
			base.RestoreGodotObjectData(info);
			Variant variant;
			if (info.TryGetProperty(NCardFlyShuffleVfx.PropertyName._vfx, ref variant))
			{
				this._vfx = variant.As<NCardTrailVfx>();
			}
			Variant variant2;
			if (info.TryGetProperty(NCardFlyShuffleVfx.PropertyName._fadeOutTween, ref variant2))
			{
				this._fadeOutTween = variant2.As<Tween>();
			}
			Variant variant3;
			if (info.TryGetProperty(NCardFlyShuffleVfx.PropertyName._vfxFading, ref variant3))
			{
				this._vfxFading = variant3.As<bool>();
			}
			Variant variant4;
			if (info.TryGetProperty(NCardFlyShuffleVfx.PropertyName._startPos, ref variant4))
			{
				this._startPos = variant4.As<Vector2>();
			}
			Variant variant5;
			if (info.TryGetProperty(NCardFlyShuffleVfx.PropertyName._endPos, ref variant5))
			{
				this._endPos = variant5.As<Vector2>();
			}
			Variant variant6;
			if (info.TryGetProperty(NCardFlyShuffleVfx.PropertyName._controlPointOffset, ref variant6))
			{
				this._controlPointOffset = variant6.As<float>();
			}
			Variant variant7;
			if (info.TryGetProperty(NCardFlyShuffleVfx.PropertyName._duration, ref variant7))
			{
				this._duration = variant7.As<float>();
			}
			Variant variant8;
			if (info.TryGetProperty(NCardFlyShuffleVfx.PropertyName._speed, ref variant8))
			{
				this._speed = variant8.As<float>();
			}
			Variant variant9;
			if (info.TryGetProperty(NCardFlyShuffleVfx.PropertyName._accel, ref variant9))
			{
				this._accel = variant9.As<float>();
			}
			Variant variant10;
			if (info.TryGetProperty(NCardFlyShuffleVfx.PropertyName._arcDir, ref variant10))
			{
				this._arcDir = variant10.As<float>();
			}
			Variant variant11;
			if (info.TryGetProperty(NCardFlyShuffleVfx.PropertyName._trailPath, ref variant11))
			{
				this._trailPath = variant11.As<string>();
			}
		}

		// Token: 0x040006E0 RID: 1760
		[Nullable(2)]
		private NCardTrailVfx _vfx;

		// Token: 0x040006E1 RID: 1761
		[Nullable(2)]
		private Tween _fadeOutTween;

		// Token: 0x040006E2 RID: 1762
		private bool _vfxFading;

		// Token: 0x040006E3 RID: 1763
		private Vector2 _startPos;

		// Token: 0x040006E4 RID: 1764
		private Vector2 _endPos;

		// Token: 0x040006E5 RID: 1765
		private float _controlPointOffset;

		// Token: 0x040006E6 RID: 1766
		private float _duration;

		// Token: 0x040006E7 RID: 1767
		private float _speed;

		// Token: 0x040006E8 RID: 1768
		private float _accel;

		// Token: 0x040006E9 RID: 1769
		private float _arcDir;

		// Token: 0x040006EA RID: 1770
		[Nullable(1)]
		private string _trailPath;

		// Token: 0x040006EB RID: 1771
		[Nullable(1)]
		private CardPile _targetPile;

		// Token: 0x040006EC RID: 1772
		[Nullable(1)]
		private readonly CancellationTokenSource _cancelToken = new CancellationTokenSource();

		// Token: 0x040006ED RID: 1773
		[Nullable(1)]
		private static readonly string _scenePath = SceneHelper.GetScenePath("vfx/vfx_card_shuffle_fly");

		/// <summary>
		/// Cached StringNames for the methods contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x02000FC1 RID: 4033
		public class MethodName : Control.MethodName
		{
			/// <summary>
			/// Cached name for the '_Ready' method.
			/// </summary>
			// Token: 0x04003EB5 RID: 16053
			public static readonly StringName _Ready = "_Ready";

			/// <summary>
			/// Cached name for the '_ExitTree' method.
			/// </summary>
			// Token: 0x04003EB6 RID: 16054
			public static readonly StringName _ExitTree = "_ExitTree";
		}

		/// <summary>
		/// Cached StringNames for the properties and fields contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x02000FC2 RID: 4034
		public class PropertyName : Control.PropertyName
		{
			/// <summary>
			/// Cached name for the '_vfx' field.
			/// </summary>
			// Token: 0x04003EB7 RID: 16055
			public static readonly StringName _vfx = "_vfx";

			/// <summary>
			/// Cached name for the '_fadeOutTween' field.
			/// </summary>
			// Token: 0x04003EB8 RID: 16056
			public static readonly StringName _fadeOutTween = "_fadeOutTween";

			/// <summary>
			/// Cached name for the '_vfxFading' field.
			/// </summary>
			// Token: 0x04003EB9 RID: 16057
			public static readonly StringName _vfxFading = "_vfxFading";

			/// <summary>
			/// Cached name for the '_startPos' field.
			/// </summary>
			// Token: 0x04003EBA RID: 16058
			public static readonly StringName _startPos = "_startPos";

			/// <summary>
			/// Cached name for the '_endPos' field.
			/// </summary>
			// Token: 0x04003EBB RID: 16059
			public static readonly StringName _endPos = "_endPos";

			/// <summary>
			/// Cached name for the '_controlPointOffset' field.
			/// </summary>
			// Token: 0x04003EBC RID: 16060
			public static readonly StringName _controlPointOffset = "_controlPointOffset";

			/// <summary>
			/// Cached name for the '_duration' field.
			/// </summary>
			// Token: 0x04003EBD RID: 16061
			public static readonly StringName _duration = "_duration";

			/// <summary>
			/// Cached name for the '_speed' field.
			/// </summary>
			// Token: 0x04003EBE RID: 16062
			public static readonly StringName _speed = "_speed";

			/// <summary>
			/// Cached name for the '_accel' field.
			/// </summary>
			// Token: 0x04003EBF RID: 16063
			public static readonly StringName _accel = "_accel";

			/// <summary>
			/// Cached name for the '_arcDir' field.
			/// </summary>
			// Token: 0x04003EC0 RID: 16064
			public static readonly StringName _arcDir = "_arcDir";

			/// <summary>
			/// Cached name for the '_trailPath' field.
			/// </summary>
			// Token: 0x04003EC1 RID: 16065
			public static readonly StringName _trailPath = "_trailPath";
		}

		/// <summary>
		/// Cached StringNames for the signals contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x02000FC3 RID: 4035
		public class SignalName : Control.SignalName
		{
		}
	}
}
