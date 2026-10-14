using System;
using Godot;
using MegaCrit.Sts2.Core.Helpers;

namespace MegaCrit.Sts2.Core.Nodes.Vfx.Utilities
{
	// Token: 0x02000220 RID: 544
	public class ScreenPunchInstance : ShakeInstance
	{
		// Token: 0x06001BCA RID: 7114 RVA: 0x000B47BC File Offset: 0x000B29BC
		public ScreenPunchInstance(float strength, double duration, float degAngle)
		{
			this._strength = strength;
			this._startDuration = duration;
			this._duration = duration;
			this._angle = Mathf.DegToRad(degAngle);
		}

		// Token: 0x06001BCB RID: 7115 RVA: 0x000B47E8 File Offset: 0x000B29E8
		public override Vector2 Update(double delta)
		{
			if (base.IsDone)
			{
				return Vector2.Zero;
			}
			this._duration -= delta;
			float num = (float)Math.Cos(this._duration * (double)ShakeInstance.WiggleSpeed);
			this._ease = Ease.CubicOut((float)(this._duration / this._startDuration));
			Vector2 vector = new Vector2(num, 0f).Rotated(this._angle) * this._strength * this._ease;
			if (this._duration < 0.0)
			{
				base.IsDone = true;
			}
			return vector;
		}

		// Token: 0x06001BCC RID: 7116 RVA: 0x000B4887 File Offset: 0x000B2A87
		public void Cancel()
		{
			this._duration = 0.0;
			base.IsDone = true;
		}
	}
}
