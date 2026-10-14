using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Godot;
using Godot.Bridge;
using Godot.NativeInterop;

namespace MegaCrit.Sts2.Core.Nodes.Screens.Map
{
	// Token: 0x020002FA RID: 762
	[ScriptPath("res://src/Core/Nodes/Screens/Map/NMouseModeMapDrawingInput.cs")]
	public class NMouseModeMapDrawingInput : NMapDrawingInput
	{
		// Token: 0x06002989 RID: 10633 RVA: 0x00120C8C File Offset: 0x0011EE8C
		[NullableContext(1)]
		public override void _Input(InputEvent inputEvent)
		{
			if (!base.IsVisibleInTree())
			{
				return;
			}
			this.ProcessMouseDrawingEvent(inputEvent);
			InputEventMouseMotion inputEventMouseMotion = inputEvent as InputEventMouseMotion;
			if (inputEventMouseMotion != null && this._drawings.IsLocalDrawing())
			{
				this._drawings.UpdateCurrentLinePositionLocal(this._drawings.GetGlobalTransform().Inverse() * inputEventMouseMotion.GlobalPosition);
			}
		}

		/// <summary>
		/// Detects mouse click up/down and does drawing
		/// </summary>
		// Token: 0x0600298A RID: 10634 RVA: 0x00120CEC File Offset: 0x0011EEEC
		[NullableContext(1)]
		private void ProcessMouseDrawingEvent(InputEvent inputEvent)
		{
			InputEventMouseButton inputEventMouseButton = inputEvent as InputEventMouseButton;
			if (inputEventMouseButton != null)
			{
				if (inputEventMouseButton.ButtonIndex == 1L)
				{
					if (inputEventMouseButton.Pressed && !this._drawings.IsLocalDrawing())
					{
						this._drawings.BeginLineLocal(this._drawings.GetGlobalTransform().Inverse() * inputEventMouseButton.GlobalPosition, null);
						return;
					}
					if (!inputEventMouseButton.Pressed && this._drawings.IsLocalDrawing())
					{
						this._drawings.StopLineLocal();
						return;
					}
				}
				else
				{
					MouseButton buttonIndex = inputEventMouseButton.ButtonIndex;
					bool flag = buttonIndex - 2L <= 1L;
					if (flag && inputEventMouseButton.Pressed)
					{
						base.StopDrawing();
					}
				}
			}
		}

		/// <summary>
		/// Get the method information for all the methods declared in this class.
		/// This method is used by Godot to register the available methods in the editor.
		/// Do not call this method.
		/// </summary>
		// Token: 0x0600298B RID: 10635 RVA: 0x00120DA4 File Offset: 0x0011EFA4
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal new static List<MethodInfo> GetGodotMethodList()
		{
			return new List<MethodInfo>(2)
			{
				new MethodInfo(NMouseModeMapDrawingInput.MethodName._Input, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, new List<PropertyInfo>
				{
					new PropertyInfo(24L, "inputEvent", 0L, "", 6L, new StringName("InputEvent"), false)
				}, null),
				new MethodInfo(NMouseModeMapDrawingInput.MethodName.ProcessMouseDrawingEvent, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, new List<PropertyInfo>
				{
					new PropertyInfo(24L, "inputEvent", 0L, "", 6L, new StringName("InputEvent"), false)
				}, null)
			};
		}

		/// <inheritdoc />
		// Token: 0x0600298C RID: 10636 RVA: 0x00120E78 File Offset: 0x0011F078
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool InvokeGodotClassMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
		{
			if ((ref method) == NMouseModeMapDrawingInput.MethodName._Input && args.Count == 1)
			{
				this._Input(VariantUtils.ConvertTo<InputEvent>(args[0]));
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NMouseModeMapDrawingInput.MethodName.ProcessMouseDrawingEvent && args.Count == 1)
			{
				this.ProcessMouseDrawingEvent(VariantUtils.ConvertTo<InputEvent>(args[0]));
				ret = default(godot_variant);
				return true;
			}
			return base.InvokeGodotClassMethod(in method, args, out ret);
		}

		/// <inheritdoc />
		// Token: 0x0600298D RID: 10637 RVA: 0x00120EF4 File Offset: 0x0011F0F4
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool HasGodotClassMethod(in godot_string_name method)
		{
			return (ref method) == NMouseModeMapDrawingInput.MethodName._Input || (ref method) == NMouseModeMapDrawingInput.MethodName.ProcessMouseDrawingEvent || base.HasGodotClassMethod(in method);
		}

		/// <inheritdoc />
		// Token: 0x0600298E RID: 10638 RVA: 0x00120F1B File Offset: 0x0011F11B
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void SaveGodotObjectData(GodotSerializationInfo info)
		{
			base.SaveGodotObjectData(info);
		}

		/// <inheritdoc />
		// Token: 0x0600298F RID: 10639 RVA: 0x00120F24 File Offset: 0x0011F124
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void RestoreGodotObjectData(GodotSerializationInfo info)
		{
			base.RestoreGodotObjectData(info);
		}

		/// <summary>
		/// Cached StringNames for the methods contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x020014CE RID: 5326
		public new class MethodName : NMapDrawingInput.MethodName
		{
			/// <summary>
			/// Cached name for the '_Input' method.
			/// </summary>
			// Token: 0x04004F39 RID: 20281
			public static readonly StringName _Input = "_Input";

			/// <summary>
			/// Cached name for the 'ProcessMouseDrawingEvent' method.
			/// </summary>
			// Token: 0x04004F3A RID: 20282
			public static readonly StringName ProcessMouseDrawingEvent = "ProcessMouseDrawingEvent";
		}

		/// <summary>
		/// Cached StringNames for the properties and fields contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x020014CF RID: 5327
		public new class PropertyName : NMapDrawingInput.PropertyName
		{
		}

		/// <summary>
		/// Cached StringNames for the signals contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x020014D0 RID: 5328
		public new class SignalName : NMapDrawingInput.SignalName
		{
		}
	}
}
