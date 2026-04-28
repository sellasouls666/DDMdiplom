using Microsoft.AspNetCore.Identity;

namespace DDMdiplom.Services
{
    public class RussianIdentityErrorDescriber : IdentityErrorDescriber
    {
        // Базовые ошибки
        public override IdentityError DefaultError() =>
            new IdentityError { Code = nameof(DefaultError), Description = "Произошла неизвестная ошибка." };
        public override IdentityError ConcurrencyFailure() =>
            new IdentityError { Code = nameof(ConcurrencyFailure), Description = "Ошибка конкурентного доступа. Данные были изменены другим пользователем." };

        // Ошибки, связанные с пользователями и ролями
        public override IdentityError InvalidUserName(string userName) =>
            new IdentityError { Code = nameof(InvalidUserName), Description = $"Имя пользователя '{userName}' недопустимо. Используйте только буквы и цифры." };
        public override IdentityError InvalidEmail(string email) =>
            new IdentityError { Code = nameof(InvalidEmail), Description = $"Email '{email}' имеет неверный формат." };
        public override IdentityError DuplicateUserName(string userName) =>
            new IdentityError { Code = nameof(DuplicateUserName), Description = $"Имя пользователя '{userName}' уже занято." };
        public override IdentityError DuplicateEmail(string email) =>
            new IdentityError { Code = nameof(DuplicateEmail), Description = $"Email '{email}' уже используется." };
        public override IdentityError InvalidRoleName(string role) =>
            new IdentityError { Code = nameof(InvalidRoleName), Description = $"Название роли '{role}' недопустимо." };
        public override IdentityError DuplicateRoleName(string role) =>
            new IdentityError { Code = nameof(DuplicateRoleName), Description = $"Роль с именем '{role}' уже существует." };

        // Ошибки, связанные с паролями
        public override IdentityError PasswordTooShort(int length) =>
            new IdentityError { Code = nameof(PasswordTooShort), Description = $"Слабый пароль. Пожалуйста, создайте надежный пароль, соответствующий приведенным ниже требованиям." };
        public override IdentityError PasswordMismatch() =>
            new IdentityError { Code = nameof(PasswordMismatch), Description = "Неверный пароль." };
        public override IdentityError PasswordRequiresUniqueChars(int uniqueChars) =>
            new IdentityError { Code = nameof(PasswordRequiresUniqueChars), Description = $"Пароль должен содержать не менее {uniqueChars} уникальных символов." };
        public override IdentityError PasswordRequiresNonAlphanumeric() =>
            new IdentityError { Code = nameof(PasswordRequiresNonAlphanumeric), Description = "Слабый пароль. Пожалуйста, создайте надежный пароль, соответствующий приведенным ниже требованиям." };
        public override IdentityError PasswordRequiresDigit() =>
            new IdentityError { Code = nameof(PasswordRequiresDigit), Description = "Слабый пароль. Пожалуйста, создайте надежный пароль, соответствующий приведенным ниже требованиям." };
        public override IdentityError PasswordRequiresLower() =>
            new IdentityError { Code = nameof(PasswordRequiresLower), Description = "Слабый пароль. Пожалуйста, создайте надежный пароль, соответствующий приведенным ниже требованиям." };
        public override IdentityError PasswordRequiresUpper() =>
            new IdentityError { Code = nameof(PasswordRequiresUpper), Description = "Слабый пароль. Пожалуйста, создайте надежный пароль, соответствующий приведенным ниже требованиям." };

        // Ошибки для логинов, токенов и блокировок
        public override IdentityError InvalidToken() =>
            new IdentityError { Code = nameof(InvalidToken), Description = "Недействительный или просроченный токен." };
        public override IdentityError RecoveryCodeRedemptionFailed() =>
            new IdentityError { Code = nameof(RecoveryCodeRedemptionFailed), Description = "Не удалось восстановить доступ по коду." };
        public override IdentityError LoginAlreadyAssociated() =>
            new IdentityError { Code = nameof(LoginAlreadyAssociated), Description = "Этот внешний логин уже привязан к существующему аккаунту." };
        public override IdentityError UserAlreadyHasPassword() =>
            new IdentityError { Code = nameof(UserAlreadyHasPassword), Description = "У пользователя уже установлен пароль." };
        public override IdentityError UserLockoutNotEnabled() =>
            new IdentityError { Code = nameof(UserLockoutNotEnabled), Description = "Блокировка аккаунта не включена для этого пользователя." };
        public override IdentityError UserAlreadyInRole(string role) =>
            new IdentityError { Code = nameof(UserAlreadyInRole), Description = $"Пользователь уже состоит в роли '{role}'." };
        public override IdentityError UserNotInRole(string role) =>
            new IdentityError { Code = nameof(UserNotInRole), Description = $"Пользователь не состоит в роли '{role}'." };
    }
}
