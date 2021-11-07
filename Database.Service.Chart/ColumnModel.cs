﻿using Database.Service.Chart;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using Utility.Database.Common;
using Utility.Entity;

namespace SQLite.Service.Service
{
    public class ColumnModel : IColumnModel
    {
        private Dictionary<IKey, IReadOnlyCollection<Common.Model.ColumnModel>> collection = new();
        private readonly ColumnDataFactory factory;

        public ColumnModel(ColumnDataFactory factory)
        {
            this.factory = factory;
        }

        public IObservable<IReadOnlyCollection<Common.Model.ColumnModel>> GetCollection(ITableKey tableKey)
        {
            collection[tableKey] = collection.GetValueOrDefault(tableKey);
            if (collection[tableKey] == null)
            {
                Subject<IReadOnlyCollection<Common.Model.ColumnModel>> subject = new();

                factory
                    .Create(tableKey)
                    .Subscribe(a =>
                   {
                       collection[tableKey] = a;
                       subject.OnNext(a);
                   });

                return subject;
            }
            return Observable.Return(collection[tableKey]);
        }
    }

}