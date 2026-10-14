using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Godot;
using Godot.Bridge;
using Godot.NativeInterop;
using MegaCrit.Sts2.addons.mega_text;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization;
using MegaCrit.Sts2.Core.Nodes.GodotExtensions;
using MegaCrit.Sts2.Core.Nodes.HoverTips;

namespace MegaCrit.sts2.Core.Nodes.TopBar
{
	// Token: 0x0200001C RID: 28
	[ScriptPath("res://src/Core/Nodes/TopBar/NTopBarHp.cs")]
	public class NTopBarHp : NClickableControl
	{
		// Token: 0x060000E6 RID: 230 RVA: 0x00008038 File Offset: 0x00006238
		public override void _Ready()
		{
			this._hpLabel = base.GetNode<MegaLabel>("%HpLabel");
			this.ConnectSignals();
		}

		// Token: 0x060000E7 RID: 231 RVA: 0x00008058 File Offset: 0x00006258
		public override void _ExitTree()
		{
			if (this._player != null)
			{
				this._player.Creature.CurrentHpChanged -= this.UpdateHealth;
				this._player.Creature.MaxHpChanged -= this.UpdateHealth;
			}
		}

		// Token: 0x060000E8 RID: 232 RVA: 0x000080A8 File Offset: 0x000062A8
		[NullableContext(1)]
		public void Initialize(Player player)
		{
			this._player = player;
			this._player.Creature.CurrentHpChanged += this.UpdateHealth;
			this._player.Creature.MaxHpChanged += this.UpdateHealth;
			this.UpdateHealth(0, 0);
		}

		// Token: 0x060000E9 RID: 233 RVA: 0x000080FC File Offset: 0x000062FC
		private void UpdateHealth(int _, int __)
		{
			if (this._player == null)
			{
				return;
			}
			Creature creature = this._player.Creature;
			int currentHp = creature.CurrentHp;
			int maxHp = creature.MaxHp;
			MegaLabel hpLabel = this._hpLabel;
			DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(1, 2);
			defaultInterpolatedStringHandler.AppendFormatted<int>(currentHp);
			defaultInterpolatedStringHandler.AppendLiteral("/");
			defaultInterpolatedStringHandler.AppendFormatted<int>(maxHp);
			hpLabel.SetTextAutoSize(defaultInterpolatedStringHandler.ToStringAndClear());
		}

		// Token: 0x060000EA RID: 234 RVA: 0x00008164 File Offset: 0x00006364
		protected override void OnFocus()
		{
			HoverTip hoverTip = new HoverTip(new LocString("static_hover_tips", "HIT_POINTS.title"), new LocString("static_hover_tips", "HIT_POINTS.description"), null);
			NHoverTipSet nhoverTipSet = NHoverTipSet.CreateAndShow(this, hoverTip, HoverTipAlignment.None);
			if (nhoverTipSet != null)
			{
				nhoverTipSet.SetGlobalPosition(base.GlobalPosition + new Vector2(0f, base.Size.Y + 20f), false);
			}
		}

		// Token: 0x060000EB RID: 235 RVA: 0x000081D5 File Offset: 0x000063D5
		protected override void OnUnfocus()
		{
			NHoverTipSet.Remove(this);
		}

		// Token: 0x060000EC RID: 236 RVA: 0x000081E0 File Offset: 0x000063E0
		[NullableContext(1)]
		public async Task LerpAtNeow()
		{
			if (this._player != null)
			{
				MegaLabel hpLabel = this._hpLabel;
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(2, 1);
				defaultInterpolatedStringHandler.AppendLiteral("0/");
				defaultInterpolatedStringHandler.AppendFormatted<int>(this._player.Creature.MaxHp);
				hpLabel.SetTextAutoSize(defaultInterpolatedStringHandler.ToStringAndClear());
				await Cmd.Wait(0.5f, false);
				Tween tween = base.CreateTween();
				tween.TweenMethod(Callable.From<float>(new Action<float>(this.UpdateHpTween)), 0f, 1f, 1.0).SetEase(2L).SetTrans(7L);
			}
		}

		// Token: 0x060000ED RID: 237 RVA: 0x00008224 File Offset: 0x00006424
		private void UpdateHpTween(float tweenAmount)
		{
			if (this._player == null)
			{
				return;
			}
			Creature creature = this._player.Creature;
			int num = (int)Math.Round((double)((float)creature.CurrentHp * tweenAmount));
			int maxHp = creature.MaxHp;
			MegaLabel hpLabel = this._hpLabel;
			DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(1, 2);
			defaultInterpolatedStringHandler.AppendFormatted<int>(num);
			defaultInterpolatedStringHandler.AppendLiteral("/");
			defaultInterpolatedStringHandler.AppendFormatted<int>(maxHp);
			hpLabel.SetTextAutoSize(defaultInterpolatedStringHandler.ToStringAndClear());
		}

		/// <summary>
		/// Get the method information for all the methods declared in this class.
		/// This method is used by Godot to register the available methods in the editor.
		/// Do not call this method.
		/// </summary>
		// Token: 0x060000EE RID: 238 RVA: 0x00008298 File Offset: 0x00006498
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal new static List<MethodInfo> GetGodotMethodList()
		{
			return new List<MethodInfo>(6)
			{
				new MethodInfo(NTopBarHp.MethodName._Ready, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NTopBarHp.MethodName._ExitTree, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NTopBarHp.MethodName.UpdateHealth, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, new List<PropertyInfo>
				{
					new PropertyInfo(2L, "_", 0L, "", 6L, false),
					new PropertyInfo(2L, "__", 0L, "", 6L, false)
				}, null),
				new MethodInfo(NTopBarHp.MethodName.OnFocus, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NTopBarHp.MethodName.OnUnfocus, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NTopBarHp.MethodName.UpdateHpTween, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, new List<PropertyInfo>
				{
					new PropertyInfo(3L, "tweenAmount", 0L, "", 6L, false)
				}, null)
			};
		}

		/// <inheritdoc />
		// Token: 0x060000EF RID: 239 RVA: 0x00008434 File Offset: 0x00006634
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool InvokeGodotClassMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
		{
			if ((ref method) == NTopBarHp.MethodName._Ready && args.Count == 0)
			{
				this._Ready();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NTopBarHp.MethodName._ExitTree && args.Count == 0)
			{
				this._ExitTree();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NTopBarHp.MethodName.UpdateHealth && args.Count == 2)
			{
				this.UpdateHealth(VariantUtils.ConvertTo<int>(args[0]), VariantUtils.ConvertTo<int>(args[1]));
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NTopBarHp.MethodName.OnFocus && args.Count == 0)
			{
				this.OnFocus();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NTopBarHp.MethodName.OnUnfocus && args.Count == 0)
			{
				this.OnUnfocus();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NTopBarHp.MethodName.UpdateHpTween && args.Count == 1)
			{
				this.UpdateHpTween(VariantUtils.ConvertTo<float>(args[0]));
				ret = default(godot_variant);
				return true;
			}
			return base.InvokeGodotClassMethod(in method, args, out ret);
		}

		/// <inheritdoc />
		// Token: 0x060000F0 RID: 240 RVA: 0x00008554 File Offset: 0x00006754
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool HasGodotClassMethod(in godot_string_name method)
		{
			return (ref method) == NTopBarHp.MethodName._Ready || (ref method) == NTopBarHp.MethodName._ExitTree || (ref method) == NTopBarHp.MethodName.UpdateHealth || (ref method) == NTopBarHp.MethodName.OnFocus || (ref method) == NTopBarHp.MethodName.OnUnfocus || (ref method) == NTopBarHp.MethodName.UpdateHpTween || base.HasGodotClassMethod(in method);
		}

		/// <inheritdoc />
		// Token: 0x060000F1 RID: 241 RVA: 0x000085C2 File Offset: 0x000067C2
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool SetGodotClassPropertyValue(in godot_string_name name, in godot_variant value)
		{
			if ((ref name) == NTopBarHp.PropertyName._hpLabel)
			{
				this._hpLabel = VariantUtils.ConvertTo<MegaLabel>(ref value);
				return true;
			}
			return base.SetGodotClassPropertyValue(in name, in value);
		}

		/// <inheritdoc />
		// Token: 0x060000F2 RID: 242 RVA: 0x000085E7 File Offset: 0x000067E7
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
		{
			if ((ref name) == NTopBarHp.PropertyName._hpLabel)
			{
				value = VariantUtils.CreateFrom<MegaLabel>(ref this._hpLabel);
				return true;
			}
			return base.GetGodotClassPropertyValue(in name, out value);
		}

		/// <summary>
		/// Get the property information for all the properties declared in this class.
		/// This method is used by Godot to register the available properties in the editor.
		/// Do not call this method.
		/// </summary>
		// Token: 0x060000F3 RID: 243 RVA: 0x00008614 File Offset: 0x00006814
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal new static List<PropertyInfo> GetGodotPropertyList()
		{
			return new List<PropertyInfo>
			{
				new PropertyInfo(24L, NTopBarHp.PropertyName._hpLabel, 0L, "", 4096L, false)
			};
		}

		/// <inheritdoc />
		// Token: 0x060000F4 RID: 244 RVA: 0x00008649 File Offset: 0x00006849
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void SaveGodotObjectData(GodotSerializationInfo info)
		{
			base.SaveGodotObjectData(info);
			info.AddProperty(NTopBarHp.PropertyName._hpLabel, Variant.From<MegaLabel>(ref this._hpLabel));
		}

		/// <inheritdoc />
		// Token: 0x060000F5 RID: 245 RVA: 0x00008668 File Offset: 0x00006868
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void RestoreGodotObjectData(GodotSerializationInfo info)
		{
			base.RestoreGodotObjectData(info);
			Variant variant;
			if (info.TryGetProperty(NTopBarHp.PropertyName._hpLabel, ref variant))
			{
				this._hpLabel = variant.As<MegaLabel>();
			}
		}

		// Token: 0x0400005B RID: 91
		[Nullable(2)]
		private Player _player;

		// Token: 0x0400005C RID: 92
		[Nullable(1)]
		private MegaLabel _hpLabel;

		/// <summary>
		/// Cached StringNames for the methods contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x02000DE3 RID: 3555
		public new class MethodName : NClickableControl.MethodName
		{
			/// <summary>
			/// Cached name for the '_Ready' method.
			/// </summary>
			// Token: 0x040031C8 RID: 12744
			public static readonly StringName _Ready = "_Ready";

			/// <summary>
			/// Cached name for the '_ExitTree' method.
			/// </summary>
			// Token: 0x040031C9 RID: 12745
			public static readonly StringName _ExitTree = "_ExitTree";

			/// <summary>
			/// Cached name for the 'UpdateHealth' method.
			/// </summary>
			// Token: 0x040031CA RID: 12746
			public static readonly StringName UpdateHealth = "UpdateHealth";

			/// <summary>
			/// Cached name for the 'OnFocus' method.
			/// </summary>
			// Token: 0x040031CB RID: 12747
			public new static readonly StringName OnFocus = "OnFocus";

			/// <summary>
			/// Cached name for the 'OnUnfocus' method.
			/// </summary>
			// Token: 0x040031CC RID: 12748
			public new static readonly StringName OnUnfocus = "OnUnfocus";

			/// <summary>
			/// Cached name for the 'UpdateHpTween' method.
			/// </summary>
			// Token: 0x040031CD RID: 12749
			public static readonly StringName UpdateHpTween = "UpdateHpTween";
		}

		/// <summary>
		/// Cached StringNames for the properties and fields contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x02000DE4 RID: 3556
		public new class PropertyName : NClickableControl.PropertyName
		{
			/// <summary>
			/// Cached name for the '_hpLabel' field.
			/// </summary>
			// Token: 0x040031CE RID: 12750
			public static readonly StringName _hpLabel = "_hpLabel";
		}

		/// <summary>
		/// Cached StringNames for the signals contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x02000DE5 RID: 3557
		public new class SignalName : NClickableControl.SignalName
		{
		}
	}
}
