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
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Nodes.Combat;
using MegaCrit.Sts2.Core.Nodes.GodotExtensions;
using MegaCrit.Sts2.Core.Nodes.Rooms;
using MegaCrit.Sts2.Core.TestSupport;

namespace MegaCrit.Sts2.Core.Nodes.Vfx
{
	// Token: 0x020001DF RID: 479
	[ScriptPath("res://src/Core/Nodes/Vfx/NPowerFlashVfx.cs")]
	public class NPowerFlashVfx : Node2D
	{
		// Token: 0x17000622 RID: 1570
		// (get) Token: 0x0600183E RID: 6206 RVA: 0x00097AC4 File Offset: 0x00095CC4
		[Nullable(1)]
		public static IEnumerable<string> AssetPaths
		{
			[NullableContext(1)]
			get
			{
				return new <>z__ReadOnlySingleElementList<string>("res://scenes/vfx/power_flash_vfx.tscn");
			}
		}

		// Token: 0x0600183F RID: 6207 RVA: 0x00097AD0 File Offset: 0x00095CD0
		public override void _Ready()
		{
			NCombatRoom instance = NCombatRoom.Instance;
			NCreature ncreature = ((instance != null) ? instance.GetCreatureNode(this._power.Owner) : null);
			if (ncreature == null)
			{
				this.QueueFreeSafely();
				return;
			}
			this._sprite = base.GetNode<Sprite2D>("Sprite2D");
			base.GlobalPosition = ncreature.VfxSpawnPosition;
			TaskHelper.RunSafely(this.StartVfx());
		}

		// Token: 0x06001840 RID: 6208 RVA: 0x00097B32 File Offset: 0x00095D32
		public override void _ExitTree()
		{
			Tween spriteTween = this._spriteTween;
			if (spriteTween == null)
			{
				return;
			}
			spriteTween.Kill();
		}

		// Token: 0x06001841 RID: 6209 RVA: 0x00097B44 File Offset: 0x00095D44
		[NullableContext(1)]
		private async Task StartVfx()
		{
			this._sprite.Texture = this._power.BigIcon;
			this._sprite.Modulate = Colors.White;
			this._spriteTween = base.CreateTween();
			this._spriteTween.SetParallel(true);
			this._spriteTween.TweenProperty(this._sprite, "scale", Vector2.One * 0.4f, 0.4);
			this._spriteTween.TweenProperty(this._sprite, "modulate:a", 1, 0.25).SetEase(1L).SetTrans(1L);
			this._spriteTween.SetParallel(false);
			this._spriteTween.TweenProperty(this._sprite, "scale", Vector2.One * 0.45f, 0.4);
			this._spriteTween.TweenProperty(this._sprite, "modulate:a", 0, 0.25).SetEase(0L).SetTrans(1L);
			bool flag = await this._spriteTween.AwaitFinished(this);
			this.QueueFreeSafely();
		}

		// Token: 0x06001842 RID: 6210 RVA: 0x00097B88 File Offset: 0x00095D88
		[NullableContext(1)]
		[return: Nullable(2)]
		public static NPowerFlashVfx Create(PowerModel power)
		{
			if (TestMode.IsOn)
			{
				return null;
			}
			if (!power.ShouldPlayVfx)
			{
				return null;
			}
			NPowerFlashVfx npowerFlashVfx = (NPowerFlashVfx)PreloadManager.Cache.GetScene("res://scenes/vfx/power_flash_vfx.tscn").Instantiate(0L);
			npowerFlashVfx._power = power;
			return npowerFlashVfx;
		}

		/// <summary>
		/// Get the method information for all the methods declared in this class.
		/// This method is used by Godot to register the available methods in the editor.
		/// Do not call this method.
		/// </summary>
		// Token: 0x06001843 RID: 6211 RVA: 0x00097BCC File Offset: 0x00095DCC
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal static List<MethodInfo> GetGodotMethodList()
		{
			return new List<MethodInfo>(2)
			{
				new MethodInfo(NPowerFlashVfx.MethodName._Ready, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NPowerFlashVfx.MethodName._ExitTree, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null)
			};
		}

		/// <inheritdoc />
		// Token: 0x06001844 RID: 6212 RVA: 0x00097C40 File Offset: 0x00095E40
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool InvokeGodotClassMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
		{
			if ((ref method) == NPowerFlashVfx.MethodName._Ready && args.Count == 0)
			{
				this._Ready();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NPowerFlashVfx.MethodName._ExitTree && args.Count == 0)
			{
				this._ExitTree();
				ret = default(godot_variant);
				return true;
			}
			return base.InvokeGodotClassMethod(ref method, args, ref ret);
		}

		/// <inheritdoc />
		// Token: 0x06001845 RID: 6213 RVA: 0x00097CA0 File Offset: 0x00095EA0
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool HasGodotClassMethod(in godot_string_name method)
		{
			return (ref method) == NPowerFlashVfx.MethodName._Ready || (ref method) == NPowerFlashVfx.MethodName._ExitTree || base.HasGodotClassMethod(ref method);
		}

		/// <inheritdoc />
		// Token: 0x06001846 RID: 6214 RVA: 0x00097CC7 File Offset: 0x00095EC7
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool SetGodotClassPropertyValue(in godot_string_name name, in godot_variant value)
		{
			if ((ref name) == NPowerFlashVfx.PropertyName._sprite)
			{
				this._sprite = VariantUtils.ConvertTo<Sprite2D>(ref value);
				return true;
			}
			if ((ref name) == NPowerFlashVfx.PropertyName._spriteTween)
			{
				this._spriteTween = VariantUtils.ConvertTo<Tween>(ref value);
				return true;
			}
			return base.SetGodotClassPropertyValue(ref name, ref value);
		}

		/// <inheritdoc />
		// Token: 0x06001847 RID: 6215 RVA: 0x00097D08 File Offset: 0x00095F08
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
		{
			if ((ref name) == NPowerFlashVfx.PropertyName._sprite)
			{
				value = VariantUtils.CreateFrom<Sprite2D>(ref this._sprite);
				return true;
			}
			if ((ref name) == NPowerFlashVfx.PropertyName._spriteTween)
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
		// Token: 0x06001848 RID: 6216 RVA: 0x00097D60 File Offset: 0x00095F60
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal static List<PropertyInfo> GetGodotPropertyList()
		{
			return new List<PropertyInfo>
			{
				new PropertyInfo(24L, NPowerFlashVfx.PropertyName._sprite, 0L, "", 4096L, false),
				new PropertyInfo(24L, NPowerFlashVfx.PropertyName._spriteTween, 0L, "", 4096L, false)
			};
		}

		/// <inheritdoc />
		// Token: 0x06001849 RID: 6217 RVA: 0x00097DB6 File Offset: 0x00095FB6
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void SaveGodotObjectData(GodotSerializationInfo info)
		{
			base.SaveGodotObjectData(info);
			info.AddProperty(NPowerFlashVfx.PropertyName._sprite, Variant.From<Sprite2D>(ref this._sprite));
			info.AddProperty(NPowerFlashVfx.PropertyName._spriteTween, Variant.From<Tween>(ref this._spriteTween));
		}

		/// <inheritdoc />
		// Token: 0x0600184A RID: 6218 RVA: 0x00097DEC File Offset: 0x00095FEC
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void RestoreGodotObjectData(GodotSerializationInfo info)
		{
			base.RestoreGodotObjectData(info);
			Variant variant;
			if (info.TryGetProperty(NPowerFlashVfx.PropertyName._sprite, ref variant))
			{
				this._sprite = variant.As<Sprite2D>();
			}
			Variant variant2;
			if (info.TryGetProperty(NPowerFlashVfx.PropertyName._spriteTween, ref variant2))
			{
				this._spriteTween = variant2.As<Tween>();
			}
		}

		// Token: 0x040008BF RID: 2239
		[Nullable(1)]
		private const string _scenePath = "res://scenes/vfx/power_flash_vfx.tscn";

		// Token: 0x040008C0 RID: 2240
		[Nullable(1)]
		private Sprite2D _sprite;

		// Token: 0x040008C1 RID: 2241
		[Nullable(1)]
		private PowerModel _power;

		// Token: 0x040008C2 RID: 2242
		[Nullable(2)]
		private Tween _spriteTween;

		/// <summary>
		/// Cached StringNames for the methods contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x020010EF RID: 4335
		public class MethodName : Node2D.MethodName
		{
			/// <summary>
			/// Cached name for the '_Ready' method.
			/// </summary>
			// Token: 0x04004249 RID: 16969
			public static readonly StringName _Ready = "_Ready";

			/// <summary>
			/// Cached name for the '_ExitTree' method.
			/// </summary>
			// Token: 0x0400424A RID: 16970
			public static readonly StringName _ExitTree = "_ExitTree";
		}

		/// <summary>
		/// Cached StringNames for the properties and fields contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x020010F0 RID: 4336
		public class PropertyName : Node2D.PropertyName
		{
			/// <summary>
			/// Cached name for the '_sprite' field.
			/// </summary>
			// Token: 0x0400424B RID: 16971
			public static readonly StringName _sprite = "_sprite";

			/// <summary>
			/// Cached name for the '_spriteTween' field.
			/// </summary>
			// Token: 0x0400424C RID: 16972
			public static readonly StringName _spriteTween = "_spriteTween";
		}

		/// <summary>
		/// Cached StringNames for the signals contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x020010F1 RID: 4337
		public class SignalName : Node2D.SignalName
		{
		}
	}
}
