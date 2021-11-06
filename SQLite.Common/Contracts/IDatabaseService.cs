﻿using Utility.Entity;

namespace SQLite.Service.Service
{
    public interface IDatabaseService
    {
        void CloseDatabase();
        void CloseDatabase(IKey key);
        void CreateNewDatabase();
        IObservable<bool> CreateTable(string sqlStatement);
        void DeleteDatabase();
        void OpenDatabase();
        void OpenDatabase(Key key);
    }
}