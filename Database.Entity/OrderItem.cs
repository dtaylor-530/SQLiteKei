﻿using ReactiveUI;
using System.Collections.ObjectModel;

namespace Database.Entity
{
    public class OrderItem : ReactiveObject
    {
        public ObservableCollection<string> Columns { get; set; }

        private string selectedColumn;
        public string SelectedColumn
        {
            get { return selectedColumn; }
            set { this.RaiseAndSetIfChanged(ref selectedColumn, value); }
        }

        private bool isDescending;
        public bool IsDescending
        {
            get { return isDescending; }
            set { this.RaiseAndSetIfChanged(ref isDescending, value); }
        }

        public OrderItem()
        {
            Columns = new ObservableCollection<string>();
        }
    }
}