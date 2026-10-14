using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Godot;
using Godot.Bridge;
using Godot.NativeInterop;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Nodes.GodotExtensions;

namespace MegaCrit.Sts2.Core.Nodes.Screens.Map
{
	/// <summary>
	/// The little arrow which indicates where you are on the map.
	/// </summary>
	// Token: 0x020002F6 RID: 758
	[ScriptPath("res://src/Core/Nodes/Screens/Map/NMapMarker.cs")]
	public class NMapMarker : TextureRect
	{
		// Token: 0x060028EA RID: 10474 RVA: 0x0011992A File Offset: 0x00117B2A
		public override void _Ready()
		{
			this._posOffset = new Vector2(-base.Size.X / 2f, -35f);
		}

		// Token: 0x060028EB RID: 10475 RVA: 0x0011994E File Offset: 0x00117B4E
		[NullableContext(1)]
		public void Initialize(Player player)
		{
			this._isEnabled = player.RunState.Players.Count == 1;
			base.Texture = player.Character.MapMarker;
			base.Visible = false;
		}

		// Token: 0x060028EC RID: 10476 RVA: 0x00119981 File Offset: 0x00117B81
		public void ResetMapPoint()
		{
			base.Visible = false;
		}

		// Token: 0x060028ED RID: 10477 RVA: 0x0011998C File Offset: 0x00117B8C
		public void HideMapPoint()
		{
			if (!this._isEnabled)
			{
				return;
			}
			Tween tween = this._tween;
			if (tween != null)
			{
				tween.FastForwardToCompletion();
			}
			this._tween = base.CreateTween();
			this._tween.TweenProperty(this, "scale", Vector2.Zero, 0.20000000298023224).From(Vector2.One);
			this._tween.TweenCallback(Callable.From(delegate
			{
				base.Visible = false;
			}));
		}

		// Token: 0x060028EE RID: 10478 RVA: 0x00119A18 File Offset: 0x00117C18
		[NullableContext(1)]
		public void SetMapPoint(NMapPoint node)
		{
			if (!this._isEnabled)
			{
				return;
			}
			Tween tween = this._tween;
			if (tween != null)
			{
				tween.FastForwardToCompletion();
			}
			if (base.Visible)
			{
				return;
			}
			base.Visible = true;
			Vector2 vector;
			vector..ctor(node.Size.X / 2f, 0f);
			base.Position = node.Position + vector + this._posOffset;
			this._tween = base.CreateTween();
			this._tween.TweenProperty(this, "scale", Vector2.One, 0.20000000298023224).From(Vector2.Down);
			this._tween.Parallel().TweenProperty(this, "position", base.Position + Vector2.Up * 25f, 0.20000000298023224).SetEase(0L)
				.SetTrans(1L)
				.FromCurrent();
			this._tween.TweenProperty(this, "position", base.Position, 0.75).SetEase(1L).SetTrans(6L);
		}

		/// <summary>
		/// Get the method information for all the methods declared in this class.
		/// This method is used by Godot to register the available methods in the editor.
		/// Do not call this method.
		/// </summary>
		// Token: 0x060028EF RID: 10479 RVA: 0x00119B60 File Offset: 0x00117D60
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal static List<MethodInfo> GetGodotMethodList()
		{
			return new List<MethodInfo>(4)
			{
				new MethodInfo(NMapMarker.MethodName._Ready, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NMapMarker.MethodName.ResetMapPoint, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NMapMarker.MethodName.HideMapPoint, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NMapMarker.MethodName.SetMapPoint, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, new List<PropertyInfo>
				{
					new PropertyInfo(24L, "node", 0L, "", 6L, new StringName("Control"), false)
				}, null)
			};
		}

		/// <inheritdoc />
		// Token: 0x060028F0 RID: 10480 RVA: 0x00119C64 File Offset: 0x00117E64
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool InvokeGodotClassMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
		{
			if ((ref method) == NMapMarker.MethodName._Ready && args.Count == 0)
			{
				this._Ready();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NMapMarker.MethodName.ResetMapPoint && args.Count == 0)
			{
				this.ResetMapPoint();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NMapMarker.MethodName.HideMapPoint && args.Count == 0)
			{
				this.HideMapPoint();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NMapMarker.MethodName.SetMapPoint && args.Count == 1)
			{
				this.SetMapPoint(VariantUtils.ConvertTo<NMapPoint>(args[0]));
				ret = default(godot_variant);
				return true;
			}
			return base.InvokeGodotClassMethod(ref method, args, ref ret);
		}

		/// <inheritdoc />
		// Token: 0x060028F1 RID: 10481 RVA: 0x00119D1C File Offset: 0x00117F1C
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool HasGodotClassMethod(in godot_string_name method)
		{
			return (ref method) == NMapMarker.MethodName._Ready || (ref method) == NMapMarker.MethodName.ResetMapPoint || (ref method) == NMapMarker.MethodName.HideMapPoint || (ref method) == NMapMarker.MethodName.SetMapPoint || base.HasGodotClassMethod(ref method);
		}

		/// <inheritdoc />
		// Token: 0x060028F2 RID: 10482 RVA: 0x00119D6C File Offset: 0x00117F6C
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool SetGodotClassPropertyValue(in godot_string_name name, in godot_variant value)
		{
			if ((ref name) == NMapMarker.PropertyName._tween)
			{
				this._tween = VariantUtils.ConvertTo<Tween>(ref value);
				return true;
			}
			if ((ref name) == NMapMarker.PropertyName._posOffset)
			{
				this._posOffset = VariantUtils.ConvertTo<Vector2>(ref value);
				return true;
			}
			if ((ref name) == NMapMarker.PropertyName._isEnabled)
			{
				this._isEnabled = VariantUtils.ConvertTo<bool>(ref value);
				return true;
			}
			return base.SetGodotClassPropertyValue(ref name, ref value);
		}

		/// <inheritdoc />
		// Token: 0x060028F3 RID: 10483 RVA: 0x00119DD4 File Offset: 0x00117FD4
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
		{
			if ((ref name) == NMapMarker.PropertyName._tween)
			{
				value = VariantUtils.CreateFrom<Tween>(ref this._tween);
				return true;
			}
			if ((ref name) == NMapMarker.PropertyName._posOffset)
			{
				value = VariantUtils.CreateFrom<Vector2>(ref this._posOffset);
				return true;
			}
			if ((ref name) == NMapMarker.PropertyName._isEnabled)
			{
				value = VariantUtils.CreateFrom<bool>(ref this._isEnabled);
				return true;
			}
			return base.GetGodotClassPropertyValue(ref name, ref value);
		}

		/// <summary>
		/// Get the property information for all the properties declared in this class.
		/// This method is used by Godot to register the available properties in the editor.
		/// Do not call this method.
		/// </summary>
		// Token: 0x060028F4 RID: 10484 RVA: 0x00119E4C File Offset: 0x0011804C
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal static List<PropertyInfo> GetGodotPropertyList()
		{
			return new List<PropertyInfo>
			{
				new PropertyInfo(24L, NMapMarker.PropertyName._tween, 0L, "", 4096L, false),
				new PropertyInfo(5L, NMapMarker.PropertyName._posOffset, 0L, "", 4096L, false),
				new PropertyInfo(1L, NMapMarker.PropertyName._isEnabled, 0L, "", 4096L, false)
			};
		}

		/// <inheritdoc />
		// Token: 0x060028F5 RID: 10485 RVA: 0x00119EC4 File Offset: 0x001180C4
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void SaveGodotObjectData(GodotSerializationInfo info)
		{
			base.SaveGodotObjectData(info);
			info.AddProperty(NMapMarker.PropertyName._tween, Variant.From<Tween>(ref this._tween));
			info.AddProperty(NMapMarker.PropertyName._posOffset, Variant.From<Vector2>(ref this._posOffset));
			info.AddProperty(NMapMarker.PropertyName._isEnabled, Variant.From<bool>(ref this._isEnabled));
		}

		/// <inheritdoc />
		// Token: 0x060028F6 RID: 10486 RVA: 0x00119F1C File Offset: 0x0011811C
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void RestoreGodotObjectData(GodotSerializationInfo info)
		{
			base.RestoreGodotObjectData(info);
			Variant variant;
			if (info.TryGetProperty(NMapMarker.PropertyName._tween, ref variant))
			{
				this._tween = variant.As<Tween>();
			}
			Variant variant2;
			if (info.TryGetProperty(NMapMarker.PropertyName._posOffset, ref variant2))
			{
				this._posOffset = variant2.As<Vector2>();
			}
			Variant variant3;
			if (info.TryGetProperty(NMapMarker.PropertyName._isEnabled, ref variant3))
			{
				this._isEnabled = variant3.As<bool>();
			}
		}

		// Token: 0x04000F47 RID: 3911
		[Nullable(2)]
		private Tween _tween;

		// Token: 0x04000F48 RID: 3912
		private Vector2 _posOffset;

		// Token: 0x04000F49 RID: 3913
		private bool _isEnabled;

		/// <summary>
		/// Cached StringNames for the methods contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x020014B9 RID: 5305
		public class MethodName : TextureRect.MethodName
		{
			/// <summary>
			/// Cached name for the '_Ready' method.
			/// </summary>
			// Token: 0x04004E9F RID: 20127
			public static readonly StringName _Ready = "_Ready";

			/// <summary>
			/// Cached name for the 'ResetMapPoint' method.
			/// </summary>
			// Token: 0x04004EA0 RID: 20128
			public static readonly StringName ResetMapPoint = "ResetMapPoint";

			/// <summary>
			/// Cached name for the 'HideMapPoint' method.
			/// </summary>
			// Token: 0x04004EA1 RID: 20129
			public static readonly StringName HideMapPoint = "HideMapPoint";

			/// <summary>
			/// Cached name for the 'SetMapPoint' method.
			/// </summary>
			// Token: 0x04004EA2 RID: 20130
			public static readonly StringName SetMapPoint = "SetMapPoint";
		}

		/// <summary>
		/// Cached StringNames for the properties and fields contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x020014BA RID: 5306
		public class PropertyName : TextureRect.PropertyName
		{
			/// <summary>
			/// Cached name for the '_tween' field.
			/// </summary>
			// Token: 0x04004EA3 RID: 20131
			public static readonly StringName _tween = "_tween";

			/// <summary>
			/// Cached name for the '_posOffset' field.
			/// </summary>
			// Token: 0x04004EA4 RID: 20132
			public static readonly StringName _posOffset = "_posOffset";

			/// <summary>
			/// Cached name for the '_isEnabled' field.
			/// </summary>
			// Token: 0x04004EA5 RID: 20133
			public static readonly StringName _isEnabled = "_isEnabled";
		}

		/// <summary>
		/// Cached StringNames for the signals contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x020014BB RID: 5307
		public class SignalName : TextureRect.SignalName
		{
		}
	}
}
