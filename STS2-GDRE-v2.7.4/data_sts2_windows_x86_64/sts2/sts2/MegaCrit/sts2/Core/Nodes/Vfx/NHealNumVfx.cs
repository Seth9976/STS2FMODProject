using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Godot;
using Godot.Bridge;
using Godot.NativeInterop;
using MegaCrit.Sts2.addons.mega_text;
using MegaCrit.Sts2.Core.Assets;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.Nodes.Combat;
using MegaCrit.Sts2.Core.Nodes.GodotExtensions;
using MegaCrit.Sts2.Core.Nodes.Rooms;
using MegaCrit.Sts2.Core.Random;

namespace MegaCrit.Sts2.Core.Nodes.Vfx
{
	/// <summary>
	/// Vfx used to pop out numbers when things heal.
	/// </summary>
	// Token: 0x020001B9 RID: 441
	[ScriptPath("res://src/Core/Nodes/Vfx/NHealNumVfx.cs")]
	public class NHealNumVfx : Node2D
	{
		// Token: 0x17000611 RID: 1553
		// (get) Token: 0x060015F2 RID: 5618 RVA: 0x000862CC File Offset: 0x000844CC
		[Nullable(1)]
		public static IEnumerable<string> AssetPaths
		{
			[NullableContext(1)]
			get
			{
				return new <>z__ReadOnlySingleElementList<string>(NHealNumVfx._scenePath);
			}
		}

		// Token: 0x060015F3 RID: 5619 RVA: 0x000862D8 File Offset: 0x000844D8
		[NullableContext(1)]
		[return: Nullable(2)]
		public static NHealNumVfx Create(Creature target, decimal amount)
		{
			NCombatRoom instance = NCombatRoom.Instance;
			NCreature ncreature = ((instance != null) ? instance.GetCreatureNode(target) : null);
			if (ncreature == null || !ncreature.IsInteractable)
			{
				return null;
			}
			NHealNumVfx nhealNumVfx = PreloadManager.Cache.GetScene(NHealNumVfx._scenePath).Instantiate<NHealNumVfx>(0L);
			nhealNumVfx._creatureNode = ncreature;
			nhealNumVfx._text = ((int)amount).ToString();
			return nhealNumVfx;
		}

		// Token: 0x060015F4 RID: 5620 RVA: 0x00086338 File Offset: 0x00084538
		public override void _Ready()
		{
			MegaLabel node = base.GetNode<MegaLabel>("Label");
			node.SetTextAutoSize(this._text);
			base.GlobalPosition = this._creatureNode.VfxSpawnPosition + NHealNumVfx._positionOffset + new Vector2(Rng.Chaotic.NextFloat(-10f, 10f), Rng.Chaotic.NextFloat(-5f, 5f));
			this._velocity = new Vector2(Rng.Chaotic.NextFloat(-100f, 100f), Rng.Chaotic.NextFloat(-600f, -300f));
			node.Scale = Vector2.One * Rng.Chaotic.NextFloat(1.2f, 1.3f);
			base.RotationDegrees = Rng.Chaotic.NextFloat(-5f, 5f);
			TaskHelper.RunSafely(this.AnimVfx());
		}

		// Token: 0x060015F5 RID: 5621 RVA: 0x0008642C File Offset: 0x0008462C
		[NullableContext(1)]
		private async Task AnimVfx()
		{
			this._tween = base.CreateTween().SetParallel(true);
			this._tween.TweenProperty(this, "modulate:a", 0f, 0.30000001192092896).SetDelay(1.0).SetEase(0L)
				.SetTrans(4L);
			this._tween.TweenProperty(this, "scale", Vector2.One, 0.5).SetEase(1L).SetTrans(4L)
				.From(Vector2.One * 2.5f);
			bool flag = await this._tween.AwaitFinished(this);
			this.QueueFreeSafely();
		}

		// Token: 0x060015F6 RID: 5622 RVA: 0x00086470 File Offset: 0x00084670
		public override void _Process(double delta)
		{
			float num = (float)delta;
			base.Position += this._velocity * num;
			if (this._velocity.LengthSquared() > 1E-06f)
			{
				this._velocity -= (this._velocity.Normalized() * NHealNumVfx._deceleration * num).LimitLength(this._velocity.Length());
			}
		}

		// Token: 0x060015F7 RID: 5623 RVA: 0x000864EE File Offset: 0x000846EE
		public override void _ExitTree()
		{
			Tween tween = this._tween;
			if (tween == null)
			{
				return;
			}
			tween.Kill();
		}

		/// <summary>
		/// Get the method information for all the methods declared in this class.
		/// This method is used by Godot to register the available methods in the editor.
		/// Do not call this method.
		/// </summary>
		// Token: 0x060015F8 RID: 5624 RVA: 0x00086500 File Offset: 0x00084700
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal static List<MethodInfo> GetGodotMethodList()
		{
			return new List<MethodInfo>(3)
			{
				new MethodInfo(NHealNumVfx.MethodName._Ready, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NHealNumVfx.MethodName._Process, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, new List<PropertyInfo>
				{
					new PropertyInfo(3L, "delta", 0L, "", 6L, false)
				}, null),
				new MethodInfo(NHealNumVfx.MethodName._ExitTree, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null)
			};
		}

		/// <inheritdoc />
		// Token: 0x060015F9 RID: 5625 RVA: 0x000865C8 File Offset: 0x000847C8
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool InvokeGodotClassMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
		{
			if ((ref method) == NHealNumVfx.MethodName._Ready && args.Count == 0)
			{
				this._Ready();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NHealNumVfx.MethodName._Process && args.Count == 1)
			{
				this._Process(VariantUtils.ConvertTo<double>(args[0]));
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NHealNumVfx.MethodName._ExitTree && args.Count == 0)
			{
				this._ExitTree();
				ret = default(godot_variant);
				return true;
			}
			return base.InvokeGodotClassMethod(ref method, args, ref ret);
		}

		/// <inheritdoc />
		// Token: 0x060015FA RID: 5626 RVA: 0x0008665B File Offset: 0x0008485B
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool HasGodotClassMethod(in godot_string_name method)
		{
			return (ref method) == NHealNumVfx.MethodName._Ready || (ref method) == NHealNumVfx.MethodName._Process || (ref method) == NHealNumVfx.MethodName._ExitTree || base.HasGodotClassMethod(ref method);
		}

		/// <inheritdoc />
		// Token: 0x060015FB RID: 5627 RVA: 0x00086694 File Offset: 0x00084894
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool SetGodotClassPropertyValue(in godot_string_name name, in godot_variant value)
		{
			if ((ref name) == NHealNumVfx.PropertyName._creatureNode)
			{
				this._creatureNode = VariantUtils.ConvertTo<NCreature>(ref value);
				return true;
			}
			if ((ref name) == NHealNumVfx.PropertyName._text)
			{
				this._text = VariantUtils.ConvertTo<string>(ref value);
				return true;
			}
			if ((ref name) == NHealNumVfx.PropertyName._tween)
			{
				this._tween = VariantUtils.ConvertTo<Tween>(ref value);
				return true;
			}
			if ((ref name) == NHealNumVfx.PropertyName._velocity)
			{
				this._velocity = VariantUtils.ConvertTo<Vector2>(ref value);
				return true;
			}
			return base.SetGodotClassPropertyValue(ref name, ref value);
		}

		/// <inheritdoc />
		// Token: 0x060015FC RID: 5628 RVA: 0x00086718 File Offset: 0x00084918
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
		{
			if ((ref name) == NHealNumVfx.PropertyName._creatureNode)
			{
				value = VariantUtils.CreateFrom<NCreature>(ref this._creatureNode);
				return true;
			}
			if ((ref name) == NHealNumVfx.PropertyName._text)
			{
				value = VariantUtils.CreateFrom<string>(ref this._text);
				return true;
			}
			if ((ref name) == NHealNumVfx.PropertyName._tween)
			{
				value = VariantUtils.CreateFrom<Tween>(ref this._tween);
				return true;
			}
			if ((ref name) == NHealNumVfx.PropertyName._velocity)
			{
				value = VariantUtils.CreateFrom<Vector2>(ref this._velocity);
				return true;
			}
			return base.GetGodotClassPropertyValue(ref name, ref value);
		}

		/// <summary>
		/// Get the property information for all the properties declared in this class.
		/// This method is used by Godot to register the available properties in the editor.
		/// Do not call this method.
		/// </summary>
		// Token: 0x060015FD RID: 5629 RVA: 0x000867B0 File Offset: 0x000849B0
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal static List<PropertyInfo> GetGodotPropertyList()
		{
			return new List<PropertyInfo>
			{
				new PropertyInfo(24L, NHealNumVfx.PropertyName._creatureNode, 0L, "", 4096L, false),
				new PropertyInfo(4L, NHealNumVfx.PropertyName._text, 0L, "", 4096L, false),
				new PropertyInfo(24L, NHealNumVfx.PropertyName._tween, 0L, "", 4096L, false),
				new PropertyInfo(5L, NHealNumVfx.PropertyName._velocity, 0L, "", 4096L, false)
			};
		}

		/// <inheritdoc />
		// Token: 0x060015FE RID: 5630 RVA: 0x00086848 File Offset: 0x00084A48
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void SaveGodotObjectData(GodotSerializationInfo info)
		{
			base.SaveGodotObjectData(info);
			info.AddProperty(NHealNumVfx.PropertyName._creatureNode, Variant.From<NCreature>(ref this._creatureNode));
			info.AddProperty(NHealNumVfx.PropertyName._text, Variant.From<string>(ref this._text));
			info.AddProperty(NHealNumVfx.PropertyName._tween, Variant.From<Tween>(ref this._tween));
			info.AddProperty(NHealNumVfx.PropertyName._velocity, Variant.From<Vector2>(ref this._velocity));
		}

		/// <inheritdoc />
		// Token: 0x060015FF RID: 5631 RVA: 0x000868B4 File Offset: 0x00084AB4
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void RestoreGodotObjectData(GodotSerializationInfo info)
		{
			base.RestoreGodotObjectData(info);
			Variant variant;
			if (info.TryGetProperty(NHealNumVfx.PropertyName._creatureNode, ref variant))
			{
				this._creatureNode = variant.As<NCreature>();
			}
			Variant variant2;
			if (info.TryGetProperty(NHealNumVfx.PropertyName._text, ref variant2))
			{
				this._text = variant2.As<string>();
			}
			Variant variant3;
			if (info.TryGetProperty(NHealNumVfx.PropertyName._tween, ref variant3))
			{
				this._tween = variant3.As<Tween>();
			}
			Variant variant4;
			if (info.TryGetProperty(NHealNumVfx.PropertyName._velocity, ref variant4))
			{
				this._velocity = variant4.As<Vector2>();
			}
		}

		// Token: 0x040007CE RID: 1998
		[Nullable(1)]
		private NCreature _creatureNode;

		// Token: 0x040007CF RID: 1999
		[Nullable(1)]
		private string _text;

		// Token: 0x040007D0 RID: 2000
		[Nullable(2)]
		private Tween _tween;

		// Token: 0x040007D1 RID: 2001
		private Vector2 _velocity;

		// Token: 0x040007D2 RID: 2002
		private static readonly float _deceleration = 2000f;

		// Token: 0x040007D3 RID: 2003
		private static readonly Vector2 _positionOffset = new Vector2(0f, -100f);

		// Token: 0x040007D4 RID: 2004
		[Nullable(1)]
		private static readonly string _scenePath = SceneHelper.GetScenePath("vfx/vfx_heal_num");

		/// <summary>
		/// Cached StringNames for the methods contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x02001061 RID: 4193
		public class MethodName : Node2D.MethodName
		{
			/// <summary>
			/// Cached name for the '_Ready' method.
			/// </summary>
			// Token: 0x0400408B RID: 16523
			public static readonly StringName _Ready = "_Ready";

			/// <summary>
			/// Cached name for the '_Process' method.
			/// </summary>
			// Token: 0x0400408C RID: 16524
			public static readonly StringName _Process = "_Process";

			/// <summary>
			/// Cached name for the '_ExitTree' method.
			/// </summary>
			// Token: 0x0400408D RID: 16525
			public static readonly StringName _ExitTree = "_ExitTree";
		}

		/// <summary>
		/// Cached StringNames for the properties and fields contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x02001062 RID: 4194
		public class PropertyName : Node2D.PropertyName
		{
			/// <summary>
			/// Cached name for the '_creatureNode' field.
			/// </summary>
			// Token: 0x0400408E RID: 16526
			public static readonly StringName _creatureNode = "_creatureNode";

			/// <summary>
			/// Cached name for the '_text' field.
			/// </summary>
			// Token: 0x0400408F RID: 16527
			public static readonly StringName _text = "_text";

			/// <summary>
			/// Cached name for the '_tween' field.
			/// </summary>
			// Token: 0x04004090 RID: 16528
			public static readonly StringName _tween = "_tween";

			/// <summary>
			/// Cached name for the '_velocity' field.
			/// </summary>
			// Token: 0x04004091 RID: 16529
			public static readonly StringName _velocity = "_velocity";
		}

		/// <summary>
		/// Cached StringNames for the signals contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x02001063 RID: 4195
		public class SignalName : Node2D.SignalName
		{
		}
	}
}
