using Order.Models.Account;

namespace Order.Interfaces
{
    public interface IAccountService
    {
        void SetFileName(string filename);

        AuthorizationResponse LoginUser(User request);

        AuthorizationResponse RegisterUser(User request);

        AuthorizationResponse UpdateUser(User request);

        AuthorizationResponse DeleteUserbyLogin(string userLogin);
        AuthorizationResponse DeleteUserbyId(string userId);
    }
}
