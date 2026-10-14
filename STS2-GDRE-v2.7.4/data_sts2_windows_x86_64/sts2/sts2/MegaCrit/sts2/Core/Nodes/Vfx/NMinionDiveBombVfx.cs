using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using Godot;
using Godot.Bridge;
using Godot.Collections;
using Godot.NativeInterop;
using MegaCrit.Sts2.Core.Assets;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.Nodes.Combat;
using MegaCrit.Sts2.Core.Nodes.GodotExtensions;
using MegaCrit.Sts2.Core.Nodes.Rooms;
using MegaCrit.Sts2.Core.Nodes.Vfx.Utilities;
using MegaCrit.Sts2.Core.TestSupport;

namespace MegaCrit.Sts2.Core.Nodes.Vfx
{
	// Token: 0x020001D3 RID: 467
	[NullableContext(2)]
	[Nullable(0)]
	[ScriptPath("res://src/Core/Nodes/Vfx/NMinionDiveBombVfx.cs")]
	public class NMinionDiveBombVfx : Node2D
	{
		// Token: 0x0600178E RID: 6030 RVA: 0x000922A8 File Offset: 0x000904A8
		public override void _ExitTree()
		{
			CancellationTokenSource cts = this._cts;
			if (cts == null)
			{
				return;
			}
			cts.Cancel();
		}

		// Token: 0x1700061B RID: 1563
		// (get) Token: 0x0600178F RID: 6031 RVA: 0x000922BA File Offset: 0x000904BA
		private Vector2 SourceFinalPosition
		{
			get
			{
				return this._sourcePosition + this._sourceOffset;
			}
		}

		// Token: 0x1700061C RID: 1564
		// (get) Token: 0x06001790 RID: 6032 RVA: 0x000922CD File Offset: 0x000904CD
		private Vector2 DestinationFinalPosition
		{
			get
			{
				return this._destinationPosition + this._destinationOffset;
			}
		}

		// Token: 0x06001791 RID: 6033 RVA: 0x000922E0 File Offset: 0x000904E0
		[NullableContext(1)]
		[return: Nullable(2)]
		public static NMinionDiveBombVfx Create(Creature owner, Creature target)
		{
			if (TestMode.IsOn)
			{
				return null;
			}
			NCombatRoom instance = NCombatRoom.Instance;
			NCreature ncreature = ((instance != null) ? instance.GetCreatureNode(owner) : null);
			NCombatRoom instance2 = NCombatRoom.Instance;
			NCreature ncreature2 = ((instance2 != null) ? instance2.GetCreatureNode(target) : null);
			if (ncreature2 != null && ncreature != null)
			{
				return NMinionDiveBombVfx.Create(ncreature.VfxSpawnPosition, ncreature2.GetBottomOfHitbox());
			}
			return null;
		}

		// Token: 0x06001792 RID: 6034 RVA: 0x00092338 File Offset: 0x00090538
		public static NMinionDiveBombVfx Create(Vector2 playerCenterPosition, Vector2 targetFloorPosition)
		{
			if (TestMode.IsOn)
			{
				return null;
			}
			NMinionDiveBombVfx nminionDiveBombVfx = PreloadManager.Cache.GetScene(NMinionDiveBombVfx.scenePath).Instantiate<NMinionDiveBombVfx>(0L);
			nminionDiveBombVfx.Initialize(playerCenterPosition, targetFloorPosition);
			return nminionDiveBombVfx;
		}

		// Token: 0x06001793 RID: 6035 RVA: 0x00092370 File Offset: 0x00090570
		private void Initialize(Vector2 sourcePosition, Vector2 destinationPosition)
		{
			this._sourcePosition = sourcePosition;
			this._destinationPosition = destinationPosition;
			this._fallingTrail.Visible = true;
			base.GlobalPosition = sourcePosition;
			for (int i = 0; i < this._minionVfx.Count; i++)
			{
				this._minionVfx[i].SetEmitting(false);
			}
		}

		// Token: 0x06001794 RID: 6036 RVA: 0x000923C6 File Offset: 0x000905C6
		public override void _Ready()
		{
			TaskHelper.RunSafely(this.PlaySequence());
		}

		// Token: 0x06001795 RID: 6037 RVA: 0x000923D4 File Offset: 0x000905D4
		private void SetMinionVisible(bool visible)
		{
			this._minionSprite.SelfModulate = (visible ? new Color(1f, 1f, 1f, 1f) : new Color(1f, 1f, 1f, 0f));
		}

		// Token: 0x06001796 RID: 6038 RVA: 0x00092424 File Offset: 0x00090624
		private void UpdateMinionSprite(int index)
		{
			if (this._previousIndex == index)
			{
				return;
			}
			this._previousIndex = index;
			Texture2D texture2D = this._minionTextures[Mathf.Clamp(index, 0, this._minionTextures.Count - 1)];
			this._minionSprite.Texture = texture2D;
			string text = this._minionAnimations[Mathf.Clamp(index, 0, this._minionAnimations.Count - 1)];
			if (!this._minionAnimator.CurrentAnimation.Equals(text))
			{
				this._minionAnimator.Play(text, -1.0, 1f, false);
			}
			for (int i = 0; i < this._minionVfx.Count; i++)
			{
				if (i == index)
				{
					this._minionVfx[i].Restart();
				}
			}
		}

		// Token: 0x06001797 RID: 6039 RVA: 0x000924EC File Offset: 0x000906EC
		[NullableContext(1)]
		private async Task PlaySequence()
		{
			this._cts = new CancellationTokenSource();
			Vector2 startPos = this.SourceFinalPosition;
			Vector2 endPos = this.DestinationFinalPosition;
			this.UpdateMinionSprite(0);
			this._minionSprite.GlobalPosition = startPos;
			this.SetMinionVisible(true);
			double timer = 0.0;
			bool isPlayingFallingVfx = false;
			while (timer < (double)this._flightTime)
			{
				float num = (float)timer / this._flightTime;
				float num2 = this._horizontalCurve.Sample(num);
				float num3 = this._verticalCurve.Sample(num);
				float num4 = this._textureCurve.Sample(num);
				this.UpdateMinionSprite(Mathf.FloorToInt(num4));
				Vector2 vector = startPos.Lerp(endPos, num2);
				vector += Vector2.Up * num3 * this._maxHeight;
				this._minionSprite.GlobalPosition = vector;
				this._fallingVfx.GlobalPosition = vector;
				if (timer >= (double)this._fallingVfxEntryTime && !isPlayingFallingVfx)
				{
					this._fallingVfx.Restart();
					this._fallingTrail.Visible = true;
					isPlayingFallingVfx = true;
				}
				timer += base.GetProcessDeltaTime();
				await this.AwaitProcessFrame(default(CancellationToken));
			}
			this.SetMinionVisible(false);
			NGame instance = NGame.Instance;
			if (instance != null)
			{
				instance.ScreenShake(ShakeStrength.Weak, ShakeDuration.Short, -1f);
			}
			this._impactVfx.GlobalPosition = this._destinationPosition;
			this._impactVfx.Restart();
			this._fallingTrail.Visible = false;
			this._fallingVfx.SetEmitting(false);
			await Cmd.Wait(2f, this._cts.Token, false);
			this.QueueFreeSafely();
		}

		/// <summary>
		/// Get the method information for all the methods declared in this class.
		/// This method is used by Godot to register the available methods in the editor.
		/// Do not call this method.
		/// </summary>
		// Token: 0x06001798 RID: 6040 RVA: 0x00092530 File Offset: 0x00090730
		[NullableContext(0)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal static List<MethodInfo> GetGodotMethodList()
		{
			return new List<MethodInfo>(6)
			{
				new MethodInfo(NMinionDiveBombVfx.MethodName._ExitTree, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NMinionDiveBombVfx.MethodName.Create, new PropertyInfo(24L, "", 0L, "", 6L, new StringName("Node2D"), false), 33L, new List<PropertyInfo>
				{
					new PropertyInfo(5L, "playerCenterPosition", 0L, "", 6L, false),
					new PropertyInfo(5L, "targetFloorPosition", 0L, "", 6L, false)
				}, null),
				new MethodInfo(NMinionDiveBombVfx.MethodName.Initialize, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, new List<PropertyInfo>
				{
					new PropertyInfo(5L, "sourcePosition", 0L, "", 6L, false),
					new PropertyInfo(5L, "destinationPosition", 0L, "", 6L, false)
				}, null),
				new MethodInfo(NMinionDiveBombVfx.MethodName._Ready, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NMinionDiveBombVfx.MethodName.SetMinionVisible, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, new List<PropertyInfo>
				{
					new PropertyInfo(1L, "visible", 0L, "", 6L, false)
				}, null),
				new MethodInfo(NMinionDiveBombVfx.MethodName.UpdateMinionSprite, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, new List<PropertyInfo>
				{
					new PropertyInfo(2L, "index", 0L, "", 6L, false)
				}, null)
			};
		}

		/// <inheritdoc />
		// Token: 0x06001799 RID: 6041 RVA: 0x00092744 File Offset: 0x00090944
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool InvokeGodotClassMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
		{
			if ((ref method) == NMinionDiveBombVfx.MethodName._ExitTree && args.Count == 0)
			{
				this._ExitTree();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NMinionDiveBombVfx.MethodName.Create && args.Count == 2)
			{
				NMinionDiveBombVfx nminionDiveBombVfx = NMinionDiveBombVfx.Create(VariantUtils.ConvertTo<Vector2>(args[0]), VariantUtils.ConvertTo<Vector2>(args[1]));
				ret = VariantUtils.CreateFrom<NMinionDiveBombVfx>(ref nminionDiveBombVfx);
				return true;
			}
			if ((ref method) == NMinionDiveBombVfx.MethodName.Initialize && args.Count == 2)
			{
				this.Initialize(VariantUtils.ConvertTo<Vector2>(args[0]), VariantUtils.ConvertTo<Vector2>(args[1]));
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NMinionDiveBombVfx.MethodName._Ready && args.Count == 0)
			{
				this._Ready();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NMinionDiveBombVfx.MethodName.SetMinionVisible && args.Count == 1)
			{
				this.SetMinionVisible(VariantUtils.ConvertTo<bool>(args[0]));
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NMinionDiveBombVfx.MethodName.UpdateMinionSprite && args.Count == 1)
			{
				this.UpdateMinionSprite(VariantUtils.ConvertTo<int>(args[0]));
				ret = default(godot_variant);
				return true;
			}
			return base.InvokeGodotClassMethod(ref method, args, ref ret);
		}

		// Token: 0x0600179A RID: 6042 RVA: 0x00092890 File Offset: 0x00090A90
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal static bool InvokeGodotClassStaticMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
		{
			if ((ref method) == NMinionDiveBombVfx.MethodName.Create && args.Count == 2)
			{
				NMinionDiveBombVfx nminionDiveBombVfx = NMinionDiveBombVfx.Create(VariantUtils.ConvertTo<Vector2>(args[0]), VariantUtils.ConvertTo<Vector2>(args[1]));
				ret = VariantUtils.CreateFrom<NMinionDiveBombVfx>(ref nminionDiveBombVfx);
				return true;
			}
			ret = default(godot_variant);
			return false;
		}

		/// <inheritdoc />
		// Token: 0x0600179B RID: 6043 RVA: 0x000928EC File Offset: 0x00090AEC
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool HasGodotClassMethod(in godot_string_name method)
		{
			return (ref method) == NMinionDiveBombVfx.MethodName._ExitTree || (ref method) == NMinionDiveBombVfx.MethodName.Create || (ref method) == NMinionDiveBombVfx.MethodName.Initialize || (ref method) == NMinionDiveBombVfx.MethodName._Ready || (ref method) == NMinionDiveBombVfx.MethodName.SetMinionVisible || (ref method) == NMinionDiveBombVfx.MethodName.UpdateMinionSprite || base.HasGodotClassMethod(ref method);
		}

		/// <inheritdoc />
		// Token: 0x0600179C RID: 6044 RVA: 0x0009295C File Offset: 0x00090B5C
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool SetGodotClassPropertyValue(in godot_string_name name, in godot_variant value)
		{
			if ((ref name) == NMinionDiveBombVfx.PropertyName._minionSprite)
			{
				this._minionSprite = VariantUtils.ConvertTo<Sprite2D>(ref value);
				return true;
			}
			if ((ref name) == NMinionDiveBombVfx.PropertyName._minionTextures)
			{
				this._minionTextures = VariantUtils.ConvertToArray<Texture2D>(ref value);
				return true;
			}
			if ((ref name) == NMinionDiveBombVfx.PropertyName._minionAnimator)
			{
				this._minionAnimator = VariantUtils.ConvertTo<AnimationPlayer>(ref value);
				return true;
			}
			if ((ref name) == NMinionDiveBombVfx.PropertyName._minionAnimations)
			{
				this._minionAnimations = VariantUtils.ConvertToArray<string>(ref value);
				return true;
			}
			if ((ref name) == NMinionDiveBombVfx.PropertyName._minionVfx)
			{
				this._minionVfx = VariantUtils.ConvertToArray<NParticlesContainer>(ref value);
				return true;
			}
			if ((ref name) == NMinionDiveBombVfx.PropertyName._fallingTrail)
			{
				this._fallingTrail = VariantUtils.ConvertTo<Node2D>(ref value);
				return true;
			}
			if ((ref name) == NMinionDiveBombVfx.PropertyName._fallingVfx)
			{
				this._fallingVfx = VariantUtils.ConvertTo<NParticlesContainer>(ref value);
				return true;
			}
			if ((ref name) == NMinionDiveBombVfx.PropertyName._impactVfx)
			{
				this._impactVfx = VariantUtils.ConvertTo<NParticlesContainer>(ref value);
				return true;
			}
			if ((ref name) == NMinionDiveBombVfx.PropertyName._flightTime)
			{
				this._flightTime = VariantUtils.ConvertTo<float>(ref value);
				return true;
			}
			if ((ref name) == NMinionDiveBombVfx.PropertyName._fallingVfxEntryTime)
			{
				this._fallingVfxEntryTime = VariantUtils.ConvertTo<float>(ref value);
				return true;
			}
			if ((ref name) == NMinionDiveBombVfx.PropertyName._horizontalCurve)
			{
				this._horizontalCurve = VariantUtils.ConvertTo<Curve>(ref value);
				return true;
			}
			if ((ref name) == NMinionDiveBombVfx.PropertyName._verticalCurve)
			{
				this._verticalCurve = VariantUtils.ConvertTo<Curve>(ref value);
				return true;
			}
			if ((ref name) == NMinionDiveBombVfx.PropertyName._textureCurve)
			{
				this._textureCurve = VariantUtils.ConvertTo<Curve>(ref value);
				return true;
			}
			if ((ref name) == NMinionDiveBombVfx.PropertyName._maxHeight)
			{
				this._maxHeight = VariantUtils.ConvertTo<float>(ref value);
				return true;
			}
			if ((ref name) == NMinionDiveBombVfx.PropertyName._sourceOffset)
			{
				this._sourceOffset = VariantUtils.ConvertTo<Vector2>(ref value);
				return true;
			}
			if ((ref name) == NMinionDiveBombVfx.PropertyName._destinationOffset)
			{
				this._destinationOffset = VariantUtils.ConvertTo<Vector2>(ref value);
				return true;
			}
			if ((ref name) == NMinionDiveBombVfx.PropertyName._previousIndex)
			{
				this._previousIndex = VariantUtils.ConvertTo<int>(ref value);
				return true;
			}
			if ((ref name) == NMinionDiveBombVfx.PropertyName._sourcePosition)
			{
				this._sourcePosition = VariantUtils.ConvertTo<Vector2>(ref value);
				return true;
			}
			if ((ref name) == NMinionDiveBombVfx.PropertyName._destinationPosition)
			{
				this._destinationPosition = VariantUtils.ConvertTo<Vector2>(ref value);
				return true;
			}
			return base.SetGodotClassPropertyValue(ref name, ref value);
		}

		/// <inheritdoc />
		// Token: 0x0600179D RID: 6045 RVA: 0x00092B74 File Offset: 0x00090D74
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
		{
			if ((ref name) == NMinionDiveBombVfx.PropertyName.SourceFinalPosition)
			{
				Vector2 vector = this.SourceFinalPosition;
				value = VariantUtils.CreateFrom<Vector2>(ref vector);
				return true;
			}
			if ((ref name) == NMinionDiveBombVfx.PropertyName.DestinationFinalPosition)
			{
				Vector2 vector = this.DestinationFinalPosition;
				value = VariantUtils.CreateFrom<Vector2>(ref vector);
				return true;
			}
			if ((ref name) == NMinionDiveBombVfx.PropertyName._minionSprite)
			{
				value = VariantUtils.CreateFrom<Sprite2D>(ref this._minionSprite);
				return true;
			}
			if ((ref name) == NMinionDiveBombVfx.PropertyName._minionTextures)
			{
				value = VariantUtils.CreateFromArray<Texture2D>(this._minionTextures);
				return true;
			}
			if ((ref name) == NMinionDiveBombVfx.PropertyName._minionAnimator)
			{
				value = VariantUtils.CreateFrom<AnimationPlayer>(ref this._minionAnimator);
				return true;
			}
			if ((ref name) == NMinionDiveBombVfx.PropertyName._minionAnimations)
			{
				value = VariantUtils.CreateFromArray<string>(this._minionAnimations);
				return true;
			}
			if ((ref name) == NMinionDiveBombVfx.PropertyName._minionVfx)
			{
				value = VariantUtils.CreateFromArray<NParticlesContainer>(this._minionVfx);
				return true;
			}
			if ((ref name) == NMinionDiveBombVfx.PropertyName._fallingTrail)
			{
				value = VariantUtils.CreateFrom<Node2D>(ref this._fallingTrail);
				return true;
			}
			if ((ref name) == NMinionDiveBombVfx.PropertyName._fallingVfx)
			{
				value = VariantUtils.CreateFrom<NParticlesContainer>(ref this._fallingVfx);
				return true;
			}
			if ((ref name) == NMinionDiveBombVfx.PropertyName._impactVfx)
			{
				value = VariantUtils.CreateFrom<NParticlesContainer>(ref this._impactVfx);
				return true;
			}
			if ((ref name) == NMinionDiveBombVfx.PropertyName._flightTime)
			{
				value = VariantUtils.CreateFrom<float>(ref this._flightTime);
				return true;
			}
			if ((ref name) == NMinionDiveBombVfx.PropertyName._fallingVfxEntryTime)
			{
				value = VariantUtils.CreateFrom<float>(ref this._fallingVfxEntryTime);
				return true;
			}
			if ((ref name) == NMinionDiveBombVfx.PropertyName._horizontalCurve)
			{
				value = VariantUtils.CreateFrom<Curve>(ref this._horizontalCurve);
				return true;
			}
			if ((ref name) == NMinionDiveBombVfx.PropertyName._verticalCurve)
			{
				value = VariantUtils.CreateFrom<Curve>(ref this._verticalCurve);
				return true;
			}
			if ((ref name) == NMinionDiveBombVfx.PropertyName._textureCurve)
			{
				value = VariantUtils.CreateFrom<Curve>(ref this._textureCurve);
				return true;
			}
			if ((ref name) == NMinionDiveBombVfx.PropertyName._maxHeight)
			{
				value = VariantUtils.CreateFrom<float>(ref this._maxHeight);
				return true;
			}
			if ((ref name) == NMinionDiveBombVfx.PropertyName._sourceOffset)
			{
				value = VariantUtils.CreateFrom<Vector2>(ref this._sourceOffset);
				return true;
			}
			if ((ref name) == NMinionDiveBombVfx.PropertyName._destinationOffset)
			{
				value = VariantUtils.CreateFrom<Vector2>(ref this._destinationOffset);
				return true;
			}
			if ((ref name) == NMinionDiveBombVfx.PropertyName._previousIndex)
			{
				value = VariantUtils.CreateFrom<int>(ref this._previousIndex);
				return true;
			}
			if ((ref name) == NMinionDiveBombVfx.PropertyName._sourcePosition)
			{
				value = VariantUtils.CreateFrom<Vector2>(ref this._sourcePosition);
				return true;
			}
			if ((ref name) == NMinionDiveBombVfx.PropertyName._destinationPosition)
			{
				value = VariantUtils.CreateFrom<Vector2>(ref this._destinationPosition);
				return true;
			}
			return base.GetGodotClassPropertyValue(ref name, ref value);
		}

		/// <summary>
		/// Get the property information for all the properties declared in this class.
		/// This method is used by Godot to register the available properties in the editor.
		/// Do not call this method.
		/// </summary>
		// Token: 0x0600179E RID: 6046 RVA: 0x00092E30 File Offset: 0x00091030
		[NullableContext(0)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal static List<PropertyInfo> GetGodotPropertyList()
		{
			return new List<PropertyInfo>
			{
				new PropertyInfo(24L, NMinionDiveBombVfx.PropertyName._minionSprite, 34L, "Sprite2D", 4102L, true),
				new PropertyInfo(28L, NMinionDiveBombVfx.PropertyName._minionTextures, 23L, "24/17:Texture2D", 4102L, true),
				new PropertyInfo(24L, NMinionDiveBombVfx.PropertyName._minionAnimator, 34L, "AnimationPlayer", 4102L, true),
				new PropertyInfo(28L, NMinionDiveBombVfx.PropertyName._minionAnimations, 23L, "4/0:", 4102L, true),
				new PropertyInfo(28L, NMinionDiveBombVfx.PropertyName._minionVfx, 23L, "24/34:Node2D", 4102L, true),
				new PropertyInfo(24L, NMinionDiveBombVfx.PropertyName._fallingTrail, 34L, "Node2D", 4102L, true),
				new PropertyInfo(24L, NMinionDiveBombVfx.PropertyName._fallingVfx, 34L, "Node2D", 4102L, true),
				new PropertyInfo(24L, NMinionDiveBombVfx.PropertyName._impactVfx, 34L, "Node2D", 4102L, true),
				new PropertyInfo(3L, NMinionDiveBombVfx.PropertyName._flightTime, 0L, "", 4102L, true),
				new PropertyInfo(3L, NMinionDiveBombVfx.PropertyName._fallingVfxEntryTime, 0L, "", 4102L, true),
				new PropertyInfo(24L, NMinionDiveBombVfx.PropertyName._horizontalCurve, 17L, "Curve", 4102L, true),
				new PropertyInfo(24L, NMinionDiveBombVfx.PropertyName._verticalCurve, 17L, "Curve", 4102L, true),
				new PropertyInfo(24L, NMinionDiveBombVfx.PropertyName._textureCurve, 17L, "Curve", 4102L, true),
				new PropertyInfo(3L, NMinionDiveBombVfx.PropertyName._maxHeight, 0L, "", 4102L, true),
				new PropertyInfo(5L, NMinionDiveBombVfx.PropertyName._sourceOffset, 0L, "", 4102L, true),
				new PropertyInfo(5L, NMinionDiveBombVfx.PropertyName._destinationOffset, 0L, "", 4102L, true),
				new PropertyInfo(2L, NMinionDiveBombVfx.PropertyName._previousIndex, 0L, "", 4096L, false),
				new PropertyInfo(5L, NMinionDiveBombVfx.PropertyName._sourcePosition, 0L, "", 4096L, false),
				new PropertyInfo(5L, NMinionDiveBombVfx.PropertyName._destinationPosition, 0L, "", 4096L, false),
				new PropertyInfo(5L, NMinionDiveBombVfx.PropertyName.SourceFinalPosition, 0L, "", 4096L, false),
				new PropertyInfo(5L, NMinionDiveBombVfx.PropertyName.DestinationFinalPosition, 0L, "", 4096L, false)
			};
		}

		/// <inheritdoc />
		// Token: 0x0600179F RID: 6047 RVA: 0x000930FC File Offset: 0x000912FC
		[NullableContext(0)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void SaveGodotObjectData(GodotSerializationInfo info)
		{
			base.SaveGodotObjectData(info);
			info.AddProperty(NMinionDiveBombVfx.PropertyName._minionSprite, Variant.From<Sprite2D>(ref this._minionSprite));
			info.AddProperty(NMinionDiveBombVfx.PropertyName._minionTextures, Variant.CreateFrom<Texture2D>(this._minionTextures));
			info.AddProperty(NMinionDiveBombVfx.PropertyName._minionAnimator, Variant.From<AnimationPlayer>(ref this._minionAnimator));
			info.AddProperty(NMinionDiveBombVfx.PropertyName._minionAnimations, Variant.CreateFrom<string>(this._minionAnimations));
			info.AddProperty(NMinionDiveBombVfx.PropertyName._minionVfx, Variant.CreateFrom<NParticlesContainer>(this._minionVfx));
			info.AddProperty(NMinionDiveBombVfx.PropertyName._fallingTrail, Variant.From<Node2D>(ref this._fallingTrail));
			info.AddProperty(NMinionDiveBombVfx.PropertyName._fallingVfx, Variant.From<NParticlesContainer>(ref this._fallingVfx));
			info.AddProperty(NMinionDiveBombVfx.PropertyName._impactVfx, Variant.From<NParticlesContainer>(ref this._impactVfx));
			info.AddProperty(NMinionDiveBombVfx.PropertyName._flightTime, Variant.From<float>(ref this._flightTime));
			info.AddProperty(NMinionDiveBombVfx.PropertyName._fallingVfxEntryTime, Variant.From<float>(ref this._fallingVfxEntryTime));
			info.AddProperty(NMinionDiveBombVfx.PropertyName._horizontalCurve, Variant.From<Curve>(ref this._horizontalCurve));
			info.AddProperty(NMinionDiveBombVfx.PropertyName._verticalCurve, Variant.From<Curve>(ref this._verticalCurve));
			info.AddProperty(NMinionDiveBombVfx.PropertyName._textureCurve, Variant.From<Curve>(ref this._textureCurve));
			info.AddProperty(NMinionDiveBombVfx.PropertyName._maxHeight, Variant.From<float>(ref this._maxHeight));
			info.AddProperty(NMinionDiveBombVfx.PropertyName._sourceOffset, Variant.From<Vector2>(ref this._sourceOffset));
			info.AddProperty(NMinionDiveBombVfx.PropertyName._destinationOffset, Variant.From<Vector2>(ref this._destinationOffset));
			info.AddProperty(NMinionDiveBombVfx.PropertyName._previousIndex, Variant.From<int>(ref this._previousIndex));
			info.AddProperty(NMinionDiveBombVfx.PropertyName._sourcePosition, Variant.From<Vector2>(ref this._sourcePosition));
			info.AddProperty(NMinionDiveBombVfx.PropertyName._destinationPosition, Variant.From<Vector2>(ref this._destinationPosition));
		}

		/// <inheritdoc />
		// Token: 0x060017A0 RID: 6048 RVA: 0x000932B4 File Offset: 0x000914B4
		[NullableContext(0)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void RestoreGodotObjectData(GodotSerializationInfo info)
		{
			base.RestoreGodotObjectData(info);
			Variant variant;
			if (info.TryGetProperty(NMinionDiveBombVfx.PropertyName._minionSprite, ref variant))
			{
				this._minionSprite = variant.As<Sprite2D>();
			}
			Variant variant2;
			if (info.TryGetProperty(NMinionDiveBombVfx.PropertyName._minionTextures, ref variant2))
			{
				this._minionTextures = variant2.AsGodotArray<Texture2D>();
			}
			Variant variant3;
			if (info.TryGetProperty(NMinionDiveBombVfx.PropertyName._minionAnimator, ref variant3))
			{
				this._minionAnimator = variant3.As<AnimationPlayer>();
			}
			Variant variant4;
			if (info.TryGetProperty(NMinionDiveBombVfx.PropertyName._minionAnimations, ref variant4))
			{
				this._minionAnimations = variant4.AsGodotArray<string>();
			}
			Variant variant5;
			if (info.TryGetProperty(NMinionDiveBombVfx.PropertyName._minionVfx, ref variant5))
			{
				this._minionVfx = variant5.AsGodotArray<NParticlesContainer>();
			}
			Variant variant6;
			if (info.TryGetProperty(NMinionDiveBombVfx.PropertyName._fallingTrail, ref variant6))
			{
				this._fallingTrail = variant6.As<Node2D>();
			}
			Variant variant7;
			if (info.TryGetProperty(NMinionDiveBombVfx.PropertyName._fallingVfx, ref variant7))
			{
				this._fallingVfx = variant7.As<NParticlesContainer>();
			}
			Variant variant8;
			if (info.TryGetProperty(NMinionDiveBombVfx.PropertyName._impactVfx, ref variant8))
			{
				this._impactVfx = variant8.As<NParticlesContainer>();
			}
			Variant variant9;
			if (info.TryGetProperty(NMinionDiveBombVfx.PropertyName._flightTime, ref variant9))
			{
				this._flightTime = variant9.As<float>();
			}
			Variant variant10;
			if (info.TryGetProperty(NMinionDiveBombVfx.PropertyName._fallingVfxEntryTime, ref variant10))
			{
				this._fallingVfxEntryTime = variant10.As<float>();
			}
			Variant variant11;
			if (info.TryGetProperty(NMinionDiveBombVfx.PropertyName._horizontalCurve, ref variant11))
			{
				this._horizontalCurve = variant11.As<Curve>();
			}
			Variant variant12;
			if (info.TryGetProperty(NMinionDiveBombVfx.PropertyName._verticalCurve, ref variant12))
			{
				this._verticalCurve = variant12.As<Curve>();
			}
			Variant variant13;
			if (info.TryGetProperty(NMinionDiveBombVfx.PropertyName._textureCurve, ref variant13))
			{
				this._textureCurve = variant13.As<Curve>();
			}
			Variant variant14;
			if (info.TryGetProperty(NMinionDiveBombVfx.PropertyName._maxHeight, ref variant14))
			{
				this._maxHeight = variant14.As<float>();
			}
			Variant variant15;
			if (info.TryGetProperty(NMinionDiveBombVfx.PropertyName._sourceOffset, ref variant15))
			{
				this._sourceOffset = variant15.As<Vector2>();
			}
			Variant variant16;
			if (info.TryGetProperty(NMinionDiveBombVfx.PropertyName._destinationOffset, ref variant16))
			{
				this._destinationOffset = variant16.As<Vector2>();
			}
			Variant variant17;
			if (info.TryGetProperty(NMinionDiveBombVfx.PropertyName._previousIndex, ref variant17))
			{
				this._previousIndex = variant17.As<int>();
			}
			Variant variant18;
			if (info.TryGetProperty(NMinionDiveBombVfx.PropertyName._sourcePosition, ref variant18))
			{
				this._sourcePosition = variant18.As<Vector2>();
			}
			Variant variant19;
			if (info.TryGetProperty(NMinionDiveBombVfx.PropertyName._destinationPosition, ref variant19))
			{
				this._destinationPosition = variant19.As<Vector2>();
			}
		}

		// Token: 0x0400086C RID: 2156
		[Nullable(1)]
		public static readonly string scenePath = SceneHelper.GetScenePath("vfx/vfx_minion_dive_bomb");

		// Token: 0x0400086D RID: 2157
		[Export(/*Failed to decode CustomAttribute blob!*/)]
		private Sprite2D _minionSprite;

		// Token: 0x0400086E RID: 2158
		[Nullable(new byte[] { 2, 1 })]
		[Export(/*Failed to decode CustomAttribute blob!*/)]
		private Array<Texture2D> _minionTextures;

		// Token: 0x0400086F RID: 2159
		[Export(/*Failed to decode CustomAttribute blob!*/)]
		private AnimationPlayer _minionAnimator;

		// Token: 0x04000870 RID: 2160
		[Nullable(new byte[] { 2, 1 })]
		[Export(/*Failed to decode CustomAttribute blob!*/)]
		private Array<string> _minionAnimations;

		// Token: 0x04000871 RID: 2161
		[Nullable(new byte[] { 2, 1 })]
		[Export(/*Failed to decode CustomAttribute blob!*/)]
		private Array<NParticlesContainer> _minionVfx;

		// Token: 0x04000872 RID: 2162
		[Export(/*Failed to decode CustomAttribute blob!*/)]
		private Node2D _fallingTrail;

		// Token: 0x04000873 RID: 2163
		[Export(/*Failed to decode CustomAttribute blob!*/)]
		private NParticlesContainer _fallingVfx;

		// Token: 0x04000874 RID: 2164
		[Export(/*Failed to decode CustomAttribute blob!*/)]
		private NParticlesContainer _impactVfx;

		// Token: 0x04000875 RID: 2165
		[Export(/*Failed to decode CustomAttribute blob!*/)]
		private float _flightTime;

		// Token: 0x04000876 RID: 2166
		[Export(/*Failed to decode CustomAttribute blob!*/)]
		private float _fallingVfxEntryTime;

		// Token: 0x04000877 RID: 2167
		[Export(/*Failed to decode CustomAttribute blob!*/)]
		private Curve _horizontalCurve;

		// Token: 0x04000878 RID: 2168
		[Export(/*Failed to decode CustomAttribute blob!*/)]
		private Curve _verticalCurve;

		// Token: 0x04000879 RID: 2169
		[Export(/*Failed to decode CustomAttribute blob!*/)]
		private Curve _textureCurve;

		// Token: 0x0400087A RID: 2170
		[Export(/*Failed to decode CustomAttribute blob!*/)]
		private float _maxHeight;

		// Token: 0x0400087B RID: 2171
		[Export(/*Failed to decode CustomAttribute blob!*/)]
		private Vector2 _sourceOffset;

		// Token: 0x0400087C RID: 2172
		[Export(/*Failed to decode CustomAttribute blob!*/)]
		private Vector2 _destinationOffset;

		// Token: 0x0400087D RID: 2173
		private int _previousIndex = -1;

		// Token: 0x0400087E RID: 2174
		private Vector2 _sourcePosition;

		// Token: 0x0400087F RID: 2175
		private CancellationTokenSource _cts;

		// Token: 0x04000880 RID: 2176
		private Vector2 _destinationPosition;

		/// <summary>
		/// Cached StringNames for the methods contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x020010C2 RID: 4290
		[NullableContext(0)]
		public class MethodName : Node2D.MethodName
		{
			/// <summary>
			/// Cached name for the '_ExitTree' method.
			/// </summary>
			// Token: 0x040041BB RID: 16827
			public static readonly StringName _ExitTree = "_ExitTree";

			/// <summary>
			/// Cached name for the 'Create' method.
			/// </summary>
			// Token: 0x040041BC RID: 16828
			public static readonly StringName Create = "Create";

			/// <summary>
			/// Cached name for the 'Initialize' method.
			/// </summary>
			// Token: 0x040041BD RID: 16829
			public static readonly StringName Initialize = "Initialize";

			/// <summary>
			/// Cached name for the '_Ready' method.
			/// </summary>
			// Token: 0x040041BE RID: 16830
			public static readonly StringName _Ready = "_Ready";

			/// <summary>
			/// Cached name for the 'SetMinionVisible' method.
			/// </summary>
			// Token: 0x040041BF RID: 16831
			public static readonly StringName SetMinionVisible = "SetMinionVisible";

			/// <summary>
			/// Cached name for the 'UpdateMinionSprite' method.
			/// </summary>
			// Token: 0x040041C0 RID: 16832
			public static readonly StringName UpdateMinionSprite = "UpdateMinionSprite";
		}

		/// <summary>
		/// Cached StringNames for the properties and fields contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x020010C3 RID: 4291
		[NullableContext(0)]
		public class PropertyName : Node2D.PropertyName
		{
			/// <summary>
			/// Cached name for the 'SourceFinalPosition' property.
			/// </summary>
			// Token: 0x040041C1 RID: 16833
			public static readonly StringName SourceFinalPosition = "SourceFinalPosition";

			/// <summary>
			/// Cached name for the 'DestinationFinalPosition' property.
			/// </summary>
			// Token: 0x040041C2 RID: 16834
			public static readonly StringName DestinationFinalPosition = "DestinationFinalPosition";

			/// <summary>
			/// Cached name for the '_minionSprite' field.
			/// </summary>
			// Token: 0x040041C3 RID: 16835
			public static readonly StringName _minionSprite = "_minionSprite";

			/// <summary>
			/// Cached name for the '_minionTextures' field.
			/// </summary>
			// Token: 0x040041C4 RID: 16836
			public static readonly StringName _minionTextures = "_minionTextures";

			/// <summary>
			/// Cached name for the '_minionAnimator' field.
			/// </summary>
			// Token: 0x040041C5 RID: 16837
			public static readonly StringName _minionAnimator = "_minionAnimator";

			/// <summary>
			/// Cached name for the '_minionAnimations' field.
			/// </summary>
			// Token: 0x040041C6 RID: 16838
			public static readonly StringName _minionAnimations = "_minionAnimations";

			/// <summary>
			/// Cached name for the '_minionVfx' field.
			/// </summary>
			// Token: 0x040041C7 RID: 16839
			public static readonly StringName _minionVfx = "_minionVfx";

			/// <summary>
			/// Cached name for the '_fallingTrail' field.
			/// </summary>
			// Token: 0x040041C8 RID: 16840
			public static readonly StringName _fallingTrail = "_fallingTrail";

			/// <summary>
			/// Cached name for the '_fallingVfx' field.
			/// </summary>
			// Token: 0x040041C9 RID: 16841
			public static readonly StringName _fallingVfx = "_fallingVfx";

			/// <summary>
			/// Cached name for the '_impactVfx' field.
			/// </summary>
			// Token: 0x040041CA RID: 16842
			public static readonly StringName _impactVfx = "_impactVfx";

			/// <summary>
			/// Cached name for the '_flightTime' field.
			/// </summary>
			// Token: 0x040041CB RID: 16843
			public static readonly StringName _flightTime = "_flightTime";

			/// <summary>
			/// Cached name for the '_fallingVfxEntryTime' field.
			/// </summary>
			// Token: 0x040041CC RID: 16844
			public static readonly StringName _fallingVfxEntryTime = "_fallingVfxEntryTime";

			/// <summary>
			/// Cached name for the '_horizontalCurve' field.
			/// </summary>
			// Token: 0x040041CD RID: 16845
			public static readonly StringName _horizontalCurve = "_horizontalCurve";

			/// <summary>
			/// Cached name for the '_verticalCurve' field.
			/// </summary>
			// Token: 0x040041CE RID: 16846
			public static readonly StringName _verticalCurve = "_verticalCurve";

			/// <summary>
			/// Cached name for the '_textureCurve' field.
			/// </summary>
			// Token: 0x040041CF RID: 16847
			public static readonly StringName _textureCurve = "_textureCurve";

			/// <summary>
			/// Cached name for the '_maxHeight' field.
			/// </summary>
			// Token: 0x040041D0 RID: 16848
			public static readonly StringName _maxHeight = "_maxHeight";

			/// <summary>
			/// Cached name for the '_sourceOffset' field.
			/// </summary>
			// Token: 0x040041D1 RID: 16849
			public static readonly StringName _sourceOffset = "_sourceOffset";

			/// <summary>
			/// Cached name for the '_destinationOffset' field.
			/// </summary>
			// Token: 0x040041D2 RID: 16850
			public static readonly StringName _destinationOffset = "_destinationOffset";

			/// <summary>
			/// Cached name for the '_previousIndex' field.
			/// </summary>
			// Token: 0x040041D3 RID: 16851
			public static readonly StringName _previousIndex = "_previousIndex";

			/// <summary>
			/// Cached name for the '_sourcePosition' field.
			/// </summary>
			// Token: 0x040041D4 RID: 16852
			public static readonly StringName _sourcePosition = "_sourcePosition";

			/// <summary>
			/// Cached name for the '_destinationPosition' field.
			/// </summary>
			// Token: 0x040041D5 RID: 16853
			public static readonly StringName _destinationPosition = "_destinationPosition";
		}

		/// <summary>
		/// Cached StringNames for the signals contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x020010C4 RID: 4292
		[NullableContext(0)]
		public class SignalName : Node2D.SignalName
		{
		}
	}
}
