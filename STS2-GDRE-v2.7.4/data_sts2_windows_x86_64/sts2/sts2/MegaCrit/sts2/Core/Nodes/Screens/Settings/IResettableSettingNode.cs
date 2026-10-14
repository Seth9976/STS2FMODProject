using System;

namespace MegaCrit.Sts2.Core.Nodes.Screens.Settings
{
	// Token: 0x02000283 RID: 643
	public interface IResettableSettingNode
	{
		/// <summary>
		/// Ensures that the state of the node reflects SettingsSave/PrefsSave.
		/// For example, if we change SettingsSave.FastMode, then this should be called on NFastModeTickbox to ensure it
		/// is ticked/unticked appropriately.
		/// </summary>
		// Token: 0x06002258 RID: 8792
		void SetFromSettings();
	}
}
