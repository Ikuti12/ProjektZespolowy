﻿using System.Threading.Tasks;
using SystemZarzadzaniaAkademikiem.Models;

namespace SystemZarzadzaniaAkademikiem.Services
{
    public interface IAdminRepo
    {
        Task<SuperUser> GetAdmin();
        Task<int> SaveAdminAsync(SuperUser admin);
    }
}
