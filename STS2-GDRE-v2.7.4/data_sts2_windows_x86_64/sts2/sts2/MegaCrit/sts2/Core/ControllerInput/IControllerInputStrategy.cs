using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Godot;

namespace MegaCrit.Sts2.Core.ControllerInput
{
	// Token: 0x02000D20 RID: 3360
	[NullableContext(1)]
	public interface IControllerInputStrategy
	{
		// Token: 0x0600878A RID: 34698
		Task Init();

		// Token: 0x0600878B RID: 34699
		void ProcessInput();

		// Token: 0x0600878C RID: 34700
		[return: Nullable(2)]
		Texture2D GetHotkeyIcon(string hotkey);

		// Token: 0x17002562 RID: 9570
		// (get) Token: 0x0600878D RID: 34701
		[Nullable(2)]
		ControllerConfig ControllerConfig
		{
			[NullableContext(2)]
			get;
		}

		// Token: 0x17002563 RID: 9571
		// (get) Token: 0x0600878E RID: 34702
		Dictionary<StringName, StringName> GetDefaultControllerInputMap { get; }

		// Token: 0x0600878F RID: 34703
		string GetControllerName();

		// Token: 0x17002564 RID: 9572
		// (get) Token: 0x06008790 RID: 34704
		bool ShouldAllowControllerRebinding { get; }

		// Token: 0x06008791 RID: 34705
		Vector2 GetLeftAnalogStickDirection();
	}
}
