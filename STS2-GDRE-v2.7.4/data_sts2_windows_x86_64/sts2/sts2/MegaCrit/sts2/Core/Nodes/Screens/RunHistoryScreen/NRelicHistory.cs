using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using Godot;
using Godot.Bridge;
using Godot.NativeInterop;
using MegaCrit.Sts2.addons.mega_text;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Entities.Relics;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.Localization;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Exceptions;
using MegaCrit.Sts2.Core.Models.Relics;
using MegaCrit.Sts2.Core.Nodes.GodotExtensions;
using MegaCrit.Sts2.Core.Nodes.Relics;
using MegaCrit.Sts2.Core.Saves.Runs;

namespace MegaCrit.Sts2.Core.Nodes.Screens.RunHistoryScreen
{
	// Token: 0x020002D2 RID: 722
	[ScriptPath("res://src/Core/Nodes/Screens/RunHistoryScreen/NRelicHistory.cs")]
	public class NRelicHistory : VBoxContainer
	{
		// Token: 0x06002643 RID: 9795 RVA: 0x00102979 File Offset: 0x00100B79
		public override void _Ready()
		{
			this._headerLabel = base.GetNode<MegaRichTextLabel>("Header");
			this._relicsContainer = base.GetNode<Control>("%RelicsContainer");
		}

		// Token: 0x06002644 RID: 9796 RVA: 0x001029A8 File Offset: 0x00100BA8
		[NullableContext(1)]
		public void LoadRelics(Player player, IEnumerable<SerializableRelic> relics)
		{
			StringBuilder stringBuilder = new StringBuilder();
			foreach (Node node in this._relicsContainer.GetChildren(false))
			{
				node.QueueFreeSafely();
			}
			Dictionary<RelicRarity, int> dictionary = new Dictionary<RelicRarity, int>();
			foreach (RelicRarity relicRarity in Enum.GetValues<RelicRarity>())
			{
				dictionary.Add(relicRarity, 0);
			}
			List<SerializableRelic> list = relics.ToList<SerializableRelic>();
			foreach (SerializableRelic serializableRelic in list)
			{
				RelicModel relicModel;
				try
				{
					relicModel = RelicModel.FromSerializable(serializableRelic);
				}
				catch (ModelNotFoundException)
				{
					relicModel = ModelDb.Relic<DeprecatedRelic>().ToMutable();
				}
				relicModel.Owner = player;
				NRelicBasicHolder holder = NRelicBasicHolder.Create(relicModel);
				holder.MouseDefaultCursorShape = 16L;
				this._relicsContainer.AddChildSafely(holder);
				holder.Connect(Control.SignalName.FocusEntered, Callable.From(delegate
				{
					this.EmitSignal(NRelicHistory.SignalName.Hovered, new Variant[] { holder });
				}), 0U);
				holder.Connect(Control.SignalName.FocusExited, Callable.From(delegate
				{
					this.EmitSignal(NRelicHistory.SignalName.Unhovered, new Variant[] { holder });
				}), 0U);
				holder.Connect(Control.SignalName.MouseEntered, Callable.From(delegate
				{
					this.EmitSignal(NRelicHistory.SignalName.Hovered, new Variant[] { holder });
				}), 0U);
				holder.Connect(Control.SignalName.MouseExited, Callable.From(delegate
				{
					this.EmitSignal(NRelicHistory.SignalName.Unhovered, new Variant[] { holder });
				}), 0U);
				holder.Connect(NClickableControl.SignalName.Released, Callable.From<NButton>(delegate(NButton _)
				{
					this.OnRelicClicked(holder.Relic);
				}), 0U);
				Dictionary<RelicRarity, int> dictionary2 = dictionary;
				RelicRarity rarity = relicModel.Rarity;
				int num = dictionary2[rarity];
				dictionary2[rarity] = num + 1;
			}
			this._relicHeader.Add("totalRelics", list.Count);
			foreach (KeyValuePair<RelicRarity, int> keyValuePair in dictionary)
			{
				this._relicCategories.Add(keyValuePair.Key.ToString() + "Relics", keyValuePair.Value);
			}
			StringBuilder stringBuilder2 = stringBuilder;
			StringBuilder stringBuilder3 = stringBuilder2;
			StringBuilder.AppendInterpolatedStringHandler appendInterpolatedStringHandler = new StringBuilder.AppendInterpolatedStringHandler(20, 1, stringBuilder2);
			appendInterpolatedStringHandler.AppendLiteral("[gold][b]");
			appendInterpolatedStringHandler.AppendFormatted(this._relicHeader.GetFormattedText());
			appendInterpolatedStringHandler.AppendLiteral("[/b][/gold]");
			stringBuilder3.Append(ref appendInterpolatedStringHandler);
			stringBuilder.Append(this._relicCategories.GetFormattedText().Trim(','));
			this._headerLabel.Text = stringBuilder.ToString();
		}

		/// <summary>
		/// Opens the Inspect Relic screen and allows the player to paginate through the rest of the top bar relics.
		/// </summary>
		/// <param name="node"></param>
		// Token: 0x06002645 RID: 9797 RVA: 0x00102CE4 File Offset: 0x00100EE4
		[NullableContext(1)]
		private void OnRelicClicked(NRelic node)
		{
			List<RelicModel> list = new List<RelicModel>();
			foreach (NRelicBasicHolder nrelicBasicHolder in this._relicsContainer.GetChildren(false).OfType<NRelicBasicHolder>())
			{
				list.Add(nrelicBasicHolder.Relic.Model);
			}
			NGame.Instance.GetInspectRelicScreen().Open(list, node.Model);
		}

		/// <summary>
		/// Get the method information for all the methods declared in this class.
		/// This method is used by Godot to register the available methods in the editor.
		/// Do not call this method.
		/// </summary>
		// Token: 0x06002646 RID: 9798 RVA: 0x00102D64 File Offset: 0x00100F64
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal static List<MethodInfo> GetGodotMethodList()
		{
			return new List<MethodInfo>(2)
			{
				new MethodInfo(NRelicHistory.MethodName._Ready, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NRelicHistory.MethodName.OnRelicClicked, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, new List<PropertyInfo>
				{
					new PropertyInfo(24L, "node", 0L, "", 6L, new StringName("Control"), false)
				}, null)
			};
		}

		/// <inheritdoc />
		// Token: 0x06002647 RID: 9799 RVA: 0x00102E08 File Offset: 0x00101008
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool InvokeGodotClassMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
		{
			if ((ref method) == NRelicHistory.MethodName._Ready && args.Count == 0)
			{
				this._Ready();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NRelicHistory.MethodName.OnRelicClicked && args.Count == 1)
			{
				this.OnRelicClicked(VariantUtils.ConvertTo<NRelic>(args[0]));
				ret = default(godot_variant);
				return true;
			}
			return base.InvokeGodotClassMethod(ref method, args, ref ret);
		}

		/// <inheritdoc />
		// Token: 0x06002648 RID: 9800 RVA: 0x00102E76 File Offset: 0x00101076
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool HasGodotClassMethod(in godot_string_name method)
		{
			return (ref method) == NRelicHistory.MethodName._Ready || (ref method) == NRelicHistory.MethodName.OnRelicClicked || base.HasGodotClassMethod(ref method);
		}

		/// <inheritdoc />
		// Token: 0x06002649 RID: 9801 RVA: 0x00102E9D File Offset: 0x0010109D
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool SetGodotClassPropertyValue(in godot_string_name name, in godot_variant value)
		{
			if ((ref name) == NRelicHistory.PropertyName._headerLabel)
			{
				this._headerLabel = VariantUtils.ConvertTo<MegaRichTextLabel>(ref value);
				return true;
			}
			if ((ref name) == NRelicHistory.PropertyName._relicsContainer)
			{
				this._relicsContainer = VariantUtils.ConvertTo<Control>(ref value);
				return true;
			}
			return base.SetGodotClassPropertyValue(ref name, ref value);
		}

		/// <inheritdoc />
		// Token: 0x0600264A RID: 9802 RVA: 0x00102EE0 File Offset: 0x001010E0
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
		{
			if ((ref name) == NRelicHistory.PropertyName._headerLabel)
			{
				value = VariantUtils.CreateFrom<MegaRichTextLabel>(ref this._headerLabel);
				return true;
			}
			if ((ref name) == NRelicHistory.PropertyName._relicsContainer)
			{
				value = VariantUtils.CreateFrom<Control>(ref this._relicsContainer);
				return true;
			}
			return base.GetGodotClassPropertyValue(ref name, ref value);
		}

		/// <summary>
		/// Get the property information for all the properties declared in this class.
		/// This method is used by Godot to register the available properties in the editor.
		/// Do not call this method.
		/// </summary>
		// Token: 0x0600264B RID: 9803 RVA: 0x00102F38 File Offset: 0x00101138
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal static List<PropertyInfo> GetGodotPropertyList()
		{
			return new List<PropertyInfo>
			{
				new PropertyInfo(24L, NRelicHistory.PropertyName._headerLabel, 0L, "", 4096L, false),
				new PropertyInfo(24L, NRelicHistory.PropertyName._relicsContainer, 0L, "", 4096L, false)
			};
		}

		/// <inheritdoc />
		// Token: 0x0600264C RID: 9804 RVA: 0x00102F90 File Offset: 0x00101190
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void SaveGodotObjectData(GodotSerializationInfo info)
		{
			base.SaveGodotObjectData(info);
			info.AddProperty(NRelicHistory.PropertyName._headerLabel, Variant.From<MegaRichTextLabel>(ref this._headerLabel));
			info.AddProperty(NRelicHistory.PropertyName._relicsContainer, Variant.From<Control>(ref this._relicsContainer));
			info.AddSignalEventDelegate(NRelicHistory.SignalName.Hovered, this.backing_Hovered);
			info.AddSignalEventDelegate(NRelicHistory.SignalName.Unhovered, this.backing_Unhovered);
		}

		/// <inheritdoc />
		// Token: 0x0600264D RID: 9805 RVA: 0x00102FF4 File Offset: 0x001011F4
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void RestoreGodotObjectData(GodotSerializationInfo info)
		{
			base.RestoreGodotObjectData(info);
			Variant variant;
			if (info.TryGetProperty(NRelicHistory.PropertyName._headerLabel, ref variant))
			{
				this._headerLabel = variant.As<MegaRichTextLabel>();
			}
			Variant variant2;
			if (info.TryGetProperty(NRelicHistory.PropertyName._relicsContainer, ref variant2))
			{
				this._relicsContainer = variant2.As<Control>();
			}
			NRelicHistory.HoveredEventHandler hoveredEventHandler;
			if (info.TryGetSignalEventDelegate<NRelicHistory.HoveredEventHandler>(NRelicHistory.SignalName.Hovered, ref hoveredEventHandler))
			{
				this.backing_Hovered = hoveredEventHandler;
			}
			NRelicHistory.UnhoveredEventHandler unhoveredEventHandler;
			if (info.TryGetSignalEventDelegate<NRelicHistory.UnhoveredEventHandler>(NRelicHistory.SignalName.Unhovered, ref unhoveredEventHandler))
			{
				this.backing_Unhovered = unhoveredEventHandler;
			}
		}

		/// <summary>
		/// Get the signal information for all the signals declared in this class.
		/// This method is used by Godot to register the available signals in the editor.
		/// Do not call this method.
		/// </summary>
		// Token: 0x0600264E RID: 9806 RVA: 0x0010306C File Offset: 0x0010126C
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal static List<MethodInfo> GetGodotSignalList()
		{
			return new List<MethodInfo>(2)
			{
				new MethodInfo(NRelicHistory.SignalName.Hovered, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, new List<PropertyInfo>
				{
					new PropertyInfo(24L, "relic", 0L, "", 6L, new StringName("Control"), false)
				}, null),
				new MethodInfo(NRelicHistory.SignalName.Unhovered, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, new List<PropertyInfo>
				{
					new PropertyInfo(24L, "relic", 0L, "", 6L, new StringName("Control"), false)
				}, null)
			};
		}

		/// <inheritdoc cref="T:MegaCrit.Sts2.Core.Nodes.Screens.RunHistoryScreen.NRelicHistory.HoveredEventHandler" />
		// Token: 0x1400001E RID: 30
		// (add) Token: 0x0600264F RID: 9807 RVA: 0x0010313F File Offset: 0x0010133F
		// (remove) Token: 0x06002650 RID: 9808 RVA: 0x00103158 File Offset: 0x00101358
		public event NRelicHistory.HoveredEventHandler Hovered
		{
			add
			{
				this.backing_Hovered = (NRelicHistory.HoveredEventHandler)Delegate.Combine(this.backing_Hovered, value);
			}
			remove
			{
				this.backing_Hovered = (NRelicHistory.HoveredEventHandler)Delegate.Remove(this.backing_Hovered, value);
			}
		}

		// Token: 0x06002651 RID: 9809 RVA: 0x00103171 File Offset: 0x00101371
		protected void EmitSignalHovered(NRelicBasicHolder relic)
		{
			base.EmitSignal(NRelicHistory.SignalName.Hovered, new Variant[] { relic });
		}

		/// <inheritdoc cref="T:MegaCrit.Sts2.Core.Nodes.Screens.RunHistoryScreen.NRelicHistory.UnhoveredEventHandler" />
		// Token: 0x1400001F RID: 31
		// (add) Token: 0x06002652 RID: 9810 RVA: 0x00103192 File Offset: 0x00101392
		// (remove) Token: 0x06002653 RID: 9811 RVA: 0x001031AB File Offset: 0x001013AB
		public event NRelicHistory.UnhoveredEventHandler Unhovered
		{
			add
			{
				this.backing_Unhovered = (NRelicHistory.UnhoveredEventHandler)Delegate.Combine(this.backing_Unhovered, value);
			}
			remove
			{
				this.backing_Unhovered = (NRelicHistory.UnhoveredEventHandler)Delegate.Remove(this.backing_Unhovered, value);
			}
		}

		// Token: 0x06002654 RID: 9812 RVA: 0x001031C4 File Offset: 0x001013C4
		protected void EmitSignalUnhovered(NRelicBasicHolder relic)
		{
			base.EmitSignal(NRelicHistory.SignalName.Unhovered, new Variant[] { relic });
		}

		/// <inheritdoc />
		// Token: 0x06002655 RID: 9813 RVA: 0x001031E8 File Offset: 0x001013E8
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void RaiseGodotClassSignalCallbacks(in godot_string_name signal, NativeVariantPtrArgs args)
		{
			if ((ref signal) == NRelicHistory.SignalName.Hovered && args.Count == 1)
			{
				NRelicHistory.HoveredEventHandler hoveredEventHandler = this.backing_Hovered;
				if (hoveredEventHandler == null)
				{
					return;
				}
				hoveredEventHandler(VariantUtils.ConvertTo<NRelicBasicHolder>(args[0]));
				return;
			}
			else
			{
				if (!((ref signal) == NRelicHistory.SignalName.Unhovered) || args.Count != 1)
				{
					base.RaiseGodotClassSignalCallbacks(ref signal, args);
					return;
				}
				NRelicHistory.UnhoveredEventHandler unhoveredEventHandler = this.backing_Unhovered;
				if (unhoveredEventHandler == null)
				{
					return;
				}
				unhoveredEventHandler(VariantUtils.ConvertTo<NRelicBasicHolder>(args[0]));
				return;
			}
		}

		/// <inheritdoc />
		// Token: 0x06002656 RID: 9814 RVA: 0x00103267 File Offset: 0x00101467
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool HasGodotClassSignal(in godot_string_name signal)
		{
			return (ref signal) == NRelicHistory.SignalName.Hovered || (ref signal) == NRelicHistory.SignalName.Unhovered || base.HasGodotClassSignal(ref signal);
		}

		// Token: 0x04000E24 RID: 3620
		[Nullable(1)]
		private readonly LocString _relicHeader = new LocString("run_history", "RELIC_HISTORY.header");

		// Token: 0x04000E25 RID: 3621
		[Nullable(1)]
		private readonly LocString _relicCategories = new LocString("run_history", "RELIC_HISTORY.categories");

		// Token: 0x04000E26 RID: 3622
		[Nullable(1)]
		private MegaRichTextLabel _headerLabel;

		// Token: 0x04000E27 RID: 3623
		[Nullable(1)]
		private Control _relicsContainer;

		// Token: 0x04000E28 RID: 3624
		private NRelicHistory.HoveredEventHandler backing_Hovered;

		// Token: 0x04000E29 RID: 3625
		private NRelicHistory.UnhoveredEventHandler backing_Unhovered;

		// Token: 0x02001437 RID: 5175
		// (Invoke) Token: 0x0600A05C RID: 41052
		[Signal]
		public delegate void HoveredEventHandler(NRelicBasicHolder relic);

		// Token: 0x02001438 RID: 5176
		// (Invoke) Token: 0x0600A060 RID: 41056
		[Signal]
		public delegate void UnhoveredEventHandler(NRelicBasicHolder relic);

		/// <summary>
		/// Cached StringNames for the methods contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x02001439 RID: 5177
		public class MethodName : VBoxContainer.MethodName
		{
			/// <summary>
			/// Cached name for the '_Ready' method.
			/// </summary>
			// Token: 0x04004CC3 RID: 19651
			public static readonly StringName _Ready = "_Ready";

			/// <summary>
			/// Cached name for the 'OnRelicClicked' method.
			/// </summary>
			// Token: 0x04004CC4 RID: 19652
			public static readonly StringName OnRelicClicked = "OnRelicClicked";
		}

		/// <summary>
		/// Cached StringNames for the properties and fields contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x0200143A RID: 5178
		public class PropertyName : VBoxContainer.PropertyName
		{
			/// <summary>
			/// Cached name for the '_headerLabel' field.
			/// </summary>
			// Token: 0x04004CC5 RID: 19653
			public static readonly StringName _headerLabel = "_headerLabel";

			/// <summary>
			/// Cached name for the '_relicsContainer' field.
			/// </summary>
			// Token: 0x04004CC6 RID: 19654
			public static readonly StringName _relicsContainer = "_relicsContainer";
		}

		/// <summary>
		/// Cached StringNames for the signals contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x0200143B RID: 5179
		public class SignalName : VBoxContainer.SignalName
		{
			/// <summary>
			/// Cached name for the 'Hovered' signal.
			/// </summary>
			// Token: 0x04004CC7 RID: 19655
			public static readonly StringName Hovered = "Hovered";

			/// <summary>
			/// Cached name for the 'Unhovered' signal.
			/// </summary>
			// Token: 0x04004CC8 RID: 19656
			public static readonly StringName Unhovered = "Unhovered";
		}
	}
}
