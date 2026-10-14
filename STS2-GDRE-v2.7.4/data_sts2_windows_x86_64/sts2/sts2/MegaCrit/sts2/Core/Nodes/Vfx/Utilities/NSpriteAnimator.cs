using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using Godot;
using Godot.Bridge;
using Godot.NativeInterop;
using MegaCrit.Sts2.Core.Helpers;

namespace MegaCrit.Sts2.Core.Nodes.Vfx.Utilities
{
	// Token: 0x0200021B RID: 539
	[ScriptPath("res://src/Core/Nodes/Vfx/Utilities/NSpriteAnimator.cs")]
	public class NSpriteAnimator : Sprite2D
	{
		// Token: 0x06001B99 RID: 7065 RVA: 0x000B379E File Offset: 0x000B199E
		public override void _Ready()
		{
			if (this._randomizeRotation)
			{
				base.RotationDegrees = (float)new Random().Next((int)this._rotationRange.X, (int)this._rotationRange.Y);
			}
			TaskHelper.RunSafely(this.PlayAnimation());
		}

		// Token: 0x06001B9A RID: 7066 RVA: 0x000B37DD File Offset: 0x000B19DD
		public override void _ExitTree()
		{
			this._cancelToken.Cancel();
		}

		// Token: 0x06001B9B RID: 7067 RVA: 0x000B37EC File Offset: 0x000B19EC
		[NullableContext(1)]
		private async Task PlayAnimation()
		{
			int i = 0;
			int interval = Mathf.RoundToInt(1000f / this._fps);
			while (!this._cancelToken.IsCancellationRequested)
			{
				base.Texture = this._frames[i];
				int num = i;
				i = num + 1;
				if (this._loop)
				{
					i %= this._frames.Length;
				}
				await Task.Delay(interval, this._cancelToken.Token);
				if (this._frames.Length <= i)
				{
					break;
				}
			}
			if (!this._cancelToken.IsCancellationRequested)
			{
				this.QueueFreeSafely();
			}
		}

		/// <summary>
		/// Get the method information for all the methods declared in this class.
		/// This method is used by Godot to register the available methods in the editor.
		/// Do not call this method.
		/// </summary>
		// Token: 0x06001B9C RID: 7068 RVA: 0x000B3830 File Offset: 0x000B1A30
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal static List<MethodInfo> GetGodotMethodList()
		{
			return new List<MethodInfo>(2)
			{
				new MethodInfo(NSpriteAnimator.MethodName._Ready, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NSpriteAnimator.MethodName._ExitTree, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null)
			};
		}

		/// <inheritdoc />
		// Token: 0x06001B9D RID: 7069 RVA: 0x000B38A4 File Offset: 0x000B1AA4
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool InvokeGodotClassMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
		{
			if ((ref method) == NSpriteAnimator.MethodName._Ready && args.Count == 0)
			{
				this._Ready();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NSpriteAnimator.MethodName._ExitTree && args.Count == 0)
			{
				this._ExitTree();
				ret = default(godot_variant);
				return true;
			}
			return base.InvokeGodotClassMethod(ref method, args, ref ret);
		}

		/// <inheritdoc />
		// Token: 0x06001B9E RID: 7070 RVA: 0x000B3904 File Offset: 0x000B1B04
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool HasGodotClassMethod(in godot_string_name method)
		{
			return (ref method) == NSpriteAnimator.MethodName._Ready || (ref method) == NSpriteAnimator.MethodName._ExitTree || base.HasGodotClassMethod(ref method);
		}

		/// <inheritdoc />
		// Token: 0x06001B9F RID: 7071 RVA: 0x000B392C File Offset: 0x000B1B2C
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool SetGodotClassPropertyValue(in godot_string_name name, in godot_variant value)
		{
			if ((ref name) == NSpriteAnimator.PropertyName._frames)
			{
				this._frames = VariantUtils.ConvertToSystemArrayOfGodotObject<Texture2D>(ref value);
				return true;
			}
			if ((ref name) == NSpriteAnimator.PropertyName._fps)
			{
				this._fps = VariantUtils.ConvertTo<float>(ref value);
				return true;
			}
			if ((ref name) == NSpriteAnimator.PropertyName._loop)
			{
				this._loop = VariantUtils.ConvertTo<bool>(ref value);
				return true;
			}
			if ((ref name) == NSpriteAnimator.PropertyName._randomizeRotation)
			{
				this._randomizeRotation = VariantUtils.ConvertTo<bool>(ref value);
				return true;
			}
			if ((ref name) == NSpriteAnimator.PropertyName._rotationRange)
			{
				this._rotationRange = VariantUtils.ConvertTo<Vector2>(ref value);
				return true;
			}
			return base.SetGodotClassPropertyValue(ref name, ref value);
		}

		/// <inheritdoc />
		// Token: 0x06001BA0 RID: 7072 RVA: 0x000B39C8 File Offset: 0x000B1BC8
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
		{
			if ((ref name) == NSpriteAnimator.PropertyName._frames)
			{
				GodotObject[] frames = this._frames;
				value = VariantUtils.CreateFromSystemArrayOfGodotObject(frames);
				return true;
			}
			if ((ref name) == NSpriteAnimator.PropertyName._fps)
			{
				value = VariantUtils.CreateFrom<float>(ref this._fps);
				return true;
			}
			if ((ref name) == NSpriteAnimator.PropertyName._loop)
			{
				value = VariantUtils.CreateFrom<bool>(ref this._loop);
				return true;
			}
			if ((ref name) == NSpriteAnimator.PropertyName._randomizeRotation)
			{
				value = VariantUtils.CreateFrom<bool>(ref this._randomizeRotation);
				return true;
			}
			if ((ref name) == NSpriteAnimator.PropertyName._rotationRange)
			{
				value = VariantUtils.CreateFrom<Vector2>(ref this._rotationRange);
				return true;
			}
			return base.GetGodotClassPropertyValue(ref name, ref value);
		}

		/// <summary>
		/// Get the property information for all the properties declared in this class.
		/// This method is used by Godot to register the available properties in the editor.
		/// Do not call this method.
		/// </summary>
		// Token: 0x06001BA1 RID: 7073 RVA: 0x000B3A80 File Offset: 0x000B1C80
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal static List<PropertyInfo> GetGodotPropertyList()
		{
			return new List<PropertyInfo>
			{
				new PropertyInfo(0L, "Animation Settings", 0L, "", 64L, true),
				new PropertyInfo(28L, NSpriteAnimator.PropertyName._frames, 23L, "24/17:Texture2D", 4102L, true),
				new PropertyInfo(3L, NSpriteAnimator.PropertyName._fps, 0L, "", 4102L, true),
				new PropertyInfo(1L, NSpriteAnimator.PropertyName._loop, 0L, "", 4102L, true),
				new PropertyInfo(0L, "Rotation Settings", 0L, "", 64L, true),
				new PropertyInfo(1L, NSpriteAnimator.PropertyName._randomizeRotation, 0L, "", 4102L, true),
				new PropertyInfo(5L, NSpriteAnimator.PropertyName._rotationRange, 0L, "", 4102L, true)
			};
		}

		/// <inheritdoc />
		// Token: 0x06001BA2 RID: 7074 RVA: 0x000B3B7C File Offset: 0x000B1D7C
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void SaveGodotObjectData(GodotSerializationInfo info)
		{
			base.SaveGodotObjectData(info);
			StringName frames = NSpriteAnimator.PropertyName._frames;
			GodotObject[] frames2 = this._frames;
			info.AddProperty(frames, Variant.CreateFrom(frames2));
			info.AddProperty(NSpriteAnimator.PropertyName._fps, Variant.From<float>(ref this._fps));
			info.AddProperty(NSpriteAnimator.PropertyName._loop, Variant.From<bool>(ref this._loop));
			info.AddProperty(NSpriteAnimator.PropertyName._randomizeRotation, Variant.From<bool>(ref this._randomizeRotation));
			info.AddProperty(NSpriteAnimator.PropertyName._rotationRange, Variant.From<Vector2>(ref this._rotationRange));
		}

		/// <inheritdoc />
		// Token: 0x06001BA3 RID: 7075 RVA: 0x000B3C00 File Offset: 0x000B1E00
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void RestoreGodotObjectData(GodotSerializationInfo info)
		{
			base.RestoreGodotObjectData(info);
			Variant variant;
			if (info.TryGetProperty(NSpriteAnimator.PropertyName._frames, ref variant))
			{
				this._frames = variant.AsGodotObjectArray<Texture2D>();
			}
			Variant variant2;
			if (info.TryGetProperty(NSpriteAnimator.PropertyName._fps, ref variant2))
			{
				this._fps = variant2.As<float>();
			}
			Variant variant3;
			if (info.TryGetProperty(NSpriteAnimator.PropertyName._loop, ref variant3))
			{
				this._loop = variant3.As<bool>();
			}
			Variant variant4;
			if (info.TryGetProperty(NSpriteAnimator.PropertyName._randomizeRotation, ref variant4))
			{
				this._randomizeRotation = variant4.As<bool>();
			}
			Variant variant5;
			if (info.TryGetProperty(NSpriteAnimator.PropertyName._rotationRange, ref variant5))
			{
				this._rotationRange = variant5.As<Vector2>();
			}
		}

		// Token: 0x04000A6D RID: 2669
		[Nullable(1)]
		[ExportGroup("Animation Settings", "")]
		[Export(/*Failed to decode CustomAttribute blob!*/)]
		private Texture2D[] _frames;

		// Token: 0x04000A6E RID: 2670
		[Export(/*Failed to decode CustomAttribute blob!*/)]
		private float _fps = 15f;

		// Token: 0x04000A6F RID: 2671
		[Export(/*Failed to decode CustomAttribute blob!*/)]
		private bool _loop;

		// Token: 0x04000A70 RID: 2672
		[ExportGroup("Rotation Settings", "")]
		[Export(/*Failed to decode CustomAttribute blob!*/)]
		private bool _randomizeRotation;

		// Token: 0x04000A71 RID: 2673
		[Export(/*Failed to decode CustomAttribute blob!*/)]
		private Vector2 _rotationRange;

		// Token: 0x04000A72 RID: 2674
		[Nullable(1)]
		private readonly CancellationTokenSource _cancelToken = new CancellationTokenSource();

		/// <summary>
		/// Cached StringNames for the methods contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x020011B9 RID: 4537
		public class MethodName : Sprite2D.MethodName
		{
			/// <summary>
			/// Cached name for the '_Ready' method.
			/// </summary>
			// Token: 0x04004507 RID: 17671
			public static readonly StringName _Ready = "_Ready";

			/// <summary>
			/// Cached name for the '_ExitTree' method.
			/// </summary>
			// Token: 0x04004508 RID: 17672
			public static readonly StringName _ExitTree = "_ExitTree";
		}

		/// <summary>
		/// Cached StringNames for the properties and fields contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x020011BA RID: 4538
		public class PropertyName : Sprite2D.PropertyName
		{
			/// <summary>
			/// Cached name for the '_frames' field.
			/// </summary>
			// Token: 0x04004509 RID: 17673
			public static readonly StringName _frames = "_frames";

			/// <summary>
			/// Cached name for the '_fps' field.
			/// </summary>
			// Token: 0x0400450A RID: 17674
			public static readonly StringName _fps = "_fps";

			/// <summary>
			/// Cached name for the '_loop' field.
			/// </summary>
			// Token: 0x0400450B RID: 17675
			public static readonly StringName _loop = "_loop";

			/// <summary>
			/// Cached name for the '_randomizeRotation' field.
			/// </summary>
			// Token: 0x0400450C RID: 17676
			public static readonly StringName _randomizeRotation = "_randomizeRotation";

			/// <summary>
			/// Cached name for the '_rotationRange' field.
			/// </summary>
			// Token: 0x0400450D RID: 17677
			public static readonly StringName _rotationRange = "_rotationRange";
		}

		/// <summary>
		/// Cached StringNames for the signals contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x020011BB RID: 4539
		public class SignalName : Sprite2D.SignalName
		{
		}
	}
}
