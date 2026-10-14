using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Logging;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Runs;

namespace MegaCrit.Sts2.Core.Modding
{
	/// <summary>
	/// Some helper methods for modding.
	/// </summary>
	// Token: 0x02000B94 RID: 2964
	[NullableContext(1)]
	[Nullable(0)]
	public static class ModHelper
	{
		/// <summary>
		/// Called by mods to add their models to a pool.
		/// Throws if the pool has already concatenated models to that pool (i.e. the mod was too late in calling this method).
		/// </summary>
		/// <typeparam name="TPoolType">The pool type to add to.</typeparam>
		/// <typeparam name="TModelType">The model to add to the pool.</typeparam>
		// Token: 0x06007BC1 RID: 31681 RVA: 0x002A137D File Offset: 0x0029F57D
		public static void AddModelToPool<[Nullable(0)] TPoolType, [Nullable(0)] TModelType>() where TPoolType : AbstractModel, IPoolModel where TModelType : AbstractModel
		{
			ModHelper.AddModelToPool(typeof(TPoolType), typeof(TModelType));
		}

		// Token: 0x06007BC2 RID: 31682 RVA: 0x002A1398 File Offset: 0x0029F598
		public static void AddModelToPool(Type poolType, Type modelType)
		{
			ModHelper.ModPoolContent modPoolContent;
			if (!ModHelper._moddedContentForPools.TryGetValue(poolType, out modPoolContent))
			{
				modPoolContent = new ModHelper.ModPoolContent
				{
					modelsToAdd = new List<Type>()
				};
				ModHelper._moddedContentForPools.Add(poolType, modPoolContent);
			}
			if (modPoolContent.isFrozen)
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(101, 2);
				defaultInterpolatedStringHandler.AppendLiteral("Tried to add model ");
				defaultInterpolatedStringHandler.AppendFormatted<Type>(modelType);
				defaultInterpolatedStringHandler.AppendLiteral(" to pool ");
				defaultInterpolatedStringHandler.AppendFormatted<Type>(poolType);
				defaultInterpolatedStringHandler.AppendLiteral(", but it's too late! You must add content before the game is initialized.");
				throw new InvalidOperationException(defaultInterpolatedStringHandler.ToStringAndClear());
			}
			modPoolContent.modelsToAdd.Add(modelType);
		}

		/// <summary>
		/// Called by a pool when it is ready to add modded content to its pool of content.
		/// </summary>
		/// <param name="poolModel">The pool that is consuming the content.</param>
		/// <param name="pool">The current pool of content.</param>
		/// <typeparam name="TModelType">The model type of the content that should have been added by mods.</typeparam>
		/// <returns>A new pool with all modded content concatenated to pool.</returns>
		// Token: 0x06007BC3 RID: 31683 RVA: 0x002A1430 File Offset: 0x0029F630
		public static IEnumerable<TModelType> ConcatModelsFromMods<[Nullable(0)] TModelType>(IPoolModel poolModel, IEnumerable<TModelType> pool) where TModelType : AbstractModel
		{
			Type type = poolModel.GetType();
			ModHelper.ModPoolContent modPoolContent;
			if (!ModHelper._moddedContentForPools.TryGetValue(type, out modPoolContent))
			{
				modPoolContent = new ModHelper.ModPoolContent();
				ModHelper._moddedContentForPools.Add(type, modPoolContent);
			}
			modPoolContent.isFrozen = true;
			if (modPoolContent.modelsToAdd == null)
			{
				return pool;
			}
			IEnumerable<TModelType> enumerable = modPoolContent.modelsToAdd.Select<Type, TModelType>((Type t) => ModelDb.GetById<TModelType>(ModelDb.GetId(t)));
			return pool.Concat<TModelType>(enumerable);
		}

		/// <summary>
		/// Called by mods when they wish to provide custom model types to a RunState when IterateHookListeners is called.
		/// </summary>
		/// <param name="id">An ID to identify the subscription. Usually the name of the mod.</param>
		/// <param name="del">The delegate provided by the mod, which returns the models to iterate over.</param>
		// Token: 0x06007BC4 RID: 31684 RVA: 0x002A14A8 File Offset: 0x0029F6A8
		public static void SubscribeForRunStateHooks(string id, RunHookSubscriptionDelegate del)
		{
			if (ModHelper._runHookSubscribers.Any<ModHelper.ModRunHookSubscriber>((ModHelper.ModRunHookSubscriber s) => s.id == id))
			{
				Log.Error("Tried to subscribe for RunState hooks with id " + id + ", but it's already been used! Ignoring subscription", 2);
				return;
			}
			ModHelper._runHookSubscribers.Add(new ModHelper.ModRunHookSubscriber
			{
				id = id,
				del = del
			});
			ModHelper._runHookSubscribers.Sort((ModHelper.ModRunHookSubscriber x, ModHelper.ModRunHookSubscriber y) => string.CompareOrdinal(x.id, y.id));
		}

		/// <summary>
		/// Called by mods when they wish to provide custom model types to a CombatState when IterateHookListeners is called.
		/// </summary>
		/// <param name="id">An ID to identify the subscription. Usually the name of the mod.</param>
		/// <param name="del">The delegate provided by the mod, which returns the models to iterate over.</param>
		// Token: 0x06007BC5 RID: 31685 RVA: 0x002A1544 File Offset: 0x0029F744
		public static void SubscribeForCombatStateHooks(string id, CombatHookSubscriptionDelegate del)
		{
			if (ModHelper._combatHookSubscribers.Any<ModHelper.ModCombatHookSubscriber>((ModHelper.ModCombatHookSubscriber s) => s.id == id))
			{
				Log.Error("Tried to subscribe for CombatState hooks with id " + id + ", but it's already been used! Ignoring subscription", 2);
				return;
			}
			ModHelper._combatHookSubscribers.Add(new ModHelper.ModCombatHookSubscriber
			{
				id = id,
				del = del
			});
			ModHelper._combatHookSubscribers.Sort((ModHelper.ModCombatHookSubscriber x, ModHelper.ModCombatHookSubscriber y) => string.CompareOrdinal(x.id, y.id));
		}

		/// <summary>
		/// Called by RunState when it is iterating hook listeners, so that custom mod AbstractModel types have hooks called
		/// on them.
		/// </summary>
		/// <param name="runState">The RunState which is iterating hook listeners.</param>
		/// <returns>All the model types from all subscribing mods who wish to receive hooks.</returns>
		// Token: 0x06007BC6 RID: 31686 RVA: 0x002A15DD File Offset: 0x0029F7DD
		public static IEnumerable<AbstractModel> IterateAllRunStateSubscribers(RunState runState)
		{
			ModHelper.<IterateAllRunStateSubscribers>d__11 <IterateAllRunStateSubscribers>d__ = new ModHelper.<IterateAllRunStateSubscribers>d__11(-2);
			<IterateAllRunStateSubscribers>d__.<>3__runState = runState;
			return <IterateAllRunStateSubscribers>d__;
		}

		/// <summary>
		/// Called by CombatState when it is iterating hook listeners, so that custom mod AbstractModel types have hooks
		/// called on them.
		/// </summary>
		/// <param name="combatState">The CombatState which is iterating hook listeners.</param>
		/// <returns>All the model types from all subscribing mods who wish to receive hooks.</returns>
		// Token: 0x06007BC7 RID: 31687 RVA: 0x002A15ED File Offset: 0x0029F7ED
		public static IEnumerable<AbstractModel> IterateAllCombatStateSubscribers(CombatState combatState)
		{
			ModHelper.<IterateAllCombatStateSubscribers>d__12 <IterateAllCombatStateSubscribers>d__ = new ModHelper.<IterateAllCombatStateSubscribers>d__12(-2);
			<IterateAllCombatStateSubscribers>d__.<>3__combatState = combatState;
			return <IterateAllCombatStateSubscribers>d__;
		}

		// Token: 0x04002920 RID: 10528
		private static readonly Dictionary<Type, ModHelper.ModPoolContent> _moddedContentForPools = new Dictionary<Type, ModHelper.ModPoolContent>();

		// Token: 0x04002921 RID: 10529
		private static readonly List<ModHelper.ModRunHookSubscriber> _runHookSubscribers = new List<ModHelper.ModRunHookSubscriber>();

		// Token: 0x04002922 RID: 10530
		private static readonly List<ModHelper.ModCombatHookSubscriber> _combatHookSubscribers = new List<ModHelper.ModCombatHookSubscriber>();

		// Token: 0x020022C1 RID: 8897
		[NullableContext(0)]
		private class ModPoolContent
		{
			// Token: 0x04009235 RID: 37429
			public bool isFrozen;

			// Token: 0x04009236 RID: 37430
			[Nullable(new byte[] { 2, 1 })]
			public List<Type> modelsToAdd;
		}

		// Token: 0x020022C2 RID: 8898
		[Nullable(0)]
		[RequiredMember]
		private class ModRunHookSubscriber
		{
			// Token: 0x0600BF2D RID: 48941 RVA: 0x00412AD6 File Offset: 0x00410CD6
			[Obsolete("Constructors of types with required members are not supported in this version of your compiler.", true)]
			[CompilerFeatureRequired("RequiredMembers")]
			public ModRunHookSubscriber()
			{
			}

			// Token: 0x04009237 RID: 37431
			[RequiredMember]
			public string id;

			// Token: 0x04009238 RID: 37432
			[RequiredMember]
			public RunHookSubscriptionDelegate del;
		}

		// Token: 0x020022C3 RID: 8899
		[Nullable(0)]
		[RequiredMember]
		private class ModCombatHookSubscriber
		{
			// Token: 0x0600BF2E RID: 48942 RVA: 0x00412ADE File Offset: 0x00410CDE
			[Obsolete("Constructors of types with required members are not supported in this version of your compiler.", true)]
			[CompilerFeatureRequired("RequiredMembers")]
			public ModCombatHookSubscriber()
			{
			}

			// Token: 0x04009239 RID: 37433
			[RequiredMember]
			public string id;

			// Token: 0x0400923A RID: 37434
			[RequiredMember]
			public CombatHookSubscriptionDelegate del;
		}
	}
}
