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
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.Nodes.Cards;
using MegaCrit.Sts2.Core.Nodes.GodotExtensions;
using MegaCrit.Sts2.Core.Nodes.Vfx.Utilities;
using MegaCrit.Sts2.Core.TestSupport;

namespace MegaCrit.Sts2.Core.Nodes.Vfx.Cards
{
	// Token: 0x02000232 RID: 562
	[NullableContext(1)]
	[Nullable(0)]
	[ScriptPath("res://src/Core/Nodes/Vfx/Cards/NCardExhaustVfx.cs")]
	public class NCardExhaustVfx : Control
	{
		// Token: 0x06001C86 RID: 7302 RVA: 0x000B9080 File Offset: 0x000B7280
		[return: Nullable(2)]
		public static NCardExhaustVfx Create(NCard cardNode)
		{
			if (TestMode.IsOn)
			{
				return null;
			}
			NCardExhaustVfx ncardExhaustVfx = PreloadManager.Cache.GetScene(NCardExhaustVfx.scenePath).Instantiate<NCardExhaustVfx>(0L);
			ncardExhaustVfx.SetParticlesPlaying(false);
			ncardExhaustVfx.SetProgress(0f);
			ncardExhaustVfx._position = cardNode.GlobalPosition;
			Node parent = cardNode.GetParent();
			if (parent != null)
			{
				parent.RemoveChildSafely(cardNode);
			}
			ncardExhaustVfx._cardParentContainer.AddChildSafely(cardNode);
			ncardExhaustVfx._cardNode = cardNode;
			return ncardExhaustVfx;
		}

		// Token: 0x06001C87 RID: 7303 RVA: 0x000B90F0 File Offset: 0x000B72F0
		private void SetParticlesPlaying(bool isPlaying)
		{
			this._particlesContainer.SetEmitting(isPlaying);
		}

		// Token: 0x06001C88 RID: 7304 RVA: 0x000B9100 File Offset: 0x000B7300
		private void SetProgress(float progress)
		{
			float num = this._exhaustCurve.Sample(progress);
			float num2 = Mathf.Lerp(this._erosionBaseRange.X, this._erosionBaseRange.Y, num);
			float num3 = Mathf.Lerp(this._particleHeightRange.X, this._particleHeightRange.Y, num);
			this._materialContainer.Set(NCardExhaustVfx._erosionBaseParameter, num2);
			this._particlesContainer.Position = new Vector2(0f, num3);
		}

		// Token: 0x06001C89 RID: 7305 RVA: 0x000B9180 File Offset: 0x000B7380
		public async Task PlayAnimation()
		{
			base.GlobalPosition = this._position;
			this._cardNode.Position = this._cardParentContainer.Size / 2f;
			this._materialContainer.SelfModulate = new Color(1f, 1f, 1f, 1f);
			this.SetParticlesPlaying(true);
			this.SetProgress(0f);
			this._materialContainer.Set(NCardExhaustVfx._erosionOffsetParameter, GD.Randf());
			float num3;
			float num4;
			for (float num = 0f; num < this._exhaustDuration; num = num3 + num4)
			{
				float num2 = num / this._exhaustDuration;
				this.SetProgress(num2);
				num3 = num;
				num4 = await this.AwaitProcessFrame(default(CancellationToken));
			}
			this.SetProgress(1f);
			this.SetParticlesPlaying(false);
			this._materialContainer.SelfModulate = new Color(1f, 1f, 1f, 0f);
			TaskHelper.RunSafely(this.DelayedFree());
		}

		// Token: 0x06001C8A RID: 7306 RVA: 0x000B91C4 File Offset: 0x000B73C4
		private async Task DelayedFree()
		{
			await Cmd.Wait(2f, false);
			this._cardNode.QueueFreeSafely();
			this.QueueFreeSafely();
		}

		// Token: 0x06001C8B RID: 7307 RVA: 0x000B9207 File Offset: 0x000B7407
		public override void _ExitTree()
		{
			if (GodotObject.IsInstanceValid(this._cardNode) && base.IsAncestorOf(this._cardNode))
			{
				this._cardNode.QueueFreeSafely();
			}
		}

		/// <summary>
		/// Get the method information for all the methods declared in this class.
		/// This method is used by Godot to register the available methods in the editor.
		/// Do not call this method.
		/// </summary>
		// Token: 0x06001C8C RID: 7308 RVA: 0x000B9230 File Offset: 0x000B7430
		[NullableContext(0)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal static List<MethodInfo> GetGodotMethodList()
		{
			return new List<MethodInfo>(4)
			{
				new MethodInfo(NCardExhaustVfx.MethodName.Create, new PropertyInfo(24L, "", 0L, "", 6L, new StringName("Control"), false), 33L, new List<PropertyInfo>
				{
					new PropertyInfo(24L, "cardNode", 0L, "", 6L, new StringName("Control"), false)
				}, null),
				new MethodInfo(NCardExhaustVfx.MethodName.SetParticlesPlaying, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, new List<PropertyInfo>
				{
					new PropertyInfo(1L, "isPlaying", 0L, "", 6L, false)
				}, null),
				new MethodInfo(NCardExhaustVfx.MethodName.SetProgress, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, new List<PropertyInfo>
				{
					new PropertyInfo(3L, "progress", 0L, "", 6L, false)
				}, null),
				new MethodInfo(NCardExhaustVfx.MethodName._ExitTree, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null)
			};
		}

		/// <inheritdoc />
		// Token: 0x06001C8D RID: 7309 RVA: 0x000B9388 File Offset: 0x000B7588
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool InvokeGodotClassMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
		{
			if ((ref method) == NCardExhaustVfx.MethodName.Create && args.Count == 1)
			{
				NCardExhaustVfx ncardExhaustVfx = NCardExhaustVfx.Create(VariantUtils.ConvertTo<NCard>(args[0]));
				ret = VariantUtils.CreateFrom<NCardExhaustVfx>(ref ncardExhaustVfx);
				return true;
			}
			if ((ref method) == NCardExhaustVfx.MethodName.SetParticlesPlaying && args.Count == 1)
			{
				this.SetParticlesPlaying(VariantUtils.ConvertTo<bool>(args[0]));
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NCardExhaustVfx.MethodName.SetProgress && args.Count == 1)
			{
				this.SetProgress(VariantUtils.ConvertTo<float>(args[0]));
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NCardExhaustVfx.MethodName._ExitTree && args.Count == 0)
			{
				this._ExitTree();
				ret = default(godot_variant);
				return true;
			}
			return base.InvokeGodotClassMethod(ref method, args, ref ret);
		}

		// Token: 0x06001C8E RID: 7310 RVA: 0x000B9464 File Offset: 0x000B7664
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal static bool InvokeGodotClassStaticMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
		{
			if ((ref method) == NCardExhaustVfx.MethodName.Create && args.Count == 1)
			{
				NCardExhaustVfx ncardExhaustVfx = NCardExhaustVfx.Create(VariantUtils.ConvertTo<NCard>(args[0]));
				ret = VariantUtils.CreateFrom<NCardExhaustVfx>(ref ncardExhaustVfx);
				return true;
			}
			ret = default(godot_variant);
			return false;
		}

		/// <inheritdoc />
		// Token: 0x06001C8F RID: 7311 RVA: 0x000B94B4 File Offset: 0x000B76B4
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool HasGodotClassMethod(in godot_string_name method)
		{
			return (ref method) == NCardExhaustVfx.MethodName.Create || (ref method) == NCardExhaustVfx.MethodName.SetParticlesPlaying || (ref method) == NCardExhaustVfx.MethodName.SetProgress || (ref method) == NCardExhaustVfx.MethodName._ExitTree || base.HasGodotClassMethod(ref method);
		}

		/// <inheritdoc />
		// Token: 0x06001C90 RID: 7312 RVA: 0x000B9504 File Offset: 0x000B7704
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool SetGodotClassPropertyValue(in godot_string_name name, in godot_variant value)
		{
			if ((ref name) == NCardExhaustVfx.PropertyName._cardParentContainer)
			{
				this._cardParentContainer = VariantUtils.ConvertTo<Control>(ref value);
				return true;
			}
			if ((ref name) == NCardExhaustVfx.PropertyName._materialContainer)
			{
				this._materialContainer = VariantUtils.ConvertTo<Control>(ref value);
				return true;
			}
			if ((ref name) == NCardExhaustVfx.PropertyName._particlesContainer)
			{
				this._particlesContainer = VariantUtils.ConvertTo<NParticlesContainer>(ref value);
				return true;
			}
			if ((ref name) == NCardExhaustVfx.PropertyName._exhaustDuration)
			{
				this._exhaustDuration = VariantUtils.ConvertTo<float>(ref value);
				return true;
			}
			if ((ref name) == NCardExhaustVfx.PropertyName._exhaustCurve)
			{
				this._exhaustCurve = VariantUtils.ConvertTo<Curve>(ref value);
				return true;
			}
			if ((ref name) == NCardExhaustVfx.PropertyName._erosionBaseRange)
			{
				this._erosionBaseRange = VariantUtils.ConvertTo<Vector2>(ref value);
				return true;
			}
			if ((ref name) == NCardExhaustVfx.PropertyName._particleHeightRange)
			{
				this._particleHeightRange = VariantUtils.ConvertTo<Vector2>(ref value);
				return true;
			}
			if ((ref name) == NCardExhaustVfx.PropertyName._cardNode)
			{
				this._cardNode = VariantUtils.ConvertTo<NCard>(ref value);
				return true;
			}
			if ((ref name) == NCardExhaustVfx.PropertyName._position)
			{
				this._position = VariantUtils.ConvertTo<Vector2>(ref value);
				return true;
			}
			return base.SetGodotClassPropertyValue(ref name, ref value);
		}

		/// <inheritdoc />
		// Token: 0x06001C91 RID: 7313 RVA: 0x000B960C File Offset: 0x000B780C
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
		{
			if ((ref name) == NCardExhaustVfx.PropertyName._cardParentContainer)
			{
				value = VariantUtils.CreateFrom<Control>(ref this._cardParentContainer);
				return true;
			}
			if ((ref name) == NCardExhaustVfx.PropertyName._materialContainer)
			{
				value = VariantUtils.CreateFrom<Control>(ref this._materialContainer);
				return true;
			}
			if ((ref name) == NCardExhaustVfx.PropertyName._particlesContainer)
			{
				value = VariantUtils.CreateFrom<NParticlesContainer>(ref this._particlesContainer);
				return true;
			}
			if ((ref name) == NCardExhaustVfx.PropertyName._exhaustDuration)
			{
				value = VariantUtils.CreateFrom<float>(ref this._exhaustDuration);
				return true;
			}
			if ((ref name) == NCardExhaustVfx.PropertyName._exhaustCurve)
			{
				value = VariantUtils.CreateFrom<Curve>(ref this._exhaustCurve);
				return true;
			}
			if ((ref name) == NCardExhaustVfx.PropertyName._erosionBaseRange)
			{
				value = VariantUtils.CreateFrom<Vector2>(ref this._erosionBaseRange);
				return true;
			}
			if ((ref name) == NCardExhaustVfx.PropertyName._particleHeightRange)
			{
				value = VariantUtils.CreateFrom<Vector2>(ref this._particleHeightRange);
				return true;
			}
			if ((ref name) == NCardExhaustVfx.PropertyName._cardNode)
			{
				value = VariantUtils.CreateFrom<NCard>(ref this._cardNode);
				return true;
			}
			if ((ref name) == NCardExhaustVfx.PropertyName._position)
			{
				value = VariantUtils.CreateFrom<Vector2>(ref this._position);
				return true;
			}
			return base.GetGodotClassPropertyValue(ref name, ref value);
		}

		/// <summary>
		/// Get the property information for all the properties declared in this class.
		/// This method is used by Godot to register the available properties in the editor.
		/// Do not call this method.
		/// </summary>
		// Token: 0x06001C92 RID: 7314 RVA: 0x000B9744 File Offset: 0x000B7944
		[NullableContext(0)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal static List<PropertyInfo> GetGodotPropertyList()
		{
			return new List<PropertyInfo>
			{
				new PropertyInfo(24L, NCardExhaustVfx.PropertyName._cardParentContainer, 34L, "Control", 4102L, true),
				new PropertyInfo(24L, NCardExhaustVfx.PropertyName._materialContainer, 34L, "Control", 4102L, true),
				new PropertyInfo(24L, NCardExhaustVfx.PropertyName._particlesContainer, 34L, "Node2D", 4102L, true),
				new PropertyInfo(3L, NCardExhaustVfx.PropertyName._exhaustDuration, 0L, "", 4102L, true),
				new PropertyInfo(24L, NCardExhaustVfx.PropertyName._exhaustCurve, 17L, "Curve", 4102L, true),
				new PropertyInfo(5L, NCardExhaustVfx.PropertyName._erosionBaseRange, 0L, "", 4102L, true),
				new PropertyInfo(5L, NCardExhaustVfx.PropertyName._particleHeightRange, 0L, "", 4102L, true),
				new PropertyInfo(24L, NCardExhaustVfx.PropertyName._cardNode, 0L, "", 4096L, false),
				new PropertyInfo(5L, NCardExhaustVfx.PropertyName._position, 0L, "", 4096L, false)
			};
		}

		/// <inheritdoc />
		// Token: 0x06001C93 RID: 7315 RVA: 0x000B9884 File Offset: 0x000B7A84
		[NullableContext(0)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void SaveGodotObjectData(GodotSerializationInfo info)
		{
			base.SaveGodotObjectData(info);
			info.AddProperty(NCardExhaustVfx.PropertyName._cardParentContainer, Variant.From<Control>(ref this._cardParentContainer));
			info.AddProperty(NCardExhaustVfx.PropertyName._materialContainer, Variant.From<Control>(ref this._materialContainer));
			info.AddProperty(NCardExhaustVfx.PropertyName._particlesContainer, Variant.From<NParticlesContainer>(ref this._particlesContainer));
			info.AddProperty(NCardExhaustVfx.PropertyName._exhaustDuration, Variant.From<float>(ref this._exhaustDuration));
			info.AddProperty(NCardExhaustVfx.PropertyName._exhaustCurve, Variant.From<Curve>(ref this._exhaustCurve));
			info.AddProperty(NCardExhaustVfx.PropertyName._erosionBaseRange, Variant.From<Vector2>(ref this._erosionBaseRange));
			info.AddProperty(NCardExhaustVfx.PropertyName._particleHeightRange, Variant.From<Vector2>(ref this._particleHeightRange));
			info.AddProperty(NCardExhaustVfx.PropertyName._cardNode, Variant.From<NCard>(ref this._cardNode));
			info.AddProperty(NCardExhaustVfx.PropertyName._position, Variant.From<Vector2>(ref this._position));
		}

		/// <inheritdoc />
		// Token: 0x06001C94 RID: 7316 RVA: 0x000B9960 File Offset: 0x000B7B60
		[NullableContext(0)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void RestoreGodotObjectData(GodotSerializationInfo info)
		{
			base.RestoreGodotObjectData(info);
			Variant variant;
			if (info.TryGetProperty(NCardExhaustVfx.PropertyName._cardParentContainer, ref variant))
			{
				this._cardParentContainer = variant.As<Control>();
			}
			Variant variant2;
			if (info.TryGetProperty(NCardExhaustVfx.PropertyName._materialContainer, ref variant2))
			{
				this._materialContainer = variant2.As<Control>();
			}
			Variant variant3;
			if (info.TryGetProperty(NCardExhaustVfx.PropertyName._particlesContainer, ref variant3))
			{
				this._particlesContainer = variant3.As<NParticlesContainer>();
			}
			Variant variant4;
			if (info.TryGetProperty(NCardExhaustVfx.PropertyName._exhaustDuration, ref variant4))
			{
				this._exhaustDuration = variant4.As<float>();
			}
			Variant variant5;
			if (info.TryGetProperty(NCardExhaustVfx.PropertyName._exhaustCurve, ref variant5))
			{
				this._exhaustCurve = variant5.As<Curve>();
			}
			Variant variant6;
			if (info.TryGetProperty(NCardExhaustVfx.PropertyName._erosionBaseRange, ref variant6))
			{
				this._erosionBaseRange = variant6.As<Vector2>();
			}
			Variant variant7;
			if (info.TryGetProperty(NCardExhaustVfx.PropertyName._particleHeightRange, ref variant7))
			{
				this._particleHeightRange = variant7.As<Vector2>();
			}
			Variant variant8;
			if (info.TryGetProperty(NCardExhaustVfx.PropertyName._cardNode, ref variant8))
			{
				this._cardNode = variant8.As<NCard>();
			}
			Variant variant9;
			if (info.TryGetProperty(NCardExhaustVfx.PropertyName._position, ref variant9))
			{
				this._position = variant9.As<Vector2>();
			}
		}

		// Token: 0x04000AE5 RID: 2789
		public static readonly string scenePath = SceneHelper.GetScenePath("vfx/ui/card/vfx_card_exhaust");

		// Token: 0x04000AE6 RID: 2790
		[Export(/*Failed to decode CustomAttribute blob!*/)]
		private Control _cardParentContainer;

		// Token: 0x04000AE7 RID: 2791
		[Export(/*Failed to decode CustomAttribute blob!*/)]
		private Control _materialContainer;

		// Token: 0x04000AE8 RID: 2792
		[Export(/*Failed to decode CustomAttribute blob!*/)]
		private NParticlesContainer _particlesContainer;

		// Token: 0x04000AE9 RID: 2793
		[Export(/*Failed to decode CustomAttribute blob!*/)]
		private float _exhaustDuration = 0.4f;

		// Token: 0x04000AEA RID: 2794
		[Export(/*Failed to decode CustomAttribute blob!*/)]
		private Curve _exhaustCurve;

		// Token: 0x04000AEB RID: 2795
		[Export(/*Failed to decode CustomAttribute blob!*/)]
		private Vector2 _erosionBaseRange;

		// Token: 0x04000AEC RID: 2796
		[Export(/*Failed to decode CustomAttribute blob!*/)]
		private Vector2 _particleHeightRange;

		// Token: 0x04000AED RID: 2797
		private NCard _cardNode;

		// Token: 0x04000AEE RID: 2798
		private Vector2 _position;

		// Token: 0x04000AEF RID: 2799
		private static readonly StringName _erosionBaseParameter = new StringName("instance_shader_parameters/erosion_base");

		// Token: 0x04000AF0 RID: 2800
		private static readonly StringName _erosionOffsetParameter = new StringName("instance_shader_parameters/erosion_texture_x_offset");

		/// <summary>
		/// Cached StringNames for the methods contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x020011F4 RID: 4596
		[NullableContext(0)]
		public class MethodName : Control.MethodName
		{
			/// <summary>
			/// Cached name for the 'Create' method.
			/// </summary>
			// Token: 0x0400459F RID: 17823
			public static readonly StringName Create = "Create";

			/// <summary>
			/// Cached name for the 'SetParticlesPlaying' method.
			/// </summary>
			// Token: 0x040045A0 RID: 17824
			public static readonly StringName SetParticlesPlaying = "SetParticlesPlaying";

			/// <summary>
			/// Cached name for the 'SetProgress' method.
			/// </summary>
			// Token: 0x040045A1 RID: 17825
			public static readonly StringName SetProgress = "SetProgress";

			/// <summary>
			/// Cached name for the '_ExitTree' method.
			/// </summary>
			// Token: 0x040045A2 RID: 17826
			public static readonly StringName _ExitTree = "_ExitTree";
		}

		/// <summary>
		/// Cached StringNames for the properties and fields contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x020011F5 RID: 4597
		[NullableContext(0)]
		public class PropertyName : Control.PropertyName
		{
			/// <summary>
			/// Cached name for the '_cardParentContainer' field.
			/// </summary>
			// Token: 0x040045A3 RID: 17827
			public static readonly StringName _cardParentContainer = "_cardParentContainer";

			/// <summary>
			/// Cached name for the '_materialContainer' field.
			/// </summary>
			// Token: 0x040045A4 RID: 17828
			public static readonly StringName _materialContainer = "_materialContainer";

			/// <summary>
			/// Cached name for the '_particlesContainer' field.
			/// </summary>
			// Token: 0x040045A5 RID: 17829
			public static readonly StringName _particlesContainer = "_particlesContainer";

			/// <summary>
			/// Cached name for the '_exhaustDuration' field.
			/// </summary>
			// Token: 0x040045A6 RID: 17830
			public static readonly StringName _exhaustDuration = "_exhaustDuration";

			/// <summary>
			/// Cached name for the '_exhaustCurve' field.
			/// </summary>
			// Token: 0x040045A7 RID: 17831
			public static readonly StringName _exhaustCurve = "_exhaustCurve";

			/// <summary>
			/// Cached name for the '_erosionBaseRange' field.
			/// </summary>
			// Token: 0x040045A8 RID: 17832
			public static readonly StringName _erosionBaseRange = "_erosionBaseRange";

			/// <summary>
			/// Cached name for the '_particleHeightRange' field.
			/// </summary>
			// Token: 0x040045A9 RID: 17833
			public static readonly StringName _particleHeightRange = "_particleHeightRange";

			/// <summary>
			/// Cached name for the '_cardNode' field.
			/// </summary>
			// Token: 0x040045AA RID: 17834
			public static readonly StringName _cardNode = "_cardNode";

			/// <summary>
			/// Cached name for the '_position' field.
			/// </summary>
			// Token: 0x040045AB RID: 17835
			public static readonly StringName _position = "_position";
		}

		/// <summary>
		/// Cached StringNames for the signals contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x020011F6 RID: 4598
		[NullableContext(0)]
		public class SignalName : Control.SignalName
		{
		}
	}
}
