using Order.Interfaces;
using Order.Models;
using Order.Models.Account;
using Order.Utils;

namespace Order.Services
{
    public class AccountService : IAccountService
    {
        private string FileName = @"accounts.json";

        public void SetFileName(string filename)
        {
            if (!string.IsNullOrEmpty(filename))
            {
                FileName = filename;
            }
        }

        private Accounts? GetAccounts()
        {
            Accounts? response;

            try
            {
                response = FileName.GetData<Accounts>();
            }
            catch
            {
                return null;

            }
            return response;
        }

        public AuthorizationResponse LoginUser(User request)
        {
            var accounts = GetAccounts();

            if (accounts == null)
            {
                return new AuthorizationResponse(new Error
                {
                    Code = "001",
                    Message = "Accounts is not Found!",
                    Target = nameof(LoginUser)
                });
            }

            var found = accounts.Users.FirstOrDefault(x => x.Login == request.Login);

            if (found == null)
            {
                return new AuthorizationResponse(new Error
                {
                    Code = "002",
                    Message = "User is not Found!",
                    Target = nameof(LoginUser)
                });
            }

            if (found.Password == request.Password)
            {
                found.Password = string.Empty;

                return new AuthorizationResponse(found);
            }

            return new AuthorizationResponse(new Error
            {
                Code = "003",
                Message = "Password is not correct!",
                Target = nameof(LoginUser)
            });
        }
        public AuthorizationResponse RegisterUser(User request)
        {
            var accounts = GetAccounts();

            if (accounts != null)
            {
                var found = accounts.Users.FirstOrDefault(x => x.Login == request.Login);
                if (found != null)
                {
                    return new AuthorizationResponse(new Error
                    {
                        Code = "004",
                        Message = "User exist!",
                        Target = nameof(RegisterUser)
                    });
                }

            }
            else
            {
                accounts = new Accounts();
                accounts.Users = new List<User>();

                var administrator = request;
                administrator.Id = Guid.NewGuid().ToString();
                administrator.Key = Guid.NewGuid().ToString();
                administrator.MemberType = MemberType.Administrator;

                accounts.Users.Add(administrator);

                FileName.WriteData(accounts);

                return new AuthorizationResponse(true);
            }

            var user = request;
            user.Id = Guid.NewGuid().ToString();
            user.Key = Guid.NewGuid().ToString();
            user.MemberType = MemberType.User;

            accounts.Users.Add(user);

            FileName.WriteData(accounts);

            return new AuthorizationResponse(user);
        }

        public AuthorizationResponse UpdateUser(User request)
        {
            var accounts = GetAccounts();

            if (accounts == null)
            {
                return new AuthorizationResponse(new Error
                {
                    Code = "001",
                    Message = "Accounts is not Found!",
                    Target = nameof(LoginUser)
                });
            }

            var found = accounts.Users.FirstOrDefault(x => x.Login == request.Login);

            if (found == null)
            {
                return new AuthorizationResponse(new Error
                {
                    Code = "002",
                    Message = "User is not Found!",
                    Target = nameof(LoginUser)
                });
            }

            request.Key = found.Key;
            request.Password = found.Password;
            request.MemberType = found.MemberType;

            accounts.Users.Remove(found);
            accounts.Users.Add(request);

            FileName.WriteData(accounts);

            return new AuthorizationResponse(true);
        }

        public AuthorizationResponse DeleteUserbyLogin(string login)
        {
            var accounts = GetAccounts();

            if (accounts == null)
            {
                return new AuthorizationResponse(new Error
                {
                    Code = "001",
                    Message = "Accounts is not Found!",
                    Target = nameof(LoginUser)
                });
            }

            var found = accounts.Users.FirstOrDefault(x => x.Login == login);

            if (found == null)
            {
                return new AuthorizationResponse(new Error
                {
                    Code = "002",
                    Message = "User is not Found!",
                    Target = nameof(LoginUser)
                });
            }

            accounts.Users.Remove(found);

            FileName.WriteData(accounts);

            return new AuthorizationResponse(true);
        }


        public AuthorizationResponse DeleteUserbyId(string userId)
        {
            var accounts = GetAccounts();

            if (accounts == null)
            {
                return new AuthorizationResponse(new Error
                {
                    Code = "001",
                    Message = "Accounts is not Found!",
                    Target = nameof(LoginUser)
                });
            }

            var found = accounts.Users.FirstOrDefault(x => x.Id == userId);

            if (found == null)
            {
                return new AuthorizationResponse(new Error
                {
                    Code = "002",
                    Message = "User is not Found!",
                    Target = nameof(LoginUser)
                });
            }

            accounts.Users.Remove(found);

            FileName.WriteData(accounts);

            return new AuthorizationResponse(true);
        }
    }
}
