namespace Haskap.Recipe.Application.Dtos.Accounts;

public class ChangePasswordInputDto
{
    public string CurrentPassword { get; set; }
    public string NewPassword { get; set; }
    public string NewPasswordConfirmation { get; set; }
}