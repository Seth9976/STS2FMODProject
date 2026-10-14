using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Godot;
using Godot.Bridge;
using Godot.NativeInterop;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.Multiplayer.Game;
using MegaCrit.Sts2.Core.Multiplayer.Quality;
using MegaCrit.Sts2.Core.Nodes.GodotExtensions;
using MegaCrit.Sts2.Core.Nodes.Vfx;
using MegaCrit.Sts2.Core.Runs;

namespace MegaCrit.Sts2.Core.Nodes.Multiplayer
{
	/// <summary>
	/// This indicator is displayed over players' indicators when we detect that our connection with them is bad.
	/// </summary>
	// Token: 0x0200038D RID: 909
	[ScriptPath("res://src/Core/Nodes/Multiplayer/NMultiplayerNetworkProblemIndicator.cs")]
	public class NMultiplayerNetworkProblemIndicator : TextureRect
	{
		// Token: 0x17000852 RID: 2130
		// (get) Token: 0x0600360E RID: 13838 RVA: 0x0019309C File Offset: 0x0019129C
		// (set) Token: 0x0600360F RID: 13839 RVA: 0x001930A4 File Offset: 0x001912A4
		public bool IsShown { get; private set; }

		// Token: 0x06003610 RID: 13840 RVA: 0x001930AD File Offset: 0x001912AD
		public void Initialize(ulong peerId)
		{
			if (!RunManager.Instance.NetService.Type.IsMultiplayer())
			{
				return;
			}
			this._peerId = peerId;
			TaskHelper.RunSafely(this.UpdateLoop());
		}

		// Token: 0x06003611 RID: 13841 RVA: 0x001930DC File Offset: 0x001912DC
		[NullableContext(1)]
		private async Task UpdateLoop()
		{
			while (RunManager.Instance.NetService.IsConnected && this.IsValid())
			{
				ConnectionStats statsForPeer = RunManager.Instance.NetService.GetStatsForPeer(this._peerId);
				if (statsForPeer == null)
				{
					return;
				}
				float num = statsForPeer.PingMsec / (1f - statsForPeer.PacketLoss);
				bool flag = num >= 350f;
				if (!this.IsShown && flag)
				{
					Tween tween = this._tween;
					if (tween != null)
					{
						tween.Kill();
					}
					base.Visible = true;
					base.Modulate = Colors.White;
				}
				else if (this.IsShown && flag)
				{
					NUiFlashVfx nuiFlashVfx = NUiFlashVfx.Create(base.Texture, Colors.White);
					this.AddChildSafely(nuiFlashVfx);
					TaskHelper.RunSafely(nuiFlashVfx.StartVfx());
				}
				else if (this.IsShown && !flag)
				{
					Tween tween2 = this._tween;
					if (tween2 != null)
					{
						tween2.Kill();
					}
					this._tween = base.CreateTween();
					this._tween.TweenProperty(this, "modulate:a", 0f, 0.5);
				}
				this.IsShown = flag;
				await Task.Delay(2000);
			}
		}

		/// <summary>
		/// Get the method information for all the methods declared in this class.
		/// This method is used by Godot to register the available methods in the editor.
		/// Do not call this method.
		/// </summary>
		// Token: 0x06003612 RID: 13842 RVA: 0x00193120 File Offset: 0x00191320
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal static List<MethodInfo> GetGodotMethodList()
		{
			return new List<MethodInfo>(1)
			{
				new MethodInfo(NMultiplayerNetworkProblemIndicator.MethodName.Initialize, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, new List<PropertyInfo>
				{
					new PropertyInfo(2L, "peerId", 0L, "", 6L, false)
				}, null)
			};
		}

		/// <inheritdoc />
		// Token: 0x06003613 RID: 13843 RVA: 0x00193189 File Offset: 0x00191389
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool InvokeGodotClassMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
		{
			if ((ref method) == NMultiplayerNetworkProblemIndicator.MethodName.Initialize && args.Count == 1)
			{
				this.Initialize(VariantUtils.ConvertTo<ulong>(args[0]));
				ret = default(godot_variant);
				return true;
			}
			return base.InvokeGodotClassMethod(ref method, args, ref ret);
		}

		/// <inheritdoc />
		// Token: 0x06003614 RID: 13844 RVA: 0x001931C7 File Offset: 0x001913C7
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool HasGodotClassMethod(in godot_string_name method)
		{
			return (ref method) == NMultiplayerNetworkProblemIndicator.MethodName.Initialize || base.HasGodotClassMethod(ref method);
		}

		/// <inheritdoc />
		// Token: 0x06003615 RID: 13845 RVA: 0x001931E0 File Offset: 0x001913E0
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool SetGodotClassPropertyValue(in godot_string_name name, in godot_variant value)
		{
			if ((ref name) == NMultiplayerNetworkProblemIndicator.PropertyName.IsShown)
			{
				this.IsShown = VariantUtils.ConvertTo<bool>(ref value);
				return true;
			}
			if ((ref name) == NMultiplayerNetworkProblemIndicator.PropertyName._peerId)
			{
				this._peerId = VariantUtils.ConvertTo<ulong>(ref value);
				return true;
			}
			if ((ref name) == NMultiplayerNetworkProblemIndicator.PropertyName._tween)
			{
				this._tween = VariantUtils.ConvertTo<Tween>(ref value);
				return true;
			}
			return base.SetGodotClassPropertyValue(ref name, ref value);
		}

		/// <inheritdoc />
		// Token: 0x06003616 RID: 13846 RVA: 0x00193248 File Offset: 0x00191448
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
		{
			if ((ref name) == NMultiplayerNetworkProblemIndicator.PropertyName.IsShown)
			{
				bool isShown = this.IsShown;
				value = VariantUtils.CreateFrom<bool>(ref isShown);
				return true;
			}
			if ((ref name) == NMultiplayerNetworkProblemIndicator.PropertyName._peerId)
			{
				value = VariantUtils.CreateFrom<ulong>(ref this._peerId);
				return true;
			}
			if ((ref name) == NMultiplayerNetworkProblemIndicator.PropertyName._tween)
			{
				value = VariantUtils.CreateFrom<Tween>(ref this._tween);
				return true;
			}
			return base.GetGodotClassPropertyValue(ref name, ref value);
		}

		/// <summary>
		/// Get the property information for all the properties declared in this class.
		/// This method is used by Godot to register the available properties in the editor.
		/// Do not call this method.
		/// </summary>
		// Token: 0x06003617 RID: 13847 RVA: 0x001932C0 File Offset: 0x001914C0
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal static List<PropertyInfo> GetGodotPropertyList()
		{
			return new List<PropertyInfo>
			{
				new PropertyInfo(2L, NMultiplayerNetworkProblemIndicator.PropertyName._peerId, 0L, "", 4096L, false),
				new PropertyInfo(24L, NMultiplayerNetworkProblemIndicator.PropertyName._tween, 0L, "", 4096L, false),
				new PropertyInfo(1L, NMultiplayerNetworkProblemIndicator.PropertyName.IsShown, 0L, "", 4096L, false)
			};
		}

		/// <inheritdoc />
		// Token: 0x06003618 RID: 13848 RVA: 0x00193338 File Offset: 0x00191538
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void SaveGodotObjectData(GodotSerializationInfo info)
		{
			base.SaveGodotObjectData(info);
			StringName isShown = NMultiplayerNetworkProblemIndicator.PropertyName.IsShown;
			bool isShown2 = this.IsShown;
			info.AddProperty(isShown, Variant.From<bool>(ref isShown2));
			info.AddProperty(NMultiplayerNetworkProblemIndicator.PropertyName._peerId, Variant.From<ulong>(ref this._peerId));
			info.AddProperty(NMultiplayerNetworkProblemIndicator.PropertyName._tween, Variant.From<Tween>(ref this._tween));
		}

		/// <inheritdoc />
		// Token: 0x06003619 RID: 13849 RVA: 0x00193394 File Offset: 0x00191594
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void RestoreGodotObjectData(GodotSerializationInfo info)
		{
			base.RestoreGodotObjectData(info);
			Variant variant;
			if (info.TryGetProperty(NMultiplayerNetworkProblemIndicator.PropertyName.IsShown, ref variant))
			{
				this.IsShown = variant.As<bool>();
			}
			Variant variant2;
			if (info.TryGetProperty(NMultiplayerNetworkProblemIndicator.PropertyName._peerId, ref variant2))
			{
				this._peerId = variant2.As<ulong>();
			}
			Variant variant3;
			if (info.TryGetProperty(NMultiplayerNetworkProblemIndicator.PropertyName._tween, ref variant3))
			{
				this._tween = variant3.As<Tween>();
			}
		}

		// Token: 0x040014E5 RID: 5349
		private const float _qualityScoreToShowAt = 350f;

		// Token: 0x040014E6 RID: 5350
		private ulong _peerId;

		// Token: 0x040014E7 RID: 5351
		[Nullable(2)]
		private Tween _tween;

		/// <summary>
		/// Cached StringNames for the methods contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x02001718 RID: 5912
		public class MethodName : TextureRect.MethodName
		{
			/// <summary>
			/// Cached name for the 'Initialize' method.
			/// </summary>
			// Token: 0x040059F6 RID: 23030
			public static readonly StringName Initialize = "Initialize";
		}

		/// <summary>
		/// Cached StringNames for the properties and fields contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x02001719 RID: 5913
		public class PropertyName : TextureRect.PropertyName
		{
			/// <summary>
			/// Cached name for the 'IsShown' property.
			/// </summary>
			// Token: 0x040059F7 RID: 23031
			public static readonly StringName IsShown = "IsShown";

			/// <summary>
			/// Cached name for the '_peerId' field.
			/// </summary>
			// Token: 0x040059F8 RID: 23032
			public static readonly StringName _peerId = "_peerId";

			/// <summary>
			/// Cached name for the '_tween' field.
			/// </summary>
			// Token: 0x040059F9 RID: 23033
			public static readonly StringName _tween = "_tween";
		}

		/// <summary>
		/// Cached StringNames for the signals contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x0200171A RID: 5914
		public class SignalName : TextureRect.SignalName
		{
		}
	}
}
