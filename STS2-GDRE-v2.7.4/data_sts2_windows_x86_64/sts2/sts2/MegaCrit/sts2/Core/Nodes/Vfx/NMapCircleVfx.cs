using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Godot;
using Godot.Bridge;
using Godot.NativeInterop;
using MegaCrit.Sts2.Core.Assets;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.Map;
using MegaCrit.Sts2.Core.Nodes.GodotExtensions;
using MegaCrit.Sts2.Core.Random;
using MegaCrit.Sts2.Core.Runs;
using MegaCrit.Sts2.Core.TestSupport;

namespace MegaCrit.Sts2.Core.Nodes.Vfx
{
	// Token: 0x020001CF RID: 463
	[ScriptPath("res://src/Core/Nodes/Vfx/NMapCircleVfx.cs")]
	public class NMapCircleVfx : Control
	{
		// Token: 0x17000618 RID: 1560
		// (get) Token: 0x06001750 RID: 5968 RVA: 0x00090B18 File Offset: 0x0008ED18
		[Nullable(1)]
		public static IEnumerable<string> AssetPaths
		{
			[NullableContext(1)]
			get
			{
				return NMapCircleVfx._textures.Append("res://scenes/vfx/map_circle_vfx.tscn");
			}
		}

		// Token: 0x06001751 RID: 5969 RVA: 0x00090B2C File Offset: 0x0008ED2C
		public override void _Ready()
		{
			this._image = base.GetNode<TextureRect>("TextureRect");
			this._image.Texture = PreloadManager.Cache.GetTexture2D(NMapCircleVfx._textures[0]);
			Rng rng = new Rng(this._runState.Rng.Seed + (ulong)((long)this._mapCoord.row) + (ulong)(131L * (long)this._mapCoord.row));
			base.RotationDegrees = rng.NextFloat(360f);
			Vector2 vector = Vector2.One * rng.NextFloat(0.85f, 0.9f);
			if (this._playAnim)
			{
				Tween tween = base.CreateTween().SetParallel(true);
				tween.TweenProperty(this, "scale", vector, 1.0).SetEase(1L).SetTrans(5L)
					.From(vector * 2f);
				tween.TweenProperty(this, "modulate:a", 0.95f, 0.5).SetEase(1L).SetTrans(5L)
					.From(0f);
				TaskHelper.RunSafely(this.AnimateSprite());
				return;
			}
			base.Scale = vector;
			base.Modulate = new Color(base.Modulate, 0.95f);
			string text = NMapCircleVfx._textures.Last<string>();
			this._image.Texture = PreloadManager.Cache.GetTexture2D(text);
		}

		// Token: 0x06001752 RID: 5970 RVA: 0x00090CB8 File Offset: 0x0008EEB8
		[NullableContext(1)]
		private async Task AnimateSprite()
		{
			foreach (string text in NMapCircleVfx._textures)
			{
				this._image.Texture = PreloadManager.Cache.GetTexture2D(text);
				SceneTreeTimer sceneTreeTimer = base.GetTree().CreateTimer(0.041666666666666664, true, false, false);
				await sceneTreeTimer.AwaitSignal(SceneTreeTimer.SignalName.Timeout, this);
			}
			string[] array = null;
		}

		// Token: 0x06001753 RID: 5971 RVA: 0x00090CFC File Offset: 0x0008EEFC
		[NullableContext(1)]
		[return: Nullable(2)]
		public static NMapCircleVfx Create(IRunState runState, MapCoord mapCoord, bool playAnim)
		{
			if (TestMode.IsOn)
			{
				return null;
			}
			NMapCircleVfx nmapCircleVfx = PreloadManager.Cache.GetScene("res://scenes/vfx/map_circle_vfx.tscn").Instantiate<NMapCircleVfx>(0L);
			nmapCircleVfx._playAnim = playAnim;
			nmapCircleVfx._mapCoord = mapCoord;
			nmapCircleVfx._runState = runState;
			return nmapCircleVfx;
		}

		/// <summary>
		/// Get the method information for all the methods declared in this class.
		/// This method is used by Godot to register the available methods in the editor.
		/// Do not call this method.
		/// </summary>
		// Token: 0x06001754 RID: 5972 RVA: 0x00090D40 File Offset: 0x0008EF40
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal static List<MethodInfo> GetGodotMethodList()
		{
			return new List<MethodInfo>(1)
			{
				new MethodInfo(NMapCircleVfx.MethodName._Ready, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null)
			};
		}

		/// <inheritdoc />
		// Token: 0x06001755 RID: 5973 RVA: 0x00090D84 File Offset: 0x0008EF84
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool InvokeGodotClassMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
		{
			if ((ref method) == NMapCircleVfx.MethodName._Ready && args.Count == 0)
			{
				this._Ready();
				ret = default(godot_variant);
				return true;
			}
			return base.InvokeGodotClassMethod(ref method, args, ref ret);
		}

		/// <inheritdoc />
		// Token: 0x06001756 RID: 5974 RVA: 0x00090DB4 File Offset: 0x0008EFB4
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool HasGodotClassMethod(in godot_string_name method)
		{
			return (ref method) == NMapCircleVfx.MethodName._Ready || base.HasGodotClassMethod(ref method);
		}

		/// <inheritdoc />
		// Token: 0x06001757 RID: 5975 RVA: 0x00090DCC File Offset: 0x0008EFCC
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool SetGodotClassPropertyValue(in godot_string_name name, in godot_variant value)
		{
			if ((ref name) == NMapCircleVfx.PropertyName._image)
			{
				this._image = VariantUtils.ConvertTo<TextureRect>(ref value);
				return true;
			}
			if ((ref name) == NMapCircleVfx.PropertyName._playAnim)
			{
				this._playAnim = VariantUtils.ConvertTo<bool>(ref value);
				return true;
			}
			return base.SetGodotClassPropertyValue(ref name, ref value);
		}

		/// <inheritdoc />
		// Token: 0x06001758 RID: 5976 RVA: 0x00090E0C File Offset: 0x0008F00C
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
		{
			if ((ref name) == NMapCircleVfx.PropertyName._image)
			{
				value = VariantUtils.CreateFrom<TextureRect>(ref this._image);
				return true;
			}
			if ((ref name) == NMapCircleVfx.PropertyName._playAnim)
			{
				value = VariantUtils.CreateFrom<bool>(ref this._playAnim);
				return true;
			}
			return base.GetGodotClassPropertyValue(ref name, ref value);
		}

		/// <summary>
		/// Get the property information for all the properties declared in this class.
		/// This method is used by Godot to register the available properties in the editor.
		/// Do not call this method.
		/// </summary>
		// Token: 0x06001759 RID: 5977 RVA: 0x00090E64 File Offset: 0x0008F064
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal static List<PropertyInfo> GetGodotPropertyList()
		{
			return new List<PropertyInfo>
			{
				new PropertyInfo(24L, NMapCircleVfx.PropertyName._image, 0L, "", 4096L, false),
				new PropertyInfo(1L, NMapCircleVfx.PropertyName._playAnim, 0L, "", 4096L, false)
			};
		}

		/// <inheritdoc />
		// Token: 0x0600175A RID: 5978 RVA: 0x00090EB9 File Offset: 0x0008F0B9
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void SaveGodotObjectData(GodotSerializationInfo info)
		{
			base.SaveGodotObjectData(info);
			info.AddProperty(NMapCircleVfx.PropertyName._image, Variant.From<TextureRect>(ref this._image));
			info.AddProperty(NMapCircleVfx.PropertyName._playAnim, Variant.From<bool>(ref this._playAnim));
		}

		/// <inheritdoc />
		// Token: 0x0600175B RID: 5979 RVA: 0x00090EF0 File Offset: 0x0008F0F0
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void RestoreGodotObjectData(GodotSerializationInfo info)
		{
			base.RestoreGodotObjectData(info);
			Variant variant;
			if (info.TryGetProperty(NMapCircleVfx.PropertyName._image, ref variant))
			{
				this._image = variant.As<TextureRect>();
			}
			Variant variant2;
			if (info.TryGetProperty(NMapCircleVfx.PropertyName._playAnim, ref variant2))
			{
				this._playAnim = variant2.As<bool>();
			}
		}

		// Token: 0x04000856 RID: 2134
		[Nullable(1)]
		private TextureRect _image;

		// Token: 0x04000857 RID: 2135
		[Nullable(1)]
		private const string _path = "res://scenes/vfx/map_circle_vfx.tscn";

		// Token: 0x04000858 RID: 2136
		[Nullable(1)]
		private static readonly string[] _textures = new string[] { "res://images/atlases/compressed.sprites/map/map_circle_0.tres", "res://images/atlases/compressed.sprites/map/map_circle_1.tres", "res://images/atlases/compressed.sprites/map/map_circle_2.tres", "res://images/atlases/compressed.sprites/map/map_circle_3.tres", "res://images/atlases/compressed.sprites/map/map_circle_4.tres" };

		// Token: 0x04000859 RID: 2137
		private const double _animInterval = 0.041666666666666664;

		// Token: 0x0400085A RID: 2138
		private bool _playAnim;

		// Token: 0x0400085B RID: 2139
		private MapCoord _mapCoord;

		// Token: 0x0400085C RID: 2140
		[Nullable(1)]
		private IRunState _runState;

		/// <summary>
		/// Cached StringNames for the methods contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x020010B3 RID: 4275
		public class MethodName : Control.MethodName
		{
			/// <summary>
			/// Cached name for the '_Ready' method.
			/// </summary>
			// Token: 0x04004193 RID: 16787
			public static readonly StringName _Ready = "_Ready";
		}

		/// <summary>
		/// Cached StringNames for the properties and fields contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x020010B4 RID: 4276
		public class PropertyName : Control.PropertyName
		{
			/// <summary>
			/// Cached name for the '_image' field.
			/// </summary>
			// Token: 0x04004194 RID: 16788
			public static readonly StringName _image = "_image";

			/// <summary>
			/// Cached name for the '_playAnim' field.
			/// </summary>
			// Token: 0x04004195 RID: 16789
			public static readonly StringName _playAnim = "_playAnim";
		}

		/// <summary>
		/// Cached StringNames for the signals contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x020010B5 RID: 4277
		public class SignalName : Control.SignalName
		{
		}
	}
}
