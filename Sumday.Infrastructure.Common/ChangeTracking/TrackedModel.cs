using System;
using Sumday.Domain.Abstractions;

namespace Sumday.Infrastructure.Common.ChangeTracking
{
    internal class TrackedModel<T>
        where T : AggregateRoot
    {
        private readonly AggregateChangeTracker<T> changeTracker;

        private TrackedModel(T model, TrackedModelState state, AggregateChangeTracker<T> changeTracker = null)
        {
            this.Model = model ?? throw new ArgumentNullException(nameof(model));
            this.State = state;
            this.changeTracker = changeTracker ?? new AggregateChangeTracker<T>(model);
        }

        public TrackedModelState State { get; }

        public T Model { get; }

        public IObjectChangeTracker ChangeTracker => this.changeTracker;

        public bool HasChange => this.changeTracker.HasChange;

        public static TrackedModel<T> New(T model) => new TrackedModel<T>(model, TrackedModelState.New);

        public static TrackedModel<T> Existing(T model) => new TrackedModel<T>(model, TrackedModelState.Existing);

        public TrackedModel<T> WithNewState(TrackedModelState newState)
        {
            return new TrackedModel<T>(this.Model, newState, this.changeTracker);
        }
    }
}
