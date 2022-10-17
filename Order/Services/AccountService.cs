using Newtonsoft.Json;
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
            var accounts = GetAccounts();

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

        public AuthorizationResponse UpdateUser(User request)
        {
            var accounts = GetAccounts();

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

            var found = accounts.Users.FirstOrDefault(x => x.Login == request.Login & x.Password == request.Password);

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

            found = request;

            using (StreamWriter writer = File.CreateText(FileName))
            {
                string output = JsonConvert.SerializeObject(accounts);
                writer.Write(output);
            };

            return new AuthorizationResponse
            {
                Sucsess = true
            };
        }

        public AuthorizationResponse DeleteUserbyLogin(string userLogin)
        {
            var accounts = GetAccounts();

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

            var found = accounts.Users.FirstOrDefault(x => x.Login == userLogin);

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

            accounts.Users.Remove(found);

            using (StreamWriter writer = File.CreateText(FileName))
            {
                string output = JsonConvert.SerializeObject(accounts);
                writer.Write(output);
            };

            return new AuthorizationResponse
            {
                Sucsess = true
            };
        }

        public AuthorizationResponse DeleteUserbyId(string userId)
        {
            var accounts = GetAccounts();

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

            var found = accounts.Users.FirstOrDefault(x => x.Key == userId);

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

            accounts.Users.Remove(found);

            using (StreamWriter writer = File.CreateText(FileName))
            {
                string output = JsonConvert.SerializeObject(accounts);
                writer.Write(output);
            };

            return new AuthorizationResponse
            {
                Sucsess = true
            };
        }
    }
}
