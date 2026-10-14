using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using Godot;
using MegaCrit.Sts2.Core.ControllerInput.ControllerConfigs;
using MegaCrit.Sts2.Core.Logging;
using MegaCrit.Sts2.Core.Nodes.CommonUi;
using MegaCrit.Sts2.Core.Nodes.GodotExtensions;
using MegaCrit.Sts2.Core.Platform.Steam;
using Steamworks;

namespace MegaCrit.Sts2.Core.ControllerInput
{
	// Token: 0x02000D22 RID: 3362
	[NullableContext(1)]
	[Nullable(0)]
	public class SteamControllerInputStrategy : IControllerInputStrategy
	{
		// Token: 0x17002566 RID: 9574
		// (get) Token: 0x06008794 RID: 34708 RVA: 0x002D9A8A File Offset: 0x002D7C8A
		[Nullable(2)]
		public ControllerConfig ControllerConfig
		{
			[NullableContext(2)]
			get
			{
				return this._controllerConfig ?? this._fallbackStrategy.ControllerConfig;
			}
		}

		// Token: 0x06008795 RID: 34709 RVA: 0x002D9AA4 File Offset: 0x002D7CA4
		public async Task Init()
		{
			await NControllerManager.Instance.AwaitProcessFrame(default(CancellationToken));
			if (SteamInitializer.Initialized)
			{
				try
				{
					SteamInput.Init(false);
					this.UpdateControllerConnections();
					this._joystickXAxis = new InputEventJoypadMotion
					{
						Axis = 0L,
						Device = 0
					};
					this._joystickYAxis = new InputEventJoypadMotion
					{
						Axis = 1L,
						Device = 0
					};
					this._up = new StringName("Up");
					this._down = new StringName("Down");
					this._left = new StringName("Left");
					this._right = new StringName("Right");
					this._joystickActionHandle = SteamInput.GetAnalogActionHandle("Joystick");
					goto IL_0144;
				}
				catch (InvalidOperationException ex)
				{
					Log.Error("Failed to initialize Steam Input: " + ex.Message, 2);
					goto IL_0144;
				}
			}
			Log.Warn("Cannot initialize Steam Input because Steamworks is not initialized. Falling back to standard input.", 2);
			IL_0144:
			await this._fallbackStrategy.Init();
		}

		// Token: 0x06008796 RID: 34710 RVA: 0x002D9AE8 File Offset: 0x002D7CE8
		public void ProcessInput()
		{
			if (!SteamInitializer.Initialized)
			{
				this._fallbackStrategy.ProcessInput();
				return;
			}
			double num = Time.GetTicksMsec() / 1000.0;
			if (num >= this._nextControllerCheckTime)
			{
				this._nextControllerCheckTime = num + 1.0;
				this.UpdateControllerConnections();
			}
			if (this._currentControllerHandle == null)
			{
				this._fallbackStrategy.ProcessInput();
				return;
			}
			try
			{
				SteamInput.RunFrame(true);
				this.ProcessDigitalInputs();
				this.ProcessAnalogInputs();
			}
			catch (InvalidOperationException ex)
			{
				Log.Error("Error running Steam Input frame: " + ex.Message, 2);
				this._currentControllerHandle = null;
				this._fallbackStrategy.ProcessInput();
			}
		}

		// Token: 0x06008797 RID: 34711 RVA: 0x002D9BA8 File Offset: 0x002D7DA8
		private void ProcessDigitalInputs()
		{
			if (this._controllerConfig == null)
			{
				return;
			}
			if (this._digitalActionHandleCache.Count == 0 && !this._attemptedHandleCacheRebuild)
			{
				this._attemptedHandleCacheRebuild = true;
				this.UpdateInputMap();
			}
			foreach (KeyValuePair<string, StringName> keyValuePair in this._controllerConfig.SteamInputControllerMap)
			{
				InputDigitalActionHandle_t inputDigitalActionHandle_t;
				if (!this._digitalActionHandleCache.TryGetValue(keyValuePair.Key, out inputDigitalActionHandle_t))
				{
					Log.Error("The input " + keyValuePair.Key + " was not cached during initialization. Skipping...", 2);
				}
				else
				{
					bool flag = SteamInput.GetDigitalActionData(this._currentControllerHandle.Value, inputDigitalActionHandle_t).bState == 1;
					bool flag2 = this._pressedInputs.Contains(keyValuePair.Key);
					if (flag && !flag2)
					{
						this._pressedInputs.Add(keyValuePair.Key);
					}
					else if (!flag && flag2)
					{
						this._pressedInputs.Remove(keyValuePair.Key);
					}
					if (flag && !flag2)
					{
						InputEventAction inputEventAction = this._inputEvents[keyValuePair.Key];
						inputEventAction.Pressed = true;
						Input.ParseInputEvent(inputEventAction);
					}
					else if (!flag && flag2)
					{
						InputEventAction inputEventAction2 = this._inputEvents[keyValuePair.Key];
						inputEventAction2.Pressed = false;
						Input.ParseInputEvent(inputEventAction2);
					}
				}
			}
		}

		// Token: 0x06008798 RID: 34712 RVA: 0x002D9D3C File Offset: 0x002D7F3C
		private void ProcessAnalogInputs()
		{
			InputAnalogActionData_t analogActionData = SteamInput.GetAnalogActionData(this._currentControllerHandle.Value, this._joystickActionHandle);
			Vector2 vector;
			vector..ctor(analogActionData.x, analogActionData.y);
			if (vector.DistanceTo(this._lStickPosition) > 0.05f)
			{
				InputEventJoypadMotion joystickXAxis = this._joystickXAxis;
				joystickXAxis.AxisValue = vector.X;
				InputEventJoypadMotion joystickYAxis = this._joystickYAxis;
				joystickYAxis.AxisValue = -vector.Y;
				Input.ParseInputEvent(joystickXAxis);
				Input.ParseInputEvent(joystickYAxis);
			}
			if (vector.Y >= 0.5f && !this._pressedInputs.Contains("Joy_Up"))
			{
				InputEventAction inputEventAction = this._inputEvents[this._up];
				inputEventAction.Pressed = true;
				Input.ParseInputEvent(inputEventAction);
				this._pressedInputs.Add("Joy_Up");
			}
			else if (vector.Y < 0.5f && this._pressedInputs.Contains("Joy_Up"))
			{
				InputEventAction inputEventAction2 = this._inputEvents[this._up];
				inputEventAction2.Pressed = false;
				Input.ParseInputEvent(inputEventAction2);
				this._pressedInputs.Remove("Joy_Up");
			}
			if (vector.Y <= -0.5f && !this._pressedInputs.Contains("Joy_Down"))
			{
				InputEventAction inputEventAction3 = this._inputEvents[this._down];
				inputEventAction3.Pressed = true;
				Input.ParseInputEvent(inputEventAction3);
				this._pressedInputs.Add("Joy_Down");
			}
			else if (vector.Y > -0.5f && this._pressedInputs.Contains("Joy_Down"))
			{
				InputEventAction inputEventAction4 = this._inputEvents[this._down];
				inputEventAction4.Pressed = false;
				Input.ParseInputEvent(inputEventAction4);
				this._pressedInputs.Remove("Joy_Down");
			}
			if (vector.X <= -0.5f && !this._pressedInputs.Contains("Joy_Left"))
			{
				InputEventAction inputEventAction5 = this._inputEvents[this._left];
				inputEventAction5.Pressed = true;
				Input.ParseInputEvent(inputEventAction5);
				this._pressedInputs.Add("Joy_Left");
			}
			else if (vector.X > -0.5f && this._pressedInputs.Contains("Joy_Left"))
			{
				InputEventAction inputEventAction6 = this._inputEvents[this._left];
				inputEventAction6.Pressed = false;
				Input.ParseInputEvent(inputEventAction6);
				this._pressedInputs.Remove("Joy_Left");
			}
			if (vector.X >= 0.5f && !this._pressedInputs.Contains("Joy_Right"))
			{
				InputEventAction inputEventAction7 = this._inputEvents[this._right];
				inputEventAction7.Pressed = true;
				Input.ParseInputEvent(inputEventAction7);
				this._pressedInputs.Add("Joy_Right");
			}
			else if (vector.X < 0.5f && this._pressedInputs.Contains("Joy_Right"))
			{
				InputEventAction inputEventAction8 = this._inputEvents[this._right];
				inputEventAction8.Pressed = false;
				Input.ParseInputEvent(inputEventAction8);
				this._pressedInputs.Remove("Joy_Right");
			}
			this._lStickPosition = vector;
		}

		// Token: 0x06008799 RID: 34713 RVA: 0x002DA058 File Offset: 0x002D8258
		private void UpdateControllerConnections()
		{
			try
			{
				InputHandle_t[] array = new InputHandle_t[16];
				if (SteamInput.GetConnectedControllers(array) == 0)
				{
					this._currentControllerHandle = null;
					this._currentInputType = null;
					if (this._controllerConfig == null)
					{
						this.UpdateControllerConfig(14);
					}
				}
				else
				{
					this._currentControllerHandle = new InputHandle_t?(array[0]);
					ESteamInputType inputTypeForHandle = SteamInput.GetInputTypeForHandle(this._currentControllerHandle.Value);
					ESteamInputType? currentInputType = this._currentInputType;
					ESteamInputType esteamInputType = inputTypeForHandle;
					if (!((currentInputType.GetValueOrDefault() == esteamInputType) & (currentInputType != null)) || this._controllerConfig == null)
					{
						this._currentInputType = new ESteamInputType?(inputTypeForHandle);
						this._attemptedHandleCacheRebuild = false;
						this.UpdateControllerConfig(inputTypeForHandle);
						this.UpdateInputMap();
					}
					this._currentActionSetHandle = new InputActionSetHandle_t?(SteamInput.GetActionSetHandle("Controls"));
					SteamInput.ActivateActionSet(this._currentControllerHandle.Value, this._currentActionSetHandle.Value);
				}
			}
			catch (InvalidOperationException ex)
			{
				Log.Error("Failed to connect to Steam controller: " + ex.Message, 2);
				this._currentControllerHandle = null;
			}
		}

		// Token: 0x0600879A RID: 34714 RVA: 0x002DA178 File Offset: 0x002D8378
		private void UpdateControllerConfig(ESteamInputType controllerType)
		{
			ControllerConfig controllerConfig = this._controllerConfig;
			switch (controllerType)
			{
			case 2:
				this._controllerConfig = new Xbox360Config();
				goto IL_0088;
			case 3:
				this._controllerConfig = new XboxOneConfig();
				goto IL_0088;
			case 5:
			case 12:
			case 13:
				this._controllerConfig = new Ps4Config();
				goto IL_0088;
			case 8:
			case 9:
			case 10:
				this._controllerConfig = new SwitchConfig();
				goto IL_0088;
			}
			this._controllerConfig = new SteamControllerConfig();
			IL_0088:
			if (controllerConfig != null && controllerConfig.ControllerMappingType != this._controllerConfig.ControllerMappingType)
			{
				NControllerManager instance = NControllerManager.Instance;
				if (instance == null)
				{
					return;
				}
				instance.OnControllerTypeChanged();
			}
		}

		// Token: 0x0600879B RID: 34715 RVA: 0x002DA234 File Offset: 0x002D8434
		public string GetControllerName()
		{
			if (this._currentInputType != null)
			{
				return this._currentInputType.Value.ToString();
			}
			return this._fallbackStrategy.GetControllerName();
		}

		// Token: 0x0600879C RID: 34716 RVA: 0x002DA274 File Offset: 0x002D8474
		private void UpdateInputMap()
		{
			if (this._controllerConfig == null)
			{
				return;
			}
			this._inputEvents.Clear();
			this._digitalActionHandleCache.Clear();
			foreach (KeyValuePair<string, StringName> keyValuePair in this._controllerConfig.SteamInputControllerMap)
			{
				this._inputEvents[keyValuePair.Key] = new InputEventAction
				{
					Action = keyValuePair.Value
				};
			}
			foreach (string text in this._controllerConfig.SteamInputControllerMap.Keys)
			{
				StringName stringName = text;
				try
				{
					InputDigitalActionHandle_t digitalActionHandle = SteamInput.GetDigitalActionHandle(stringName);
					this._digitalActionHandleCache[stringName] = digitalActionHandle;
				}
				catch (InvalidOperationException ex)
				{
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(44, 2);
					defaultInterpolatedStringHandler.AppendLiteral("Failed to cache digital action handle for ");
					defaultInterpolatedStringHandler.AppendFormatted<StringName>(stringName);
					defaultInterpolatedStringHandler.AppendLiteral(": ");
					defaultInterpolatedStringHandler.AppendFormatted(ex.Message);
					Log.Error(defaultInterpolatedStringHandler.ToStringAndClear(), 2);
				}
			}
		}

		// Token: 0x0600879D RID: 34717 RVA: 0x002DA3CC File Offset: 0x002D85CC
		[return: Nullable(2)]
		public Texture2D GetHotkeyIcon(string hotkey)
		{
			if (!SteamInitializer.Initialized || this._currentControllerHandle == null)
			{
				return this._fallbackStrategy.GetHotkeyIcon(hotkey);
			}
			Dictionary<string, StringName> dictionary;
			if (this.ControllerConfig == null)
			{
				dictionary = new SteamControllerConfig().SteamInputControllerMap;
			}
			else
			{
				dictionary = this.ControllerConfig.SteamInputControllerMap;
			}
			string key = dictionary.FirstOrDefault<KeyValuePair<string, StringName>>((KeyValuePair<string, StringName> kvp) => kvp.Value == hotkey).Key;
			if (key == null)
			{
				ControllerConfig controllerConfig = this.ControllerConfig;
				if (controllerConfig == null)
				{
					return null;
				}
				return controllerConfig.GetButtonIcon(hotkey);
			}
			else
			{
				EInputActionOrigin[] array = new EInputActionOrigin[8];
				InputDigitalActionHandle_t inputDigitalActionHandle_t;
				if (!this._digitalActionHandleCache.TryGetValue(key, out inputDigitalActionHandle_t))
				{
					Log.Error("The input " + key + " was not cached during initialization.", 2);
					ControllerConfig controllerConfig2 = this.ControllerConfig;
					if (controllerConfig2 == null)
					{
						return null;
					}
					return controllerConfig2.GetButtonIcon(key);
				}
				else
				{
					SteamInput.GetDigitalActionOrigins(this._currentControllerHandle.Value, this._currentActionSetHandle.Value, inputDigitalActionHandle_t, array);
					if (array.Length != 0 && array[0] != null)
					{
						ESteamInputType inputTypeForHandle = SteamInput.GetInputTypeForHandle(this._currentControllerHandle.Value);
						EInputActionOrigin einputActionOrigin = SteamInput.TranslateActionOrigin(inputTypeForHandle, array[0]);
						StringName stringName;
						if (!this._steamInputsToMegaInputs.TryGetValue(einputActionOrigin, out stringName))
						{
							if (!this._fallbackSteamGlyphs.ContainsKey(einputActionOrigin))
							{
								string glyphSVGForActionOrigin = SteamInput.GetGlyphSVGForActionOrigin(einputActionOrigin, 0U);
								Image image = Image.LoadFromFile(glyphSVGForActionOrigin);
								this._fallbackSteamGlyphs.Add(einputActionOrigin, ImageTexture.CreateFromImage(image));
							}
							return this._fallbackSteamGlyphs[einputActionOrigin];
						}
						ControllerConfig controllerConfig3 = this.ControllerConfig;
						if (controllerConfig3 == null)
						{
							return null;
						}
						return controllerConfig3.GetButtonIcon(stringName);
					}
					else
					{
						ControllerConfig controllerConfig4 = this.ControllerConfig;
						if (controllerConfig4 == null)
						{
							return null;
						}
						return controllerConfig4.GetButtonIcon(hotkey);
					}
				}
			}
		}

		// Token: 0x0600879E RID: 34718 RVA: 0x002DA57A File Offset: 0x002D877A
		public Vector2 GetLeftAnalogStickDirection()
		{
			if (SteamInitializer.Initialized)
			{
				return new Vector2(this._lStickPosition.X, -this._lStickPosition.Y);
			}
			return this._fallbackStrategy.GetLeftAnalogStickDirection();
		}

		// Token: 0x17002567 RID: 9575
		// (get) Token: 0x0600879F RID: 34719 RVA: 0x002DA5AB File Offset: 0x002D87AB
		public Dictionary<StringName, StringName> GetDefaultControllerInputMap
		{
			get
			{
				if (this.ControllerConfig == null)
				{
					return this._fallbackStrategy.ControllerConfig.DefaultControllerInputMap;
				}
				return this.ControllerConfig.DefaultControllerInputMap;
			}
		}

		// Token: 0x17002568 RID: 9576
		// (get) Token: 0x060087A0 RID: 34720 RVA: 0x002DA5D1 File Offset: 0x002D87D1
		public bool ShouldAllowControllerRebinding
		{
			get
			{
				return !SteamInitializer.Initialized && this._fallbackStrategy.ShouldAllowControllerRebinding;
			}
		}

		// Token: 0x04002F44 RID: 12100
		private readonly Dictionary<EInputActionOrigin, StringName> _steamInputsToMegaInputs = new Dictionary<EInputActionOrigin, StringName>
		{
			{
				1,
				Controller.faceButtonSouth
			},
			{
				2,
				Controller.faceButtonEast
			},
			{
				3,
				Controller.faceButtonWest
			},
			{
				4,
				Controller.faceButtonNorth
			},
			{
				5,
				Controller.leftBumper
			},
			{
				6,
				Controller.rightBumper
			},
			{
				25,
				Controller.leftTrigger
			},
			{
				26,
				Controller.leftTrigger
			},
			{
				27,
				Controller.rightTrigger
			},
			{
				28,
				Controller.rightTrigger
			},
			{
				31,
				Controller.lStickUp
			},
			{
				32,
				Controller.lStickDown
			},
			{
				33,
				Controller.lStickLeft
			},
			{
				34,
				Controller.lStickRight
			},
			{
				30,
				Controller.lStickPress
			},
			{
				9,
				Controller.startButton
			},
			{
				10,
				Controller.selectButton
			},
			{
				50,
				Controller.faceButtonSouth
			},
			{
				51,
				Controller.faceButtonEast
			},
			{
				53,
				Controller.faceButtonWest
			},
			{
				52,
				Controller.faceButtonNorth
			},
			{
				54,
				Controller.leftBumper
			},
			{
				55,
				Controller.rightBumper
			},
			{
				79,
				Controller.leftTrigger
			},
			{
				80,
				Controller.leftTrigger
			},
			{
				81,
				Controller.rightTrigger
			},
			{
				82,
				Controller.rightTrigger
			},
			{
				95,
				Controller.dPadUp
			},
			{
				96,
				Controller.dPadDown
			},
			{
				97,
				Controller.dPadLeft
			},
			{
				98,
				Controller.dPadRight
			},
			{
				85,
				Controller.lStickUp
			},
			{
				86,
				Controller.lStickDown
			},
			{
				87,
				Controller.lStickLeft
			},
			{
				88,
				Controller.lStickRight
			},
			{
				84,
				Controller.lStickPress
			},
			{
				56,
				Controller.startButton
			},
			{
				57,
				Controller.selectButton
			},
			{
				74,
				Controller.ps4Touchpad
			},
			{
				67,
				Controller.ps4Touchpad
			},
			{
				60,
				Controller.ps4Touchpad
			},
			{
				258,
				Controller.faceButtonSouth
			},
			{
				259,
				Controller.faceButtonEast
			},
			{
				261,
				Controller.faceButtonWest
			},
			{
				260,
				Controller.faceButtonNorth
			},
			{
				262,
				Controller.leftBumper
			},
			{
				263,
				Controller.rightBumper
			},
			{
				288,
				Controller.leftTrigger
			},
			{
				289,
				Controller.leftTrigger
			},
			{
				290,
				Controller.rightTrigger
			},
			{
				291,
				Controller.rightTrigger
			},
			{
				304,
				Controller.dPadUp
			},
			{
				305,
				Controller.dPadDown
			},
			{
				306,
				Controller.dPadLeft
			},
			{
				307,
				Controller.dPadRight
			},
			{
				294,
				Controller.lStickUp
			},
			{
				295,
				Controller.lStickDown
			},
			{
				296,
				Controller.lStickLeft
			},
			{
				297,
				Controller.lStickRight
			},
			{
				293,
				Controller.lStickPress
			},
			{
				264,
				Controller.startButton
			},
			{
				265,
				Controller.selectButton
			},
			{
				283,
				Controller.ps4Touchpad
			},
			{
				276,
				Controller.ps4Touchpad
			},
			{
				269,
				Controller.ps4Touchpad
			},
			{
				114,
				Controller.faceButtonSouth
			},
			{
				115,
				Controller.faceButtonEast
			},
			{
				116,
				Controller.faceButtonWest
			},
			{
				117,
				Controller.faceButtonNorth
			},
			{
				118,
				Controller.leftBumper
			},
			{
				119,
				Controller.rightBumper
			},
			{
				122,
				Controller.leftTrigger
			},
			{
				123,
				Controller.leftTrigger
			},
			{
				124,
				Controller.rightTrigger
			},
			{
				125,
				Controller.rightTrigger
			},
			{
				138,
				Controller.dPadUp
			},
			{
				139,
				Controller.dPadDown
			},
			{
				140,
				Controller.dPadLeft
			},
			{
				141,
				Controller.dPadRight
			},
			{
				128,
				Controller.lStickUp
			},
			{
				129,
				Controller.lStickDown
			},
			{
				130,
				Controller.lStickLeft
			},
			{
				131,
				Controller.lStickRight
			},
			{
				127,
				Controller.lStickPress
			},
			{
				120,
				Controller.startButton
			},
			{
				121,
				Controller.selectButton
			},
			{
				153,
				Controller.faceButtonSouth
			},
			{
				154,
				Controller.faceButtonEast
			},
			{
				155,
				Controller.faceButtonWest
			},
			{
				156,
				Controller.faceButtonNorth
			},
			{
				157,
				Controller.leftBumper
			},
			{
				158,
				Controller.rightBumper
			},
			{
				161,
				Controller.leftTrigger
			},
			{
				162,
				Controller.leftTrigger
			},
			{
				163,
				Controller.rightTrigger
			},
			{
				164,
				Controller.rightTrigger
			},
			{
				177,
				Controller.dPadUp
			},
			{
				178,
				Controller.dPadDown
			},
			{
				179,
				Controller.dPadLeft
			},
			{
				180,
				Controller.dPadRight
			},
			{
				159,
				Controller.startButton
			},
			{
				160,
				Controller.selectButton
			},
			{
				192,
				Controller.faceButtonEast
			},
			{
				193,
				Controller.faceButtonSouth
			},
			{
				194,
				Controller.faceButtonNorth
			},
			{
				195,
				Controller.faceButtonWest
			},
			{
				196,
				Controller.leftBumper
			},
			{
				197,
				Controller.rightBumper
			},
			{
				217,
				Controller.dPadUp
			},
			{
				218,
				Controller.dPadDown
			},
			{
				219,
				Controller.dPadLeft
			},
			{
				220,
				Controller.dPadRight
			},
			{
				198,
				Controller.startButton
			},
			{
				199,
				Controller.selectButton
			},
			{
				333,
				Controller.faceButtonSouth
			},
			{
				334,
				Controller.faceButtonEast
			},
			{
				335,
				Controller.faceButtonWest
			},
			{
				336,
				Controller.faceButtonNorth
			},
			{
				337,
				Controller.leftBumper
			},
			{
				338,
				Controller.rightBumper
			},
			{
				356,
				Controller.leftTrigger
			},
			{
				358,
				Controller.rightTrigger
			},
			{
				360,
				Controller.lStickPress
			},
			{
				367,
				Controller.lStickPress
			},
			{
				378,
				Controller.dPadUp
			},
			{
				379,
				Controller.dPadDown
			},
			{
				380,
				Controller.dPadLeft
			},
			{
				381,
				Controller.dPadRight
			},
			{
				339,
				Controller.startButton
			},
			{
				340,
				Controller.selectButton
			}
		};

		// Token: 0x04002F45 RID: 12101
		private InputHandle_t? _currentControllerHandle;

		// Token: 0x04002F46 RID: 12102
		private InputActionSetHandle_t? _currentActionSetHandle;

		// Token: 0x04002F47 RID: 12103
		private ESteamInputType? _currentInputType;

		// Token: 0x04002F48 RID: 12104
		[Nullable(2)]
		private ControllerConfig _controllerConfig;

		// Token: 0x04002F49 RID: 12105
		private readonly List<string> _pressedInputs = new List<string>();

		// Token: 0x04002F4A RID: 12106
		private readonly Dictionary<StringName, InputEventAction> _inputEvents = new Dictionary<StringName, InputEventAction>();

		// Token: 0x04002F4B RID: 12107
		private readonly IControllerInputStrategy _fallbackStrategy = new GodotControllerInputStrategy();

		// Token: 0x04002F4C RID: 12108
		private double _nextControllerCheckTime;

		// Token: 0x04002F4D RID: 12109
		private readonly Dictionary<StringName, InputDigitalActionHandle_t> _digitalActionHandleCache = new Dictionary<StringName, InputDigitalActionHandle_t>();

		// Token: 0x04002F4E RID: 12110
		private bool _attemptedHandleCacheRebuild;

		// Token: 0x04002F4F RID: 12111
		private Dictionary<EInputActionOrigin, Texture2D> _fallbackSteamGlyphs = new Dictionary<EInputActionOrigin, Texture2D>();

		// Token: 0x04002F50 RID: 12112
		private InputAnalogActionHandle_t _joystickActionHandle;

		// Token: 0x04002F51 RID: 12113
		private Vector2 _lStickPosition;

		// Token: 0x04002F52 RID: 12114
		private InputEventJoypadMotion _joystickXAxis;

		// Token: 0x04002F53 RID: 12115
		private InputEventJoypadMotion _joystickYAxis;

		// Token: 0x04002F54 RID: 12116
		private StringName _up;

		// Token: 0x04002F55 RID: 12117
		private StringName _down;

		// Token: 0x04002F56 RID: 12118
		private StringName _left;

		// Token: 0x04002F57 RID: 12119
		private StringName _right;
	}
}
