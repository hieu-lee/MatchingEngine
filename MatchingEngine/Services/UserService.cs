namespace MatchingEngine.Services
{
    public class UserService
    {
        private readonly AppDbContext dbContext;

        public UserService(AppDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task<User> GetUserAsync(string Id)
        {
            return await dbContext.Users.Where(s => s.Id == Id).FirstOrDefaultAsync();
        }

        public async Task<User> GetUserByEmailAsync(string Email)
        {
            return await dbContext.Users.Where(s => s.Email == Email).FirstOrDefaultAsync();
        }

        public async Task<Result> CreateUserAsync(User user)
        {
            var tmp = await dbContext.Users.Where(s => s.Email == user.Email).FirstOrDefaultAsync();
            if (tmp is not null)
            {
                return new("User's email has been taken");
            }
            await dbContext.Users.AddAsync(user);
            await dbContext.SaveChangesAsync();
            return new();
        }

        public async Task<Result> UpdateUserAsync(User user)
        {
            var tmp = await dbContext.Users.Where(s => s.Email == user.Email).FirstOrDefaultAsync();
            if (tmp is null)
            {
                return new("User doesn't exists");
            }
            tmp.Name = user.Name;
            tmp.Email = user.Email;
            dbContext.Users.Update(tmp);
            await dbContext.SaveChangesAsync();
            return new();
        }

        public async Task<Result> DeleteUserAsync(User user)
        {
            var tmp = await dbContext.Users.Where(s => s.Email == user.Email).FirstOrDefaultAsync();
            if (tmp is null)
            {
                return new("User doesn't exists");
            }
            dbContext.Users.Remove(tmp);
            await dbContext.SaveChangesAsync();
            return new();
        }
    }
}
