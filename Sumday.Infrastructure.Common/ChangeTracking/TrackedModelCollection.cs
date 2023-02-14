using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Sumday.Domain.Abstractions;

namespace Sumday.Infrastructure.Common.ChangeTracking
{
    internal class TrackedModelCollection<T> : IEnumerable<TrackedModel<T>>
        where T : AggregateRoot
    {
        private readonly Dictionary<T, TrackedModel<T>> trackedModels = new Dictionary<T, TrackedModel<T>>();

        public IEnumerator<TrackedModel<T>> GetEnumerator() => this.trackedModels.Values.GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => this.GetEnumerator();

        public void New(T model)
        {
            if (model == null)
            {
                throw new ArgumentNullException(nameof(model));
            }

            if (this.trackedModels.TryGetValue(model, out var existingModel) && existingModel.State == TrackedModelState.Removed)
            {
                this.trackedModels[model] = existingModel.WithNewState(TrackedModelState.Existing);
                return;
            }

            this.trackedModels.Add(model, TrackedModel<T>.New(model));
        }

        public void Existing(T model)
        {
            if (model == null)
            {
                throw new ArgumentNullException(nameof(model));
            }

            if (!this.trackedModels.ContainsKey(model))
            {
                this.trackedModels.Add(model, TrackedModel<T>.Existing(model));
            }
        }

        public void Remove(T model)
        {
            if (model == null)
            {
                throw new ArgumentNullException(nameof(model));
            }

            var trackedModel = this.trackedModels[model];
            if (trackedModel.State == TrackedModelState.New)
            {
                this.trackedModels.Remove(model);
                return;
            }

            this.trackedModels[model] = trackedModel.WithNewState(TrackedModelState.Removed);
        }

        public IEnumerable<TrackedModel<T>> OfState(TrackedModelState state) => this.Where(m => m.State == state);
    }
}
