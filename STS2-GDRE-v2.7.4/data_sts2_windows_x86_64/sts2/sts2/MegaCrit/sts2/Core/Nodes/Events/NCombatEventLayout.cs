using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Godot;
using Godot.Bridge;
using Godot.NativeInterop;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Nodes.GodotExtensions;
using MegaCrit.Sts2.Core.Nodes.Rooms;
using MegaCrit.Sts2.Core.Rooms;
using MegaCrit.Sts2.Core.Runs;

namespace MegaCrit.Sts2.Core.Nodes.Events
{
	// Token: 0x020003BB RID: 955
	[ScriptPath("res://src/Core/Nodes/Events/NCombatEventLayout.cs")]
	public class NCombatEventLayout : NEventLayout
	{
		// Token: 0x1700087B RID: 2171
		// (get) Token: 0x06003970 RID: 14704 RVA: 0x001B37A0 File Offset: 0x001B19A0
		// (set) Token: 0x06003971 RID: 14705 RVA: 0x001B37A8 File Offset: 0x001B19A8
		[Nullable(2)]
		public NCombatRoom EmbeddedCombatRoom
		{
			[NullableContext(2)]
			get;
			[NullableContext(2)]
			private set;
		}

		// Token: 0x1700087C RID: 2172
		// (get) Token: 0x06003972 RID: 14706 RVA: 0x001B37B1 File Offset: 0x001B19B1
		// (set) Token: 0x06003973 RID: 14707 RVA: 0x001B37B9 File Offset: 0x001B19B9
		public bool HasCombatStarted { get; private set; }

		// Token: 0x06003974 RID: 14708 RVA: 0x001B37C2 File Offset: 0x001B19C2
		public override void _Ready()
		{
			base._Ready();
			this._combatRoomContainer = base.GetNode<Control>("%CombatRoomContainer");
		}

		// Token: 0x06003975 RID: 14709 RVA: 0x001B37E0 File Offset: 0x001B19E0
		[NullableContext(2)]
		public void SetCombatRoomNode(NCombatRoom combatRoomNode)
		{
			if (combatRoomNode == null)
			{
				return;
			}
			if (this.EmbeddedCombatRoom != null)
			{
				throw new InvalidOperationException("Combat room node was already set.");
			}
			this.EmbeddedCombatRoom = combatRoomNode;
			this._combatRoomContainer.AddChildSafely(combatRoomNode);
		}

		// Token: 0x06003976 RID: 14710 RVA: 0x001B380C File Offset: 0x001B1A0C
		[NullableContext(1)]
		public override void SetEvent(EventModel eventModel)
		{
			IRunState runState = eventModel.Owner.RunState;
			ICombatRoomVisuals combatRoomVisuals = eventModel.CreateCombatRoomVisuals(runState.Players, runState.Act);
			NCombatRoom ncombatRoom = NCombatRoom.Create(combatRoomVisuals, CombatRoomMode.VisualOnly);
			this.SetCombatRoomNode(ncombatRoom);
			if (ncombatRoom != null)
			{
				ncombatRoom.SetUpBackground(runState);
			}
			base.SetEvent(eventModel);
		}

		// Token: 0x06003977 RID: 14711 RVA: 0x001B3858 File Offset: 0x001B1A58
		protected override void InitializeVisuals()
		{
		}

		// Token: 0x06003978 RID: 14712 RVA: 0x001B385C File Offset: 0x001B1A5C
		public void HideEventVisuals()
		{
			if (this._description != null)
			{
				this._description.Visible = false;
			}
			if (this._sharedEventLabel != null)
			{
				this._sharedEventLabel.Visible = false;
			}
			this._optionsContainer.Visible = false;
			this.HasCombatStarted = true;
			Control defaultFocusedControl = this.DefaultFocusedControl;
			if (defaultFocusedControl == null)
			{
				return;
			}
			defaultFocusedControl.TryGrabFocus();
		}

		// Token: 0x1700087D RID: 2173
		// (get) Token: 0x06003979 RID: 14713 RVA: 0x001B38B4 File Offset: 0x001B1AB4
		[Nullable(2)]
		public override Control DefaultFocusedControl
		{
			[NullableContext(2)]
			get
			{
				if (!this.HasCombatStarted)
				{
					return base.DefaultFocusedControl;
				}
				NCombatRoom embeddedCombatRoom = this.EmbeddedCombatRoom;
				if (embeddedCombatRoom == null)
				{
					return null;
				}
				return embeddedCombatRoom.DefaultFocusedControl;
			}
		}

		/// <summary>
		/// Get the method information for all the methods declared in this class.
		/// This method is used by Godot to register the available methods in the editor.
		/// Do not call this method.
		/// </summary>
		// Token: 0x0600397A RID: 14714 RVA: 0x001B38D8 File Offset: 0x001B1AD8
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal new static List<MethodInfo> GetGodotMethodList()
		{
			return new List<MethodInfo>(4)
			{
				new MethodInfo(NCombatEventLayout.MethodName._Ready, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NCombatEventLayout.MethodName.SetCombatRoomNode, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, new List<PropertyInfo>
				{
					new PropertyInfo(24L, "combatRoomNode", 0L, "", 6L, new StringName("Control"), false)
				}, null),
				new MethodInfo(NCombatEventLayout.MethodName.InitializeVisuals, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NCombatEventLayout.MethodName.HideEventVisuals, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null)
			};
		}

		/// <inheritdoc />
		// Token: 0x0600397B RID: 14715 RVA: 0x001B39DC File Offset: 0x001B1BDC
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool InvokeGodotClassMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
		{
			if ((ref method) == NCombatEventLayout.MethodName._Ready && args.Count == 0)
			{
				this._Ready();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NCombatEventLayout.MethodName.SetCombatRoomNode && args.Count == 1)
			{
				this.SetCombatRoomNode(VariantUtils.ConvertTo<NCombatRoom>(args[0]));
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NCombatEventLayout.MethodName.InitializeVisuals && args.Count == 0)
			{
				this.InitializeVisuals();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NCombatEventLayout.MethodName.HideEventVisuals && args.Count == 0)
			{
				this.HideEventVisuals();
				ret = default(godot_variant);
				return true;
			}
			return base.InvokeGodotClassMethod(in method, args, out ret);
		}

		/// <inheritdoc />
		// Token: 0x0600397C RID: 14716 RVA: 0x001B3A94 File Offset: 0x001B1C94
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool HasGodotClassMethod(in godot_string_name method)
		{
			return (ref method) == NCombatEventLayout.MethodName._Ready || (ref method) == NCombatEventLayout.MethodName.SetCombatRoomNode || (ref method) == NCombatEventLayout.MethodName.InitializeVisuals || (ref method) == NCombatEventLayout.MethodName.HideEventVisuals || base.HasGodotClassMethod(in method);
		}

		/// <inheritdoc />
		// Token: 0x0600397D RID: 14717 RVA: 0x001B3AE4 File Offset: 0x001B1CE4
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool SetGodotClassPropertyValue(in godot_string_name name, in godot_variant value)
		{
			if ((ref name) == NCombatEventLayout.PropertyName.EmbeddedCombatRoom)
			{
				this.EmbeddedCombatRoom = VariantUtils.ConvertTo<NCombatRoom>(ref value);
				return true;
			}
			if ((ref name) == NCombatEventLayout.PropertyName.HasCombatStarted)
			{
				this.HasCombatStarted = VariantUtils.ConvertTo<bool>(ref value);
				return true;
			}
			if ((ref name) == NCombatEventLayout.PropertyName._combatRoomContainer)
			{
				this._combatRoomContainer = VariantUtils.ConvertTo<Control>(ref value);
				return true;
			}
			return base.SetGodotClassPropertyValue(in name, in value);
		}

		/// <inheritdoc />
		// Token: 0x0600397E RID: 14718 RVA: 0x001B3B4C File Offset: 0x001B1D4C
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
		{
			if ((ref name) == NCombatEventLayout.PropertyName.EmbeddedCombatRoom)
			{
				NCombatRoom embeddedCombatRoom = this.EmbeddedCombatRoom;
				value = VariantUtils.CreateFrom<NCombatRoom>(ref embeddedCombatRoom);
				return true;
			}
			if ((ref name) == NCombatEventLayout.PropertyName.HasCombatStarted)
			{
				bool hasCombatStarted = this.HasCombatStarted;
				value = VariantUtils.CreateFrom<bool>(ref hasCombatStarted);
				return true;
			}
			if ((ref name) == NCombatEventLayout.PropertyName.DefaultFocusedControl)
			{
				Control defaultFocusedControl = this.DefaultFocusedControl;
				value = VariantUtils.CreateFrom<Control>(ref defaultFocusedControl);
				return true;
			}
			if ((ref name) == NCombatEventLayout.PropertyName._combatRoomContainer)
			{
				value = VariantUtils.CreateFrom<Control>(ref this._combatRoomContainer);
				return true;
			}
			return base.GetGodotClassPropertyValue(in name, out value);
		}

		/// <summary>
		/// Get the property information for all the properties declared in this class.
		/// This method is used by Godot to register the available properties in the editor.
		/// Do not call this method.
		/// </summary>
		// Token: 0x0600397F RID: 14719 RVA: 0x001B3BEC File Offset: 0x001B1DEC
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal new static List<PropertyInfo> GetGodotPropertyList()
		{
			return new List<PropertyInfo>
			{
				new PropertyInfo(24L, NCombatEventLayout.PropertyName._combatRoomContainer, 0L, "", 4096L, false),
				new PropertyInfo(24L, NCombatEventLayout.PropertyName.EmbeddedCombatRoom, 0L, "", 4096L, false),
				new PropertyInfo(1L, NCombatEventLayout.PropertyName.HasCombatStarted, 0L, "", 4096L, false),
				new PropertyInfo(24L, NCombatEventLayout.PropertyName.DefaultFocusedControl, 0L, "", 4096L, false)
			};
		}

		/// <inheritdoc />
		// Token: 0x06003980 RID: 14720 RVA: 0x001B3C84 File Offset: 0x001B1E84
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void SaveGodotObjectData(GodotSerializationInfo info)
		{
			base.SaveGodotObjectData(info);
			StringName embeddedCombatRoom = NCombatEventLayout.PropertyName.EmbeddedCombatRoom;
			NCombatRoom embeddedCombatRoom2 = this.EmbeddedCombatRoom;
			info.AddProperty(embeddedCombatRoom, Variant.From<NCombatRoom>(ref embeddedCombatRoom2));
			StringName hasCombatStarted = NCombatEventLayout.PropertyName.HasCombatStarted;
			bool hasCombatStarted2 = this.HasCombatStarted;
			info.AddProperty(hasCombatStarted, Variant.From<bool>(ref hasCombatStarted2));
			info.AddProperty(NCombatEventLayout.PropertyName._combatRoomContainer, Variant.From<Control>(ref this._combatRoomContainer));
		}

		/// <inheritdoc />
		// Token: 0x06003981 RID: 14721 RVA: 0x001B3CE0 File Offset: 0x001B1EE0
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void RestoreGodotObjectData(GodotSerializationInfo info)
		{
			base.RestoreGodotObjectData(info);
			Variant variant;
			if (info.TryGetProperty(NCombatEventLayout.PropertyName.EmbeddedCombatRoom, ref variant))
			{
				this.EmbeddedCombatRoom = variant.As<NCombatRoom>();
			}
			Variant variant2;
			if (info.TryGetProperty(NCombatEventLayout.PropertyName.HasCombatStarted, ref variant2))
			{
				this.HasCombatStarted = variant2.As<bool>();
			}
			Variant variant3;
			if (info.TryGetProperty(NCombatEventLayout.PropertyName._combatRoomContainer, ref variant3))
			{
				this._combatRoomContainer = variant3.As<Control>();
			}
		}

		// Token: 0x0400165A RID: 5722
		[Nullable(1)]
		public const string combatScenePath = "res://scenes/events/combat_event_layout.tscn";

		// Token: 0x0400165B RID: 5723
		[Nullable(1)]
		private Control _combatRoomContainer;

		/// <summary>
		/// Cached StringNames for the methods contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x020017CB RID: 6091
		public new class MethodName : NEventLayout.MethodName
		{
			/// <summary>
			/// Cached name for the '_Ready' method.
			/// </summary>
			// Token: 0x04005CBC RID: 23740
			public new static readonly StringName _Ready = "_Ready";

			/// <summary>
			/// Cached name for the 'SetCombatRoomNode' method.
			/// </summary>
			// Token: 0x04005CBD RID: 23741
			public static readonly StringName SetCombatRoomNode = "SetCombatRoomNode";

			/// <summary>
			/// Cached name for the 'InitializeVisuals' method.
			/// </summary>
			// Token: 0x04005CBE RID: 23742
			public new static readonly StringName InitializeVisuals = "InitializeVisuals";

			/// <summary>
			/// Cached name for the 'HideEventVisuals' method.
			/// </summary>
			// Token: 0x04005CBF RID: 23743
			public static readonly StringName HideEventVisuals = "HideEventVisuals";
		}

		/// <summary>
		/// Cached StringNames for the properties and fields contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x020017CC RID: 6092
		public new class PropertyName : NEventLayout.PropertyName
		{
			/// <summary>
			/// Cached name for the 'EmbeddedCombatRoom' property.
			/// </summary>
			// Token: 0x04005CC0 RID: 23744
			public static readonly StringName EmbeddedCombatRoom = "EmbeddedCombatRoom";

			/// <summary>
			/// Cached name for the 'HasCombatStarted' property.
			/// </summary>
			// Token: 0x04005CC1 RID: 23745
			public static readonly StringName HasCombatStarted = "HasCombatStarted";

			/// <summary>
			/// Cached name for the 'DefaultFocusedControl' property.
			/// </summary>
			// Token: 0x04005CC2 RID: 23746
			public new static readonly StringName DefaultFocusedControl = "DefaultFocusedControl";

			/// <summary>
			/// Cached name for the '_combatRoomContainer' field.
			/// </summary>
			// Token: 0x04005CC3 RID: 23747
			public static readonly StringName _combatRoomContainer = "_combatRoomContainer";
		}

		/// <summary>
		/// Cached StringNames for the signals contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x020017CD RID: 6093
		public new class SignalName : NEventLayout.SignalName
		{
		}
	}
}
