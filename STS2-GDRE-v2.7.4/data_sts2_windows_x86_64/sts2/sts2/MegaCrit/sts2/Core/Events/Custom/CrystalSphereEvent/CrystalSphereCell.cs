using System;
using System.Runtime.CompilerServices;

namespace MegaCrit.Sts2.Core.Events.Custom.CrystalSphereEvent
{
	/// <summary>
	/// Represents a cell on the grid for the Crystal SphereMinigame
	/// </summary>
	// Token: 0x02000C4E RID: 3150
	[NullableContext(2)]
	[Nullable(0)]
	public class CrystalSphereCell
	{
		// Token: 0x1700236B RID: 9067
		// (get) Token: 0x060081C7 RID: 33223 RVA: 0x002BF7CB File Offset: 0x002BD9CB
		// (set) Token: 0x060081C8 RID: 33224 RVA: 0x002BF7D3 File Offset: 0x002BD9D3
		public CrystalSphereItem Item { get; private set; }

		// Token: 0x1700236C RID: 9068
		// (get) Token: 0x060081C9 RID: 33225 RVA: 0x002BF7DC File Offset: 0x002BD9DC
		// (set) Token: 0x060081CA RID: 33226 RVA: 0x002BF7E4 File Offset: 0x002BD9E4
		public int X { get; private set; }

		// Token: 0x1700236D RID: 9069
		// (get) Token: 0x060081CB RID: 33227 RVA: 0x002BF7ED File Offset: 0x002BD9ED
		// (set) Token: 0x060081CC RID: 33228 RVA: 0x002BF7F5 File Offset: 0x002BD9F5
		public int Y { get; private set; }

		/// <summary>
		/// If true, then this cell is still obscured by fog.
		/// </summary>
		// Token: 0x1700236E RID: 9070
		// (get) Token: 0x060081CD RID: 33229 RVA: 0x002BF7FE File Offset: 0x002BD9FE
		// (set) Token: 0x060081CE RID: 33230 RVA: 0x002BF806 File Offset: 0x002BDA06
		public bool IsHidden
		{
			get
			{
				return this._isHidden;
			}
			set
			{
				this._isHidden = value;
				Action fogUpdated = this.FogUpdated;
				if (fogUpdated == null)
				{
					return;
				}
				fogUpdated();
			}
		}

		// Token: 0x140000AD RID: 173
		// (add) Token: 0x060081CF RID: 33231 RVA: 0x002BF820 File Offset: 0x002BDA20
		// (remove) Token: 0x060081D0 RID: 33232 RVA: 0x002BF858 File Offset: 0x002BDA58
		public event Action FogUpdated;

		// Token: 0x1700236F RID: 9071
		// (get) Token: 0x060081D1 RID: 33233 RVA: 0x002BF88D File Offset: 0x002BDA8D
		// (set) Token: 0x060081D2 RID: 33234 RVA: 0x002BF895 File Offset: 0x002BDA95
		public bool IsHighlighted
		{
			get
			{
				return this._isHighlighted;
			}
			set
			{
				this._isHighlighted = value;
				Action highlightUpdated = this.HighlightUpdated;
				if (highlightUpdated == null)
				{
					return;
				}
				highlightUpdated();
			}
		}

		// Token: 0x17002370 RID: 9072
		// (get) Token: 0x060081D3 RID: 33235 RVA: 0x002BF8AE File Offset: 0x002BDAAE
		// (set) Token: 0x060081D4 RID: 33236 RVA: 0x002BF8B6 File Offset: 0x002BDAB6
		public bool IsHovered
		{
			get
			{
				return this._isHovered;
			}
			set
			{
				this._isHovered = value;
				Action highlightUpdated = this.HighlightUpdated;
				if (highlightUpdated == null)
				{
					return;
				}
				highlightUpdated();
			}
		}

		// Token: 0x140000AE RID: 174
		// (add) Token: 0x060081D5 RID: 33237 RVA: 0x002BF8D0 File Offset: 0x002BDAD0
		// (remove) Token: 0x060081D6 RID: 33238 RVA: 0x002BF908 File Offset: 0x002BDB08
		public event Action HighlightUpdated;

		// Token: 0x060081D7 RID: 33239 RVA: 0x002BF93D File Offset: 0x002BDB3D
		public CrystalSphereCell(int x, int y)
		{
			this.X = x;
			this.Y = y;
			this.IsHidden = true;
		}

		// Token: 0x060081D8 RID: 33240 RVA: 0x002BF95A File Offset: 0x002BDB5A
		public void SetItem(CrystalSphereItem item)
		{
			if (this.Item != null)
			{
				throw new InvalidOperationException("An item already occupies this cell");
			}
			this.Item = item;
		}

		// Token: 0x04002B80 RID: 11136
		private bool _isHidden;

		// Token: 0x04002B82 RID: 11138
		private bool _isHighlighted;

		// Token: 0x04002B83 RID: 11139
		private bool _isHovered;
	}
}
