using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using Godot;
using Godot.Bridge;
using Godot.NativeInterop;
using MegaCrit.Sts2.Core.Assets;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Context;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Nodes.Combat;
using MegaCrit.Sts2.Core.Nodes.GodotExtensions;

namespace MegaCrit.Sts2.Core.Nodes.Orbs
{
	// Token: 0x02000389 RID: 905
	[NullableContext(1)]
	[Nullable(0)]
	[ScriptPath("res://src/Core/Nodes/Orbs/NOrbManager.cs")]
	public class NOrbManager : Control
	{
		// Token: 0x17000849 RID: 2121
		// (get) Token: 0x060035BE RID: 13758 RVA: 0x00191188 File Offset: 0x0018F388
		private static string ScenePath
		{
			get
			{
				return SceneHelper.GetScenePath("/orbs/orb_manager");
			}
		}

		// Token: 0x1700084A RID: 2122
		// (get) Token: 0x060035BF RID: 13759 RVA: 0x00191194 File Offset: 0x0018F394
		public static IEnumerable<string> AssetPaths
		{
			get
			{
				return new <>z__ReadOnlySingleElementList<string>(NOrbManager.ScenePath);
			}
		}

		// Token: 0x1700084B RID: 2123
		// (get) Token: 0x060035C0 RID: 13760 RVA: 0x001911A0 File Offset: 0x0018F3A0
		// (set) Token: 0x060035C1 RID: 13761 RVA: 0x001911A8 File Offset: 0x0018F3A8
		public bool IsLocal { get; private set; }

		// Token: 0x1700084C RID: 2124
		// (get) Token: 0x060035C2 RID: 13762 RVA: 0x001911B1 File Offset: 0x0018F3B1
		private Player Player
		{
			get
			{
				return this._creatureNode.Entity.Player;
			}
		}

		// Token: 0x060035C3 RID: 13763 RVA: 0x001911C3 File Offset: 0x0018F3C3
		public override void _Ready()
		{
			this._orbContainer = base.GetNode<Control>("%Orbs");
		}

		// Token: 0x060035C4 RID: 13764 RVA: 0x001911DB File Offset: 0x0018F3DB
		public override void _EnterTree()
		{
			base._EnterTree();
			CombatManager.Instance.StateTracker.CombatStateChanged += this.OnCombatStateChanged;
			CombatManager.Instance.CombatSetUp += this.OnCombatSetup;
		}

		// Token: 0x060035C5 RID: 13765 RVA: 0x00191214 File Offset: 0x0018F414
		public override void _ExitTree()
		{
			base._ExitTree();
			CombatManager.Instance.StateTracker.CombatStateChanged -= this.OnCombatStateChanged;
			CombatManager.Instance.CombatSetUp -= this.OnCombatSetup;
		}

		// Token: 0x060035C6 RID: 13766 RVA: 0x00191250 File Offset: 0x0018F450
		public static NOrbManager Create(NCreature creature, bool isLocal)
		{
			if (creature.Entity.Player == null)
			{
				throw new InvalidOperationException("NOrbManager can only be applied to player creatures");
			}
			NOrbManager norbManager = PreloadManager.Cache.GetScene(NOrbManager.ScenePath).Instantiate<NOrbManager>(0L);
			norbManager._creatureNode = creature;
			norbManager.IsLocal = isLocal;
			return norbManager;
		}

		// Token: 0x060035C7 RID: 13767 RVA: 0x0019129B File Offset: 0x0018F49B
		private void OnCombatSetup(CombatState _)
		{
			if (this.Player.Creature.IsAlive && this.Player.PlayerCombatState != null)
			{
				this.AddSlotAnim(this.Player.PlayerCombatState.OrbQueue.Capacity);
			}
		}

		// Token: 0x060035C8 RID: 13768 RVA: 0x001912D8 File Offset: 0x0018F4D8
		public void RemoveSlotAnim(int amount)
		{
			if (amount > this._orbs.Count)
			{
				throw new InvalidOperationException("There are not enough slots to remove.");
			}
			for (int i = 0; i < amount; i++)
			{
				NOrb norb = this._orbs.Last<NOrb>();
				norb.QueueFreeSafely();
				this._orbs.Remove(norb);
				if (norb.HasFocus())
				{
					this._creatureNode.Hitbox.TryGrabFocus();
				}
			}
			this.TweenLayout();
			this.UpdateControllerNavigation();
		}

		// Token: 0x060035C9 RID: 13769 RVA: 0x00191350 File Offset: 0x0018F550
		public void AddSlotAnim(int amount)
		{
			for (int i = 0; i < amount; i++)
			{
				NOrb norb = NOrb.Create(LocalContext.IsMe(this.Player));
				this._orbContainer.AddChildSafely(norb);
				this._orbs.Add(norb);
				norb.Position = Vector2.Zero;
			}
			this.TweenLayout();
			this.UpdateControllerNavigation();
		}

		// Token: 0x060035CA RID: 13770 RVA: 0x001913AC File Offset: 0x0018F5AC
		public void ReplaceOrb(OrbModel oldOrb, OrbModel newOrb)
		{
			for (int i = 0; i < this._orbs.Count; i++)
			{
				if (this._orbs[i].Model == oldOrb)
				{
					this._orbs[i].ReplaceOrb(newOrb);
				}
			}
			this.UpdateControllerNavigation();
		}

		// Token: 0x060035CB RID: 13771 RVA: 0x001913FC File Offset: 0x0018F5FC
		public void AddOrbAnim()
		{
			OrbModel orbModel = this.Player.PlayerCombatState.OrbQueue.Orbs.LastOrDefault<OrbModel>();
			NOrb norb = this._orbs.FirstOrDefault<NOrb>((NOrb node) => node.Model == null);
			if (norb == null)
			{
				this.EvokeOrbAnim(this._orbs.First<NOrb>((NOrb node) => node.Model != null).Model);
				norb = (NOrb)this._orbContainer.GetChildren(false).First<Node>((Node node) => ((NOrb)node).Model == null);
			}
			NOrb norb2 = NOrb.Create(LocalContext.IsMe(this.Player), orbModel);
			norb.AddSiblingSafely(norb2);
			this._orbs.Insert(this._orbs.IndexOf(norb), norb2);
			norb2.Position = norb.Position;
			this._orbContainer.RemoveChildSafely(norb);
			this._orbs.Remove(norb);
			norb.QueueFreeSafely();
			this.TweenLayout();
			this.UpdateControllerNavigation();
		}

		// Token: 0x060035CC RID: 13772 RVA: 0x00191528 File Offset: 0x0018F728
		public void EvokeOrbAnim(OrbModel orb)
		{
			NOrb norb = this._orbs.Last<NOrb>((NOrb node) => node.Model == orb);
			Tween tween = base.CreateTween();
			this._orbs.Remove(norb);
			tween.TweenProperty(norb, "modulate:a", 0, 0.25);
			tween.Chain().TweenCallback(Callable.From(new Action(norb.QueueFreeSafely)));
			NOrb norb2 = NOrb.Create(LocalContext.IsMe(this.Player));
			this._orbContainer.AddChildSafely(norb2);
			this._orbs.Add(norb2);
			norb2.Position = Vector2.Zero;
			if (norb.HasFocus())
			{
				this._creatureNode.Hitbox.TryGrabFocus();
			}
			this.TweenLayout();
			this.UpdateControllerNavigation();
		}

		// Token: 0x060035CD RID: 13773 RVA: 0x00191604 File Offset: 0x0018F804
		private void UpdateControllerNavigation()
		{
			for (int i = 0; i < this._orbs.Count; i++)
			{
				Control control = this._orbs[i];
				NodePath nodePath;
				if (i <= 0)
				{
					List<NOrb> orbs = this._orbs;
					nodePath = orbs[orbs.Count - 1].GetPath();
				}
				else
				{
					nodePath = this._orbs[i - 1].GetPath();
				}
				control.FocusNeighborRight = nodePath;
				this._orbs[i].FocusNeighborLeft = ((i < this._orbs.Count - 1) ? this._orbs[i + 1].GetPath() : this._orbs[0].GetPath());
				this._orbs[i].FocusNeighborTop = this._orbs[i].GetPath();
				this._orbs[i].FocusNeighborBottom = this._creatureNode.Hitbox.GetPath();
			}
			this._creatureNode.UpdateNavigation();
		}

		// Token: 0x060035CE RID: 13774 RVA: 0x00191704 File Offset: 0x0018F904
		private void TweenLayout()
		{
			int capacity = this.Player.PlayerCombatState.OrbQueue.Capacity;
			if (capacity == 0)
			{
				return;
			}
			float num = 125f;
			float num2 = num / (float)(capacity - 1);
			float num3 = Mathf.Lerp(225f, 300f, ((float)capacity - 3f) / 7f);
			if (!this.IsLocal)
			{
				num3 *= 0.75f;
			}
			Tween curTween = this._curTween;
			if (curTween != null)
			{
				curTween.Kill();
			}
			this._curTween = base.CreateTween().SetParallel(true);
			for (int i = 0; i < capacity; i++)
			{
				float num4 = float.DegreesToRadians(-25f - num);
				Vector2 vector = new Vector2(-Mathf.Cos(num4), Mathf.Sin(num4)) * num3;
				this._curTween.TweenProperty(this._orbs[i], "position", vector, 0.44999998807907104).SetEase(2L).SetTrans(1L);
				num -= num2;
			}
		}

		// Token: 0x060035CF RID: 13775 RVA: 0x00191808 File Offset: 0x0018FA08
		private void OnCombatStateChanged(CombatState _)
		{
			this.UpdateVisuals(OrbEvokeType.None);
		}

		// Token: 0x060035D0 RID: 13776 RVA: 0x00191814 File Offset: 0x0018FA14
		public void UpdateVisuals(OrbEvokeType evokeType)
		{
			foreach (NOrb norb in this._orbs)
			{
				norb.UpdateVisuals(false);
			}
			switch (evokeType)
			{
			case OrbEvokeType.None:
				break;
			case OrbEvokeType.Front:
			{
				NOrb norb2 = this._orbs.FirstOrDefault<NOrb>();
				if (norb2 == null)
				{
					return;
				}
				norb2.UpdateVisuals(true);
				return;
			}
			case OrbEvokeType.All:
				foreach (NOrb norb3 in this._orbs)
				{
					norb3.UpdateVisuals(true);
				}
				break;
			default:
				return;
			}
		}

		// Token: 0x060035D1 RID: 13777 RVA: 0x001918D4 File Offset: 0x0018FAD4
		public void ClearOrbs()
		{
			Tween curTween = this._curTween;
			if (curTween != null)
			{
				curTween.Kill();
			}
			if (this._orbs.Count == 0)
			{
				return;
			}
			this._curTween = base.CreateTween();
			foreach (NOrb norb in this._orbs)
			{
				this._curTween.Parallel().TweenProperty(norb, "position", Vector2.Zero, 1.0).SetEase(2L)
					.SetTrans(1L);
				this._curTween.Parallel().TweenProperty(norb, "modulate:a", 0, 0.25);
			}
			foreach (NOrb norb2 in this._orbs)
			{
				this._curTween.Chain().TweenCallback(Callable.From(new Action(norb2.QueueFreeSafely)));
			}
			this._orbs.Clear();
		}

		// Token: 0x1700084D RID: 2125
		// (get) Token: 0x060035D2 RID: 13778 RVA: 0x00191A1C File Offset: 0x0018FC1C
		public Control DefaultFocusOwner
		{
			get
			{
				if (this._orbs.Count <= 0)
				{
					return this._creatureNode.Hitbox;
				}
				return this._orbs.First<NOrb>();
			}
		}

		/// <summary>
		/// Get the method information for all the methods declared in this class.
		/// This method is used by Godot to register the available methods in the editor.
		/// Do not call this method.
		/// </summary>
		// Token: 0x060035D3 RID: 13779 RVA: 0x00191A44 File Offset: 0x0018FC44
		[NullableContext(0)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal static List<MethodInfo> GetGodotMethodList()
		{
			return new List<MethodInfo>(11)
			{
				new MethodInfo(NOrbManager.MethodName._Ready, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NOrbManager.MethodName._EnterTree, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NOrbManager.MethodName._ExitTree, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NOrbManager.MethodName.Create, new PropertyInfo(24L, "", 0L, "", 6L, new StringName("Control"), false), 33L, new List<PropertyInfo>
				{
					new PropertyInfo(24L, "creature", 0L, "", 6L, new StringName("Control"), false),
					new PropertyInfo(1L, "isLocal", 0L, "", 6L, false)
				}, null),
				new MethodInfo(NOrbManager.MethodName.RemoveSlotAnim, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, new List<PropertyInfo>
				{
					new PropertyInfo(2L, "amount", 0L, "", 6L, false)
				}, null),
				new MethodInfo(NOrbManager.MethodName.AddSlotAnim, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, new List<PropertyInfo>
				{
					new PropertyInfo(2L, "amount", 0L, "", 6L, false)
				}, null),
				new MethodInfo(NOrbManager.MethodName.AddOrbAnim, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NOrbManager.MethodName.UpdateControllerNavigation, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NOrbManager.MethodName.TweenLayout, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NOrbManager.MethodName.UpdateVisuals, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, new List<PropertyInfo>
				{
					new PropertyInfo(2L, "evokeType", 0L, "", 6L, false)
				}, null),
				new MethodInfo(NOrbManager.MethodName.ClearOrbs, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null)
			};
		}

		/// <inheritdoc />
		// Token: 0x060035D4 RID: 13780 RVA: 0x00191D2C File Offset: 0x0018FF2C
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool InvokeGodotClassMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
		{
			if ((ref method) == NOrbManager.MethodName._Ready && args.Count == 0)
			{
				this._Ready();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NOrbManager.MethodName._EnterTree && args.Count == 0)
			{
				this._EnterTree();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NOrbManager.MethodName._ExitTree && args.Count == 0)
			{
				this._ExitTree();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NOrbManager.MethodName.Create && args.Count == 2)
			{
				NOrbManager norbManager = NOrbManager.Create(VariantUtils.ConvertTo<NCreature>(args[0]), VariantUtils.ConvertTo<bool>(args[1]));
				ret = VariantUtils.CreateFrom<NOrbManager>(ref norbManager);
				return true;
			}
			if ((ref method) == NOrbManager.MethodName.RemoveSlotAnim && args.Count == 1)
			{
				this.RemoveSlotAnim(VariantUtils.ConvertTo<int>(args[0]));
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NOrbManager.MethodName.AddSlotAnim && args.Count == 1)
			{
				this.AddSlotAnim(VariantUtils.ConvertTo<int>(args[0]));
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NOrbManager.MethodName.AddOrbAnim && args.Count == 0)
			{
				this.AddOrbAnim();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NOrbManager.MethodName.UpdateControllerNavigation && args.Count == 0)
			{
				this.UpdateControllerNavigation();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NOrbManager.MethodName.TweenLayout && args.Count == 0)
			{
				this.TweenLayout();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NOrbManager.MethodName.UpdateVisuals && args.Count == 1)
			{
				this.UpdateVisuals(VariantUtils.ConvertTo<OrbEvokeType>(args[0]));
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NOrbManager.MethodName.ClearOrbs && args.Count == 0)
			{
				this.ClearOrbs();
				ret = default(godot_variant);
				return true;
			}
			return base.InvokeGodotClassMethod(ref method, args, ref ret);
		}

		// Token: 0x060035D5 RID: 13781 RVA: 0x00191F24 File Offset: 0x00190124
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal static bool InvokeGodotClassStaticMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
		{
			if ((ref method) == NOrbManager.MethodName.Create && args.Count == 2)
			{
				NOrbManager norbManager = NOrbManager.Create(VariantUtils.ConvertTo<NCreature>(args[0]), VariantUtils.ConvertTo<bool>(args[1]));
				ret = VariantUtils.CreateFrom<NOrbManager>(ref norbManager);
				return true;
			}
			ret = default(godot_variant);
			return false;
		}

		/// <inheritdoc />
		// Token: 0x060035D6 RID: 13782 RVA: 0x00191F80 File Offset: 0x00190180
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool HasGodotClassMethod(in godot_string_name method)
		{
			return (ref method) == NOrbManager.MethodName._Ready || (ref method) == NOrbManager.MethodName._EnterTree || (ref method) == NOrbManager.MethodName._ExitTree || (ref method) == NOrbManager.MethodName.Create || (ref method) == NOrbManager.MethodName.RemoveSlotAnim || (ref method) == NOrbManager.MethodName.AddSlotAnim || (ref method) == NOrbManager.MethodName.AddOrbAnim || (ref method) == NOrbManager.MethodName.UpdateControllerNavigation || (ref method) == NOrbManager.MethodName.TweenLayout || (ref method) == NOrbManager.MethodName.UpdateVisuals || (ref method) == NOrbManager.MethodName.ClearOrbs || base.HasGodotClassMethod(ref method);
		}

		/// <inheritdoc />
		// Token: 0x060035D7 RID: 13783 RVA: 0x0019203C File Offset: 0x0019023C
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool SetGodotClassPropertyValue(in godot_string_name name, in godot_variant value)
		{
			if ((ref name) == NOrbManager.PropertyName.IsLocal)
			{
				this.IsLocal = VariantUtils.ConvertTo<bool>(ref value);
				return true;
			}
			if ((ref name) == NOrbManager.PropertyName._orbContainer)
			{
				this._orbContainer = VariantUtils.ConvertTo<Control>(ref value);
				return true;
			}
			if ((ref name) == NOrbManager.PropertyName._creatureNode)
			{
				this._creatureNode = VariantUtils.ConvertTo<NCreature>(ref value);
				return true;
			}
			if ((ref name) == NOrbManager.PropertyName._curTween)
			{
				this._curTween = VariantUtils.ConvertTo<Tween>(ref value);
				return true;
			}
			return base.SetGodotClassPropertyValue(ref name, ref value);
		}

		/// <inheritdoc />
		// Token: 0x060035D8 RID: 13784 RVA: 0x001920C0 File Offset: 0x001902C0
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
		{
			if ((ref name) == NOrbManager.PropertyName.IsLocal)
			{
				bool isLocal = this.IsLocal;
				value = VariantUtils.CreateFrom<bool>(ref isLocal);
				return true;
			}
			if ((ref name) == NOrbManager.PropertyName.DefaultFocusOwner)
			{
				Control defaultFocusOwner = this.DefaultFocusOwner;
				value = VariantUtils.CreateFrom<Control>(ref defaultFocusOwner);
				return true;
			}
			if ((ref name) == NOrbManager.PropertyName._orbContainer)
			{
				value = VariantUtils.CreateFrom<Control>(ref this._orbContainer);
				return true;
			}
			if ((ref name) == NOrbManager.PropertyName._creatureNode)
			{
				value = VariantUtils.CreateFrom<NCreature>(ref this._creatureNode);
				return true;
			}
			if ((ref name) == NOrbManager.PropertyName._curTween)
			{
				value = VariantUtils.CreateFrom<Tween>(ref this._curTween);
				return true;
			}
			return base.GetGodotClassPropertyValue(ref name, ref value);
		}

		/// <summary>
		/// Get the property information for all the properties declared in this class.
		/// This method is used by Godot to register the available properties in the editor.
		/// Do not call this method.
		/// </summary>
		// Token: 0x060035D9 RID: 13785 RVA: 0x0019217C File Offset: 0x0019037C
		[NullableContext(0)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal static List<PropertyInfo> GetGodotPropertyList()
		{
			return new List<PropertyInfo>
			{
				new PropertyInfo(1L, NOrbManager.PropertyName.IsLocal, 0L, "", 4096L, false),
				new PropertyInfo(24L, NOrbManager.PropertyName._orbContainer, 0L, "", 4096L, false),
				new PropertyInfo(24L, NOrbManager.PropertyName._creatureNode, 0L, "", 4096L, false),
				new PropertyInfo(24L, NOrbManager.PropertyName._curTween, 0L, "", 4096L, false),
				new PropertyInfo(24L, NOrbManager.PropertyName.DefaultFocusOwner, 0L, "", 4096L, false)
			};
		}

		/// <inheritdoc />
		// Token: 0x060035DA RID: 13786 RVA: 0x00192234 File Offset: 0x00190434
		[NullableContext(0)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void SaveGodotObjectData(GodotSerializationInfo info)
		{
			base.SaveGodotObjectData(info);
			StringName isLocal = NOrbManager.PropertyName.IsLocal;
			bool isLocal2 = this.IsLocal;
			info.AddProperty(isLocal, Variant.From<bool>(ref isLocal2));
			info.AddProperty(NOrbManager.PropertyName._orbContainer, Variant.From<Control>(ref this._orbContainer));
			info.AddProperty(NOrbManager.PropertyName._creatureNode, Variant.From<NCreature>(ref this._creatureNode));
			info.AddProperty(NOrbManager.PropertyName._curTween, Variant.From<Tween>(ref this._curTween));
		}

		/// <inheritdoc />
		// Token: 0x060035DB RID: 13787 RVA: 0x001922A4 File Offset: 0x001904A4
		[NullableContext(0)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void RestoreGodotObjectData(GodotSerializationInfo info)
		{
			base.RestoreGodotObjectData(info);
			Variant variant;
			if (info.TryGetProperty(NOrbManager.PropertyName.IsLocal, ref variant))
			{
				this.IsLocal = variant.As<bool>();
			}
			Variant variant2;
			if (info.TryGetProperty(NOrbManager.PropertyName._orbContainer, ref variant2))
			{
				this._orbContainer = variant2.As<Control>();
			}
			Variant variant3;
			if (info.TryGetProperty(NOrbManager.PropertyName._creatureNode, ref variant3))
			{
				this._creatureNode = variant3.As<NCreature>();
			}
			Variant variant4;
			if (info.TryGetProperty(NOrbManager.PropertyName._curTween, ref variant4))
			{
				this._curTween = variant4.As<Tween>();
			}
		}

		// Token: 0x040014D1 RID: 5329
		private Control _orbContainer;

		// Token: 0x040014D2 RID: 5330
		private readonly List<NOrb> _orbs = new List<NOrb>();

		// Token: 0x040014D3 RID: 5331
		private NCreature _creatureNode;

		// Token: 0x040014D4 RID: 5332
		private const float _minRadius = 225f;

		// Token: 0x040014D5 RID: 5333
		private const float _maxRadius = 300f;

		// Token: 0x040014D6 RID: 5334
		private const float _range = 150f;

		// Token: 0x040014D7 RID: 5335
		private const float _angleOffset = -25f;

		// Token: 0x040014D8 RID: 5336
		private const float _tweenSpeed = 0.45f;

		// Token: 0x040014D9 RID: 5337
		[Nullable(2)]
		private Tween _curTween;

		/// <summary>
		/// Cached StringNames for the methods contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x0200170A RID: 5898
		[NullableContext(0)]
		public class MethodName : Control.MethodName
		{
			/// <summary>
			/// Cached name for the '_Ready' method.
			/// </summary>
			// Token: 0x040059D0 RID: 22992
			public static readonly StringName _Ready = "_Ready";

			/// <summary>
			/// Cached name for the '_EnterTree' method.
			/// </summary>
			// Token: 0x040059D1 RID: 22993
			public static readonly StringName _EnterTree = "_EnterTree";

			/// <summary>
			/// Cached name for the '_ExitTree' method.
			/// </summary>
			// Token: 0x040059D2 RID: 22994
			public static readonly StringName _ExitTree = "_ExitTree";

			/// <summary>
			/// Cached name for the 'Create' method.
			/// </summary>
			// Token: 0x040059D3 RID: 22995
			public static readonly StringName Create = "Create";

			/// <summary>
			/// Cached name for the 'RemoveSlotAnim' method.
			/// </summary>
			// Token: 0x040059D4 RID: 22996
			public static readonly StringName RemoveSlotAnim = "RemoveSlotAnim";

			/// <summary>
			/// Cached name for the 'AddSlotAnim' method.
			/// </summary>
			// Token: 0x040059D5 RID: 22997
			public static readonly StringName AddSlotAnim = "AddSlotAnim";

			/// <summary>
			/// Cached name for the 'AddOrbAnim' method.
			/// </summary>
			// Token: 0x040059D6 RID: 22998
			public static readonly StringName AddOrbAnim = "AddOrbAnim";

			/// <summary>
			/// Cached name for the 'UpdateControllerNavigation' method.
			/// </summary>
			// Token: 0x040059D7 RID: 22999
			public static readonly StringName UpdateControllerNavigation = "UpdateControllerNavigation";

			/// <summary>
			/// Cached name for the 'TweenLayout' method.
			/// </summary>
			// Token: 0x040059D8 RID: 23000
			public static readonly StringName TweenLayout = "TweenLayout";

			/// <summary>
			/// Cached name for the 'UpdateVisuals' method.
			/// </summary>
			// Token: 0x040059D9 RID: 23001
			public static readonly StringName UpdateVisuals = "UpdateVisuals";

			/// <summary>
			/// Cached name for the 'ClearOrbs' method.
			/// </summary>
			// Token: 0x040059DA RID: 23002
			public static readonly StringName ClearOrbs = "ClearOrbs";
		}

		/// <summary>
		/// Cached StringNames for the properties and fields contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x0200170B RID: 5899
		[NullableContext(0)]
		public class PropertyName : Control.PropertyName
		{
			/// <summary>
			/// Cached name for the 'IsLocal' property.
			/// </summary>
			// Token: 0x040059DB RID: 23003
			public static readonly StringName IsLocal = "IsLocal";

			/// <summary>
			/// Cached name for the 'DefaultFocusOwner' property.
			/// </summary>
			// Token: 0x040059DC RID: 23004
			public static readonly StringName DefaultFocusOwner = "DefaultFocusOwner";

			/// <summary>
			/// Cached name for the '_orbContainer' field.
			/// </summary>
			// Token: 0x040059DD RID: 23005
			public static readonly StringName _orbContainer = "_orbContainer";

			/// <summary>
			/// Cached name for the '_creatureNode' field.
			/// </summary>
			// Token: 0x040059DE RID: 23006
			public static readonly StringName _creatureNode = "_creatureNode";

			/// <summary>
			/// Cached name for the '_curTween' field.
			/// </summary>
			// Token: 0x040059DF RID: 23007
			public static readonly StringName _curTween = "_curTween";
		}

		/// <summary>
		/// Cached StringNames for the signals contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x0200170C RID: 5900
		[NullableContext(0)]
		public class SignalName : Control.SignalName
		{
		}
	}
}
