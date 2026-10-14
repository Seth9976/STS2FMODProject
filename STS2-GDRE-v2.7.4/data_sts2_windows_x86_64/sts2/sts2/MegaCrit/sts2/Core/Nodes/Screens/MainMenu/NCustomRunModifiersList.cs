using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using Godot;
using Godot.Bridge;
using Godot.NativeInterop;
using MegaCrit.Sts2.Core.Entities.UI;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Nodes.CommonUi;
using MegaCrit.Sts2.Core.Nodes.Screens.CustomRun;

namespace MegaCrit.Sts2.Core.Nodes.Screens.MainMenu
{
	// Token: 0x020002FF RID: 767
	[ScriptPath("res://src/Core/Nodes/Screens/MainMenu/NCustomRunModifiersList.cs")]
	public class NCustomRunModifiersList : Control
	{
		// Token: 0x060029F9 RID: 10745 RVA: 0x00125AF4 File Offset: 0x00123CF4
		public override void _Ready()
		{
			this._container = base.GetNode<Control>("ScrollContainer/Mask/Content");
			foreach (Node node in this._container.GetChildren(false))
			{
				node.QueueFreeSafely();
			}
			foreach (ModifierModel modifierModel in this.GetAllModifiers())
			{
				NRunModifierTickbox nrunModifierTickbox = NRunModifierTickbox.Create(modifierModel);
				this._container.AddChildSafely(nrunModifierTickbox);
				this._modifierTickboxes.Add(nrunModifierTickbox);
				nrunModifierTickbox.Connect(NTickbox.SignalName.Toggled, Callable.From<NRunModifierTickbox>(new Action<NRunModifierTickbox>(this.AfterModifiersChanged)), 0U);
			}
		}

		/// <summary>
		/// Call this once when the menu opens with the appropriate mode for the screen we're displayed on.
		/// </summary>
		/// <param name="mode">See the documentation for the enumeration for the correct value to pass.</param>
		// Token: 0x060029FA RID: 10746 RVA: 0x00125BD4 File Offset: 0x00123DD4
		public void Initialize(MultiplayerUiMode mode)
		{
			this._mode = mode;
			bool flag = mode - MultiplayerUiMode.Client <= 1;
			if (flag)
			{
				foreach (NRunModifierTickbox nrunModifierTickbox in this._modifierTickboxes)
				{
					nrunModifierTickbox.Disable();
				}
			}
		}

		/// <summary>
		/// Call this when:
		/// - We are in client mode and the host changes the modifiers that are selected
		/// - Once when the game is loaded from a multiplayer save
		/// </summary>
		/// <param name="modifiers">The modifiers to display as checked.</param>
		// Token: 0x060029FB RID: 10747 RVA: 0x00125C3C File Offset: 0x00123E3C
		[NullableContext(1)]
		public void SyncModifierList(IReadOnlyCollection<ModifierModel> modifiers)
		{
			MultiplayerUiMode mode = this._mode;
			bool flag = mode - MultiplayerUiMode.Singleplayer <= 1;
			if (flag)
			{
				throw new InvalidOperationException("This should only be called in client or load mode!");
			}
			using (List<NRunModifierTickbox>.Enumerator enumerator = this._modifierTickboxes.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					NRunModifierTickbox tickbox = enumerator.Current;
					tickbox.IsTicked = modifiers.FirstOrDefault<ModifierModel>((ModifierModel m) => m.IsEquivalent(tickbox.Modifier)) != null;
				}
			}
		}

		/// <summary>
		/// Ticks on exactly the given modifiers, unticking all others, then notifies listeners so the change is
		/// propagated to the lobby (and synced on to any clients).
		/// Only valid in host or singleplayer mode. Clients receive modifier changes through <see cref="M:MegaCrit.Sts2.Core.Nodes.Screens.MainMenu.NCustomRunModifiersList.SyncModifierList(System.Collections.Generic.IReadOnlyCollection{MegaCrit.Sts2.Core.Models.ModifierModel})" />.
		/// </summary>
		/// <param name="modifiers">The modifiers to display as ticked. Assumed to already respect mutual exclusivity.</param>
		// Token: 0x060029FC RID: 10748 RVA: 0x00125CD4 File Offset: 0x00123ED4
		[NullableContext(1)]
		public void SetTickedModifiers(IReadOnlyCollection<ModifierModel> modifiers)
		{
			MultiplayerUiMode mode = this._mode;
			bool flag = mode - MultiplayerUiMode.Client <= 1;
			if (flag)
			{
				throw new InvalidOperationException("This should only be called in host or singleplayer mode!");
			}
			using (List<NRunModifierTickbox>.Enumerator enumerator = this._modifierTickboxes.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					NRunModifierTickbox tickbox = enumerator.Current;
					tickbox.IsTicked = modifiers.Any<ModifierModel>((ModifierModel m) => m.IsEquivalent(tickbox.Modifier));
				}
			}
			base.EmitSignal(NCustomRunModifiersList.SignalName.ModifiersChanged, Array.Empty<Variant>());
		}

		// Token: 0x060029FD RID: 10749 RVA: 0x00125D7C File Offset: 0x00123F7C
		[NullableContext(1)]
		private IEnumerable<ModifierModel> GetAllModifiers()
		{
			return new NCustomRunModifiersList.<GetAllModifiers>d__8(-2);
		}

		// Token: 0x060029FE RID: 10750 RVA: 0x00125D88 File Offset: 0x00123F88
		[NullableContext(1)]
		private void UntickMutuallyExclusiveModifiersForTickbox(NRunModifierTickbox tickbox)
		{
			if (!tickbox.IsTicked)
			{
				return;
			}
			Func<ModifierModel, bool> <>9__1;
			IReadOnlySet<ModifierModel> readOnlySet = ModelDb.MutuallyExclusiveModifiers.FirstOrDefault<IReadOnlySet<ModifierModel>>(delegate(IReadOnlySet<ModifierModel> s)
			{
				Func<ModifierModel, bool> func;
				if ((func = <>9__1) == null)
				{
					func = (<>9__1 = (ModifierModel m) => m.GetType() == tickbox.Modifier.GetType());
				}
				return s.Any<ModifierModel>(func);
			});
			if (readOnlySet == null)
			{
				return;
			}
			using (List<NRunModifierTickbox>.Enumerator enumerator = this._modifierTickboxes.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					NRunModifierTickbox otherTickbox = enumerator.Current;
					if (!(otherTickbox.Modifier.GetType() == tickbox.Modifier.GetType()) && readOnlySet.Any<ModifierModel>((ModifierModel m) => m.GetType() == otherTickbox.Modifier.GetType()))
					{
						otherTickbox.IsTicked = false;
					}
				}
			}
		}

		// Token: 0x060029FF RID: 10751 RVA: 0x00125E5C File Offset: 0x0012405C
		[NullableContext(1)]
		private void AfterModifiersChanged(NRunModifierTickbox tickbox)
		{
			this.UntickMutuallyExclusiveModifiersForTickbox(tickbox);
			base.EmitSignal(NCustomRunModifiersList.SignalName.ModifiersChanged, Array.Empty<Variant>());
		}

		/// <summary>
		/// Returns the list of modifiers that are ticked on.
		/// </summary>
		// Token: 0x06002A00 RID: 10752 RVA: 0x00125E78 File Offset: 0x00124078
		[NullableContext(1)]
		public List<ModifierModel> GetModifiersTickedOn()
		{
			return (from t in this._modifierTickboxes
				where t.IsTicked
				select t.Modifier).ToList<ModifierModel>();
		}

		// Token: 0x17000736 RID: 1846
		// (get) Token: 0x06002A01 RID: 10753 RVA: 0x00125ED8 File Offset: 0x001240D8
		[Nullable(2)]
		public Control DefaultFocusedControl
		{
			[NullableContext(2)]
			get
			{
				return this._modifierTickboxes.FirstOrDefault<NRunModifierTickbox>();
			}
		}

		/// <summary>
		/// Get the method information for all the methods declared in this class.
		/// This method is used by Godot to register the available methods in the editor.
		/// Do not call this method.
		/// </summary>
		// Token: 0x06002A02 RID: 10754 RVA: 0x00125EE8 File Offset: 0x001240E8
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal static List<MethodInfo> GetGodotMethodList()
		{
			return new List<MethodInfo>(4)
			{
				new MethodInfo(NCustomRunModifiersList.MethodName._Ready, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NCustomRunModifiersList.MethodName.Initialize, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, new List<PropertyInfo>
				{
					new PropertyInfo(2L, "mode", 0L, "", 6L, false)
				}, null),
				new MethodInfo(NCustomRunModifiersList.MethodName.UntickMutuallyExclusiveModifiersForTickbox, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, new List<PropertyInfo>
				{
					new PropertyInfo(24L, "tickbox", 0L, "", 6L, new StringName("Control"), false)
				}, null),
				new MethodInfo(NCustomRunModifiersList.MethodName.AfterModifiersChanged, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, new List<PropertyInfo>
				{
					new PropertyInfo(24L, "tickbox", 0L, "", 6L, new StringName("Control"), false)
				}, null)
			};
		}

		/// <inheritdoc />
		// Token: 0x06002A03 RID: 10755 RVA: 0x00126040 File Offset: 0x00124240
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool InvokeGodotClassMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
		{
			if ((ref method) == NCustomRunModifiersList.MethodName._Ready && args.Count == 0)
			{
				this._Ready();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NCustomRunModifiersList.MethodName.Initialize && args.Count == 1)
			{
				this.Initialize(VariantUtils.ConvertTo<MultiplayerUiMode>(args[0]));
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NCustomRunModifiersList.MethodName.UntickMutuallyExclusiveModifiersForTickbox && args.Count == 1)
			{
				this.UntickMutuallyExclusiveModifiersForTickbox(VariantUtils.ConvertTo<NRunModifierTickbox>(args[0]));
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NCustomRunModifiersList.MethodName.AfterModifiersChanged && args.Count == 1)
			{
				this.AfterModifiersChanged(VariantUtils.ConvertTo<NRunModifierTickbox>(args[0]));
				ret = default(godot_variant);
				return true;
			}
			return base.InvokeGodotClassMethod(ref method, args, ref ret);
		}

		/// <inheritdoc />
		// Token: 0x06002A04 RID: 10756 RVA: 0x00126114 File Offset: 0x00124314
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool HasGodotClassMethod(in godot_string_name method)
		{
			return (ref method) == NCustomRunModifiersList.MethodName._Ready || (ref method) == NCustomRunModifiersList.MethodName.Initialize || (ref method) == NCustomRunModifiersList.MethodName.UntickMutuallyExclusiveModifiersForTickbox || (ref method) == NCustomRunModifiersList.MethodName.AfterModifiersChanged || base.HasGodotClassMethod(ref method);
		}

		/// <inheritdoc />
		// Token: 0x06002A05 RID: 10757 RVA: 0x00126164 File Offset: 0x00124364
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool SetGodotClassPropertyValue(in godot_string_name name, in godot_variant value)
		{
			if ((ref name) == NCustomRunModifiersList.PropertyName._container)
			{
				this._container = VariantUtils.ConvertTo<Control>(ref value);
				return true;
			}
			if ((ref name) == NCustomRunModifiersList.PropertyName._mode)
			{
				this._mode = VariantUtils.ConvertTo<MultiplayerUiMode>(ref value);
				return true;
			}
			return base.SetGodotClassPropertyValue(ref name, ref value);
		}

		/// <inheritdoc />
		// Token: 0x06002A06 RID: 10758 RVA: 0x001261A4 File Offset: 0x001243A4
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
		{
			if ((ref name) == NCustomRunModifiersList.PropertyName.DefaultFocusedControl)
			{
				Control defaultFocusedControl = this.DefaultFocusedControl;
				value = VariantUtils.CreateFrom<Control>(ref defaultFocusedControl);
				return true;
			}
			if ((ref name) == NCustomRunModifiersList.PropertyName._container)
			{
				value = VariantUtils.CreateFrom<Control>(ref this._container);
				return true;
			}
			if ((ref name) == NCustomRunModifiersList.PropertyName._mode)
			{
				value = VariantUtils.CreateFrom<MultiplayerUiMode>(ref this._mode);
				return true;
			}
			return base.GetGodotClassPropertyValue(ref name, ref value);
		}

		/// <summary>
		/// Get the property information for all the properties declared in this class.
		/// This method is used by Godot to register the available properties in the editor.
		/// Do not call this method.
		/// </summary>
		// Token: 0x06002A07 RID: 10759 RVA: 0x0012621C File Offset: 0x0012441C
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal static List<PropertyInfo> GetGodotPropertyList()
		{
			return new List<PropertyInfo>
			{
				new PropertyInfo(24L, NCustomRunModifiersList.PropertyName._container, 0L, "", 4096L, false),
				new PropertyInfo(2L, NCustomRunModifiersList.PropertyName._mode, 0L, "", 4096L, false),
				new PropertyInfo(24L, NCustomRunModifiersList.PropertyName.DefaultFocusedControl, 0L, "", 4096L, false)
			};
		}

		/// <inheritdoc />
		// Token: 0x06002A08 RID: 10760 RVA: 0x00126294 File Offset: 0x00124494
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void SaveGodotObjectData(GodotSerializationInfo info)
		{
			base.SaveGodotObjectData(info);
			info.AddProperty(NCustomRunModifiersList.PropertyName._container, Variant.From<Control>(ref this._container));
			info.AddProperty(NCustomRunModifiersList.PropertyName._mode, Variant.From<MultiplayerUiMode>(ref this._mode));
			info.AddSignalEventDelegate(NCustomRunModifiersList.SignalName.ModifiersChanged, this.backing_ModifiersChanged);
		}

		/// <inheritdoc />
		// Token: 0x06002A09 RID: 10761 RVA: 0x001262E8 File Offset: 0x001244E8
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void RestoreGodotObjectData(GodotSerializationInfo info)
		{
			base.RestoreGodotObjectData(info);
			Variant variant;
			if (info.TryGetProperty(NCustomRunModifiersList.PropertyName._container, ref variant))
			{
				this._container = variant.As<Control>();
			}
			Variant variant2;
			if (info.TryGetProperty(NCustomRunModifiersList.PropertyName._mode, ref variant2))
			{
				this._mode = variant2.As<MultiplayerUiMode>();
			}
			NCustomRunModifiersList.ModifiersChangedEventHandler modifiersChangedEventHandler;
			if (info.TryGetSignalEventDelegate<NCustomRunModifiersList.ModifiersChangedEventHandler>(NCustomRunModifiersList.SignalName.ModifiersChanged, ref modifiersChangedEventHandler))
			{
				this.backing_ModifiersChanged = modifiersChangedEventHandler;
			}
		}

		/// <summary>
		/// Get the signal information for all the signals declared in this class.
		/// This method is used by Godot to register the available signals in the editor.
		/// Do not call this method.
		/// </summary>
		// Token: 0x06002A0A RID: 10762 RVA: 0x0012634C File Offset: 0x0012454C
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal static List<MethodInfo> GetGodotSignalList()
		{
			return new List<MethodInfo>(1)
			{
				new MethodInfo(NCustomRunModifiersList.SignalName.ModifiersChanged, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null)
			};
		}

		/// <inheritdoc cref="T:MegaCrit.Sts2.Core.Nodes.Screens.MainMenu.NCustomRunModifiersList.ModifiersChangedEventHandler" />
		// Token: 0x14000025 RID: 37
		// (add) Token: 0x06002A0B RID: 10763 RVA: 0x00126390 File Offset: 0x00124590
		// (remove) Token: 0x06002A0C RID: 10764 RVA: 0x001263A9 File Offset: 0x001245A9
		public event NCustomRunModifiersList.ModifiersChangedEventHandler ModifiersChanged
		{
			add
			{
				this.backing_ModifiersChanged = (NCustomRunModifiersList.ModifiersChangedEventHandler)Delegate.Combine(this.backing_ModifiersChanged, value);
			}
			remove
			{
				this.backing_ModifiersChanged = (NCustomRunModifiersList.ModifiersChangedEventHandler)Delegate.Remove(this.backing_ModifiersChanged, value);
			}
		}

		// Token: 0x06002A0D RID: 10765 RVA: 0x001263C2 File Offset: 0x001245C2
		protected void EmitSignalModifiersChanged()
		{
			base.EmitSignal(NCustomRunModifiersList.SignalName.ModifiersChanged, Array.Empty<Variant>());
		}

		/// <inheritdoc />
		// Token: 0x06002A0E RID: 10766 RVA: 0x001263D5 File Offset: 0x001245D5
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void RaiseGodotClassSignalCallbacks(in godot_string_name signal, NativeVariantPtrArgs args)
		{
			if (!((ref signal) == NCustomRunModifiersList.SignalName.ModifiersChanged) || args.Count != 0)
			{
				base.RaiseGodotClassSignalCallbacks(ref signal, args);
				return;
			}
			NCustomRunModifiersList.ModifiersChangedEventHandler modifiersChangedEventHandler = this.backing_ModifiersChanged;
			if (modifiersChangedEventHandler == null)
			{
				return;
			}
			modifiersChangedEventHandler();
		}

		/// <inheritdoc />
		// Token: 0x06002A0F RID: 10767 RVA: 0x00126406 File Offset: 0x00124606
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool HasGodotClassSignal(in godot_string_name signal)
		{
			return (ref signal) == NCustomRunModifiersList.SignalName.ModifiersChanged || base.HasGodotClassSignal(ref signal);
		}

		// Token: 0x04000FB9 RID: 4025
		[Nullable(1)]
		private readonly List<NRunModifierTickbox> _modifierTickboxes = new List<NRunModifierTickbox>();

		// Token: 0x04000FBA RID: 4026
		[Nullable(1)]
		private Control _container;

		// Token: 0x04000FBB RID: 4027
		private MultiplayerUiMode _mode;

		// Token: 0x04000FBC RID: 4028
		private NCustomRunModifiersList.ModifiersChangedEventHandler backing_ModifiersChanged;

		// Token: 0x020014DD RID: 5341
		// (Invoke) Token: 0x0600A19F RID: 41375
		[Signal]
		public delegate void ModifiersChangedEventHandler();

		/// <summary>
		/// Cached StringNames for the methods contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x020014DE RID: 5342
		public class MethodName : Control.MethodName
		{
			/// <summary>
			/// Cached name for the '_Ready' method.
			/// </summary>
			// Token: 0x04004F96 RID: 20374
			public static readonly StringName _Ready = "_Ready";

			/// <summary>
			/// Cached name for the 'Initialize' method.
			/// </summary>
			// Token: 0x04004F97 RID: 20375
			public static readonly StringName Initialize = "Initialize";

			/// <summary>
			/// Cached name for the 'UntickMutuallyExclusiveModifiersForTickbox' method.
			/// </summary>
			// Token: 0x04004F98 RID: 20376
			public static readonly StringName UntickMutuallyExclusiveModifiersForTickbox = "UntickMutuallyExclusiveModifiersForTickbox";

			/// <summary>
			/// Cached name for the 'AfterModifiersChanged' method.
			/// </summary>
			// Token: 0x04004F99 RID: 20377
			public static readonly StringName AfterModifiersChanged = "AfterModifiersChanged";
		}

		/// <summary>
		/// Cached StringNames for the properties and fields contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x020014DF RID: 5343
		public class PropertyName : Control.PropertyName
		{
			/// <summary>
			/// Cached name for the 'DefaultFocusedControl' property.
			/// </summary>
			// Token: 0x04004F9A RID: 20378
			public static readonly StringName DefaultFocusedControl = "DefaultFocusedControl";

			/// <summary>
			/// Cached name for the '_container' field.
			/// </summary>
			// Token: 0x04004F9B RID: 20379
			public static readonly StringName _container = "_container";

			/// <summary>
			/// Cached name for the '_mode' field.
			/// </summary>
			// Token: 0x04004F9C RID: 20380
			public static readonly StringName _mode = "_mode";
		}

		/// <summary>
		/// Cached StringNames for the signals contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x020014E0 RID: 5344
		public class SignalName : Control.SignalName
		{
			/// <summary>
			/// Cached name for the 'ModifiersChanged' signal.
			/// </summary>
			// Token: 0x04004F9D RID: 20381
			public static readonly StringName ModifiersChanged = "ModifiersChanged";
		}
	}
}
