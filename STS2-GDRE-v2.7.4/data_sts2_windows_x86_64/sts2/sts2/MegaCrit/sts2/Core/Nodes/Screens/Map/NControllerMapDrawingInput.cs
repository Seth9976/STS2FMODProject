using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Godot;
using Godot.Bridge;
using Godot.NativeInterop;
using MegaCrit.Sts2.Core.Assets;
using MegaCrit.Sts2.Core.ControllerInput;
using MegaCrit.Sts2.Core.Nodes.CommonUi;
using MegaCrit.Sts2.Core.Nodes.GodotExtensions;
using MegaCrit.Sts2.Core.Nodes.Screens.ScreenContext;

namespace MegaCrit.Sts2.Core.Nodes.Screens.Map
{
	// Token: 0x020002EE RID: 750
	[ScriptPath("res://src/Core/Nodes/Screens/Map/NControllerMapDrawingInput.cs")]
	public class NControllerMapDrawingInput : NMapDrawingInput
	{
		// Token: 0x06002849 RID: 10313 RVA: 0x001144D4 File Offset: 0x001126D4
		[NullableContext(1)]
		public static NMapDrawingInput Create()
		{
			return PreloadManager.Cache.GetScene("res://scenes/screens/map/controller_map_drawing_input.tscn").Instantiate<NControllerMapDrawingInput>(0L);
		}

		// Token: 0x0600284A RID: 10314 RVA: 0x001144EC File Offset: 0x001126EC
		public override void _Ready()
		{
			base._Ready();
			this._cursor = base.GetNode<Control>("%Cursor");
			this.TryGrabFocus();
			base.GetViewport().Connect(Viewport.SignalName.GuiFocusChanged, Callable.From<Control>(delegate(Control _)
			{
				base.StopDrawing();
			}), 0U);
			if (base.DrawingMode == DrawingMode.Drawing)
			{
				this._cursorTex = ImageTexture.CreateFromImage(PreloadManager.Cache.GetAsset<Image>("res://images/packed/common_ui/cursor_quill.png"));
				this._cursorTiltedTex = ImageTexture.CreateFromImage(PreloadManager.Cache.GetAsset<Image>("res://images/packed/common_ui/cursor_quill_tilted.png"));
			}
			else
			{
				this._cursorTex = ImageTexture.CreateFromImage(PreloadManager.Cache.GetAsset<Image>("res://images/packed/common_ui/cursor_eraser.png"));
				this._cursorTiltedTex = ImageTexture.CreateFromImage(PreloadManager.Cache.GetAsset<Image>("res://images/packed/common_ui/cursor_eraser_tilted.png"));
			}
			this._cursor.GetNode<TextureRect>("TextureRect").Texture = this._cursorTex;
			this._cursor.GetNode<TextureRect>("TextureRect").Position = ((base.DrawingMode == DrawingMode.Drawing) ? this._drawingIconPos : this._eraserIconPos);
		}

		// Token: 0x0600284B RID: 10315 RVA: 0x00114604 File Offset: 0x00112804
		public override void _Process(double delta)
		{
			if (Input.IsActionPressed(MegaInput.select, false))
			{
				if (!this._isPressed)
				{
					this._drawings.BeginLineLocal(this._drawings.GetGlobalTransform().Inverse() * this._cursor.GlobalPosition, null);
					this._cursor.GetNode<TextureRect>("TextureRect").Texture = this._cursorTiltedTex;
					this._isPressed = true;
				}
			}
			else if (this._isPressed)
			{
				this._drawings.StopLineLocal();
				this._cursor.GetNode<TextureRect>("TextureRect").Texture = this._cursorTex;
				this._isPressed = false;
			}
			this._direction = NControllerManager.Instance.GetLeftAnalogStickDirection();
			if (this._direction.Length() < 0.1f)
			{
				this._direction += Input.GetVector(Controller.dPadLeft, Controller.dPadRight, Controller.dPadUp, Controller.dPadDown, -1f);
			}
			if (this._direction.Length() > 0f)
			{
				this._cursor.GlobalPosition += this._direction * 700f * (float)delta;
				this._cursor.GlobalPosition = this._cursor.GlobalPosition.Clamp(NGame.Instance.GlobalPosition, NGame.Instance.GlobalPosition + NGame.Instance.Size);
				if (this._drawings.IsLocalDrawing())
				{
					this._drawings.UpdateCurrentLinePositionLocal(this._drawings.GetGlobalTransform().Inverse() * this._cursor.GlobalPosition);
				}
			}
		}

		// Token: 0x0600284C RID: 10316 RVA: 0x001147D2 File Offset: 0x001129D2
		[NullableContext(1)]
		public override void _Input(InputEvent input)
		{
			if (!base.IsVisibleInTree())
			{
				return;
			}
			if (input.IsActionPressed(MegaInput.cancel, false, false))
			{
				base.StopDrawing();
				ActiveScreenContext.Instance.FocusOnDefaultControl();
			}
		}

		/// <summary>
		/// Get the method information for all the methods declared in this class.
		/// This method is used by Godot to register the available methods in the editor.
		/// Do not call this method.
		/// </summary>
		// Token: 0x0600284D RID: 10317 RVA: 0x001147FC File Offset: 0x001129FC
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal new static List<MethodInfo> GetGodotMethodList()
		{
			return new List<MethodInfo>(4)
			{
				new MethodInfo(NControllerMapDrawingInput.MethodName.Create, new PropertyInfo(24L, "", 0L, "", 6L, new StringName("Control"), false), 33L, null, null),
				new MethodInfo(NControllerMapDrawingInput.MethodName._Ready, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NControllerMapDrawingInput.MethodName._Process, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, new List<PropertyInfo>
				{
					new PropertyInfo(3L, "delta", 0L, "", 6L, false)
				}, null),
				new MethodInfo(NControllerMapDrawingInput.MethodName._Input, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, new List<PropertyInfo>
				{
					new PropertyInfo(24L, "input", 0L, "", 6L, new StringName("InputEvent"), false)
				}, null)
			};
		}

		/// <inheritdoc />
		// Token: 0x0600284E RID: 10318 RVA: 0x00114930 File Offset: 0x00112B30
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool InvokeGodotClassMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
		{
			if ((ref method) == NControllerMapDrawingInput.MethodName.Create && args.Count == 0)
			{
				NMapDrawingInput nmapDrawingInput = NControllerMapDrawingInput.Create();
				ret = VariantUtils.CreateFrom<NMapDrawingInput>(ref nmapDrawingInput);
				return true;
			}
			if ((ref method) == NControllerMapDrawingInput.MethodName._Ready && args.Count == 0)
			{
				this._Ready();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NControllerMapDrawingInput.MethodName._Process && args.Count == 1)
			{
				this._Process(VariantUtils.ConvertTo<double>(args[0]));
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NControllerMapDrawingInput.MethodName._Input && args.Count == 1)
			{
				this._Input(VariantUtils.ConvertTo<InputEvent>(args[0]));
				ret = default(godot_variant);
				return true;
			}
			return base.InvokeGodotClassMethod(in method, args, out ret);
		}

		// Token: 0x0600284F RID: 10319 RVA: 0x001149FC File Offset: 0x00112BFC
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal new static bool InvokeGodotClassStaticMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
		{
			if ((ref method) == NControllerMapDrawingInput.MethodName.Create && args.Count == 0)
			{
				NMapDrawingInput nmapDrawingInput = NControllerMapDrawingInput.Create();
				ret = VariantUtils.CreateFrom<NMapDrawingInput>(ref nmapDrawingInput);
				return true;
			}
			ret = default(godot_variant);
			return false;
		}

		/// <inheritdoc />
		// Token: 0x06002850 RID: 10320 RVA: 0x00114A3C File Offset: 0x00112C3C
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool HasGodotClassMethod(in godot_string_name method)
		{
			return (ref method) == NControllerMapDrawingInput.MethodName.Create || (ref method) == NControllerMapDrawingInput.MethodName._Ready || (ref method) == NControllerMapDrawingInput.MethodName._Process || (ref method) == NControllerMapDrawingInput.MethodName._Input || base.HasGodotClassMethod(in method);
		}

		/// <inheritdoc />
		// Token: 0x06002851 RID: 10321 RVA: 0x00114A8C File Offset: 0x00112C8C
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool SetGodotClassPropertyValue(in godot_string_name name, in godot_variant value)
		{
			if ((ref name) == NControllerMapDrawingInput.PropertyName._eraserIconPos)
			{
				this._eraserIconPos = VariantUtils.ConvertTo<Vector2>(ref value);
				return true;
			}
			if ((ref name) == NControllerMapDrawingInput.PropertyName._drawingIconPos)
			{
				this._drawingIconPos = VariantUtils.ConvertTo<Vector2>(ref value);
				return true;
			}
			if ((ref name) == NControllerMapDrawingInput.PropertyName._isPressed)
			{
				this._isPressed = VariantUtils.ConvertTo<bool>(ref value);
				return true;
			}
			if ((ref name) == NControllerMapDrawingInput.PropertyName._cursorTex)
			{
				this._cursorTex = VariantUtils.ConvertTo<Texture2D>(ref value);
				return true;
			}
			if ((ref name) == NControllerMapDrawingInput.PropertyName._cursorTiltedTex)
			{
				this._cursorTiltedTex = VariantUtils.ConvertTo<Texture2D>(ref value);
				return true;
			}
			if ((ref name) == NControllerMapDrawingInput.PropertyName._cursor)
			{
				this._cursor = VariantUtils.ConvertTo<Control>(ref value);
				return true;
			}
			if ((ref name) == NControllerMapDrawingInput.PropertyName._direction)
			{
				this._direction = VariantUtils.ConvertTo<Vector2>(ref value);
				return true;
			}
			return base.SetGodotClassPropertyValue(in name, in value);
		}

		/// <inheritdoc />
		// Token: 0x06002852 RID: 10322 RVA: 0x00114B60 File Offset: 0x00112D60
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
		{
			if ((ref name) == NControllerMapDrawingInput.PropertyName._eraserIconPos)
			{
				value = VariantUtils.CreateFrom<Vector2>(ref this._eraserIconPos);
				return true;
			}
			if ((ref name) == NControllerMapDrawingInput.PropertyName._drawingIconPos)
			{
				value = VariantUtils.CreateFrom<Vector2>(ref this._drawingIconPos);
				return true;
			}
			if ((ref name) == NControllerMapDrawingInput.PropertyName._isPressed)
			{
				value = VariantUtils.CreateFrom<bool>(ref this._isPressed);
				return true;
			}
			if ((ref name) == NControllerMapDrawingInput.PropertyName._cursorTex)
			{
				value = VariantUtils.CreateFrom<Texture2D>(ref this._cursorTex);
				return true;
			}
			if ((ref name) == NControllerMapDrawingInput.PropertyName._cursorTiltedTex)
			{
				value = VariantUtils.CreateFrom<Texture2D>(ref this._cursorTiltedTex);
				return true;
			}
			if ((ref name) == NControllerMapDrawingInput.PropertyName._cursor)
			{
				value = VariantUtils.CreateFrom<Control>(ref this._cursor);
				return true;
			}
			if ((ref name) == NControllerMapDrawingInput.PropertyName._direction)
			{
				value = VariantUtils.CreateFrom<Vector2>(ref this._direction);
				return true;
			}
			return base.GetGodotClassPropertyValue(in name, out value);
		}

		/// <summary>
		/// Get the property information for all the properties declared in this class.
		/// This method is used by Godot to register the available properties in the editor.
		/// Do not call this method.
		/// </summary>
		// Token: 0x06002853 RID: 10323 RVA: 0x00114C58 File Offset: 0x00112E58
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal new static List<PropertyInfo> GetGodotPropertyList()
		{
			return new List<PropertyInfo>
			{
				new PropertyInfo(5L, NControllerMapDrawingInput.PropertyName._eraserIconPos, 0L, "", 4096L, false),
				new PropertyInfo(5L, NControllerMapDrawingInput.PropertyName._drawingIconPos, 0L, "", 4096L, false),
				new PropertyInfo(1L, NControllerMapDrawingInput.PropertyName._isPressed, 0L, "", 4096L, false),
				new PropertyInfo(24L, NControllerMapDrawingInput.PropertyName._cursorTex, 0L, "", 4096L, false),
				new PropertyInfo(24L, NControllerMapDrawingInput.PropertyName._cursorTiltedTex, 0L, "", 4096L, false),
				new PropertyInfo(24L, NControllerMapDrawingInput.PropertyName._cursor, 0L, "", 4096L, false),
				new PropertyInfo(5L, NControllerMapDrawingInput.PropertyName._direction, 0L, "", 4096L, false)
			};
		}

		/// <inheritdoc />
		// Token: 0x06002854 RID: 10324 RVA: 0x00114D50 File Offset: 0x00112F50
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void SaveGodotObjectData(GodotSerializationInfo info)
		{
			base.SaveGodotObjectData(info);
			info.AddProperty(NControllerMapDrawingInput.PropertyName._eraserIconPos, Variant.From<Vector2>(ref this._eraserIconPos));
			info.AddProperty(NControllerMapDrawingInput.PropertyName._drawingIconPos, Variant.From<Vector2>(ref this._drawingIconPos));
			info.AddProperty(NControllerMapDrawingInput.PropertyName._isPressed, Variant.From<bool>(ref this._isPressed));
			info.AddProperty(NControllerMapDrawingInput.PropertyName._cursorTex, Variant.From<Texture2D>(ref this._cursorTex));
			info.AddProperty(NControllerMapDrawingInput.PropertyName._cursorTiltedTex, Variant.From<Texture2D>(ref this._cursorTiltedTex));
			info.AddProperty(NControllerMapDrawingInput.PropertyName._cursor, Variant.From<Control>(ref this._cursor));
			info.AddProperty(NControllerMapDrawingInput.PropertyName._direction, Variant.From<Vector2>(ref this._direction));
		}

		/// <inheritdoc />
		// Token: 0x06002855 RID: 10325 RVA: 0x00114E00 File Offset: 0x00113000
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void RestoreGodotObjectData(GodotSerializationInfo info)
		{
			base.RestoreGodotObjectData(info);
			Variant variant;
			if (info.TryGetProperty(NControllerMapDrawingInput.PropertyName._eraserIconPos, ref variant))
			{
				this._eraserIconPos = variant.As<Vector2>();
			}
			Variant variant2;
			if (info.TryGetProperty(NControllerMapDrawingInput.PropertyName._drawingIconPos, ref variant2))
			{
				this._drawingIconPos = variant2.As<Vector2>();
			}
			Variant variant3;
			if (info.TryGetProperty(NControllerMapDrawingInput.PropertyName._isPressed, ref variant3))
			{
				this._isPressed = variant3.As<bool>();
			}
			Variant variant4;
			if (info.TryGetProperty(NControllerMapDrawingInput.PropertyName._cursorTex, ref variant4))
			{
				this._cursorTex = variant4.As<Texture2D>();
			}
			Variant variant5;
			if (info.TryGetProperty(NControllerMapDrawingInput.PropertyName._cursorTiltedTex, ref variant5))
			{
				this._cursorTiltedTex = variant5.As<Texture2D>();
			}
			Variant variant6;
			if (info.TryGetProperty(NControllerMapDrawingInput.PropertyName._cursor, ref variant6))
			{
				this._cursor = variant6.As<Control>();
			}
			Variant variant7;
			if (info.TryGetProperty(NControllerMapDrawingInput.PropertyName._direction, ref variant7))
			{
				this._direction = variant7.As<Vector2>();
			}
		}

		// Token: 0x04000EFA RID: 3834
		[Nullable(1)]
		private const string _scenePath = "res://scenes/screens/map/controller_map_drawing_input.tscn";

		// Token: 0x04000EFB RID: 3835
		private Vector2 _eraserIconPos = new Vector2(-34f, -76f);

		// Token: 0x04000EFC RID: 3836
		private Vector2 _drawingIconPos = new Vector2(-10f, -76f);

		// Token: 0x04000EFD RID: 3837
		private bool _isPressed;

		// Token: 0x04000EFE RID: 3838
		[Nullable(1)]
		private Texture2D _cursorTex;

		// Token: 0x04000EFF RID: 3839
		[Nullable(1)]
		private Texture2D _cursorTiltedTex;

		// Token: 0x04000F00 RID: 3840
		[Nullable(1)]
		private Control _cursor;

		// Token: 0x04000F01 RID: 3841
		private Vector2 _direction;

		/// <summary>
		/// Cached StringNames for the methods contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x0200149B RID: 5275
		public new class MethodName : NMapDrawingInput.MethodName
		{
			/// <summary>
			/// Cached name for the 'Create' method.
			/// </summary>
			// Token: 0x04004E35 RID: 20021
			public new static readonly StringName Create = "Create";

			/// <summary>
			/// Cached name for the '_Ready' method.
			/// </summary>
			// Token: 0x04004E36 RID: 20022
			public new static readonly StringName _Ready = "_Ready";

			/// <summary>
			/// Cached name for the '_Process' method.
			/// </summary>
			// Token: 0x04004E37 RID: 20023
			public static readonly StringName _Process = "_Process";

			/// <summary>
			/// Cached name for the '_Input' method.
			/// </summary>
			// Token: 0x04004E38 RID: 20024
			public static readonly StringName _Input = "_Input";
		}

		/// <summary>
		/// Cached StringNames for the properties and fields contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x0200149C RID: 5276
		public new class PropertyName : NMapDrawingInput.PropertyName
		{
			/// <summary>
			/// Cached name for the '_eraserIconPos' field.
			/// </summary>
			// Token: 0x04004E39 RID: 20025
			public static readonly StringName _eraserIconPos = "_eraserIconPos";

			/// <summary>
			/// Cached name for the '_drawingIconPos' field.
			/// </summary>
			// Token: 0x04004E3A RID: 20026
			public static readonly StringName _drawingIconPos = "_drawingIconPos";

			/// <summary>
			/// Cached name for the '_isPressed' field.
			/// </summary>
			// Token: 0x04004E3B RID: 20027
			public static readonly StringName _isPressed = "_isPressed";

			/// <summary>
			/// Cached name for the '_cursorTex' field.
			/// </summary>
			// Token: 0x04004E3C RID: 20028
			public static readonly StringName _cursorTex = "_cursorTex";

			/// <summary>
			/// Cached name for the '_cursorTiltedTex' field.
			/// </summary>
			// Token: 0x04004E3D RID: 20029
			public static readonly StringName _cursorTiltedTex = "_cursorTiltedTex";

			/// <summary>
			/// Cached name for the '_cursor' field.
			/// </summary>
			// Token: 0x04004E3E RID: 20030
			public static readonly StringName _cursor = "_cursor";

			/// <summary>
			/// Cached name for the '_direction' field.
			/// </summary>
			// Token: 0x04004E3F RID: 20031
			public static readonly StringName _direction = "_direction";
		}

		/// <summary>
		/// Cached StringNames for the signals contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x0200149D RID: 5277
		public new class SignalName : NMapDrawingInput.SignalName
		{
		}
	}
}
