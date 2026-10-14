using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Godot;
using Godot.Bridge;
using Godot.NativeInterop;
using MegaCrit.Sts2.Core.Assets;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.Nodes.Combat;
using MegaCrit.Sts2.Core.Nodes.Rooms;

namespace MegaCrit.Sts2.Core.Nodes.Vfx
{
	// Token: 0x020001E1 RID: 481
	[NullableContext(1)]
	[Nullable(0)]
	[ScriptPath("res://src/Core/Nodes/Vfx/NPowerUpVfx.cs")]
	public class NPowerUpVfx : Node2D
	{
		// Token: 0x17000624 RID: 1572
		// (get) Token: 0x0600185B RID: 6235 RVA: 0x000983DA File Offset: 0x000965DA
		private static string NormalScenePath
		{
			get
			{
				return SceneHelper.GetScenePath("/vfx/vfx_power_up/vfx_power_up");
			}
		}

		// Token: 0x17000625 RID: 1573
		// (get) Token: 0x0600185C RID: 6236 RVA: 0x000983E6 File Offset: 0x000965E6
		private static string GhostlyScenePath
		{
			get
			{
				return SceneHelper.GetScenePath("/vfx/vfx_ghostly_power_up/vfx_ghostly_power_up");
			}
		}

		// Token: 0x17000626 RID: 1574
		// (get) Token: 0x0600185D RID: 6237 RVA: 0x000983F2 File Offset: 0x000965F2
		public static IEnumerable<string> AssetPaths
		{
			get
			{
				return new <>z__ReadOnlyArray<string>(new string[]
				{
					NPowerUpVfx.NormalScenePath,
					NPowerUpVfx.GhostlyScenePath
				});
			}
		}

		// Token: 0x0600185E RID: 6238 RVA: 0x0009840F File Offset: 0x0009660F
		[return: Nullable(2)]
		public static NPowerUpVfx CreateNormal(Creature target)
		{
			return NPowerUpVfx.CreatePowerUpVfx(target, NPowerUpVfx.NormalScenePath);
		}

		// Token: 0x0600185F RID: 6239 RVA: 0x0009841C File Offset: 0x0009661C
		[return: Nullable(2)]
		public static NPowerUpVfx CreateGhostly(Creature target)
		{
			return NPowerUpVfx.CreatePowerUpVfx(target, NPowerUpVfx.GhostlyScenePath);
		}

		// Token: 0x06001860 RID: 6240 RVA: 0x0009842C File Offset: 0x0009662C
		[return: Nullable(2)]
		private static NPowerUpVfx CreatePowerUpVfx(Creature target, string scenePath)
		{
			NCombatRoom instance = NCombatRoom.Instance;
			NCreature ncreature = ((instance != null) ? instance.GetCreatureNode(target) : null);
			if (ncreature == null || !ncreature.IsInteractable)
			{
				return null;
			}
			NPowerUpVfx npowerUpVfx = PreloadManager.Cache.GetScene(scenePath).Instantiate<NPowerUpVfx>(0L);
			npowerUpVfx.GlobalPosition = ncreature.VfxSpawnPosition;
			NCombatRoom.Instance.CombatVfxContainer.AddChildSafely(npowerUpVfx);
			return npowerUpVfx;
		}

		// Token: 0x06001861 RID: 6241 RVA: 0x0009848C File Offset: 0x0009668C
		public override void _Ready()
		{
			this._timer = 1f;
			this._backVfx = base.GetNode<Sprite2D>("%BackVfx");
			Vector2 globalPosition = this._backVfx.GlobalPosition;
			this._backVfx.Reparent(NCombatRoom.Instance.BackCombatVfxContainer, true);
			this._backVfx.GlobalPosition = globalPosition;
		}

		// Token: 0x06001862 RID: 6242 RVA: 0x000984E8 File Offset: 0x000966E8
		public override void _Process(double delta)
		{
			this._timer -= (float)delta;
			float num = 1f;
			if (Mathf.Abs(this._timer / 1f - 0.5f) > 0.4f)
			{
				num = Mathf.Max(0f, 1f - (Mathf.Abs(this._timer / 1f - 0.5f) - 0.4f) / 0.1f);
			}
			base.Modulate = new Color(1f, 1f, 1f, num);
			this._backVfx.Modulate = new Color(1f, 1f, 1f, num);
			if (this._timer < 0f)
			{
				this.QueueFreeSafely();
				this._backVfx.QueueFreeSafely();
			}
		}

		/// <summary>
		/// Get the method information for all the methods declared in this class.
		/// This method is used by Godot to register the available methods in the editor.
		/// Do not call this method.
		/// </summary>
		// Token: 0x06001863 RID: 6243 RVA: 0x000985B8 File Offset: 0x000967B8
		[NullableContext(0)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal static List<MethodInfo> GetGodotMethodList()
		{
			return new List<MethodInfo>(2)
			{
				new MethodInfo(NPowerUpVfx.MethodName._Ready, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NPowerUpVfx.MethodName._Process, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, new List<PropertyInfo>
				{
					new PropertyInfo(3L, "delta", 0L, "", 6L, false)
				}, null)
			};
		}

		/// <inheritdoc />
		// Token: 0x06001864 RID: 6244 RVA: 0x00098650 File Offset: 0x00096850
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool InvokeGodotClassMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
		{
			if ((ref method) == NPowerUpVfx.MethodName._Ready && args.Count == 0)
			{
				this._Ready();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NPowerUpVfx.MethodName._Process && args.Count == 1)
			{
				this._Process(VariantUtils.ConvertTo<double>(args[0]));
				ret = default(godot_variant);
				return true;
			}
			return base.InvokeGodotClassMethod(ref method, args, ref ret);
		}

		/// <inheritdoc />
		// Token: 0x06001865 RID: 6245 RVA: 0x000986BE File Offset: 0x000968BE
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool HasGodotClassMethod(in godot_string_name method)
		{
			return (ref method) == NPowerUpVfx.MethodName._Ready || (ref method) == NPowerUpVfx.MethodName._Process || base.HasGodotClassMethod(ref method);
		}

		/// <inheritdoc />
		// Token: 0x06001866 RID: 6246 RVA: 0x000986E8 File Offset: 0x000968E8
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool SetGodotClassPropertyValue(in godot_string_name name, in godot_variant value)
		{
			if ((ref name) == NPowerUpVfx.PropertyName._timer)
			{
				this._timer = VariantUtils.ConvertTo<float>(ref value);
				return true;
			}
			if ((ref name) == NPowerUpVfx.PropertyName._creatureVisuals)
			{
				this._creatureVisuals = VariantUtils.ConvertTo<Control>(ref value);
				return true;
			}
			if ((ref name) == NPowerUpVfx.PropertyName._backVfx)
			{
				this._backVfx = VariantUtils.ConvertTo<Sprite2D>(ref value);
				return true;
			}
			return base.SetGodotClassPropertyValue(ref name, ref value);
		}

		/// <inheritdoc />
		// Token: 0x06001867 RID: 6247 RVA: 0x00098750 File Offset: 0x00096950
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
		{
			if ((ref name) == NPowerUpVfx.PropertyName._timer)
			{
				value = VariantUtils.CreateFrom<float>(ref this._timer);
				return true;
			}
			if ((ref name) == NPowerUpVfx.PropertyName._creatureVisuals)
			{
				value = VariantUtils.CreateFrom<Control>(ref this._creatureVisuals);
				return true;
			}
			if ((ref name) == NPowerUpVfx.PropertyName._backVfx)
			{
				value = VariantUtils.CreateFrom<Sprite2D>(ref this._backVfx);
				return true;
			}
			return base.GetGodotClassPropertyValue(ref name, ref value);
		}

		/// <summary>
		/// Get the property information for all the properties declared in this class.
		/// This method is used by Godot to register the available properties in the editor.
		/// Do not call this method.
		/// </summary>
		// Token: 0x06001868 RID: 6248 RVA: 0x000987C8 File Offset: 0x000969C8
		[NullableContext(0)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal static List<PropertyInfo> GetGodotPropertyList()
		{
			return new List<PropertyInfo>
			{
				new PropertyInfo(3L, NPowerUpVfx.PropertyName._timer, 0L, "", 4096L, false),
				new PropertyInfo(24L, NPowerUpVfx.PropertyName._creatureVisuals, 0L, "", 4096L, false),
				new PropertyInfo(24L, NPowerUpVfx.PropertyName._backVfx, 0L, "", 4096L, false)
			};
		}

		/// <inheritdoc />
		// Token: 0x06001869 RID: 6249 RVA: 0x00098840 File Offset: 0x00096A40
		[NullableContext(0)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void SaveGodotObjectData(GodotSerializationInfo info)
		{
			base.SaveGodotObjectData(info);
			info.AddProperty(NPowerUpVfx.PropertyName._timer, Variant.From<float>(ref this._timer));
			info.AddProperty(NPowerUpVfx.PropertyName._creatureVisuals, Variant.From<Control>(ref this._creatureVisuals));
			info.AddProperty(NPowerUpVfx.PropertyName._backVfx, Variant.From<Sprite2D>(ref this._backVfx));
		}

		/// <inheritdoc />
		// Token: 0x0600186A RID: 6250 RVA: 0x00098898 File Offset: 0x00096A98
		[NullableContext(0)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void RestoreGodotObjectData(GodotSerializationInfo info)
		{
			base.RestoreGodotObjectData(info);
			Variant variant;
			if (info.TryGetProperty(NPowerUpVfx.PropertyName._timer, ref variant))
			{
				this._timer = variant.As<float>();
			}
			Variant variant2;
			if (info.TryGetProperty(NPowerUpVfx.PropertyName._creatureVisuals, ref variant2))
			{
				this._creatureVisuals = variant2.As<Control>();
			}
			Variant variant3;
			if (info.TryGetProperty(NPowerUpVfx.PropertyName._backVfx, ref variant3))
			{
				this._backVfx = variant3.As<Sprite2D>();
			}
		}

		// Token: 0x040008CB RID: 2251
		private float _timer;

		// Token: 0x040008CC RID: 2252
		private const float _vfxDuration = 1f;

		// Token: 0x040008CD RID: 2253
		private Control _creatureVisuals;

		// Token: 0x040008CE RID: 2254
		private Sprite2D _backVfx;

		/// <summary>
		/// Cached StringNames for the methods contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x020010F7 RID: 4343
		[NullableContext(0)]
		public class MethodName : Node2D.MethodName
		{
			/// <summary>
			/// Cached name for the '_Ready' method.
			/// </summary>
			// Token: 0x0400425C RID: 16988
			public static readonly StringName _Ready = "_Ready";

			/// <summary>
			/// Cached name for the '_Process' method.
			/// </summary>
			// Token: 0x0400425D RID: 16989
			public static readonly StringName _Process = "_Process";
		}

		/// <summary>
		/// Cached StringNames for the properties and fields contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x020010F8 RID: 4344
		[NullableContext(0)]
		public class PropertyName : Node2D.PropertyName
		{
			/// <summary>
			/// Cached name for the '_timer' field.
			/// </summary>
			// Token: 0x0400425E RID: 16990
			public static readonly StringName _timer = "_timer";

			/// <summary>
			/// Cached name for the '_creatureVisuals' field.
			/// </summary>
			// Token: 0x0400425F RID: 16991
			public static readonly StringName _creatureVisuals = "_creatureVisuals";

			/// <summary>
			/// Cached name for the '_backVfx' field.
			/// </summary>
			// Token: 0x04004260 RID: 16992
			public static readonly StringName _backVfx = "_backVfx";
		}

		/// <summary>
		/// Cached StringNames for the signals contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x020010F9 RID: 4345
		[NullableContext(0)]
		public class SignalName : Node2D.SignalName
		{
		}
	}
}
