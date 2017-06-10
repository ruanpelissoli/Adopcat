﻿using Adopcat.Model;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Adopcat.Services.Interfaces
{
    public interface IUserService
    {
        Task<User> GetByEmail(string email);
        Task<User> GetById(int id);
        Task<List<User>> GetAllActive();
        Task<bool> Deactivate(int idUser);
        Task<User> UpdateOrCreateAsync(User model);
        Task<bool> EmailExists(int idUser, string email);
    }
}
