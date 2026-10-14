using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Godot;
using Godot.Bridge;
using Godot.NativeInterop;
using MegaCrit.Sts2.Core.Context;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Runs;

namespace MegaCrit.Sts2.Core.Nodes.Reaction
{
	// Token: 0x0200037C RID: 892
	[NullableContext(1)]
	[Nullable(0)]
	[ScriptPath("res://src/Core/Nodes/Reaction/NReactionWheel.cs")]
	public class NReactionWheel : Control
	{
		// Token: 0x060034D8 RID: 13528 RVA: 0x00189528 File Offset: 0x00187728
		public override void _Ready()
		{
			this._rightWedge = base.GetNode<NReactionWheelWedge>("RightWedge");
			this._downRightWedge = base.GetNode<NReactionWheelWedge>("DownRightWedge");
			this._downWedge = base.GetNode<NReactionWheelWedge>("DownWedge");
			this._downLeftWedge = base.GetNode<NReactionWheelWedge>("DownLeftWedge");
			this._leftWedge = base.GetNode<NReactionWheelWedge>("LeftWedge");
			this._upLeftWedge = base.GetNode<NReactionWheelWedge>("UpLeftWedge");
			this._upWedge = base.GetNode<NReactionWheelWedge>("UpWedge");
			this._upRightWedge = base.GetNode<NReactionWheelWedge>("UpRightWedge");
			this._marker = base.GetNode<TextureRect>("Marker");
			base.Visible = false;
		}

		// Token: 0x060034D9 RID: 13529 RVA: 0x00189602 File Offset: 0x00187802
		public override void _EnterTree()
		{
			RunManager.Instance.RunStarted += this.OnRunStarted;
		}

		// Token: 0x060034DA RID: 13530 RVA: 0x0018961A File Offset: 0x0018781A
		public override void _ExitTree()
		{
			RunManager.Instance.RunStarted -= this.OnRunStarted;
		}

		// Token: 0x060034DB RID: 13531 RVA: 0x00189632 File Offset: 0x00187832
		public override void _Notification(int what)
		{
			if (base.Visible && (long)what == 2017L)
			{
				base.Visible = false;
			}
		}

		// Token: 0x060034DC RID: 13532 RVA: 0x00189650 File Offset: 0x00187850
		public override void _Input(InputEvent inputEvent)
		{
			Control control = base.GetViewport().GuiGetFocusOwner();
			bool flag = control is TextEdit || control is LineEdit;
			bool flag2 = flag;
			if (!NGame.Instance.ReactionContainer.InMultiplayer)
			{
				if (base.Visible)
				{
					this.HideWheel();
				}
				return;
			}
			InputEventMouseMotion inputEventMouseMotion = inputEvent as InputEventMouseMotion;
			if (inputEventMouseMotion != null)
			{
				if (this._ignoreNextMouseInput)
				{
					this._ignoreNextMouseInput = false;
					return;
				}
				if (base.Visible)
				{
					this.MoveMarker(inputEventMouseMotion.Relative);
					this._ignoreNextMouseInput = true;
					this.WarpMouseBackToOriginalPosition();
					return;
				}
			}
			else
			{
				if (inputEvent.IsActionPressed(NReactionWheel._reactWheel, false, false) && !flag2)
				{
					base.Visible = true;
					if (this._localPlayer != null)
					{
						this._marker.Texture = this._localPlayer.Character.MapMarker;
					}
					this._centerPosition = base.GetViewport().GetMousePosition();
					this._marker.Position = (base.Size - this._marker.Size) * 0.5f;
					base.GlobalPosition = this._centerPosition - base.Size * base.Scale * 0.5f;
					Input.MouseMode = 1L;
					return;
				}
				if (inputEvent.IsActionReleased(NReactionWheel._reactWheel, false) && base.Visible)
				{
					this.HideWheel();
					this.React();
				}
			}
		}

		// Token: 0x060034DD RID: 13533 RVA: 0x001897B4 File Offset: 0x001879B4
		private void OnRunStarted(RunState runState)
		{
			this._localPlayer = LocalContext.GetMe(runState);
		}

		// Token: 0x060034DE RID: 13534 RVA: 0x001897C2 File Offset: 0x001879C2
		private void HideWheel()
		{
			Input.MouseMode = 0L;
			this.WarpMouseBackToOriginalPosition();
			base.Visible = false;
		}

		// Token: 0x060034DF RID: 13535 RVA: 0x001897D8 File Offset: 0x001879D8
		private void WarpMouseBackToOriginalPosition()
		{
			Transform2D viewportTransform = base.GetViewportTransform();
			Input.WarpMouse(viewportTransform * this._centerPosition);
		}

		// Token: 0x060034E0 RID: 13536 RVA: 0x001897FD File Offset: 0x001879FD
		private void React()
		{
			if (this._selectedWedge == null)
			{
				return;
			}
			NGame.Instance.ReactionContainer.DoLocalReaction(this._selectedWedge.Reaction, this._centerPosition);
		}

		// Token: 0x060034E1 RID: 13537 RVA: 0x00189828 File Offset: 0x00187A28
		private void MoveMarker(Vector2 relative)
		{
			Vector2 vector = (base.Size - this._marker.Size) * 0.5f;
			Vector2 vector2 = this._marker.Position - vector;
			vector2 = (vector2 + relative).LimitLength(70f);
			this._marker.Position = vector + vector2;
			float num = Mathf.Atan2(vector2.Y, vector2.X);
			this._marker.Rotation = num - 1.5707964f;
			NReactionWheelWedge selectedWedge = this.GetSelectedWedge(num);
			if (this._selectedWedge != selectedWedge)
			{
				NReactionWheelWedge selectedWedge2 = this._selectedWedge;
				if (selectedWedge2 != null)
				{
					selectedWedge2.OnDeselected();
				}
				this._selectedWedge = selectedWedge;
				NReactionWheelWedge selectedWedge3 = this._selectedWedge;
				if (selectedWedge3 == null)
				{
					return;
				}
				selectedWedge3.OnSelected();
			}
		}

		// Token: 0x060034E2 RID: 13538 RVA: 0x001898EC File Offset: 0x00187AEC
		private NReactionWheelWedge GetSelectedWedge(float angle)
		{
			float num = Mathf.Wrap(angle + 0.3926991f, 0f, 6.2831855f);
			float num2 = 0.7853982f;
			NReactionWheelWedge nreactionWheelWedge;
			switch ((int)(num / num2))
			{
			case 0:
				nreactionWheelWedge = this._rightWedge;
				break;
			case 1:
				nreactionWheelWedge = this._downRightWedge;
				break;
			case 2:
				nreactionWheelWedge = this._downWedge;
				break;
			case 3:
				nreactionWheelWedge = this._downLeftWedge;
				break;
			case 4:
				nreactionWheelWedge = this._leftWedge;
				break;
			case 5:
				nreactionWheelWedge = this._upLeftWedge;
				break;
			case 6:
				nreactionWheelWedge = this._upWedge;
				break;
			case 7:
				nreactionWheelWedge = this._upRightWedge;
				break;
			default:
				throw new InvalidOperationException();
			}
			return nreactionWheelWedge;
		}

		/// <summary>
		/// Get the method information for all the methods declared in this class.
		/// This method is used by Godot to register the available methods in the editor.
		/// Do not call this method.
		/// </summary>
		// Token: 0x060034E3 RID: 13539 RVA: 0x00189994 File Offset: 0x00187B94
		[NullableContext(0)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal static List<MethodInfo> GetGodotMethodList()
		{
			return new List<MethodInfo>(10)
			{
				new MethodInfo(NReactionWheel.MethodName._Ready, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NReactionWheel.MethodName._EnterTree, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NReactionWheel.MethodName._ExitTree, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NReactionWheel.MethodName._Notification, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, new List<PropertyInfo>
				{
					new PropertyInfo(2L, "what", 0L, "", 6L, false)
				}, null),
				new MethodInfo(NReactionWheel.MethodName._Input, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, new List<PropertyInfo>
				{
					new PropertyInfo(24L, "inputEvent", 0L, "", 6L, new StringName("InputEvent"), false)
				}, null),
				new MethodInfo(NReactionWheel.MethodName.HideWheel, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NReactionWheel.MethodName.WarpMouseBackToOriginalPosition, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NReactionWheel.MethodName.React, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NReactionWheel.MethodName.MoveMarker, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, new List<PropertyInfo>
				{
					new PropertyInfo(5L, "relative", 0L, "", 6L, false)
				}, null),
				new MethodInfo(NReactionWheel.MethodName.GetSelectedWedge, new PropertyInfo(24L, "", 0L, "", 6L, new StringName("TextureRect"), false), 1L, new List<PropertyInfo>
				{
					new PropertyInfo(3L, "angle", 0L, "", 6L, false)
				}, null)
			};
		}

		/// <inheritdoc />
		// Token: 0x060034E4 RID: 13540 RVA: 0x00189C2C File Offset: 0x00187E2C
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool InvokeGodotClassMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
		{
			if ((ref method) == NReactionWheel.MethodName._Ready && args.Count == 0)
			{
				this._Ready();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NReactionWheel.MethodName._EnterTree && args.Count == 0)
			{
				this._EnterTree();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NReactionWheel.MethodName._ExitTree && args.Count == 0)
			{
				this._ExitTree();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NReactionWheel.MethodName._Notification && args.Count == 1)
			{
				this._Notification(VariantUtils.ConvertTo<int>(args[0]));
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NReactionWheel.MethodName._Input && args.Count == 1)
			{
				this._Input(VariantUtils.ConvertTo<InputEvent>(args[0]));
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NReactionWheel.MethodName.HideWheel && args.Count == 0)
			{
				this.HideWheel();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NReactionWheel.MethodName.WarpMouseBackToOriginalPosition && args.Count == 0)
			{
				this.WarpMouseBackToOriginalPosition();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NReactionWheel.MethodName.React && args.Count == 0)
			{
				this.React();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NReactionWheel.MethodName.MoveMarker && args.Count == 1)
			{
				this.MoveMarker(VariantUtils.ConvertTo<Vector2>(args[0]));
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NReactionWheel.MethodName.GetSelectedWedge && args.Count == 1)
			{
				NReactionWheelWedge selectedWedge = this.GetSelectedWedge(VariantUtils.ConvertTo<float>(args[0]));
				ret = VariantUtils.CreateFrom<NReactionWheelWedge>(ref selectedWedge);
				return true;
			}
			return base.InvokeGodotClassMethod(ref method, args, ref ret);
		}

		/// <inheritdoc />
		// Token: 0x060034E5 RID: 13541 RVA: 0x00189DF4 File Offset: 0x00187FF4
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool HasGodotClassMethod(in godot_string_name method)
		{
			return (ref method) == NReactionWheel.MethodName._Ready || (ref method) == NReactionWheel.MethodName._EnterTree || (ref method) == NReactionWheel.MethodName._ExitTree || (ref method) == NReactionWheel.MethodName._Notification || (ref method) == NReactionWheel.MethodName._Input || (ref method) == NReactionWheel.MethodName.HideWheel || (ref method) == NReactionWheel.MethodName.WarpMouseBackToOriginalPosition || (ref method) == NReactionWheel.MethodName.React || (ref method) == NReactionWheel.MethodName.MoveMarker || (ref method) == NReactionWheel.MethodName.GetSelectedWedge || base.HasGodotClassMethod(ref method);
		}

		/// <inheritdoc />
		// Token: 0x060034E6 RID: 13542 RVA: 0x00189EA0 File Offset: 0x001880A0
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool SetGodotClassPropertyValue(in godot_string_name name, in godot_variant value)
		{
			if ((ref name) == NReactionWheel.PropertyName._rightWedge)
			{
				this._rightWedge = VariantUtils.ConvertTo<NReactionWheelWedge>(ref value);
				return true;
			}
			if ((ref name) == NReactionWheel.PropertyName._downRightWedge)
			{
				this._downRightWedge = VariantUtils.ConvertTo<NReactionWheelWedge>(ref value);
				return true;
			}
			if ((ref name) == NReactionWheel.PropertyName._downWedge)
			{
				this._downWedge = VariantUtils.ConvertTo<NReactionWheelWedge>(ref value);
				return true;
			}
			if ((ref name) == NReactionWheel.PropertyName._downLeftWedge)
			{
				this._downLeftWedge = VariantUtils.ConvertTo<NReactionWheelWedge>(ref value);
				return true;
			}
			if ((ref name) == NReactionWheel.PropertyName._leftWedge)
			{
				this._leftWedge = VariantUtils.ConvertTo<NReactionWheelWedge>(ref value);
				return true;
			}
			if ((ref name) == NReactionWheel.PropertyName._upLeftWedge)
			{
				this._upLeftWedge = VariantUtils.ConvertTo<NReactionWheelWedge>(ref value);
				return true;
			}
			if ((ref name) == NReactionWheel.PropertyName._upWedge)
			{
				this._upWedge = VariantUtils.ConvertTo<NReactionWheelWedge>(ref value);
				return true;
			}
			if ((ref name) == NReactionWheel.PropertyName._upRightWedge)
			{
				this._upRightWedge = VariantUtils.ConvertTo<NReactionWheelWedge>(ref value);
				return true;
			}
			if ((ref name) == NReactionWheel.PropertyName._marker)
			{
				this._marker = VariantUtils.ConvertTo<TextureRect>(ref value);
				return true;
			}
			if ((ref name) == NReactionWheel.PropertyName._ignoreNextMouseInput)
			{
				this._ignoreNextMouseInput = VariantUtils.ConvertTo<bool>(ref value);
				return true;
			}
			if ((ref name) == NReactionWheel.PropertyName._centerPosition)
			{
				this._centerPosition = VariantUtils.ConvertTo<Vector2>(ref value);
				return true;
			}
			if ((ref name) == NReactionWheel.PropertyName._selectedWedge)
			{
				this._selectedWedge = VariantUtils.ConvertTo<NReactionWheelWedge>(ref value);
				return true;
			}
			return base.SetGodotClassPropertyValue(ref name, ref value);
		}

		/// <inheritdoc />
		// Token: 0x060034E7 RID: 13543 RVA: 0x00189FFC File Offset: 0x001881FC
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
		{
			if ((ref name) == NReactionWheel.PropertyName._rightWedge)
			{
				value = VariantUtils.CreateFrom<NReactionWheelWedge>(ref this._rightWedge);
				return true;
			}
			if ((ref name) == NReactionWheel.PropertyName._downRightWedge)
			{
				value = VariantUtils.CreateFrom<NReactionWheelWedge>(ref this._downRightWedge);
				return true;
			}
			if ((ref name) == NReactionWheel.PropertyName._downWedge)
			{
				value = VariantUtils.CreateFrom<NReactionWheelWedge>(ref this._downWedge);
				return true;
			}
			if ((ref name) == NReactionWheel.PropertyName._downLeftWedge)
			{
				value = VariantUtils.CreateFrom<NReactionWheelWedge>(ref this._downLeftWedge);
				return true;
			}
			if ((ref name) == NReactionWheel.PropertyName._leftWedge)
			{
				value = VariantUtils.CreateFrom<NReactionWheelWedge>(ref this._leftWedge);
				return true;
			}
			if ((ref name) == NReactionWheel.PropertyName._upLeftWedge)
			{
				value = VariantUtils.CreateFrom<NReactionWheelWedge>(ref this._upLeftWedge);
				return true;
			}
			if ((ref name) == NReactionWheel.PropertyName._upWedge)
			{
				value = VariantUtils.CreateFrom<NReactionWheelWedge>(ref this._upWedge);
				return true;
			}
			if ((ref name) == NReactionWheel.PropertyName._upRightWedge)
			{
				value = VariantUtils.CreateFrom<NReactionWheelWedge>(ref this._upRightWedge);
				return true;
			}
			if ((ref name) == NReactionWheel.PropertyName._marker)
			{
				value = VariantUtils.CreateFrom<TextureRect>(ref this._marker);
				return true;
			}
			if ((ref name) == NReactionWheel.PropertyName._ignoreNextMouseInput)
			{
				value = VariantUtils.CreateFrom<bool>(ref this._ignoreNextMouseInput);
				return true;
			}
			if ((ref name) == NReactionWheel.PropertyName._centerPosition)
			{
				value = VariantUtils.CreateFrom<Vector2>(ref this._centerPosition);
				return true;
			}
			if ((ref name) == NReactionWheel.PropertyName._selectedWedge)
			{
				value = VariantUtils.CreateFrom<NReactionWheelWedge>(ref this._selectedWedge);
				return true;
			}
			return base.GetGodotClassPropertyValue(ref name, ref value);
		}

		/// <summary>
		/// Get the property information for all the properties declared in this class.
		/// This method is used by Godot to register the available properties in the editor.
		/// Do not call this method.
		/// </summary>
		// Token: 0x060034E8 RID: 13544 RVA: 0x0018A194 File Offset: 0x00188394
		[NullableContext(0)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal static List<PropertyInfo> GetGodotPropertyList()
		{
			return new List<PropertyInfo>
			{
				new PropertyInfo(24L, NReactionWheel.PropertyName._rightWedge, 0L, "", 4096L, false),
				new PropertyInfo(24L, NReactionWheel.PropertyName._downRightWedge, 0L, "", 4096L, false),
				new PropertyInfo(24L, NReactionWheel.PropertyName._downWedge, 0L, "", 4096L, false),
				new PropertyInfo(24L, NReactionWheel.PropertyName._downLeftWedge, 0L, "", 4096L, false),
				new PropertyInfo(24L, NReactionWheel.PropertyName._leftWedge, 0L, "", 4096L, false),
				new PropertyInfo(24L, NReactionWheel.PropertyName._upLeftWedge, 0L, "", 4096L, false),
				new PropertyInfo(24L, NReactionWheel.PropertyName._upWedge, 0L, "", 4096L, false),
				new PropertyInfo(24L, NReactionWheel.PropertyName._upRightWedge, 0L, "", 4096L, false),
				new PropertyInfo(24L, NReactionWheel.PropertyName._marker, 0L, "", 4096L, false),
				new PropertyInfo(1L, NReactionWheel.PropertyName._ignoreNextMouseInput, 0L, "", 4096L, false),
				new PropertyInfo(5L, NReactionWheel.PropertyName._centerPosition, 0L, "", 4096L, false),
				new PropertyInfo(24L, NReactionWheel.PropertyName._selectedWedge, 0L, "", 4096L, false)
			};
		}

		/// <inheritdoc />
		// Token: 0x060034E9 RID: 13545 RVA: 0x0018A334 File Offset: 0x00188534
		[NullableContext(0)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void SaveGodotObjectData(GodotSerializationInfo info)
		{
			base.SaveGodotObjectData(info);
			info.AddProperty(NReactionWheel.PropertyName._rightWedge, Variant.From<NReactionWheelWedge>(ref this._rightWedge));
			info.AddProperty(NReactionWheel.PropertyName._downRightWedge, Variant.From<NReactionWheelWedge>(ref this._downRightWedge));
			info.AddProperty(NReactionWheel.PropertyName._downWedge, Variant.From<NReactionWheelWedge>(ref this._downWedge));
			info.AddProperty(NReactionWheel.PropertyName._downLeftWedge, Variant.From<NReactionWheelWedge>(ref this._downLeftWedge));
			info.AddProperty(NReactionWheel.PropertyName._leftWedge, Variant.From<NReactionWheelWedge>(ref this._leftWedge));
			info.AddProperty(NReactionWheel.PropertyName._upLeftWedge, Variant.From<NReactionWheelWedge>(ref this._upLeftWedge));
			info.AddProperty(NReactionWheel.PropertyName._upWedge, Variant.From<NReactionWheelWedge>(ref this._upWedge));
			info.AddProperty(NReactionWheel.PropertyName._upRightWedge, Variant.From<NReactionWheelWedge>(ref this._upRightWedge));
			info.AddProperty(NReactionWheel.PropertyName._marker, Variant.From<TextureRect>(ref this._marker));
			info.AddProperty(NReactionWheel.PropertyName._ignoreNextMouseInput, Variant.From<bool>(ref this._ignoreNextMouseInput));
			info.AddProperty(NReactionWheel.PropertyName._centerPosition, Variant.From<Vector2>(ref this._centerPosition));
			info.AddProperty(NReactionWheel.PropertyName._selectedWedge, Variant.From<NReactionWheelWedge>(ref this._selectedWedge));
		}

		/// <inheritdoc />
		// Token: 0x060034EA RID: 13546 RVA: 0x0018A450 File Offset: 0x00188650
		[NullableContext(0)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void RestoreGodotObjectData(GodotSerializationInfo info)
		{
			base.RestoreGodotObjectData(info);
			Variant variant;
			if (info.TryGetProperty(NReactionWheel.PropertyName._rightWedge, ref variant))
			{
				this._rightWedge = variant.As<NReactionWheelWedge>();
			}
			Variant variant2;
			if (info.TryGetProperty(NReactionWheel.PropertyName._downRightWedge, ref variant2))
			{
				this._downRightWedge = variant2.As<NReactionWheelWedge>();
			}
			Variant variant3;
			if (info.TryGetProperty(NReactionWheel.PropertyName._downWedge, ref variant3))
			{
				this._downWedge = variant3.As<NReactionWheelWedge>();
			}
			Variant variant4;
			if (info.TryGetProperty(NReactionWheel.PropertyName._downLeftWedge, ref variant4))
			{
				this._downLeftWedge = variant4.As<NReactionWheelWedge>();
			}
			Variant variant5;
			if (info.TryGetProperty(NReactionWheel.PropertyName._leftWedge, ref variant5))
			{
				this._leftWedge = variant5.As<NReactionWheelWedge>();
			}
			Variant variant6;
			if (info.TryGetProperty(NReactionWheel.PropertyName._upLeftWedge, ref variant6))
			{
				this._upLeftWedge = variant6.As<NReactionWheelWedge>();
			}
			Variant variant7;
			if (info.TryGetProperty(NReactionWheel.PropertyName._upWedge, ref variant7))
			{
				this._upWedge = variant7.As<NReactionWheelWedge>();
			}
			Variant variant8;
			if (info.TryGetProperty(NReactionWheel.PropertyName._upRightWedge, ref variant8))
			{
				this._upRightWedge = variant8.As<NReactionWheelWedge>();
			}
			Variant variant9;
			if (info.TryGetProperty(NReactionWheel.PropertyName._marker, ref variant9))
			{
				this._marker = variant9.As<TextureRect>();
			}
			Variant variant10;
			if (info.TryGetProperty(NReactionWheel.PropertyName._ignoreNextMouseInput, ref variant10))
			{
				this._ignoreNextMouseInput = variant10.As<bool>();
			}
			Variant variant11;
			if (info.TryGetProperty(NReactionWheel.PropertyName._centerPosition, ref variant11))
			{
				this._centerPosition = variant11.As<Vector2>();
			}
			Variant variant12;
			if (info.TryGetProperty(NReactionWheel.PropertyName._selectedWedge, ref variant12))
			{
				this._selectedWedge = variant12.As<NReactionWheelWedge>();
			}
		}

		// Token: 0x0400147D RID: 5245
		private static readonly StringName _reactWheel = new StringName("react_wheel");

		// Token: 0x0400147E RID: 5246
		private const float _centerRadius = 70f;

		// Token: 0x0400147F RID: 5247
		private NReactionWheelWedge _rightWedge;

		// Token: 0x04001480 RID: 5248
		private NReactionWheelWedge _downRightWedge;

		// Token: 0x04001481 RID: 5249
		private NReactionWheelWedge _downWedge;

		// Token: 0x04001482 RID: 5250
		private NReactionWheelWedge _downLeftWedge;

		// Token: 0x04001483 RID: 5251
		private NReactionWheelWedge _leftWedge;

		// Token: 0x04001484 RID: 5252
		private NReactionWheelWedge _upLeftWedge;

		// Token: 0x04001485 RID: 5253
		private NReactionWheelWedge _upWedge;

		// Token: 0x04001486 RID: 5254
		private NReactionWheelWedge _upRightWedge;

		// Token: 0x04001487 RID: 5255
		private TextureRect _marker;

		// Token: 0x04001488 RID: 5256
		private bool _ignoreNextMouseInput;

		// Token: 0x04001489 RID: 5257
		private Vector2 _centerPosition;

		// Token: 0x0400148A RID: 5258
		[Nullable(2)]
		private NReactionWheelWedge _selectedWedge;

		// Token: 0x0400148B RID: 5259
		[Nullable(2)]
		private Player _localPlayer;

		/// <summary>
		/// Cached StringNames for the methods contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x020016DE RID: 5854
		[NullableContext(0)]
		public class MethodName : Control.MethodName
		{
			/// <summary>
			/// Cached name for the '_Ready' method.
			/// </summary>
			// Token: 0x04005914 RID: 22804
			public static readonly StringName _Ready = "_Ready";

			/// <summary>
			/// Cached name for the '_EnterTree' method.
			/// </summary>
			// Token: 0x04005915 RID: 22805
			public static readonly StringName _EnterTree = "_EnterTree";

			/// <summary>
			/// Cached name for the '_ExitTree' method.
			/// </summary>
			// Token: 0x04005916 RID: 22806
			public static readonly StringName _ExitTree = "_ExitTree";

			/// <summary>
			/// Cached name for the '_Notification' method.
			/// </summary>
			// Token: 0x04005917 RID: 22807
			public static readonly StringName _Notification = "_Notification";

			/// <summary>
			/// Cached name for the '_Input' method.
			/// </summary>
			// Token: 0x04005918 RID: 22808
			public static readonly StringName _Input = "_Input";

			/// <summary>
			/// Cached name for the 'HideWheel' method.
			/// </summary>
			// Token: 0x04005919 RID: 22809
			public static readonly StringName HideWheel = "HideWheel";

			/// <summary>
			/// Cached name for the 'WarpMouseBackToOriginalPosition' method.
			/// </summary>
			// Token: 0x0400591A RID: 22810
			public static readonly StringName WarpMouseBackToOriginalPosition = "WarpMouseBackToOriginalPosition";

			/// <summary>
			/// Cached name for the 'React' method.
			/// </summary>
			// Token: 0x0400591B RID: 22811
			public static readonly StringName React = "React";

			/// <summary>
			/// Cached name for the 'MoveMarker' method.
			/// </summary>
			// Token: 0x0400591C RID: 22812
			public static readonly StringName MoveMarker = "MoveMarker";

			/// <summary>
			/// Cached name for the 'GetSelectedWedge' method.
			/// </summary>
			// Token: 0x0400591D RID: 22813
			public static readonly StringName GetSelectedWedge = "GetSelectedWedge";
		}

		/// <summary>
		/// Cached StringNames for the properties and fields contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x020016DF RID: 5855
		[NullableContext(0)]
		public class PropertyName : Control.PropertyName
		{
			/// <summary>
			/// Cached name for the '_rightWedge' field.
			/// </summary>
			// Token: 0x0400591E RID: 22814
			public static readonly StringName _rightWedge = "_rightWedge";

			/// <summary>
			/// Cached name for the '_downRightWedge' field.
			/// </summary>
			// Token: 0x0400591F RID: 22815
			public static readonly StringName _downRightWedge = "_downRightWedge";

			/// <summary>
			/// Cached name for the '_downWedge' field.
			/// </summary>
			// Token: 0x04005920 RID: 22816
			public static readonly StringName _downWedge = "_downWedge";

			/// <summary>
			/// Cached name for the '_downLeftWedge' field.
			/// </summary>
			// Token: 0x04005921 RID: 22817
			public static readonly StringName _downLeftWedge = "_downLeftWedge";

			/// <summary>
			/// Cached name for the '_leftWedge' field.
			/// </summary>
			// Token: 0x04005922 RID: 22818
			public static readonly StringName _leftWedge = "_leftWedge";

			/// <summary>
			/// Cached name for the '_upLeftWedge' field.
			/// </summary>
			// Token: 0x04005923 RID: 22819
			public static readonly StringName _upLeftWedge = "_upLeftWedge";

			/// <summary>
			/// Cached name for the '_upWedge' field.
			/// </summary>
			// Token: 0x04005924 RID: 22820
			public static readonly StringName _upWedge = "_upWedge";

			/// <summary>
			/// Cached name for the '_upRightWedge' field.
			/// </summary>
			// Token: 0x04005925 RID: 22821
			public static readonly StringName _upRightWedge = "_upRightWedge";

			/// <summary>
			/// Cached name for the '_marker' field.
			/// </summary>
			// Token: 0x04005926 RID: 22822
			public static readonly StringName _marker = "_marker";

			/// <summary>
			/// Cached name for the '_ignoreNextMouseInput' field.
			/// </summary>
			// Token: 0x04005927 RID: 22823
			public static readonly StringName _ignoreNextMouseInput = "_ignoreNextMouseInput";

			/// <summary>
			/// Cached name for the '_centerPosition' field.
			/// </summary>
			// Token: 0x04005928 RID: 22824
			public static readonly StringName _centerPosition = "_centerPosition";

			/// <summary>
			/// Cached name for the '_selectedWedge' field.
			/// </summary>
			// Token: 0x04005929 RID: 22825
			public static readonly StringName _selectedWedge = "_selectedWedge";
		}

		/// <summary>
		/// Cached StringNames for the signals contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x020016E0 RID: 5856
		[NullableContext(0)]
		public class SignalName : Control.SignalName
		{
		}
	}
}
