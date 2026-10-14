using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using Godot;
using Godot.Bridge;
using Godot.NativeInterop;

namespace MegaCrit.Sts2.Core.Nodes.Screens.Shops
{
	/// <summary>
	/// Manages the shop items for the fake merchant shop. We need this because we need to override the default controller navigation
	/// </summary>
	// Token: 0x02000279 RID: 633
	[ScriptPath("res://src/Core/Nodes/Screens/Shops/NFakeMerchantInventory.cs")]
	public class NFakeMerchantInventory : NMerchantInventory
	{
		// Token: 0x06002184 RID: 8580 RVA: 0x000E441C File Offset: 0x000E261C
		protected override void UpdateNavigation()
		{
			Control relicContainer = this._relicContainer;
			List<NMerchantSlot> list = ((relicContainer != null) ? relicContainer.GetChildren(false).OfType<NMerchantSlot>().ToList<NMerchantSlot>() : null) ?? new List<NMerchantSlot>();
			List<NMerchantSlot> list2 = new List<NMerchantSlot>(new <>z__ReadOnlyArray<NMerchantSlot>(new NMerchantSlot[]
			{
				list[0],
				list[1]
			})).Where<NMerchantSlot>((NMerchantSlot r) => r.Entry.IsStocked).ToList<NMerchantSlot>();
			List<NMerchantSlot> list3 = new List<NMerchantSlot>(new <>z__ReadOnlyArray<NMerchantSlot>(new NMerchantSlot[]
			{
				list[2],
				list[3],
				list[4]
			})).Where<NMerchantSlot>((NMerchantSlot r) => r.Entry.IsStocked).ToList<NMerchantSlot>();
			List<NMerchantSlot> list4 = new List<NMerchantSlot>(new <>z__ReadOnlySingleElementList<NMerchantSlot>(list[5])).Where<NMerchantSlot>((NMerchantSlot r) => r.Entry.IsStocked).ToList<NMerchantSlot>();
			List<List<NMerchantSlot>> list5 = new List<List<NMerchantSlot>>(new <>z__ReadOnlyArray<List<NMerchantSlot>>(new List<NMerchantSlot>[] { list2, list3, list4 })).Where<List<NMerchantSlot>>((List<NMerchantSlot> r) => r.Count > 0).ToList<List<NMerchantSlot>>();
			for (int i = 0; i < list5.Count; i++)
			{
				for (int j = 0; j < list5[i].Count; j++)
				{
					list5[i][j].FocusNeighborLeft = ((j > 0) ? list5[i][j - 1].GetPath() : list5[i][j].GetPath());
					list5[i][j].FocusNeighborRight = ((j < list5[i].Count - 1) ? list5[i][j + 1].GetPath() : list5[i][j].GetPath());
					if (i > 0)
					{
						list5[i][j].FocusNeighborTop = ((j < list5[i - 1].Count) ? list5[i - 1][j].GetPath() : list5[i - 1][list5[i - 1].Count - 1].GetPath());
					}
					else
					{
						list5[i][j].FocusNeighborTop = list5[i][j].GetPath();
					}
					if (i < list5.Count - 1)
					{
						list5[i][j].FocusNeighborBottom = ((j < list5[i + 1].Count) ? list5[i + 1][j].GetPath() : list5[i + 1][list5[i + 1].Count - 1].GetPath());
					}
					else
					{
						list5[i][j].FocusNeighborBottom = list5[i][j].GetPath();
					}
				}
			}
		}

		/// <summary>
		/// Get the method information for all the methods declared in this class.
		/// This method is used by Godot to register the available methods in the editor.
		/// Do not call this method.
		/// </summary>
		// Token: 0x06002185 RID: 8581 RVA: 0x000E4794 File Offset: 0x000E2994
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal new static List<MethodInfo> GetGodotMethodList()
		{
			return new List<MethodInfo>(1)
			{
				new MethodInfo(NFakeMerchantInventory.MethodName.UpdateNavigation, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null)
			};
		}

		/// <inheritdoc />
		// Token: 0x06002186 RID: 8582 RVA: 0x000E47D8 File Offset: 0x000E29D8
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool InvokeGodotClassMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
		{
			if ((ref method) == NFakeMerchantInventory.MethodName.UpdateNavigation && args.Count == 0)
			{
				this.UpdateNavigation();
				ret = default(godot_variant);
				return true;
			}
			return base.InvokeGodotClassMethod(in method, args, out ret);
		}

		/// <inheritdoc />
		// Token: 0x06002187 RID: 8583 RVA: 0x000E4808 File Offset: 0x000E2A08
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool HasGodotClassMethod(in godot_string_name method)
		{
			return (ref method) == NFakeMerchantInventory.MethodName.UpdateNavigation || base.HasGodotClassMethod(in method);
		}

		/// <inheritdoc />
		// Token: 0x06002188 RID: 8584 RVA: 0x000E4820 File Offset: 0x000E2A20
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void SaveGodotObjectData(GodotSerializationInfo info)
		{
			base.SaveGodotObjectData(info);
		}

		/// <inheritdoc />
		// Token: 0x06002189 RID: 8585 RVA: 0x000E4829 File Offset: 0x000E2A29
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void RestoreGodotObjectData(GodotSerializationInfo info)
		{
			base.RestoreGodotObjectData(info);
		}

		/// <summary>
		/// Cached StringNames for the methods contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x02001311 RID: 4881
		public new class MethodName : NMerchantInventory.MethodName
		{
			/// <summary>
			/// Cached name for the 'UpdateNavigation' method.
			/// </summary>
			// Token: 0x04004A24 RID: 18980
			public new static readonly StringName UpdateNavigation = "UpdateNavigation";
		}

		/// <summary>
		/// Cached StringNames for the properties and fields contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x02001312 RID: 4882
		public new class PropertyName : NMerchantInventory.PropertyName
		{
		}

		/// <summary>
		/// Cached StringNames for the signals contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x02001313 RID: 4883
		public new class SignalName : NMerchantInventory.SignalName
		{
		}
	}
}
