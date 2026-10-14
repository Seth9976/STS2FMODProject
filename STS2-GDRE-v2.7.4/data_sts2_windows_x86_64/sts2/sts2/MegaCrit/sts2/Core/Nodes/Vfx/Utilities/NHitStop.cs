using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using Godot;
using Godot.Bridge;
using Godot.NativeInterop;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.Nodes.GodotExtensions;

namespace MegaCrit.Sts2.Core.Nodes.Vfx.Utilities
{
	// Token: 0x02000218 RID: 536
	[ScriptPath("res://src/Core/Nodes/Vfx/Utilities/NHitStop.cs")]
	public class NHitStop : Node
	{
		// Token: 0x06001B6F RID: 7023 RVA: 0x000B268B File Offset: 0x000B088B
		public void DoHitStop(ShakeStrength strength, ShakeDuration duration)
		{
			CancellationTokenSource cancelToken = this._cancelToken;
			if (cancelToken != null)
			{
				cancelToken.Cancel();
			}
			this._cancelToken = new CancellationTokenSource();
			TaskHelper.RunSafely(this.HitStopTask(this.EaseForStrength(strength), this.SecondsForDuration(duration)));
		}

		// Token: 0x06001B70 RID: 7024 RVA: 0x000B26C4 File Offset: 0x000B08C4
		[NullableContext(1)]
		private async Task HitStopTask(Ease.Functions easing, float seconds)
		{
			this.SetTimeScale(0.1f);
			ulong lastTicks = Time.GetTicksMsec();
			float timer = 0f;
			while (timer <= seconds)
			{
				await this.AwaitProcessFrame(default(CancellationToken));
				CancellationTokenSource cancelToken = this._cancelToken;
				if (cancelToken != null && cancelToken.IsCancellationRequested)
				{
					return;
				}
				timer += (Time.GetTicksMsec() - lastTicks) / 1000f;
				float num = Ease.Interpolate(timer / seconds, easing);
				float num2 = Mathf.Min(0.1f + num * 0.9f, 1f);
				this.SetTimeScale(num2);
				lastTicks = Time.GetTicksMsec();
			}
			this.SetTimeScale(1f);
		}

		// Token: 0x06001B71 RID: 7025 RVA: 0x000B2717 File Offset: 0x000B0917
		private void SetTimeScale(float timeScale)
		{
			Engine.SetTimeScale((double)timeScale);
		}

		// Token: 0x06001B72 RID: 7026 RVA: 0x000B2720 File Offset: 0x000B0920
		private Ease.Functions EaseForStrength(ShakeStrength strength)
		{
			Ease.Functions functions;
			switch (strength)
			{
			case ShakeStrength.VeryWeak:
				functions = Ease.Functions.CircIn;
				break;
			case ShakeStrength.Weak:
				functions = Ease.Functions.SineIn;
				break;
			case ShakeStrength.Medium:
				functions = Ease.Functions.QuadIn;
				break;
			case ShakeStrength.Strong:
				functions = Ease.Functions.QuartIn;
				break;
			case ShakeStrength.TooMuch:
				functions = Ease.Functions.ExpoIn;
				break;
			default:
				throw new ArgumentOutOfRangeException("strength", strength, null);
			}
			return functions;
		}

		// Token: 0x06001B73 RID: 7027 RVA: 0x000B2778 File Offset: 0x000B0978
		private float SecondsForDuration(ShakeDuration duration)
		{
			float num;
			switch (duration)
			{
			case ShakeDuration.Short:
				num = 0.15f;
				break;
			case ShakeDuration.Normal:
				num = 0.3f;
				break;
			case ShakeDuration.Long:
				num = 0.6f;
				break;
			case ShakeDuration.Forever:
				num = 2f;
				break;
			default:
				throw new ArgumentOutOfRangeException("duration", duration, null);
			}
			return num;
		}

		/// <summary>
		/// Get the method information for all the methods declared in this class.
		/// This method is used by Godot to register the available methods in the editor.
		/// Do not call this method.
		/// </summary>
		// Token: 0x06001B74 RID: 7028 RVA: 0x000B27D4 File Offset: 0x000B09D4
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal static List<MethodInfo> GetGodotMethodList()
		{
			return new List<MethodInfo>(4)
			{
				new MethodInfo(NHitStop.MethodName.DoHitStop, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, new List<PropertyInfo>
				{
					new PropertyInfo(2L, "strength", 0L, "", 6L, false),
					new PropertyInfo(2L, "duration", 0L, "", 6L, false)
				}, null),
				new MethodInfo(NHitStop.MethodName.SetTimeScale, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, new List<PropertyInfo>
				{
					new PropertyInfo(3L, "timeScale", 0L, "", 6L, false)
				}, null),
				new MethodInfo(NHitStop.MethodName.EaseForStrength, new PropertyInfo(2L, "", 0L, "", 6L, false), 1L, new List<PropertyInfo>
				{
					new PropertyInfo(2L, "strength", 0L, "", 6L, false)
				}, null),
				new MethodInfo(NHitStop.MethodName.SecondsForDuration, new PropertyInfo(3L, "", 0L, "", 6L, false), 1L, new List<PropertyInfo>
				{
					new PropertyInfo(2L, "duration", 0L, "", 6L, false)
				}, null)
			};
		}

		/// <inheritdoc />
		// Token: 0x06001B75 RID: 7029 RVA: 0x000B295C File Offset: 0x000B0B5C
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool InvokeGodotClassMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
		{
			if ((ref method) == NHitStop.MethodName.DoHitStop && args.Count == 2)
			{
				this.DoHitStop(VariantUtils.ConvertTo<ShakeStrength>(args[0]), VariantUtils.ConvertTo<ShakeDuration>(args[1]));
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NHitStop.MethodName.SetTimeScale && args.Count == 1)
			{
				this.SetTimeScale(VariantUtils.ConvertTo<float>(args[0]));
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NHitStop.MethodName.EaseForStrength && args.Count == 1)
			{
				Ease.Functions functions = this.EaseForStrength(VariantUtils.ConvertTo<ShakeStrength>(args[0]));
				ret = VariantUtils.CreateFrom<Ease.Functions>(ref functions);
				return true;
			}
			if ((ref method) == NHitStop.MethodName.SecondsForDuration && args.Count == 1)
			{
				float num = this.SecondsForDuration(VariantUtils.ConvertTo<ShakeDuration>(args[0]));
				ret = VariantUtils.CreateFrom<float>(ref num);
				return true;
			}
			return base.InvokeGodotClassMethod(ref method, args, ref ret);
		}

		/// <inheritdoc />
		// Token: 0x06001B76 RID: 7030 RVA: 0x000B2A5C File Offset: 0x000B0C5C
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool HasGodotClassMethod(in godot_string_name method)
		{
			return (ref method) == NHitStop.MethodName.DoHitStop || (ref method) == NHitStop.MethodName.SetTimeScale || (ref method) == NHitStop.MethodName.EaseForStrength || (ref method) == NHitStop.MethodName.SecondsForDuration || base.HasGodotClassMethod(ref method);
		}

		/// <inheritdoc />
		// Token: 0x06001B77 RID: 7031 RVA: 0x000B2AAC File Offset: 0x000B0CAC
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void SaveGodotObjectData(GodotSerializationInfo info)
		{
			base.SaveGodotObjectData(info);
		}

		/// <inheritdoc />
		// Token: 0x06001B78 RID: 7032 RVA: 0x000B2AB5 File Offset: 0x000B0CB5
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void RestoreGodotObjectData(GodotSerializationInfo info)
		{
			base.RestoreGodotObjectData(info);
		}

		// Token: 0x04000A62 RID: 2658
		private const float _minTimeScale = 0.1f;

		// Token: 0x04000A63 RID: 2659
		[Nullable(2)]
		private CancellationTokenSource _cancelToken;

		/// <summary>
		/// Cached StringNames for the methods contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x020011AF RID: 4527
		public class MethodName : Node.MethodName
		{
			/// <summary>
			/// Cached name for the 'DoHitStop' method.
			/// </summary>
			// Token: 0x040044EC RID: 17644
			public static readonly StringName DoHitStop = "DoHitStop";

			/// <summary>
			/// Cached name for the 'SetTimeScale' method.
			/// </summary>
			// Token: 0x040044ED RID: 17645
			public static readonly StringName SetTimeScale = "SetTimeScale";

			/// <summary>
			/// Cached name for the 'EaseForStrength' method.
			/// </summary>
			// Token: 0x040044EE RID: 17646
			public static readonly StringName EaseForStrength = "EaseForStrength";

			/// <summary>
			/// Cached name for the 'SecondsForDuration' method.
			/// </summary>
			// Token: 0x040044EF RID: 17647
			public static readonly StringName SecondsForDuration = "SecondsForDuration";
		}

		/// <summary>
		/// Cached StringNames for the properties and fields contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x020011B0 RID: 4528
		public class PropertyName : Node.PropertyName
		{
		}

		/// <summary>
		/// Cached StringNames for the signals contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x020011B1 RID: 4529
		public class SignalName : Node.SignalName
		{
		}
	}
}
