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
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.Nodes.GodotExtensions;

namespace MegaCrit.Sts2.Core.Nodes.Screens.Shops
{
	// Token: 0x0200027E RID: 638
	[GlobalClass]
	[ScriptPath("res://src/Core/Nodes/Screens/Shops/NMerchantHand.cs")]
	public class NMerchantHand : Node
	{
		// Token: 0x060021CE RID: 8654 RVA: 0x000E65E8 File Offset: 0x000E47E8
		public override void _Ready()
		{
			this._parent = base.GetParent<Node2D>();
			this._animController = new MegaSprite(this._parent);
			this._rug = this._parent.GetParent<Control>();
			this._startPos = this._parent.GlobalPosition;
			this._targetPos = this._startPos;
			this._noise = new FastNoiseLite();
			this._noise.NoiseType = 3L;
			this._noise.Frequency = 1f;
			this.RunWhenSpineReady(this._animController, delegate(MegaAnimationState animState)
			{
				MegaSkeleton skeleton = this._animController.GetSkeleton();
				this._bone = ((skeleton != null) ? skeleton.FindBone("rotate_me") : null);
				animState.SetAnimation("default", true, 0);
			});
		}

		// Token: 0x060021CF RID: 8655 RVA: 0x000E6685 File Offset: 0x000E4885
		public override void _ExitTree()
		{
			base._ExitTree();
			CancellationTokenSource stopPointingToken = this._stopPointingToken;
			if (stopPointingToken == null)
			{
				return;
			}
			stopPointingToken.Cancel();
		}

		// Token: 0x060021D0 RID: 8656 RVA: 0x000E66A0 File Offset: 0x000E48A0
		public override void _Process(double delta)
		{
			if (this._targetNode != null && GodotObject.IsInstanceValid(this._targetNode))
			{
				this._targetPos = this._targetNode.GlobalPosition + this._targetOffset;
			}
			this._time += (float)delta;
			float num = this._noise.GetNoise1D(this._time * 0.1f) + 0.4f;
			float num2 = this._noise.GetNoise1D((this._time + 0.25f) * 0.1f) - 0.5f;
			this._parent.GlobalPosition = this._parent.GlobalPosition.Lerp(this._targetPos + new Vector2(num, num2) * 100f, (float)delta * 4f);
			MegaBone bone = this._bone;
			if (bone == null)
			{
				return;
			}
			bone.SetRotation(Mathf.Lerp(-10f, 10f, (this._parent.Position.X - this._rug.Size.X * 0.5f - 50f) * 0.01f));
		}

		// Token: 0x060021D1 RID: 8657 RVA: 0x000E67C3 File Offset: 0x000E49C3
		[NullableContext(1)]
		public void PointAtTarget(Control target, Vector2 offset)
		{
			CancellationTokenSource stopPointingToken = this._stopPointingToken;
			if (stopPointingToken != null)
			{
				stopPointingToken.Cancel();
			}
			this._targetNode = target;
			this._targetOffset = offset;
		}

		// Token: 0x060021D2 RID: 8658 RVA: 0x000E67E4 File Offset: 0x000E49E4
		public void StopPointing(float lingerTime)
		{
			this._targetNode = null;
			CancellationTokenSource stopPointingToken = this._stopPointingToken;
			if (stopPointingToken != null)
			{
				stopPointingToken.Cancel();
			}
			this._stopPointingToken = new CancellationTokenSource();
			TaskHelper.RunSafely(this.WaitAndReturn(this._stopPointingToken, lingerTime));
		}

		// Token: 0x060021D3 RID: 8659 RVA: 0x000E681C File Offset: 0x000E4A1C
		[NullableContext(1)]
		private async Task WaitAndReturn(CancellationTokenSource cancelToken, float lingerTime)
		{
			float num = 0f;
			while (num < lingerTime)
			{
				if (!cancelToken.IsCancellationRequested)
				{
					if (this.IsValid() && base.IsInsideTree())
					{
						float num2 = num;
						float num3 = await this.AwaitProcessFrame(default(CancellationToken));
						num = num2 + num3;
						continue;
					}
				}
				return;
			}
			this._targetPos = this._startPos;
		}

		/// <summary>
		/// Get the method information for all the methods declared in this class.
		/// This method is used by Godot to register the available methods in the editor.
		/// Do not call this method.
		/// </summary>
		// Token: 0x060021D4 RID: 8660 RVA: 0x000E6870 File Offset: 0x000E4A70
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal static List<MethodInfo> GetGodotMethodList()
		{
			return new List<MethodInfo>(5)
			{
				new MethodInfo(NMerchantHand.MethodName._Ready, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NMerchantHand.MethodName._ExitTree, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NMerchantHand.MethodName._Process, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, new List<PropertyInfo>
				{
					new PropertyInfo(3L, "delta", 0L, "", 6L, false)
				}, null),
				new MethodInfo(NMerchantHand.MethodName.PointAtTarget, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, new List<PropertyInfo>
				{
					new PropertyInfo(24L, "target", 0L, "", 6L, new StringName("Control"), false),
					new PropertyInfo(5L, "offset", 0L, "", 6L, false)
				}, null),
				new MethodInfo(NMerchantHand.MethodName.StopPointing, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, new List<PropertyInfo>
				{
					new PropertyInfo(3L, "lingerTime", 0L, "", 6L, false)
				}, null)
			};
		}

		/// <inheritdoc />
		// Token: 0x060021D5 RID: 8661 RVA: 0x000E6A0C File Offset: 0x000E4C0C
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool InvokeGodotClassMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
		{
			if ((ref method) == NMerchantHand.MethodName._Ready && args.Count == 0)
			{
				this._Ready();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NMerchantHand.MethodName._ExitTree && args.Count == 0)
			{
				this._ExitTree();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NMerchantHand.MethodName._Process && args.Count == 1)
			{
				this._Process(VariantUtils.ConvertTo<double>(args[0]));
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NMerchantHand.MethodName.PointAtTarget && args.Count == 2)
			{
				this.PointAtTarget(VariantUtils.ConvertTo<Control>(args[0]), VariantUtils.ConvertTo<Vector2>(args[1]));
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NMerchantHand.MethodName.StopPointing && args.Count == 1)
			{
				this.StopPointing(VariantUtils.ConvertTo<float>(args[0]));
				ret = default(godot_variant);
				return true;
			}
			return base.InvokeGodotClassMethod(ref method, args, ref ret);
		}

		/// <inheritdoc />
		// Token: 0x060021D6 RID: 8662 RVA: 0x000E6B14 File Offset: 0x000E4D14
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool HasGodotClassMethod(in godot_string_name method)
		{
			return (ref method) == NMerchantHand.MethodName._Ready || (ref method) == NMerchantHand.MethodName._ExitTree || (ref method) == NMerchantHand.MethodName._Process || (ref method) == NMerchantHand.MethodName.PointAtTarget || (ref method) == NMerchantHand.MethodName.StopPointing || base.HasGodotClassMethod(ref method);
		}

		/// <inheritdoc />
		// Token: 0x060021D7 RID: 8663 RVA: 0x000E6B74 File Offset: 0x000E4D74
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool SetGodotClassPropertyValue(in godot_string_name name, in godot_variant value)
		{
			if ((ref name) == NMerchantHand.PropertyName._startPos)
			{
				this._startPos = VariantUtils.ConvertTo<Vector2>(ref value);
				return true;
			}
			if ((ref name) == NMerchantHand.PropertyName._targetPos)
			{
				this._targetPos = VariantUtils.ConvertTo<Vector2>(ref value);
				return true;
			}
			if ((ref name) == NMerchantHand.PropertyName._targetNode)
			{
				this._targetNode = VariantUtils.ConvertTo<Control>(ref value);
				return true;
			}
			if ((ref name) == NMerchantHand.PropertyName._targetOffset)
			{
				this._targetOffset = VariantUtils.ConvertTo<Vector2>(ref value);
				return true;
			}
			if ((ref name) == NMerchantHand.PropertyName._noise)
			{
				this._noise = VariantUtils.ConvertTo<FastNoiseLite>(ref value);
				return true;
			}
			if ((ref name) == NMerchantHand.PropertyName._time)
			{
				this._time = VariantUtils.ConvertTo<float>(ref value);
				return true;
			}
			if ((ref name) == NMerchantHand.PropertyName._rug)
			{
				this._rug = VariantUtils.ConvertTo<Control>(ref value);
				return true;
			}
			if ((ref name) == NMerchantHand.PropertyName._parent)
			{
				this._parent = VariantUtils.ConvertTo<Node2D>(ref value);
				return true;
			}
			return base.SetGodotClassPropertyValue(ref name, ref value);
		}

		/// <inheritdoc />
		// Token: 0x060021D8 RID: 8664 RVA: 0x000E6C64 File Offset: 0x000E4E64
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
		{
			if ((ref name) == NMerchantHand.PropertyName._startPos)
			{
				value = VariantUtils.CreateFrom<Vector2>(ref this._startPos);
				return true;
			}
			if ((ref name) == NMerchantHand.PropertyName._targetPos)
			{
				value = VariantUtils.CreateFrom<Vector2>(ref this._targetPos);
				return true;
			}
			if ((ref name) == NMerchantHand.PropertyName._targetNode)
			{
				value = VariantUtils.CreateFrom<Control>(ref this._targetNode);
				return true;
			}
			if ((ref name) == NMerchantHand.PropertyName._targetOffset)
			{
				value = VariantUtils.CreateFrom<Vector2>(ref this._targetOffset);
				return true;
			}
			if ((ref name) == NMerchantHand.PropertyName._noise)
			{
				value = VariantUtils.CreateFrom<FastNoiseLite>(ref this._noise);
				return true;
			}
			if ((ref name) == NMerchantHand.PropertyName._time)
			{
				value = VariantUtils.CreateFrom<float>(ref this._time);
				return true;
			}
			if ((ref name) == NMerchantHand.PropertyName._rug)
			{
				value = VariantUtils.CreateFrom<Control>(ref this._rug);
				return true;
			}
			if ((ref name) == NMerchantHand.PropertyName._parent)
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
		// Token: 0x060021D9 RID: 8665 RVA: 0x000E6D7C File Offset: 0x000E4F7C
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal static List<PropertyInfo> GetGodotPropertyList()
		{
			return new List<PropertyInfo>
			{
				new PropertyInfo(5L, NMerchantHand.PropertyName._startPos, 0L, "", 4096L, false),
				new PropertyInfo(5L, NMerchantHand.PropertyName._targetPos, 0L, "", 4096L, false),
				new PropertyInfo(24L, NMerchantHand.PropertyName._targetNode, 0L, "", 4096L, false),
				new PropertyInfo(5L, NMerchantHand.PropertyName._targetOffset, 0L, "", 4096L, false),
				new PropertyInfo(24L, NMerchantHand.PropertyName._noise, 0L, "", 4096L, false),
				new PropertyInfo(3L, NMerchantHand.PropertyName._time, 0L, "", 4096L, false),
				new PropertyInfo(24L, NMerchantHand.PropertyName._rug, 0L, "", 4096L, false),
				new PropertyInfo(24L, NMerchantHand.PropertyName._parent, 0L, "", 4096L, false)
			};
		}

		/// <inheritdoc />
		// Token: 0x060021DA RID: 8666 RVA: 0x000E6E94 File Offset: 0x000E5094
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void SaveGodotObjectData(GodotSerializationInfo info)
		{
			base.SaveGodotObjectData(info);
			info.AddProperty(NMerchantHand.PropertyName._startPos, Variant.From<Vector2>(ref this._startPos));
			info.AddProperty(NMerchantHand.PropertyName._targetPos, Variant.From<Vector2>(ref this._targetPos));
			info.AddProperty(NMerchantHand.PropertyName._targetNode, Variant.From<Control>(ref this._targetNode));
			info.AddProperty(NMerchantHand.PropertyName._targetOffset, Variant.From<Vector2>(ref this._targetOffset));
			info.AddProperty(NMerchantHand.PropertyName._noise, Variant.From<FastNoiseLite>(ref this._noise));
			info.AddProperty(NMerchantHand.PropertyName._time, Variant.From<float>(ref this._time));
			info.AddProperty(NMerchantHand.PropertyName._rug, Variant.From<Control>(ref this._rug));
			info.AddProperty(NMerchantHand.PropertyName._parent, Variant.From<Node2D>(ref this._parent));
		}

		/// <inheritdoc />
		// Token: 0x060021DB RID: 8667 RVA: 0x000E6F58 File Offset: 0x000E5158
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void RestoreGodotObjectData(GodotSerializationInfo info)
		{
			base.RestoreGodotObjectData(info);
			Variant variant;
			if (info.TryGetProperty(NMerchantHand.PropertyName._startPos, ref variant))
			{
				this._startPos = variant.As<Vector2>();
			}
			Variant variant2;
			if (info.TryGetProperty(NMerchantHand.PropertyName._targetPos, ref variant2))
			{
				this._targetPos = variant2.As<Vector2>();
			}
			Variant variant3;
			if (info.TryGetProperty(NMerchantHand.PropertyName._targetNode, ref variant3))
			{
				this._targetNode = variant3.As<Control>();
			}
			Variant variant4;
			if (info.TryGetProperty(NMerchantHand.PropertyName._targetOffset, ref variant4))
			{
				this._targetOffset = variant4.As<Vector2>();
			}
			Variant variant5;
			if (info.TryGetProperty(NMerchantHand.PropertyName._noise, ref variant5))
			{
				this._noise = variant5.As<FastNoiseLite>();
			}
			Variant variant6;
			if (info.TryGetProperty(NMerchantHand.PropertyName._time, ref variant6))
			{
				this._time = variant6.As<float>();
			}
			Variant variant7;
			if (info.TryGetProperty(NMerchantHand.PropertyName._rug, ref variant7))
			{
				this._rug = variant7.As<Control>();
			}
			Variant variant8;
			if (info.TryGetProperty(NMerchantHand.PropertyName._parent, ref variant8))
			{
				this._parent = variant8.As<Node2D>();
			}
		}

		// Token: 0x04000D31 RID: 3377
		private Vector2 _startPos;

		// Token: 0x04000D32 RID: 3378
		private Vector2 _targetPos;

		// Token: 0x04000D33 RID: 3379
		[Nullable(2)]
		private Control _targetNode;

		// Token: 0x04000D34 RID: 3380
		private Vector2 _targetOffset;

		// Token: 0x04000D35 RID: 3381
		[Nullable(2)]
		private MegaBone _bone;

		// Token: 0x04000D36 RID: 3382
		[Nullable(1)]
		private FastNoiseLite _noise;

		// Token: 0x04000D37 RID: 3383
		private float _time;

		// Token: 0x04000D38 RID: 3384
		[Nullable(2)]
		private CancellationTokenSource _stopPointingToken;

		// Token: 0x04000D39 RID: 3385
		[Nullable(1)]
		private Control _rug;

		// Token: 0x04000D3A RID: 3386
		[Nullable(1)]
		private Node2D _parent;

		// Token: 0x04000D3B RID: 3387
		[Nullable(1)]
		private MegaSprite _animController;

		/// <summary>
		/// Cached StringNames for the methods contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x02001324 RID: 4900
		public class MethodName : Node.MethodName
		{
			/// <summary>
			/// Cached name for the '_Ready' method.
			/// </summary>
			// Token: 0x04004A58 RID: 19032
			public static readonly StringName _Ready = "_Ready";

			/// <summary>
			/// Cached name for the '_ExitTree' method.
			/// </summary>
			// Token: 0x04004A59 RID: 19033
			public static readonly StringName _ExitTree = "_ExitTree";

			/// <summary>
			/// Cached name for the '_Process' method.
			/// </summary>
			// Token: 0x04004A5A RID: 19034
			public static readonly StringName _Process = "_Process";

			/// <summary>
			/// Cached name for the 'PointAtTarget' method.
			/// </summary>
			// Token: 0x04004A5B RID: 19035
			public static readonly StringName PointAtTarget = "PointAtTarget";

			/// <summary>
			/// Cached name for the 'StopPointing' method.
			/// </summary>
			// Token: 0x04004A5C RID: 19036
			public static readonly StringName StopPointing = "StopPointing";
		}

		/// <summary>
		/// Cached StringNames for the properties and fields contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x02001325 RID: 4901
		public class PropertyName : Node.PropertyName
		{
			/// <summary>
			/// Cached name for the '_startPos' field.
			/// </summary>
			// Token: 0x04004A5D RID: 19037
			public static readonly StringName _startPos = "_startPos";

			/// <summary>
			/// Cached name for the '_targetPos' field.
			/// </summary>
			// Token: 0x04004A5E RID: 19038
			public static readonly StringName _targetPos = "_targetPos";

			/// <summary>
			/// Cached name for the '_targetNode' field.
			/// </summary>
			// Token: 0x04004A5F RID: 19039
			public static readonly StringName _targetNode = "_targetNode";

			/// <summary>
			/// Cached name for the '_targetOffset' field.
			/// </summary>
			// Token: 0x04004A60 RID: 19040
			public static readonly StringName _targetOffset = "_targetOffset";

			/// <summary>
			/// Cached name for the '_noise' field.
			/// </summary>
			// Token: 0x04004A61 RID: 19041
			public static readonly StringName _noise = "_noise";

			/// <summary>
			/// Cached name for the '_time' field.
			/// </summary>
			// Token: 0x04004A62 RID: 19042
			public static readonly StringName _time = "_time";

			/// <summary>
			/// Cached name for the '_rug' field.
			/// </summary>
			// Token: 0x04004A63 RID: 19043
			public static readonly StringName _rug = "_rug";

			/// <summary>
			/// Cached name for the '_parent' field.
			/// </summary>
			// Token: 0x04004A64 RID: 19044
			public static readonly StringName _parent = "_parent";
		}

		/// <summary>
		/// Cached StringNames for the signals contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x02001326 RID: 4902
		public class SignalName : Node.SignalName
		{
		}
	}
}
