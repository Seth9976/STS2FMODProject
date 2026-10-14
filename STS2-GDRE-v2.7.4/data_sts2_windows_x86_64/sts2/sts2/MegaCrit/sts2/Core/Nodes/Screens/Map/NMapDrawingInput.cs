using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Godot;
using Godot.Bridge;
using Godot.NativeInterop;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.Nodes.CommonUi;
using MegaCrit.Sts2.Core.Nodes.Screens.ScreenContext;

namespace MegaCrit.Sts2.Core.Nodes.Screens.Map
{
	/// <summary>
	/// An abstract base class to manage the drawing on the map
	/// Inherited classes manage map drawing for different types of inputs.
	/// Is created and destroyed on demand.
	/// </summary>
	// Token: 0x020002F2 RID: 754
	[ScriptPath("res://src/Core/Nodes/Screens/Map/NMapDrawingInput.cs")]
	public abstract class NMapDrawingInput : Control
	{
		// Token: 0x17000713 RID: 1811
		// (get) Token: 0x06002882 RID: 10370 RVA: 0x0011644E File Offset: 0x0011464E
		// (set) Token: 0x06002883 RID: 10371 RVA: 0x00116456 File Offset: 0x00114656
		public DrawingMode DrawingMode { get; private set; }

		// Token: 0x06002884 RID: 10372 RVA: 0x00116460 File Offset: 0x00114660
		[NullableContext(1)]
		public static NMapDrawingInput Create(NMapDrawings drawings, DrawingMode drawingMode, bool stopOnMouseRelease = false)
		{
			NMapDrawingInput nmapDrawingInput;
			if (stopOnMouseRelease)
			{
				nmapDrawingInput = new NMouseHeldMapDrawingInput();
			}
			else if (NControllerManager.Instance.IsUsingController)
			{
				nmapDrawingInput = NControllerMapDrawingInput.Create();
			}
			else
			{
				nmapDrawingInput = new NMouseModeMapDrawingInput();
			}
			nmapDrawingInput._drawings = drawings;
			nmapDrawingInput.DrawingMode = drawingMode;
			nmapDrawingInput._drawings.SetDrawingModeLocal(drawingMode);
			return nmapDrawingInput;
		}

		// Token: 0x06002885 RID: 10373 RVA: 0x001164B0 File Offset: 0x001146B0
		public override void _Ready()
		{
			NControllerManager.Instance.Connect(NControllerManager.SignalName.MouseDetected, Callable.From(new Action(this.StopDrawing)), 0U);
			NControllerManager.Instance.Connect(NControllerManager.SignalName.ControllerDetected, Callable.From(new Action(this.StopDrawing)), 0U);
		}

		// Token: 0x06002886 RID: 10374 RVA: 0x00116501 File Offset: 0x00114701
		public override void _EnterTree()
		{
			ActiveScreenContext.Instance.Updated += this.StopDrawing;
		}

		// Token: 0x06002887 RID: 10375 RVA: 0x00116519 File Offset: 0x00114719
		public override void _ExitTree()
		{
			ActiveScreenContext.Instance.Updated -= this.StopDrawing;
		}

		// Token: 0x06002888 RID: 10376 RVA: 0x00116531 File Offset: 0x00114731
		public void StopDrawing()
		{
			if (this._drawings.IsLocalDrawing())
			{
				this._drawings.StopLineLocal();
			}
			this._drawings.SetDrawingModeLocal(DrawingMode.None);
			this.EmitSignalFinished();
			this.QueueFreeSafely();
		}

		/// <summary>
		/// Get the method information for all the methods declared in this class.
		/// This method is used by Godot to register the available methods in the editor.
		/// Do not call this method.
		/// </summary>
		// Token: 0x06002889 RID: 10377 RVA: 0x00116564 File Offset: 0x00114764
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal static List<MethodInfo> GetGodotMethodList()
		{
			return new List<MethodInfo>(5)
			{
				new MethodInfo(NMapDrawingInput.MethodName.Create, new PropertyInfo(24L, "", 0L, "", 6L, new StringName("Control"), false), 33L, new List<PropertyInfo>
				{
					new PropertyInfo(24L, "drawings", 0L, "", 6L, new StringName("Control"), false),
					new PropertyInfo(2L, "drawingMode", 0L, "", 6L, false),
					new PropertyInfo(1L, "stopOnMouseRelease", 0L, "", 6L, false)
				}, null),
				new MethodInfo(NMapDrawingInput.MethodName._Ready, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NMapDrawingInput.MethodName._EnterTree, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NMapDrawingInput.MethodName._ExitTree, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NMapDrawingInput.MethodName.StopDrawing, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null)
			};
		}

		/// <inheritdoc />
		// Token: 0x0600288A RID: 10378 RVA: 0x001166E4 File Offset: 0x001148E4
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool InvokeGodotClassMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
		{
			if ((ref method) == NMapDrawingInput.MethodName.Create && args.Count == 3)
			{
				NMapDrawingInput nmapDrawingInput = NMapDrawingInput.Create(VariantUtils.ConvertTo<NMapDrawings>(args[0]), VariantUtils.ConvertTo<DrawingMode>(args[1]), VariantUtils.ConvertTo<bool>(args[2]));
				ret = VariantUtils.CreateFrom<NMapDrawingInput>(ref nmapDrawingInput);
				return true;
			}
			if ((ref method) == NMapDrawingInput.MethodName._Ready && args.Count == 0)
			{
				this._Ready();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NMapDrawingInput.MethodName._EnterTree && args.Count == 0)
			{
				this._EnterTree();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NMapDrawingInput.MethodName._ExitTree && args.Count == 0)
			{
				this._ExitTree();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NMapDrawingInput.MethodName.StopDrawing && args.Count == 0)
			{
				this.StopDrawing();
				ret = default(godot_variant);
				return true;
			}
			return base.InvokeGodotClassMethod(ref method, args, ref ret);
		}

		// Token: 0x0600288B RID: 10379 RVA: 0x001167E4 File Offset: 0x001149E4
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal static bool InvokeGodotClassStaticMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
		{
			if ((ref method) == NMapDrawingInput.MethodName.Create && args.Count == 3)
			{
				NMapDrawingInput nmapDrawingInput = NMapDrawingInput.Create(VariantUtils.ConvertTo<NMapDrawings>(args[0]), VariantUtils.ConvertTo<DrawingMode>(args[1]), VariantUtils.ConvertTo<bool>(args[2]));
				ret = VariantUtils.CreateFrom<NMapDrawingInput>(ref nmapDrawingInput);
				return true;
			}
			ret = default(godot_variant);
			return false;
		}

		/// <inheritdoc />
		// Token: 0x0600288C RID: 10380 RVA: 0x0011684C File Offset: 0x00114A4C
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool HasGodotClassMethod(in godot_string_name method)
		{
			return (ref method) == NMapDrawingInput.MethodName.Create || (ref method) == NMapDrawingInput.MethodName._Ready || (ref method) == NMapDrawingInput.MethodName._EnterTree || (ref method) == NMapDrawingInput.MethodName._ExitTree || (ref method) == NMapDrawingInput.MethodName.StopDrawing || base.HasGodotClassMethod(ref method);
		}

		/// <inheritdoc />
		// Token: 0x0600288D RID: 10381 RVA: 0x001168AB File Offset: 0x00114AAB
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool SetGodotClassPropertyValue(in godot_string_name name, in godot_variant value)
		{
			if ((ref name) == NMapDrawingInput.PropertyName.DrawingMode)
			{
				this.DrawingMode = VariantUtils.ConvertTo<DrawingMode>(ref value);
				return true;
			}
			if ((ref name) == NMapDrawingInput.PropertyName._drawings)
			{
				this._drawings = VariantUtils.ConvertTo<NMapDrawings>(ref value);
				return true;
			}
			return base.SetGodotClassPropertyValue(ref name, ref value);
		}

		/// <inheritdoc />
		// Token: 0x0600288E RID: 10382 RVA: 0x001168EC File Offset: 0x00114AEC
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
		{
			if ((ref name) == NMapDrawingInput.PropertyName.DrawingMode)
			{
				DrawingMode drawingMode = this.DrawingMode;
				value = VariantUtils.CreateFrom<DrawingMode>(ref drawingMode);
				return true;
			}
			if ((ref name) == NMapDrawingInput.PropertyName._drawings)
			{
				value = VariantUtils.CreateFrom<NMapDrawings>(ref this._drawings);
				return true;
			}
			return base.GetGodotClassPropertyValue(ref name, ref value);
		}

		/// <summary>
		/// Get the property information for all the properties declared in this class.
		/// This method is used by Godot to register the available properties in the editor.
		/// Do not call this method.
		/// </summary>
		// Token: 0x0600288F RID: 10383 RVA: 0x00116944 File Offset: 0x00114B44
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal static List<PropertyInfo> GetGodotPropertyList()
		{
			return new List<PropertyInfo>
			{
				new PropertyInfo(24L, NMapDrawingInput.PropertyName._drawings, 0L, "", 4096L, false),
				new PropertyInfo(2L, NMapDrawingInput.PropertyName.DrawingMode, 0L, "", 4096L, false)
			};
		}

		/// <inheritdoc />
		// Token: 0x06002890 RID: 10384 RVA: 0x0011699C File Offset: 0x00114B9C
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void SaveGodotObjectData(GodotSerializationInfo info)
		{
			base.SaveGodotObjectData(info);
			StringName drawingMode = NMapDrawingInput.PropertyName.DrawingMode;
			DrawingMode drawingMode2 = this.DrawingMode;
			info.AddProperty(drawingMode, Variant.From<DrawingMode>(ref drawingMode2));
			info.AddProperty(NMapDrawingInput.PropertyName._drawings, Variant.From<NMapDrawings>(ref this._drawings));
			info.AddSignalEventDelegate(NMapDrawingInput.SignalName.Finished, this.backing_Finished);
		}

		/// <inheritdoc />
		// Token: 0x06002891 RID: 10385 RVA: 0x001169F0 File Offset: 0x00114BF0
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void RestoreGodotObjectData(GodotSerializationInfo info)
		{
			base.RestoreGodotObjectData(info);
			Variant variant;
			if (info.TryGetProperty(NMapDrawingInput.PropertyName.DrawingMode, ref variant))
			{
				this.DrawingMode = variant.As<DrawingMode>();
			}
			Variant variant2;
			if (info.TryGetProperty(NMapDrawingInput.PropertyName._drawings, ref variant2))
			{
				this._drawings = variant2.As<NMapDrawings>();
			}
			NMapDrawingInput.FinishedEventHandler finishedEventHandler;
			if (info.TryGetSignalEventDelegate<NMapDrawingInput.FinishedEventHandler>(NMapDrawingInput.SignalName.Finished, ref finishedEventHandler))
			{
				this.backing_Finished = finishedEventHandler;
			}
		}

		/// <summary>
		/// Get the signal information for all the signals declared in this class.
		/// This method is used by Godot to register the available signals in the editor.
		/// Do not call this method.
		/// </summary>
		// Token: 0x06002892 RID: 10386 RVA: 0x00116A54 File Offset: 0x00114C54
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal static List<MethodInfo> GetGodotSignalList()
		{
			return new List<MethodInfo>(1)
			{
				new MethodInfo(NMapDrawingInput.SignalName.Finished, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null)
			};
		}

		/// <inheritdoc cref="T:MegaCrit.Sts2.Core.Nodes.Screens.Map.NMapDrawingInput.FinishedEventHandler" />
		// Token: 0x14000021 RID: 33
		// (add) Token: 0x06002893 RID: 10387 RVA: 0x00116A98 File Offset: 0x00114C98
		// (remove) Token: 0x06002894 RID: 10388 RVA: 0x00116AB1 File Offset: 0x00114CB1
		public event NMapDrawingInput.FinishedEventHandler Finished
		{
			add
			{
				this.backing_Finished = (NMapDrawingInput.FinishedEventHandler)Delegate.Combine(this.backing_Finished, value);
			}
			remove
			{
				this.backing_Finished = (NMapDrawingInput.FinishedEventHandler)Delegate.Remove(this.backing_Finished, value);
			}
		}

		// Token: 0x06002895 RID: 10389 RVA: 0x00116ACA File Offset: 0x00114CCA
		protected void EmitSignalFinished()
		{
			base.EmitSignal(NMapDrawingInput.SignalName.Finished, Array.Empty<Variant>());
		}

		/// <inheritdoc />
		// Token: 0x06002896 RID: 10390 RVA: 0x00116ADD File Offset: 0x00114CDD
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void RaiseGodotClassSignalCallbacks(in godot_string_name signal, NativeVariantPtrArgs args)
		{
			if (!((ref signal) == NMapDrawingInput.SignalName.Finished) || args.Count != 0)
			{
				base.RaiseGodotClassSignalCallbacks(ref signal, args);
				return;
			}
			NMapDrawingInput.FinishedEventHandler finishedEventHandler = this.backing_Finished;
			if (finishedEventHandler == null)
			{
				return;
			}
			finishedEventHandler();
		}

		/// <inheritdoc />
		// Token: 0x06002897 RID: 10391 RVA: 0x00116B0E File Offset: 0x00114D0E
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool HasGodotClassSignal(in godot_string_name signal)
		{
			return (ref signal) == NMapDrawingInput.SignalName.Finished || base.HasGodotClassSignal(ref signal);
		}

		// Token: 0x04000F1E RID: 3870
		[Nullable(1)]
		protected NMapDrawings _drawings;

		// Token: 0x04000F20 RID: 3872
		private NMapDrawingInput.FinishedEventHandler backing_Finished;

		// Token: 0x020014A7 RID: 5287
		// (Invoke) Token: 0x0600A133 RID: 41267
		[Signal]
		public delegate void FinishedEventHandler();

		/// <summary>
		/// Cached StringNames for the methods contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x020014A8 RID: 5288
		public class MethodName : Control.MethodName
		{
			/// <summary>
			/// Cached name for the 'Create' method.
			/// </summary>
			// Token: 0x04004E58 RID: 20056
			public static readonly StringName Create = "Create";

			/// <summary>
			/// Cached name for the '_Ready' method.
			/// </summary>
			// Token: 0x04004E59 RID: 20057
			public static readonly StringName _Ready = "_Ready";

			/// <summary>
			/// Cached name for the '_EnterTree' method.
			/// </summary>
			// Token: 0x04004E5A RID: 20058
			public static readonly StringName _EnterTree = "_EnterTree";

			/// <summary>
			/// Cached name for the '_ExitTree' method.
			/// </summary>
			// Token: 0x04004E5B RID: 20059
			public static readonly StringName _ExitTree = "_ExitTree";

			/// <summary>
			/// Cached name for the 'StopDrawing' method.
			/// </summary>
			// Token: 0x04004E5C RID: 20060
			public static readonly StringName StopDrawing = "StopDrawing";
		}

		/// <summary>
		/// Cached StringNames for the properties and fields contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x020014A9 RID: 5289
		public class PropertyName : Control.PropertyName
		{
			/// <summary>
			/// Cached name for the 'DrawingMode' property.
			/// </summary>
			// Token: 0x04004E5D RID: 20061
			public static readonly StringName DrawingMode = "DrawingMode";

			/// <summary>
			/// Cached name for the '_drawings' field.
			/// </summary>
			// Token: 0x04004E5E RID: 20062
			public static readonly StringName _drawings = "_drawings";
		}

		/// <summary>
		/// Cached StringNames for the signals contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x020014AA RID: 5290
		public class SignalName : Control.SignalName
		{
			/// <summary>
			/// Cached name for the 'Finished' signal.
			/// </summary>
			// Token: 0x04004E5F RID: 20063
			public static readonly StringName Finished = "Finished";
		}
	}
}
