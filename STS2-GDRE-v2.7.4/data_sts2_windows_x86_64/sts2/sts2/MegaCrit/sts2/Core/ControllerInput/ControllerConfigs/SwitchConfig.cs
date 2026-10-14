using System;
using System.Runtime.CompilerServices;
using MegaCrit.Sts2.Core.Helpers;

namespace MegaCrit.Sts2.Core.ControllerInput.ControllerConfigs
{
	// Token: 0x02000D25 RID: 3365
	[NullableContext(1)]
	[Nullable(0)]
	public class SwitchConfig : ControllerConfig
	{
		// Token: 0x1700256E RID: 9582
		// (get) Token: 0x060087A9 RID: 34729 RVA: 0x002DB01D File Offset: 0x002D921D
		protected override string FolderPath
		{
			get
			{
				return "atlases/controller_atlas.sprites/switch";
			}
		}

		// Token: 0x1700256F RID: 9583
		// (get) Token: 0x060087AA RID: 34730 RVA: 0x002DB024 File Offset: 0x002D9224
		public override ControllerMappingType ControllerMappingType
		{
			get
			{
				return ControllerMappingType.NintendoSwitch;
			}
		}

		// Token: 0x17002570 RID: 9584
		// (get) Token: 0x060087AB RID: 34731 RVA: 0x002DB027 File Offset: 0x002D9227
		protected override string FaceButtonSouthGlyph
		{
			get
			{
				return ImageHelper.GetImagePath(this.FolderPath + "/b.tres");
			}
		}

		// Token: 0x17002571 RID: 9585
		// (get) Token: 0x060087AC RID: 34732 RVA: 0x002DB03E File Offset: 0x002D923E
		protected override string FaceButtonEastGlyph
		{
			get
			{
				return ImageHelper.GetImagePath(this.FolderPath + "/a.tres");
			}
		}

		// Token: 0x17002572 RID: 9586
		// (get) Token: 0x060087AD RID: 34733 RVA: 0x002DB055 File Offset: 0x002D9255
		protected override string FaceButtonNorthGlyph
		{
			get
			{
				return ImageHelper.GetImagePath(this.FolderPath + "/x.tres");
			}
		}

		// Token: 0x17002573 RID: 9587
		// (get) Token: 0x060087AE RID: 34734 RVA: 0x002DB06C File Offset: 0x002D926C
		protected override string FaceButtonWestGlyph
		{
			get
			{
				return ImageHelper.GetImagePath(this.FolderPath + "/y.tres");
			}
		}
	}
}
