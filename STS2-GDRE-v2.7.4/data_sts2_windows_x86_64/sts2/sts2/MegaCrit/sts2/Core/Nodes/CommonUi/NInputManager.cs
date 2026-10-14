using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Godot;
using Godot.Bridge;
using Godot.NativeInterop;
using MegaCrit.Sts2.Core.ControllerInput;
using MegaCrit.Sts2.Core.Debug;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.Nodes.Debug;
using MegaCrit.Sts2.Core.Platform;
using MegaCrit.Sts2.Core.Saves;

namespace MegaCrit.Sts2.Core.Nodes.CommonUi
{
	/// <summary>
	/// Listens for keyboard and controller inputs, and map them into input events that the UX
	/// can listen for (for navigation or hotkeys)
	/// That map can be edited to rebind different keyboard/controller inputs to different UX events
	/// </summary>
	// Token: 0x020003E2 RID: 994
	[NullableContext(1)]
	[Nullable(0)]
	[ScriptPath("res://src/Core/Nodes/CommonUi/NInputManager.cs")]
	public class NInputManager : Node
	{
		// Token: 0x170008C7 RID: 2247
		// (get) Token: 0x06003C7C RID: 15484 RVA: 0x001CEE64 File Offset: 0x001CD064
		[Nullable(2)]
		public static NInputManager Instance
		{
			[NullableContext(2)]
			get
			{
				if (NGame.Instance == null)
				{
					return null;
				}
				return NGame.Instance.InputManager;
			}
		}

		// Token: 0x170008C8 RID: 2248
		// (get) Token: 0x06003C7D RID: 15485 RVA: 0x001CEE7C File Offset: 0x001CD07C
		private static Dictionary<StringName, Key> DefaultKeyboardInputMap
		{
			get
			{
				return new Dictionary<StringName, Key>
				{
					{
						MegaInput.accept,
						69L
					},
					{
						MegaInput.select,
						4194309L
					},
					{
						MegaInput.viewDiscardPile,
						83L
					},
					{
						MegaInput.viewDeckAndTabLeft,
						68L
					},
					{
						MegaInput.viewExhaustPileAndTabRight,
						88L
					},
					{
						MegaInput.viewDrawPile,
						65L
					},
					{
						MegaInput.viewMap,
						77L
					},
					{
						MegaInput.cancel,
						4194305L
					},
					{
						MegaInput.peek,
						32L
					},
					{
						MegaInput.up,
						4194320L
					},
					{
						MegaInput.down,
						4194322L
					},
					{
						MegaInput.left,
						4194319L
					},
					{
						MegaInput.right,
						4194321L
					},
					{
						MegaInput.pauseAndBack,
						4194305L
					},
					{
						MegaInput.selectCard1,
						49L
					},
					{
						MegaInput.selectCard2,
						50L
					},
					{
						MegaInput.selectCard3,
						51L
					},
					{
						MegaInput.selectCard4,
						52L
					},
					{
						MegaInput.selectCard5,
						53L
					},
					{
						MegaInput.selectCard6,
						54L
					},
					{
						MegaInput.selectCard7,
						55L
					},
					{
						MegaInput.selectCard8,
						56L
					},
					{
						MegaInput.selectCard9,
						57L
					},
					{
						MegaInput.selectCard10,
						48L
					},
					{
						MegaInput.releaseCard,
						4194322L
					}
				};
			}
		}

		// Token: 0x170008C9 RID: 2249
		// (get) Token: 0x06003C7E RID: 15486 RVA: 0x001CF004 File Offset: 0x001CD204
		// (set) Token: 0x06003C7F RID: 15487 RVA: 0x001CF00C File Offset: 0x001CD20C
		public NControllerManager ControllerManager { get; private set; }

		// Token: 0x06003C80 RID: 15488 RVA: 0x001CF015 File Offset: 0x001CD215
		public override void _EnterTree()
		{
			this.ControllerManager = base.GetNode<NControllerManager>("%ControllerManager");
		}

		// Token: 0x06003C81 RID: 15489 RVA: 0x001CF02D File Offset: 0x001CD22D
		public override void _Ready()
		{
			this.ControllerManager.Connect(NControllerManager.SignalName.ControllerTypeChanged, Callable.From(new Action(this.OnControllerTypeChanged)), 0U);
			TaskHelper.RunSafely(this.Init());
		}

		// Token: 0x06003C82 RID: 15490 RVA: 0x001CF060 File Offset: 0x001CD260
		private async Task Init()
		{
			await this.ControllerManager.Init();
			SettingsSave settingsSave = SaveManager.Instance.SettingsSave;
			if (settingsSave.KeyboardMapping.Count > 0)
			{
				this._keyboardInputMap = new Dictionary<StringName, Key>(NInputManager.DefaultKeyboardInputMap);
				using (Dictionary<string, string>.Enumerator enumerator = settingsSave.KeyboardMapping.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						KeyValuePair<string, string> keyValuePair = enumerator.Current;
						Key key;
						if (Enum.TryParse<Key>(keyValuePair.Value, out key))
						{
							this._keyboardInputMap[keyValuePair.Key] = key;
						}
					}
					goto IL_010D;
				}
			}
			this._keyboardInputMap = NInputManager.DefaultKeyboardInputMap;
			this.SaveKeyboardInputMapping();
			IL_010D:
			if (settingsSave.ControllerMapping.Count > 0 && settingsSave.ControllerMappingType == this.ControllerManager.ControllerMappingType)
			{
				this._controllerInputMap = NInputManager.MergeSavedControllerBindings(this.ControllerManager.GetDefaultControllerInputMap, settingsSave.ControllerMapping);
			}
			else
			{
				this._controllerInputMap = this.ControllerManager.GetDefaultControllerInputMap;
				this.SaveControllerInputMapping();
			}
		}

		/// <summary>
		/// Overlays a saved controller mapping onto the defaults, ignoring any saved binding whose
		/// value is not a registered InputMap action. A binding can point at a missing action when a
		/// save was migrated forward by a newer build and then loaded by an older one, or when a save
		/// was hand-edited. Applying it would make <see cref="M:MegaCrit.Sts2.Core.Nodes.CommonUi.NInputManager._UnhandledInput(Godot.InputEvent)" /> call
		/// <c>IsActionPressed</c> on a nonexistent action and spam errors, so the default is kept.
		/// </summary>
		// Token: 0x06003C83 RID: 15491 RVA: 0x001CF0A4 File Offset: 0x001CD2A4
		public static Dictionary<StringName, StringName> MergeSavedControllerBindings(Dictionary<StringName, StringName> defaults, Dictionary<string, string> savedMapping)
		{
			Dictionary<StringName, StringName> dictionary = new Dictionary<StringName, StringName>(defaults);
			foreach (KeyValuePair<string, string> keyValuePair in savedMapping)
			{
				if (InputMap.HasAction(keyValuePair.Value))
				{
					dictionary[keyValuePair.Key] = keyValuePair.Value;
				}
			}
			return dictionary;
		}

		// Token: 0x06003C84 RID: 15492 RVA: 0x001CF124 File Offset: 0x001CD324
		public override void _UnhandledKeyInput(InputEvent inputEvent)
		{
			this.ProcessShortcutKeyInput(inputEvent);
			this.ProcessDebugKeyInput(inputEvent);
		}

		// Token: 0x06003C85 RID: 15493 RVA: 0x001CF134 File Offset: 0x001CD334
		private void ProcessDebugKeyInput(InputEvent inputEvent)
		{
			InputEventKey inputEventKey = inputEvent as InputEventKey;
			if (inputEventKey == null)
			{
				return;
			}
			if (PlatformUtil.IsPlatformOverlayOpen())
			{
				return;
			}
			if (!DisplayServer.WindowIsFocused(0))
			{
				return;
			}
			if (NDevConsole.IsConsoleVisible || !NGame.IsTrailerMode)
			{
				return;
			}
			foreach (KeyValuePair<Key, StringName> keyValuePair in this._debugInputMap)
			{
				if (inputEventKey.Keycode == keyValuePair.Key)
				{
					InputEventAction inputEventAction = new InputEventAction
					{
						Action = keyValuePair.Value,
						Pressed = inputEvent.IsPressed()
					};
					Input.ParseInputEvent(inputEventAction);
				}
			}
		}

		// Token: 0x06003C86 RID: 15494 RVA: 0x001CF1E0 File Offset: 0x001CD3E0
		private void ProcessShortcutKeyInput(InputEvent inputEvent)
		{
			if (NGame.Instance.Transition.InTransition)
			{
				return;
			}
			if (!NGame.IsGameFocusedWindow())
			{
				return;
			}
			InputEventKey inputEventKey = inputEvent as InputEventKey;
			if (inputEventKey == null)
			{
				return;
			}
			foreach (KeyValuePair<StringName, Key> keyValuePair in this._keyboardInputMap)
			{
				if (inputEventKey.Keycode == keyValuePair.Value && !inputEvent.IsEcho())
				{
					InputEventAction inputEventAction = new InputEventAction
					{
						Action = keyValuePair.Key,
						Pressed = inputEvent.IsPressed()
					};
					Input.ParseInputEvent(inputEventAction);
				}
			}
		}

		// Token: 0x06003C87 RID: 15495 RVA: 0x001CF290 File Offset: 0x001CD490
		public override void _UnhandledInput(InputEvent inputEvent)
		{
			if (NGame.Instance.Transition.InTransition)
			{
				return;
			}
			if (!NGame.IsGameFocusedWindow())
			{
				return;
			}
			foreach (KeyValuePair<StringName, StringName> keyValuePair in this._controllerInputMap)
			{
				if (inputEvent.IsActionPressed(keyValuePair.Value, false, false))
				{
					InputEventAction inputEventAction = new InputEventAction
					{
						Action = keyValuePair.Key,
						Pressed = true
					};
					Input.ParseInputEvent(inputEventAction);
				}
				else if (inputEvent.IsActionReleased(keyValuePair.Value, false))
				{
					InputEventAction inputEventAction2 = new InputEventAction
					{
						Action = keyValuePair.Key,
						Pressed = false
					};
					Input.ParseInputEvent(inputEventAction2);
				}
			}
		}

		// Token: 0x06003C88 RID: 15496 RVA: 0x001CF35C File Offset: 0x001CD55C
		public Key GetShortcutKey(StringName input)
		{
			Key key;
			if (!this._keyboardInputMap.TryGetValue(input, out key))
			{
				return 0L;
			}
			return key;
		}

		// Token: 0x06003C89 RID: 15497 RVA: 0x001CF380 File Offset: 0x001CD580
		[return: Nullable(2)]
		public Texture2D GetHotkeyIcon(string hotkey)
		{
			StringName stringName;
			if (this._controllerInputMap.TryGetValue(hotkey, out stringName))
			{
				return this.ControllerManager.GetHotkeyIcon(stringName);
			}
			return null;
		}

		// Token: 0x06003C8A RID: 15498 RVA: 0x001CF3B8 File Offset: 0x001CD5B8
		public void ModifyShortcutKey(StringName input, Key shortcutKey)
		{
			KeyValuePair<StringName, Key> keyValuePair = this._keyboardInputMap.FirstOrDefault<KeyValuePair<StringName, Key>>((KeyValuePair<StringName, Key> kvp) => kvp.Value == shortcutKey && NInputManager.remappableKeyboardInputs.Contains(kvp.Key));
			if (keyValuePair.Key != null)
			{
				Key key = this._keyboardInputMap[input];
				this._keyboardInputMap[keyValuePair.Key] = key;
			}
			this._keyboardInputMap[input] = shortcutKey;
			this.SaveKeyboardInputMapping();
			this.EmitSignalInputRebound();
		}

		// Token: 0x06003C8B RID: 15499 RVA: 0x001CF438 File Offset: 0x001CD638
		public void ModifyControllerButton(StringName input, StringName controllerInput)
		{
			KeyValuePair<StringName, StringName> keyValuePair = this._controllerInputMap.FirstOrDefault<KeyValuePair<StringName, StringName>>((KeyValuePair<StringName, StringName> kvp) => kvp.Value == controllerInput && NInputManager.remappableControllerInputs.Contains(kvp.Key));
			if (keyValuePair.Key != null)
			{
				StringName stringName = this._controllerInputMap[input];
				this._controllerInputMap[keyValuePair.Key] = stringName;
			}
			this._controllerInputMap[input] = controllerInput;
			this.SaveControllerInputMapping();
			this.EmitSignalInputRebound();
		}

		// Token: 0x06003C8C RID: 15500 RVA: 0x001CF4B7 File Offset: 0x001CD6B7
		public void ResetToDefaults()
		{
			this._keyboardInputMap = NInputManager.DefaultKeyboardInputMap;
			this._controllerInputMap = this.ControllerManager.GetDefaultControllerInputMap;
			this.SaveControllerInputMapping();
			this.SaveKeyboardInputMapping();
			this.EmitSignalInputRebound();
		}

		// Token: 0x06003C8D RID: 15501 RVA: 0x001CF4E7 File Offset: 0x001CD6E7
		private void OnControllerTypeChanged()
		{
			if (this.ControllerManager.ControllerMappingType != SaveManager.Instance.SettingsSave.ControllerMappingType)
			{
				this._controllerInputMap = this.ControllerManager.GetDefaultControllerInputMap;
				this.SaveControllerInputMapping();
				this.EmitSignalInputRebound();
			}
		}

		// Token: 0x06003C8E RID: 15502 RVA: 0x001CF524 File Offset: 0x001CD724
		private void SaveControllerInputMapping()
		{
			Dictionary<string, string> dictionary = new Dictionary<string, string>();
			foreach (KeyValuePair<StringName, StringName> keyValuePair in this._controllerInputMap)
			{
				dictionary.Add(keyValuePair.Key.ToString(), keyValuePair.Value.ToString());
			}
			SaveManager.Instance.SettingsSave.ControllerMappingType = this.ControllerManager.ControllerMappingType;
			SaveManager.Instance.SettingsSave.ControllerMapping = dictionary;
			SaveManager.Instance.SaveSettings();
		}

		// Token: 0x06003C8F RID: 15503 RVA: 0x001CF5C8 File Offset: 0x001CD7C8
		private void SaveKeyboardInputMapping()
		{
			Dictionary<string, string> dictionary = new Dictionary<string, string>();
			foreach (KeyValuePair<StringName, Key> keyValuePair in this._keyboardInputMap)
			{
				dictionary.Add(keyValuePair.Key.ToString(), keyValuePair.Value.ToString());
			}
			SaveManager.Instance.SettingsSave.KeyboardMapping = dictionary;
			SaveManager.Instance.SaveSettings();
		}

		/// <summary>
		/// Get the method information for all the methods declared in this class.
		/// This method is used by Godot to register the available methods in the editor.
		/// Do not call this method.
		/// </summary>
		// Token: 0x06003C90 RID: 15504 RVA: 0x001CF65C File Offset: 0x001CD85C
		[NullableContext(0)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal static List<MethodInfo> GetGodotMethodList()
		{
			return new List<MethodInfo>(14)
			{
				new MethodInfo(NInputManager.MethodName._EnterTree, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NInputManager.MethodName._Ready, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NInputManager.MethodName._UnhandledKeyInput, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, new List<PropertyInfo>
				{
					new PropertyInfo(24L, "inputEvent", 0L, "", 6L, new StringName("InputEvent"), false)
				}, null),
				new MethodInfo(NInputManager.MethodName.ProcessDebugKeyInput, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, new List<PropertyInfo>
				{
					new PropertyInfo(24L, "inputEvent", 0L, "", 6L, new StringName("InputEvent"), false)
				}, null),
				new MethodInfo(NInputManager.MethodName.ProcessShortcutKeyInput, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, new List<PropertyInfo>
				{
					new PropertyInfo(24L, "inputEvent", 0L, "", 6L, new StringName("InputEvent"), false)
				}, null),
				new MethodInfo(NInputManager.MethodName._UnhandledInput, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, new List<PropertyInfo>
				{
					new PropertyInfo(24L, "inputEvent", 0L, "", 6L, new StringName("InputEvent"), false)
				}, null),
				new MethodInfo(NInputManager.MethodName.GetShortcutKey, new PropertyInfo(2L, "", 0L, "", 6L, false), 1L, new List<PropertyInfo>
				{
					new PropertyInfo(21L, "input", 0L, "", 6L, false)
				}, null),
				new MethodInfo(NInputManager.MethodName.GetHotkeyIcon, new PropertyInfo(24L, "", 0L, "", 6L, new StringName("Texture2D"), false), 1L, new List<PropertyInfo>
				{
					new PropertyInfo(4L, "hotkey", 0L, "", 6L, false)
				}, null),
				new MethodInfo(NInputManager.MethodName.ModifyShortcutKey, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, new List<PropertyInfo>
				{
					new PropertyInfo(21L, "input", 0L, "", 6L, false),
					new PropertyInfo(2L, "shortcutKey", 0L, "", 6L, false)
				}, null),
				new MethodInfo(NInputManager.MethodName.ModifyControllerButton, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, new List<PropertyInfo>
				{
					new PropertyInfo(21L, "input", 0L, "", 6L, false),
					new PropertyInfo(21L, "controllerInput", 0L, "", 6L, false)
				}, null),
				new MethodInfo(NInputManager.MethodName.ResetToDefaults, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NInputManager.MethodName.OnControllerTypeChanged, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NInputManager.MethodName.SaveControllerInputMapping, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NInputManager.MethodName.SaveKeyboardInputMapping, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null)
			};
		}

		/// <inheritdoc />
		// Token: 0x06003C91 RID: 15505 RVA: 0x001CFAAC File Offset: 0x001CDCAC
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool InvokeGodotClassMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
		{
			if ((ref method) == NInputManager.MethodName._EnterTree && args.Count == 0)
			{
				this._EnterTree();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NInputManager.MethodName._Ready && args.Count == 0)
			{
				this._Ready();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NInputManager.MethodName._UnhandledKeyInput && args.Count == 1)
			{
				this._UnhandledKeyInput(VariantUtils.ConvertTo<InputEvent>(args[0]));
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NInputManager.MethodName.ProcessDebugKeyInput && args.Count == 1)
			{
				this.ProcessDebugKeyInput(VariantUtils.ConvertTo<InputEvent>(args[0]));
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NInputManager.MethodName.ProcessShortcutKeyInput && args.Count == 1)
			{
				this.ProcessShortcutKeyInput(VariantUtils.ConvertTo<InputEvent>(args[0]));
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NInputManager.MethodName._UnhandledInput && args.Count == 1)
			{
				this._UnhandledInput(VariantUtils.ConvertTo<InputEvent>(args[0]));
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NInputManager.MethodName.GetShortcutKey && args.Count == 1)
			{
				Key shortcutKey = this.GetShortcutKey(VariantUtils.ConvertTo<StringName>(args[0]));
				ret = VariantUtils.CreateFrom<Key>(ref shortcutKey);
				return true;
			}
			if ((ref method) == NInputManager.MethodName.GetHotkeyIcon && args.Count == 1)
			{
				Texture2D hotkeyIcon = this.GetHotkeyIcon(VariantUtils.ConvertTo<string>(args[0]));
				ret = VariantUtils.CreateFrom<Texture2D>(ref hotkeyIcon);
				return true;
			}
			if ((ref method) == NInputManager.MethodName.ModifyShortcutKey && args.Count == 2)
			{
				this.ModifyShortcutKey(VariantUtils.ConvertTo<StringName>(args[0]), VariantUtils.ConvertTo<Key>(args[1]));
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NInputManager.MethodName.ModifyControllerButton && args.Count == 2)
			{
				this.ModifyControllerButton(VariantUtils.ConvertTo<StringName>(args[0]), VariantUtils.ConvertTo<StringName>(args[1]));
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NInputManager.MethodName.ResetToDefaults && args.Count == 0)
			{
				this.ResetToDefaults();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NInputManager.MethodName.OnControllerTypeChanged && args.Count == 0)
			{
				this.OnControllerTypeChanged();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NInputManager.MethodName.SaveControllerInputMapping && args.Count == 0)
			{
				this.SaveControllerInputMapping();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NInputManager.MethodName.SaveKeyboardInputMapping && args.Count == 0)
			{
				this.SaveKeyboardInputMapping();
				ret = default(godot_variant);
				return true;
			}
			return base.InvokeGodotClassMethod(ref method, args, ref ret);
		}

		/// <inheritdoc />
		// Token: 0x06003C92 RID: 15506 RVA: 0x001CFD60 File Offset: 0x001CDF60
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool HasGodotClassMethod(in godot_string_name method)
		{
			return (ref method) == NInputManager.MethodName._EnterTree || (ref method) == NInputManager.MethodName._Ready || (ref method) == NInputManager.MethodName._UnhandledKeyInput || (ref method) == NInputManager.MethodName.ProcessDebugKeyInput || (ref method) == NInputManager.MethodName.ProcessShortcutKeyInput || (ref method) == NInputManager.MethodName._UnhandledInput || (ref method) == NInputManager.MethodName.GetShortcutKey || (ref method) == NInputManager.MethodName.GetHotkeyIcon || (ref method) == NInputManager.MethodName.ModifyShortcutKey || (ref method) == NInputManager.MethodName.ModifyControllerButton || (ref method) == NInputManager.MethodName.ResetToDefaults || (ref method) == NInputManager.MethodName.OnControllerTypeChanged || (ref method) == NInputManager.MethodName.SaveControllerInputMapping || (ref method) == NInputManager.MethodName.SaveKeyboardInputMapping || base.HasGodotClassMethod(ref method);
		}

		/// <inheritdoc />
		// Token: 0x06003C93 RID: 15507 RVA: 0x001CFE46 File Offset: 0x001CE046
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool SetGodotClassPropertyValue(in godot_string_name name, in godot_variant value)
		{
			if ((ref name) == NInputManager.PropertyName.ControllerManager)
			{
				this.ControllerManager = VariantUtils.ConvertTo<NControllerManager>(ref value);
				return true;
			}
			return base.SetGodotClassPropertyValue(ref name, ref value);
		}

		/// <inheritdoc />
		// Token: 0x06003C94 RID: 15508 RVA: 0x001CFE6C File Offset: 0x001CE06C
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
		{
			if ((ref name) == NInputManager.PropertyName.ControllerManager)
			{
				NControllerManager controllerManager = this.ControllerManager;
				value = VariantUtils.CreateFrom<NControllerManager>(ref controllerManager);
				return true;
			}
			return base.GetGodotClassPropertyValue(ref name, ref value);
		}

		/// <summary>
		/// Get the property information for all the properties declared in this class.
		/// This method is used by Godot to register the available properties in the editor.
		/// Do not call this method.
		/// </summary>
		// Token: 0x06003C95 RID: 15509 RVA: 0x001CFEA4 File Offset: 0x001CE0A4
		[NullableContext(0)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal static List<PropertyInfo> GetGodotPropertyList()
		{
			return new List<PropertyInfo>
			{
				new PropertyInfo(24L, NInputManager.PropertyName.ControllerManager, 0L, "", 4096L, false)
			};
		}

		/// <inheritdoc />
		// Token: 0x06003C96 RID: 15510 RVA: 0x001CFEDC File Offset: 0x001CE0DC
		[NullableContext(0)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void SaveGodotObjectData(GodotSerializationInfo info)
		{
			base.SaveGodotObjectData(info);
			StringName controllerManager = NInputManager.PropertyName.ControllerManager;
			NControllerManager controllerManager2 = this.ControllerManager;
			info.AddProperty(controllerManager, Variant.From<NControllerManager>(ref controllerManager2));
			info.AddSignalEventDelegate(NInputManager.SignalName.InputRebound, this.backing_InputRebound);
		}

		/// <inheritdoc />
		// Token: 0x06003C97 RID: 15511 RVA: 0x001CFF1C File Offset: 0x001CE11C
		[NullableContext(0)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void RestoreGodotObjectData(GodotSerializationInfo info)
		{
			base.RestoreGodotObjectData(info);
			Variant variant;
			if (info.TryGetProperty(NInputManager.PropertyName.ControllerManager, ref variant))
			{
				this.ControllerManager = variant.As<NControllerManager>();
			}
			NInputManager.InputReboundEventHandler inputReboundEventHandler;
			if (info.TryGetSignalEventDelegate<NInputManager.InputReboundEventHandler>(NInputManager.SignalName.InputRebound, ref inputReboundEventHandler))
			{
				this.backing_InputRebound = inputReboundEventHandler;
			}
		}

		/// <summary>
		/// Get the signal information for all the signals declared in this class.
		/// This method is used by Godot to register the available signals in the editor.
		/// Do not call this method.
		/// </summary>
		// Token: 0x06003C98 RID: 15512 RVA: 0x001CFF64 File Offset: 0x001CE164
		[NullableContext(0)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal static List<MethodInfo> GetGodotSignalList()
		{
			return new List<MethodInfo>(1)
			{
				new MethodInfo(NInputManager.SignalName.InputRebound, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null)
			};
		}

		/// <inheritdoc cref="T:MegaCrit.Sts2.Core.Nodes.CommonUi.NInputManager.InputReboundEventHandler" />
		// Token: 0x14000043 RID: 67
		// (add) Token: 0x06003C99 RID: 15513 RVA: 0x001CFFA8 File Offset: 0x001CE1A8
		// (remove) Token: 0x06003C9A RID: 15514 RVA: 0x001CFFC1 File Offset: 0x001CE1C1
		[Nullable(0)]
		public event NInputManager.InputReboundEventHandler InputRebound
		{
			[NullableContext(0)]
			add
			{
				this.backing_InputRebound = (NInputManager.InputReboundEventHandler)Delegate.Combine(this.backing_InputRebound, value);
			}
			[NullableContext(0)]
			remove
			{
				this.backing_InputRebound = (NInputManager.InputReboundEventHandler)Delegate.Remove(this.backing_InputRebound, value);
			}
		}

		// Token: 0x06003C9B RID: 15515 RVA: 0x001CFFDA File Offset: 0x001CE1DA
		protected void EmitSignalInputRebound()
		{
			base.EmitSignal(NInputManager.SignalName.InputRebound, Array.Empty<Variant>());
		}

		/// <inheritdoc />
		// Token: 0x06003C9C RID: 15516 RVA: 0x001CFFED File Offset: 0x001CE1ED
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void RaiseGodotClassSignalCallbacks(in godot_string_name signal, NativeVariantPtrArgs args)
		{
			if (!((ref signal) == NInputManager.SignalName.InputRebound) || args.Count != 0)
			{
				base.RaiseGodotClassSignalCallbacks(ref signal, args);
				return;
			}
			NInputManager.InputReboundEventHandler inputReboundEventHandler = this.backing_InputRebound;
			if (inputReboundEventHandler == null)
			{
				return;
			}
			inputReboundEventHandler();
		}

		/// <inheritdoc />
		// Token: 0x06003C9D RID: 15517 RVA: 0x001D001E File Offset: 0x001CE21E
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool HasGodotClassSignal(in godot_string_name signal)
		{
			return (ref signal) == NInputManager.SignalName.InputRebound || base.HasGodotClassSignal(ref signal);
		}

		// Token: 0x0400179C RID: 6044
		private readonly Dictionary<Key, StringName> _debugInputMap = new Dictionary<Key, StringName>
		{
			{
				49L,
				DebugHotkey.hideTopBar
			},
			{
				50L,
				DebugHotkey.hideIntents
			},
			{
				51L,
				DebugHotkey.hideCombatUi
			},
			{
				52L,
				DebugHotkey.hidePlayContainer
			},
			{
				53L,
				DebugHotkey.hideHand
			},
			{
				54L,
				DebugHotkey.hideHpBars
			},
			{
				55L,
				DebugHotkey.hideTextVfx
			},
			{
				56L,
				DebugHotkey.hideTargetingUi
			},
			{
				57L,
				DebugHotkey.slowRewards
			},
			{
				48L,
				DebugHotkey.hideVersionInfo
			},
			{
				45L,
				DebugHotkey.speedDown
			},
			{
				61L,
				DebugHotkey.speedUp
			},
			{
				4194332L,
				DebugHotkey.hideRestSite
			},
			{
				4194334L,
				DebugHotkey.hideEventUi
			},
			{
				4194335L,
				DebugHotkey.hideProceedButton
			},
			{
				4194336L,
				DebugHotkey.hideHoverTips
			},
			{
				4194337L,
				DebugHotkey.hideMpCursors
			},
			{
				4194338L,
				DebugHotkey.hideMpTargeting
			},
			{
				4194340L,
				DebugHotkey.hideMpIntents
			},
			{
				4194341L,
				DebugHotkey.hideMpHealthBars
			},
			{
				85L,
				DebugHotkey.unlockCharacters
			}
		};

		// Token: 0x0400179D RID: 6045
		public static readonly IReadOnlyList<StringName> remappableKeyboardInputs = new List<StringName>
		{
			MegaInput.select,
			MegaInput.cancel,
			MegaInput.viewMap,
			MegaInput.viewDeckAndTabLeft,
			MegaInput.viewDrawPile,
			MegaInput.viewDiscardPile,
			MegaInput.viewExhaustPileAndTabRight,
			MegaInput.accept,
			MegaInput.peek,
			MegaInput.up,
			MegaInput.down,
			MegaInput.left,
			MegaInput.right,
			MegaInput.selectCard1,
			MegaInput.selectCard2,
			MegaInput.selectCard3,
			MegaInput.selectCard4,
			MegaInput.selectCard5,
			MegaInput.selectCard6,
			MegaInput.selectCard7,
			MegaInput.selectCard8,
			MegaInput.selectCard9,
			MegaInput.selectCard10,
			MegaInput.releaseCard
		};

		// Token: 0x0400179E RID: 6046
		public static readonly IReadOnlyList<StringName> remappableControllerInputs = new List<StringName>
		{
			MegaInput.select,
			MegaInput.cancel,
			MegaInput.viewMap,
			MegaInput.topPanel,
			MegaInput.viewDeckAndTabLeft,
			MegaInput.viewDrawPile,
			MegaInput.viewDiscardPile,
			MegaInput.viewExhaustPileAndTabRight,
			MegaInput.accept,
			MegaInput.peek,
			MegaInput.up,
			MegaInput.down,
			MegaInput.left,
			MegaInput.right
		};

		// Token: 0x0400179F RID: 6047
		private Dictionary<StringName, Key> _keyboardInputMap = new Dictionary<StringName, Key>();

		// Token: 0x040017A0 RID: 6048
		private Dictionary<StringName, StringName> _controllerInputMap = new Dictionary<StringName, StringName>();

		// Token: 0x040017A2 RID: 6050
		[Nullable(0)]
		private NInputManager.InputReboundEventHandler backing_InputRebound;

		// Token: 0x02001857 RID: 6231
		// (Invoke) Token: 0x0600A87C RID: 43132
		[NullableContext(0)]
		[Signal]
		public delegate void InputReboundEventHandler();

		/// <summary>
		/// Cached StringNames for the methods contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x02001858 RID: 6232
		[NullableContext(0)]
		public class MethodName : Node.MethodName
		{
			/// <summary>
			/// Cached name for the '_EnterTree' method.
			/// </summary>
			// Token: 0x04005F44 RID: 24388
			public static readonly StringName _EnterTree = "_EnterTree";

			/// <summary>
			/// Cached name for the '_Ready' method.
			/// </summary>
			// Token: 0x04005F45 RID: 24389
			public static readonly StringName _Ready = "_Ready";

			/// <summary>
			/// Cached name for the '_UnhandledKeyInput' method.
			/// </summary>
			// Token: 0x04005F46 RID: 24390
			public static readonly StringName _UnhandledKeyInput = "_UnhandledKeyInput";

			/// <summary>
			/// Cached name for the 'ProcessDebugKeyInput' method.
			/// </summary>
			// Token: 0x04005F47 RID: 24391
			public static readonly StringName ProcessDebugKeyInput = "ProcessDebugKeyInput";

			/// <summary>
			/// Cached name for the 'ProcessShortcutKeyInput' method.
			/// </summary>
			// Token: 0x04005F48 RID: 24392
			public static readonly StringName ProcessShortcutKeyInput = "ProcessShortcutKeyInput";

			/// <summary>
			/// Cached name for the '_UnhandledInput' method.
			/// </summary>
			// Token: 0x04005F49 RID: 24393
			public static readonly StringName _UnhandledInput = "_UnhandledInput";

			/// <summary>
			/// Cached name for the 'GetShortcutKey' method.
			/// </summary>
			// Token: 0x04005F4A RID: 24394
			public static readonly StringName GetShortcutKey = "GetShortcutKey";

			/// <summary>
			/// Cached name for the 'GetHotkeyIcon' method.
			/// </summary>
			// Token: 0x04005F4B RID: 24395
			public static readonly StringName GetHotkeyIcon = "GetHotkeyIcon";

			/// <summary>
			/// Cached name for the 'ModifyShortcutKey' method.
			/// </summary>
			// Token: 0x04005F4C RID: 24396
			public static readonly StringName ModifyShortcutKey = "ModifyShortcutKey";

			/// <summary>
			/// Cached name for the 'ModifyControllerButton' method.
			/// </summary>
			// Token: 0x04005F4D RID: 24397
			public static readonly StringName ModifyControllerButton = "ModifyControllerButton";

			/// <summary>
			/// Cached name for the 'ResetToDefaults' method.
			/// </summary>
			// Token: 0x04005F4E RID: 24398
			public static readonly StringName ResetToDefaults = "ResetToDefaults";

			/// <summary>
			/// Cached name for the 'OnControllerTypeChanged' method.
			/// </summary>
			// Token: 0x04005F4F RID: 24399
			public static readonly StringName OnControllerTypeChanged = "OnControllerTypeChanged";

			/// <summary>
			/// Cached name for the 'SaveControllerInputMapping' method.
			/// </summary>
			// Token: 0x04005F50 RID: 24400
			public static readonly StringName SaveControllerInputMapping = "SaveControllerInputMapping";

			/// <summary>
			/// Cached name for the 'SaveKeyboardInputMapping' method.
			/// </summary>
			// Token: 0x04005F51 RID: 24401
			public static readonly StringName SaveKeyboardInputMapping = "SaveKeyboardInputMapping";
		}

		/// <summary>
		/// Cached StringNames for the properties and fields contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x02001859 RID: 6233
		[NullableContext(0)]
		public class PropertyName : Node.PropertyName
		{
			/// <summary>
			/// Cached name for the 'ControllerManager' property.
			/// </summary>
			// Token: 0x04005F52 RID: 24402
			public static readonly StringName ControllerManager = "ControllerManager";
		}

		/// <summary>
		/// Cached StringNames for the signals contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x0200185A RID: 6234
		[NullableContext(0)]
		public class SignalName : Node.SignalName
		{
			/// <summary>
			/// Cached name for the 'InputRebound' signal.
			/// </summary>
			// Token: 0x04005F53 RID: 24403
			public static readonly StringName InputRebound = "InputRebound";
		}
	}
}
