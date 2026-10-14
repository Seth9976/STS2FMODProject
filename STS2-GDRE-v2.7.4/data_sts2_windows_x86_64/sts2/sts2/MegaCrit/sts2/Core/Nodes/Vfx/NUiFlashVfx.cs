using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Godot;
using Godot.Bridge;
using Godot.NativeInterop;
using MegaCrit.Sts2.Core.Assets;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.Nodes.GodotExtensions;
using MegaCrit.Sts2.Core.TestSupport;

namespace MegaCrit.Sts2.Core.Nodes.Vfx
{
	// Token: 0x02000209 RID: 521
	[ScriptPath("res://src/Core/Nodes/Vfx/NUiFlashVfx.cs")]
	public class NUiFlashVfx : Control
	{
		// Token: 0x17000638 RID: 1592
		// (get) Token: 0x06001AF2 RID: 6898 RVA: 0x000AD9E4 File Offset: 0x000ABBE4
		[Nullable(1)]
		public static IEnumerable<string> AssetPaths
		{
			[NullableContext(1)]
			get
			{
				return new <>z__ReadOnlySingleElementList<string>("res://scenes/vfx/ui_flash_vfx.tscn");
			}
		}

		// Token: 0x06001AF3 RID: 6899 RVA: 0x000AD9F0 File Offset: 0x000ABBF0
		public override void _Ready()
		{
			this._textureRect = base.GetNode<TextureRect>("TextureRect");
			this._textureRect.Texture = this._texture;
		}

		// Token: 0x06001AF4 RID: 6900 RVA: 0x000ADA1C File Offset: 0x000ABC1C
		[NullableContext(1)]
		public async Task StartVfx()
		{
			CanvasItem textureRect = this._textureRect;
			Color modulate = this._modulate;
			modulate.A = 0f;
			textureRect.Modulate = modulate;
			this._textureRect.PivotOffset = this._textureRect.Size * 0.5f;
			this._spriteTween = base.CreateTween();
			this._spriteTween.SetParallel(true);
			this._spriteTween.TweenProperty(this._textureRect, "scale", Vector2.One * 1.3f, 0.5);
			this._spriteTween.TweenProperty(this._textureRect, "modulate:a", 1f, 0.25).SetEase(1L).SetTrans(1L);
			this._spriteTween.TweenProperty(this._textureRect, "modulate:a", 0f, 0.25).SetEase(0L).SetTrans(1L)
				.SetDelay(0.3499999940395355);
			bool flag = await this._spriteTween.AwaitFinished(this);
			this.QueueFreeSafely();
		}

		// Token: 0x06001AF5 RID: 6901 RVA: 0x000ADA60 File Offset: 0x000ABC60
		[NullableContext(1)]
		[return: Nullable(2)]
		public static NUiFlashVfx Create(Texture2D tex, Color modulate)
		{
			if (TestMode.IsOn)
			{
				return null;
			}
			NUiFlashVfx nuiFlashVfx = (NUiFlashVfx)PreloadManager.Cache.GetScene("res://scenes/vfx/ui_flash_vfx.tscn").Instantiate(0L);
			nuiFlashVfx._texture = tex;
			nuiFlashVfx._modulate = modulate;
			return nuiFlashVfx;
		}

		// Token: 0x06001AF6 RID: 6902 RVA: 0x000ADAA1 File Offset: 0x000ABCA1
		public override void _ExitTree()
		{
			Tween spriteTween = this._spriteTween;
			if (spriteTween == null)
			{
				return;
			}
			spriteTween.Kill();
		}

		/// <summary>
		/// Get the method information for all the methods declared in this class.
		/// This method is used by Godot to register the available methods in the editor.
		/// Do not call this method.
		/// </summary>
		// Token: 0x06001AF7 RID: 6903 RVA: 0x000ADAB4 File Offset: 0x000ABCB4
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal static List<MethodInfo> GetGodotMethodList()
		{
			return new List<MethodInfo>(3)
			{
				new MethodInfo(NUiFlashVfx.MethodName._Ready, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NUiFlashVfx.MethodName.Create, new PropertyInfo(24L, "", 0L, "", 6L, new StringName("Control"), false), 33L, new List<PropertyInfo>
				{
					new PropertyInfo(24L, "tex", 0L, "", 6L, new StringName("Texture2D"), false),
					new PropertyInfo(20L, "modulate", 0L, "", 6L, false)
				}, null),
				new MethodInfo(NUiFlashVfx.MethodName._ExitTree, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null)
			};
		}

		/// <inheritdoc />
		// Token: 0x06001AF8 RID: 6904 RVA: 0x000ADBB4 File Offset: 0x000ABDB4
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool InvokeGodotClassMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
		{
			if ((ref method) == NUiFlashVfx.MethodName._Ready && args.Count == 0)
			{
				this._Ready();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NUiFlashVfx.MethodName.Create && args.Count == 2)
			{
				NUiFlashVfx nuiFlashVfx = NUiFlashVfx.Create(VariantUtils.ConvertTo<Texture2D>(args[0]), VariantUtils.ConvertTo<Color>(args[1]));
				ret = VariantUtils.CreateFrom<NUiFlashVfx>(ref nuiFlashVfx);
				return true;
			}
			if ((ref method) == NUiFlashVfx.MethodName._ExitTree && args.Count == 0)
			{
				this._ExitTree();
				ret = default(godot_variant);
				return true;
			}
			return base.InvokeGodotClassMethod(ref method, args, ref ret);
		}

		// Token: 0x06001AF9 RID: 6905 RVA: 0x000ADC5C File Offset: 0x000ABE5C
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal static bool InvokeGodotClassStaticMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
		{
			if ((ref method) == NUiFlashVfx.MethodName.Create && args.Count == 2)
			{
				NUiFlashVfx nuiFlashVfx = NUiFlashVfx.Create(VariantUtils.ConvertTo<Texture2D>(args[0]), VariantUtils.ConvertTo<Color>(args[1]));
				ret = VariantUtils.CreateFrom<NUiFlashVfx>(ref nuiFlashVfx);
				return true;
			}
			ret = default(godot_variant);
			return false;
		}

		/// <inheritdoc />
		// Token: 0x06001AFA RID: 6906 RVA: 0x000ADCB7 File Offset: 0x000ABEB7
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool HasGodotClassMethod(in godot_string_name method)
		{
			return (ref method) == NUiFlashVfx.MethodName._Ready || (ref method) == NUiFlashVfx.MethodName.Create || (ref method) == NUiFlashVfx.MethodName._ExitTree || base.HasGodotClassMethod(ref method);
		}

		/// <inheritdoc />
		// Token: 0x06001AFB RID: 6907 RVA: 0x000ADCF0 File Offset: 0x000ABEF0
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool SetGodotClassPropertyValue(in godot_string_name name, in godot_variant value)
		{
			if ((ref name) == NUiFlashVfx.PropertyName._textureRect)
			{
				this._textureRect = VariantUtils.ConvertTo<TextureRect>(ref value);
				return true;
			}
			if ((ref name) == NUiFlashVfx.PropertyName._texture)
			{
				this._texture = VariantUtils.ConvertTo<Texture2D>(ref value);
				return true;
			}
			if ((ref name) == NUiFlashVfx.PropertyName._modulate)
			{
				this._modulate = VariantUtils.ConvertTo<Color>(ref value);
				return true;
			}
			if ((ref name) == NUiFlashVfx.PropertyName._spriteTween)
			{
				this._spriteTween = VariantUtils.ConvertTo<Tween>(ref value);
				return true;
			}
			return base.SetGodotClassPropertyValue(ref name, ref value);
		}

		/// <inheritdoc />
		// Token: 0x06001AFC RID: 6908 RVA: 0x000ADD74 File Offset: 0x000ABF74
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
		{
			if ((ref name) == NUiFlashVfx.PropertyName._textureRect)
			{
				value = VariantUtils.CreateFrom<TextureRect>(ref this._textureRect);
				return true;
			}
			if ((ref name) == NUiFlashVfx.PropertyName._texture)
			{
				value = VariantUtils.CreateFrom<Texture2D>(ref this._texture);
				return true;
			}
			if ((ref name) == NUiFlashVfx.PropertyName._modulate)
			{
				value = VariantUtils.CreateFrom<Color>(ref this._modulate);
				return true;
			}
			if ((ref name) == NUiFlashVfx.PropertyName._spriteTween)
			{
				value = VariantUtils.CreateFrom<Tween>(ref this._spriteTween);
				return true;
			}
			return base.GetGodotClassPropertyValue(ref name, ref value);
		}

		/// <summary>
		/// Get the property information for all the properties declared in this class.
		/// This method is used by Godot to register the available properties in the editor.
		/// Do not call this method.
		/// </summary>
		// Token: 0x06001AFD RID: 6909 RVA: 0x000ADE0C File Offset: 0x000AC00C
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal static List<PropertyInfo> GetGodotPropertyList()
		{
			return new List<PropertyInfo>
			{
				new PropertyInfo(24L, NUiFlashVfx.PropertyName._textureRect, 0L, "", 4096L, false),
				new PropertyInfo(24L, NUiFlashVfx.PropertyName._texture, 0L, "", 4096L, false),
				new PropertyInfo(20L, NUiFlashVfx.PropertyName._modulate, 0L, "", 4096L, false),
				new PropertyInfo(24L, NUiFlashVfx.PropertyName._spriteTween, 0L, "", 4096L, false)
			};
		}

		/// <inheritdoc />
		// Token: 0x06001AFE RID: 6910 RVA: 0x000ADEA4 File Offset: 0x000AC0A4
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void SaveGodotObjectData(GodotSerializationInfo info)
		{
			base.SaveGodotObjectData(info);
			info.AddProperty(NUiFlashVfx.PropertyName._textureRect, Variant.From<TextureRect>(ref this._textureRect));
			info.AddProperty(NUiFlashVfx.PropertyName._texture, Variant.From<Texture2D>(ref this._texture));
			info.AddProperty(NUiFlashVfx.PropertyName._modulate, Variant.From<Color>(ref this._modulate));
			info.AddProperty(NUiFlashVfx.PropertyName._spriteTween, Variant.From<Tween>(ref this._spriteTween));
		}

		/// <inheritdoc />
		// Token: 0x06001AFF RID: 6911 RVA: 0x000ADF10 File Offset: 0x000AC110
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void RestoreGodotObjectData(GodotSerializationInfo info)
		{
			base.RestoreGodotObjectData(info);
			Variant variant;
			if (info.TryGetProperty(NUiFlashVfx.PropertyName._textureRect, ref variant))
			{
				this._textureRect = variant.As<TextureRect>();
			}
			Variant variant2;
			if (info.TryGetProperty(NUiFlashVfx.PropertyName._texture, ref variant2))
			{
				this._texture = variant2.As<Texture2D>();
			}
			Variant variant3;
			if (info.TryGetProperty(NUiFlashVfx.PropertyName._modulate, ref variant3))
			{
				this._modulate = variant3.As<Color>();
			}
			Variant variant4;
			if (info.TryGetProperty(NUiFlashVfx.PropertyName._spriteTween, ref variant4))
			{
				this._spriteTween = variant4.As<Tween>();
			}
		}

		// Token: 0x04000A07 RID: 2567
		[Nullable(1)]
		private const string _scenePath = "res://scenes/vfx/ui_flash_vfx.tscn";

		// Token: 0x04000A08 RID: 2568
		[Nullable(1)]
		private TextureRect _textureRect;

		// Token: 0x04000A09 RID: 2569
		[Nullable(1)]
		private Texture2D _texture;

		// Token: 0x04000A0A RID: 2570
		private Color _modulate;

		// Token: 0x04000A0B RID: 2571
		[Nullable(2)]
		private Tween _spriteTween;

		/// <summary>
		/// Cached StringNames for the methods contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x02001190 RID: 4496
		public class MethodName : Control.MethodName
		{
			/// <summary>
			/// Cached name for the '_Ready' method.
			/// </summary>
			// Token: 0x04004476 RID: 17526
			public static readonly StringName _Ready = "_Ready";

			/// <summary>
			/// Cached name for the 'Create' method.
			/// </summary>
			// Token: 0x04004477 RID: 17527
			public static readonly StringName Create = "Create";

			/// <summary>
			/// Cached name for the '_ExitTree' method.
			/// </summary>
			// Token: 0x04004478 RID: 17528
			public static readonly StringName _ExitTree = "_ExitTree";
		}

		/// <summary>
		/// Cached StringNames for the properties and fields contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x02001191 RID: 4497
		public class PropertyName : Control.PropertyName
		{
			/// <summary>
			/// Cached name for the '_textureRect' field.
			/// </summary>
			// Token: 0x04004479 RID: 17529
			public static readonly StringName _textureRect = "_textureRect";

			/// <summary>
			/// Cached name for the '_texture' field.
			/// </summary>
			// Token: 0x0400447A RID: 17530
			public static readonly StringName _texture = "_texture";

			/// <summary>
			/// Cached name for the '_modulate' field.
			/// </summary>
			// Token: 0x0400447B RID: 17531
			public static readonly StringName _modulate = "_modulate";

			/// <summary>
			/// Cached name for the '_spriteTween' field.
			/// </summary>
			// Token: 0x0400447C RID: 17532
			public static readonly StringName _spriteTween = "_spriteTween";
		}

		/// <summary>
		/// Cached StringNames for the signals contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x02001192 RID: 4498
		public class SignalName : Control.SignalName
		{
		}
	}
}
