using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Godot;
using Godot.Bridge;
using Godot.NativeInterop;

namespace MegaCrit.Sts2.Core.Nodes.CommonUi
{
	/// <summary>
	/// A card preview container intended to be used when lots and lots of cards are previewed and the player isn't
	/// meant to be able to parse every single card. Example usage in the Reflections event
	/// </summary>
	// Token: 0x020003E4 RID: 996
	[ScriptPath("res://src/Core/Nodes/CommonUi/NMessyCardPreviewContainer.cs")]
	public class NMessyCardPreviewContainer : Control
	{
		// Token: 0x06003CA7 RID: 15527 RVA: 0x001D0442 File Offset: 0x001CE642
		public override void _Ready()
		{
			base.Connect(Node.SignalName.ChildEnteredTree, Callable.From<Node>(new Action<Node>(this.PositionNewChild)), 0U);
		}

		// Token: 0x06003CA8 RID: 15528 RVA: 0x001D0464 File Offset: 0x001CE664
		[NullableContext(1)]
		private void PositionNewChild(Node node)
		{
			if (Time.GetTicksMsec() > this._resetNewCardTimer)
			{
				this._currentMaxPosition = 0f;
				this.ResetSamples();
			}
			this._resetNewCardTimer = Time.GetTicksMsec() + 2000UL;
			if (!this._samples.MoveNext())
			{
				this.ResetSamples();
			}
			Vector2 vector = this._samples.Current;
			Control control = node as Control;
			if (control != null)
			{
				control.Position = vector;
				return;
			}
			Node2D node2D = node as Node2D;
			if (node2D != null)
			{
				node2D.Position = vector;
			}
		}

		// Token: 0x06003CA9 RID: 15529 RVA: 0x001D04E4 File Offset: 0x001CE6E4
		private void ResetSamples()
		{
			NMessyCardPreviewContainer.PoissonDiscSampler poissonDiscSampler = new NMessyCardPreviewContainer.PoissonDiscSampler(base.Size.X, base.Size.Y, 150f);
			this._samples = poissonDiscSampler.Samples();
		}

		/// <summary>
		/// Get the method information for all the methods declared in this class.
		/// This method is used by Godot to register the available methods in the editor.
		/// Do not call this method.
		/// </summary>
		// Token: 0x06003CAA RID: 15530 RVA: 0x001D0520 File Offset: 0x001CE720
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal static List<MethodInfo> GetGodotMethodList()
		{
			return new List<MethodInfo>(3)
			{
				new MethodInfo(NMessyCardPreviewContainer.MethodName._Ready, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NMessyCardPreviewContainer.MethodName.PositionNewChild, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, new List<PropertyInfo>
				{
					new PropertyInfo(24L, "node", 0L, "", 6L, new StringName("Node"), false)
				}, null),
				new MethodInfo(NMessyCardPreviewContainer.MethodName.ResetSamples, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null)
			};
		}

		/// <inheritdoc />
		// Token: 0x06003CAB RID: 15531 RVA: 0x001D05F4 File Offset: 0x001CE7F4
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool InvokeGodotClassMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
		{
			if ((ref method) == NMessyCardPreviewContainer.MethodName._Ready && args.Count == 0)
			{
				this._Ready();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NMessyCardPreviewContainer.MethodName.PositionNewChild && args.Count == 1)
			{
				this.PositionNewChild(VariantUtils.ConvertTo<Node>(args[0]));
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NMessyCardPreviewContainer.MethodName.ResetSamples && args.Count == 0)
			{
				this.ResetSamples();
				ret = default(godot_variant);
				return true;
			}
			return base.InvokeGodotClassMethod(ref method, args, ref ret);
		}

		/// <inheritdoc />
		// Token: 0x06003CAC RID: 15532 RVA: 0x001D0687 File Offset: 0x001CE887
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool HasGodotClassMethod(in godot_string_name method)
		{
			return (ref method) == NMessyCardPreviewContainer.MethodName._Ready || (ref method) == NMessyCardPreviewContainer.MethodName.PositionNewChild || (ref method) == NMessyCardPreviewContainer.MethodName.ResetSamples || base.HasGodotClassMethod(ref method);
		}

		/// <inheritdoc />
		// Token: 0x06003CAD RID: 15533 RVA: 0x001D06BD File Offset: 0x001CE8BD
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool SetGodotClassPropertyValue(in godot_string_name name, in godot_variant value)
		{
			if ((ref name) == NMessyCardPreviewContainer.PropertyName._resetNewCardTimer)
			{
				this._resetNewCardTimer = VariantUtils.ConvertTo<ulong>(ref value);
				return true;
			}
			if ((ref name) == NMessyCardPreviewContainer.PropertyName._currentMaxPosition)
			{
				this._currentMaxPosition = VariantUtils.ConvertTo<float>(ref value);
				return true;
			}
			return base.SetGodotClassPropertyValue(ref name, ref value);
		}

		/// <inheritdoc />
		// Token: 0x06003CAE RID: 15534 RVA: 0x001D0700 File Offset: 0x001CE900
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
		{
			if ((ref name) == NMessyCardPreviewContainer.PropertyName._resetNewCardTimer)
			{
				value = VariantUtils.CreateFrom<ulong>(ref this._resetNewCardTimer);
				return true;
			}
			if ((ref name) == NMessyCardPreviewContainer.PropertyName._currentMaxPosition)
			{
				value = VariantUtils.CreateFrom<float>(ref this._currentMaxPosition);
				return true;
			}
			return base.GetGodotClassPropertyValue(ref name, ref value);
		}

		/// <summary>
		/// Get the property information for all the properties declared in this class.
		/// This method is used by Godot to register the available properties in the editor.
		/// Do not call this method.
		/// </summary>
		// Token: 0x06003CAF RID: 15535 RVA: 0x001D0758 File Offset: 0x001CE958
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal static List<PropertyInfo> GetGodotPropertyList()
		{
			return new List<PropertyInfo>
			{
				new PropertyInfo(2L, NMessyCardPreviewContainer.PropertyName._resetNewCardTimer, 0L, "", 4096L, false),
				new PropertyInfo(3L, NMessyCardPreviewContainer.PropertyName._currentMaxPosition, 0L, "", 4096L, false)
			};
		}

		/// <inheritdoc />
		// Token: 0x06003CB0 RID: 15536 RVA: 0x001D07AC File Offset: 0x001CE9AC
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void SaveGodotObjectData(GodotSerializationInfo info)
		{
			base.SaveGodotObjectData(info);
			info.AddProperty(NMessyCardPreviewContainer.PropertyName._resetNewCardTimer, Variant.From<ulong>(ref this._resetNewCardTimer));
			info.AddProperty(NMessyCardPreviewContainer.PropertyName._currentMaxPosition, Variant.From<float>(ref this._currentMaxPosition));
		}

		/// <inheritdoc />
		// Token: 0x06003CB1 RID: 15537 RVA: 0x001D07E4 File Offset: 0x001CE9E4
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void RestoreGodotObjectData(GodotSerializationInfo info)
		{
			base.RestoreGodotObjectData(info);
			Variant variant;
			if (info.TryGetProperty(NMessyCardPreviewContainer.PropertyName._resetNewCardTimer, ref variant))
			{
				this._resetNewCardTimer = variant.As<ulong>();
			}
			Variant variant2;
			if (info.TryGetProperty(NMessyCardPreviewContainer.PropertyName._currentMaxPosition, ref variant2))
			{
				this._currentMaxPosition = variant2.As<float>();
			}
		}

		// Token: 0x040017A3 RID: 6051
		private const float _spacing = 150f;

		// Token: 0x040017A4 RID: 6052
		private const int _resetNewCardMsec = 2000;

		// Token: 0x040017A5 RID: 6053
		private ulong _resetNewCardTimer;

		// Token: 0x040017A6 RID: 6054
		private float _currentMaxPosition;

		// Token: 0x040017A7 RID: 6055
		[Nullable(2)]
		private IEnumerator<Vector2> _samples;

		// Token: 0x02001861 RID: 6241
		[NullableContext(1)]
		[Nullable(0)]
		public class PoissonDiscSampler
		{
			/// Create a sampler with the following parameters:
			///
			/// width:  each sample's x coordinate will be between [0, width]
			/// height: each sample's y coordinate will be between [0, height]
			/// radius: each sample will be at least `radius` units away from any other sample, and at most 2 * `radius`.
			// Token: 0x0600A88F RID: 43151 RVA: 0x0035526C File Offset: 0x0035346C
			public PoissonDiscSampler(float width, float height, float radius)
			{
				this._rect = new Rect2(0f, 0f, width, height);
				this._radius2 = radius * radius;
				this._cellSize = radius / Mathf.Sqrt(2f);
				this._grid = new Vector2[Mathf.CeilToInt(width / this._cellSize), Mathf.CeilToInt(height / this._cellSize)];
			}

			/// Return a lazy sequence of samples.
			// Token: 0x0600A890 RID: 43152 RVA: 0x003552E1 File Offset: 0x003534E1
			public IEnumerator<Vector2> Samples()
			{
				NMessyCardPreviewContainer.PoissonDiscSampler.<Samples>d__8 <Samples>d__ = new NMessyCardPreviewContainer.PoissonDiscSampler.<Samples>d__8(0);
				<Samples>d__.<>4__this = this;
				return <Samples>d__;
			}

			// Token: 0x0600A891 RID: 43153 RVA: 0x003552F0 File Offset: 0x003534F0
			private bool IsFarEnough(Vector2 sample)
			{
				NMessyCardPreviewContainer.PoissonDiscSampler.GridPos gridPos = new NMessyCardPreviewContainer.PoissonDiscSampler.GridPos(sample, this._cellSize);
				int num = Mathf.Max(gridPos.x - 2, 0);
				int num2 = Mathf.Max(gridPos.y - 2, 0);
				int num3 = Mathf.Min(gridPos.x + 2, this._grid.GetLength(0) - 1);
				int num4 = Mathf.Min(gridPos.y + 2, this._grid.GetLength(1) - 1);
				for (int i = num2; i <= num4; i++)
				{
					for (int j = num; j <= num3; j++)
					{
						Vector2 vector = this._grid[j, i];
						if (vector != Vector2.Zero)
						{
							Vector2 vector2 = vector - sample;
							if (vector2.X * vector2.X + vector2.Y * vector2.Y < this._radius2)
							{
								return false;
							}
						}
					}
				}
				return true;
			}

			/// Adds the sample to the active samples queue and the grid before returning it
			// Token: 0x0600A892 RID: 43154 RVA: 0x003553D8 File Offset: 0x003535D8
			private Vector2 AddSample(Vector2 sample)
			{
				this._activeSamples.Add(sample);
				NMessyCardPreviewContainer.PoissonDiscSampler.GridPos gridPos = new NMessyCardPreviewContainer.PoissonDiscSampler.GridPos(sample, this._cellSize);
				this._grid[gridPos.x, gridPos.y] = sample;
				return sample;
			}

			// Token: 0x04005F5B RID: 24411
			private const int _maxAttempts = 30;

			// Token: 0x04005F5C RID: 24412
			private readonly Rect2 _rect;

			// Token: 0x04005F5D RID: 24413
			private readonly float _radius2;

			// Token: 0x04005F5E RID: 24414
			private readonly float _cellSize;

			// Token: 0x04005F5F RID: 24415
			private readonly Vector2[,] _grid;

			// Token: 0x04005F60 RID: 24416
			private readonly List<Vector2> _activeSamples = new List<Vector2>();

			/// Helper struct to calculate the x and y indices of a sample in the grid
			// Token: 0x02002595 RID: 9621
			[NullableContext(0)]
			private struct GridPos
			{
				// Token: 0x0600C693 RID: 50835 RVA: 0x0044FE8E File Offset: 0x0044E08E
				public GridPos(Vector2 sample, float cellSize)
				{
					this.x = (int)(sample.X / cellSize);
					this.y = (int)(sample.Y / cellSize);
				}

				// Token: 0x0400A09C RID: 41116
				public readonly int x;

				// Token: 0x0400A09D RID: 41117
				public readonly int y;
			}
		}

		/// <summary>
		/// Cached StringNames for the methods contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x02001862 RID: 6242
		public class MethodName : Control.MethodName
		{
			/// <summary>
			/// Cached name for the '_Ready' method.
			/// </summary>
			// Token: 0x04005F61 RID: 24417
			public static readonly StringName _Ready = "_Ready";

			/// <summary>
			/// Cached name for the 'PositionNewChild' method.
			/// </summary>
			// Token: 0x04005F62 RID: 24418
			public static readonly StringName PositionNewChild = "PositionNewChild";

			/// <summary>
			/// Cached name for the 'ResetSamples' method.
			/// </summary>
			// Token: 0x04005F63 RID: 24419
			public static readonly StringName ResetSamples = "ResetSamples";
		}

		/// <summary>
		/// Cached StringNames for the properties and fields contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x02001863 RID: 6243
		public class PropertyName : Control.PropertyName
		{
			/// <summary>
			/// Cached name for the '_resetNewCardTimer' field.
			/// </summary>
			// Token: 0x04005F64 RID: 24420
			public static readonly StringName _resetNewCardTimer = "_resetNewCardTimer";

			/// <summary>
			/// Cached name for the '_currentMaxPosition' field.
			/// </summary>
			// Token: 0x04005F65 RID: 24421
			public static readonly StringName _currentMaxPosition = "_currentMaxPosition";
		}

		/// <summary>
		/// Cached StringNames for the signals contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x02001864 RID: 6244
		public class SignalName : Control.SignalName
		{
		}
	}
}
