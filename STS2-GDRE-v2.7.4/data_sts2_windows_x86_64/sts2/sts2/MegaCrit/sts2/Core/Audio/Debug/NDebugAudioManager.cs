using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using Godot;
using Godot.Bridge;
using Godot.NativeInterop;
using MegaCrit.Sts2.Core.Assets;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.Logging;
using MegaCrit.Sts2.Core.Nodes;
using MegaCrit.Sts2.Core.Random;

namespace MegaCrit.Sts2.Core.Audio.Debug
{
	/// <summary>
	/// Audio manager for use with temporary sound effects (hopefully they make it into FMOD someday).
	/// </summary>
	// Token: 0x02000D8A RID: 3466
	[ScriptPath("res://src/Core/Audio/Debug/NDebugAudioManager.cs")]
	public class NDebugAudioManager : Node
	{
		// Token: 0x17002653 RID: 9811
		// (get) Token: 0x06008B63 RID: 35683 RVA: 0x002E8153 File Offset: 0x002E6353
		[Nullable(2)]
		public static NDebugAudioManager Instance
		{
			[NullableContext(2)]
			get
			{
				NGame instance = NGame.Instance;
				if (instance == null)
				{
					return null;
				}
				return instance.DebugAudio;
			}
		}

		// Token: 0x06008B64 RID: 35684 RVA: 0x002E8165 File Offset: 0x002E6365
		public override void _Ready()
		{
			this._freeAudioPlayers.AddRange(base.GetChildren(false).OfType<AudioStreamPlayer>());
		}

		/// <summary>
		/// Play a sound.
		/// </summary>
		/// <param name="streamName">The name of the audio sample file in res://debug_audio to play. Include the file extension.</param>
		/// <param name="volume">The volume to play the sound at. 0 is muted, 2 is 2x the volume.</param>
		/// <param name="variance">The randomized pitch variance with which to play the sound.</param>
		/// <returns>An integer ID. If this is a one-shot sound, then the value can be ignored. If the sound is looping, or
		///          you wish to stop the sound early, pass this value to Stop to stop the sound from playing.</returns>
		// Token: 0x06008B65 RID: 35685 RVA: 0x002E8180 File Offset: 0x002E6380
		[NullableContext(1)]
		public int Play(string streamName, float volume = 1f, PitchVariance variance = PitchVariance.None)
		{
			NDebugAudioManager.<>c__DisplayClass9_0 CS$<>8__locals1 = new NDebugAudioManager.<>c__DisplayClass9_0();
			CS$<>8__locals1.<>4__this = this;
			if (this._freeAudioPlayers.Count > 0)
			{
				NDebugAudioManager.<>c__DisplayClass9_0 CS$<>8__locals2 = CS$<>8__locals1;
				List<AudioStreamPlayer> freeAudioPlayers = this._freeAudioPlayers;
				CS$<>8__locals2.streamPlayer = freeAudioPlayers[freeAudioPlayers.Count - 1];
				this._freeAudioPlayers.RemoveAt(this._freeAudioPlayers.Count - 1);
			}
			else
			{
				CS$<>8__locals1.streamPlayer = new AudioStreamPlayer();
				this.AddChildSafely(CS$<>8__locals1.streamPlayer);
			}
			AudioStream asset = PreloadManager.Cache.GetAsset<AudioStream>(TmpSfx.GetPath(streamName));
			CS$<>8__locals1.streamPlayer.Name = "StreamPlayer-" + streamName;
			CS$<>8__locals1.streamPlayer.Stream = asset;
			CS$<>8__locals1.streamPlayer.VolumeLinear = volume;
			CS$<>8__locals1.streamPlayer.PitchScale = this.GetRandomPitchScale(variance);
			CS$<>8__locals1.streamPlayer.Bus = NDebugAudioManager._sfx;
			Callable callable = Callable.From(delegate
			{
				CS$<>8__locals1.<>4__this.PlayerFinished(CS$<>8__locals1.streamPlayer);
			});
			CS$<>8__locals1.streamPlayer.Connect(AudioStreamPlayer.SignalName.Finished, callable, 0U);
			CS$<>8__locals1.streamPlayer.Play(0f);
			NDebugAudioManager.PlayingSound playingSound = new NDebugAudioManager.PlayingSound
			{
				id = this._nextId,
				player = CS$<>8__locals1.streamPlayer,
				callable = callable
			};
			this._playingSounds.Add(playingSound);
			this._nextId++;
			return playingSound.id;
		}

		/// <summary>
		/// Stops all currently playing sounds
		/// </summary>
		// Token: 0x06008B66 RID: 35686 RVA: 0x002E82DC File Offset: 0x002E64DC
		public void StopAll()
		{
			List<NDebugAudioManager.PlayingSound> list = this._playingSounds.ToList<NDebugAudioManager.PlayingSound>();
			foreach (NDebugAudioManager.PlayingSound playingSound in list)
			{
				this.Stop(playingSound.id, 0f);
			}
		}

		/// <summary>
		/// Stops a sound.
		/// </summary>
		/// <param name="id">The ID returned from the Play call of the sound you wish to stop.</param>
		/// <param name="fadeTime">Amount of time that the sound will fade to zero before stopping.</param>
		// Token: 0x06008B67 RID: 35687 RVA: 0x002E8340 File Offset: 0x002E6540
		public void Stop(int id, float fadeTime = 0.5f)
		{
			int i = 0;
			Action <>9__0;
			while (i < this._playingSounds.Count)
			{
				NDebugAudioManager.PlayingSound playingSound = this._playingSounds[i];
				if (playingSound.id == id)
				{
					if (fadeTime > 0f)
					{
						Tween tween = base.CreateTween();
						tween.TweenProperty(playingSound.player, "volume_linear", 0f, (double)fadeTime);
						Tween tween2 = tween;
						Action action;
						if ((action = <>9__0) == null)
						{
							action = (<>9__0 = delegate
							{
								this.StopInternalById(id);
							});
						}
						tween2.TweenCallback(Callable.From(action));
						return;
					}
					this.StopInternal(i);
					return;
				}
				else
				{
					i++;
				}
			}
			DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(65, 1);
			defaultInterpolatedStringHandler.AppendLiteral("Tried to stop sound with ID ");
			defaultInterpolatedStringHandler.AppendFormatted<int>(id);
			defaultInterpolatedStringHandler.AppendLiteral(" but no sound with that ID was found!");
			Log.Warn(defaultInterpolatedStringHandler.ToStringAndClear(), 2);
		}

		// Token: 0x06008B68 RID: 35688 RVA: 0x002E843C File Offset: 0x002E663C
		private void StopInternalById(int id)
		{
			for (int i = 0; i < this._playingSounds.Count; i++)
			{
				NDebugAudioManager.PlayingSound playingSound = this._playingSounds[i];
				if (playingSound.id == id)
				{
					this.StopInternal(i);
					return;
				}
			}
		}

		// Token: 0x06008B69 RID: 35689 RVA: 0x002E8480 File Offset: 0x002E6680
		private void StopInternal(int soundIndex)
		{
			NDebugAudioManager.PlayingSound playingSound = this._playingSounds[soundIndex];
			if (playingSound.player.IsPlaying())
			{
				playingSound.player.Stop();
			}
			playingSound.player.Disconnect(AudioStreamPlayer.SignalName.Finished, playingSound.callable);
			this._playingSounds.RemoveAt(soundIndex);
			this._freeAudioPlayers.Add(playingSound.player);
		}

		/// <summary>
		/// Sets the volume of the master bus.
		/// </summary>
		/// <param name="linearVolume">Volume from 0-1.</param>
		// Token: 0x06008B6A RID: 35690 RVA: 0x002E84E5 File Offset: 0x002E66E5
		public void SetMasterAudioVolume(float linearVolume)
		{
			AudioServer.Singleton.SetBusVolumeDb(AudioServer.Singleton.GetBusIndex(NDebugAudioManager._master), Mathf.LinearToDb(Mathf.Pow(linearVolume, 2f)));
		}

		/// <summary>
		/// Sets the volume of the SFX bus.
		/// </summary>
		/// <param name="linearVolume">Volume from 0-1.</param>
		// Token: 0x06008B6B RID: 35691 RVA: 0x002E8510 File Offset: 0x002E6710
		public void SetSfxAudioVolume(float linearVolume)
		{
			AudioServer.Singleton.SetBusVolumeDb(AudioServer.Singleton.GetBusIndex(NDebugAudioManager._sfx), Mathf.LinearToDb(Mathf.Pow(linearVolume, 2f)));
		}

		// Token: 0x06008B6C RID: 35692 RVA: 0x002E853C File Offset: 0x002E673C
		[NullableContext(1)]
		private void PlayerFinished(AudioStreamPlayer player)
		{
			for (int i = 0; i < this._playingSounds.Count; i++)
			{
				NDebugAudioManager.PlayingSound playingSound = this._playingSounds[i];
				if (playingSound.player == player)
				{
					this.StopInternal(i);
					return;
				}
			}
		}

		// Token: 0x06008B6D RID: 35693 RVA: 0x002E8580 File Offset: 0x002E6780
		private float GetRandomPitchScale(PitchVariance variance)
		{
			float num;
			switch (variance)
			{
			case PitchVariance.None:
				num = 0f;
				break;
			case PitchVariance.Small:
				num = 0.02f;
				break;
			case PitchVariance.Medium:
				num = 0.05f;
				break;
			case PitchVariance.Large:
				num = 0.1f;
				break;
			case PitchVariance.TooMuch:
				num = 0.2f;
				break;
			default:
				throw new ArgumentOutOfRangeException("variance", variance, null);
			}
			float num2 = num;
			if (num2 == 0f)
			{
				return 1f;
			}
			return 1f + Rng.Chaotic.NextFloat(-num2, num2);
		}

		/// <summary>
		/// Get the method information for all the methods declared in this class.
		/// This method is used by Godot to register the available methods in the editor.
		/// Do not call this method.
		/// </summary>
		// Token: 0x06008B6E RID: 35694 RVA: 0x002E8608 File Offset: 0x002E6808
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal static List<MethodInfo> GetGodotMethodList()
		{
			return new List<MethodInfo>(10)
			{
				new MethodInfo(NDebugAudioManager.MethodName._Ready, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NDebugAudioManager.MethodName.Play, new PropertyInfo(2L, "", 0L, "", 6L, false), 1L, new List<PropertyInfo>
				{
					new PropertyInfo(4L, "streamName", 0L, "", 6L, false),
					new PropertyInfo(3L, "volume", 0L, "", 6L, false),
					new PropertyInfo(2L, "variance", 0L, "", 6L, false)
				}, null),
				new MethodInfo(NDebugAudioManager.MethodName.StopAll, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NDebugAudioManager.MethodName.Stop, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, new List<PropertyInfo>
				{
					new PropertyInfo(2L, "id", 0L, "", 6L, false),
					new PropertyInfo(3L, "fadeTime", 0L, "", 6L, false)
				}, null),
				new MethodInfo(NDebugAudioManager.MethodName.StopInternalById, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, new List<PropertyInfo>
				{
					new PropertyInfo(2L, "id", 0L, "", 6L, false)
				}, null),
				new MethodInfo(NDebugAudioManager.MethodName.StopInternal, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, new List<PropertyInfo>
				{
					new PropertyInfo(2L, "soundIndex", 0L, "", 6L, false)
				}, null),
				new MethodInfo(NDebugAudioManager.MethodName.SetMasterAudioVolume, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, new List<PropertyInfo>
				{
					new PropertyInfo(3L, "linearVolume", 0L, "", 6L, false)
				}, null),
				new MethodInfo(NDebugAudioManager.MethodName.SetSfxAudioVolume, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, new List<PropertyInfo>
				{
					new PropertyInfo(3L, "linearVolume", 0L, "", 6L, false)
				}, null),
				new MethodInfo(NDebugAudioManager.MethodName.PlayerFinished, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, new List<PropertyInfo>
				{
					new PropertyInfo(24L, "player", 0L, "", 6L, new StringName("AudioStreamPlayer"), false)
				}, null),
				new MethodInfo(NDebugAudioManager.MethodName.GetRandomPitchScale, new PropertyInfo(3L, "", 0L, "", 6L, false), 1L, new List<PropertyInfo>
				{
					new PropertyInfo(2L, "variance", 0L, "", 6L, false)
				}, null)
			};
		}

		/// <inheritdoc />
		// Token: 0x06008B6F RID: 35695 RVA: 0x002E898C File Offset: 0x002E6B8C
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool InvokeGodotClassMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
		{
			if ((ref method) == NDebugAudioManager.MethodName._Ready && args.Count == 0)
			{
				this._Ready();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NDebugAudioManager.MethodName.Play && args.Count == 3)
			{
				int num = this.Play(VariantUtils.ConvertTo<string>(args[0]), VariantUtils.ConvertTo<float>(args[1]), VariantUtils.ConvertTo<PitchVariance>(args[2]));
				ret = VariantUtils.CreateFrom<int>(ref num);
				return true;
			}
			if ((ref method) == NDebugAudioManager.MethodName.StopAll && args.Count == 0)
			{
				this.StopAll();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NDebugAudioManager.MethodName.Stop && args.Count == 2)
			{
				this.Stop(VariantUtils.ConvertTo<int>(args[0]), VariantUtils.ConvertTo<float>(args[1]));
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NDebugAudioManager.MethodName.StopInternalById && args.Count == 1)
			{
				this.StopInternalById(VariantUtils.ConvertTo<int>(args[0]));
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NDebugAudioManager.MethodName.StopInternal && args.Count == 1)
			{
				this.StopInternal(VariantUtils.ConvertTo<int>(args[0]));
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NDebugAudioManager.MethodName.SetMasterAudioVolume && args.Count == 1)
			{
				this.SetMasterAudioVolume(VariantUtils.ConvertTo<float>(args[0]));
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NDebugAudioManager.MethodName.SetSfxAudioVolume && args.Count == 1)
			{
				this.SetSfxAudioVolume(VariantUtils.ConvertTo<float>(args[0]));
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NDebugAudioManager.MethodName.PlayerFinished && args.Count == 1)
			{
				this.PlayerFinished(VariantUtils.ConvertTo<AudioStreamPlayer>(args[0]));
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NDebugAudioManager.MethodName.GetRandomPitchScale && args.Count == 1)
			{
				float randomPitchScale = this.GetRandomPitchScale(VariantUtils.ConvertTo<PitchVariance>(args[0]));
				ret = VariantUtils.CreateFrom<float>(ref randomPitchScale);
				return true;
			}
			return base.InvokeGodotClassMethod(ref method, args, ref ret);
		}

		/// <inheritdoc />
		// Token: 0x06008B70 RID: 35696 RVA: 0x002E8BBC File Offset: 0x002E6DBC
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool HasGodotClassMethod(in godot_string_name method)
		{
			return (ref method) == NDebugAudioManager.MethodName._Ready || (ref method) == NDebugAudioManager.MethodName.Play || (ref method) == NDebugAudioManager.MethodName.StopAll || (ref method) == NDebugAudioManager.MethodName.Stop || (ref method) == NDebugAudioManager.MethodName.StopInternalById || (ref method) == NDebugAudioManager.MethodName.StopInternal || (ref method) == NDebugAudioManager.MethodName.SetMasterAudioVolume || (ref method) == NDebugAudioManager.MethodName.SetSfxAudioVolume || (ref method) == NDebugAudioManager.MethodName.PlayerFinished || (ref method) == NDebugAudioManager.MethodName.GetRandomPitchScale || base.HasGodotClassMethod(ref method);
		}

		/// <inheritdoc />
		// Token: 0x06008B71 RID: 35697 RVA: 0x002E8C66 File Offset: 0x002E6E66
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool SetGodotClassPropertyValue(in godot_string_name name, in godot_variant value)
		{
			if ((ref name) == NDebugAudioManager.PropertyName._nextId)
			{
				this._nextId = VariantUtils.ConvertTo<int>(ref value);
				return true;
			}
			return base.SetGodotClassPropertyValue(ref name, ref value);
		}

		/// <inheritdoc />
		// Token: 0x06008B72 RID: 35698 RVA: 0x002E8C8B File Offset: 0x002E6E8B
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
		{
			if ((ref name) == NDebugAudioManager.PropertyName._nextId)
			{
				value = VariantUtils.CreateFrom<int>(ref this._nextId);
				return true;
			}
			return base.GetGodotClassPropertyValue(ref name, ref value);
		}

		/// <summary>
		/// Get the property information for all the properties declared in this class.
		/// This method is used by Godot to register the available properties in the editor.
		/// Do not call this method.
		/// </summary>
		// Token: 0x06008B73 RID: 35699 RVA: 0x002E8CB8 File Offset: 0x002E6EB8
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal static List<PropertyInfo> GetGodotPropertyList()
		{
			return new List<PropertyInfo>
			{
				new PropertyInfo(2L, NDebugAudioManager.PropertyName._nextId, 0L, "", 4096L, false)
			};
		}

		/// <inheritdoc />
		// Token: 0x06008B74 RID: 35700 RVA: 0x002E8CEC File Offset: 0x002E6EEC
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void SaveGodotObjectData(GodotSerializationInfo info)
		{
			base.SaveGodotObjectData(info);
			info.AddProperty(NDebugAudioManager.PropertyName._nextId, Variant.From<int>(ref this._nextId));
		}

		/// <inheritdoc />
		// Token: 0x06008B75 RID: 35701 RVA: 0x002E8D0C File Offset: 0x002E6F0C
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void RestoreGodotObjectData(GodotSerializationInfo info)
		{
			base.RestoreGodotObjectData(info);
			Variant variant;
			if (info.TryGetProperty(NDebugAudioManager.PropertyName._nextId, ref variant))
			{
				this._nextId = variant.As<int>();
			}
		}

		// Token: 0x0400307E RID: 12414
		[Nullable(1)]
		private static readonly StringName _sfx = new StringName("SFX");

		// Token: 0x0400307F RID: 12415
		[Nullable(1)]
		private static readonly StringName _master = new StringName("Master");

		// Token: 0x04003080 RID: 12416
		[Nullable(1)]
		private List<AudioStreamPlayer> _freeAudioPlayers = new List<AudioStreamPlayer>();

		// Token: 0x04003081 RID: 12417
		[Nullable(1)]
		private readonly List<NDebugAudioManager.PlayingSound> _playingSounds = new List<NDebugAudioManager.PlayingSound>();

		// Token: 0x04003082 RID: 12418
		private int _nextId;

		// Token: 0x02002557 RID: 9559
		private struct PlayingSound
		{
			// Token: 0x04009FD2 RID: 40914
			public int id;

			// Token: 0x04009FD3 RID: 40915
			[Nullable(1)]
			public AudioStreamPlayer player;

			// Token: 0x04009FD4 RID: 40916
			public Callable callable;
		}

		/// <summary>
		/// Cached StringNames for the methods contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x02002558 RID: 9560
		public class MethodName : Node.MethodName
		{
			/// <summary>
			/// Cached name for the '_Ready' method.
			/// </summary>
			// Token: 0x04009FD5 RID: 40917
			public static readonly StringName _Ready = "_Ready";

			/// <summary>
			/// Cached name for the 'Play' method.
			/// </summary>
			// Token: 0x04009FD6 RID: 40918
			public static readonly StringName Play = "Play";

			/// <summary>
			/// Cached name for the 'StopAll' method.
			/// </summary>
			// Token: 0x04009FD7 RID: 40919
			public static readonly StringName StopAll = "StopAll";

			/// <summary>
			/// Cached name for the 'Stop' method.
			/// </summary>
			// Token: 0x04009FD8 RID: 40920
			public static readonly StringName Stop = "Stop";

			/// <summary>
			/// Cached name for the 'StopInternalById' method.
			/// </summary>
			// Token: 0x04009FD9 RID: 40921
			public static readonly StringName StopInternalById = "StopInternalById";

			/// <summary>
			/// Cached name for the 'StopInternal' method.
			/// </summary>
			// Token: 0x04009FDA RID: 40922
			public static readonly StringName StopInternal = "StopInternal";

			/// <summary>
			/// Cached name for the 'SetMasterAudioVolume' method.
			/// </summary>
			// Token: 0x04009FDB RID: 40923
			public static readonly StringName SetMasterAudioVolume = "SetMasterAudioVolume";

			/// <summary>
			/// Cached name for the 'SetSfxAudioVolume' method.
			/// </summary>
			// Token: 0x04009FDC RID: 40924
			public static readonly StringName SetSfxAudioVolume = "SetSfxAudioVolume";

			/// <summary>
			/// Cached name for the 'PlayerFinished' method.
			/// </summary>
			// Token: 0x04009FDD RID: 40925
			public static readonly StringName PlayerFinished = "PlayerFinished";

			/// <summary>
			/// Cached name for the 'GetRandomPitchScale' method.
			/// </summary>
			// Token: 0x04009FDE RID: 40926
			public static readonly StringName GetRandomPitchScale = "GetRandomPitchScale";
		}

		/// <summary>
		/// Cached StringNames for the properties and fields contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x02002559 RID: 9561
		public class PropertyName : Node.PropertyName
		{
			/// <summary>
			/// Cached name for the '_nextId' field.
			/// </summary>
			// Token: 0x04009FDF RID: 40927
			public static readonly StringName _nextId = "_nextId";
		}

		/// <summary>
		/// Cached StringNames for the signals contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x0200255A RID: 9562
		public class SignalName : Node.SignalName
		{
		}
	}
}
