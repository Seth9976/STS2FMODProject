using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using Godot;
using MegaCrit.Sts2.Core.ControllerInput.ControllerConfigs;
using MegaCrit.Sts2.Core.Helpers;

namespace MegaCrit.Sts2.Core.ControllerInput
{
	/// <summary>
	/// Maps controller specific behavior for different types of controllers.
	/// - used to override certain default controller inputs based on the controller types (ie, ps5 controllers use the touchpad to open the map)
	/// - Used to dynamically update the hotkey icons depending on what type of controller you are using.
	/// </summary>
	// Token: 0x02000D1D RID: 3357
	[NullableContext(1)]
	[Nullable(0)]
	public abstract class ControllerConfig
	{
		// Token: 0x17002544 RID: 9540
		// (get) Token: 0x06008762 RID: 34658
		protected abstract string FolderPath { get; }

		// Token: 0x17002545 RID: 9541
		// (get) Token: 0x06008763 RID: 34659 RVA: 0x002D8C11 File Offset: 0x002D6E11
		public virtual ControllerMappingType ControllerMappingType
		{
			get
			{
				return ControllerMappingType.Default;
			}
		}

		/// <summary>
		/// The mapping between Steam Input Actions (defined in the vdf file), and the default controller input they
		/// correspond to.
		/// </summary>
		// Token: 0x17002546 RID: 9542
		// (get) Token: 0x06008764 RID: 34660 RVA: 0x002D8C14 File Offset: 0x002D6E14
		public virtual Dictionary<string, StringName> SteamInputControllerMap
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
						Controller.selectButton
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

		/// <summary>
		/// The default map between controller inputs and specific in game commands.
		/// We have this because inputs can vary between controller types (ex: On ps4 controllers, we use the touchpad instead of
		/// the back/share button to open the map.
		/// </summary>
		// Token: 0x17002547 RID: 9543
		// (get) Token: 0x06008765 RID: 34661 RVA: 0x002D8D18 File Offset: 0x002D6F18
		public virtual Dictionary<StringName, StringName> DefaultControllerInputMap
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
						Controller.selectButton
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

		// Token: 0x17002548 RID: 9544
		// (get) Token: 0x06008766 RID: 34662 RVA: 0x002D8E5A File Offset: 0x002D705A
		protected virtual string FaceButtonNorthGlyph
		{
			get
			{
				return ImageHelper.GetImagePath(this.FolderPath + "/y.tres");
			}
		}

		// Token: 0x17002549 RID: 9545
		// (get) Token: 0x06008767 RID: 34663 RVA: 0x002D8E71 File Offset: 0x002D7071
		protected virtual string FaceButtonSouthGlyph
		{
			get
			{
				return ImageHelper.GetImagePath(this.FolderPath + "/a.tres");
			}
		}

		// Token: 0x1700254A RID: 9546
		// (get) Token: 0x06008768 RID: 34664 RVA: 0x002D8E88 File Offset: 0x002D7088
		protected virtual string FaceButtonEastGlyph
		{
			get
			{
				return ImageHelper.GetImagePath(this.FolderPath + "/b.tres");
			}
		}

		// Token: 0x1700254B RID: 9547
		// (get) Token: 0x06008769 RID: 34665 RVA: 0x002D8E9F File Offset: 0x002D709F
		protected virtual string FaceButtonWestGlyph
		{
			get
			{
				return ImageHelper.GetImagePath(this.FolderPath + "/x.tres");
			}
		}

		// Token: 0x1700254C RID: 9548
		// (get) Token: 0x0600876A RID: 34666 RVA: 0x002D8EB6 File Offset: 0x002D70B6
		private string LeftTriggerGlyph
		{
			get
			{
				return ImageHelper.GetImagePath(this.FolderPath + "/lt.tres");
			}
		}

		// Token: 0x1700254D RID: 9549
		// (get) Token: 0x0600876B RID: 34667 RVA: 0x002D8ECD File Offset: 0x002D70CD
		private string RightTriggerGlyph
		{
			get
			{
				return ImageHelper.GetImagePath(this.FolderPath + "/rt.tres");
			}
		}

		// Token: 0x1700254E RID: 9550
		// (get) Token: 0x0600876C RID: 34668 RVA: 0x002D8EE4 File Offset: 0x002D70E4
		private string SelectButtonGlyph
		{
			get
			{
				return ImageHelper.GetImagePath(this.FolderPath + "/back.tres");
			}
		}

		// Token: 0x1700254F RID: 9551
		// (get) Token: 0x0600876D RID: 34669 RVA: 0x002D8EFB File Offset: 0x002D70FB
		private string LeftBumperGlyph
		{
			get
			{
				return ImageHelper.GetImagePath(this.FolderPath + "/lb.tres");
			}
		}

		// Token: 0x17002550 RID: 9552
		// (get) Token: 0x0600876E RID: 34670 RVA: 0x002D8F12 File Offset: 0x002D7112
		private string RightBumperGlyph
		{
			get
			{
				return ImageHelper.GetImagePath(this.FolderPath + "/rb.tres");
			}
		}

		// Token: 0x17002551 RID: 9553
		// (get) Token: 0x0600876F RID: 34671 RVA: 0x002D8F29 File Offset: 0x002D7129
		private string StartButtonGlyph
		{
			get
			{
				return ImageHelper.GetImagePath(this.FolderPath + "/start.tres");
			}
		}

		// Token: 0x17002552 RID: 9554
		// (get) Token: 0x06008770 RID: 34672 RVA: 0x002D8F40 File Offset: 0x002D7140
		private string JoystickPressGlyph
		{
			get
			{
				return ImageHelper.GetImagePath(this.FolderPath + "/ls.tres");
			}
		}

		// Token: 0x17002553 RID: 9555
		// (get) Token: 0x06008771 RID: 34673 RVA: 0x002D8F57 File Offset: 0x002D7157
		private string DPadNorth
		{
			get
			{
				return ImageHelper.GetImagePath(this.FolderPath + "/up.tres");
			}
		}

		// Token: 0x17002554 RID: 9556
		// (get) Token: 0x06008772 RID: 34674 RVA: 0x002D8F6E File Offset: 0x002D716E
		private string DPadSouth
		{
			get
			{
				return ImageHelper.GetImagePath(this.FolderPath + "/down.tres");
			}
		}

		// Token: 0x17002555 RID: 9557
		// (get) Token: 0x06008773 RID: 34675 RVA: 0x002D8F85 File Offset: 0x002D7185
		private string DPadWest
		{
			get
			{
				return ImageHelper.GetImagePath(this.FolderPath + "/left.tres");
			}
		}

		// Token: 0x17002556 RID: 9558
		// (get) Token: 0x06008774 RID: 34676 RVA: 0x002D8F9C File Offset: 0x002D719C
		private string DPadEast
		{
			get
			{
				return ImageHelper.GetImagePath(this.FolderPath + "/right.tres");
			}
		}

		// Token: 0x17002557 RID: 9559
		// (get) Token: 0x06008775 RID: 34677 RVA: 0x002D8FB3 File Offset: 0x002D71B3
		private string Ps4Touchpad
		{
			get
			{
				return ImageHelper.GetImagePath(this.FolderPath + "/touchpad.tres");
			}
		}

		// Token: 0x17002558 RID: 9560
		// (get) Token: 0x06008776 RID: 34678 RVA: 0x002D8FCA File Offset: 0x002D71CA
		private string RightJoystickNorth
		{
			get
			{
				return ImageHelper.GetImagePath(this.FolderPath + "/rs_up.tres");
			}
		}

		// Token: 0x17002559 RID: 9561
		// (get) Token: 0x06008777 RID: 34679 RVA: 0x002D8FE1 File Offset: 0x002D71E1
		private string RightJoystickSouth
		{
			get
			{
				return ImageHelper.GetImagePath(this.FolderPath + "/rs_down.tres");
			}
		}

		// Token: 0x1700255A RID: 9562
		// (get) Token: 0x06008778 RID: 34680 RVA: 0x002D8FF8 File Offset: 0x002D71F8
		private string RightJoystickEast
		{
			get
			{
				return ImageHelper.GetImagePath(this.FolderPath + "/rs_right.tres");
			}
		}

		// Token: 0x1700255B RID: 9563
		// (get) Token: 0x06008779 RID: 34681 RVA: 0x002D900F File Offset: 0x002D720F
		private string RightJoystickWest
		{
			get
			{
				return ImageHelper.GetImagePath(this.FolderPath + "/rs_left.tres");
			}
		}

		// Token: 0x1700255C RID: 9564
		// (get) Token: 0x0600877A RID: 34682 RVA: 0x002D9028 File Offset: 0x002D7228
		private Dictionary<string, string> GlyphMap
		{
			get
			{
				if (this._glyphs == null)
				{
					this._glyphs = new Dictionary<string, string>
					{
						{
							Controller.faceButtonNorth,
							this.FaceButtonNorthGlyph
						},
						{
							Controller.faceButtonSouth,
							this.FaceButtonSouthGlyph
						},
						{
							Controller.faceButtonEast,
							this.FaceButtonEastGlyph
						},
						{
							Controller.faceButtonWest,
							this.FaceButtonWestGlyph
						},
						{
							Controller.leftTrigger,
							this.LeftTriggerGlyph
						},
						{
							Controller.rightTrigger,
							this.RightTriggerGlyph
						},
						{
							Controller.leftBumper,
							this.LeftBumperGlyph
						},
						{
							Controller.rightBumper,
							this.RightBumperGlyph
						},
						{
							Controller.selectButton,
							this.SelectButtonGlyph
						},
						{
							Controller.startButton,
							this.StartButtonGlyph
						},
						{
							Controller.lStickPress,
							this.JoystickPressGlyph
						},
						{
							Controller.dPadUp,
							this.DPadNorth
						},
						{
							Controller.dPadDown,
							this.DPadSouth
						},
						{
							Controller.dPadRight,
							this.DPadEast
						},
						{
							Controller.dPadLeft,
							this.DPadWest
						},
						{
							Controller.ps4Touchpad,
							this.Ps4Touchpad
						},
						{
							Controller.rStickUp,
							this.RightJoystickNorth
						},
						{
							Controller.rStickDown,
							this.RightJoystickSouth
						},
						{
							Controller.rStickLeft,
							this.RightJoystickWest
						},
						{
							Controller.rStickRight,
							this.RightJoystickEast
						}
					};
				}
				return this._glyphs;
			}
		}

		// Token: 0x0600877B RID: 34683 RVA: 0x002D920C File Offset: 0x002D740C
		[return: Nullable(2)]
		public Texture2D GetButtonIcon(string button)
		{
			string text;
			if (this.GlyphMap.TryGetValue(button, out text))
			{
				return ResourceLoader.Load<Texture2D>(text, null, 1L);
			}
			return null;
		}

		// Token: 0x1700255D RID: 9565
		// (get) Token: 0x0600877C RID: 34684 RVA: 0x002D9234 File Offset: 0x002D7434
		public IEnumerable<string> AssetPaths
		{
			get
			{
				return this.GlyphMap.Values.Where<string>((string path) => ResourceLoader.Exists(path, ""));
			}
		}

		// Token: 0x1700255E RID: 9566
		// (get) Token: 0x0600877D RID: 34685 RVA: 0x002D9268 File Offset: 0x002D7468
		public static IEnumerable<string> AllAssetPaths
		{
			get
			{
				return new ControllerConfig[]
				{
					new SteamControllerConfig(),
					new Ps4Config(),
					new Xbox360Config(),
					new XboxOneConfig()
				}.SelectMany<ControllerConfig, string>((ControllerConfig c) => c.AssetPaths);
			}
		}

		// Token: 0x04002F13 RID: 12051
		[Nullable(new byte[] { 2, 1, 1 })]
		private Dictionary<string, string> _glyphs;
	}
}
