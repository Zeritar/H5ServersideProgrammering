using H5ServersideProgrammering.Data;

namespace H5ServersideProgrammering.Repository
{
    public interface IUserCPRRepository
    {
        public List<UserCpr> GetAll();
        public UserCpr? GetById(int id);
        public UserCpr? GetByUserId(string userID);
        public void Add(UserCpr userCpr);
        public void Update(UserCpr userCpr);
        public bool Delete(int id);
    }

    public class UserCPRRepository : IUserCPRRepository
    {
        private readonly AppDataContext _context;

        public UserCPRRepository(AppDataContext context)
        {
            _context = context;
        }

        public List<UserCpr> GetAll()
        {
            return _context.UserCprs.ToList();
        }

        public UserCpr? GetById(int id)
        {
            return _context.UserCprs.FirstOrDefault(x => x.Id == id);
        }

        public UserCpr? GetByUserId(string userID)
        {
            return _context.UserCprs.FirstOrDefault(x => x.UserId == userID);
        }

        public void Add(UserCpr userCpr)
        {
            _context.UserCprs.Add(userCpr);
            _context.SaveChanges();
        }

        public void Update(UserCpr userCpr)
        {
            _context.UserCprs.Update(userCpr);
            _context.SaveChanges();
        }

        public bool Delete(int id)
        {
            var userCpr = _context.UserCprs.First(x => x.Id == id);
            if (userCpr != null)
            {
                _context.UserCprs.Remove(userCpr);
                _context.SaveChanges();
                return true;
            }

            return false;
        }
    }
}
