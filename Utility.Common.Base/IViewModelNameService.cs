﻿using Utility.Entity;

namespace Utility.Common.Contracts
{
    public interface IViewModelNameService
    {
        string Get(Key key);
    }
}