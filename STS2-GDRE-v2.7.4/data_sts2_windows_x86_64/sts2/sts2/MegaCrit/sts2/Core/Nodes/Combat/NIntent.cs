using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Godot;
using Godot.Bridge;
using Godot.NativeInterop;
using MegaCrit.Sts2.addons.mega_text;
using MegaCrit.Sts2.Core.Assets;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Intents;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.MonsterMoves.Intents;
using MegaCrit.Sts2.Core.Nodes.Rooms;

namespace MegaCrit.Sts2.Core.Nodes.Combat
{
	// Token: 0x02000404 RID: 1028
	[NullableContext(1)]
	[Nullable(0)]
	[ScriptPath("res://src/Core/Nodes/Combat/NIntent.cs")]
	public class NIntent : Control
	{
		// Token: 0x06004018 RID: 16408 RVA: 0x001ED5DC File Offset: 0x001EB7DC
		public override void _Ready()
		{
			this._intentHolder = base.GetNode<Control>("%IntentHolder");
			this._intentSprite = base.GetNode<Sprite2D>("%Intent");
			this._valueLabel = base.GetNode<MegaRichTextLabel>("%Value");
			this._intentParticle = base.GetNode<CpuParticles2D>("%IntentParticle");
			base.Connect(Control.SignalName.MouseEntered, Callable.From(new Action(this.OnHovered)), 0U);
			base.Connect(Control.SignalName.MouseExited, Callable.From(new Action(this.OnUnhovered)), 0U);
			this._intentHolder.Modulate = (NCombatUi.IsDebugHidingIntent ? Colors.Transparent : Colors.White);
		}

		// Token: 0x06004019 RID: 16409 RVA: 0x001ED69C File Offset: 0x001EB89C
		public override void _EnterTree()
		{
			CombatManager.Instance.StateTracker.CombatStateChanged += this.OnCombatStateChanged;
			if (NCombatRoom.Instance != null)
			{
				this._combatRoom = NCombatRoom.Instance;
				this._combatRoom.Ui.DebugToggleIntent += this.DebugToggleVisibility;
			}
		}

		// Token: 0x0600401A RID: 16410 RVA: 0x001ED6F2 File Offset: 0x001EB8F2
		private void DebugToggleVisibility()
		{
			this._intentHolder.Modulate = (NCombatUi.IsDebugHidingIntent ? Colors.Transparent : Colors.White);
		}

		// Token: 0x0600401B RID: 16411 RVA: 0x001ED714 File Offset: 0x001EB914
		public override void _ExitTree()
		{
			CombatManager.Instance.StateTracker.CombatStateChanged -= this.OnCombatStateChanged;
			if (this._combatRoom != null)
			{
				this._combatRoom.Ui.DebugToggleIntent -= this.DebugToggleVisibility;
			}
		}

		// Token: 0x0600401C RID: 16412 RVA: 0x001ED760 File Offset: 0x001EB960
		public void UpdateIntent(AbstractIntent intent, IEnumerable<Creature> targets, Creature owner)
		{
			this._owner = owner;
			this._targets = targets;
			this._intent = intent;
			this.UpdateVisuals();
		}

		// Token: 0x0600401D RID: 16413 RVA: 0x001ED77D File Offset: 0x001EB97D
		private void OnCombatStateChanged(CombatState _)
		{
			if (!this._isFrozen)
			{
				this.UpdateVisuals();
			}
		}

		// Token: 0x0600401E RID: 16414 RVA: 0x001ED790 File Offset: 0x001EB990
		private void UpdateVisuals()
		{
			string animation = this._intent.GetAnimation(this._targets, this._owner);
			if (this._animationName != animation)
			{
				this._animationName = animation;
				this._animationFrame = null;
				this._timeAccumulator = 0f;
				int animationFrameCount = IntentAnimData.GetAnimationFrameCount(animation);
				this._animationFrames.Clear();
				for (int i = 0; i < animationFrameCount; i++)
				{
					this._animationFrames.Add(PreloadManager.Cache.GetTexture2D(IntentAnimData.GetAnimationFrame(animation, i)));
				}
			}
			this._intentParticle.Texture = this._intent.GetTexture(this._targets, this._owner);
			MegaRichTextLabel valueLabel = this._valueLabel;
			AbstractIntent intent = this._intent;
			AttackIntent attackIntent = intent as AttackIntent;
			string text;
			if (attackIntent == null)
			{
				if (!(intent is StatusIntent))
				{
					text = string.Empty;
				}
				else
				{
					text = this._intent.GetIntentLabel(this._targets, this._owner).GetFormattedText() ?? "";
				}
			}
			else
			{
				text = attackIntent.GetIntentLabel(this._targets, this._owner).GetFormattedText() ?? "";
			}
			valueLabel.Text = text;
		}

		// Token: 0x0600401F RID: 16415 RVA: 0x001ED8C4 File Offset: 0x001EBAC4
		public override void _Process(double delta)
		{
			this._intentHolder.Position = Vector2.Up * (Mathf.Sin(Time.GetTicksMsec() * 0.001f * 3.1415927f + this._timeOffset) * 10f + 8f);
			if (this._animationFrames.Count > 0)
			{
				int num = (int)(this._timeAccumulator * 15f) % this._animationFrames.Count;
				int? animationFrame = this._animationFrame;
				int num2 = num;
				if (!((animationFrame.GetValueOrDefault() == num2) & (animationFrame != null)))
				{
					this._animationFrame = new int?(num);
					this._intentSprite.Texture = this._animationFrames[num];
				}
				this._timeAccumulator += (float)delta;
			}
		}

		// Token: 0x06004020 RID: 16416 RVA: 0x001ED988 File Offset: 0x001EBB88
		public static NIntent Create(float startTime)
		{
			NIntent nintent = PreloadManager.Cache.GetScene("res://scenes/combat/intent.tscn").Instantiate<NIntent>(0L);
			nintent._timeOffset = startTime;
			return nintent;
		}

		// Token: 0x1700093B RID: 2363
		// (get) Token: 0x06004021 RID: 16417 RVA: 0x001ED9B4 File Offset: 0x001EBBB4
		public static IEnumerable<string> AssetPaths
		{
			get
			{
				List<string> list = new List<string>();
				list.Add("res://scenes/combat/intent.tscn");
				list.AddRange(IntentAnimData.AssetPaths);
				return new <>z__ReadOnlyList<string>(list);
			}
		}

		// Token: 0x06004022 RID: 16418 RVA: 0x001ED9D6 File Offset: 0x001EBBD6
		public void PlayPerform()
		{
			this._intentParticle.Emitting = true;
		}

		// Token: 0x06004023 RID: 16419 RVA: 0x001ED9E4 File Offset: 0x001EBBE4
		public void SetFrozen(bool isFrozen)
		{
			this._isFrozen = isFrozen;
		}

		// Token: 0x06004024 RID: 16420 RVA: 0x001ED9F0 File Offset: 0x001EBBF0
		private void OnHovered()
		{
			if (!this._intent.HasIntentTip)
			{
				return;
			}
			NCombatRoom instance = NCombatRoom.Instance;
			if (instance == null)
			{
				return;
			}
			NCreature creatureNode = instance.GetCreatureNode(this._owner);
			if (creatureNode == null)
			{
				return;
			}
			creatureNode.ShowHoverTips(new <>z__ReadOnlySingleElementList<IHoverTip>(this._intent.GetHoverTip(this._targets, this._owner)));
		}

		// Token: 0x06004025 RID: 16421 RVA: 0x001EDA4B File Offset: 0x001EBC4B
		private void OnUnhovered()
		{
			NCombatRoom instance = NCombatRoom.Instance;
			if (instance == null)
			{
				return;
			}
			NCreature creatureNode = instance.GetCreatureNode(this._owner);
			if (creatureNode == null)
			{
				return;
			}
			creatureNode.HideHoverTips();
		}

		/// <summary>
		/// Get the method information for all the methods declared in this class.
		/// This method is used by Godot to register the available methods in the editor.
		/// Do not call this method.
		/// </summary>
		// Token: 0x06004026 RID: 16422 RVA: 0x001EDA6C File Offset: 0x001EBC6C
		[NullableContext(0)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal static List<MethodInfo> GetGodotMethodList()
		{
			return new List<MethodInfo>(11)
			{
				new MethodInfo(NIntent.MethodName._Ready, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NIntent.MethodName._EnterTree, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NIntent.MethodName.DebugToggleVisibility, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NIntent.MethodName._ExitTree, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NIntent.MethodName.UpdateVisuals, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NIntent.MethodName._Process, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, new List<PropertyInfo>
				{
					new PropertyInfo(3L, "delta", 0L, "", 6L, false)
				}, null),
				new MethodInfo(NIntent.MethodName.Create, new PropertyInfo(24L, "", 0L, "", 6L, new StringName("Control"), false), 33L, new List<PropertyInfo>
				{
					new PropertyInfo(3L, "startTime", 0L, "", 6L, false)
				}, null),
				new MethodInfo(NIntent.MethodName.PlayPerform, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NIntent.MethodName.SetFrozen, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, new List<PropertyInfo>
				{
					new PropertyInfo(1L, "isFrozen", 0L, "", 6L, false)
				}, null),
				new MethodInfo(NIntent.MethodName.OnHovered, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NIntent.MethodName.OnUnhovered, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null)
			};
		}

		/// <inheritdoc />
		// Token: 0x06004027 RID: 16423 RVA: 0x001EDD04 File Offset: 0x001EBF04
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool InvokeGodotClassMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
		{
			if ((ref method) == NIntent.MethodName._Ready && args.Count == 0)
			{
				this._Ready();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NIntent.MethodName._EnterTree && args.Count == 0)
			{
				this._EnterTree();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NIntent.MethodName.DebugToggleVisibility && args.Count == 0)
			{
				this.DebugToggleVisibility();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NIntent.MethodName._ExitTree && args.Count == 0)
			{
				this._ExitTree();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NIntent.MethodName.UpdateVisuals && args.Count == 0)
			{
				this.UpdateVisuals();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NIntent.MethodName._Process && args.Count == 1)
			{
				this._Process(VariantUtils.ConvertTo<double>(args[0]));
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NIntent.MethodName.Create && args.Count == 1)
			{
				NIntent nintent = NIntent.Create(VariantUtils.ConvertTo<float>(args[0]));
				ret = VariantUtils.CreateFrom<NIntent>(ref nintent);
				return true;
			}
			if ((ref method) == NIntent.MethodName.PlayPerform && args.Count == 0)
			{
				this.PlayPerform();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NIntent.MethodName.SetFrozen && args.Count == 1)
			{
				this.SetFrozen(VariantUtils.ConvertTo<bool>(args[0]));
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NIntent.MethodName.OnHovered && args.Count == 0)
			{
				this.OnHovered();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NIntent.MethodName.OnUnhovered && args.Count == 0)
			{
				this.OnUnhovered();
				ret = default(godot_variant);
				return true;
			}
			return base.InvokeGodotClassMethod(ref method, args, ref ret);
		}

		// Token: 0x06004028 RID: 16424 RVA: 0x001EDEE4 File Offset: 0x001EC0E4
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal static bool InvokeGodotClassStaticMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
		{
			if ((ref method) == NIntent.MethodName.Create && args.Count == 1)
			{
				NIntent nintent = NIntent.Create(VariantUtils.ConvertTo<float>(args[0]));
				ret = VariantUtils.CreateFrom<NIntent>(ref nintent);
				return true;
			}
			ret = default(godot_variant);
			return false;
		}

		/// <inheritdoc />
		// Token: 0x06004029 RID: 16425 RVA: 0x001EDF34 File Offset: 0x001EC134
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool HasGodotClassMethod(in godot_string_name method)
		{
			return (ref method) == NIntent.MethodName._Ready || (ref method) == NIntent.MethodName._EnterTree || (ref method) == NIntent.MethodName.DebugToggleVisibility || (ref method) == NIntent.MethodName._ExitTree || (ref method) == NIntent.MethodName.UpdateVisuals || (ref method) == NIntent.MethodName._Process || (ref method) == NIntent.MethodName.Create || (ref method) == NIntent.MethodName.PlayPerform || (ref method) == NIntent.MethodName.SetFrozen || (ref method) == NIntent.MethodName.OnHovered || (ref method) == NIntent.MethodName.OnUnhovered || base.HasGodotClassMethod(ref method);
		}

		/// <inheritdoc />
		// Token: 0x0600402A RID: 16426 RVA: 0x001EDFF0 File Offset: 0x001EC1F0
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool SetGodotClassPropertyValue(in godot_string_name name, in godot_variant value)
		{
			if ((ref name) == NIntent.PropertyName._intentHolder)
			{
				this._intentHolder = VariantUtils.ConvertTo<Control>(ref value);
				return true;
			}
			if ((ref name) == NIntent.PropertyName._intentSprite)
			{
				this._intentSprite = VariantUtils.ConvertTo<Sprite2D>(ref value);
				return true;
			}
			if ((ref name) == NIntent.PropertyName._valueLabel)
			{
				this._valueLabel = VariantUtils.ConvertTo<MegaRichTextLabel>(ref value);
				return true;
			}
			if ((ref name) == NIntent.PropertyName._intentParticle)
			{
				this._intentParticle = VariantUtils.ConvertTo<CpuParticles2D>(ref value);
				return true;
			}
			if ((ref name) == NIntent.PropertyName._timeOffset)
			{
				this._timeOffset = VariantUtils.ConvertTo<float>(ref value);
				return true;
			}
			if ((ref name) == NIntent.PropertyName._timeAccumulator)
			{
				this._timeAccumulator = VariantUtils.ConvertTo<float>(ref value);
				return true;
			}
			if ((ref name) == NIntent.PropertyName._isFrozen)
			{
				this._isFrozen = VariantUtils.ConvertTo<bool>(ref value);
				return true;
			}
			if ((ref name) == NIntent.PropertyName._animationName)
			{
				this._animationName = VariantUtils.ConvertTo<string>(ref value);
				return true;
			}
			if ((ref name) == NIntent.PropertyName._combatRoom)
			{
				this._combatRoom = VariantUtils.ConvertTo<NCombatRoom>(ref value);
				return true;
			}
			return base.SetGodotClassPropertyValue(ref name, ref value);
		}

		/// <inheritdoc />
		// Token: 0x0600402B RID: 16427 RVA: 0x001EE0F8 File Offset: 0x001EC2F8
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
		{
			if ((ref name) == NIntent.PropertyName._intentHolder)
			{
				value = VariantUtils.CreateFrom<Control>(ref this._intentHolder);
				return true;
			}
			if ((ref name) == NIntent.PropertyName._intentSprite)
			{
				value = VariantUtils.CreateFrom<Sprite2D>(ref this._intentSprite);
				return true;
			}
			if ((ref name) == NIntent.PropertyName._valueLabel)
			{
				value = VariantUtils.CreateFrom<MegaRichTextLabel>(ref this._valueLabel);
				return true;
			}
			if ((ref name) == NIntent.PropertyName._intentParticle)
			{
				value = VariantUtils.CreateFrom<CpuParticles2D>(ref this._intentParticle);
				return true;
			}
			if ((ref name) == NIntent.PropertyName._timeOffset)
			{
				value = VariantUtils.CreateFrom<float>(ref this._timeOffset);
				return true;
			}
			if ((ref name) == NIntent.PropertyName._timeAccumulator)
			{
				value = VariantUtils.CreateFrom<float>(ref this._timeAccumulator);
				return true;
			}
			if ((ref name) == NIntent.PropertyName._isFrozen)
			{
				value = VariantUtils.CreateFrom<bool>(ref this._isFrozen);
				return true;
			}
			if ((ref name) == NIntent.PropertyName._animationName)
			{
				value = VariantUtils.CreateFrom<string>(ref this._animationName);
				return true;
			}
			if ((ref name) == NIntent.PropertyName._combatRoom)
			{
				value = VariantUtils.CreateFrom<NCombatRoom>(ref this._combatRoom);
				return true;
			}
			return base.GetGodotClassPropertyValue(ref name, ref value);
		}

		/// <summary>
		/// Get the property information for all the properties declared in this class.
		/// This method is used by Godot to register the available properties in the editor.
		/// Do not call this method.
		/// </summary>
		// Token: 0x0600402C RID: 16428 RVA: 0x001EE230 File Offset: 0x001EC430
		[NullableContext(0)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal static List<PropertyInfo> GetGodotPropertyList()
		{
			return new List<PropertyInfo>
			{
				new PropertyInfo(24L, NIntent.PropertyName._intentHolder, 0L, "", 4096L, false),
				new PropertyInfo(24L, NIntent.PropertyName._intentSprite, 0L, "", 4096L, false),
				new PropertyInfo(24L, NIntent.PropertyName._valueLabel, 0L, "", 4096L, false),
				new PropertyInfo(24L, NIntent.PropertyName._intentParticle, 0L, "", 4096L, false),
				new PropertyInfo(3L, NIntent.PropertyName._timeOffset, 0L, "", 4096L, false),
				new PropertyInfo(3L, NIntent.PropertyName._timeAccumulator, 0L, "", 4096L, false),
				new PropertyInfo(1L, NIntent.PropertyName._isFrozen, 0L, "", 4096L, false),
				new PropertyInfo(4L, NIntent.PropertyName._animationName, 0L, "", 4096L, false),
				new PropertyInfo(24L, NIntent.PropertyName._combatRoom, 0L, "", 4096L, false)
			};
		}

		/// <inheritdoc />
		// Token: 0x0600402D RID: 16429 RVA: 0x001EE36C File Offset: 0x001EC56C
		[NullableContext(0)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void SaveGodotObjectData(GodotSerializationInfo info)
		{
			base.SaveGodotObjectData(info);
			info.AddProperty(NIntent.PropertyName._intentHolder, Variant.From<Control>(ref this._intentHolder));
			info.AddProperty(NIntent.PropertyName._intentSprite, Variant.From<Sprite2D>(ref this._intentSprite));
			info.AddProperty(NIntent.PropertyName._valueLabel, Variant.From<MegaRichTextLabel>(ref this._valueLabel));
			info.AddProperty(NIntent.PropertyName._intentParticle, Variant.From<CpuParticles2D>(ref this._intentParticle));
			info.AddProperty(NIntent.PropertyName._timeOffset, Variant.From<float>(ref this._timeOffset));
			info.AddProperty(NIntent.PropertyName._timeAccumulator, Variant.From<float>(ref this._timeAccumulator));
			info.AddProperty(NIntent.PropertyName._isFrozen, Variant.From<bool>(ref this._isFrozen));
			info.AddProperty(NIntent.PropertyName._animationName, Variant.From<string>(ref this._animationName));
			info.AddProperty(NIntent.PropertyName._combatRoom, Variant.From<NCombatRoom>(ref this._combatRoom));
		}

		/// <inheritdoc />
		// Token: 0x0600402E RID: 16430 RVA: 0x001EE448 File Offset: 0x001EC648
		[NullableContext(0)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void RestoreGodotObjectData(GodotSerializationInfo info)
		{
			base.RestoreGodotObjectData(info);
			Variant variant;
			if (info.TryGetProperty(NIntent.PropertyName._intentHolder, ref variant))
			{
				this._intentHolder = variant.As<Control>();
			}
			Variant variant2;
			if (info.TryGetProperty(NIntent.PropertyName._intentSprite, ref variant2))
			{
				this._intentSprite = variant2.As<Sprite2D>();
			}
			Variant variant3;
			if (info.TryGetProperty(NIntent.PropertyName._valueLabel, ref variant3))
			{
				this._valueLabel = variant3.As<MegaRichTextLabel>();
			}
			Variant variant4;
			if (info.TryGetProperty(NIntent.PropertyName._intentParticle, ref variant4))
			{
				this._intentParticle = variant4.As<CpuParticles2D>();
			}
			Variant variant5;
			if (info.TryGetProperty(NIntent.PropertyName._timeOffset, ref variant5))
			{
				this._timeOffset = variant5.As<float>();
			}
			Variant variant6;
			if (info.TryGetProperty(NIntent.PropertyName._timeAccumulator, ref variant6))
			{
				this._timeAccumulator = variant6.As<float>();
			}
			Variant variant7;
			if (info.TryGetProperty(NIntent.PropertyName._isFrozen, ref variant7))
			{
				this._isFrozen = variant7.As<bool>();
			}
			Variant variant8;
			if (info.TryGetProperty(NIntent.PropertyName._animationName, ref variant8))
			{
				this._animationName = variant8.As<string>();
			}
			Variant variant9;
			if (info.TryGetProperty(NIntent.PropertyName._combatRoom, ref variant9))
			{
				this._combatRoom = variant9.As<NCombatRoom>();
			}
		}

		// Token: 0x040018E9 RID: 6377
		private const string _scenePath = "res://scenes/combat/intent.tscn";

		// Token: 0x040018EA RID: 6378
		private const float _bobSpeed = 3.1415927f;

		// Token: 0x040018EB RID: 6379
		private const float _bobDistance = 10f;

		// Token: 0x040018EC RID: 6380
		private const float _bobOffset = 8f;

		// Token: 0x040018ED RID: 6381
		private const int _animationFps = 15;

		// Token: 0x040018EE RID: 6382
		private Control _intentHolder;

		// Token: 0x040018EF RID: 6383
		private Sprite2D _intentSprite;

		// Token: 0x040018F0 RID: 6384
		private MegaRichTextLabel _valueLabel;

		// Token: 0x040018F1 RID: 6385
		private CpuParticles2D _intentParticle;

		// Token: 0x040018F2 RID: 6386
		private Creature _owner;

		// Token: 0x040018F3 RID: 6387
		private IEnumerable<Creature> _targets;

		// Token: 0x040018F4 RID: 6388
		private AbstractIntent _intent;

		// Token: 0x040018F5 RID: 6389
		private float _timeOffset;

		// Token: 0x040018F6 RID: 6390
		private float _timeAccumulator;

		// Token: 0x040018F7 RID: 6391
		private bool _isFrozen;

		// Token: 0x040018F8 RID: 6392
		[Nullable(2)]
		private string _animationName;

		// Token: 0x040018F9 RID: 6393
		private readonly List<Texture2D> _animationFrames = new List<Texture2D>();

		// Token: 0x040018FA RID: 6394
		private int? _animationFrame;

		// Token: 0x040018FB RID: 6395
		[Nullable(2)]
		private NCombatRoom _combatRoom;

		/// <summary>
		/// Cached StringNames for the methods contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x020018F2 RID: 6386
		[NullableContext(0)]
		public class MethodName : Control.MethodName
		{
			/// <summary>
			/// Cached name for the '_Ready' method.
			/// </summary>
			// Token: 0x040061E0 RID: 25056
			public static readonly StringName _Ready = "_Ready";

			/// <summary>
			/// Cached name for the '_EnterTree' method.
			/// </summary>
			// Token: 0x040061E1 RID: 25057
			public static readonly StringName _EnterTree = "_EnterTree";

			/// <summary>
			/// Cached name for the 'DebugToggleVisibility' method.
			/// </summary>
			// Token: 0x040061E2 RID: 25058
			public static readonly StringName DebugToggleVisibility = "DebugToggleVisibility";

			/// <summary>
			/// Cached name for the '_ExitTree' method.
			/// </summary>
			// Token: 0x040061E3 RID: 25059
			public static readonly StringName _ExitTree = "_ExitTree";

			/// <summary>
			/// Cached name for the 'UpdateVisuals' method.
			/// </summary>
			// Token: 0x040061E4 RID: 25060
			public static readonly StringName UpdateVisuals = "UpdateVisuals";

			/// <summary>
			/// Cached name for the '_Process' method.
			/// </summary>
			// Token: 0x040061E5 RID: 25061
			public static readonly StringName _Process = "_Process";

			/// <summary>
			/// Cached name for the 'Create' method.
			/// </summary>
			// Token: 0x040061E6 RID: 25062
			public static readonly StringName Create = "Create";

			/// <summary>
			/// Cached name for the 'PlayPerform' method.
			/// </summary>
			// Token: 0x040061E7 RID: 25063
			public static readonly StringName PlayPerform = "PlayPerform";

			/// <summary>
			/// Cached name for the 'SetFrozen' method.
			/// </summary>
			// Token: 0x040061E8 RID: 25064
			public static readonly StringName SetFrozen = "SetFrozen";

			/// <summary>
			/// Cached name for the 'OnHovered' method.
			/// </summary>
			// Token: 0x040061E9 RID: 25065
			public static readonly StringName OnHovered = "OnHovered";

			/// <summary>
			/// Cached name for the 'OnUnhovered' method.
			/// </summary>
			// Token: 0x040061EA RID: 25066
			public static readonly StringName OnUnhovered = "OnUnhovered";
		}

		/// <summary>
		/// Cached StringNames for the properties and fields contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x020018F3 RID: 6387
		[NullableContext(0)]
		public class PropertyName : Control.PropertyName
		{
			/// <summary>
			/// Cached name for the '_intentHolder' field.
			/// </summary>
			// Token: 0x040061EB RID: 25067
			public static readonly StringName _intentHolder = "_intentHolder";

			/// <summary>
			/// Cached name for the '_intentSprite' field.
			/// </summary>
			// Token: 0x040061EC RID: 25068
			public static readonly StringName _intentSprite = "_intentSprite";

			/// <summary>
			/// Cached name for the '_valueLabel' field.
			/// </summary>
			// Token: 0x040061ED RID: 25069
			public static readonly StringName _valueLabel = "_valueLabel";

			/// <summary>
			/// Cached name for the '_intentParticle' field.
			/// </summary>
			// Token: 0x040061EE RID: 25070
			public static readonly StringName _intentParticle = "_intentParticle";

			/// <summary>
			/// Cached name for the '_timeOffset' field.
			/// </summary>
			// Token: 0x040061EF RID: 25071
			public static readonly StringName _timeOffset = "_timeOffset";

			/// <summary>
			/// Cached name for the '_timeAccumulator' field.
			/// </summary>
			// Token: 0x040061F0 RID: 25072
			public static readonly StringName _timeAccumulator = "_timeAccumulator";

			/// <summary>
			/// Cached name for the '_isFrozen' field.
			/// </summary>
			// Token: 0x040061F1 RID: 25073
			public static readonly StringName _isFrozen = "_isFrozen";

			/// <summary>
			/// Cached name for the '_animationName' field.
			/// </summary>
			// Token: 0x040061F2 RID: 25074
			public static readonly StringName _animationName = "_animationName";

			/// <summary>
			/// Cached name for the '_combatRoom' field.
			/// </summary>
			// Token: 0x040061F3 RID: 25075
			public static readonly StringName _combatRoom = "_combatRoom";
		}

		/// <summary>
		/// Cached StringNames for the signals contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x020018F4 RID: 6388
		[NullableContext(0)]
		public class SignalName : Control.SignalName
		{
		}
	}
}
