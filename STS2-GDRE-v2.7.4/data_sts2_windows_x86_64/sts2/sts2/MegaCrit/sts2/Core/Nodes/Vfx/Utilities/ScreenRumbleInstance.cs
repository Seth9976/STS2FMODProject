using System;
using System.Runtime.CompilerServices;
using Godot;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.Random;

namespace MegaCrit.Sts2.Core.Nodes.Vfx.Utilities
{
	// Token: 0x02000221 RID: 545
	public class ScreenRumbleInstance : ShakeInstance
	{
		// Token: 0x06001BCD RID: 7117 RVA: 0x000B48A0 File Offset: 0x000B2AA0
		public ScreenRumbleInstance(float strength, double duration, float speedMultiplier, RumbleStyle style)
		{
			this._noise.Frequency = ((style == RumbleStyle.Rumble) ? 6f : 0.1f);
			this._noise.NoiseType = 3L;
			this._strength = strength * ((style == RumbleStyle.Rumble) ? 0.5f : 5f);
			this._speed *= speedMultiplier;
			this._startDuration = duration;
			this._duration = duration;
			this._style = style;
		}

		// Token: 0x06001BCE RID: 7118 RVA: 0x000B4950 File Offset: 0x000B2B50
		public override Vector2 Update(double delta)
		{
			if (base.IsDone)
			{
				return Vector2.Zero;
			}
			this._duration -= delta;
			float num = (float)this._duration;
			float noise1D = this._noise.GetNoise1D(num + this._randomOffset);
			float noise1D2 = this._noise.GetNoise1D(num + this._randomOffset + this._randomOffset);
			this._ease = Ease.CubicOut((float)(this._duration / this._startDuration));
			Vector2 vector;
			if (this._style == RumbleStyle.Drunk)
			{
				this._targetOffset = this._targetOffset.Lerp(new Vector2(noise1D, noise1D2), Mathf.Clamp((float)delta * this._speed, 0f, 1f));
				vector = this._targetOffset * this._strength * this._ease;
			}
			else
			{
				vector = new Vector2(noise1D, noise1D2) * this._strength * this._ease;
			}
			if (this._duration < 0.0)
			{
				base.IsDone = true;
			}
			return vector;
		}

		// Token: 0x04000A7D RID: 2685
		[Nullable(1)]
		private readonly FastNoiseLite _noise = new FastNoiseLite();

		// Token: 0x04000A7E RID: 2686
		private readonly float _randomOffset = Rng.Chaotic.NextFloat(99999f);

		// Token: 0x04000A7F RID: 2687
		private readonly float _speed = 10f;

		// Token: 0x04000A80 RID: 2688
		private Vector2 _targetOffset = Vector2.Zero;

		// Token: 0x04000A81 RID: 2689
		private readonly RumbleStyle _style;
	}
}
