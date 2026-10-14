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
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Nodes.GodotExtensions;
using MegaCrit.Sts2.Core.Nodes.Rooms;
using MegaCrit.Sts2.Core.TestSupport;

namespace MegaCrit.Sts2.Core.Nodes.Vfx
{
	/// <summary>
	/// Renders 3 additive blended images of a relic above a creature
	/// </summary>
	// Token: 0x020001E6 RID: 486
	[NullableContext(1)]
	[Nullable(0)]
	[ScriptPath("res://src/Core/Nodes/Vfx/NRelicFlashVfx.cs")]
	public class NRelicFlashVfx : Control
	{
		// Token: 0x17000628 RID: 1576
		// (get) Token: 0x060018A5 RID: 6309 RVA: 0x0009A57D File Offset: 0x0009877D
		public static IEnumerable<string> AssetPaths
		{
			get
			{
				return new <>z__ReadOnlySingleElementList<string>("res://scenes/vfx/relic_flash_vfx.tscn");
			}
		}

		// Token: 0x060018A6 RID: 6310 RVA: 0x0009A58C File Offset: 0x0009878C
		[return: Nullable(2)]
		public static NRelicFlashVfx Create(RelicModel relic)
		{
			if (TestMode.IsOn)
			{
				return null;
			}
			NRelicFlashVfx nrelicFlashVfx = PreloadManager.Cache.GetScene("res://scenes/vfx/relic_flash_vfx.tscn").Instantiate<NRelicFlashVfx>(0L);
			nrelicFlashVfx._relic = relic;
			return nrelicFlashVfx;
		}

		// Token: 0x060018A7 RID: 6311 RVA: 0x0009A5C4 File Offset: 0x000987C4
		[return: Nullable(2)]
		public static NRelicFlashVfx Create(RelicModel relic, Creature target)
		{
			NRelicFlashVfx nrelicFlashVfx = NRelicFlashVfx.Create(relic);
			if (nrelicFlashVfx == null)
			{
				return null;
			}
			nrelicFlashVfx._target = target;
			return nrelicFlashVfx;
		}

		// Token: 0x060018A8 RID: 6312 RVA: 0x0009A5E8 File Offset: 0x000987E8
		public override void _Ready()
		{
			this._sprite = base.GetNode<TextureRect>("Image1");
			this._sprite2 = base.GetNode<TextureRect>("Image2");
			this._sprite3 = base.GetNode<TextureRect>("Image3");
			if (this._target != null)
			{
				base.GlobalPosition = NCombatRoom.Instance.GetCreatureNode(this._target).GetTopOfHitbox();
			}
			TaskHelper.RunSafely(this.StartVfx());
		}

		// Token: 0x060018A9 RID: 6313 RVA: 0x0009A666 File Offset: 0x00098866
		public override void _ExitTree()
		{
			Tween tween = this._tween;
			if (tween == null)
			{
				return;
			}
			tween.Kill();
		}

		// Token: 0x060018AA RID: 6314 RVA: 0x0009A678 File Offset: 0x00098878
		private async Task StartVfx()
		{
			this._sprite.Texture = this._relic.Icon;
			this._sprite2.Texture = this._relic.Icon;
			this._sprite3.Texture = this._relic.Icon;
			this._tween = base.CreateTween().SetParallel(true);
			if (this._target != null)
			{
				base.Position += new Vector2(0f, 64f);
				this._tween.TweenProperty(this, "position:y", base.Position.Y - 64f, 1.0).SetEase(1L).SetTrans(5L);
			}
			this._tween.TweenProperty(this._sprite, "modulate:a", 1f, 0.01);
			this._tween.TweenProperty(this._sprite, "scale", NRelicFlashVfx._targetScale, 1.0).SetEase(1L).SetTrans(7L);
			this._tween.TweenProperty(this._sprite, "modulate:a", 0f, 1.5).SetDelay(0.01);
			this._tween.TweenProperty(this._sprite2, "modulate:a", 1f, 0.01).SetDelay(0.2);
			this._tween.TweenProperty(this._sprite2, "scale", NRelicFlashVfx._targetScale, 1.0).SetEase(1L).SetTrans(7L)
				.SetDelay(0.2);
			this._tween.TweenProperty(this._sprite2, "modulate:a", 0f, 1.5).SetDelay(0.21);
			this._tween.TweenProperty(this._sprite3, "modulate:a", 1f, 0.01).SetDelay(0.4);
			this._tween.TweenProperty(this._sprite3, "scale", NRelicFlashVfx._targetScale, 1.0).SetEase(1L).SetTrans(7L)
				.SetDelay(0.4);
			this._tween.TweenProperty(this._sprite3, "modulate:a", 0f, 1.5).SetDelay(0.41);
			bool flag = await this._tween.AwaitFinished(this);
			this.QueueFreeSafely();
		}

		/// <summary>
		/// Get the method information for all the methods declared in this class.
		/// This method is used by Godot to register the available methods in the editor.
		/// Do not call this method.
		/// </summary>
		// Token: 0x060018AB RID: 6315 RVA: 0x0009A6BC File Offset: 0x000988BC
		[NullableContext(0)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal static List<MethodInfo> GetGodotMethodList()
		{
			return new List<MethodInfo>(2)
			{
				new MethodInfo(NRelicFlashVfx.MethodName._Ready, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NRelicFlashVfx.MethodName._ExitTree, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null)
			};
		}

		/// <inheritdoc />
		// Token: 0x060018AC RID: 6316 RVA: 0x0009A730 File Offset: 0x00098930
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool InvokeGodotClassMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
		{
			if ((ref method) == NRelicFlashVfx.MethodName._Ready && args.Count == 0)
			{
				this._Ready();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NRelicFlashVfx.MethodName._ExitTree && args.Count == 0)
			{
				this._ExitTree();
				ret = default(godot_variant);
				return true;
			}
			return base.InvokeGodotClassMethod(ref method, args, ref ret);
		}

		/// <inheritdoc />
		// Token: 0x060018AD RID: 6317 RVA: 0x0009A790 File Offset: 0x00098990
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool HasGodotClassMethod(in godot_string_name method)
		{
			return (ref method) == NRelicFlashVfx.MethodName._Ready || (ref method) == NRelicFlashVfx.MethodName._ExitTree || base.HasGodotClassMethod(ref method);
		}

		/// <inheritdoc />
		// Token: 0x060018AE RID: 6318 RVA: 0x0009A7B8 File Offset: 0x000989B8
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool SetGodotClassPropertyValue(in godot_string_name name, in godot_variant value)
		{
			if ((ref name) == NRelicFlashVfx.PropertyName._sprite)
			{
				this._sprite = VariantUtils.ConvertTo<TextureRect>(ref value);
				return true;
			}
			if ((ref name) == NRelicFlashVfx.PropertyName._sprite2)
			{
				this._sprite2 = VariantUtils.ConvertTo<TextureRect>(ref value);
				return true;
			}
			if ((ref name) == NRelicFlashVfx.PropertyName._sprite3)
			{
				this._sprite3 = VariantUtils.ConvertTo<TextureRect>(ref value);
				return true;
			}
			if ((ref name) == NRelicFlashVfx.PropertyName._tween)
			{
				this._tween = VariantUtils.ConvertTo<Tween>(ref value);
				return true;
			}
			return base.SetGodotClassPropertyValue(ref name, ref value);
		}

		/// <inheritdoc />
		// Token: 0x060018AF RID: 6319 RVA: 0x0009A83C File Offset: 0x00098A3C
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
		{
			if ((ref name) == NRelicFlashVfx.PropertyName._sprite)
			{
				value = VariantUtils.CreateFrom<TextureRect>(ref this._sprite);
				return true;
			}
			if ((ref name) == NRelicFlashVfx.PropertyName._sprite2)
			{
				value = VariantUtils.CreateFrom<TextureRect>(ref this._sprite2);
				return true;
			}
			if ((ref name) == NRelicFlashVfx.PropertyName._sprite3)
			{
				value = VariantUtils.CreateFrom<TextureRect>(ref this._sprite3);
				return true;
			}
			if ((ref name) == NRelicFlashVfx.PropertyName._tween)
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
		// Token: 0x060018B0 RID: 6320 RVA: 0x0009A8D4 File Offset: 0x00098AD4
		[NullableContext(0)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal static List<PropertyInfo> GetGodotPropertyList()
		{
			return new List<PropertyInfo>
			{
				new PropertyInfo(24L, NRelicFlashVfx.PropertyName._sprite, 0L, "", 4096L, false),
				new PropertyInfo(24L, NRelicFlashVfx.PropertyName._sprite2, 0L, "", 4096L, false),
				new PropertyInfo(24L, NRelicFlashVfx.PropertyName._sprite3, 0L, "", 4096L, false),
				new PropertyInfo(24L, NRelicFlashVfx.PropertyName._tween, 0L, "", 4096L, false)
			};
		}

		/// <inheritdoc />
		// Token: 0x060018B1 RID: 6321 RVA: 0x0009A96C File Offset: 0x00098B6C
		[NullableContext(0)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void SaveGodotObjectData(GodotSerializationInfo info)
		{
			base.SaveGodotObjectData(info);
			info.AddProperty(NRelicFlashVfx.PropertyName._sprite, Variant.From<TextureRect>(ref this._sprite));
			info.AddProperty(NRelicFlashVfx.PropertyName._sprite2, Variant.From<TextureRect>(ref this._sprite2));
			info.AddProperty(NRelicFlashVfx.PropertyName._sprite3, Variant.From<TextureRect>(ref this._sprite3));
			info.AddProperty(NRelicFlashVfx.PropertyName._tween, Variant.From<Tween>(ref this._tween));
		}

		/// <inheritdoc />
		// Token: 0x060018B2 RID: 6322 RVA: 0x0009A9D8 File Offset: 0x00098BD8
		[NullableContext(0)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void RestoreGodotObjectData(GodotSerializationInfo info)
		{
			base.RestoreGodotObjectData(info);
			Variant variant;
			if (info.TryGetProperty(NRelicFlashVfx.PropertyName._sprite, ref variant))
			{
				this._sprite = variant.As<TextureRect>();
			}
			Variant variant2;
			if (info.TryGetProperty(NRelicFlashVfx.PropertyName._sprite2, ref variant2))
			{
				this._sprite2 = variant2.As<TextureRect>();
			}
			Variant variant3;
			if (info.TryGetProperty(NRelicFlashVfx.PropertyName._sprite3, ref variant3))
			{
				this._sprite3 = variant3.As<TextureRect>();
			}
			Variant variant4;
			if (info.TryGetProperty(NRelicFlashVfx.PropertyName._tween, ref variant4))
			{
				this._tween = variant4.As<Tween>();
			}
		}

		// Token: 0x040008E7 RID: 2279
		public const float activationDuration = 1f;

		// Token: 0x040008E8 RID: 2280
		private const string _scenePath = "res://scenes/vfx/relic_flash_vfx.tscn";

		// Token: 0x040008E9 RID: 2281
		private TextureRect _sprite;

		// Token: 0x040008EA RID: 2282
		private TextureRect _sprite2;

		// Token: 0x040008EB RID: 2283
		private TextureRect _sprite3;

		// Token: 0x040008EC RID: 2284
		private static readonly Vector2 _targetScale = Vector2.One * 1.25f;

		// Token: 0x040008ED RID: 2285
		[Nullable(2)]
		private RelicModel _relic;

		// Token: 0x040008EE RID: 2286
		[Nullable(2)]
		private Creature _target;

		// Token: 0x040008EF RID: 2287
		[Nullable(2)]
		private Tween _tween;

		/// <summary>
		/// Cached StringNames for the methods contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x02001107 RID: 4359
		[NullableContext(0)]
		public class MethodName : Control.MethodName
		{
			/// <summary>
			/// Cached name for the '_Ready' method.
			/// </summary>
			// Token: 0x04004288 RID: 17032
			public static readonly StringName _Ready = "_Ready";

			/// <summary>
			/// Cached name for the '_ExitTree' method.
			/// </summary>
			// Token: 0x04004289 RID: 17033
			public static readonly StringName _ExitTree = "_ExitTree";
		}

		/// <summary>
		/// Cached StringNames for the properties and fields contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x02001108 RID: 4360
		[NullableContext(0)]
		public class PropertyName : Control.PropertyName
		{
			/// <summary>
			/// Cached name for the '_sprite' field.
			/// </summary>
			// Token: 0x0400428A RID: 17034
			public static readonly StringName _sprite = "_sprite";

			/// <summary>
			/// Cached name for the '_sprite2' field.
			/// </summary>
			// Token: 0x0400428B RID: 17035
			public static readonly StringName _sprite2 = "_sprite2";

			/// <summary>
			/// Cached name for the '_sprite3' field.
			/// </summary>
			// Token: 0x0400428C RID: 17036
			public static readonly StringName _sprite3 = "_sprite3";

			/// <summary>
			/// Cached name for the '_tween' field.
			/// </summary>
			// Token: 0x0400428D RID: 17037
			public static readonly StringName _tween = "_tween";
		}

		/// <summary>
		/// Cached StringNames for the signals contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x02001109 RID: 4361
		[NullableContext(0)]
		public class SignalName : Control.SignalName
		{
		}
	}
}
