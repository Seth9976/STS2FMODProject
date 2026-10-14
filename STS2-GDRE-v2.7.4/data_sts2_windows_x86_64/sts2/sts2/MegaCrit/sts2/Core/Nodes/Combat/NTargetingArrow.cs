using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Godot;
using Godot.Bridge;
using Godot.NativeInterop;
using MegaCrit.Sts2.Core.Assets;
using MegaCrit.Sts2.Core.Helpers;

namespace MegaCrit.Sts2.Core.Nodes.Combat
{
	// Token: 0x02000410 RID: 1040
	[NullableContext(1)]
	[Nullable(0)]
	[ScriptPath("res://src/Core/Nodes/Combat/NTargetingArrow.cs")]
	public class NTargetingArrow : Node2D
	{
		// Token: 0x17000959 RID: 2393
		// (get) Token: 0x06004170 RID: 16752 RVA: 0x001F9C0F File Offset: 0x001F7E0F
		private Vector2 From
		{
			get
			{
				Control fromControl = this._fromControl;
				if (fromControl == null)
				{
					return this._fromPos;
				}
				return fromControl.GlobalPosition;
			}
		}

		// Token: 0x1700095A RID: 2394
		// (get) Token: 0x06004171 RID: 16753 RVA: 0x001F9C27 File Offset: 0x001F7E27
		private static Texture2D SegmentHead
		{
			get
			{
				return PreloadManager.Cache.GetTexture2D(NTargetingArrow._segmentHeadPath);
			}
		}

		// Token: 0x1700095B RID: 2395
		// (get) Token: 0x06004172 RID: 16754 RVA: 0x001F9C38 File Offset: 0x001F7E38
		private static Texture2D SegmentBlock
		{
			get
			{
				return PreloadManager.Cache.GetTexture2D(NTargetingArrow._segmentBlockPath);
			}
		}

		// Token: 0x1700095C RID: 2396
		// (get) Token: 0x06004173 RID: 16755 RVA: 0x001F9C49 File Offset: 0x001F7E49
		public static IEnumerable<string> AssetPaths
		{
			get
			{
				return new <>z__ReadOnlyArray<string>(new string[]
				{
					NTargetingArrow._segmentHeadPath,
					NTargetingArrow._segmentBlockPath
				});
			}
		}

		// Token: 0x06004174 RID: 16756 RVA: 0x001F9C68 File Offset: 0x001F7E68
		public override void _Ready()
		{
			if (this._initialized)
			{
				return;
			}
			this._initialized = true;
			for (int i = 0; i < 19; i++)
			{
				this._segments[i] = new Sprite2D();
				this._segments[i].Texture = NTargetingArrow.SegmentBlock;
				this.AddChildSafely(this._segments[i]);
			}
			this._arrowHead = new Sprite2D();
			this._arrowHead.Texture = NTargetingArrow.SegmentHead;
			this.AddChildSafely(this._arrowHead);
			this.StopDrawing();
		}

		// Token: 0x06004175 RID: 16757 RVA: 0x001F9CEC File Offset: 0x001F7EEC
		public override void _Process(double delta)
		{
			if (!base.Visible)
			{
				return;
			}
			if (this._followMouse)
			{
				this.UpdateDrawingTo(base.GetViewport().GetMousePosition());
				this.UpdateArrowPosition(this._toPosition);
				return;
			}
			if (this._currentArrowPos == null)
			{
				return;
			}
			this._currentArrowPos = new Vector2?(this._currentArrowPos.Value.Lerp(this._toPosition, (float)delta * 14f));
			this.UpdateArrowPosition(this._currentArrowPos.Value);
		}

		// Token: 0x06004176 RID: 16758 RVA: 0x001F9D74 File Offset: 0x001F7F74
		private void UpdateArrowPosition(Vector2 targetPos)
		{
			this._arrowHead.Position = targetPos + new Vector2(0f, 88f).Rotated(this._arrowHead.Rotation);
			Vector2 vector = targetPos + new Vector2(0f, 40f).Rotated(this._arrowHead.Rotation);
			Vector2 zero = Vector2.Zero;
			zero.X = this.From.X - (this._arrowHead.Position.X - this.From.X) * 0.25f;
			if (this.From.Y > 540f)
			{
				zero.Y = this._arrowHead.Position.Y + (this._arrowHead.Position.Y - this.From.Y) * 0.5f;
			}
			else
			{
				zero.Y = this._arrowHead.Position.Y * 0.75f + this.From.Y * 0.25f;
			}
			this._arrowHead.Rotation = (targetPos - zero).Angle() + 1.5707964f;
			this.UpdateSegments(this.From, vector, zero);
		}

		// Token: 0x06004177 RID: 16759 RVA: 0x001F9EC4 File Offset: 0x001F80C4
		public void SetHighlightingOn(bool isEnemy)
		{
			Tween arrowHeadTween = this._arrowHeadTween;
			if (arrowHeadTween != null)
			{
				arrowHeadTween.Kill();
			}
			this._arrowHeadTween = base.CreateTween();
			this._arrowHeadTween.TweenProperty(this._arrowHead, "scale", NTargetingArrow._arrowHeadHoverScale, 1.0).SetEase(1L).SetTrans(6L);
			base.Modulate = (isEnemy ? StsColors.targetingArrowEnemy : StsColors.targetingArrowAlly);
		}

		// Token: 0x06004178 RID: 16760 RVA: 0x001F9F40 File Offset: 0x001F8140
		public void SetHighlightingOff()
		{
			Tween arrowHeadTween = this._arrowHeadTween;
			if (arrowHeadTween != null)
			{
				arrowHeadTween.Kill();
			}
			this._arrowHead.Scale = NTargetingArrow._arrowHeadDefaultScale;
			base.Modulate = Colors.White;
		}

		// Token: 0x06004179 RID: 16761 RVA: 0x001F9F70 File Offset: 0x001F8170
		private void UpdateSegments(Vector2 initialPos, Vector2 finalPos, Vector2 controlPoint)
		{
			for (int i = 0; i < 19; i++)
			{
				this._segments[i].Scale = Vector2.One * Mathf.Lerp(0.28f, 0.42f, (float)i * 2f / 19f);
				this._segments[i].Position = MathHelper.BezierCurve(initialPos, finalPos, controlPoint, (float)i / 20f);
				if (i == 0)
				{
					this._segments[i].Rotation = ((this._segments[i].GlobalPosition.Y > 540f) ? 0f : 3.1415927f);
				}
				else
				{
					this._segments[i].Rotation = (this._segments[i].Position - this._segments[i - 1].Position).Angle() + 1.5707964f;
				}
			}
			this._segments[0].Rotation = (this._segments[0].Position - this._segments[1].Position).Angle() - 1.5707964f;
		}

		// Token: 0x0600417A RID: 16762 RVA: 0x001FA090 File Offset: 0x001F8290
		public void StartDrawingFrom(Vector2 from, bool usingController)
		{
			this._followMouse = !usingController;
			if (this._followMouse)
			{
				Input.MouseMode = 1L;
			}
			this._fromPos = from;
			if (usingController)
			{
				this._currentArrowPos = new Vector2?(from);
				this._toPosition = from;
			}
			else
			{
				this._currentArrowPos = null;
			}
			base.Visible = !NCombatUi.IsDebugHideTargetingUi;
		}

		// Token: 0x0600417B RID: 16763 RVA: 0x001FA0F0 File Offset: 0x001F82F0
		public void StartDrawingFrom(Control control, bool usingController)
		{
			this._followMouse = !usingController;
			base.ZIndex = control.ZIndex + 1;
			this._fromControl = control;
			if (usingController)
			{
				this._currentArrowPos = new Vector2?(control.GlobalPosition);
				this._toPosition = control.GlobalPosition;
			}
			else
			{
				Input.MouseMode = 1L;
				this._currentArrowPos = null;
			}
			base.Visible = !NCombatUi.IsDebugHideTargetingUi;
		}

		// Token: 0x0600417C RID: 16764 RVA: 0x001FA15F File Offset: 0x001F835F
		public void StopDrawing()
		{
			if (this._followMouse)
			{
				Input.MouseMode = 0L;
			}
			this._fromControl = null;
			this._currentArrowPos = null;
			base.Visible = false;
			this.SetHighlightingOff();
		}

		// Token: 0x0600417D RID: 16765 RVA: 0x001FA190 File Offset: 0x001F8390
		public override void _ExitTree()
		{
			if (this._followMouse)
			{
				Input.MouseMode = 0L;
			}
		}

		// Token: 0x0600417E RID: 16766 RVA: 0x001FA1A1 File Offset: 0x001F83A1
		public void UpdateDrawingTo(Vector2 position)
		{
			this._toPosition = position;
			if (!this._followMouse && this._currentArrowPos == null)
			{
				this._currentArrowPos = new Vector2?(this._toPosition);
			}
		}

		/// <summary>
		/// Get the method information for all the methods declared in this class.
		/// This method is used by Godot to register the available methods in the editor.
		/// Do not call this method.
		/// </summary>
		// Token: 0x0600417F RID: 16767 RVA: 0x001FA1D0 File Offset: 0x001F83D0
		[NullableContext(0)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal static List<MethodInfo> GetGodotMethodList()
		{
			return new List<MethodInfo>(10)
			{
				new MethodInfo(NTargetingArrow.MethodName._Ready, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NTargetingArrow.MethodName._Process, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, new List<PropertyInfo>
				{
					new PropertyInfo(3L, "delta", 0L, "", 6L, false)
				}, null),
				new MethodInfo(NTargetingArrow.MethodName.UpdateArrowPosition, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, new List<PropertyInfo>
				{
					new PropertyInfo(5L, "targetPos", 0L, "", 6L, false)
				}, null),
				new MethodInfo(NTargetingArrow.MethodName.SetHighlightingOn, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, new List<PropertyInfo>
				{
					new PropertyInfo(1L, "isEnemy", 0L, "", 6L, false)
				}, null),
				new MethodInfo(NTargetingArrow.MethodName.SetHighlightingOff, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NTargetingArrow.MethodName.UpdateSegments, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, new List<PropertyInfo>
				{
					new PropertyInfo(5L, "initialPos", 0L, "", 6L, false),
					new PropertyInfo(5L, "finalPos", 0L, "", 6L, false),
					new PropertyInfo(5L, "controlPoint", 0L, "", 6L, false)
				}, null),
				new MethodInfo(NTargetingArrow.MethodName.StartDrawingFrom, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, new List<PropertyInfo>
				{
					new PropertyInfo(5L, "from", 0L, "", 6L, false),
					new PropertyInfo(1L, "usingController", 0L, "", 6L, false)
				}, null),
				new MethodInfo(NTargetingArrow.MethodName.StopDrawing, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NTargetingArrow.MethodName._ExitTree, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NTargetingArrow.MethodName.UpdateDrawingTo, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, new List<PropertyInfo>
				{
					new PropertyInfo(5L, "position", 0L, "", 6L, false)
				}, null)
			};
		}

		/// <inheritdoc />
		// Token: 0x06004180 RID: 16768 RVA: 0x001FA500 File Offset: 0x001F8700
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool InvokeGodotClassMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
		{
			if ((ref method) == NTargetingArrow.MethodName._Ready && args.Count == 0)
			{
				this._Ready();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NTargetingArrow.MethodName._Process && args.Count == 1)
			{
				this._Process(VariantUtils.ConvertTo<double>(args[0]));
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NTargetingArrow.MethodName.UpdateArrowPosition && args.Count == 1)
			{
				this.UpdateArrowPosition(VariantUtils.ConvertTo<Vector2>(args[0]));
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NTargetingArrow.MethodName.SetHighlightingOn && args.Count == 1)
			{
				this.SetHighlightingOn(VariantUtils.ConvertTo<bool>(args[0]));
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NTargetingArrow.MethodName.SetHighlightingOff && args.Count == 0)
			{
				this.SetHighlightingOff();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NTargetingArrow.MethodName.UpdateSegments && args.Count == 3)
			{
				this.UpdateSegments(VariantUtils.ConvertTo<Vector2>(args[0]), VariantUtils.ConvertTo<Vector2>(args[1]), VariantUtils.ConvertTo<Vector2>(args[2]));
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NTargetingArrow.MethodName.StartDrawingFrom && args.Count == 2)
			{
				this.StartDrawingFrom(VariantUtils.ConvertTo<Vector2>(args[0]), VariantUtils.ConvertTo<bool>(args[1]));
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NTargetingArrow.MethodName.StopDrawing && args.Count == 0)
			{
				this.StopDrawing();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NTargetingArrow.MethodName._ExitTree && args.Count == 0)
			{
				this._ExitTree();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NTargetingArrow.MethodName.UpdateDrawingTo && args.Count == 1)
			{
				this.UpdateDrawingTo(VariantUtils.ConvertTo<Vector2>(args[0]));
				ret = default(godot_variant);
				return true;
			}
			return base.InvokeGodotClassMethod(ref method, args, ref ret);
		}

		/// <inheritdoc />
		// Token: 0x06004181 RID: 16769 RVA: 0x001FA704 File Offset: 0x001F8904
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool HasGodotClassMethod(in godot_string_name method)
		{
			return (ref method) == NTargetingArrow.MethodName._Ready || (ref method) == NTargetingArrow.MethodName._Process || (ref method) == NTargetingArrow.MethodName.UpdateArrowPosition || (ref method) == NTargetingArrow.MethodName.SetHighlightingOn || (ref method) == NTargetingArrow.MethodName.SetHighlightingOff || (ref method) == NTargetingArrow.MethodName.UpdateSegments || (ref method) == NTargetingArrow.MethodName.StartDrawingFrom || (ref method) == NTargetingArrow.MethodName.StopDrawing || (ref method) == NTargetingArrow.MethodName._ExitTree || (ref method) == NTargetingArrow.MethodName.UpdateDrawingTo || base.HasGodotClassMethod(ref method);
		}

		/// <inheritdoc />
		// Token: 0x06004182 RID: 16770 RVA: 0x001FA7B0 File Offset: 0x001F89B0
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool SetGodotClassPropertyValue(in godot_string_name name, in godot_variant value)
		{
			if ((ref name) == NTargetingArrow.PropertyName._fromPos)
			{
				this._fromPos = VariantUtils.ConvertTo<Vector2>(ref value);
				return true;
			}
			if ((ref name) == NTargetingArrow.PropertyName._fromControl)
			{
				this._fromControl = VariantUtils.ConvertTo<Control>(ref value);
				return true;
			}
			if ((ref name) == NTargetingArrow.PropertyName._toPosition)
			{
				this._toPosition = VariantUtils.ConvertTo<Vector2>(ref value);
				return true;
			}
			if ((ref name) == NTargetingArrow.PropertyName._segments)
			{
				this._segments = VariantUtils.ConvertToSystemArrayOfGodotObject<Sprite2D>(ref value);
				return true;
			}
			if ((ref name) == NTargetingArrow.PropertyName._arrowHead)
			{
				this._arrowHead = VariantUtils.ConvertTo<Sprite2D>(ref value);
				return true;
			}
			if ((ref name) == NTargetingArrow.PropertyName._arrowHeadTween)
			{
				this._arrowHeadTween = VariantUtils.ConvertTo<Tween>(ref value);
				return true;
			}
			if ((ref name) == NTargetingArrow.PropertyName._initialized)
			{
				this._initialized = VariantUtils.ConvertTo<bool>(ref value);
				return true;
			}
			if ((ref name) == NTargetingArrow.PropertyName._followMouse)
			{
				this._followMouse = VariantUtils.ConvertTo<bool>(ref value);
				return true;
			}
			return base.SetGodotClassPropertyValue(ref name, ref value);
		}

		/// <inheritdoc />
		// Token: 0x06004183 RID: 16771 RVA: 0x001FA8A0 File Offset: 0x001F8AA0
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
		{
			if ((ref name) == NTargetingArrow.PropertyName.From)
			{
				Vector2 from = this.From;
				value = VariantUtils.CreateFrom<Vector2>(ref from);
				return true;
			}
			if ((ref name) == NTargetingArrow.PropertyName._fromPos)
			{
				value = VariantUtils.CreateFrom<Vector2>(ref this._fromPos);
				return true;
			}
			if ((ref name) == NTargetingArrow.PropertyName._fromControl)
			{
				value = VariantUtils.CreateFrom<Control>(ref this._fromControl);
				return true;
			}
			if ((ref name) == NTargetingArrow.PropertyName._toPosition)
			{
				value = VariantUtils.CreateFrom<Vector2>(ref this._toPosition);
				return true;
			}
			if ((ref name) == NTargetingArrow.PropertyName._segments)
			{
				GodotObject[] segments = this._segments;
				value = VariantUtils.CreateFromSystemArrayOfGodotObject(segments);
				return true;
			}
			if ((ref name) == NTargetingArrow.PropertyName._arrowHead)
			{
				value = VariantUtils.CreateFrom<Sprite2D>(ref this._arrowHead);
				return true;
			}
			if ((ref name) == NTargetingArrow.PropertyName._arrowHeadTween)
			{
				value = VariantUtils.CreateFrom<Tween>(ref this._arrowHeadTween);
				return true;
			}
			if ((ref name) == NTargetingArrow.PropertyName._initialized)
			{
				value = VariantUtils.CreateFrom<bool>(ref this._initialized);
				return true;
			}
			if ((ref name) == NTargetingArrow.PropertyName._followMouse)
			{
				value = VariantUtils.CreateFrom<bool>(ref this._followMouse);
				return true;
			}
			return base.GetGodotClassPropertyValue(ref name, ref value);
		}

		/// <summary>
		/// Get the property information for all the properties declared in this class.
		/// This method is used by Godot to register the available properties in the editor.
		/// Do not call this method.
		/// </summary>
		// Token: 0x06004184 RID: 16772 RVA: 0x001FA9DC File Offset: 0x001F8BDC
		[NullableContext(0)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal static List<PropertyInfo> GetGodotPropertyList()
		{
			return new List<PropertyInfo>
			{
				new PropertyInfo(5L, NTargetingArrow.PropertyName._fromPos, 0L, "", 4096L, false),
				new PropertyInfo(24L, NTargetingArrow.PropertyName._fromControl, 0L, "", 4096L, false),
				new PropertyInfo(5L, NTargetingArrow.PropertyName._toPosition, 0L, "", 4096L, false),
				new PropertyInfo(5L, NTargetingArrow.PropertyName.From, 0L, "", 4096L, false),
				new PropertyInfo(28L, NTargetingArrow.PropertyName._segments, 0L, "", 4096L, false),
				new PropertyInfo(24L, NTargetingArrow.PropertyName._arrowHead, 0L, "", 4096L, false),
				new PropertyInfo(24L, NTargetingArrow.PropertyName._arrowHeadTween, 0L, "", 4096L, false),
				new PropertyInfo(1L, NTargetingArrow.PropertyName._initialized, 0L, "", 4096L, false),
				new PropertyInfo(1L, NTargetingArrow.PropertyName._followMouse, 0L, "", 4096L, false)
			};
		}

		/// <inheritdoc />
		// Token: 0x06004185 RID: 16773 RVA: 0x001FAB14 File Offset: 0x001F8D14
		[NullableContext(0)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void SaveGodotObjectData(GodotSerializationInfo info)
		{
			base.SaveGodotObjectData(info);
			info.AddProperty(NTargetingArrow.PropertyName._fromPos, Variant.From<Vector2>(ref this._fromPos));
			info.AddProperty(NTargetingArrow.PropertyName._fromControl, Variant.From<Control>(ref this._fromControl));
			info.AddProperty(NTargetingArrow.PropertyName._toPosition, Variant.From<Vector2>(ref this._toPosition));
			StringName segments = NTargetingArrow.PropertyName._segments;
			GodotObject[] segments2 = this._segments;
			info.AddProperty(segments, Variant.CreateFrom(segments2));
			info.AddProperty(NTargetingArrow.PropertyName._arrowHead, Variant.From<Sprite2D>(ref this._arrowHead));
			info.AddProperty(NTargetingArrow.PropertyName._arrowHeadTween, Variant.From<Tween>(ref this._arrowHeadTween));
			info.AddProperty(NTargetingArrow.PropertyName._initialized, Variant.From<bool>(ref this._initialized));
			info.AddProperty(NTargetingArrow.PropertyName._followMouse, Variant.From<bool>(ref this._followMouse));
		}

		/// <inheritdoc />
		// Token: 0x06004186 RID: 16774 RVA: 0x001FABDC File Offset: 0x001F8DDC
		[NullableContext(0)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void RestoreGodotObjectData(GodotSerializationInfo info)
		{
			base.RestoreGodotObjectData(info);
			Variant variant;
			if (info.TryGetProperty(NTargetingArrow.PropertyName._fromPos, ref variant))
			{
				this._fromPos = variant.As<Vector2>();
			}
			Variant variant2;
			if (info.TryGetProperty(NTargetingArrow.PropertyName._fromControl, ref variant2))
			{
				this._fromControl = variant2.As<Control>();
			}
			Variant variant3;
			if (info.TryGetProperty(NTargetingArrow.PropertyName._toPosition, ref variant3))
			{
				this._toPosition = variant3.As<Vector2>();
			}
			Variant variant4;
			if (info.TryGetProperty(NTargetingArrow.PropertyName._segments, ref variant4))
			{
				this._segments = variant4.AsGodotObjectArray<Sprite2D>();
			}
			Variant variant5;
			if (info.TryGetProperty(NTargetingArrow.PropertyName._arrowHead, ref variant5))
			{
				this._arrowHead = variant5.As<Sprite2D>();
			}
			Variant variant6;
			if (info.TryGetProperty(NTargetingArrow.PropertyName._arrowHeadTween, ref variant6))
			{
				this._arrowHeadTween = variant6.As<Tween>();
			}
			Variant variant7;
			if (info.TryGetProperty(NTargetingArrow.PropertyName._initialized, ref variant7))
			{
				this._initialized = variant7.As<bool>();
			}
			Variant variant8;
			if (info.TryGetProperty(NTargetingArrow.PropertyName._followMouse, ref variant8))
			{
				this._followMouse = variant8.As<bool>();
			}
		}

		// Token: 0x0400196C RID: 6508
		private const int _segmentCount = 19;

		/// <summary>
		/// Where the arrow starts from.
		/// </summary>
		// Token: 0x0400196D RID: 6509
		private Vector2 _fromPos;

		// Token: 0x0400196E RID: 6510
		[Nullable(2)]
		private Control _fromControl;

		/// <summary>
		/// Where the arrow tip is trying to be.
		/// </summary>
		// Token: 0x0400196F RID: 6511
		private Vector2 _toPosition;

		/// <summary>
		/// Where the arrows tip is currently at. Typically making its way towards the _toPosition, as it lerps between targets.
		/// Used only when the game is in controller mode, since in mouse mode we are always tracking the mouse position.
		/// </summary>
		// Token: 0x04001970 RID: 6512
		private Vector2? _currentArrowPos;

		// Token: 0x04001971 RID: 6513
		private static readonly string _segmentHeadPath = ImageHelper.GetImagePath("ui/combat/targeting_arrow_head.png");

		// Token: 0x04001972 RID: 6514
		private static readonly string _segmentBlockPath = ImageHelper.GetImagePath("ui/combat/targeting_arrow_segment.png");

		// Token: 0x04001973 RID: 6515
		private Sprite2D[] _segments = new Sprite2D[19];

		// Token: 0x04001974 RID: 6516
		private Sprite2D _arrowHead;

		// Token: 0x04001975 RID: 6517
		[Nullable(2)]
		private Tween _arrowHeadTween;

		// Token: 0x04001976 RID: 6518
		private bool _initialized;

		// Token: 0x04001977 RID: 6519
		private bool _followMouse;

		// Token: 0x04001978 RID: 6520
		private const float _segmentScaleStart = 0.28f;

		// Token: 0x04001979 RID: 6521
		private const float _segmentScaleEnd = 0.42f;

		// Token: 0x0400197A RID: 6522
		private static readonly Vector2 _arrowHeadDefaultScale = Vector2.One * 0.95f;

		// Token: 0x0400197B RID: 6523
		private static readonly Vector2 _arrowHeadHoverScale = Vector2.One * 1.05f;

		/// <summary>
		/// Cached StringNames for the methods contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x0200192A RID: 6442
		[NullableContext(0)]
		public class MethodName : Node2D.MethodName
		{
			/// <summary>
			/// Cached name for the '_Ready' method.
			/// </summary>
			// Token: 0x04006304 RID: 25348
			public static readonly StringName _Ready = "_Ready";

			/// <summary>
			/// Cached name for the '_Process' method.
			/// </summary>
			// Token: 0x04006305 RID: 25349
			public static readonly StringName _Process = "_Process";

			/// <summary>
			/// Cached name for the 'UpdateArrowPosition' method.
			/// </summary>
			// Token: 0x04006306 RID: 25350
			public static readonly StringName UpdateArrowPosition = "UpdateArrowPosition";

			/// <summary>
			/// Cached name for the 'SetHighlightingOn' method.
			/// </summary>
			// Token: 0x04006307 RID: 25351
			public static readonly StringName SetHighlightingOn = "SetHighlightingOn";

			/// <summary>
			/// Cached name for the 'SetHighlightingOff' method.
			/// </summary>
			// Token: 0x04006308 RID: 25352
			public static readonly StringName SetHighlightingOff = "SetHighlightingOff";

			/// <summary>
			/// Cached name for the 'UpdateSegments' method.
			/// </summary>
			// Token: 0x04006309 RID: 25353
			public static readonly StringName UpdateSegments = "UpdateSegments";

			/// <summary>
			/// Cached name for the 'StartDrawingFrom' method.
			/// </summary>
			// Token: 0x0400630A RID: 25354
			public static readonly StringName StartDrawingFrom = "StartDrawingFrom";

			/// <summary>
			/// Cached name for the 'StopDrawing' method.
			/// </summary>
			// Token: 0x0400630B RID: 25355
			public static readonly StringName StopDrawing = "StopDrawing";

			/// <summary>
			/// Cached name for the '_ExitTree' method.
			/// </summary>
			// Token: 0x0400630C RID: 25356
			public static readonly StringName _ExitTree = "_ExitTree";

			/// <summary>
			/// Cached name for the 'UpdateDrawingTo' method.
			/// </summary>
			// Token: 0x0400630D RID: 25357
			public static readonly StringName UpdateDrawingTo = "UpdateDrawingTo";
		}

		/// <summary>
		/// Cached StringNames for the properties and fields contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x0200192B RID: 6443
		[NullableContext(0)]
		public class PropertyName : Node2D.PropertyName
		{
			/// <summary>
			/// Cached name for the 'From' property.
			/// </summary>
			// Token: 0x0400630E RID: 25358
			public static readonly StringName From = "From";

			/// <summary>
			/// Cached name for the '_fromPos' field.
			/// </summary>
			// Token: 0x0400630F RID: 25359
			public static readonly StringName _fromPos = "_fromPos";

			/// <summary>
			/// Cached name for the '_fromControl' field.
			/// </summary>
			// Token: 0x04006310 RID: 25360
			public static readonly StringName _fromControl = "_fromControl";

			/// <summary>
			/// Cached name for the '_toPosition' field.
			/// </summary>
			// Token: 0x04006311 RID: 25361
			public static readonly StringName _toPosition = "_toPosition";

			/// <summary>
			/// Cached name for the '_segments' field.
			/// </summary>
			// Token: 0x04006312 RID: 25362
			public static readonly StringName _segments = "_segments";

			/// <summary>
			/// Cached name for the '_arrowHead' field.
			/// </summary>
			// Token: 0x04006313 RID: 25363
			public static readonly StringName _arrowHead = "_arrowHead";

			/// <summary>
			/// Cached name for the '_arrowHeadTween' field.
			/// </summary>
			// Token: 0x04006314 RID: 25364
			public static readonly StringName _arrowHeadTween = "_arrowHeadTween";

			/// <summary>
			/// Cached name for the '_initialized' field.
			/// </summary>
			// Token: 0x04006315 RID: 25365
			public static readonly StringName _initialized = "_initialized";

			/// <summary>
			/// Cached name for the '_followMouse' field.
			/// </summary>
			// Token: 0x04006316 RID: 25366
			public static readonly StringName _followMouse = "_followMouse";
		}

		/// <summary>
		/// Cached StringNames for the signals contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x0200192C RID: 6444
		[NullableContext(0)]
		public class SignalName : Node2D.SignalName
		{
		}
	}
}
