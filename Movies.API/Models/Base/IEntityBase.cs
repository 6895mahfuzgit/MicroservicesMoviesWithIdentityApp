﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Movies.API.Models.Base
{
    public interface IEntityBase<TId>
    {
        TId Id { get; }
    }
}