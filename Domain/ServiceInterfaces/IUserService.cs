using Domain.DTOs;
using System.Threading.Tasks;

namespace Domain.ServiceInterfaces
{
	public interface IUserService
	{
		Task<LoginResponseDTO> LoginAsync(LoginDTO model);
		Task<(bool Succeeded, string Message)> RegisterAsync(RegistrationDTO model);
		Task LogoutAsync();
	}
}
