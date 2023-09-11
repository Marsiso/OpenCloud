using FluentValidation;
using OpenCloud.Domain.Validators;

namespace OpenCloud.Core.Commands.Users;

public class UpdateUserCommandValidator : AbstractValidator<UpdateUserCommand>
{
	public UpdateUserCommandValidator()
	{
		RuleFor(user => user.UserID)
			.GreaterThan(0)
			.WithMessage("Primární klíč uživatele je požadován.");

		RuleFor(user => user.Identifier)
			.NotEmpty()
			.WithMessage("Identifikátor uživatele je požadován.")
			.MaximumLength(256)
			.WithMessage("Identifikátor uživatele musí obsahovat nejvýše 512 znaků.");

		RuleFor(user => user.FirstName)
			.NotEmpty()
			.WithMessage("Jméno uživatele je požadováno.")
			.MaximumLength(256)
			.WithMessage("Jméno uživatele musí obsahovat nejvýše 255 znaků.");

		RuleFor(user => user.LastName)
			.NotEmpty()
			.WithMessage("Přijmení uživatele je požadováno.")
			.MaximumLength(256)
			.WithMessage("Příjmení uživatele musí obsahovat nejvýše 255 znaků.");

		When(user => !string.IsNullOrEmpty(user.ProfilePhotoUrl), () =>
		{
			RuleFor(user => user.ProfilePhotoUrl)
				.NotEmpty()
				.WithMessage("Odkaz na profilový obrázek uživatele nemůže být tvořen prázdným řetězcem znaků či mezerami.")
				.URL()
				.WithMessage("Odkaz na profilový obrázek uživatele má nesprávný formát.")
				.MaximumLength(2048)
				.WithMessage("Odkaz na profilový obrázek uživatele musí obsahovat nejvýše 2048 znaků.");
		});

		RuleFor(user => user.Email)
			.NotEmpty()
			.WithMessage("Emailová adresa uživatele je požadována.")
			.EmailAddress()
			.WithMessage("Emailová adresá uživatele má nesprávný formát.")
			.MaximumLength(256)
			.WithMessage("Emailová adresá uživatele musí obsahovat nejvýše 255 znaků.");
	}
}