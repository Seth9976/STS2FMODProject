using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Godot;
using Godot.Bridge;
using Godot.NativeInterop;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Nodes.Combat;

namespace MegaCrit.Sts2.Core.Nodes.Screens.Bestiary
{
	// Token: 0x02000361 RID: 865
	[ScriptPath("res://src/Core/Nodes/Screens/Bestiary/NBestiaryLayout.cs")]
	public abstract class NBestiaryLayout : Control
	{
		// Token: 0x06003251 RID: 12881
		public abstract void Cleanup();

		// Token: 0x06003252 RID: 12882
		[NullableContext(1)]
		public abstract List<BestiaryMonsterMove> Setup(BestiaryEntry entry, Tween tween);

		// Token: 0x06003253 RID: 12883
		[NullableContext(1)]
		public abstract IEnumerable<NCreature> GetCreatures();

		/// <summary>
		/// Get the method information for all the methods declared in this class.
		/// This method is used by Godot to register the available methods in the editor.
		/// Do not call this method.
		/// </summary>
		// Token: 0x06003254 RID: 12884 RVA: 0x001777F4 File Offset: 0x001759F4
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal static List<MethodInfo> GetGodotMethodList()
		{
			return new List<MethodInfo>(1)
			{
				new MethodInfo(NBestiaryLayout.MethodName.Cleanup, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null)
			};
		}

		/// <inheritdoc />
		// Token: 0x06003255 RID: 12885 RVA: 0x00177838 File Offset: 0x00175A38
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool InvokeGodotClassMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
		{
			if ((ref method) == NBestiaryLayout.MethodName.Cleanup && args.Count == 0)
			{
				this.Cleanup();
				ret = default(godot_variant);
				return true;
			}
			return base.InvokeGodotClassMethod(ref method, args, ref ret);
		}

		/// <inheritdoc />
		// Token: 0x06003256 RID: 12886 RVA: 0x00177868 File Offset: 0x00175A68
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool HasGodotClassMethod(in godot_string_name method)
		{
			return (ref method) == NBestiaryLayout.MethodName.Cleanup || base.HasGodotClassMethod(ref method);
		}

		/// <inheritdoc />
		// Token: 0x06003257 RID: 12887 RVA: 0x00177880 File Offset: 0x00175A80
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void SaveGodotObjectData(GodotSerializationInfo info)
		{
			base.SaveGodotObjectData(info);
		}

		/// <inheritdoc />
		// Token: 0x06003258 RID: 12888 RVA: 0x00177889 File Offset: 0x00175A89
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void RestoreGodotObjectData(GodotSerializationInfo info)
		{
			base.RestoreGodotObjectData(info);
		}

		/// <summary>
		/// Cached StringNames for the methods contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x02001662 RID: 5730
		public class MethodName : Control.MethodName
		{
			/// <summary>
			/// Cached name for the 'Cleanup' method.
			/// </summary>
			// Token: 0x04005759 RID: 22361
			public static readonly StringName Cleanup = "Cleanup";
		}

		/// <summary>
		/// Cached StringNames for the properties and fields contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x02001663 RID: 5731
		public class PropertyName : Control.PropertyName
		{
		}

		/// <summary>
		/// Cached StringNames for the signals contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x02001664 RID: 5732
		public class SignalName : Control.SignalName
		{
		}
	}
}
