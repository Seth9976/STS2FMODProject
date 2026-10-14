using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using Godot;
using Godot.Bridge;
using Godot.NativeInterop;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.Models;

namespace MegaCrit.Sts2.Core.Nodes.Combat
{
	// Token: 0x0200040B RID: 1035
	[ScriptPath("res://src/Core/Nodes/Combat/NPowerContainer.cs")]
	public class NPowerContainer : Control
	{
		// Token: 0x06004117 RID: 16663 RVA: 0x001F6F11 File Offset: 0x001F5111
		public override void _EnterTree()
		{
			base._EnterTree();
			this.ConnectCreatureSignals();
		}

		// Token: 0x06004118 RID: 16664 RVA: 0x001F6F1F File Offset: 0x001F511F
		public override void _ExitTree()
		{
			base._ExitTree();
			if (this._creature != null)
			{
				this._creature.PowerApplied -= this.OnPowerApplied;
				this._creature.PowerRemoved -= this.OnPowerRemoved;
			}
		}

		// Token: 0x06004119 RID: 16665 RVA: 0x001F6F60 File Offset: 0x001F5160
		private void ConnectCreatureSignals()
		{
			if (this._creature != null)
			{
				this._creature.PowerApplied -= this.OnPowerApplied;
				this._creature.PowerRemoved -= this.OnPowerRemoved;
				this._creature.PowerApplied += this.OnPowerApplied;
				this._creature.PowerRemoved += this.OnPowerRemoved;
			}
		}

		// Token: 0x0600411A RID: 16666 RVA: 0x001F6FD4 File Offset: 0x001F51D4
		[NullableContext(1)]
		public void SetCreatureBounds(Control bounds)
		{
			base.GlobalPosition = new Vector2(bounds.GlobalPosition.X, base.GlobalPosition.Y);
			base.Size = new Vector2(bounds.Size.X * bounds.Scale.X + 25f, base.Size.Y);
			this._originalPosition = new Vector2?(base.Position);
			this.UpdatePositions();
		}

		// Token: 0x0600411B RID: 16667 RVA: 0x001F704C File Offset: 0x001F524C
		[NullableContext(1)]
		private void Add(PowerModel power)
		{
			if (!power.IsVisible)
			{
				return;
			}
			NPower npower = NPower.Create(power);
			npower.Container = this;
			this._powerNodes.Add(npower);
			this.AddChildSafely(npower);
			this.UpdatePositions();
		}

		// Token: 0x0600411C RID: 16668 RVA: 0x001F708C File Offset: 0x001F528C
		[NullableContext(1)]
		private void Remove(PowerModel power)
		{
			if (!CombatManager.Instance.IsInProgress)
			{
				return;
			}
			NPower npower = this._powerNodes.FirstOrDefault<NPower>((NPower n) => n.Model == power);
			if (npower == null)
			{
				return;
			}
			this._powerNodes.Remove(npower);
			this.UpdatePositions();
			npower.QueueFreeSafely();
		}

		// Token: 0x0600411D RID: 16669 RVA: 0x001F70E8 File Offset: 0x001F52E8
		private void UpdatePositions()
		{
			if (this._powerNodes.Count == 0)
			{
				return;
			}
			float x = this._powerNodes[0].Size.X;
			float num = (float)Mathf.CeilToInt(base.Size.X / x);
			num = Mathf.Max((float)Mathf.CeilToInt((float)this._powerNodes.Count / 2f), num);
			for (int i = 0; i < this._powerNodes.Count; i++)
			{
				this._powerNodes[i].Position = new Vector2(x * ((float)i % num), Mathf.Floor((float)i / num) * x);
			}
			float num2 = x * Mathf.Min(num, (float)this._powerNodes.Count);
			base.Position = (this._originalPosition ?? base.Position) + Vector2.Left * Mathf.Max(0f, num2 - base.Size.X) / 2f;
		}

		// Token: 0x0600411E RID: 16670 RVA: 0x001F71F8 File Offset: 0x001F53F8
		[NullableContext(1)]
		public void SetCreature(Creature creature)
		{
			if (this._creature != null)
			{
				throw new InvalidOperationException("Creature was already set.");
			}
			this._creature = creature;
			this.ConnectCreatureSignals();
			foreach (PowerModel powerModel in this._creature.Powers)
			{
				this.Add(powerModel);
			}
		}

		// Token: 0x0600411F RID: 16671 RVA: 0x001F726C File Offset: 0x001F546C
		[NullableContext(1)]
		private void OnPowerApplied(PowerModel power)
		{
			this.Add(power);
		}

		// Token: 0x06004120 RID: 16672 RVA: 0x001F7275 File Offset: 0x001F5475
		[NullableContext(1)]
		private void OnPowerRemoved(PowerModel power)
		{
			this.Remove(power);
		}

		/// <summary>
		/// Get the method information for all the methods declared in this class.
		/// This method is used by Godot to register the available methods in the editor.
		/// Do not call this method.
		/// </summary>
		// Token: 0x06004121 RID: 16673 RVA: 0x001F7280 File Offset: 0x001F5480
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal static List<MethodInfo> GetGodotMethodList()
		{
			return new List<MethodInfo>(5)
			{
				new MethodInfo(NPowerContainer.MethodName._EnterTree, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NPowerContainer.MethodName._ExitTree, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NPowerContainer.MethodName.ConnectCreatureSignals, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NPowerContainer.MethodName.SetCreatureBounds, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, new List<PropertyInfo>
				{
					new PropertyInfo(24L, "bounds", 0L, "", 6L, new StringName("Control"), false)
				}, null),
				new MethodInfo(NPowerContainer.MethodName.UpdatePositions, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null)
			};
		}

		/// <inheritdoc />
		// Token: 0x06004122 RID: 16674 RVA: 0x001F73B0 File Offset: 0x001F55B0
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool InvokeGodotClassMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
		{
			if ((ref method) == NPowerContainer.MethodName._EnterTree && args.Count == 0)
			{
				this._EnterTree();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NPowerContainer.MethodName._ExitTree && args.Count == 0)
			{
				this._ExitTree();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NPowerContainer.MethodName.ConnectCreatureSignals && args.Count == 0)
			{
				this.ConnectCreatureSignals();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NPowerContainer.MethodName.SetCreatureBounds && args.Count == 1)
			{
				this.SetCreatureBounds(VariantUtils.ConvertTo<Control>(args[0]));
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NPowerContainer.MethodName.UpdatePositions && args.Count == 0)
			{
				this.UpdatePositions();
				ret = default(godot_variant);
				return true;
			}
			return base.InvokeGodotClassMethod(ref method, args, ref ret);
		}

		/// <inheritdoc />
		// Token: 0x06004123 RID: 16675 RVA: 0x001F7490 File Offset: 0x001F5690
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool HasGodotClassMethod(in godot_string_name method)
		{
			return (ref method) == NPowerContainer.MethodName._EnterTree || (ref method) == NPowerContainer.MethodName._ExitTree || (ref method) == NPowerContainer.MethodName.ConnectCreatureSignals || (ref method) == NPowerContainer.MethodName.SetCreatureBounds || (ref method) == NPowerContainer.MethodName.UpdatePositions || base.HasGodotClassMethod(ref method);
		}

		/// <inheritdoc />
		// Token: 0x06004124 RID: 16676 RVA: 0x001F74EF File Offset: 0x001F56EF
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void SaveGodotObjectData(GodotSerializationInfo info)
		{
			base.SaveGodotObjectData(info);
		}

		/// <inheritdoc />
		// Token: 0x06004125 RID: 16677 RVA: 0x001F74F8 File Offset: 0x001F56F8
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void RestoreGodotObjectData(GodotSerializationInfo info)
		{
			base.RestoreGodotObjectData(info);
		}

		// Token: 0x0400194D RID: 6477
		[Nullable(2)]
		private Creature _creature;

		// Token: 0x0400194E RID: 6478
		private Vector2? _originalPosition;

		// Token: 0x0400194F RID: 6479
		[Nullable(1)]
		private readonly List<NPower> _powerNodes = new List<NPower>();

		/// <summary>
		/// Cached StringNames for the methods contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x02001919 RID: 6425
		public class MethodName : Control.MethodName
		{
			/// <summary>
			/// Cached name for the '_EnterTree' method.
			/// </summary>
			// Token: 0x040062D0 RID: 25296
			public static readonly StringName _EnterTree = "_EnterTree";

			/// <summary>
			/// Cached name for the '_ExitTree' method.
			/// </summary>
			// Token: 0x040062D1 RID: 25297
			public static readonly StringName _ExitTree = "_ExitTree";

			/// <summary>
			/// Cached name for the 'ConnectCreatureSignals' method.
			/// </summary>
			// Token: 0x040062D2 RID: 25298
			public static readonly StringName ConnectCreatureSignals = "ConnectCreatureSignals";

			/// <summary>
			/// Cached name for the 'SetCreatureBounds' method.
			/// </summary>
			// Token: 0x040062D3 RID: 25299
			public static readonly StringName SetCreatureBounds = "SetCreatureBounds";

			/// <summary>
			/// Cached name for the 'UpdatePositions' method.
			/// </summary>
			// Token: 0x040062D4 RID: 25300
			public static readonly StringName UpdatePositions = "UpdatePositions";
		}

		/// <summary>
		/// Cached StringNames for the properties and fields contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x0200191A RID: 6426
		public class PropertyName : Control.PropertyName
		{
		}

		/// <summary>
		/// Cached StringNames for the signals contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x0200191B RID: 6427
		public class SignalName : Control.SignalName
		{
		}
	}
}
