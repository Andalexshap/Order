using Order.Models;

namespace Order.Interfaces
{
    public interface IAccountService
    {
        void SetFileName(string filename);

        AuthorizationResponse LoginUser(User request);

        AuthorizationResponse RegisterUser(User request);
    }
}
