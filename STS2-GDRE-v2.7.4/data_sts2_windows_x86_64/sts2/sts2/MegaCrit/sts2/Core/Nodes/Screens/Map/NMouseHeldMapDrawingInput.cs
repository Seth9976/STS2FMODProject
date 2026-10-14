using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Godot;
using Godot.Bridge;
using Godot.NativeInterop;

namespace MegaCrit.Sts2.Core.Nodes.Screens.Map
{
	// Token: 0x020002F9 RID: 761
	[ScriptPath("res://src/Core/Nodes/Screens/Map/NMouseHeldMapDrawingInput.cs")]
	public class NMouseHeldMapDrawingInput : NMapDrawingInput
	{
		// Token: 0x1700072C RID: 1836
		// (get) Token: 0x0600297D RID: 10621 RVA: 0x0012093B File Offset: 0x0011EB3B
		private MouseButton ListeningButton
		{
			get
			{
				if (base.DrawingMode != DrawingMode.Drawing)
				{
					return 3L;
				}
				return 2L;
			}
		}

		// Token: 0x0600297E RID: 10622 RVA: 0x0012094C File Offset: 0x0011EB4C
		public override void _Ready()
		{
			base._Ready();
			this._drawings.BeginLineLocal(this._drawings.GetGlobalTransform().Inverse() * base.GetGlobalMousePosition(), null);
		}

		// Token: 0x0600297F RID: 10623 RVA: 0x00120994 File Offset: 0x0011EB94
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

		// Token: 0x06002980 RID: 10624 RVA: 0x001209F4 File Offset: 0x0011EBF4
		[NullableContext(1)]
		private void ProcessMouseDrawingEvent(InputEvent inputEvent)
		{
			InputEventMouseButton inputEventMouseButton = inputEvent as InputEventMouseButton;
			if (inputEventMouseButton != null && inputEventMouseButton.ButtonIndex == this.ListeningButton && !inputEventMouseButton.IsPressed())
			{
				base.StopDrawing();
			}
		}

		/// <summary>
		/// Get the method information for all the methods declared in this class.
		/// This method is used by Godot to register the available methods in the editor.
		/// Do not call this method.
		/// </summary>
		// Token: 0x06002981 RID: 10625 RVA: 0x00120A28 File Offset: 0x0011EC28
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal new static List<MethodInfo> GetGodotMethodList()
		{
			return new List<MethodInfo>(3)
			{
				new MethodInfo(NMouseHeldMapDrawingInput.MethodName._Ready, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NMouseHeldMapDrawingInput.MethodName._Input, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, new List<PropertyInfo>
				{
					new PropertyInfo(24L, "inputEvent", 0L, "", 6L, new StringName("InputEvent"), false)
				}, null),
				new MethodInfo(NMouseHeldMapDrawingInput.MethodName.ProcessMouseDrawingEvent, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, new List<PropertyInfo>
				{
					new PropertyInfo(24L, "inputEvent", 0L, "", 6L, new StringName("InputEvent"), false)
				}, null)
			};
		}

		/// <inheritdoc />
		// Token: 0x06002982 RID: 10626 RVA: 0x00120B2C File Offset: 0x0011ED2C
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool InvokeGodotClassMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
		{
			if ((ref method) == NMouseHeldMapDrawingInput.MethodName._Ready && args.Count == 0)
			{
				this._Ready();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NMouseHeldMapDrawingInput.MethodName._Input && args.Count == 1)
			{
				this._Input(VariantUtils.ConvertTo<InputEvent>(args[0]));
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NMouseHeldMapDrawingInput.MethodName.ProcessMouseDrawingEvent && args.Count == 1)
			{
				this.ProcessMouseDrawingEvent(VariantUtils.ConvertTo<InputEvent>(args[0]));
				ret = default(godot_variant);
				return true;
			}
			return base.InvokeGodotClassMethod(in method, args, out ret);
		}

		/// <inheritdoc />
		// Token: 0x06002983 RID: 10627 RVA: 0x00120BCD File Offset: 0x0011EDCD
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool HasGodotClassMethod(in godot_string_name method)
		{
			return (ref method) == NMouseHeldMapDrawingInput.MethodName._Ready || (ref method) == NMouseHeldMapDrawingInput.MethodName._Input || (ref method) == NMouseHeldMapDrawingInput.MethodName.ProcessMouseDrawingEvent || base.HasGodotClassMethod(in method);
		}

		/// <inheritdoc />
		// Token: 0x06002984 RID: 10628 RVA: 0x00120C04 File Offset: 0x0011EE04
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
		{
			if ((ref name) == NMouseHeldMapDrawingInput.PropertyName.ListeningButton)
			{
				MouseButton listeningButton = this.ListeningButton;
				value = VariantUtils.CreateFrom<MouseButton>(ref listeningButton);
				return true;
			}
			return base.GetGodotClassPropertyValue(in name, out value);
		}

		/// <summary>
		/// Get the property information for all the properties declared in this class.
		/// This method is used by Godot to register the available properties in the editor.
		/// Do not call this method.
		/// </summary>
		// Token: 0x06002985 RID: 10629 RVA: 0x00120C3C File Offset: 0x0011EE3C
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal new static List<PropertyInfo> GetGodotPropertyList()
		{
			return new List<PropertyInfo>
			{
				new PropertyInfo(2L, NMouseHeldMapDrawingInput.PropertyName.ListeningButton, 0L, "", 4096L, false)
			};
		}

		/// <inheritdoc />
		// Token: 0x06002986 RID: 10630 RVA: 0x00120C70 File Offset: 0x0011EE70
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void SaveGodotObjectData(GodotSerializationInfo info)
		{
			base.SaveGodotObjectData(info);
		}

		/// <inheritdoc />
		// Token: 0x06002987 RID: 10631 RVA: 0x00120C79 File Offset: 0x0011EE79
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void RestoreGodotObjectData(GodotSerializationInfo info)
		{
			base.RestoreGodotObjectData(info);
		}

		/// <summary>
		/// Cached StringNames for the methods contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x020014CB RID: 5323
		public new class MethodName : NMapDrawingInput.MethodName
		{
			/// <summary>
			/// Cached name for the '_Ready' method.
			/// </summary>
			// Token: 0x04004F35 RID: 20277
			public new static readonly StringName _Ready = "_Ready";

			/// <summary>
			/// Cached name for the '_Input' method.
			/// </summary>
			// Token: 0x04004F36 RID: 20278
			public static readonly StringName _Input = "_Input";

			/// <summary>
			/// Cached name for the 'ProcessMouseDrawingEvent' method.
			/// </summary>
			// Token: 0x04004F37 RID: 20279
			public static readonly StringName ProcessMouseDrawingEvent = "ProcessMouseDrawingEvent";
		}

		/// <summary>
		/// Cached StringNames for the properties and fields contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x020014CC RID: 5324
		public new class PropertyName : NMapDrawingInput.PropertyName
		{
			/// <summary>
			/// Cached name for the 'ListeningButton' property.
			/// </summary>
			// Token: 0x04004F38 RID: 20280
			public static readonly StringName ListeningButton = "ListeningButton";
		}

		/// <summary>
		/// Cached StringNames for the signals contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x020014CD RID: 5325
		public new class SignalName : NMapDrawingInput.SignalName
		{
		}
	}
}
