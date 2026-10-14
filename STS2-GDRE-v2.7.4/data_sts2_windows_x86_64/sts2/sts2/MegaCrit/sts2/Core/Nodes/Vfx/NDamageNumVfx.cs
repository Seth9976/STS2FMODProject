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
using MegaCrit.Sts2.Core.Context;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.Models.Monsters;
using MegaCrit.Sts2.Core.Nodes.Combat;
using MegaCrit.Sts2.Core.Nodes.GodotExtensions;
using MegaCrit.Sts2.Core.Nodes.Rooms;
using MegaCrit.Sts2.Core.Random;
using MegaCrit.Sts2.Core.TestSupport;

namespace MegaCrit.Sts2.Core.Nodes.Vfx
{
	/// <summary>
	/// Vfx used to pop out numbers when we deal damage to things.
	/// </summary>
	// Token: 0x0200019A RID: 410
	[ScriptPath("res://src/Core/Nodes/Vfx/NDamageNumVfx.cs")]
	public class NDamageNumVfx : Node2D
	{
		// Token: 0x17000602 RID: 1538
		// (get) Token: 0x06001426 RID: 5158 RVA: 0x000799BA File Offset: 0x00077BBA
		[Nullable(1)]
		public static IEnumerable<string> AssetPaths
		{
			[NullableContext(1)]
			get
			{
				return new <>z__ReadOnlySingleElementList<string>(NDamageNumVfx._scenePath);
			}
		}

		// Token: 0x06001427 RID: 5159 RVA: 0x000799C8 File Offset: 0x00077BC8
		[NullableContext(1)]
		[return: Nullable(2)]
		public static NDamageNumVfx Create(Creature target, DamageResult result)
		{
			if (TestMode.IsOn)
			{
				return null;
			}
			int num = result.UnblockedDamage;
			if (!(target.Monster is Osty))
			{
				num += result.OverkillDamage;
			}
			return NDamageNumVfx.Create(target, num, true);
		}

		// Token: 0x06001428 RID: 5160 RVA: 0x00079A04 File Offset: 0x00077C04
		[NullableContext(1)]
		[return: Nullable(2)]
		public static NDamageNumVfx Create(Creature target, int damage, bool requireInteractable = true)
		{
			if (TestMode.IsOn)
			{
				return null;
			}
			NCombatRoom instance = NCombatRoom.Instance;
			NCreature ncreature = ((instance != null) ? instance.GetCreatureNode(target) : null);
			Vector2 vector = Vector2.Zero;
			if (requireInteractable && (ncreature == null || !ncreature.IsInteractable))
			{
				if (!LocalContext.IsMe(target))
				{
					return null;
				}
				Vector2 size = ((SceneTree)Engine.GetMainLoop()).Root.GetViewport().GetVisibleRect().Size;
				vector = size * new Vector2(0.25f, 0.5f);
			}
			else if (ncreature != null)
			{
				vector = ncreature.VfxSpawnPosition + NDamageNumVfx._positionOffset + new Vector2(Rng.Chaotic.NextFloat(-10f, 10f), Rng.Chaotic.NextFloat(-5f, 5f));
			}
			return NDamageNumVfx.Create(vector, damage);
		}

		// Token: 0x06001429 RID: 5161 RVA: 0x00079AD4 File Offset: 0x00077CD4
		[NullableContext(2)]
		public static NDamageNumVfx Create(Vector2 globalPosition, int damage)
		{
			if (TestMode.IsOn)
			{
				return null;
			}
			NDamageNumVfx ndamageNumVfx = PreloadManager.Cache.GetScene(NDamageNumVfx._scenePath).Instantiate<NDamageNumVfx>(0L);
			ndamageNumVfx._globalSpawnPosition = globalPosition;
			ndamageNumVfx._text = damage.ToString();
			return ndamageNumVfx;
		}

		// Token: 0x0600142A RID: 5162 RVA: 0x00079B18 File Offset: 0x00077D18
		public override void _Ready()
		{
			MegaLabel node = base.GetNode<MegaLabel>("Label");
			node.SetTextAutoSize(this._text);
			base.GlobalPosition = this._globalSpawnPosition;
			this._velocity = new Vector2(Rng.Chaotic.NextFloat(-100f, 100f), Rng.Chaotic.NextFloat(-800f, -700f));
			node.Scale = Vector2.One * Rng.Chaotic.NextFloat(1.2f, 1.3f);
			base.RotationDegrees = Rng.Chaotic.NextFloat(-5f, 5f);
			TaskHelper.RunSafely(this.AnimVfx());
		}

		// Token: 0x0600142B RID: 5163 RVA: 0x00079BCC File Offset: 0x00077DCC
		[NullableContext(1)]
		private async Task AnimVfx()
		{
			this._tween = base.CreateTween().SetParallel(true);
			this._tween.TweenProperty(this, "modulate", StsColors.cream, 0.5).SetEase(1L).SetTrans(7L);
			this._tween.TweenProperty(this, "modulate:a", 0f, 2.0).SetEase(0L).SetTrans(4L);
			this._tween.TweenProperty(this, "scale", Vector2.One, 1.2000000476837158).SetEase(1L).SetTrans(4L)
				.From(Vector2.One * 2.5f);
			bool flag = await this._tween.AwaitFinished(this);
			this.QueueFreeSafely();
		}

		// Token: 0x0600142C RID: 5164 RVA: 0x00079C10 File Offset: 0x00077E10
		public override void _Process(double delta)
		{
			float num = (float)delta;
			base.Position += this._velocity * num;
			this._velocity += NDamageNumVfx._gravity * num;
		}

		// Token: 0x0600142D RID: 5165 RVA: 0x00079C59 File Offset: 0x00077E59
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
		// Token: 0x0600142E RID: 5166 RVA: 0x00079C6C File Offset: 0x00077E6C
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal static List<MethodInfo> GetGodotMethodList()
		{
			return new List<MethodInfo>(4)
			{
				new MethodInfo(NDamageNumVfx.MethodName.Create, new PropertyInfo(24L, "", 0L, "", 6L, new StringName("Node2D"), false), 33L, new List<PropertyInfo>
				{
					new PropertyInfo(5L, "globalPosition", 0L, "", 6L, false),
					new PropertyInfo(2L, "damage", 0L, "", 6L, false)
				}, null),
				new MethodInfo(NDamageNumVfx.MethodName._Ready, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NDamageNumVfx.MethodName._Process, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, new List<PropertyInfo>
				{
					new PropertyInfo(3L, "delta", 0L, "", 6L, false)
				}, null),
				new MethodInfo(NDamageNumVfx.MethodName._ExitTree, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null)
			};
		}

		/// <inheritdoc />
		// Token: 0x0600142F RID: 5167 RVA: 0x00079DB4 File Offset: 0x00077FB4
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool InvokeGodotClassMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
		{
			if ((ref method) == NDamageNumVfx.MethodName.Create && args.Count == 2)
			{
				NDamageNumVfx ndamageNumVfx = NDamageNumVfx.Create(VariantUtils.ConvertTo<Vector2>(args[0]), VariantUtils.ConvertTo<int>(args[1]));
				ret = VariantUtils.CreateFrom<NDamageNumVfx>(ref ndamageNumVfx);
				return true;
			}
			if ((ref method) == NDamageNumVfx.MethodName._Ready && args.Count == 0)
			{
				this._Ready();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NDamageNumVfx.MethodName._Process && args.Count == 1)
			{
				this._Process(VariantUtils.ConvertTo<double>(args[0]));
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NDamageNumVfx.MethodName._ExitTree && args.Count == 0)
			{
				this._ExitTree();
				ret = default(godot_variant);
				return true;
			}
			return base.InvokeGodotClassMethod(ref method, args, ref ret);
		}

		// Token: 0x06001430 RID: 5168 RVA: 0x00079E90 File Offset: 0x00078090
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal static bool InvokeGodotClassStaticMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
		{
			if ((ref method) == NDamageNumVfx.MethodName.Create && args.Count == 2)
			{
				NDamageNumVfx ndamageNumVfx = NDamageNumVfx.Create(VariantUtils.ConvertTo<Vector2>(args[0]), VariantUtils.ConvertTo<int>(args[1]));
				ret = VariantUtils.CreateFrom<NDamageNumVfx>(ref ndamageNumVfx);
				return true;
			}
			ret = default(godot_variant);
			return false;
		}

		/// <inheritdoc />
		// Token: 0x06001431 RID: 5169 RVA: 0x00079EEC File Offset: 0x000780EC
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool HasGodotClassMethod(in godot_string_name method)
		{
			return (ref method) == NDamageNumVfx.MethodName.Create || (ref method) == NDamageNumVfx.MethodName._Ready || (ref method) == NDamageNumVfx.MethodName._Process || (ref method) == NDamageNumVfx.MethodName._ExitTree || base.HasGodotClassMethod(ref method);
		}

		/// <inheritdoc />
		// Token: 0x06001432 RID: 5170 RVA: 0x00079F3C File Offset: 0x0007813C
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool SetGodotClassPropertyValue(in godot_string_name name, in godot_variant value)
		{
			if ((ref name) == NDamageNumVfx.PropertyName._globalSpawnPosition)
			{
				this._globalSpawnPosition = VariantUtils.ConvertTo<Vector2>(ref value);
				return true;
			}
			if ((ref name) == NDamageNumVfx.PropertyName._text)
			{
				this._text = VariantUtils.ConvertTo<string>(ref value);
				return true;
			}
			if ((ref name) == NDamageNumVfx.PropertyName._tween)
			{
				this._tween = VariantUtils.ConvertTo<Tween>(ref value);
				return true;
			}
			if ((ref name) == NDamageNumVfx.PropertyName._velocity)
			{
				this._velocity = VariantUtils.ConvertTo<Vector2>(ref value);
				return true;
			}
			return base.SetGodotClassPropertyValue(ref name, ref value);
		}

		/// <inheritdoc />
		// Token: 0x06001433 RID: 5171 RVA: 0x00079FC0 File Offset: 0x000781C0
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
		{
			if ((ref name) == NDamageNumVfx.PropertyName._globalSpawnPosition)
			{
				value = VariantUtils.CreateFrom<Vector2>(ref this._globalSpawnPosition);
				return true;
			}
			if ((ref name) == NDamageNumVfx.PropertyName._text)
			{
				value = VariantUtils.CreateFrom<string>(ref this._text);
				return true;
			}
			if ((ref name) == NDamageNumVfx.PropertyName._tween)
			{
				value = VariantUtils.CreateFrom<Tween>(ref this._tween);
				return true;
			}
			if ((ref name) == NDamageNumVfx.PropertyName._velocity)
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
		// Token: 0x06001434 RID: 5172 RVA: 0x0007A058 File Offset: 0x00078258
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal static List<PropertyInfo> GetGodotPropertyList()
		{
			return new List<PropertyInfo>
			{
				new PropertyInfo(5L, NDamageNumVfx.PropertyName._globalSpawnPosition, 0L, "", 4096L, false),
				new PropertyInfo(4L, NDamageNumVfx.PropertyName._text, 0L, "", 4096L, false),
				new PropertyInfo(24L, NDamageNumVfx.PropertyName._tween, 0L, "", 4096L, false),
				new PropertyInfo(5L, NDamageNumVfx.PropertyName._velocity, 0L, "", 4096L, false)
			};
		}

		/// <inheritdoc />
		// Token: 0x06001435 RID: 5173 RVA: 0x0007A0F0 File Offset: 0x000782F0
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void SaveGodotObjectData(GodotSerializationInfo info)
		{
			base.SaveGodotObjectData(info);
			info.AddProperty(NDamageNumVfx.PropertyName._globalSpawnPosition, Variant.From<Vector2>(ref this._globalSpawnPosition));
			info.AddProperty(NDamageNumVfx.PropertyName._text, Variant.From<string>(ref this._text));
			info.AddProperty(NDamageNumVfx.PropertyName._tween, Variant.From<Tween>(ref this._tween));
			info.AddProperty(NDamageNumVfx.PropertyName._velocity, Variant.From<Vector2>(ref this._velocity));
		}

		/// <inheritdoc />
		// Token: 0x06001436 RID: 5174 RVA: 0x0007A15C File Offset: 0x0007835C
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void RestoreGodotObjectData(GodotSerializationInfo info)
		{
			base.RestoreGodotObjectData(info);
			Variant variant;
			if (info.TryGetProperty(NDamageNumVfx.PropertyName._globalSpawnPosition, ref variant))
			{
				this._globalSpawnPosition = variant.As<Vector2>();
			}
			Variant variant2;
			if (info.TryGetProperty(NDamageNumVfx.PropertyName._text, ref variant2))
			{
				this._text = variant2.As<string>();
			}
			Variant variant3;
			if (info.TryGetProperty(NDamageNumVfx.PropertyName._tween, ref variant3))
			{
				this._tween = variant3.As<Tween>();
			}
			Variant variant4;
			if (info.TryGetProperty(NDamageNumVfx.PropertyName._velocity, ref variant4))
			{
				this._velocity = variant4.As<Vector2>();
			}
		}

		// Token: 0x04000730 RID: 1840
		private Vector2 _globalSpawnPosition;

		// Token: 0x04000731 RID: 1841
		[Nullable(1)]
		private string _text;

		// Token: 0x04000732 RID: 1842
		[Nullable(2)]
		private Tween _tween;

		// Token: 0x04000733 RID: 1843
		private Vector2 _velocity;

		// Token: 0x04000734 RID: 1844
		private static readonly Vector2 _gravity = new Vector2(0f, 2000f);

		// Token: 0x04000735 RID: 1845
		private static readonly Vector2 _positionOffset = new Vector2(0f, -100f);

		// Token: 0x04000736 RID: 1846
		[Nullable(1)]
		private static readonly string _scenePath = SceneHelper.GetScenePath("vfx/vfx_damage_num");

		/// <summary>
		/// Cached StringNames for the methods contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x02000FF2 RID: 4082
		public class MethodName : Node2D.MethodName
		{
			/// <summary>
			/// Cached name for the 'Create' method.
			/// </summary>
			// Token: 0x04003F59 RID: 16217
			public static readonly StringName Create = "Create";

			/// <summary>
			/// Cached name for the '_Ready' method.
			/// </summary>
			// Token: 0x04003F5A RID: 16218
			public static readonly StringName _Ready = "_Ready";

			/// <summary>
			/// Cached name for the '_Process' method.
			/// </summary>
			// Token: 0x04003F5B RID: 16219
			public static readonly StringName _Process = "_Process";

			/// <summary>
			/// Cached name for the '_ExitTree' method.
			/// </summary>
			// Token: 0x04003F5C RID: 16220
			public static readonly StringName _ExitTree = "_ExitTree";
		}

		/// <summary>
		/// Cached StringNames for the properties and fields contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x02000FF3 RID: 4083
		public class PropertyName : Node2D.PropertyName
		{
			/// <summary>
			/// Cached name for the '_globalSpawnPosition' field.
			/// </summary>
			// Token: 0x04003F5D RID: 16221
			public static readonly StringName _globalSpawnPosition = "_globalSpawnPosition";

			/// <summary>
			/// Cached name for the '_text' field.
			/// </summary>
			// Token: 0x04003F5E RID: 16222
			public static readonly StringName _text = "_text";

			/// <summary>
			/// Cached name for the '_tween' field.
			/// </summary>
			// Token: 0x04003F5F RID: 16223
			public static readonly StringName _tween = "_tween";

			/// <summary>
			/// Cached name for the '_velocity' field.
			/// </summary>
			// Token: 0x04003F60 RID: 16224
			public static readonly StringName _velocity = "_velocity";
		}

		/// <summary>
		/// Cached StringNames for the signals contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x02000FF4 RID: 4084
		public class SignalName : Node2D.SignalName
		{
		}
	}
}
