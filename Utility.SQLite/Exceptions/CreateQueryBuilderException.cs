using System;

namespace SQLiteKei.DataAccess.Exceptions
{
    public class CreateQueryBuilderException : Exception
    {
        public CreateQueryBuilderException(string message)
            : base(message)
        {
        }
    }
}