using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Godot;
using Godot.Bridge;
using Godot.NativeInterop;
using MegaCrit.Sts2.Core.Assets;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.Nodes.Combat;
using MegaCrit.Sts2.Core.Nodes.GodotExtensions;
using MegaCrit.Sts2.Core.Nodes.Rooms;
using MegaCrit.Sts2.Core.Random;
using MegaCrit.Sts2.Core.TestSupport;

namespace MegaCrit.Sts2.Core.Nodes.Vfx
{
	// Token: 0x020001FE RID: 510
	[ScriptPath("res://src/Core/Nodes/Vfx/NStabVfx.cs")]
	public class NStabVfx : Node2D
	{
		/// <summary>
		/// Stab vfx is a silly line that goes into the creature center.
		/// Pass in the creature vfx center position!
		/// </summary>
		/// <param name="target">The creature to target</param>
		/// <param name="facingEnemies">Whether the VFX is facing enemies</param>
		/// <param name="vfxColor">The color of the VFX</param>
		/// <returns>The created VFX instance or null if in test mode</returns>
		// Token: 0x06001A41 RID: 6721 RVA: 0x000A8520 File Offset: 0x000A6720
		[NullableContext(2)]
		public static NStabVfx Create(Creature target, bool facingEnemies = false, VfxColor vfxColor = VfxColor.Red)
		{
			if (TestMode.IsOn)
			{
				return null;
			}
			NCreature creatureNode = NCombatRoom.Instance.GetCreatureNode(target);
			if (creatureNode == null)
			{
				return null;
			}
			Vector2 vfxSpawnPosition = creatureNode.VfxSpawnPosition;
			NStabVfx nstabVfx = PreloadManager.Cache.GetScene("res://scenes/vfx/stab_vfx.tscn").Instantiate<NStabVfx>(0L);
			nstabVfx._vfxColor = vfxColor;
			nstabVfx._facingEnemies = facingEnemies;
			Vector2 vector;
			vector..ctor(facingEnemies ? Rng.Chaotic.NextFloat(0f, 48f) : Rng.Chaotic.NextFloat(-48f, 0f), Rng.Chaotic.NextFloat(-50f, 50f));
			nstabVfx._creatureCenter = vfxSpawnPosition + vector;
			return nstabVfx;
		}

		// Token: 0x06001A42 RID: 6722 RVA: 0x000A85CC File Offset: 0x000A67CC
		public override void _Ready()
		{
			this._primaryVfx = base.GetNode<Node2D>("Primary");
			this._secondaryVfx = base.GetNode<Node2D>("%Secondary");
			this._primaryVfx.GlobalPosition = this.GenerateSpawnPosition();
			this._primaryVfx.Rotation = MathHelper.GetAngle(this._primaryVfx.GlobalPosition - this._creatureCenter) + 1.5707964f;
			this.SetColor();
			TaskHelper.RunSafely(this.Animate());
		}

		// Token: 0x06001A43 RID: 6723 RVA: 0x000A8654 File Offset: 0x000A6854
		private void SetColor()
		{
			switch (this._vfxColor)
			{
			case VfxColor.Green:
				this._primaryVfx.SelfModulate = new Color("00A52F");
				this._secondaryVfx.SelfModulate = new Color("FFCB2D");
				return;
			case VfxColor.Blue:
				this._primaryVfx.SelfModulate = new Color("007BDD");
				this._secondaryVfx.SelfModulate = new Color("00EFF6");
				return;
			case VfxColor.Purple:
				this._primaryVfx.SelfModulate = new Color("A803FF");
				this._secondaryVfx.SelfModulate = new Color("00EFF3");
				return;
			case VfxColor.Black:
				return;
			case VfxColor.White:
				this._primaryVfx.SelfModulate = new Color("808080");
				this._secondaryVfx.SelfModulate = new Color("FFFFFF");
				return;
			case VfxColor.Cyan:
				this._primaryVfx.SelfModulate = new Color("009599");
				this._secondaryVfx.SelfModulate = new Color("5CDCFF");
				return;
			case VfxColor.Gold:
				this._primaryVfx.SelfModulate = new Color("EBA800");
				this._secondaryVfx.SelfModulate = new Color("FFE39C");
				return;
			}
			this._primaryVfx.SelfModulate = new Color("FF0000");
			this._secondaryVfx.SelfModulate = new Color("FFCB2D");
		}

		/// <summary>
		/// Spawns our stab vfx in a random position to the left or right of the target.
		/// </summary>
		// Token: 0x06001A44 RID: 6724 RVA: 0x000A87C0 File Offset: 0x000A69C0
		private Vector2 GenerateSpawnPosition()
		{
			Vector2 vector;
			vector..ctor(Rng.Chaotic.NextFloat(-12f, 12f), Rng.Chaotic.NextFloat(-64f, 64f));
			Vector2 vector2;
			vector2..ctor(this._facingEnemies ? (-200f) : 200f, 0f);
			return this._creatureCenter + vector + vector2;
		}

		// Token: 0x06001A45 RID: 6725 RVA: 0x000A882E File Offset: 0x000A6A2E
		public override void _ExitTree()
		{
			Tween tween = this._tween;
			if (tween == null)
			{
				return;
			}
			tween.Kill();
		}

		// Token: 0x06001A46 RID: 6726 RVA: 0x000A8840 File Offset: 0x000A6A40
		[NullableContext(1)]
		private async Task Animate()
		{
			this._tween = base.CreateTween().SetParallel(true);
			this._tween.TweenProperty(this, "modulate:a", 1f, 0.25);
			this._tween.TweenProperty(this._primaryVfx, "position", this._creatureCenter, 0.5).SetEase(1L).SetTrans(6L);
			this._tween.TweenProperty(this, "modulate:a", 0f, 0.25).SetDelay(0.25);
			bool flag = await this._tween.AwaitFinished(this);
			this.QueueFreeSafely();
		}

		/// <summary>
		/// Get the method information for all the methods declared in this class.
		/// This method is used by Godot to register the available methods in the editor.
		/// Do not call this method.
		/// </summary>
		// Token: 0x06001A47 RID: 6727 RVA: 0x000A8884 File Offset: 0x000A6A84
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal static List<MethodInfo> GetGodotMethodList()
		{
			return new List<MethodInfo>(4)
			{
				new MethodInfo(NStabVfx.MethodName._Ready, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NStabVfx.MethodName.SetColor, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NStabVfx.MethodName.GenerateSpawnPosition, new PropertyInfo(5L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NStabVfx.MethodName._ExitTree, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null)
			};
		}

		/// <inheritdoc />
		// Token: 0x06001A48 RID: 6728 RVA: 0x000A8958 File Offset: 0x000A6B58
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool InvokeGodotClassMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
		{
			if ((ref method) == NStabVfx.MethodName._Ready && args.Count == 0)
			{
				this._Ready();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NStabVfx.MethodName.SetColor && args.Count == 0)
			{
				this.SetColor();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NStabVfx.MethodName.GenerateSpawnPosition && args.Count == 0)
			{
				Vector2 vector = this.GenerateSpawnPosition();
				ret = VariantUtils.CreateFrom<Vector2>(ref vector);
				return true;
			}
			if ((ref method) == NStabVfx.MethodName._ExitTree && args.Count == 0)
			{
				this._ExitTree();
				ret = default(godot_variant);
				return true;
			}
			return base.InvokeGodotClassMethod(ref method, args, ref ret);
		}

		/// <inheritdoc />
		// Token: 0x06001A49 RID: 6729 RVA: 0x000A8A0C File Offset: 0x000A6C0C
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool HasGodotClassMethod(in godot_string_name method)
		{
			return (ref method) == NStabVfx.MethodName._Ready || (ref method) == NStabVfx.MethodName.SetColor || (ref method) == NStabVfx.MethodName.GenerateSpawnPosition || (ref method) == NStabVfx.MethodName._ExitTree || base.HasGodotClassMethod(ref method);
		}

		/// <inheritdoc />
		// Token: 0x06001A4A RID: 6730 RVA: 0x000A8A5C File Offset: 0x000A6C5C
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool SetGodotClassPropertyValue(in godot_string_name name, in godot_variant value)
		{
			if ((ref name) == NStabVfx.PropertyName._primaryVfx)
			{
				this._primaryVfx = VariantUtils.ConvertTo<Node2D>(ref value);
				return true;
			}
			if ((ref name) == NStabVfx.PropertyName._secondaryVfx)
			{
				this._secondaryVfx = VariantUtils.ConvertTo<Node2D>(ref value);
				return true;
			}
			if ((ref name) == NStabVfx.PropertyName._creatureCenter)
			{
				this._creatureCenter = VariantUtils.ConvertTo<Vector2>(ref value);
				return true;
			}
			if ((ref name) == NStabVfx.PropertyName._vfxColor)
			{
				this._vfxColor = VariantUtils.ConvertTo<VfxColor>(ref value);
				return true;
			}
			if ((ref name) == NStabVfx.PropertyName._facingEnemies)
			{
				this._facingEnemies = VariantUtils.ConvertTo<bool>(ref value);
				return true;
			}
			if ((ref name) == NStabVfx.PropertyName._tween)
			{
				this._tween = VariantUtils.ConvertTo<Tween>(ref value);
				return true;
			}
			return base.SetGodotClassPropertyValue(ref name, ref value);
		}

		/// <inheritdoc />
		// Token: 0x06001A4B RID: 6731 RVA: 0x000A8B14 File Offset: 0x000A6D14
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
		{
			if ((ref name) == NStabVfx.PropertyName._primaryVfx)
			{
				value = VariantUtils.CreateFrom<Node2D>(ref this._primaryVfx);
				return true;
			}
			if ((ref name) == NStabVfx.PropertyName._secondaryVfx)
			{
				value = VariantUtils.CreateFrom<Node2D>(ref this._secondaryVfx);
				return true;
			}
			if ((ref name) == NStabVfx.PropertyName._creatureCenter)
			{
				value = VariantUtils.CreateFrom<Vector2>(ref this._creatureCenter);
				return true;
			}
			if ((ref name) == NStabVfx.PropertyName._vfxColor)
			{
				value = VariantUtils.CreateFrom<VfxColor>(ref this._vfxColor);
				return true;
			}
			if ((ref name) == NStabVfx.PropertyName._facingEnemies)
			{
				value = VariantUtils.CreateFrom<bool>(ref this._facingEnemies);
				return true;
			}
			if ((ref name) == NStabVfx.PropertyName._tween)
			{
				value = VariantUtils.CreateFrom<Tween>(ref this._tween);
				return true;
			}
			return base.GetGodotClassPropertyValue(ref name, ref value);
		}

		/// <summary>
		/// Get the property information for all the properties declared in this class.
		/// This method is used by Godot to register the available properties in the editor.
		/// Do not call this method.
		/// </summary>
		// Token: 0x06001A4C RID: 6732 RVA: 0x000A8BEC File Offset: 0x000A6DEC
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal static List<PropertyInfo> GetGodotPropertyList()
		{
			return new List<PropertyInfo>
			{
				new PropertyInfo(24L, NStabVfx.PropertyName._primaryVfx, 0L, "", 4096L, false),
				new PropertyInfo(24L, NStabVfx.PropertyName._secondaryVfx, 0L, "", 4096L, false),
				new PropertyInfo(5L, NStabVfx.PropertyName._creatureCenter, 0L, "", 4096L, false),
				new PropertyInfo(2L, NStabVfx.PropertyName._vfxColor, 0L, "", 4096L, false),
				new PropertyInfo(1L, NStabVfx.PropertyName._facingEnemies, 0L, "", 4096L, false),
				new PropertyInfo(24L, NStabVfx.PropertyName._tween, 0L, "", 4096L, false)
			};
		}

		/// <inheritdoc />
		// Token: 0x06001A4D RID: 6733 RVA: 0x000A8CC4 File Offset: 0x000A6EC4
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void SaveGodotObjectData(GodotSerializationInfo info)
		{
			base.SaveGodotObjectData(info);
			info.AddProperty(NStabVfx.PropertyName._primaryVfx, Variant.From<Node2D>(ref this._primaryVfx));
			info.AddProperty(NStabVfx.PropertyName._secondaryVfx, Variant.From<Node2D>(ref this._secondaryVfx));
			info.AddProperty(NStabVfx.PropertyName._creatureCenter, Variant.From<Vector2>(ref this._creatureCenter));
			info.AddProperty(NStabVfx.PropertyName._vfxColor, Variant.From<VfxColor>(ref this._vfxColor));
			info.AddProperty(NStabVfx.PropertyName._facingEnemies, Variant.From<bool>(ref this._facingEnemies));
			info.AddProperty(NStabVfx.PropertyName._tween, Variant.From<Tween>(ref this._tween));
		}

		/// <inheritdoc />
		// Token: 0x06001A4E RID: 6734 RVA: 0x000A8D5C File Offset: 0x000A6F5C
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void RestoreGodotObjectData(GodotSerializationInfo info)
		{
			base.RestoreGodotObjectData(info);
			Variant variant;
			if (info.TryGetProperty(NStabVfx.PropertyName._primaryVfx, ref variant))
			{
				this._primaryVfx = variant.As<Node2D>();
			}
			Variant variant2;
			if (info.TryGetProperty(NStabVfx.PropertyName._secondaryVfx, ref variant2))
			{
				this._secondaryVfx = variant2.As<Node2D>();
			}
			Variant variant3;
			if (info.TryGetProperty(NStabVfx.PropertyName._creatureCenter, ref variant3))
			{
				this._creatureCenter = variant3.As<Vector2>();
			}
			Variant variant4;
			if (info.TryGetProperty(NStabVfx.PropertyName._vfxColor, ref variant4))
			{
				this._vfxColor = variant4.As<VfxColor>();
			}
			Variant variant5;
			if (info.TryGetProperty(NStabVfx.PropertyName._facingEnemies, ref variant5))
			{
				this._facingEnemies = variant5.As<bool>();
			}
			Variant variant6;
			if (info.TryGetProperty(NStabVfx.PropertyName._tween, ref variant6))
			{
				this._tween = variant6.As<Tween>();
			}
		}

		// Token: 0x040009BD RID: 2493
		[Nullable(1)]
		private const string _scenePath = "res://scenes/vfx/stab_vfx.tscn";

		// Token: 0x040009BE RID: 2494
		[Nullable(1)]
		private Node2D _primaryVfx;

		// Token: 0x040009BF RID: 2495
		[Nullable(1)]
		private Node2D _secondaryVfx;

		// Token: 0x040009C0 RID: 2496
		private Vector2 _creatureCenter;

		// Token: 0x040009C1 RID: 2497
		private VfxColor _vfxColor;

		// Token: 0x040009C2 RID: 2498
		private bool _facingEnemies;

		// Token: 0x040009C3 RID: 2499
		[Nullable(2)]
		private Tween _tween;

		/// <summary>
		/// Cached StringNames for the methods contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x02001166 RID: 4454
		public class MethodName : Node2D.MethodName
		{
			/// <summary>
			/// Cached name for the '_Ready' method.
			/// </summary>
			// Token: 0x040043E9 RID: 17385
			public static readonly StringName _Ready = "_Ready";

			/// <summary>
			/// Cached name for the 'SetColor' method.
			/// </summary>
			// Token: 0x040043EA RID: 17386
			public static readonly StringName SetColor = "SetColor";

			/// <summary>
			/// Cached name for the 'GenerateSpawnPosition' method.
			/// </summary>
			// Token: 0x040043EB RID: 17387
			public static readonly StringName GenerateSpawnPosition = "GenerateSpawnPosition";

			/// <summary>
			/// Cached name for the '_ExitTree' method.
			/// </summary>
			// Token: 0x040043EC RID: 17388
			public static readonly StringName _ExitTree = "_ExitTree";
		}

		/// <summary>
		/// Cached StringNames for the properties and fields contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x02001167 RID: 4455
		public class PropertyName : Node2D.PropertyName
		{
			/// <summary>
			/// Cached name for the '_primaryVfx' field.
			/// </summary>
			// Token: 0x040043ED RID: 17389
			public static readonly StringName _primaryVfx = "_primaryVfx";

			/// <summary>
			/// Cached name for the '_secondaryVfx' field.
			/// </summary>
			// Token: 0x040043EE RID: 17390
			public static readonly StringName _secondaryVfx = "_secondaryVfx";

			/// <summary>
			/// Cached name for the '_creatureCenter' field.
			/// </summary>
			// Token: 0x040043EF RID: 17391
			public static readonly StringName _creatureCenter = "_creatureCenter";

			/// <summary>
			/// Cached name for the '_vfxColor' field.
			/// </summary>
			// Token: 0x040043F0 RID: 17392
			public static readonly StringName _vfxColor = "_vfxColor";

			/// <summary>
			/// Cached name for the '_facingEnemies' field.
			/// </summary>
			// Token: 0x040043F1 RID: 17393
			public static readonly StringName _facingEnemies = "_facingEnemies";

			/// <summary>
			/// Cached name for the '_tween' field.
			/// </summary>
			// Token: 0x040043F2 RID: 17394
			public static readonly StringName _tween = "_tween";
		}

		/// <summary>
		/// Cached StringNames for the signals contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x02001168 RID: 4456
		public class SignalName : Node2D.SignalName
		{
		}
	}
}
