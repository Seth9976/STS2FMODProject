using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Godot;

namespace MegaCrit.Sts2.Core.ControllerInput.ControllerConfigs
{
	// Token: 0x02000D23 RID: 3363
	[NullableContext(1)]
	[Nullable(0)]
	public class Ps4Config : ControllerConfig
	{
		// Token: 0x17002569 RID: 9577
		// (get) Token: 0x060087A2 RID: 34722 RVA: 0x002DADB5 File Offset: 0x002D8FB5
		protected override string FolderPath
		{
			get
			{
				return "atlases/controller_atlas.sprites/ps4";
			}
		}

		// Token: 0x1700256A RID: 9578
		// (get) Token: 0x060087A3 RID: 34723 RVA: 0x002DADBC File Offset: 0x002D8FBC
		public override ControllerMappingType ControllerMappingType
		{
			get
			{
				return ControllerMappingType.Playstation;
			}
		}

		// Token: 0x1700256B RID: 9579
		// (get) Token: 0x060087A4 RID: 34724 RVA: 0x002DADC0 File Offset: 0x002D8FC0
		public override Dictionary<string, StringName> SteamInputControllerMap
		{
			get
			{
				return new Dictionary<string, StringName>
				{
					{
						"Confirm",
						Controller.faceButtonNorth
					},
					{
						"Cancel",
						Controller.faceButtonEast
					},
					{
						"Up",
						Controller.dPadUp
					},
					{
						"Down",
						Controller.dPadDown
					},
					{
						"Left",
						Controller.dPadLeft
					},
					{
						"Right",
						Controller.dPadRight
					},
					{
						"Select",
						Controller.faceButtonSouth
					},
					{
						"Top_Panel",
						Controller.faceButtonWest
					},
					{
						"View_Draw_Pile",
						Controller.leftTrigger
					},
					{
						"View_Discard_Pile",
						Controller.rightTrigger
					},
					{
						"Tab_Right",
						Controller.rightBumper
					},
					{
						"Tab_Left",
						Controller.leftBumper
					},
					{
						"View_Map",
						Controller.ps4Touchpad
					},
					{
						"Settings",
						Controller.startButton
					},
					{
						"Peek",
						Controller.lStickPress
					}
				};
			}
		}

		// Token: 0x1700256C RID: 9580
		// (get) Token: 0x060087A5 RID: 34725 RVA: 0x002DAEC4 File Offset: 0x002D90C4
		public override Dictionary<StringName, StringName> DefaultControllerInputMap
		{
			get
			{
				return new Dictionary<StringName, StringName>
				{
					{
						MegaInput.accept,
						Controller.faceButtonNorth
					},
					{
						MegaInput.cancel,
						Controller.faceButtonEast
					},
					{
						MegaInput.select,
						Controller.faceButtonSouth
					},
					{
						MegaInput.viewExhaustPileAndTabRight,
						Controller.rightBumper
					},
					{
						MegaInput.viewDeckAndTabLeft,
						Controller.leftBumper
					},
					{
						MegaInput.topPanel,
						Controller.faceButtonWest
					},
					{
						MegaInput.viewDrawPile,
						Controller.leftTrigger
					},
					{
						MegaInput.viewDiscardPile,
						Controller.rightTrigger
					},
					{
						MegaInput.viewMap,
						Controller.ps4Touchpad
					},
					{
						MegaInput.peek,
						Controller.lStickPress
					},
					{
						MegaInput.up,
						Controller.dPadUp
					},
					{
						MegaInput.down,
						Controller.dPadDown
					},
					{
						MegaInput.left,
						Controller.dPadLeft
					},
					{
						MegaInput.right,
						Controller.dPadRight
					},
					{
						MegaInput.altUp,
						Controller.rStickUp
					},
					{
						MegaInput.altDown,
						Controller.rStickDown
					},
					{
						MegaInput.altLeft,
						Controller.rStickLeft
					},
					{
						MegaInput.altRight,
						Controller.rStickRight
					},
					{
						MegaInput.pauseAndBack,
						Controller.startButton
					}
				};
			}
		}
	}
}
