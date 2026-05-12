using BackendCsharp.API.DTOs;
using BackendCsharp.API.Entities;
using BackendCsharp.API.Infra;
using BackendCsharp.API.Responses;
using BackendCsharp.API.Services;
using BackendCsharp.API.Validations;

namespace BackendCsharp.API.Repositories
{
    public class UserRepository
    {
        private readonly DataDbContext db;
        private readonly UserValidator validator;
        private readonly PasswordService passwordService;
        public UserRepository(DataDbContext db, UserValidator validator,PasswordService passwordService)
        {
            this.db = db;
            this.validator = validator;
            this.passwordService = passwordService;
        }
        public UserResponse Save(UserRequests request)
        {
            if(!Validate(request)) throw new ArgumentException("Dados invalidos!");
            var passwordHash = passwordService.Hash(request.Password);
            var entity = new UserEntity(request.Username,passwordHash);
            db.Add(entity);
            db.SaveChanges();
            return new UserResponse(entity.Id,entity.Username,entity.CreatedAt);
        }

        public UserEntity Find(string username)
        {
            var user = db.Users.FirstOrDefault(x => x.Username == username);
            if (user!=null)
            {
                return user;
            }
            return null;
        }

        private bool Validate(UserRequests request)
        {
            
            var result = validator.Validate(request);
            if (result.IsValid == false)
            {
                return false;
            }
            return true;
        }
    }
}
