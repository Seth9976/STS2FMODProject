using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Godot;
using Godot.Bridge;
using Godot.NativeInterop;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Nodes.Rooms;

namespace MegaCrit.Sts2.Core.Nodes.Combat
{
	// Token: 0x0200040C RID: 1036
	[ScriptPath("res://src/Core/Nodes/Combat/NRemoteTargetingIndicator.cs")]
	public class NRemoteTargetingIndicator : Node2D
	{
		// Token: 0x06004127 RID: 16679 RVA: 0x001F7514 File Offset: 0x001F5714
		public override void _Ready()
		{
			this._line = base.GetNode<Line2D>("Line");
			this._lineBack = base.GetNode<Line2D>("LineBack");
			for (int i = 0; i < 101; i++)
			{
				this._line.AddPoint(Vector2.Zero, -1);
				this._lineBack.AddPoint(Vector2.Zero, -1);
			}
			this.StopDrawing();
		}

		// Token: 0x06004128 RID: 16680 RVA: 0x001F7584 File Offset: 0x001F5784
		[NullableContext(1)]
		public void Initialize(Player player)
		{
			this._player = player;
			CharacterModel character = player.Character;
			this._line.DefaultColor = character.RemoteTargetingLineColor;
			this._lineBack.DefaultColor = character.RemoteTargetingLineOutline;
			Gradient gradient = this._line.GetGradient();
			if (gradient != null)
			{
				for (int i = 0; i < gradient.GetPointCount(); i++)
				{
					gradient.SetColor(i, gradient.GetColor(i) * character.RemoteTargetingLineColor);
				}
				this._line.SetGradient(gradient);
			}
			Gradient gradient2 = this._lineBack.GetGradient();
			if (gradient2 != null)
			{
				for (int j = 0; j < gradient2.GetPointCount(); j++)
				{
					gradient2.SetColor(j, gradient2.GetColor(j) * character.RemoteTargetingLineOutline);
				}
				this._lineBack.SetGradient(gradient);
			}
		}

		// Token: 0x06004129 RID: 16681 RVA: 0x001F7654 File Offset: 0x001F5854
		public override void _Process(double delta)
		{
			Vector2 zero = Vector2.Zero;
			zero.X = this._fromPosition.X + (this._toPosition.X - this._fromPosition.X) * 0.5f;
			zero.Y = this._fromPosition.Y - (this._toPosition.Y - this._fromPosition.Y) * 0.5f;
			for (int i = 0; i < 100; i++)
			{
				Vector2 vector = MathHelper.BezierCurve(this._fromPosition, this._toPosition, zero, (float)i / 101f);
				this._line.SetPointPosition(i, vector);
				this._lineBack.SetPointPosition(i, vector);
			}
			this._line.SetPointPosition(100, this._toPosition);
			this._lineBack.SetPointPosition(100, this._toPosition);
			bool flag = false;
			ICombatState combatState = this._player.Creature.CombatState;
			foreach (Creature creature in (((combatState != null) ? combatState.Enemies : null) ?? Array.Empty<Creature>()))
			{
				NCombatRoom instance = NCombatRoom.Instance;
				NCreature ncreature = ((instance != null) ? instance.GetCreatureNode(creature) : null);
				if (ncreature != null && ncreature.Hitbox.GetGlobalRect().HasPoint(base.GlobalPosition + this._toPosition))
				{
					flag = true;
					break;
				}
			}
			this.DoTargetingCreatureTween(flag);
		}

		// Token: 0x0600412A RID: 16682 RVA: 0x001F77DC File Offset: 0x001F59DC
		public void StartDrawingFrom(Vector2 from)
		{
			if (NCombatUi.IsDebugHideMpTargetingUi)
			{
				return;
			}
			this._fromPosition = from;
			base.Visible = true;
			base.ProcessMode = (base.Visible ? 0L : 4L);
		}

		// Token: 0x0600412B RID: 16683 RVA: 0x001F7808 File Offset: 0x001F5A08
		public void StopDrawing()
		{
			base.Visible = false;
			base.ProcessMode = 4L;
			Color modulate = base.Modulate;
			modulate.A = 0.5f;
			base.Modulate = modulate;
		}

		// Token: 0x0600412C RID: 16684 RVA: 0x001F783E File Offset: 0x001F5A3E
		public void UpdateDrawingTo(Vector2 position)
		{
			this._toPosition = position;
		}

		// Token: 0x0600412D RID: 16685 RVA: 0x001F7848 File Offset: 0x001F5A48
		private void DoTargetingCreatureTween(bool isTargetingCreature)
		{
			if (isTargetingCreature == this._isTargetingCreature)
			{
				return;
			}
			Tween tween = this._tween;
			if (tween != null)
			{
				tween.Kill();
			}
			this._tween = base.CreateTween();
			if (isTargetingCreature)
			{
				this._tween.TweenProperty(this, "modulate:a", 1f, 0.10000000149011612);
			}
			else
			{
				this._tween.TweenProperty(this, "modulate:a", 0.5f, 0.25);
			}
			this._isTargetingCreature = isTargetingCreature;
		}

		/// <summary>
		/// Get the method information for all the methods declared in this class.
		/// This method is used by Godot to register the available methods in the editor.
		/// Do not call this method.
		/// </summary>
		// Token: 0x0600412E RID: 16686 RVA: 0x001F78DC File Offset: 0x001F5ADC
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal static List<MethodInfo> GetGodotMethodList()
		{
			return new List<MethodInfo>(6)
			{
				new MethodInfo(NRemoteTargetingIndicator.MethodName._Ready, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NRemoteTargetingIndicator.MethodName._Process, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, new List<PropertyInfo>
				{
					new PropertyInfo(3L, "delta", 0L, "", 6L, false)
				}, null),
				new MethodInfo(NRemoteTargetingIndicator.MethodName.StartDrawingFrom, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, new List<PropertyInfo>
				{
					new PropertyInfo(5L, "from", 0L, "", 6L, false)
				}, null),
				new MethodInfo(NRemoteTargetingIndicator.MethodName.StopDrawing, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NRemoteTargetingIndicator.MethodName.UpdateDrawingTo, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, new List<PropertyInfo>
				{
					new PropertyInfo(5L, "position", 0L, "", 6L, false)
				}, null),
				new MethodInfo(NRemoteTargetingIndicator.MethodName.DoTargetingCreatureTween, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, new List<PropertyInfo>
				{
					new PropertyInfo(1L, "isTargetingCreature", 0L, "", 6L, false)
				}, null)
			};
		}

		/// <inheritdoc />
		// Token: 0x0600412F RID: 16687 RVA: 0x001F7AA0 File Offset: 0x001F5CA0
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool InvokeGodotClassMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
		{
			if ((ref method) == NRemoteTargetingIndicator.MethodName._Ready && args.Count == 0)
			{
				this._Ready();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NRemoteTargetingIndicator.MethodName._Process && args.Count == 1)
			{
				this._Process(VariantUtils.ConvertTo<double>(args[0]));
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NRemoteTargetingIndicator.MethodName.StartDrawingFrom && args.Count == 1)
			{
				this.StartDrawingFrom(VariantUtils.ConvertTo<Vector2>(args[0]));
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NRemoteTargetingIndicator.MethodName.StopDrawing && args.Count == 0)
			{
				this.StopDrawing();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NRemoteTargetingIndicator.MethodName.UpdateDrawingTo && args.Count == 1)
			{
				this.UpdateDrawingTo(VariantUtils.ConvertTo<Vector2>(args[0]));
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NRemoteTargetingIndicator.MethodName.DoTargetingCreatureTween && args.Count == 1)
			{
				this.DoTargetingCreatureTween(VariantUtils.ConvertTo<bool>(args[0]));
				ret = default(godot_variant);
				return true;
			}
			return base.InvokeGodotClassMethod(ref method, args, ref ret);
		}

		/// <inheritdoc />
		// Token: 0x06004130 RID: 16688 RVA: 0x001F7BCC File Offset: 0x001F5DCC
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool HasGodotClassMethod(in godot_string_name method)
		{
			return (ref method) == NRemoteTargetingIndicator.MethodName._Ready || (ref method) == NRemoteTargetingIndicator.MethodName._Process || (ref method) == NRemoteTargetingIndicator.MethodName.StartDrawingFrom || (ref method) == NRemoteTargetingIndicator.MethodName.StopDrawing || (ref method) == NRemoteTargetingIndicator.MethodName.UpdateDrawingTo || (ref method) == NRemoteTargetingIndicator.MethodName.DoTargetingCreatureTween || base.HasGodotClassMethod(ref method);
		}

		/// <inheritdoc />
		// Token: 0x06004131 RID: 16689 RVA: 0x001F7C3C File Offset: 0x001F5E3C
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool SetGodotClassPropertyValue(in godot_string_name name, in godot_variant value)
		{
			if ((ref name) == NRemoteTargetingIndicator.PropertyName._fromPosition)
			{
				this._fromPosition = VariantUtils.ConvertTo<Vector2>(ref value);
				return true;
			}
			if ((ref name) == NRemoteTargetingIndicator.PropertyName._toPosition)
			{
				this._toPosition = VariantUtils.ConvertTo<Vector2>(ref value);
				return true;
			}
			if ((ref name) == NRemoteTargetingIndicator.PropertyName._line)
			{
				this._line = VariantUtils.ConvertTo<Line2D>(ref value);
				return true;
			}
			if ((ref name) == NRemoteTargetingIndicator.PropertyName._lineBack)
			{
				this._lineBack = VariantUtils.ConvertTo<Line2D>(ref value);
				return true;
			}
			if ((ref name) == NRemoteTargetingIndicator.PropertyName._tween)
			{
				this._tween = VariantUtils.ConvertTo<Tween>(ref value);
				return true;
			}
			if ((ref name) == NRemoteTargetingIndicator.PropertyName._isTargetingCreature)
			{
				this._isTargetingCreature = VariantUtils.ConvertTo<bool>(ref value);
				return true;
			}
			return base.SetGodotClassPropertyValue(ref name, ref value);
		}

		/// <inheritdoc />
		// Token: 0x06004132 RID: 16690 RVA: 0x001F7CF4 File Offset: 0x001F5EF4
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
		{
			if ((ref name) == NRemoteTargetingIndicator.PropertyName._fromPosition)
			{
				value = VariantUtils.CreateFrom<Vector2>(ref this._fromPosition);
				return true;
			}
			if ((ref name) == NRemoteTargetingIndicator.PropertyName._toPosition)
			{
				value = VariantUtils.CreateFrom<Vector2>(ref this._toPosition);
				return true;
			}
			if ((ref name) == NRemoteTargetingIndicator.PropertyName._line)
			{
				value = VariantUtils.CreateFrom<Line2D>(ref this._line);
				return true;
			}
			if ((ref name) == NRemoteTargetingIndicator.PropertyName._lineBack)
			{
				value = VariantUtils.CreateFrom<Line2D>(ref this._lineBack);
				return true;
			}
			if ((ref name) == NRemoteTargetingIndicator.PropertyName._tween)
			{
				value = VariantUtils.CreateFrom<Tween>(ref this._tween);
				return true;
			}
			if ((ref name) == NRemoteTargetingIndicator.PropertyName._isTargetingCreature)
			{
				value = VariantUtils.CreateFrom<bool>(ref this._isTargetingCreature);
				return true;
			}
			return base.GetGodotClassPropertyValue(ref name, ref value);
		}

		/// <summary>
		/// Get the property information for all the properties declared in this class.
		/// This method is used by Godot to register the available properties in the editor.
		/// Do not call this method.
		/// </summary>
		// Token: 0x06004133 RID: 16691 RVA: 0x001F7DCC File Offset: 0x001F5FCC
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal static List<PropertyInfo> GetGodotPropertyList()
		{
			return new List<PropertyInfo>
			{
				new PropertyInfo(5L, NRemoteTargetingIndicator.PropertyName._fromPosition, 0L, "", 4096L, false),
				new PropertyInfo(5L, NRemoteTargetingIndicator.PropertyName._toPosition, 0L, "", 4096L, false),
				new PropertyInfo(24L, NRemoteTargetingIndicator.PropertyName._line, 0L, "", 4096L, false),
				new PropertyInfo(24L, NRemoteTargetingIndicator.PropertyName._lineBack, 0L, "", 4096L, false),
				new PropertyInfo(24L, NRemoteTargetingIndicator.PropertyName._tween, 0L, "", 4096L, false),
				new PropertyInfo(1L, NRemoteTargetingIndicator.PropertyName._isTargetingCreature, 0L, "", 4096L, false)
			};
		}

		/// <inheritdoc />
		// Token: 0x06004134 RID: 16692 RVA: 0x001F7EA4 File Offset: 0x001F60A4
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void SaveGodotObjectData(GodotSerializationInfo info)
		{
			base.SaveGodotObjectData(info);
			info.AddProperty(NRemoteTargetingIndicator.PropertyName._fromPosition, Variant.From<Vector2>(ref this._fromPosition));
			info.AddProperty(NRemoteTargetingIndicator.PropertyName._toPosition, Variant.From<Vector2>(ref this._toPosition));
			info.AddProperty(NRemoteTargetingIndicator.PropertyName._line, Variant.From<Line2D>(ref this._line));
			info.AddProperty(NRemoteTargetingIndicator.PropertyName._lineBack, Variant.From<Line2D>(ref this._lineBack));
			info.AddProperty(NRemoteTargetingIndicator.PropertyName._tween, Variant.From<Tween>(ref this._tween));
			info.AddProperty(NRemoteTargetingIndicator.PropertyName._isTargetingCreature, Variant.From<bool>(ref this._isTargetingCreature));
		}

		/// <inheritdoc />
		// Token: 0x06004135 RID: 16693 RVA: 0x001F7F3C File Offset: 0x001F613C
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void RestoreGodotObjectData(GodotSerializationInfo info)
		{
			base.RestoreGodotObjectData(info);
			Variant variant;
			if (info.TryGetProperty(NRemoteTargetingIndicator.PropertyName._fromPosition, ref variant))
			{
				this._fromPosition = variant.As<Vector2>();
			}
			Variant variant2;
			if (info.TryGetProperty(NRemoteTargetingIndicator.PropertyName._toPosition, ref variant2))
			{
				this._toPosition = variant2.As<Vector2>();
			}
			Variant variant3;
			if (info.TryGetProperty(NRemoteTargetingIndicator.PropertyName._line, ref variant3))
			{
				this._line = variant3.As<Line2D>();
			}
			Variant variant4;
			if (info.TryGetProperty(NRemoteTargetingIndicator.PropertyName._lineBack, ref variant4))
			{
				this._lineBack = variant4.As<Line2D>();
			}
			Variant variant5;
			if (info.TryGetProperty(NRemoteTargetingIndicator.PropertyName._tween, ref variant5))
			{
				this._tween = variant5.As<Tween>();
			}
			Variant variant6;
			if (info.TryGetProperty(NRemoteTargetingIndicator.PropertyName._isTargetingCreature, ref variant6))
			{
				this._isTargetingCreature = variant6.As<bool>();
			}
		}

		// Token: 0x04001950 RID: 6480
		private const int _segmentCount = 100;

		// Token: 0x04001951 RID: 6481
		private const float _defaultAlpha = 0.5f;

		// Token: 0x04001952 RID: 6482
		private const float _targetingAlpha = 1f;

		// Token: 0x04001953 RID: 6483
		[Nullable(1)]
		private Player _player;

		// Token: 0x04001954 RID: 6484
		private Vector2 _fromPosition;

		// Token: 0x04001955 RID: 6485
		private Vector2 _toPosition;

		// Token: 0x04001956 RID: 6486
		[Nullable(1)]
		private Line2D _line;

		// Token: 0x04001957 RID: 6487
		[Nullable(1)]
		private Line2D _lineBack;

		// Token: 0x04001958 RID: 6488
		[Nullable(2)]
		private Tween _tween;

		// Token: 0x04001959 RID: 6489
		private bool _isTargetingCreature;

		/// <summary>
		/// Cached StringNames for the methods contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x0200191D RID: 6429
		public class MethodName : Node2D.MethodName
		{
			/// <summary>
			/// Cached name for the '_Ready' method.
			/// </summary>
			// Token: 0x040062D6 RID: 25302
			public static readonly StringName _Ready = "_Ready";

			/// <summary>
			/// Cached name for the '_Process' method.
			/// </summary>
			// Token: 0x040062D7 RID: 25303
			public static readonly StringName _Process = "_Process";

			/// <summary>
			/// Cached name for the 'StartDrawingFrom' method.
			/// </summary>
			// Token: 0x040062D8 RID: 25304
			public static readonly StringName StartDrawingFrom = "StartDrawingFrom";

			/// <summary>
			/// Cached name for the 'StopDrawing' method.
			/// </summary>
			// Token: 0x040062D9 RID: 25305
			public static readonly StringName StopDrawing = "StopDrawing";

			/// <summary>
			/// Cached name for the 'UpdateDrawingTo' method.
			/// </summary>
			// Token: 0x040062DA RID: 25306
			public static readonly StringName UpdateDrawingTo = "UpdateDrawingTo";

			/// <summary>
			/// Cached name for the 'DoTargetingCreatureTween' method.
			/// </summary>
			// Token: 0x040062DB RID: 25307
			public static readonly StringName DoTargetingCreatureTween = "DoTargetingCreatureTween";
		}

		/// <summary>
		/// Cached StringNames for the properties and fields contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x0200191E RID: 6430
		public class PropertyName : Node2D.PropertyName
		{
			/// <summary>
			/// Cached name for the '_fromPosition' field.
			/// </summary>
			// Token: 0x040062DC RID: 25308
			public static readonly StringName _fromPosition = "_fromPosition";

			/// <summary>
			/// Cached name for the '_toPosition' field.
			/// </summary>
			// Token: 0x040062DD RID: 25309
			public static readonly StringName _toPosition = "_toPosition";

			/// <summary>
			/// Cached name for the '_line' field.
			/// </summary>
			// Token: 0x040062DE RID: 25310
			public static readonly StringName _line = "_line";

			/// <summary>
			/// Cached name for the '_lineBack' field.
			/// </summary>
			// Token: 0x040062DF RID: 25311
			public static readonly StringName _lineBack = "_lineBack";

			/// <summary>
			/// Cached name for the '_tween' field.
			/// </summary>
			// Token: 0x040062E0 RID: 25312
			public static readonly StringName _tween = "_tween";

			/// <summary>
			/// Cached name for the '_isTargetingCreature' field.
			/// </summary>
			// Token: 0x040062E1 RID: 25313
			public static readonly StringName _isTargetingCreature = "_isTargetingCreature";
		}

		/// <summary>
		/// Cached StringNames for the signals contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x0200191F RID: 6431
		public class SignalName : Node2D.SignalName
		{
		}
	}
}
