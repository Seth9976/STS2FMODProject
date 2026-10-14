using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using Godot;
using Godot.Bridge;
using Godot.NativeInterop;
using MegaCrit.Sts2.Core.ControllerInput;
using MegaCrit.Sts2.Core.Nodes.Debug;
using MegaCrit.Sts2.Core.Nodes.GodotExtensions;

namespace MegaCrit.Sts2.Core.Nodes.CommonUi
{
	/// <summary>
	/// Class that manages Hotkeys/shortcuts for various buttons and UI in the game.
	/// Manages which buttons get priority if multiple of them share the same Hotkey
	/// (typically the last one added gets priority)
	/// </summary>
	// Token: 0x020003E1 RID: 993
	[NullableContext(1)]
	[Nullable(0)]
	[ScriptPath("res://src/Core/Nodes/CommonUi/NHotkeyManager.cs")]
	public class NHotkeyManager : Node
	{
		// Token: 0x170008C6 RID: 2246
		// (get) Token: 0x06003C6E RID: 15470 RVA: 0x001CE806 File Offset: 0x001CCA06
		[Nullable(2)]
		public static NHotkeyManager Instance
		{
			[NullableContext(2)]
			get
			{
				if (NGame.Instance == null)
				{
					return null;
				}
				return NGame.Instance.HotkeyManager;
			}
		}

		// Token: 0x06003C6F RID: 15471 RVA: 0x001CE81C File Offset: 0x001CCA1C
		public void PushHotkeyPressedBinding(string hotkey, Action action)
		{
			if (!this._hotkeyPressedBindings.ContainsKey(hotkey))
			{
				this._hotkeyPressedBindings.Add(hotkey, new List<Action>());
			}
			if (!this._hotkeyPressedBindings[hotkey].Contains(action))
			{
				this._hotkeyPressedBindings[hotkey].Add(action);
			}
		}

		// Token: 0x06003C70 RID: 15472 RVA: 0x001CE884 File Offset: 0x001CCA84
		public void RemoveHotkeyPressedBinding(string hotkey, Action action)
		{
			List<Action> list;
			if (!this._hotkeyPressedBindings.TryGetValue(hotkey, out list))
			{
				return;
			}
			list.Remove(action);
			if (this._hotkeyPressedBindings[hotkey].Count == 0)
			{
				this._hotkeyPressedBindings.Remove(hotkey);
			}
		}

		// Token: 0x06003C71 RID: 15473 RVA: 0x001CE8DC File Offset: 0x001CCADC
		public void PushHotkeyReleasedBinding(string hotkey, Action action)
		{
			if (!this._hotkeyReleasedBindings.ContainsKey(hotkey))
			{
				this._hotkeyReleasedBindings.Add(hotkey, new List<Action>());
			}
			if (!this._hotkeyReleasedBindings[hotkey].Contains(action))
			{
				this._hotkeyReleasedBindings[hotkey].Add(action);
			}
		}

		// Token: 0x06003C72 RID: 15474 RVA: 0x001CE944 File Offset: 0x001CCB44
		public void RemoveHotkeyReleasedBinding(string hotkey, Action action)
		{
			List<Action> list;
			if (!this._hotkeyReleasedBindings.TryGetValue(hotkey, out list))
			{
				return;
			}
			list.Remove(action);
			if (this._hotkeyReleasedBindings[hotkey].Count == 0)
			{
				this._hotkeyReleasedBindings.Remove(hotkey);
			}
		}

		/// <summary>
		/// we want to block all previous hotkeys while this screen is open
		/// New hotkeys can be added on top though.
		/// ex: we don't want to be able to hotkey to view our draw pile if the pause menu is open
		/// </summary>
		/// <param name="screen">Screen that is block all previous hotkey input</param>
		// Token: 0x06003C73 RID: 15475 RVA: 0x001CE99C File Offset: 0x001CCB9C
		public void AddBlockingScreen(Node screen)
		{
			Action action = delegate
			{
			};
			foreach (string text in MegaInput.AllInputs)
			{
				this.PushHotkeyPressedBinding(text, action);
			}
			foreach (string text2 in MegaInput.AllInputs)
			{
				this.PushHotkeyReleasedBinding(text2, action);
			}
			this._blockingScreens.Add(screen, action);
		}

		/// <summary>
		/// Removes the all hotkey blockers related to a screen.
		/// </summary>
		/// <param name="screen">Screen that want to remove the hotkey blockage from</param>
		// Token: 0x06003C74 RID: 15476 RVA: 0x001CEA20 File Offset: 0x001CCC20
		public void RemoveBlockingScreen(Node screen)
		{
			Action action;
			if (!this._blockingScreens.TryGetValue(screen, out action))
			{
				return;
			}
			foreach (string text in MegaInput.AllInputs)
			{
				this.RemoveHotkeyPressedBinding(text, action);
			}
			foreach (string text2 in MegaInput.AllInputs)
			{
				this.RemoveHotkeyReleasedBinding(text2, action);
			}
			this._blockingScreens.Remove(screen);
		}

		// Token: 0x06003C75 RID: 15477 RVA: 0x001CEA98 File Offset: 0x001CCC98
		public override void _UnhandledInput(InputEvent inputEvent)
		{
			if (!NGame.IsGameFocusedWindow())
			{
				return;
			}
			if (NDevConsole.IsConsoleVisible)
			{
				return;
			}
			Viewport viewport = base.GetViewport();
			Control control = ((viewport != null) ? viewport.GuiGetFocusOwner() : null);
			if (control != null)
			{
				LineEdit lineEdit = control as LineEdit;
				if (lineEdit != null && lineEdit.IsEditing())
				{
					return;
				}
				NMegaTextEdit nmegaTextEdit = control as NMegaTextEdit;
				if (nmegaTextEdit != null && nmegaTextEdit.IsEditing())
				{
					return;
				}
			}
			foreach (KeyValuePair<StringName, List<Action>> keyValuePair in this._hotkeyPressedBindings)
			{
				if (inputEvent.IsActionPressed(keyValuePair.Key, false, false) && !inputEvent.IsEcho())
				{
					Action action = keyValuePair.Value.LastOrDefault<Action>();
					if (action != null)
					{
						Callable.From(new Action(action.Invoke)).CallDeferred(Array.Empty<Variant>());
						if (viewport != null)
						{
							viewport.SetInputAsHandled();
						}
					}
				}
			}
			foreach (KeyValuePair<StringName, List<Action>> keyValuePair2 in this._hotkeyReleasedBindings)
			{
				if (inputEvent.IsActionReleased(keyValuePair2.Key, false))
				{
					Action action2 = keyValuePair2.Value.LastOrDefault<Action>();
					if (action2 != null)
					{
						Callable.From(new Action(action2.Invoke)).CallDeferred(Array.Empty<Variant>());
						if (viewport != null)
						{
							viewport.SetInputAsHandled();
						}
					}
				}
			}
		}

		/// <summary>
		/// Get the method information for all the methods declared in this class.
		/// This method is used by Godot to register the available methods in the editor.
		/// Do not call this method.
		/// </summary>
		// Token: 0x06003C76 RID: 15478 RVA: 0x001CEC10 File Offset: 0x001CCE10
		[NullableContext(0)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal static List<MethodInfo> GetGodotMethodList()
		{
			return new List<MethodInfo>(3)
			{
				new MethodInfo(NHotkeyManager.MethodName.AddBlockingScreen, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, new List<PropertyInfo>
				{
					new PropertyInfo(24L, "screen", 0L, "", 6L, new StringName("Node"), false)
				}, null),
				new MethodInfo(NHotkeyManager.MethodName.RemoveBlockingScreen, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, new List<PropertyInfo>
				{
					new PropertyInfo(24L, "screen", 0L, "", 6L, new StringName("Node"), false)
				}, null),
				new MethodInfo(NHotkeyManager.MethodName._UnhandledInput, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, new List<PropertyInfo>
				{
					new PropertyInfo(24L, "inputEvent", 0L, "", 6L, new StringName("InputEvent"), false)
				}, null)
			};
		}

		/// <inheritdoc />
		// Token: 0x06003C77 RID: 15479 RVA: 0x001CED44 File Offset: 0x001CCF44
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool InvokeGodotClassMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
		{
			if ((ref method) == NHotkeyManager.MethodName.AddBlockingScreen && args.Count == 1)
			{
				this.AddBlockingScreen(VariantUtils.ConvertTo<Node>(args[0]));
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NHotkeyManager.MethodName.RemoveBlockingScreen && args.Count == 1)
			{
				this.RemoveBlockingScreen(VariantUtils.ConvertTo<Node>(args[0]));
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NHotkeyManager.MethodName._UnhandledInput && args.Count == 1)
			{
				this._UnhandledInput(VariantUtils.ConvertTo<InputEvent>(args[0]));
				ret = default(godot_variant);
				return true;
			}
			return base.InvokeGodotClassMethod(ref method, args, ref ret);
		}

		/// <inheritdoc />
		// Token: 0x06003C78 RID: 15480 RVA: 0x001CEDF3 File Offset: 0x001CCFF3
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool HasGodotClassMethod(in godot_string_name method)
		{
			return (ref method) == NHotkeyManager.MethodName.AddBlockingScreen || (ref method) == NHotkeyManager.MethodName.RemoveBlockingScreen || (ref method) == NHotkeyManager.MethodName._UnhandledInput || base.HasGodotClassMethod(ref method);
		}

		/// <inheritdoc />
		// Token: 0x06003C79 RID: 15481 RVA: 0x001CEE29 File Offset: 0x001CD029
		[NullableContext(0)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void SaveGodotObjectData(GodotSerializationInfo info)
		{
			base.SaveGodotObjectData(info);
		}

		/// <inheritdoc />
		// Token: 0x06003C7A RID: 15482 RVA: 0x001CEE32 File Offset: 0x001CD032
		[NullableContext(0)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void RestoreGodotObjectData(GodotSerializationInfo info)
		{
			base.RestoreGodotObjectData(info);
		}

		// Token: 0x04001799 RID: 6041
		private readonly Dictionary<StringName, List<Action>> _hotkeyPressedBindings = new Dictionary<StringName, List<Action>>();

		// Token: 0x0400179A RID: 6042
		private readonly Dictionary<StringName, List<Action>> _hotkeyReleasedBindings = new Dictionary<StringName, List<Action>>();

		// Token: 0x0400179B RID: 6043
		private Dictionary<Node, Action> _blockingScreens = new Dictionary<Node, Action>();

		/// <summary>
		/// Cached StringNames for the methods contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x02001853 RID: 6227
		[NullableContext(0)]
		public class MethodName : Node.MethodName
		{
			/// <summary>
			/// Cached name for the 'AddBlockingScreen' method.
			/// </summary>
			// Token: 0x04005F3F RID: 24383
			public static readonly StringName AddBlockingScreen = "AddBlockingScreen";

			/// <summary>
			/// Cached name for the 'RemoveBlockingScreen' method.
			/// </summary>
			// Token: 0x04005F40 RID: 24384
			public static readonly StringName RemoveBlockingScreen = "RemoveBlockingScreen";

			/// <summary>
			/// Cached name for the '_UnhandledInput' method.
			/// </summary>
			// Token: 0x04005F41 RID: 24385
			public static readonly StringName _UnhandledInput = "_UnhandledInput";
		}

		/// <summary>
		/// Cached StringNames for the properties and fields contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x02001854 RID: 6228
		[NullableContext(0)]
		public class PropertyName : Node.PropertyName
		{
		}

		/// <summary>
		/// Cached StringNames for the signals contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x02001855 RID: 6229
		[NullableContext(0)]
		public class SignalName : Node.SignalName
		{
		}
	}
}
