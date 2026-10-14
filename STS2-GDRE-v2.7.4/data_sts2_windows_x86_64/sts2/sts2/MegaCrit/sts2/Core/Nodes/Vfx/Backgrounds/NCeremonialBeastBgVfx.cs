using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using Godot;
using Godot.Bridge;
using Godot.NativeInterop;
using MegaCrit.Sts2.Core.Bindings.MegaSpine;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Context;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Models.Monsters;
using MegaCrit.Sts2.Core.Models.Powers;
using MegaCrit.Sts2.Core.Nodes.Audio;

namespace MegaCrit.Sts2.Core.Nodes.Vfx.Backgrounds
{
	// Token: 0x0200023E RID: 574
	[GlobalClass]
	[ScriptPath("res://src/Core/Nodes/Vfx/Backgrounds/NCeremonialBeastBgVfx.cs")]
	public class NCeremonialBeastBgVfx : Node
	{
		// Token: 0x06001D35 RID: 7477 RVA: 0x000BE640 File Offset: 0x000BC840
		public override void _Ready()
		{
			this._parent = base.GetParent<Node2D>();
			this._parent.Visible = false;
			this._animController = new MegaSprite(this._parent);
		}

		// Token: 0x06001D36 RID: 7478 RVA: 0x000BE670 File Offset: 0x000BC870
		public override void _EnterTree()
		{
			CombatManager.Instance.StateTracker.CombatStateChanged += this.UpdateState;
		}

		// Token: 0x06001D37 RID: 7479 RVA: 0x000BE68D File Offset: 0x000BC88D
		public override void _ExitTree()
		{
			CombatManager.Instance.StateTracker.CombatStateChanged -= this.UpdateState;
		}

		// Token: 0x06001D38 RID: 7480 RVA: 0x000BE6AA File Offset: 0x000BC8AA
		[NullableContext(2)]
		private void UpdateState(CombatState combatState)
		{
			if (combatState == null)
			{
				return;
			}
			this.UpdateRingingSfx(combatState);
			this.UpdateVfxAndMusic(combatState);
		}

		// Token: 0x06001D39 RID: 7481 RVA: 0x000BE6C0 File Offset: 0x000BC8C0
		[NullableContext(1)]
		private void UpdateRingingSfx(CombatState combatState)
		{
			Player me = LocalContext.GetMe(combatState);
			bool flag = me != null && me.Creature.HasPower<RingingPower>();
			NRunMusicController instance = NRunMusicController.Instance;
			if (instance == null)
			{
				return;
			}
			instance.UpdateMusicParameter("ringing", flag > false);
		}

		// Token: 0x06001D3A RID: 7482 RVA: 0x000BE700 File Offset: 0x000BC900
		[NullableContext(1)]
		private void UpdateVfxAndMusic(CombatState combatState)
		{
			Creature creature = combatState.Creatures.FirstOrDefault<Creature>((Creature c) => c.Monster is CeremonialBeast);
			if (creature == null)
			{
				NRunMusicController instance = NRunMusicController.Instance;
				if (instance != null)
				{
					instance.UpdateMusicParameter("ceremonial_beast_progress", 5f);
				}
				this.PlayFlowers();
				return;
			}
			CeremonialBeast ceremonialBeast = creature.Monster as CeremonialBeast;
			if (ceremonialBeast != null && !ceremonialBeast.IsInSecondPhase)
			{
				this._parent.Visible = false;
				return;
			}
			this._parent.Visible = true;
			if ((float)creature.CurrentHp > (float)creature.MaxHp * 0.33f)
			{
				NRunMusicController instance2 = NRunMusicController.Instance;
				if (instance2 != null)
				{
					instance2.UpdateMusicParameter("ceremonial_beast_progress", 1f);
				}
				this.PlayGlow();
				return;
			}
			if (creature.IsAlive)
			{
				NRunMusicController instance3 = NRunMusicController.Instance;
				if (instance3 != null)
				{
					instance3.UpdateMusicParameter("ceremonial_beast_progress", 1f);
				}
				this.PlaySkulls();
				return;
			}
			NRunMusicController instance4 = NRunMusicController.Instance;
			if (instance4 != null)
			{
				instance4.UpdateMusicParameter("ceremonial_beast_progress", 5f);
			}
			this.PlayFlowers();
		}

		// Token: 0x06001D3B RID: 7483 RVA: 0x000BE80C File Offset: 0x000BCA0C
		private void PlayGlow()
		{
			if (this._isGlowOn)
			{
				return;
			}
			this._isGlowOn = true;
			MegaAnimationState animationState = this._animController.GetAnimationState();
			animationState.SetAnimation("glow_spawn", true, 0);
			animationState.AddAnimation("glow_idle", 0f, true, 0);
		}

		// Token: 0x06001D3C RID: 7484 RVA: 0x000BE854 File Offset: 0x000BCA54
		private void PlaySkulls()
		{
			if (this._areSkullsOn)
			{
				return;
			}
			this._areSkullsOn = true;
			MegaAnimationState animationState = this._animController.GetAnimationState();
			animationState.SetAnimation("skulls_spawn", true, 0);
			animationState.AddAnimation("glow_and_skulls_idle", 0f, true, 0);
		}

		// Token: 0x06001D3D RID: 7485 RVA: 0x000BE89C File Offset: 0x000BCA9C
		private void PlayFlowers()
		{
			MegaAnimationState animationState = this._animController.GetAnimationState();
			animationState.SetAnimation("glow_and_skulls_idle", true, 0);
			animationState.AddAnimation("plants_spawn", 4.5f, false, 0);
		}

		/// <summary>
		/// Get the method information for all the methods declared in this class.
		/// This method is used by Godot to register the available methods in the editor.
		/// Do not call this method.
		/// </summary>
		// Token: 0x06001D3E RID: 7486 RVA: 0x000BE8D4 File Offset: 0x000BCAD4
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal static List<MethodInfo> GetGodotMethodList()
		{
			return new List<MethodInfo>(6)
			{
				new MethodInfo(NCeremonialBeastBgVfx.MethodName._Ready, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NCeremonialBeastBgVfx.MethodName._EnterTree, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NCeremonialBeastBgVfx.MethodName._ExitTree, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NCeremonialBeastBgVfx.MethodName.PlayGlow, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NCeremonialBeastBgVfx.MethodName.PlaySkulls, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NCeremonialBeastBgVfx.MethodName.PlayFlowers, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null)
			};
		}

		/// <inheritdoc />
		// Token: 0x06001D3F RID: 7487 RVA: 0x000BEA04 File Offset: 0x000BCC04
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool InvokeGodotClassMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
		{
			if ((ref method) == NCeremonialBeastBgVfx.MethodName._Ready && args.Count == 0)
			{
				this._Ready();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NCeremonialBeastBgVfx.MethodName._EnterTree && args.Count == 0)
			{
				this._EnterTree();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NCeremonialBeastBgVfx.MethodName._ExitTree && args.Count == 0)
			{
				this._ExitTree();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NCeremonialBeastBgVfx.MethodName.PlayGlow && args.Count == 0)
			{
				this.PlayGlow();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NCeremonialBeastBgVfx.MethodName.PlaySkulls && args.Count == 0)
			{
				this.PlaySkulls();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NCeremonialBeastBgVfx.MethodName.PlayFlowers && args.Count == 0)
			{
				this.PlayFlowers();
				ret = default(godot_variant);
				return true;
			}
			return base.InvokeGodotClassMethod(ref method, args, ref ret);
		}

		/// <inheritdoc />
		// Token: 0x06001D40 RID: 7488 RVA: 0x000BEAF8 File Offset: 0x000BCCF8
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool HasGodotClassMethod(in godot_string_name method)
		{
			return (ref method) == NCeremonialBeastBgVfx.MethodName._Ready || (ref method) == NCeremonialBeastBgVfx.MethodName._EnterTree || (ref method) == NCeremonialBeastBgVfx.MethodName._ExitTree || (ref method) == NCeremonialBeastBgVfx.MethodName.PlayGlow || (ref method) == NCeremonialBeastBgVfx.MethodName.PlaySkulls || (ref method) == NCeremonialBeastBgVfx.MethodName.PlayFlowers || base.HasGodotClassMethod(ref method);
		}

		/// <inheritdoc />
		// Token: 0x06001D41 RID: 7489 RVA: 0x000BEB68 File Offset: 0x000BCD68
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool SetGodotClassPropertyValue(in godot_string_name name, in godot_variant value)
		{
			if ((ref name) == NCeremonialBeastBgVfx.PropertyName._isGlowOn)
			{
				this._isGlowOn = VariantUtils.ConvertTo<bool>(ref value);
				return true;
			}
			if ((ref name) == NCeremonialBeastBgVfx.PropertyName._areSkullsOn)
			{
				this._areSkullsOn = VariantUtils.ConvertTo<bool>(ref value);
				return true;
			}
			if ((ref name) == NCeremonialBeastBgVfx.PropertyName._parent)
			{
				this._parent = VariantUtils.ConvertTo<Node2D>(ref value);
				return true;
			}
			return base.SetGodotClassPropertyValue(ref name, ref value);
		}

		/// <inheritdoc />
		// Token: 0x06001D42 RID: 7490 RVA: 0x000BEBD0 File Offset: 0x000BCDD0
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
		{
			if ((ref name) == NCeremonialBeastBgVfx.PropertyName._isGlowOn)
			{
				value = VariantUtils.CreateFrom<bool>(ref this._isGlowOn);
				return true;
			}
			if ((ref name) == NCeremonialBeastBgVfx.PropertyName._areSkullsOn)
			{
				value = VariantUtils.CreateFrom<bool>(ref this._areSkullsOn);
				return true;
			}
			if ((ref name) == NCeremonialBeastBgVfx.PropertyName._parent)
			{
				value = VariantUtils.CreateFrom<Node2D>(ref this._parent);
				return true;
			}
			return base.GetGodotClassPropertyValue(ref name, ref value);
		}

		/// <summary>
		/// Get the property information for all the properties declared in this class.
		/// This method is used by Godot to register the available properties in the editor.
		/// Do not call this method.
		/// </summary>
		// Token: 0x06001D43 RID: 7491 RVA: 0x000BEC48 File Offset: 0x000BCE48
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal static List<PropertyInfo> GetGodotPropertyList()
		{
			return new List<PropertyInfo>
			{
				new PropertyInfo(1L, NCeremonialBeastBgVfx.PropertyName._isGlowOn, 0L, "", 4096L, false),
				new PropertyInfo(1L, NCeremonialBeastBgVfx.PropertyName._areSkullsOn, 0L, "", 4096L, false),
				new PropertyInfo(24L, NCeremonialBeastBgVfx.PropertyName._parent, 0L, "", 4096L, false)
			};
		}

		/// <inheritdoc />
		// Token: 0x06001D44 RID: 7492 RVA: 0x000BECC0 File Offset: 0x000BCEC0
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void SaveGodotObjectData(GodotSerializationInfo info)
		{
			base.SaveGodotObjectData(info);
			info.AddProperty(NCeremonialBeastBgVfx.PropertyName._isGlowOn, Variant.From<bool>(ref this._isGlowOn));
			info.AddProperty(NCeremonialBeastBgVfx.PropertyName._areSkullsOn, Variant.From<bool>(ref this._areSkullsOn));
			info.AddProperty(NCeremonialBeastBgVfx.PropertyName._parent, Variant.From<Node2D>(ref this._parent));
		}

		/// <inheritdoc />
		// Token: 0x06001D45 RID: 7493 RVA: 0x000BED18 File Offset: 0x000BCF18
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void RestoreGodotObjectData(GodotSerializationInfo info)
		{
			base.RestoreGodotObjectData(info);
			Variant variant;
			if (info.TryGetProperty(NCeremonialBeastBgVfx.PropertyName._isGlowOn, ref variant))
			{
				this._isGlowOn = variant.As<bool>();
			}
			Variant variant2;
			if (info.TryGetProperty(NCeremonialBeastBgVfx.PropertyName._areSkullsOn, ref variant2))
			{
				this._areSkullsOn = variant2.As<bool>();
			}
			Variant variant3;
			if (info.TryGetProperty(NCeremonialBeastBgVfx.PropertyName._parent, ref variant3))
			{
				this._parent = variant3.As<Node2D>();
			}
		}

		// Token: 0x04000B46 RID: 2886
		private bool _isGlowOn;

		// Token: 0x04000B47 RID: 2887
		private bool _areSkullsOn;

		// Token: 0x04000B48 RID: 2888
		[Nullable(1)]
		private Node2D _parent;

		// Token: 0x04000B49 RID: 2889
		[Nullable(1)]
		private MegaSprite _animController;

		/// <summary>
		/// Cached StringNames for the methods contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x02001227 RID: 4647
		public class MethodName : Node.MethodName
		{
			/// <summary>
			/// Cached name for the '_Ready' method.
			/// </summary>
			// Token: 0x0400464C RID: 17996
			public static readonly StringName _Ready = "_Ready";

			/// <summary>
			/// Cached name for the '_EnterTree' method.
			/// </summary>
			// Token: 0x0400464D RID: 17997
			public static readonly StringName _EnterTree = "_EnterTree";

			/// <summary>
			/// Cached name for the '_ExitTree' method.
			/// </summary>
			// Token: 0x0400464E RID: 17998
			public static readonly StringName _ExitTree = "_ExitTree";

			/// <summary>
			/// Cached name for the 'PlayGlow' method.
			/// </summary>
			// Token: 0x0400464F RID: 17999
			public static readonly StringName PlayGlow = "PlayGlow";

			/// <summary>
			/// Cached name for the 'PlaySkulls' method.
			/// </summary>
			// Token: 0x04004650 RID: 18000
			public static readonly StringName PlaySkulls = "PlaySkulls";

			/// <summary>
			/// Cached name for the 'PlayFlowers' method.
			/// </summary>
			// Token: 0x04004651 RID: 18001
			public static readonly StringName PlayFlowers = "PlayFlowers";
		}

		/// <summary>
		/// Cached StringNames for the properties and fields contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x02001228 RID: 4648
		public class PropertyName : Node.PropertyName
		{
			/// <summary>
			/// Cached name for the '_isGlowOn' field.
			/// </summary>
			// Token: 0x04004652 RID: 18002
			public static readonly StringName _isGlowOn = "_isGlowOn";

			/// <summary>
			/// Cached name for the '_areSkullsOn' field.
			/// </summary>
			// Token: 0x04004653 RID: 18003
			public static readonly StringName _areSkullsOn = "_areSkullsOn";

			/// <summary>
			/// Cached name for the '_parent' field.
			/// </summary>
			// Token: 0x04004654 RID: 18004
			public static readonly StringName _parent = "_parent";
		}

		/// <summary>
		/// Cached StringNames for the signals contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x02001229 RID: 4649
		public class SignalName : Node.SignalName
		{
		}
	}
}
