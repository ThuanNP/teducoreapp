﻿using TeduCoreApp.Data.Entities;
using TeduCoreApp.infrastructure.Interfaces;

namespace TeduCoreApp.Data.IRepositories
{
    public interface IBlogRepository : IRepository<Blog, int>
    {
    }
}