using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using Godot;
using MegaCrit.Sts2.Core.AutoSlay;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.Localization;
using MegaCrit.Sts2.Core.Logging;
using MegaCrit.Sts2.Core.Modding;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Nodes;
using MegaCrit.Sts2.Core.Platform.Steam;
using MegaCrit.Sts2.Core.Rooms;
using MegaCrit.Sts2.Core.Runs;
using MegaCrit.Sts2.Core.Saves;
using Sentry;

namespace MegaCrit.Sts2.Core.Debug
{
	/// <summary>
	/// Manages Sentry .NET SDK initialization and error reporting for C# exceptions.
	/// This complements the GDExtension SDK which handles native crashes.
	/// </summary>
	// Token: 0x02000D18 RID: 3352
	[NullableContext(1)]
	[Nullable(0)]
	public static class SentryService
	{
		// Token: 0x1700253E RID: 9534
		// (get) Token: 0x06008736 RID: 34614 RVA: 0x002D77E0 File Offset: 0x002D59E0
		// (set) Token: 0x06008737 RID: 34615 RVA: 0x002D77E7 File Offset: 0x002D59E7
		public static bool IsEnabled { get; private set; }

		// Token: 0x1700253F RID: 9535
		// (get) Token: 0x06008738 RID: 34616 RVA: 0x002D77EF File Offset: 0x002D59EF
		// (set) Token: 0x06008739 RID: 34617 RVA: 0x002D77F6 File Offset: 0x002D59F6
		public static bool SampleForNonSteamBranches { get; private set; }

		// Token: 0x17002540 RID: 9536
		// (get) Token: 0x0600873A RID: 34618 RVA: 0x002D77FE File Offset: 0x002D59FE
		// (set) Token: 0x0600873B RID: 34619 RVA: 0x002D7805 File Offset: 0x002D5A05
		public static bool IsForcedOn { get; private set; }

		// Token: 0x17002541 RID: 9537
		// (get) Token: 0x0600873C RID: 34620 RVA: 0x002D780D File Offset: 0x002D5A0D
		public static string SessionId
		{
			get
			{
				return SentryService._sessionId;
			}
		}

		/// <summary>
		/// Suppresses all Sentry event capture when mods are detected. Called right after
		/// ModManager.Initialize so mod errors during the rest of startup are never reported,
		/// before AfterGameInit shuts things down.
		///
		/// Both SDKs get the same treatment. The native GDExtension SDK is set to reject every event
		/// rather than closed, since it stays running until the app shuts down. The managed SDK is
		/// suppressed with a flag read as the first line of FilterEvent. That flag is a plain bool, so
		/// unlike the sampler it cannot throw during early startup and fail open (Sentry sends the
		/// event when BeforeSend throws).
		/// </summary>
		// Token: 0x0600873D RID: 34621 RVA: 0x002D7814 File Offset: 0x002D5A14
		public static void DisableSentryIfModded()
		{
			if (!ModManager.IsRunningModded())
			{
				return;
			}
			SentryService._suppressAllEvents = true;
			SceneTree sceneTree = Engine.GetMainLoop() as SceneTree;
			Node node = ((sceneTree != null) ? sceneTree.Root.GetNodeOrNull("SentryInit") : null);
			if (node != null)
			{
				GodotObject godotObject = node;
				StringName setShouldSampleEventMethod = SentryService._setShouldSampleEventMethod;
				Variant[] array = new Variant[1];
				int num = 0;
				Func<bool> func;
				if ((func = SentryService.<>O.<0>__AlwaysRejectEvent) == null)
				{
					func = (SentryService.<>O.<0>__AlwaysRejectEvent = new Func<bool>(SentryService.AlwaysRejectEvent));
				}
				array[num] = Callable.From<bool>(func);
				godotObject.Call(setShouldSampleEventMethod, array);
			}
		}

		// Token: 0x0600873E RID: 34622 RVA: 0x002D7899 File Offset: 0x002D5A99
		private static bool AlwaysRejectEvent()
		{
			return false;
		}

		/// <summary>
		/// Initializes the Sentry .NET SDK. Should be called early in game startup.
		/// Disabled in editor (unless headless), uses "playtesters" environment with release_info, "development" without.
		/// </summary>
		// Token: 0x0600873F RID: 34623 RVA: 0x002D789C File Offset: 0x002D5A9C
		public static void Initialize()
		{
			SentryService.<>c__DisplayClass37_0 CS$<>8__locals1 = new SentryService.<>c__DisplayClass37_0();
			bool flag = OS.HasFeature("editor");
			bool flag2 = DisplayServer.GetName().Equals("headless", StringComparison.OrdinalIgnoreCase);
			CS$<>8__locals1.isForcedOn = CommandLineHelper.HasArg("force-sentry");
			if (flag && !flag2 && !CS$<>8__locals1.isForcedOn)
			{
				Log.Info("[Sentry.NET] Disabled in editor", 2);
				return;
			}
			SentryService.SampleForNonSteamBranches = flag2 | CS$<>8__locals1.isForcedOn;
			SentryService.IsForcedOn = CS$<>8__locals1.isForcedOn;
			CS$<>8__locals1.dsn = SentryService.GetDsn();
			if (string.IsNullOrEmpty(CS$<>8__locals1.dsn))
			{
				Log.Info("[Sentry.NET] Disabled: no DSN configured in project settings", 2);
				return;
			}
			CS$<>8__locals1.releaseInfo = ReleaseInfoManager.Instance.ReleaseInfo;
			CS$<>8__locals1.environment = "unknown";
			SentryService.<>c__DisplayClass37_0 CS$<>8__locals2 = CS$<>8__locals1;
			ReleaseInfo releaseInfo = CS$<>8__locals1.releaseInfo;
			CS$<>8__locals2.release = ((releaseInfo != null) ? releaseInfo.Version : null) ?? "dev";
			SentryService._sentryInstance = SentrySdk.Init(delegate(SentryOptions options)
			{
				options.Dsn = CS$<>8__locals1.dsn;
				options.Environment = CS$<>8__locals1.environment;
				options.Release = CS$<>8__locals1.release;
				options.Debug = CS$<>8__locals1.isForcedOn;
				options.AutoSessionTracking = true;
				options.IsGlobalModeEnabled = true;
				options.SendDefaultPii = false;
				options.SetBeforeSend((SentryEvent sentryEvent, SentryHint hint) => SentryService.FilterEvent(sentryEvent));
			});
			SentryService.IsEnabled = SentrySdk.IsEnabled;
			if (!SentryService.IsEnabled)
			{
				Log.Warn("[Sentry.NET] SDK initialization failed", 2);
				return;
			}
			SentrySdk.ConfigureScope(delegate(Scope scope)
			{
				scope.SetTag("sdk", "dotnet");
				scope.SetTag("session_id", SentryService._sessionId);
				scope.SetExtra("assembly.main_hash", AssemblyHasher.GetMainAssemblyHash());
				if (CS$<>8__locals1.releaseInfo != null)
				{
					scope.SetTag("branch", CS$<>8__locals1.releaseInfo.Branch);
					scope.SetExtra("build.commit", CS$<>8__locals1.releaseInfo.Commit);
					scope.SetExtra("build.main_hash", CS$<>8__locals1.releaseInfo.MainAssemblyHash);
					scope.SetExtra("build.date", CS$<>8__locals1.releaseInfo.Date.ToString("o"));
				}
			});
			Action<LogLevel, string, int> action;
			if ((action = SentryService.<>O.<1>__OnLogCallback) == null)
			{
				action = (SentryService.<>O.<1>__OnLogCallback = new Action<LogLevel, string, int>(SentryService.OnLogCallback));
			}
			Log.LogCallback += action;
			Log.Info("[Sentry.NET] Initialized: env=" + CS$<>8__locals1.environment + ", release=" + CS$<>8__locals1.release, 2);
		}

		// Token: 0x06008740 RID: 34624 RVA: 0x002D79F4 File Offset: 0x002D5BF4
		public static void AfterGameInit([Nullable(2)] string platformBranch, string uniqueId, Node treeRoot)
		{
			if (!SentryService.IsEnabled)
			{
				return;
			}
			SentryService._sentryInit = treeRoot.GetNode("SentryInit");
			Node sentryInit = SentryService._sentryInit;
			if (sentryInit != null)
			{
				sentryInit.Call(SentryService._setCsharpContextMethod, new Variant[]
				{
					SentryService._sessionId,
					AssemblyHasher.GetMainAssemblyHash()
				});
			}
			if (!SentryService.ShouldStayAliveAfterInit(true))
			{
				Log.Info("[Sentry.NET] Shutting down because event reporting is disabled.", 2);
				SentryService.Shutdown();
			}
			else
			{
				SentrySdk.ConfigureScope(delegate(Scope scope)
				{
					scope.User = new SentryUser
					{
						Id = uniqueId
					};
				});
				SentryService.SetGdExtensionUser(uniqueId);
				Log.Debug("[Sentry.NET] User context set", 2);
				SentryService.SetPlatformBranch(platformBranch);
			}
			SentryService._isGameInitialized = true;
		}

		// Token: 0x06008741 RID: 34625 RVA: 0x002D7AB8 File Offset: 0x002D5CB8
		private static void OnLogCallback(LogLevel level, string message, int skipFrames)
		{
			if (!SentryService.IsEnabled)
			{
				return;
			}
			if (level != LogLevel.Warn)
			{
				if (level == LogLevel.Error)
				{
					SentrySdk.AddBreadcrumb(message, "log", null, null, 2);
					SentryService.SetGdExtensionBreadcrumb(message, "log", 2);
					return;
				}
			}
			else
			{
				SentrySdk.AddBreadcrumb(message, "log", null, null, 1);
				SentryService.SetGdExtensionBreadcrumb(message, "log", 1);
			}
		}

		// Token: 0x06008742 RID: 34626 RVA: 0x002D7B0C File Offset: 0x002D5D0C
		private static void SetGdExtensionUser(string uniqueId)
		{
			try
			{
				if (Engine.HasSingleton(SentryService._sentrySdkSingleton))
				{
					if (SentryService._extensionSdk == null)
					{
						SentryService._extensionSdk = Engine.GetSingleton(SentryService._sentrySdkSingleton);
					}
					GodotObject godotObject = ClassDB.Instantiate(SentryService._sentryUserClass).AsGodotObject();
					godotObject.Set(SentryService._idProperty, uniqueId);
					SentryService._extensionSdk.Call(SentryService._setUserMethod, new Variant[] { godotObject });
				}
			}
			catch (Exception ex)
			{
				Log.Warn("[Sentry] Failed to set GDExtension user: " + ex.Message, 2);
			}
		}

		// Token: 0x06008743 RID: 34627 RVA: 0x002D7BB0 File Offset: 0x002D5DB0
		private static void SetGdExtensionBreadcrumb(string message, string category, BreadcrumbLevel level)
		{
			try
			{
				if (Engine.HasSingleton(SentryService._sentrySdkSingleton))
				{
					if (SentryService._extensionSdk == null)
					{
						SentryService._extensionSdk = Engine.GetSingleton(SentryService._sentrySdkSingleton);
					}
					GodotObject godotObject = ClassDB.ClassCallStatic(SentryService._sentryBreadcrumbClass, SentryService._createMethod, new Variant[] { message }).AsGodotObject();
					godotObject.Set(SentryService._categoryProperty, category);
					godotObject.Set(SentryService._levelProperty, level + 1);
					SentryService._extensionSdk.Call(SentryService._addBreadcrumbMethod, new Variant[] { godotObject });
				}
			}
			catch (Exception ex)
			{
				Log.Warn("[Sentry] Failed to set GDExtension breadcrumb: " + ex.Message, 2);
			}
		}

		/// <summary>
		/// Configures sampling rate based on the Steam branch. Call after Steam initializes.
		/// Shuts down Sentry entirely for non-Steam builds (null branch).
		/// </summary>
		// Token: 0x06008744 RID: 34628 RVA: 0x002D7C84 File Offset: 0x002D5E84
		[NullableContext(2)]
		private static void SetPlatformBranch(string branch)
		{
			string branch2 = branch;
			float num;
			if (!(branch2 == "public"))
			{
				if (!(branch2 == "private-beta"))
				{
					if (!(branch2 == "public-beta"))
					{
						if (branch != null)
						{
							num = 0.1f;
						}
						else
						{
							num = (SentryService.SampleForNonSteamBranches ? 1f : 0f);
						}
					}
					else
					{
						num = 0.2f;
					}
				}
				else
				{
					num = 1f;
				}
			}
			else
			{
				num = 0.1f;
			}
			SentryService._sampleRate = num;
			Node sentryInit = SentryService._sentryInit;
			if (sentryInit != null)
			{
				StringName setShouldSampleEventMethod = SentryService._setShouldSampleEventMethod;
				Variant[] array = new Variant[1];
				int num2 = 0;
				Func<bool> func;
				if ((func = SentryService.<>O.<2>__ShouldSampleEvent) == null)
				{
					func = (SentryService.<>O.<2>__ShouldSampleEvent = new Func<bool>(SentryService.ShouldSampleEvent));
				}
				array[num2] = Callable.From<bool>(func);
				sentryInit.Call(setShouldSampleEventMethod, array);
			}
			if (branch != null)
			{
				Node sentryInit2 = SentryService._sentryInit;
				if (sentryInit2 != null)
				{
					sentryInit2.Call(SentryService._setPlatformBranchMethod, new Variant[] { branch });
				}
			}
			if (SentryService.IsEnabled)
			{
				if (SentryService._sampleRate == 0f)
				{
					Log.Info("[Sentry.NET] Disabled: no platform branch (non-Steam build)", 2);
					SentryService.Shutdown();
					return;
				}
				if (branch != null)
				{
					SentrySdk.ConfigureScope(delegate(Scope scope)
					{
						scope.SetTag("platform.branch", branch);
						scope.Environment = branch;
					});
				}
			}
			DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(45, 2);
			defaultInterpolatedStringHandler.AppendLiteral("[Sentry.NET] Platform branch: ");
			defaultInterpolatedStringHandler.AppendFormatted(branch);
			defaultInterpolatedStringHandler.AppendLiteral(", sample rate: ");
			defaultInterpolatedStringHandler.AppendFormatted<float>(SentryService._sampleRate, "P0");
			Log.Info(defaultInterpolatedStringHandler.ToStringAndClear(), 2);
		}

		/// <summary>
		/// Adds a breadcrumb for tracking user actions leading up to an error.
		/// </summary>
		// Token: 0x06008745 RID: 34629 RVA: 0x002D7E18 File Offset: 0x002D6018
		public static void AddBreadcrumb(string message, string category = "app", BreadcrumbLevel level = 0)
		{
			if (!SentryService.IsEnabled)
			{
				return;
			}
			SentrySdk.AddBreadcrumb(message, category, null, null, level);
		}

		/// <summary>
		/// Captures an exception and sends it to Sentry.
		/// Attaches current game state context for debugging.
		/// Respects user consent settings.
		/// </summary>
		// Token: 0x06008746 RID: 34630 RVA: 0x002D7E2C File Offset: 0x002D602C
		public static void CaptureException(Exception ex)
		{
			if (!SentryService.IsEnabled)
			{
				return;
			}
			SentrySdk.CaptureException(ex, delegate(Scope scope)
			{
				SentryService.AttachGameState(scope);
			});
		}

		/// <summary>
		/// Captures an exception with additional scope configuration.
		/// AttachGameState is called first, then the caller's configureScope action.
		/// </summary>
		// Token: 0x06008747 RID: 34631 RVA: 0x002D7E5C File Offset: 0x002D605C
		public static void CaptureException(Exception ex, Action<Scope> configureScope)
		{
			if (!SentryService.IsEnabled)
			{
				return;
			}
			SentrySdk.CaptureException(ex, delegate(Scope scope)
			{
				SentryService.AttachGameState(scope);
				configureScope(scope);
			});
		}

		/// <summary>
		/// Captures a message and sends it to Sentry.
		/// Respects user consent settings.
		/// </summary>
		// Token: 0x06008748 RID: 34632 RVA: 0x002D7E94 File Offset: 0x002D6094
		public static void CaptureMessage(string message, SentryLevel level = 1, [Nullable(new byte[] { 2, 1 })] Action<Scope> configureScope = null)
		{
			if (!SentryService.IsEnabled)
			{
				return;
			}
			SentryEvent sentryEvent = new SentryEvent
			{
				Message = message,
				Level = new SentryLevel?(level)
			};
			SentrySdk.CaptureEvent(sentryEvent, delegate(Scope scope)
			{
				SentryService.AttachGameState(scope);
				Action<Scope> configureScope2 = configureScope;
				if (configureScope2 == null)
				{
					return;
				}
				configureScope2(scope);
			});
		}

		/// <summary>
		/// Sets a tag on the current scope for filtering in Sentry.
		/// </summary>
		// Token: 0x06008749 RID: 34633 RVA: 0x002D7EE8 File Offset: 0x002D60E8
		public static void SetTag(string key, string value)
		{
			if (!SentryService.IsEnabled)
			{
				return;
			}
			SentrySdk.ConfigureScope(delegate(Scope scope)
			{
				scope.SetTag(key, value);
			});
		}

		/// <summary>
		/// Sets extra context data on the current scope.
		/// </summary>
		// Token: 0x0600874A RID: 34634 RVA: 0x002D7F24 File Offset: 0x002D6124
		public static void SetExtra(string key, object value)
		{
			if (!SentryService.IsEnabled)
			{
				return;
			}
			SentrySdk.ConfigureScope(delegate(Scope scope)
			{
				scope.SetExtra(key, value);
			});
		}

		/// <summary>
		/// Initializes Sentry for testing purposes, bypassing editor and release checks.
		/// Only use from dev console commands.
		/// </summary>
		// Token: 0x0600874B RID: 34635 RVA: 0x002D7F60 File Offset: 0x002D6160
		public static void InitializeForTesting()
		{
			if (SentryService.IsEnabled)
			{
				return;
			}
			string dsn = SentryService.GetDsn();
			if (string.IsNullOrEmpty(dsn))
			{
				Log.Warn("[Sentry.NET] Cannot initialize for testing: no DSN configured", 2);
				return;
			}
			SentryService._sentryInstance = SentrySdk.Init(delegate(SentryOptions options)
			{
				options.Dsn = dsn;
				options.Environment = "development";
				ReleaseInfo releaseInfo = ReleaseInfoManager.Instance.ReleaseInfo;
				options.Release = ((releaseInfo != null) ? releaseInfo.Version : null) ?? "dev-console-test";
				options.Debug = false;
				options.AutoSessionTracking = false;
				options.IsGlobalModeEnabled = true;
				options.SendDefaultPii = false;
			});
			SentryService.IsEnabled = SentrySdk.IsEnabled;
			if (!SentryService.IsEnabled)
			{
				Log.Warn("[Sentry.NET] SDK initialization failed for testing", 2);
				return;
			}
			SentrySdk.ConfigureScope(delegate(Scope scope)
			{
				scope.SetTag("sdk", "dotnet");
				scope.SetTag("session_id", SentryService._sessionId);
				scope.SetTag("source", "dev-console-test");
			});
			Log.Info("[Sentry.NET] Initialized for testing via dev console", 2);
		}

		/// <summary>
		/// Shuts down the Sentry SDK gracefully.
		/// Should be called when the game exits.
		/// </summary>
		// Token: 0x0600874C RID: 34636 RVA: 0x002D8004 File Offset: 0x002D6204
		public static void Shutdown()
		{
			if (!SentryService.IsEnabled)
			{
				return;
			}
			Action<LogLevel, string, int> action;
			if ((action = SentryService.<>O.<1>__OnLogCallback) == null)
			{
				action = (SentryService.<>O.<1>__OnLogCallback = new Action<LogLevel, string, int>(SentryService.OnLogCallback));
			}
			Log.LogCallback -= action;
			Log.Info("[Sentry.NET] Shutting down", 2);
			IDisposable sentryInstance = SentryService._sentryInstance;
			if (sentryInstance != null)
			{
				sentryInstance.Dispose();
			}
			SentryService._sentryInstance = null;
			Node sentryInit = SentryService._sentryInit;
			if (sentryInit != null)
			{
				sentryInit.Call(SentryService._shutdownMethod, Array.Empty<Variant>());
			}
			SentryService.IsEnabled = false;
		}

		// Token: 0x0600874D RID: 34637 RVA: 0x002D807C File Offset: 0x002D627C
		private static string GetDsn()
		{
			return ProjectSettings.GetSetting("sentry/config/dsn", "").AsString();
		}

		/// <summary>
		/// Attaches current game state to the Sentry scope for debugging context.
		/// Collects scene, run info, and combat state when available.
		/// </summary>
		// Token: 0x0600874E RID: 34638 RVA: 0x002D80A8 File Offset: 0x002D62A8
		private static void AttachGameState(Scope scope)
		{
			try
			{
				scope.SetExtra("loc.language", LocManager.Instance.Language);
				string currentSceneName = SentryService.GetCurrentSceneName();
				if (currentSceneName != null)
				{
					scope.SetTag("game.scene", currentSceneName);
				}
				RunState runState = RunManager.Instance.DebugOnlyGetState();
				if (RunManager.Instance.IsInProgress && runState != null)
				{
					scope.SetTag("game.in_run", "true");
					scope.SetExtra("game.seed", runState.Rng.StringSeed);
					scope.SetExtra("game.ascension", runState.AscensionLevel);
					scope.SetExtra("game.act", runState.CurrentActIndex + 1);
					scope.SetExtra("game.act_name", runState.Act.Id.ToString());
					scope.SetExtra("game.floor", runState.TotalFloor);
					scope.SetExtra("game.mode", runState.GameMode);
					AbstractRoom currentRoom = runState.CurrentRoom;
					scope.SetExtra("game.room_type", (currentRoom != null) ? currentRoom.GetType().Name : null);
					EventRoom eventRoom = currentRoom as EventRoom;
					if (eventRoom != null)
					{
						scope.SetExtra("game.event", eventRoom.CanonicalEvent.Id.Entry);
					}
					IReadOnlyList<Player> players = runState.Players;
					if (players.Count > 0)
					{
						scope.SetExtra("game.characters", string.Join<ModelId>(", ", players.Select<Player, ModelId>((Player p) => p.Character.Id)));
						scope.SetExtra("game.player_count", players.Count);
					}
				}
				else
				{
					scope.SetTag("game.in_run", "false");
				}
				CombatState combatState = CombatManager.Instance.DebugOnlyGetState();
				if (combatState != null)
				{
					string text = "combat.encounter";
					EncounterModel encounter = combatState.Encounter;
					scope.SetExtra(text, (encounter != null) ? encounter.Id.Entry : null);
					scope.SetExtra("combat.round", combatState.RoundNumber);
					scope.SetExtra("combat.enemy_count", combatState.Enemies.Count);
					scope.SetExtra("combat.enemies", string.Join(", ", combatState.Enemies.Select<Creature, string>(delegate(Creature e)
					{
						MonsterModel monster = e.Monster;
						return ((monster != null) ? monster.Id.ToString() : null) ?? "unknown";
					})));
					List<string> list = combatState.Players.Select<Player, string>(delegate(Player p)
					{
						DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(1, 2);
						defaultInterpolatedStringHandler.AppendFormatted<int>(p.Creature.CurrentHp);
						defaultInterpolatedStringHandler.AppendLiteral("/");
						defaultInterpolatedStringHandler.AppendFormatted<int>(p.Creature.MaxHp);
						return defaultInterpolatedStringHandler.ToStringAndClear();
					}).ToList<string>();
					if (list.Count > 0)
					{
						scope.SetExtra("combat.player_hp", string.Join(", ", list));
					}
				}
			}
			catch
			{
			}
		}

		// Token: 0x0600874F RID: 34639 RVA: 0x002D8374 File Offset: 0x002D6574
		[NullableContext(2)]
		private static string GetCurrentSceneName()
		{
			string text;
			try
			{
				NGame instance = NGame.Instance;
				if (instance == null)
				{
					text = null;
				}
				else if (instance.MainMenu != null)
				{
					text = "MainMenu";
				}
				else if (instance.CurrentRunNode != null)
				{
					NRun currentRunNode = instance.CurrentRunNode;
					if (currentRunNode.CombatRoom != null)
					{
						text = "CombatRoom";
					}
					else if (currentRunNode.MapRoom != null)
					{
						text = "MapRoom";
					}
					else if (currentRunNode.EventRoom != null)
					{
						text = "EventRoom";
					}
					else if (currentRunNode.RestSiteRoom != null)
					{
						text = "RestSiteRoom";
					}
					else if (currentRunNode.MerchantRoom != null)
					{
						text = "MerchantRoom";
					}
					else if (currentRunNode.TreasureRoom != null)
					{
						text = "TreasureRoom";
					}
					else
					{
						text = "Run";
					}
				}
				else if (instance.LogoAnimation != null)
				{
					text = "LogoAnimation";
				}
				else
				{
					text = null;
				}
			}
			catch
			{
				text = null;
			}
			return text;
		}

		/// <summary>
		/// The BeforeSend filter. Suppression (set for modded sessions) is checked first and is a
		/// plain bool read, so it cannot throw and cannot be defeated by an exception in the sampler
		/// below it (Sentry sends the event if BeforeSend throws).
		/// </summary>
		// Token: 0x06008750 RID: 34640 RVA: 0x002D8448 File Offset: 0x002D6648
		[return: Nullable(2)]
		private static SentryEvent FilterEvent(SentryEvent sentryEvent)
		{
			if (SentryService._suppressAllEvents)
			{
				return null;
			}
			if (sentryEvent.Exception is AutoSlayTimeoutException)
			{
				return null;
			}
			if (!SentryService.ShouldSampleEvent())
			{
				return null;
			}
			return sentryEvent;
		}

		// Token: 0x06008751 RID: 34641 RVA: 0x002D8470 File Offset: 0x002D6670
		private static bool ShouldSampleEvent()
		{
			return Random.Shared.NextDouble() < (double)SentryService._sampleRate && (!SaveManager.Instance.IsProfileInitialized || SaveManager.Instance.PrefsSave.UploadData) && (SentryService._isGameInitialized || SentryService.ShouldStayAliveAfterInit(false));
		}

		/// <summary>
		/// Returns true if the Sentry service should shutdown after we've finished initializing the game.
		/// This should only return false for things that don't change during the runtime of the game.
		/// We shutdown the service rather than filtering events so that crashes (which can't get filtered) don't get sent
		/// for modded games.
		/// </summary>
		// Token: 0x06008752 RID: 34642 RVA: 0x002D84C4 File Offset: 0x002D66C4
		private static bool ShouldStayAliveAfterInit(bool shouldLog)
		{
			if (SentryService.IsForcedOn)
			{
				if (shouldLog)
				{
					Log.Info("[Sentry.NET] Staying alive because we're forced on", 2);
				}
				return true;
			}
			if (!SteamInitializer.Initialized)
			{
				if (shouldLog)
				{
					Log.Info("[Sentry.NET] Steam not initialized", 2);
				}
				return false;
			}
			try
			{
				if (SaveManager.Instance.SettingsSave.FullConsole)
				{
					if (shouldLog)
					{
						Log.Info("[Sentry.NET] Full console is on", 2);
					}
					return false;
				}
			}
			catch
			{
				if (shouldLog)
				{
					Log.Info("[Sentry.NET] Exception while checking UploadData or FullConsole", 2);
				}
				return false;
			}
			if (ModManager.IsRunningModded())
			{
				if (shouldLog)
				{
					Log.Info("[Sentry.NET] Is running modded", 2);
				}
				return false;
			}
			LocManager instance = LocManager.Instance;
			if (instance != null && instance.OverridesActive)
			{
				if (shouldLog)
				{
					Log.Info("[Sentry.NET] Loc overrides are active", 2);
				}
				return false;
			}
			if (ModManager.HasHarmonyPatches())
			{
				if (shouldLog)
				{
					Log.Info("[Sentry.NET] Harmony patches active", 2);
				}
				return false;
			}
			return true;
		}

		// Token: 0x04002EDC RID: 11996
		private const string _dsnSettingPath = "sentry/config/dsn";

		// Token: 0x04002EDD RID: 11997
		private static readonly StringName _sentrySdkSingleton = new StringName("SentrySDK");

		// Token: 0x04002EDE RID: 11998
		private static readonly StringName _sentryUserClass = new StringName("SentryUser");

		// Token: 0x04002EDF RID: 11999
		private static readonly StringName _sentryBreadcrumbClass = new StringName("SentryBreadcrumb");

		// Token: 0x04002EE0 RID: 12000
		private static readonly StringName _levelProperty = new StringName("level");

		// Token: 0x04002EE1 RID: 12001
		private static readonly StringName _categoryProperty = new StringName("category");

		// Token: 0x04002EE2 RID: 12002
		private static readonly StringName _idProperty = new StringName("id");

		// Token: 0x04002EE3 RID: 12003
		private static readonly StringName _createMethod = new StringName("create");

		// Token: 0x04002EE4 RID: 12004
		private static readonly StringName _setUserMethod = new StringName("set_user");

		// Token: 0x04002EE5 RID: 12005
		private static readonly StringName _addBreadcrumbMethod = new StringName("add_breadcrumb");

		// Token: 0x04002EE6 RID: 12006
		private static readonly StringName _shutdownMethod = new StringName("shutdown");

		// Token: 0x04002EE7 RID: 12007
		private static readonly StringName _setShouldSampleEventMethod = new StringName("set_should_sample_event");

		// Token: 0x04002EE8 RID: 12008
		private static readonly StringName _setPlatformBranchMethod = new StringName("set_platform_branch");

		// Token: 0x04002EE9 RID: 12009
		private static readonly StringName _setCsharpContextMethod = new StringName("set_csharp_context");

		// Token: 0x04002EEA RID: 12010
		[Nullable(2)]
		private static IDisposable _sentryInstance;

		// Token: 0x04002EEB RID: 12011
		private static float _sampleRate = 1f;

		// Token: 0x04002EEC RID: 12012
		private static bool _isGameInitialized;

		// Token: 0x04002EED RID: 12013
		private static volatile bool _suppressAllEvents;

		// Token: 0x04002EF1 RID: 12017
		private static readonly string _sessionId = Guid.NewGuid().ToString();

		// Token: 0x04002EF2 RID: 12018
		[Nullable(2)]
		private static Node _sentryInit;

		// Token: 0x04002EF3 RID: 12019
		[Nullable(2)]
		private static GodotObject _extensionSdk;

		// Token: 0x02002427 RID: 9255
		[CompilerGenerated]
		private static class <>O
		{
			// Token: 0x040098BB RID: 39099
			[Nullable(0)]
			public static Func<bool> <0>__AlwaysRejectEvent;

			// Token: 0x040098BC RID: 39100
			[Nullable(new byte[] { 0, 1 })]
			public static Action<LogLevel, string, int> <1>__OnLogCallback;

			// Token: 0x040098BD RID: 39101
			[Nullable(0)]
			public static Func<bool> <2>__ShouldSampleEvent;
		}
	}
}
