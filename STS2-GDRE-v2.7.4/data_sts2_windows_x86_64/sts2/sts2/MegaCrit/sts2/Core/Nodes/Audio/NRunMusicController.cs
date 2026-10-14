using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;
using Godot;
using Godot.Bridge;
using Godot.NativeInterop;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Nodes.Screens.Map;
using MegaCrit.Sts2.Core.Random;
using MegaCrit.Sts2.Core.Rooms;
using MegaCrit.Sts2.Core.Runs;

namespace MegaCrit.Sts2.Core.Nodes.Audio
{
	/// <summary>
	/// Manages the music specifically for run.
	/// Looking into info such as room type to decided what tracks to transition to.
	/// </summary>
	// Token: 0x02000423 RID: 1059
	[NullableContext(1)]
	[Nullable(0)]
	[ScriptPath("res://src/Core/Nodes/Audio/NRunMusicController.cs")]
	public class NRunMusicController : Node
	{
		// Token: 0x170009A2 RID: 2466
		// (get) Token: 0x060043A2 RID: 17314 RVA: 0x0020D67D File Offset: 0x0020B87D
		[Nullable(2)]
		public static NRunMusicController Instance
		{
			[NullableContext(2)]
			get
			{
				NRun instance = NRun.Instance;
				if (instance == null)
				{
					return null;
				}
				return instance.RunMusicController;
			}
		}

		// Token: 0x060043A3 RID: 17315 RVA: 0x0020D690 File Offset: 0x0020B890
		private NRunMusicController.MusicProgressTrack GetTrack(RoomType roomType)
		{
			if (roomType.IsCombatRoom() && !CombatManager.Instance.IsInProgress)
			{
				return NRunMusicController.MusicProgressTrack.CombatEnd;
			}
			switch (roomType)
			{
			case RoomType.Monster:
				return NRunMusicController.MusicProgressTrack.Enemy;
			case RoomType.Elite:
				return NRunMusicController.MusicProgressTrack.Elite;
			case RoomType.Boss:
				return NRunMusicController.MusicProgressTrack.Elite;
			case RoomType.Treasure:
				return NRunMusicController.MusicProgressTrack.Treasure;
			case RoomType.Shop:
				return NRunMusicController.MusicProgressTrack.Merchant;
			case RoomType.Event:
			{
				EventRoom eventRoom = this._runState.CurrentRoom as EventRoom;
				if (eventRoom != null && eventRoom.CanonicalEvent is AncientEventModel)
				{
					return NRunMusicController.MusicProgressTrack.Init;
				}
				return NRunMusicController.MusicProgressTrack.Unknown;
			}
			case RoomType.RestSite:
				return NRunMusicController.MusicProgressTrack.Rest;
			default:
				return NRunMusicController.MusicProgressTrack.Init;
			}
		}

		// Token: 0x060043A4 RID: 17316 RVA: 0x0020D70B File Offset: 0x0020B90B
		public override void _Ready()
		{
			this._proxy = base.GetNode<Node>("Proxy");
		}

		// Token: 0x060043A5 RID: 17317 RVA: 0x0020D723 File Offset: 0x0020B923
		public override void _ExitTree()
		{
			this.StopMusic();
		}

		// Token: 0x060043A6 RID: 17318 RVA: 0x0020D72B File Offset: 0x0020B92B
		public void SetRunState(IRunState runState)
		{
			this._runState = runState;
		}

		/// <summary>
		/// Resolves which act background track to play for a given run seed, or null when nothing
		/// should change. Selection is deterministic in <paramref name="seed" />, so a run always
		/// picks the same track. Returns null when the act has no music, or when the selected track
		/// already equals <paramref name="currentTrack" />. The equality check makes a redundant call
		/// a no-op instead of stopping and recreating the FMOD event, which restarts the track from
		/// the top. Two such calls fire on run start, from NRun._Ready and the act-entry path.
		/// </summary>
		// Token: 0x060043A7 RID: 17319 RVA: 0x0020D734 File Offset: 0x0020B934
		public static NRunMusicController.MusicSelection? ResolveMusic([Nullable(2)] string currentTrack, string[] options, string[] bankPaths, ulong seed)
		{
			if (options.Length == 0)
			{
				return null;
			}
			int num = new Rng(seed, "bg_music").NextInt(0, options.Length);
			string text = options[num];
			if (text == currentTrack)
			{
				return null;
			}
			return new NRunMusicController.MusicSelection?(new NRunMusicController.MusicSelection(text, bankPaths[num]));
		}

		// Token: 0x060043A8 RID: 17320 RVA: 0x0020D78C File Offset: 0x0020B98C
		public void UpdateMusic()
		{
			if (NonInteractiveMode.IsActive)
			{
				return;
			}
			NRunMusicController.MusicSelection? musicSelection = NRunMusicController.ResolveMusic(this._currentTrack, this._runState.Act.BgMusicOptions, this._runState.Act.MusicBankPaths, this._runState.Rng.Seed);
			if (musicSelection == null)
			{
				return;
			}
			if (musicSelection.Value.Track == this._failedTrack)
			{
				return;
			}
			if (!this.LoadActBank(musicSelection.Value.BankPath, musicSelection.Value.Track))
			{
				this._failedTrack = musicSelection.Value.Track;
				return;
			}
			this._failedTrack = null;
			this._currentTrack = musicSelection.Value.Track;
			this._proxy.Call("update_music", new Variant[] { this._currentTrack });
			this._proxy.Call("update_global_parameter", new Variant[] { "Progress", 0 });
			this.UpdateAmbience();
		}

		// Token: 0x060043A9 RID: 17321 RVA: 0x0020D8CC File Offset: 0x0020BACC
		public void PlayCustomMusic(string customMusic)
		{
			if (NonInteractiveMode.IsActive)
			{
				return;
			}
			this._proxy.Call(NRunMusicController._stopMusic, Array.Empty<Variant>());
			this._proxy.Call("update_music", new Variant[] { customMusic });
		}

		// Token: 0x060043AA RID: 17322 RVA: 0x0020D920 File Offset: 0x0020BB20
		public void UpdateCustomTrack(string customTrack, float label)
		{
			if (NonInteractiveMode.IsActive)
			{
				return;
			}
			if (!RunManager.Instance.IsInProgress)
			{
				return;
			}
			this._proxy.Call("update_custom_track", new Variant[] { customTrack, label });
		}

		// Token: 0x060043AB RID: 17323 RVA: 0x0020D978 File Offset: 0x0020BB78
		public void StopCustomMusic()
		{
			if (NonInteractiveMode.IsActive)
			{
				return;
			}
			this._proxy.Call(NRunMusicController._stopMusic, Array.Empty<Variant>());
			if (this._currentTrack == null)
			{
				return;
			}
			this._proxy.Call("update_music", new Variant[] { this._currentTrack });
			this._proxy.Call("update_global_parameter", new Variant[] { "Progress", 7 });
		}

		// Token: 0x060043AC RID: 17324 RVA: 0x0020DA14 File Offset: 0x0020BC14
		public void UpdateAmbience()
		{
			if (NonInteractiveMode.IsActive)
			{
				return;
			}
			string text = this._runState.Act.AmbientSfx;
			CombatRoom combatRoom = this._runState.CurrentRoom as CombatRoom;
			EncounterModel encounterModel = ((combatRoom != null) ? combatRoom.Encounter : null);
			if (encounterModel != null && encounterModel.HasAmbientSfx)
			{
				text = encounterModel.AmbientSfx;
			}
			if (this._currentAmbience != text)
			{
				this._currentAmbience = text;
				this._proxy.Call("update_ambience", new Variant[] { this._currentAmbience });
			}
		}

		// Token: 0x060043AD RID: 17325 RVA: 0x0020DAAC File Offset: 0x0020BCAC
		public void UpdateTrack()
		{
			if (NonInteractiveMode.IsActive)
			{
				return;
			}
			NRunMusicController.MusicProgressTrack track = this.GetTrack(this._runState.CurrentRoom.RoomType);
			this.UpdateTrack("Progress", (float)track);
			if (this._runState.CurrentRoom is RestSiteRoom)
			{
				this._proxy.Call("update_campfire_ambience", new Variant[] { 0 });
			}
		}

		// Token: 0x060043AE RID: 17326 RVA: 0x0020DB20 File Offset: 0x0020BD20
		private void UpdateTrack(string label, float trackIndex)
		{
			this._proxy.Call("update_global_parameter", new Variant[] { label, trackIndex });
		}

		// Token: 0x060043AF RID: 17327 RVA: 0x0020DB58 File Offset: 0x0020BD58
		public void UpdateMusicParameter(string label, float trackIndex)
		{
			if (NonInteractiveMode.IsActive)
			{
				return;
			}
			this._proxy.Call("update_music_parameter", new Variant[] { label, trackIndex });
		}

		// Token: 0x060043B0 RID: 17328 RVA: 0x0020DB98 File Offset: 0x0020BD98
		public void ToggleMerchantTrack()
		{
			if (NonInteractiveMode.IsActive)
			{
				return;
			}
			if (this._runState.CurrentRoom == null)
			{
				return;
			}
			if (this._runState.CurrentRoom.RoomType != RoomType.Shop)
			{
				throw new InvalidOperationException("You can only trigger the merchant transition in a merchant room");
			}
			NMapScreen instance = NMapScreen.Instance;
			NRunMusicController.MusicProgressTrack musicProgressTrack = ((instance != null && instance.IsVisible()) ? NRunMusicController.MusicProgressTrack.MerchantEnd : NRunMusicController.MusicProgressTrack.Merchant);
			this._proxy.Call("update_global_parameter", new Variant[]
			{
				"Progress",
				(int)musicProgressTrack
			});
		}

		// Token: 0x060043B1 RID: 17329 RVA: 0x0020DC2C File Offset: 0x0020BE2C
		public void TriggerEliteSecondPhase()
		{
			if (NonInteractiveMode.IsActive)
			{
				return;
			}
			if (this._runState.CurrentRoom.RoomType != RoomType.Elite)
			{
				throw new InvalidOperationException("You can only trigger the elite transition in an elite room");
			}
			this._proxy.Call("update_global_parameter", new Variant[] { "Progress", 8 });
		}

		// Token: 0x060043B2 RID: 17330 RVA: 0x0020DC9C File Offset: 0x0020BE9C
		public void TriggerCampfireGoingOut()
		{
			if (NonInteractiveMode.IsActive)
			{
				return;
			}
			if (this._runState.CurrentRoom.RoomType != RoomType.RestSite)
			{
				throw new InvalidOperationException("You can only trigger the rest site transition in a rest site room");
			}
			this._proxy.Call("update_campfire_ambience", new Variant[] { 1 });
		}

		// Token: 0x060043B3 RID: 17331 RVA: 0x0020DCF8 File Offset: 0x0020BEF8
		public void StopMusic()
		{
			if (NonInteractiveMode.IsActive)
			{
				return;
			}
			this._proxy.Call("update_global_parameter", new Variant[] { "Progress", 0 });
			this._proxy.Call(NRunMusicController._stopMusic, Array.Empty<Variant>());
			this._proxy.Call(NRunMusicController._stopAmbience, Array.Empty<Variant>());
			this._currentTrack = null;
			this._failedTrack = null;
			this.UnloadActBanks();
		}

		// Token: 0x060043B4 RID: 17332 RVA: 0x0020DD88 File Offset: 0x0020BF88
		private bool LoadActBank(string bankPath, string verifyEvent)
		{
			return ActBankLoadRetry.Run(bankPath, () => this._proxy.Call("load_act_bank", new Variant[] { bankPath, verifyEvent }).AsBool(), null, null);
		}

		// Token: 0x060043B5 RID: 17333 RVA: 0x0020DDC9 File Offset: 0x0020BFC9
		private void UnloadActBanks()
		{
			this._proxy.Call("unload_act_banks", Array.Empty<Variant>());
		}

		/// <summary>
		/// Get the method information for all the methods declared in this class.
		/// This method is used by Godot to register the available methods in the editor.
		/// Do not call this method.
		/// </summary>
		// Token: 0x060043B6 RID: 17334 RVA: 0x0020DDE8 File Offset: 0x0020BFE8
		[NullableContext(0)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal static List<MethodInfo> GetGodotMethodList()
		{
			return new List<MethodInfo>(17)
			{
				new MethodInfo(NRunMusicController.MethodName.GetTrack, new PropertyInfo(2L, "", 0L, "", 6L, false), 1L, new List<PropertyInfo>
				{
					new PropertyInfo(2L, "roomType", 0L, "", 6L, false)
				}, null),
				new MethodInfo(NRunMusicController.MethodName._Ready, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NRunMusicController.MethodName._ExitTree, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NRunMusicController.MethodName.UpdateMusic, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NRunMusicController.MethodName.PlayCustomMusic, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, new List<PropertyInfo>
				{
					new PropertyInfo(4L, "customMusic", 0L, "", 6L, false)
				}, null),
				new MethodInfo(NRunMusicController.MethodName.UpdateCustomTrack, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, new List<PropertyInfo>
				{
					new PropertyInfo(4L, "customTrack", 0L, "", 6L, false),
					new PropertyInfo(3L, "label", 0L, "", 6L, false)
				}, null),
				new MethodInfo(NRunMusicController.MethodName.StopCustomMusic, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NRunMusicController.MethodName.UpdateAmbience, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NRunMusicController.MethodName.UpdateTrack, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NRunMusicController.MethodName.UpdateTrack, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, new List<PropertyInfo>
				{
					new PropertyInfo(4L, "label", 0L, "", 6L, false),
					new PropertyInfo(3L, "trackIndex", 0L, "", 6L, false)
				}, null),
				new MethodInfo(NRunMusicController.MethodName.UpdateMusicParameter, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, new List<PropertyInfo>
				{
					new PropertyInfo(4L, "label", 0L, "", 6L, false),
					new PropertyInfo(3L, "trackIndex", 0L, "", 6L, false)
				}, null),
				new MethodInfo(NRunMusicController.MethodName.ToggleMerchantTrack, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NRunMusicController.MethodName.TriggerEliteSecondPhase, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NRunMusicController.MethodName.TriggerCampfireGoingOut, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NRunMusicController.MethodName.StopMusic, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NRunMusicController.MethodName.LoadActBank, new PropertyInfo(1L, "", 0L, "", 6L, false), 1L, new List<PropertyInfo>
				{
					new PropertyInfo(4L, "bankPath", 0L, "", 6L, false),
					new PropertyInfo(4L, "verifyEvent", 0L, "", 6L, false)
				}, null),
				new MethodInfo(NRunMusicController.MethodName.UnloadActBanks, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null)
			};
		}

		/// <inheritdoc />
		// Token: 0x060043B7 RID: 17335 RVA: 0x0020E280 File Offset: 0x0020C480
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool InvokeGodotClassMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
		{
			if ((ref method) == NRunMusicController.MethodName.GetTrack && args.Count == 1)
			{
				NRunMusicController.MusicProgressTrack track = this.GetTrack(VariantUtils.ConvertTo<RoomType>(args[0]));
				ret = VariantUtils.CreateFrom<NRunMusicController.MusicProgressTrack>(ref track);
				return true;
			}
			if ((ref method) == NRunMusicController.MethodName._Ready && args.Count == 0)
			{
				this._Ready();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NRunMusicController.MethodName._ExitTree && args.Count == 0)
			{
				this._ExitTree();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NRunMusicController.MethodName.UpdateMusic && args.Count == 0)
			{
				this.UpdateMusic();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NRunMusicController.MethodName.PlayCustomMusic && args.Count == 1)
			{
				this.PlayCustomMusic(VariantUtils.ConvertTo<string>(args[0]));
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NRunMusicController.MethodName.UpdateCustomTrack && args.Count == 2)
			{
				this.UpdateCustomTrack(VariantUtils.ConvertTo<string>(args[0]), VariantUtils.ConvertTo<float>(args[1]));
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NRunMusicController.MethodName.StopCustomMusic && args.Count == 0)
			{
				this.StopCustomMusic();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NRunMusicController.MethodName.UpdateAmbience && args.Count == 0)
			{
				this.UpdateAmbience();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NRunMusicController.MethodName.UpdateTrack && args.Count == 0)
			{
				this.UpdateTrack();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NRunMusicController.MethodName.UpdateTrack && args.Count == 2)
			{
				this.UpdateTrack(VariantUtils.ConvertTo<string>(args[0]), VariantUtils.ConvertTo<float>(args[1]));
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NRunMusicController.MethodName.UpdateMusicParameter && args.Count == 2)
			{
				this.UpdateMusicParameter(VariantUtils.ConvertTo<string>(args[0]), VariantUtils.ConvertTo<float>(args[1]));
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NRunMusicController.MethodName.ToggleMerchantTrack && args.Count == 0)
			{
				this.ToggleMerchantTrack();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NRunMusicController.MethodName.TriggerEliteSecondPhase && args.Count == 0)
			{
				this.TriggerEliteSecondPhase();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NRunMusicController.MethodName.TriggerCampfireGoingOut && args.Count == 0)
			{
				this.TriggerCampfireGoingOut();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NRunMusicController.MethodName.StopMusic && args.Count == 0)
			{
				this.StopMusic();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NRunMusicController.MethodName.LoadActBank && args.Count == 2)
			{
				bool flag = this.LoadActBank(VariantUtils.ConvertTo<string>(args[0]), VariantUtils.ConvertTo<string>(args[1]));
				ret = VariantUtils.CreateFrom<bool>(ref flag);
				return true;
			}
			if ((ref method) == NRunMusicController.MethodName.UnloadActBanks && args.Count == 0)
			{
				this.UnloadActBanks();
				ret = default(godot_variant);
				return true;
			}
			return base.InvokeGodotClassMethod(ref method, args, ref ret);
		}

		/// <inheritdoc />
		// Token: 0x060043B8 RID: 17336 RVA: 0x0020E5A4 File Offset: 0x0020C7A4
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool HasGodotClassMethod(in godot_string_name method)
		{
			return (ref method) == NRunMusicController.MethodName.GetTrack || (ref method) == NRunMusicController.MethodName._Ready || (ref method) == NRunMusicController.MethodName._ExitTree || (ref method) == NRunMusicController.MethodName.UpdateMusic || (ref method) == NRunMusicController.MethodName.PlayCustomMusic || (ref method) == NRunMusicController.MethodName.UpdateCustomTrack || (ref method) == NRunMusicController.MethodName.StopCustomMusic || (ref method) == NRunMusicController.MethodName.UpdateAmbience || (ref method) == NRunMusicController.MethodName.UpdateTrack || (ref method) == NRunMusicController.MethodName.UpdateMusicParameter || (ref method) == NRunMusicController.MethodName.ToggleMerchantTrack || (ref method) == NRunMusicController.MethodName.TriggerEliteSecondPhase || (ref method) == NRunMusicController.MethodName.TriggerCampfireGoingOut || (ref method) == NRunMusicController.MethodName.StopMusic || (ref method) == NRunMusicController.MethodName.LoadActBank || (ref method) == NRunMusicController.MethodName.UnloadActBanks || base.HasGodotClassMethod(ref method);
		}

		/// <inheritdoc />
		// Token: 0x060043B9 RID: 17337 RVA: 0x0020E6A8 File Offset: 0x0020C8A8
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool SetGodotClassPropertyValue(in godot_string_name name, in godot_variant value)
		{
			if ((ref name) == NRunMusicController.PropertyName._proxy)
			{
				this._proxy = VariantUtils.ConvertTo<Node>(ref value);
				return true;
			}
			if ((ref name) == NRunMusicController.PropertyName._currentTrack)
			{
				this._currentTrack = VariantUtils.ConvertTo<string>(ref value);
				return true;
			}
			if ((ref name) == NRunMusicController.PropertyName._currentAmbience)
			{
				this._currentAmbience = VariantUtils.ConvertTo<string>(ref value);
				return true;
			}
			if ((ref name) == NRunMusicController.PropertyName._failedTrack)
			{
				this._failedTrack = VariantUtils.ConvertTo<string>(ref value);
				return true;
			}
			return base.SetGodotClassPropertyValue(ref name, ref value);
		}

		/// <inheritdoc />
		// Token: 0x060043BA RID: 17338 RVA: 0x0020E72C File Offset: 0x0020C92C
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
		{
			if ((ref name) == NRunMusicController.PropertyName._proxy)
			{
				value = VariantUtils.CreateFrom<Node>(ref this._proxy);
				return true;
			}
			if ((ref name) == NRunMusicController.PropertyName._currentTrack)
			{
				value = VariantUtils.CreateFrom<string>(ref this._currentTrack);
				return true;
			}
			if ((ref name) == NRunMusicController.PropertyName._currentAmbience)
			{
				value = VariantUtils.CreateFrom<string>(ref this._currentAmbience);
				return true;
			}
			if ((ref name) == NRunMusicController.PropertyName._failedTrack)
			{
				value = VariantUtils.CreateFrom<string>(ref this._failedTrack);
				return true;
			}
			return base.GetGodotClassPropertyValue(ref name, ref value);
		}

		/// <summary>
		/// Get the property information for all the properties declared in this class.
		/// This method is used by Godot to register the available properties in the editor.
		/// Do not call this method.
		/// </summary>
		// Token: 0x060043BB RID: 17339 RVA: 0x0020E7C4 File Offset: 0x0020C9C4
		[NullableContext(0)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal static List<PropertyInfo> GetGodotPropertyList()
		{
			return new List<PropertyInfo>
			{
				new PropertyInfo(24L, NRunMusicController.PropertyName._proxy, 0L, "", 4096L, false),
				new PropertyInfo(4L, NRunMusicController.PropertyName._currentTrack, 0L, "", 4096L, false),
				new PropertyInfo(4L, NRunMusicController.PropertyName._currentAmbience, 0L, "", 4096L, false),
				new PropertyInfo(4L, NRunMusicController.PropertyName._failedTrack, 0L, "", 4096L, false)
			};
		}

		/// <inheritdoc />
		// Token: 0x060043BC RID: 17340 RVA: 0x0020E85C File Offset: 0x0020CA5C
		[NullableContext(0)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void SaveGodotObjectData(GodotSerializationInfo info)
		{
			base.SaveGodotObjectData(info);
			info.AddProperty(NRunMusicController.PropertyName._proxy, Variant.From<Node>(ref this._proxy));
			info.AddProperty(NRunMusicController.PropertyName._currentTrack, Variant.From<string>(ref this._currentTrack));
			info.AddProperty(NRunMusicController.PropertyName._currentAmbience, Variant.From<string>(ref this._currentAmbience));
			info.AddProperty(NRunMusicController.PropertyName._failedTrack, Variant.From<string>(ref this._failedTrack));
		}

		/// <inheritdoc />
		// Token: 0x060043BD RID: 17341 RVA: 0x0020E8C8 File Offset: 0x0020CAC8
		[NullableContext(0)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void RestoreGodotObjectData(GodotSerializationInfo info)
		{
			base.RestoreGodotObjectData(info);
			Variant variant;
			if (info.TryGetProperty(NRunMusicController.PropertyName._proxy, ref variant))
			{
				this._proxy = variant.As<Node>();
			}
			Variant variant2;
			if (info.TryGetProperty(NRunMusicController.PropertyName._currentTrack, ref variant2))
			{
				this._currentTrack = variant2.As<string>();
			}
			Variant variant3;
			if (info.TryGetProperty(NRunMusicController.PropertyName._currentAmbience, ref variant3))
			{
				this._currentAmbience = variant3.As<string>();
			}
			Variant variant4;
			if (info.TryGetProperty(NRunMusicController.PropertyName._failedTrack, ref variant4))
			{
				this._failedTrack = variant4.As<string>();
			}
		}

		// Token: 0x04001A43 RID: 6723
		private static readonly StringName _stopAmbience = new StringName("stop_ambience");

		// Token: 0x04001A44 RID: 6724
		private static readonly StringName _stopMusic = new StringName("stop_music");

		// Token: 0x04001A45 RID: 6725
		private const string _musicProgressParameter = "Progress";

		// Token: 0x04001A46 RID: 6726
		private const string _updateGlobalParameterCallback = "update_global_parameter";

		// Token: 0x04001A47 RID: 6727
		private const string _updateMusicParameterCallback = "update_music_parameter";

		// Token: 0x04001A48 RID: 6728
		private const string _updateMusicCallback = "update_music";

		// Token: 0x04001A49 RID: 6729
		private const string _updateAmbienceCallback = "update_ambience";

		// Token: 0x04001A4A RID: 6730
		private const string _updateCampfireAmbienceCallback = "update_campfire_ambience";

		// Token: 0x04001A4B RID: 6731
		private const string _updateCustomTrack = "update_custom_track";

		// Token: 0x04001A4C RID: 6732
		private const string _loadActBankCallback = "load_act_bank";

		// Token: 0x04001A4D RID: 6733
		private const string _unloadActBanksCallback = "unload_act_banks";

		// Token: 0x04001A4E RID: 6734
		private const string _bgMusicRngName = "bg_music";

		// Token: 0x04001A4F RID: 6735
		private IRunState _runState = NullRunState.Instance;

		// Token: 0x04001A50 RID: 6736
		private Node _proxy;

		// Token: 0x04001A51 RID: 6737
		[Nullable(2)]
		private string _currentTrack;

		// Token: 0x04001A52 RID: 6738
		private string _currentAmbience;

		// Token: 0x04001A53 RID: 6739
		[Nullable(2)]
		private string _failedTrack;

		// Token: 0x0200197B RID: 6523
		[NullableContext(0)]
		private enum MusicProgressTrack
		{
			// Token: 0x040064B1 RID: 25777
			Init,
			// Token: 0x040064B2 RID: 25778
			Enemy,
			// Token: 0x040064B3 RID: 25779
			Merchant,
			// Token: 0x040064B4 RID: 25780
			Rest,
			// Token: 0x040064B5 RID: 25781
			Unknown,
			// Token: 0x040064B6 RID: 25782
			Treasure,
			// Token: 0x040064B7 RID: 25783
			Elite,
			// Token: 0x040064B8 RID: 25784
			CombatEnd,
			// Token: 0x040064B9 RID: 25785
			Elite2,
			// Token: 0x040064BA RID: 25786
			MerchantEnd
		}

		// Token: 0x0200197C RID: 6524
		[NullableContext(0)]
		private enum CampfireState
		{
			// Token: 0x040064BC RID: 25788
			On,
			// Token: 0x040064BD RID: 25789
			Off
		}

		/// <summary>
		/// A resolved act-music change: the track event to play and the bank that contains it.
		/// </summary>
		// Token: 0x0200197D RID: 6525
		[Nullable(0)]
		public readonly struct MusicSelection : IEquatable<NRunMusicController.MusicSelection>
		{
			/// <summary>
			/// A resolved act-music change: the track event to play and the bank that contains it.
			/// </summary>
			// Token: 0x0600AAE4 RID: 43748 RVA: 0x0035E4E5 File Offset: 0x0035C6E5
			public MusicSelection(string Track, string BankPath)
			{
				this.Track = Track;
				this.BankPath = BankPath;
			}

			// Token: 0x170026BE RID: 9918
			// (get) Token: 0x0600AAE5 RID: 43749 RVA: 0x0035E4F5 File Offset: 0x0035C6F5
			// (set) Token: 0x0600AAE6 RID: 43750 RVA: 0x0035E4FD File Offset: 0x0035C6FD
			public string Track { get; set; }

			// Token: 0x170026BF RID: 9919
			// (get) Token: 0x0600AAE7 RID: 43751 RVA: 0x0035E506 File Offset: 0x0035C706
			// (set) Token: 0x0600AAE8 RID: 43752 RVA: 0x0035E50E File Offset: 0x0035C70E
			public string BankPath { get; set; }

			// Token: 0x0600AAE9 RID: 43753 RVA: 0x0035E518 File Offset: 0x0035C718
			[NullableContext(0)]
			[CompilerGenerated]
			public override string ToString()
			{
				StringBuilder stringBuilder = new StringBuilder();
				stringBuilder.Append("MusicSelection");
				stringBuilder.Append(" { ");
				if (this.PrintMembers(stringBuilder))
				{
					stringBuilder.Append(' ');
				}
				stringBuilder.Append('}');
				return stringBuilder.ToString();
			}

			// Token: 0x0600AAEA RID: 43754 RVA: 0x0035E564 File Offset: 0x0035C764
			[NullableContext(0)]
			[CompilerGenerated]
			private bool PrintMembers(StringBuilder builder)
			{
				builder.Append("Track = ");
				builder.Append(this.Track);
				builder.Append(", BankPath = ");
				builder.Append(this.BankPath);
				return true;
			}

			// Token: 0x0600AAEB RID: 43755 RVA: 0x0035E599 File Offset: 0x0035C799
			[CompilerGenerated]
			public static bool operator !=(NRunMusicController.MusicSelection left, NRunMusicController.MusicSelection right)
			{
				return !(left == right);
			}

			// Token: 0x0600AAEC RID: 43756 RVA: 0x0035E5A5 File Offset: 0x0035C7A5
			[CompilerGenerated]
			public static bool operator ==(NRunMusicController.MusicSelection left, NRunMusicController.MusicSelection right)
			{
				return left.Equals(right);
			}

			// Token: 0x0600AAED RID: 43757 RVA: 0x0035E5AF File Offset: 0x0035C7AF
			[CompilerGenerated]
			public override int GetHashCode()
			{
				return EqualityComparer<string>.Default.GetHashCode(this.<Track>k__BackingField) * -1521134295 + EqualityComparer<string>.Default.GetHashCode(this.<BankPath>k__BackingField);
			}

			// Token: 0x0600AAEE RID: 43758 RVA: 0x0035E5D8 File Offset: 0x0035C7D8
			[NullableContext(0)]
			[CompilerGenerated]
			public override bool Equals(object obj)
			{
				return obj is NRunMusicController.MusicSelection && this.Equals((NRunMusicController.MusicSelection)obj);
			}

			// Token: 0x0600AAEF RID: 43759 RVA: 0x0035E5F0 File Offset: 0x0035C7F0
			[CompilerGenerated]
			public bool Equals(NRunMusicController.MusicSelection other)
			{
				return EqualityComparer<string>.Default.Equals(this.<Track>k__BackingField, other.<Track>k__BackingField) && EqualityComparer<string>.Default.Equals(this.<BankPath>k__BackingField, other.<BankPath>k__BackingField);
			}

			// Token: 0x0600AAF0 RID: 43760 RVA: 0x0035E622 File Offset: 0x0035C822
			[CompilerGenerated]
			public void Deconstruct(out string Track, out string BankPath)
			{
				Track = this.Track;
				BankPath = this.BankPath;
			}
		}

		/// <summary>
		/// Cached StringNames for the methods contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x0200197E RID: 6526
		[NullableContext(0)]
		public class MethodName : Node.MethodName
		{
			/// <summary>
			/// Cached name for the 'GetTrack' method.
			/// </summary>
			// Token: 0x040064C0 RID: 25792
			public static readonly StringName GetTrack = "GetTrack";

			/// <summary>
			/// Cached name for the '_Ready' method.
			/// </summary>
			// Token: 0x040064C1 RID: 25793
			public static readonly StringName _Ready = "_Ready";

			/// <summary>
			/// Cached name for the '_ExitTree' method.
			/// </summary>
			// Token: 0x040064C2 RID: 25794
			public static readonly StringName _ExitTree = "_ExitTree";

			/// <summary>
			/// Cached name for the 'UpdateMusic' method.
			/// </summary>
			// Token: 0x040064C3 RID: 25795
			public static readonly StringName UpdateMusic = "UpdateMusic";

			/// <summary>
			/// Cached name for the 'PlayCustomMusic' method.
			/// </summary>
			// Token: 0x040064C4 RID: 25796
			public static readonly StringName PlayCustomMusic = "PlayCustomMusic";

			/// <summary>
			/// Cached name for the 'UpdateCustomTrack' method.
			/// </summary>
			// Token: 0x040064C5 RID: 25797
			public static readonly StringName UpdateCustomTrack = "UpdateCustomTrack";

			/// <summary>
			/// Cached name for the 'StopCustomMusic' method.
			/// </summary>
			// Token: 0x040064C6 RID: 25798
			public static readonly StringName StopCustomMusic = "StopCustomMusic";

			/// <summary>
			/// Cached name for the 'UpdateAmbience' method.
			/// </summary>
			// Token: 0x040064C7 RID: 25799
			public static readonly StringName UpdateAmbience = "UpdateAmbience";

			/// <summary>
			/// Cached name for the 'UpdateTrack' method.
			/// </summary>
			// Token: 0x040064C8 RID: 25800
			public static readonly StringName UpdateTrack = "UpdateTrack";

			/// <summary>
			/// Cached name for the 'UpdateMusicParameter' method.
			/// </summary>
			// Token: 0x040064C9 RID: 25801
			public static readonly StringName UpdateMusicParameter = "UpdateMusicParameter";

			/// <summary>
			/// Cached name for the 'ToggleMerchantTrack' method.
			/// </summary>
			// Token: 0x040064CA RID: 25802
			public static readonly StringName ToggleMerchantTrack = "ToggleMerchantTrack";

			/// <summary>
			/// Cached name for the 'TriggerEliteSecondPhase' method.
			/// </summary>
			// Token: 0x040064CB RID: 25803
			public static readonly StringName TriggerEliteSecondPhase = "TriggerEliteSecondPhase";

			/// <summary>
			/// Cached name for the 'TriggerCampfireGoingOut' method.
			/// </summary>
			// Token: 0x040064CC RID: 25804
			public static readonly StringName TriggerCampfireGoingOut = "TriggerCampfireGoingOut";

			/// <summary>
			/// Cached name for the 'StopMusic' method.
			/// </summary>
			// Token: 0x040064CD RID: 25805
			public static readonly StringName StopMusic = "StopMusic";

			/// <summary>
			/// Cached name for the 'LoadActBank' method.
			/// </summary>
			// Token: 0x040064CE RID: 25806
			public static readonly StringName LoadActBank = "LoadActBank";

			/// <summary>
			/// Cached name for the 'UnloadActBanks' method.
			/// </summary>
			// Token: 0x040064CF RID: 25807
			public static readonly StringName UnloadActBanks = "UnloadActBanks";
		}

		/// <summary>
		/// Cached StringNames for the properties and fields contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x0200197F RID: 6527
		[NullableContext(0)]
		public class PropertyName : Node.PropertyName
		{
			/// <summary>
			/// Cached name for the '_proxy' field.
			/// </summary>
			// Token: 0x040064D0 RID: 25808
			public static readonly StringName _proxy = "_proxy";

			/// <summary>
			/// Cached name for the '_currentTrack' field.
			/// </summary>
			// Token: 0x040064D1 RID: 25809
			public static readonly StringName _currentTrack = "_currentTrack";

			/// <summary>
			/// Cached name for the '_currentAmbience' field.
			/// </summary>
			// Token: 0x040064D2 RID: 25810
			public static readonly StringName _currentAmbience = "_currentAmbience";

			/// <summary>
			/// Cached name for the '_failedTrack' field.
			/// </summary>
			// Token: 0x040064D3 RID: 25811
			public static readonly StringName _failedTrack = "_failedTrack";
		}

		/// <summary>
		/// Cached StringNames for the signals contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x02001980 RID: 6528
		[NullableContext(0)]
		public class SignalName : Node.SignalName
		{
		}
	}
}
