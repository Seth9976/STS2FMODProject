using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Godot;
using MegaCrit.Sts2.Core.ControllerInput.ControllerConfigs;
using MegaCrit.Sts2.Core.Nodes.CommonUi;

namespace MegaCrit.Sts2.Core.ControllerInput
{
	// Token: 0x02000D1F RID: 3359
	[NullableContext(1)]
	[Nullable(0)]
	public class GodotControllerInputStrategy : IControllerInputStrategy
	{
		// Token: 0x1700255F RID: 9567
		// (get) Token: 0x0600877F RID: 34687 RVA: 0x002D92C7 File Offset: 0x002D74C7
		[Nullable(2)]
		public ControllerConfig ControllerConfig
		{
			[NullableContext(2)]
			get
			{
				if (this._controllerConfig == null)
				{
					this.UpdateControllerConfig();
				}
				return this._controllerConfig;
			}
		}

		// Token: 0x06008780 RID: 34688 RVA: 0x002D92DD File Offset: 0x002D74DD
		public Task Init()
		{
			this.UpdateControllerConfig();
			return Task.FromResult<bool>(true);
		}

		// Token: 0x06008781 RID: 34689 RVA: 0x002D92EC File Offset: 0x002D74EC
		public void ProcessInput()
		{
			foreach (StringName stringName in Controller.AllControllerInputs)
			{
				if (Input.IsActionJustPressed(stringName, false))
				{
					this.UpdateControllerConfig();
				}
			}
			foreach (KeyValuePair<StringName, StringName[]> keyValuePair in this._analogToDigitalInput)
			{
				if (Input.IsActionJustPressed(keyValuePair.Key, false))
				{
					foreach (StringName stringName2 in keyValuePair.Value)
					{
						InputEventAction inputEventAction = new InputEventAction
						{
							Action = stringName2,
							Pressed = true
						};
						Input.ParseInputEvent(inputEventAction);
					}
				}
				else if (Input.IsActionJustReleased(keyValuePair.Key, false))
				{
					foreach (StringName stringName3 in keyValuePair.Value)
					{
						InputEventAction inputEventAction2 = new InputEventAction
						{
							Action = stringName3,
							Pressed = false
						};
						Input.ParseInputEvent(inputEventAction2);
					}
				}
			}
		}

		// Token: 0x06008782 RID: 34690 RVA: 0x002D9408 File Offset: 0x002D7608
		private void UpdateControllerConfig()
		{
			if (Input.GetConnectedJoypads().Count == 0)
			{
				this._controllerConfig = new SteamControllerConfig();
				return;
			}
			string joyName = Input.GetJoyName(0);
			if (joyName == this._currentControllerType)
			{
				return;
			}
			this._currentControllerType = joyName;
			if (this._currentControllerType.Contains("Xbox One") || this._currentControllerType.Contains("XInput"))
			{
				this._controllerConfig = new XboxOneConfig();
			}
			else if (this._currentControllerType.Contains("Xbox 360"))
			{
				this._controllerConfig = new Xbox360Config();
			}
			else if (this._currentControllerType.Contains("PS3"))
			{
				this._controllerConfig = new Ps4Config();
			}
			else if (this._currentControllerType.Contains("PS4") || this._currentControllerType.Contains("DualSense"))
			{
				this._controllerConfig = new Ps4Config();
			}
			else if (this._currentControllerType.Contains("PS5"))
			{
				this._controllerConfig = new Ps4Config();
			}
			else if (this._currentControllerType.Contains("Switch"))
			{
				this._controllerConfig = new SwitchConfig();
			}
			else
			{
				this._controllerConfig = new SteamControllerConfig();
			}
			NControllerManager instance = NControllerManager.Instance;
			if (instance == null)
			{
				return;
			}
			instance.OnControllerTypeChanged();
		}

		// Token: 0x06008783 RID: 34691 RVA: 0x002D9548 File Offset: 0x002D7748
		[return: Nullable(2)]
		public Texture2D GetHotkeyIcon(string hotkey)
		{
			ControllerConfig controllerConfig = this._controllerConfig;
			if (controllerConfig == null)
			{
				return null;
			}
			return controllerConfig.GetButtonIcon(hotkey);
		}

		// Token: 0x17002560 RID: 9568
		// (get) Token: 0x06008784 RID: 34692 RVA: 0x002D955C File Offset: 0x002D775C
		public Dictionary<StringName, StringName> GetDefaultControllerInputMap
		{
			get
			{
				if (this.ControllerConfig == null)
				{
					this.UpdateControllerConfig();
				}
				return this.ControllerConfig.DefaultControllerInputMap;
			}
		}

		// Token: 0x06008785 RID: 34693 RVA: 0x002D9577 File Offset: 0x002D7777
		public string GetControllerName()
		{
			if (Input.GetConnectedJoypads().Count == 0)
			{
				return "NONE";
			}
			return Input.GetJoyName(0);
		}

		// Token: 0x06008786 RID: 34694 RVA: 0x002D9591 File Offset: 0x002D7791
		public Vector2 GetLeftAnalogStickDirection()
		{
			return Input.GetVector(GodotControllerInputStrategy._lStickLeftRaw, GodotControllerInputStrategy._lStickRightRaw, GodotControllerInputStrategy._lStickUpRaw, GodotControllerInputStrategy._lStickDownRaw, -1f);
		}

		// Token: 0x17002561 RID: 9569
		// (get) Token: 0x06008787 RID: 34695 RVA: 0x002D95B1 File Offset: 0x002D77B1
		public bool ShouldAllowControllerRebinding
		{
			get
			{
				return true;
			}
		}

		// Token: 0x04002F18 RID: 12056
		private static readonly StringName _rStickLeftRaw = "raw_r_stick_left";

		// Token: 0x04002F19 RID: 12057
		private static readonly StringName _rStickRightRaw = "raw_r_stick_right";

		// Token: 0x04002F1A RID: 12058
		private static readonly StringName _rStickUpRaw = "raw_r_stick_up";

		// Token: 0x04002F1B RID: 12059
		private static readonly StringName _rStickDownRaw = "raw_r_stick_down";

		// Token: 0x04002F1C RID: 12060
		private static readonly StringName _lStickLeftRaw = "raw_l_stick_left";

		// Token: 0x04002F1D RID: 12061
		private static readonly StringName _lStickRightRaw = "raw_l_stick_right";

		// Token: 0x04002F1E RID: 12062
		private static readonly StringName _lStickUpRaw = "raw_l_stick_up";

		// Token: 0x04002F1F RID: 12063
		private static readonly StringName _lStickDownRaw = "raw_l_stick_down";

		// Token: 0x04002F20 RID: 12064
		private static readonly StringName _leftTriggerRaw = "raw_left_trigger";

		// Token: 0x04002F21 RID: 12065
		private static readonly StringName _rightTriggerRaw = "raw_right_trigger";

		/// <summary>
		/// Maps analog inputs with pressure sensitive input values (i.e. stick axis being a value between 0.0 and 1.0)
		/// and flattens it out to "isPressed" and "isReleased". This is important multiple events do not fire
		/// as that analog value changes.
		/// </summary>
		// Token: 0x04002F22 RID: 12066
		private readonly Dictionary<StringName, StringName[]> _analogToDigitalInput = new Dictionary<StringName, StringName[]>
		{
			{
				GodotControllerInputStrategy._rStickUpRaw,
				new StringName[] { Controller.rStickUp }
			},
			{
				GodotControllerInputStrategy._rStickDownRaw,
				new StringName[] { Controller.rStickDown }
			},
			{
				GodotControllerInputStrategy._rStickLeftRaw,
				new StringName[] { Controller.rStickLeft }
			},
			{
				GodotControllerInputStrategy._rStickRightRaw,
				new StringName[] { Controller.rStickRight }
			},
			{
				GodotControllerInputStrategy._leftTriggerRaw,
				new StringName[] { Controller.leftTrigger }
			},
			{
				GodotControllerInputStrategy._rightTriggerRaw,
				new StringName[] { Controller.rightTrigger }
			},
			{
				GodotControllerInputStrategy._lStickUpRaw,
				new StringName[]
				{
					Controller.dPadUp,
					Controller.lStickUp
				}
			},
			{
				GodotControllerInputStrategy._lStickDownRaw,
				new StringName[]
				{
					Controller.dPadDown,
					Controller.lStickDown
				}
			},
			{
				GodotControllerInputStrategy._lStickLeftRaw,
				new StringName[]
				{
					Controller.dPadLeft,
					Controller.lStickLeft
				}
			},
			{
				GodotControllerInputStrategy._lStickRightRaw,
				new StringName[]
				{
					Controller.dPadRight,
					Controller.lStickRight
				}
			}
		};

		// Token: 0x04002F23 RID: 12067
		[Nullable(2)]
		private string _currentControllerType;

		// Token: 0x04002F24 RID: 12068
		[Nullable(2)]
		private ControllerConfig _controllerConfig;
	}
}
