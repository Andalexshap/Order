using Newtonsoft.Json;
using Order.Interfaces;
using Order.Models;
using Order.Models.Account;

namespace Order.Servises
{
    public class AccountService : IAccountService
    {
        private string FileName;
        public void SetFileName(string filename)
        {
            FileName = filename;
        }

        private Accounts GetAllAccounts()
        {
            Accounts response;

            try
            {
                response = JsonConvert
                    .DeserializeObject<Accounts>(File
                    .ReadAllText(FileName));
            }
            catch
            {
                return null;

            }
            return response;
        }

        public AuthorizationResponse LoginUser(User request)
        {
            var accounts = GetAllAccounts();

            if (accounts == null)
            {
                return new AuthorizationResponse
                {
                    Sucsess = false,
                    Error = new List<Error>
                    {
                        new Error
                        {
                            Code = "001",
                            Message = "Accounts is not Found!",
                            Target = nameof(LoginUser)
                        }
                    }
                };
            }

            var found = accounts.Users.FirstOrDefault(x => x.Login == request.Login);
            if (found == null)
            {
                return new AuthorizationResponse
                {
                    Sucsess = false,
                    Error = new List<Error>
                    {
                        new Error
                        {
                            Code = "002",
                            Message = "User is not Found!",
                            Target = nameof(LoginUser)
                        }
                    }
                };
            }

            if (found.Password == request.Password)
            {
                found.Password = string.Empty;

                return new AuthorizationResponse
                {
                    Sucsess = true,
                    User = found
                };
            }

            return new AuthorizationResponse
            {
                Sucsess = false,
                Error = new List<Error>
                    {
                        new Error
                        {
                            Code = "003",
                            Message = "Password is not correct!",
                            Target = nameof(LoginUser)
                        }
                    }
            };
        }
        public AuthorizationResponse RegisterUser(User request)
        {
            var accounts = GetAllAccounts();

            if (accounts != null)
            {
                var found = accounts.Users.FirstOrDefault(x => x.Login == request.Login);
                if (found != null)
                {
                    return new AuthorizationResponse
                    {
                        Sucsess = false,
                        Error = new List<Error>
                        {
                            new Error
                            {
                                Code = "004",
                                Message = "User exist!",
                                Target= nameof(RegisterUser)
                            }
                        }
                    };
                }

            }
            else
            {
                accounts = new Accounts();
                accounts.Users = new List<User>();

                var administrator = request;
                administrator.Key = Guid.NewGuid().ToString();
                administrator.MemberType = MemberType.Administrator;

                accounts.Users.Add(administrator);

                using (StreamWriter writer = File.CreateText(FileName))
                {
                    string output = JsonConvert.SerializeObject(accounts);
                    writer.Write(output);
                };

                return new AuthorizationResponse
                {
                    Sucsess = true,
                    User = administrator
                };
            }

            var user = request;
            user.Key = Guid.NewGuid().ToString();
            user.MemberType = MemberType.User;

            accounts.Users.Add(user);

            using (StreamWriter writer = File.CreateText(FileName))
            {
                string output = JsonConvert.SerializeObject(accounts);
                writer.Write(output);
            };

            return new AuthorizationResponse
            {
                Sucsess = true,
                User = user
            };

        }

    }
}
