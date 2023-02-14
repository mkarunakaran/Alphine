using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Reflection;
using System.Threading;
using Sumday.Domain.Abstractions;

namespace Sumday.Infrastructure.Common
{
    public class BaseAggregateClassMap
    {
        private static readonly ReaderWriterLockSlim ConfigLock = new ReaderWriterLockSlim();
        private static readonly Dictionary<Type, DeltaUpdateConfiguration> UpdateConfigurations =
            new Dictionary<Type, DeltaUpdateConfiguration>();

        private readonly Type aggregateClassType;
        private readonly List<EntityMemberMap> allMemberMaps; // includes inherited member maps
        private readonly ReadOnlyCollection<EntityMemberMap> allMemberMapsReadonly;
        private readonly List<EntityMemberMap> declaredMemberMaps; // only the members declared in this class

        public BaseAggregateClassMap(Type aggregateClassType)
        {
            this.aggregateClassType = aggregateClassType;
            this.allMemberMaps = new List<EntityMemberMap>();
            this.allMemberMapsReadonly = this.allMemberMaps.AsReadOnly();
            this.declaredMemberMaps = new List<EntityMemberMap>();
        }

        public Type AggregateClassType
        {
            get { return this.aggregateClassType; }
        }

        public ReadOnlyCollection<EntityMemberMap> AllMemberMaps
        {
            get { return this.allMemberMapsReadonly; }
        }

        public IEnumerable<EntityMemberMap> DeclaredMemberMaps
        {
            get { return this.declaredMemberMaps; }
        }

        public static void RegisterUpdateStrategy<TAggregate>(Action<Type> aggregateMapInitializer)
              where TAggregate : AggregateRoot
        {
            ConfigLock.EnterWriteLock();
            try
            {
                aggregateMapInitializer(typeof(TAggregate));
            }
            finally
            {
                ConfigLock.ExitWriteLock();
            }
        }

        public static void UseDeltaUpdateStrategy<TAggregate>()
            where TAggregate : AggregateRoot
        {
            ChangeConfigWithWriteLock<TAggregate>(config => config.EnableDeltaUpdateStrategy());
        }

        internal static bool ShouldUseDeltaUpdateStrategy<TAggregate>()
             where TAggregate : AggregateRoot
        {
            return GetConfigValueWithReadLock<TAggregate, bool>(config => config.UseDeltaUpdateStrategy);
        }

        private static DeltaUpdateConfiguration GetDeltaUpdateConfiguration<T>()
             where T : Entity
        {
            ConfigLock.EnterReadLock();
            try
            {
                if (UpdateConfigurations.TryGetValue(typeof(T), out var config))
                {
                    return config;
                }
            }
            finally
            {
                ConfigLock.ExitReadLock();
            }

            ConfigLock.EnterWriteLock();
            try
            {
                var config = new DeltaUpdateConfiguration();
                UpdateConfigurations.Add(typeof(T), config);
                return config;
            }
            finally
            {
                ConfigLock.ExitWriteLock();
            }
        }

        private static void ChangeConfigWithWriteLock<T>(Action<DeltaUpdateConfiguration> action)
            where T : Entity
        {
            var config = GetDeltaUpdateConfiguration<T>();

            ConfigLock.EnterWriteLock();
            try
            {
                action(config);
            }
            finally
            {
                ConfigLock.ExitWriteLock();
            }
        }

        private static T GetConfigValueWithReadLock<TEntity, T>(Func<DeltaUpdateConfiguration, T> expression)
            where TEntity : Entity
        {
            var config = GetDeltaUpdateConfiguration<TEntity>();

            ConfigLock.EnterReadLock();
            try
            {
                return expression(config);
            }
            finally
            {
                ConfigLock.ExitReadLock();
            }
        }
    }
}
