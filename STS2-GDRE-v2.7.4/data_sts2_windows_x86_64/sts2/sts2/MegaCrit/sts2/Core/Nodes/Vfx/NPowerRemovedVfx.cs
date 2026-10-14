using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Godot;
using Godot.Bridge;
using Godot.NativeInterop;
using MegaCrit.Sts2.addons.mega_text;
using MegaCrit.Sts2.Core.Assets;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.Localization;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Nodes.Combat;
using MegaCrit.Sts2.Core.Nodes.GodotExtensions;
using MegaCrit.Sts2.Core.Nodes.Rooms;
using MegaCrit.Sts2.Core.TestSupport;

namespace MegaCrit.Sts2.Core.Nodes.Vfx
{
	// Token: 0x020001E0 RID: 480
	[NullableContext(1)]
	[Nullable(0)]
	[ScriptPath("res://src/Core/Nodes/Vfx/NPowerRemovedVfx.cs")]
	public class NPowerRemovedVfx : Node2D
	{
		// Token: 0x17000623 RID: 1571
		// (get) Token: 0x0600184C RID: 6220 RVA: 0x00097E40 File Offset: 0x00096040
		public static IEnumerable<string> AssetPaths
		{
			get
			{
				return new <>z__ReadOnlySingleElementList<string>("res://scenes/vfx/power_removed_vfx.tscn");
			}
		}

		// Token: 0x0600184D RID: 6221 RVA: 0x00097E4C File Offset: 0x0009604C
		public override void _Ready()
		{
			NCombatRoom instance = NCombatRoom.Instance;
			NCreature ncreature = ((instance != null) ? instance.GetCreatureNode(this._power.Owner) : null);
			if (ncreature == null)
			{
				this.QueueFreeSafely();
				return;
			}
			this._sprite = base.GetNode<TextureRect>("%TextureRect");
			this._powerField = base.GetNode<MegaLabel>("%PowerField");
			this._vfxContainer = base.GetNode<Control>("%Container");
			base.GlobalPosition = ncreature.GetTopOfHitbox();
			TaskHelper.RunSafely(this.StartVfx());
		}

		// Token: 0x0600184E RID: 6222 RVA: 0x00097EDA File Offset: 0x000960DA
		public override void _ExitTree()
		{
			Tween textTween = this._textTween;
			if (textTween != null)
			{
				textTween.Kill();
			}
			Tween positionTween = this._positionTween;
			if (positionTween == null)
			{
				return;
			}
			positionTween.Kill();
		}

		// Token: 0x0600184F RID: 6223 RVA: 0x00097F00 File Offset: 0x00096100
		private async Task StartVfx()
		{
			base.GetNode<MegaLabel>("%WearsOff").SetTextAutoSize(NPowerRemovedVfx._wearsOffLoc.GetRawText());
			this._powerField.SetTextAutoSize(this._power.Title.GetFormattedText());
			this._sprite.Texture = this._power.BigIcon;
			this._vfxContainer.Position -= this._vfxContainer.Size * 0.5f;
			this._textTween = base.CreateTween();
			this._textTween.TweenProperty(this._vfxContainer, "modulate:a", 1f, 0.25).SetEase(1L).SetTrans(5L);
			this._textTween.TweenInterval(0.5);
			this._textTween.TweenProperty(this._vfxContainer, "modulate:a", 0f, 1.0).SetEase(1L).SetTrans(0L);
			this._positionTween = base.CreateTween();
			this._positionTween.TweenProperty(this._vfxContainer, "position:y", this._powerField.Position.Y - 160f, 2.0).SetEase(1L).SetTrans(3L);
			bool flag = await this._textTween.AwaitFinished(this);
			this.QueueFreeSafely();
		}

		// Token: 0x06001850 RID: 6224 RVA: 0x00097F44 File Offset: 0x00096144
		[return: Nullable(2)]
		public static NPowerRemovedVfx Create(PowerModel power)
		{
			if (TestMode.IsOn)
			{
				return null;
			}
			if (NCombatUi.IsDebugHideTextVfx)
			{
				return null;
			}
			if (!power.ShouldPlayVfx)
			{
				return null;
			}
			NPowerRemovedVfx npowerRemovedVfx = PreloadManager.Cache.GetScene("res://scenes/vfx/power_removed_vfx.tscn").Instantiate<NPowerRemovedVfx>(0L);
			npowerRemovedVfx._power = power;
			return npowerRemovedVfx;
		}

		/// <summary>
		/// Get the method information for all the methods declared in this class.
		/// This method is used by Godot to register the available methods in the editor.
		/// Do not call this method.
		/// </summary>
		// Token: 0x06001851 RID: 6225 RVA: 0x00097F8C File Offset: 0x0009618C
		[NullableContext(0)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal static List<MethodInfo> GetGodotMethodList()
		{
			return new List<MethodInfo>(2)
			{
				new MethodInfo(NPowerRemovedVfx.MethodName._Ready, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NPowerRemovedVfx.MethodName._ExitTree, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null)
			};
		}

		/// <inheritdoc />
		// Token: 0x06001852 RID: 6226 RVA: 0x00098000 File Offset: 0x00096200
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool InvokeGodotClassMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
		{
			if ((ref method) == NPowerRemovedVfx.MethodName._Ready && args.Count == 0)
			{
				this._Ready();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NPowerRemovedVfx.MethodName._ExitTree && args.Count == 0)
			{
				this._ExitTree();
				ret = default(godot_variant);
				return true;
			}
			return base.InvokeGodotClassMethod(ref method, args, ref ret);
		}

		/// <inheritdoc />
		// Token: 0x06001853 RID: 6227 RVA: 0x00098060 File Offset: 0x00096260
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool HasGodotClassMethod(in godot_string_name method)
		{
			return (ref method) == NPowerRemovedVfx.MethodName._Ready || (ref method) == NPowerRemovedVfx.MethodName._ExitTree || base.HasGodotClassMethod(ref method);
		}

		/// <inheritdoc />
		// Token: 0x06001854 RID: 6228 RVA: 0x00098088 File Offset: 0x00096288
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool SetGodotClassPropertyValue(in godot_string_name name, in godot_variant value)
		{
			if ((ref name) == NPowerRemovedVfx.PropertyName._sprite)
			{
				this._sprite = VariantUtils.ConvertTo<TextureRect>(ref value);
				return true;
			}
			if ((ref name) == NPowerRemovedVfx.PropertyName._powerField)
			{
				this._powerField = VariantUtils.ConvertTo<MegaLabel>(ref value);
				return true;
			}
			if ((ref name) == NPowerRemovedVfx.PropertyName._vfxContainer)
			{
				this._vfxContainer = VariantUtils.ConvertTo<Control>(ref value);
				return true;
			}
			if ((ref name) == NPowerRemovedVfx.PropertyName._textTween)
			{
				this._textTween = VariantUtils.ConvertTo<Tween>(ref value);
				return true;
			}
			if ((ref name) == NPowerRemovedVfx.PropertyName._positionTween)
			{
				this._positionTween = VariantUtils.ConvertTo<Tween>(ref value);
				return true;
			}
			return base.SetGodotClassPropertyValue(ref name, ref value);
		}

		/// <inheritdoc />
		// Token: 0x06001855 RID: 6229 RVA: 0x00098124 File Offset: 0x00096324
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
		{
			if ((ref name) == NPowerRemovedVfx.PropertyName._sprite)
			{
				value = VariantUtils.CreateFrom<TextureRect>(ref this._sprite);
				return true;
			}
			if ((ref name) == NPowerRemovedVfx.PropertyName._powerField)
			{
				value = VariantUtils.CreateFrom<MegaLabel>(ref this._powerField);
				return true;
			}
			if ((ref name) == NPowerRemovedVfx.PropertyName._vfxContainer)
			{
				value = VariantUtils.CreateFrom<Control>(ref this._vfxContainer);
				return true;
			}
			if ((ref name) == NPowerRemovedVfx.PropertyName._textTween)
			{
				value = VariantUtils.CreateFrom<Tween>(ref this._textTween);
				return true;
			}
			if ((ref name) == NPowerRemovedVfx.PropertyName._positionTween)
			{
				value = VariantUtils.CreateFrom<Tween>(ref this._positionTween);
				return true;
			}
			return base.GetGodotClassPropertyValue(ref name, ref value);
		}

		/// <summary>
		/// Get the property information for all the properties declared in this class.
		/// This method is used by Godot to register the available properties in the editor.
		/// Do not call this method.
		/// </summary>
		// Token: 0x06001856 RID: 6230 RVA: 0x000981DC File Offset: 0x000963DC
		[NullableContext(0)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal static List<PropertyInfo> GetGodotPropertyList()
		{
			return new List<PropertyInfo>
			{
				new PropertyInfo(24L, NPowerRemovedVfx.PropertyName._sprite, 0L, "", 4096L, false),
				new PropertyInfo(24L, NPowerRemovedVfx.PropertyName._powerField, 0L, "", 4096L, false),
				new PropertyInfo(24L, NPowerRemovedVfx.PropertyName._vfxContainer, 0L, "", 4096L, false),
				new PropertyInfo(24L, NPowerRemovedVfx.PropertyName._textTween, 0L, "", 4096L, false),
				new PropertyInfo(24L, NPowerRemovedVfx.PropertyName._positionTween, 0L, "", 4096L, false)
			};
		}

		/// <inheritdoc />
		// Token: 0x06001857 RID: 6231 RVA: 0x00098298 File Offset: 0x00096498
		[NullableContext(0)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void SaveGodotObjectData(GodotSerializationInfo info)
		{
			base.SaveGodotObjectData(info);
			info.AddProperty(NPowerRemovedVfx.PropertyName._sprite, Variant.From<TextureRect>(ref this._sprite));
			info.AddProperty(NPowerRemovedVfx.PropertyName._powerField, Variant.From<MegaLabel>(ref this._powerField));
			info.AddProperty(NPowerRemovedVfx.PropertyName._vfxContainer, Variant.From<Control>(ref this._vfxContainer));
			info.AddProperty(NPowerRemovedVfx.PropertyName._textTween, Variant.From<Tween>(ref this._textTween));
			info.AddProperty(NPowerRemovedVfx.PropertyName._positionTween, Variant.From<Tween>(ref this._positionTween));
		}

		/// <inheritdoc />
		// Token: 0x06001858 RID: 6232 RVA: 0x0009831C File Offset: 0x0009651C
		[NullableContext(0)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void RestoreGodotObjectData(GodotSerializationInfo info)
		{
			base.RestoreGodotObjectData(info);
			Variant variant;
			if (info.TryGetProperty(NPowerRemovedVfx.PropertyName._sprite, ref variant))
			{
				this._sprite = variant.As<TextureRect>();
			}
			Variant variant2;
			if (info.TryGetProperty(NPowerRemovedVfx.PropertyName._powerField, ref variant2))
			{
				this._powerField = variant2.As<MegaLabel>();
			}
			Variant variant3;
			if (info.TryGetProperty(NPowerRemovedVfx.PropertyName._vfxContainer, ref variant3))
			{
				this._vfxContainer = variant3.As<Control>();
			}
			Variant variant4;
			if (info.TryGetProperty(NPowerRemovedVfx.PropertyName._textTween, ref variant4))
			{
				this._textTween = variant4.As<Tween>();
			}
			Variant variant5;
			if (info.TryGetProperty(NPowerRemovedVfx.PropertyName._positionTween, ref variant5))
			{
				this._positionTween = variant5.As<Tween>();
			}
		}

		// Token: 0x040008C3 RID: 2243
		private const string _scenePath = "res://scenes/vfx/power_removed_vfx.tscn";

		// Token: 0x040008C4 RID: 2244
		private static LocString _wearsOffLoc = new LocString("vfx", "POWER_WEARS_OFF");

		// Token: 0x040008C5 RID: 2245
		private TextureRect _sprite;

		// Token: 0x040008C6 RID: 2246
		private MegaLabel _powerField;

		// Token: 0x040008C7 RID: 2247
		private Control _vfxContainer;

		// Token: 0x040008C8 RID: 2248
		private PowerModel _power;

		// Token: 0x040008C9 RID: 2249
		[Nullable(2)]
		private Tween _textTween;

		// Token: 0x040008CA RID: 2250
		[Nullable(2)]
		private Tween _positionTween;

		/// <summary>
		/// Cached StringNames for the methods contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x020010F3 RID: 4339
		[NullableContext(0)]
		public class MethodName : Node2D.MethodName
		{
			/// <summary>
			/// Cached name for the '_Ready' method.
			/// </summary>
			// Token: 0x04004251 RID: 16977
			public static readonly StringName _Ready = "_Ready";

			/// <summary>
			/// Cached name for the '_ExitTree' method.
			/// </summary>
			// Token: 0x04004252 RID: 16978
			public static readonly StringName _ExitTree = "_ExitTree";
		}

		/// <summary>
		/// Cached StringNames for the properties and fields contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x020010F4 RID: 4340
		[NullableContext(0)]
		public class PropertyName : Node2D.PropertyName
		{
			/// <summary>
			/// Cached name for the '_sprite' field.
			/// </summary>
			// Token: 0x04004253 RID: 16979
			public static readonly StringName _sprite = "_sprite";

			/// <summary>
			/// Cached name for the '_powerField' field.
			/// </summary>
			// Token: 0x04004254 RID: 16980
			public static readonly StringName _powerField = "_powerField";

			/// <summary>
			/// Cached name for the '_vfxContainer' field.
			/// </summary>
			// Token: 0x04004255 RID: 16981
			public static readonly StringName _vfxContainer = "_vfxContainer";

			/// <summary>
			/// Cached name for the '_textTween' field.
			/// </summary>
			// Token: 0x04004256 RID: 16982
			public static readonly StringName _textTween = "_textTween";

			/// <summary>
			/// Cached name for the '_positionTween' field.
			/// </summary>
			// Token: 0x04004257 RID: 16983
			public static readonly StringName _positionTween = "_positionTween";
		}

		/// <summary>
		/// Cached StringNames for the signals contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x020010F5 RID: 4341
		[NullableContext(0)]
		public class SignalName : Node2D.SignalName
		{
		}
	}
}
