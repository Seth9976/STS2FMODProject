using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Godot;
using Godot.Bridge;
using Godot.NativeInterop;
using MegaCrit.Sts2.Core.Nodes.Screens.ScreenContext;

namespace MegaCrit.Sts2.Core.Nodes.Screens.MainMenu
{
	/// <summary>
	/// Manages submenus in the main menu.
	/// Also manages specific submenus while in-game (the compendium stack).
	/// </summary>
	// Token: 0x02000315 RID: 789
	[ScriptPath("res://src/Core/Nodes/Screens/MainMenu/NSubmenuStack.cs")]
	public abstract class NSubmenuStack : Control
	{
		// Token: 0x17000751 RID: 1873
		// (get) Token: 0x06002BC5 RID: 11205 RVA: 0x00135C24 File Offset: 0x00133E24
		public bool SubmenusOpen
		{
			get
			{
				return this._submenus.Count > 0;
			}
		}

		// Token: 0x06002BC6 RID: 11206 RVA: 0x00135C34 File Offset: 0x00133E34
		[NullableContext(1)]
		public void InitializeForMainMenu(NMainMenu mainMenu)
		{
			this._mainMenu = mainMenu;
		}

		// Token: 0x06002BC7 RID: 11207
		[NullableContext(1)]
		public abstract T PushSubmenuType<[Nullable(0)] T>() where T : NSubmenu;

		// Token: 0x06002BC8 RID: 11208
		[NullableContext(1)]
		public abstract T GetSubmenuType<[Nullable(0)] T>() where T : NSubmenu;

		// Token: 0x06002BC9 RID: 11209
		[NullableContext(1)]
		public abstract NSubmenu PushSubmenuType(Type type);

		// Token: 0x06002BCA RID: 11210
		[NullableContext(1)]
		public abstract NSubmenu GetSubmenuType(Type type);

		// Token: 0x06002BCB RID: 11211 RVA: 0x00135C40 File Offset: 0x00133E40
		[NullableContext(1)]
		public void Push(NSubmenu screen)
		{
			if (this._submenus.Count > 0)
			{
				NSubmenu nsubmenu = this._submenus.Peek();
				nsubmenu.Visible = false;
				nsubmenu.MouseFilter = 2L;
			}
			screen.SetStack(this);
			this._submenus.Push(screen);
			screen.OnSubmenuOpened();
			screen.Visible = true;
			screen.MouseFilter = 0L;
			NMainMenu mainMenu = this._mainMenu;
			if (mainMenu != null)
			{
				mainMenu.EnableBackstop();
			}
			ActiveScreenContext.Instance.Update();
			base.EmitSignal(NSubmenuStack.SignalName.StackModified, Array.Empty<Variant>());
		}

		// Token: 0x06002BCC RID: 11212 RVA: 0x00135CCC File Offset: 0x00133ECC
		public void Pop()
		{
			NSubmenu nsubmenu = this._submenus.Pop();
			nsubmenu.Visible = false;
			nsubmenu.MouseFilter = 2L;
			nsubmenu.OnSubmenuClosed();
			if (this._submenus.Count > 0)
			{
				NSubmenu nsubmenu2 = this._submenus.Peek();
				nsubmenu2.Visible = true;
				nsubmenu2.MouseFilter = 0L;
			}
			else
			{
				this.HideBackstop();
			}
			ActiveScreenContext.Instance.Update();
			base.EmitSignal(NSubmenuStack.SignalName.StackModified, Array.Empty<Variant>());
		}

		// Token: 0x06002BCD RID: 11213 RVA: 0x00135D46 File Offset: 0x00133F46
		private void ShowBackstop()
		{
			NMainMenu mainMenu = this._mainMenu;
			if (mainMenu == null)
			{
				return;
			}
			mainMenu.EnableBackstop();
		}

		// Token: 0x06002BCE RID: 11214 RVA: 0x00135D58 File Offset: 0x00133F58
		private void HideBackstop()
		{
			NMainMenu mainMenu = this._mainMenu;
			if (mainMenu == null)
			{
				return;
			}
			mainMenu.DisableBackstop();
		}

		// Token: 0x06002BCF RID: 11215 RVA: 0x00135D6C File Offset: 0x00133F6C
		[NullableContext(2)]
		public NSubmenu Peek()
		{
			NSubmenu nsubmenu;
			if (!this._submenus.TryPeek(out nsubmenu))
			{
				return null;
			}
			return nsubmenu;
		}

		/// <summary>
		/// Get the method information for all the methods declared in this class.
		/// This method is used by Godot to register the available methods in the editor.
		/// Do not call this method.
		/// </summary>
		// Token: 0x06002BD0 RID: 11216 RVA: 0x00135D8C File Offset: 0x00133F8C
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal static List<MethodInfo> GetGodotMethodList()
		{
			return new List<MethodInfo>(6)
			{
				new MethodInfo(NSubmenuStack.MethodName.InitializeForMainMenu, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, new List<PropertyInfo>
				{
					new PropertyInfo(24L, "mainMenu", 0L, "", 6L, new StringName("Control"), false)
				}, null),
				new MethodInfo(NSubmenuStack.MethodName.Push, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, new List<PropertyInfo>
				{
					new PropertyInfo(24L, "screen", 0L, "", 6L, new StringName("Control"), false)
				}, null),
				new MethodInfo(NSubmenuStack.MethodName.Pop, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NSubmenuStack.MethodName.ShowBackstop, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NSubmenuStack.MethodName.HideBackstop, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NSubmenuStack.MethodName.Peek, new PropertyInfo(24L, "", 0L, "", 6L, new StringName("Control"), false), 1L, null, null)
			};
		}

		/// <inheritdoc />
		// Token: 0x06002BD1 RID: 11217 RVA: 0x00135F28 File Offset: 0x00134128
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool InvokeGodotClassMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
		{
			if ((ref method) == NSubmenuStack.MethodName.InitializeForMainMenu && args.Count == 1)
			{
				this.InitializeForMainMenu(VariantUtils.ConvertTo<NMainMenu>(args[0]));
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NSubmenuStack.MethodName.Push && args.Count == 1)
			{
				this.Push(VariantUtils.ConvertTo<NSubmenu>(args[0]));
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NSubmenuStack.MethodName.Pop && args.Count == 0)
			{
				this.Pop();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NSubmenuStack.MethodName.ShowBackstop && args.Count == 0)
			{
				this.ShowBackstop();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NSubmenuStack.MethodName.HideBackstop && args.Count == 0)
			{
				this.HideBackstop();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NSubmenuStack.MethodName.Peek && args.Count == 0)
			{
				NSubmenu nsubmenu = this.Peek();
				ret = VariantUtils.CreateFrom<NSubmenu>(ref nsubmenu);
				return true;
			}
			return base.InvokeGodotClassMethod(ref method, args, ref ret);
		}

		/// <inheritdoc />
		// Token: 0x06002BD2 RID: 11218 RVA: 0x00136040 File Offset: 0x00134240
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool HasGodotClassMethod(in godot_string_name method)
		{
			return (ref method) == NSubmenuStack.MethodName.InitializeForMainMenu || (ref method) == NSubmenuStack.MethodName.Push || (ref method) == NSubmenuStack.MethodName.Pop || (ref method) == NSubmenuStack.MethodName.ShowBackstop || (ref method) == NSubmenuStack.MethodName.HideBackstop || (ref method) == NSubmenuStack.MethodName.Peek || base.HasGodotClassMethod(ref method);
		}

		/// <inheritdoc />
		// Token: 0x06002BD3 RID: 11219 RVA: 0x001360AE File Offset: 0x001342AE
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool SetGodotClassPropertyValue(in godot_string_name name, in godot_variant value)
		{
			if ((ref name) == NSubmenuStack.PropertyName._mainMenu)
			{
				this._mainMenu = VariantUtils.ConvertTo<NMainMenu>(ref value);
				return true;
			}
			return base.SetGodotClassPropertyValue(ref name, ref value);
		}

		/// <inheritdoc />
		// Token: 0x06002BD4 RID: 11220 RVA: 0x001360D4 File Offset: 0x001342D4
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
		{
			if ((ref name) == NSubmenuStack.PropertyName.SubmenusOpen)
			{
				bool submenusOpen = this.SubmenusOpen;
				value = VariantUtils.CreateFrom<bool>(ref submenusOpen);
				return true;
			}
			if ((ref name) == NSubmenuStack.PropertyName._mainMenu)
			{
				value = VariantUtils.CreateFrom<NMainMenu>(ref this._mainMenu);
				return true;
			}
			return base.GetGodotClassPropertyValue(ref name, ref value);
		}

		/// <summary>
		/// Get the property information for all the properties declared in this class.
		/// This method is used by Godot to register the available properties in the editor.
		/// Do not call this method.
		/// </summary>
		// Token: 0x06002BD5 RID: 11221 RVA: 0x0013612C File Offset: 0x0013432C
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal static List<PropertyInfo> GetGodotPropertyList()
		{
			return new List<PropertyInfo>
			{
				new PropertyInfo(24L, NSubmenuStack.PropertyName._mainMenu, 0L, "", 4096L, false),
				new PropertyInfo(1L, NSubmenuStack.PropertyName.SubmenusOpen, 0L, "", 4096L, false)
			};
		}

		/// <inheritdoc />
		// Token: 0x06002BD6 RID: 11222 RVA: 0x00136181 File Offset: 0x00134381
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void SaveGodotObjectData(GodotSerializationInfo info)
		{
			base.SaveGodotObjectData(info);
			info.AddProperty(NSubmenuStack.PropertyName._mainMenu, Variant.From<NMainMenu>(ref this._mainMenu));
			info.AddSignalEventDelegate(NSubmenuStack.SignalName.StackModified, this.backing_StackModified);
		}

		/// <inheritdoc />
		// Token: 0x06002BD7 RID: 11223 RVA: 0x001361B4 File Offset: 0x001343B4
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void RestoreGodotObjectData(GodotSerializationInfo info)
		{
			base.RestoreGodotObjectData(info);
			Variant variant;
			if (info.TryGetProperty(NSubmenuStack.PropertyName._mainMenu, ref variant))
			{
				this._mainMenu = variant.As<NMainMenu>();
			}
			NSubmenuStack.StackModifiedEventHandler stackModifiedEventHandler;
			if (info.TryGetSignalEventDelegate<NSubmenuStack.StackModifiedEventHandler>(NSubmenuStack.SignalName.StackModified, ref stackModifiedEventHandler))
			{
				this.backing_StackModified = stackModifiedEventHandler;
			}
		}

		/// <summary>
		/// Get the signal information for all the signals declared in this class.
		/// This method is used by Godot to register the available signals in the editor.
		/// Do not call this method.
		/// </summary>
		// Token: 0x06002BD8 RID: 11224 RVA: 0x001361FC File Offset: 0x001343FC
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal static List<MethodInfo> GetGodotSignalList()
		{
			return new List<MethodInfo>(1)
			{
				new MethodInfo(NSubmenuStack.SignalName.StackModified, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null)
			};
		}

		/// <inheritdoc cref="T:MegaCrit.Sts2.Core.Nodes.Screens.MainMenu.NSubmenuStack.StackModifiedEventHandler" />
		// Token: 0x14000026 RID: 38
		// (add) Token: 0x06002BD9 RID: 11225 RVA: 0x00136240 File Offset: 0x00134440
		// (remove) Token: 0x06002BDA RID: 11226 RVA: 0x00136259 File Offset: 0x00134459
		public event NSubmenuStack.StackModifiedEventHandler StackModified
		{
			add
			{
				this.backing_StackModified = (NSubmenuStack.StackModifiedEventHandler)Delegate.Combine(this.backing_StackModified, value);
			}
			remove
			{
				this.backing_StackModified = (NSubmenuStack.StackModifiedEventHandler)Delegate.Remove(this.backing_StackModified, value);
			}
		}

		// Token: 0x06002BDB RID: 11227 RVA: 0x00136272 File Offset: 0x00134472
		protected void EmitSignalStackModified()
		{
			base.EmitSignal(NSubmenuStack.SignalName.StackModified, Array.Empty<Variant>());
		}

		/// <inheritdoc />
		// Token: 0x06002BDC RID: 11228 RVA: 0x00136285 File Offset: 0x00134485
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void RaiseGodotClassSignalCallbacks(in godot_string_name signal, NativeVariantPtrArgs args)
		{
			if (!((ref signal) == NSubmenuStack.SignalName.StackModified) || args.Count != 0)
			{
				base.RaiseGodotClassSignalCallbacks(ref signal, args);
				return;
			}
			NSubmenuStack.StackModifiedEventHandler stackModifiedEventHandler = this.backing_StackModified;
			if (stackModifiedEventHandler == null)
			{
				return;
			}
			stackModifiedEventHandler();
		}

		/// <inheritdoc />
		// Token: 0x06002BDD RID: 11229 RVA: 0x001362B6 File Offset: 0x001344B6
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool HasGodotClassSignal(in godot_string_name signal)
		{
			return (ref signal) == NSubmenuStack.SignalName.StackModified || base.HasGodotClassSignal(ref signal);
		}

		// Token: 0x04001076 RID: 4214
		[Nullable(1)]
		private readonly Stack<NSubmenu> _submenus = new Stack<NSubmenu>();

		// Token: 0x04001077 RID: 4215
		[Nullable(2)]
		private NMainMenu _mainMenu;

		// Token: 0x04001078 RID: 4216
		private NSubmenuStack.StackModifiedEventHandler backing_StackModified;

		// Token: 0x02001539 RID: 5433
		// (Invoke) Token: 0x0600A24E RID: 41550
		[Signal]
		public delegate void StackModifiedEventHandler();

		/// <summary>
		/// Cached StringNames for the methods contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x0200153A RID: 5434
		public class MethodName : Control.MethodName
		{
			/// <summary>
			/// Cached name for the 'InitializeForMainMenu' method.
			/// </summary>
			// Token: 0x0400512C RID: 20780
			public static readonly StringName InitializeForMainMenu = "InitializeForMainMenu";

			/// <summary>
			/// Cached name for the 'Push' method.
			/// </summary>
			// Token: 0x0400512D RID: 20781
			public static readonly StringName Push = "Push";

			/// <summary>
			/// Cached name for the 'Pop' method.
			/// </summary>
			// Token: 0x0400512E RID: 20782
			public static readonly StringName Pop = "Pop";

			/// <summary>
			/// Cached name for the 'ShowBackstop' method.
			/// </summary>
			// Token: 0x0400512F RID: 20783
			public static readonly StringName ShowBackstop = "ShowBackstop";

			/// <summary>
			/// Cached name for the 'HideBackstop' method.
			/// </summary>
			// Token: 0x04005130 RID: 20784
			public static readonly StringName HideBackstop = "HideBackstop";

			/// <summary>
			/// Cached name for the 'Peek' method.
			/// </summary>
			// Token: 0x04005131 RID: 20785
			public static readonly StringName Peek = "Peek";
		}

		/// <summary>
		/// Cached StringNames for the properties and fields contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x0200153B RID: 5435
		public class PropertyName : Control.PropertyName
		{
			/// <summary>
			/// Cached name for the 'SubmenusOpen' property.
			/// </summary>
			// Token: 0x04005132 RID: 20786
			public static readonly StringName SubmenusOpen = "SubmenusOpen";

			/// <summary>
			/// Cached name for the '_mainMenu' field.
			/// </summary>
			// Token: 0x04005133 RID: 20787
			public static readonly StringName _mainMenu = "_mainMenu";
		}

		/// <summary>
		/// Cached StringNames for the signals contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x0200153C RID: 5436
		public class SignalName : Control.SignalName
		{
			/// <summary>
			/// Cached name for the 'StackModified' signal.
			/// </summary>
			// Token: 0x04005134 RID: 20788
			public static readonly StringName StackModified = "StackModified";
		}
	}
}
