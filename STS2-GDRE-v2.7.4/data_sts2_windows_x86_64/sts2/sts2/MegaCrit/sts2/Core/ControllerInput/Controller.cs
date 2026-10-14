using System;
using System.Runtime.CompilerServices;
using Godot;

namespace MegaCrit.Sts2.Core.ControllerInput
{
	// Token: 0x02000D1C RID: 3356
	[NullableContext(1)]
	[Nullable(0)]
	public static class Controller
	{
		// Token: 0x17002543 RID: 9539
		// (get) Token: 0x06008760 RID: 34656 RVA: 0x002D89B8 File Offset: 0x002D6BB8
		public static StringName[] AllControllerInputs
		{
			get
			{
				return new StringName[]
				{
					Controller.dPadRight,
					Controller.dPadUp,
					Controller.dPadDown,
					Controller.dPadLeft,
					Controller.faceButtonEast,
					Controller.faceButtonNorth,
					Controller.faceButtonSouth,
					Controller.faceButtonWest,
					Controller.lStickDown,
					Controller.lStickLeft,
					Controller.lStickPress,
					Controller.lStickRight,
					Controller.lStickUp,
					Controller.leftBumper,
					Controller.leftTrigger,
					Controller.rightBumper,
					Controller.rightTrigger,
					Controller.selectButton,
					Controller.startButton,
					Controller.ps4Touchpad,
					Controller.rStickLeft,
					Controller.rStickRight,
					Controller.rStickUp,
					Controller.rStickDown
				};
			}
		}

		// Token: 0x04002EFB RID: 12027
		public static readonly StringName leftTrigger = "controller_left_trigger";

		// Token: 0x04002EFC RID: 12028
		public static readonly StringName rightTrigger = "controller_right_trigger";

		// Token: 0x04002EFD RID: 12029
		public static readonly StringName leftBumper = "controller_left_bumper";

		// Token: 0x04002EFE RID: 12030
		public static readonly StringName rightBumper = "controller_right_bumper";

		// Token: 0x04002EFF RID: 12031
		public static readonly StringName faceButtonNorth = "controller_face_button_north";

		// Token: 0x04002F00 RID: 12032
		public static readonly StringName faceButtonSouth = "controller_face_button_south";

		// Token: 0x04002F01 RID: 12033
		public static readonly StringName faceButtonEast = "controller_face_button_east";

		// Token: 0x04002F02 RID: 12034
		public static readonly StringName faceButtonWest = "controller_face_button_west";

		// Token: 0x04002F03 RID: 12035
		public static readonly StringName startButton = "controller_start_button";

		// Token: 0x04002F04 RID: 12036
		public static readonly StringName selectButton = "controller_select_button";

		// Token: 0x04002F05 RID: 12037
		public static readonly StringName dPadUp = "controller_d_pad_up";

		// Token: 0x04002F06 RID: 12038
		public static readonly StringName dPadDown = "controller_d_pad_down";

		// Token: 0x04002F07 RID: 12039
		public static readonly StringName dPadLeft = "controller_d_pad_left";

		// Token: 0x04002F08 RID: 12040
		public static readonly StringName dPadRight = "controller_d_pad_right";

		// Token: 0x04002F09 RID: 12041
		public static readonly StringName lStickPress = "controller_l_stick_press";

		// Token: 0x04002F0A RID: 12042
		public static readonly StringName lStickLeft = "controller_l_stick_left";

		// Token: 0x04002F0B RID: 12043
		public static readonly StringName lStickRight = "controller_l_stick_right";

		// Token: 0x04002F0C RID: 12044
		public static readonly StringName lStickUp = "controller_l_stick_up";

		// Token: 0x04002F0D RID: 12045
		public static readonly StringName lStickDown = "controller_l_stick_down";

		// Token: 0x04002F0E RID: 12046
		public static readonly StringName rStickLeft = "controller_r_stick_left";

		// Token: 0x04002F0F RID: 12047
		public static readonly StringName rStickRight = "controller_r_stick_right";

		// Token: 0x04002F10 RID: 12048
		public static readonly StringName rStickUp = "controller_r_stick_up";

		// Token: 0x04002F11 RID: 12049
		public static readonly StringName rStickDown = "controller_r_stick_down";

		// Token: 0x04002F12 RID: 12050
		public static readonly StringName ps4Touchpad = "ui_controller_touch_pad";
	}
}
