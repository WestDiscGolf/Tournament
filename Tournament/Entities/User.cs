using System;
using System.Security.Cryptography;
using System.Text;

namespace Tournament.Entities
{
    /// <summary>
    /// Based on the RaccoonBlog implementation (https://github.com/ayende/RaccoonBlog/blob/master/RaccoonBlog.Web/Models/User.cs)
    /// </summary>
    public class User : RootAggregate
    {
        public string UserName { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public bool Enabled { get; set; }
        public string HashedPassword { get; private set; }
        
        private string _passwordSalt;
        private string PasswordSalt
        {
            get
            {
                return _passwordSalt ?? (_passwordSalt = Guid.NewGuid().ToString("N"));
            }
            // this makes r# complain, but it is due to the fact it is a private property which is still saved in RavenDB and hence needs a setter
            set { _passwordSalt = value; }
        }

        public User SetPassword(string pwd)
        {
            HashedPassword = GetHashedPassword(pwd);
            return this;
        }

        private string GetHashedPassword(string pwd)
        {
            using (var sha = SHA256.Create())
            {
                var computedHash = sha.ComputeHash(Encoding.Unicode.GetBytes(PasswordSalt + pwd + Settings.ConstantSalt));
                return Convert.ToBase64String(computedHash);
            }
        }

        public bool ValidatePassword(string maybePwd)
        {
            if (HashedPassword == null)
                return true;
            return HashedPassword == GetHashedPassword(maybePwd);
        }
    }
}