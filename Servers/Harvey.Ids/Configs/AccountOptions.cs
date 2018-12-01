using System;
using System.Collections.Generic;

namespace Harvey.Ids.Configs
{
    public class AccountOptions
    {
        public static bool AllowLocalLogin = true;
        public static TimeSpan RememberMeLoginDuration = TimeSpan.FromDays(30);

        public static bool ShowLogoutPrompt = true;
        public static bool AutomaticRedirectAfterSignOut = false;

        // specify the Windows authentication scheme being used
        public static readonly string WindowsAuthenticationSchemeName = Microsoft.AspNetCore.Server.IISIntegration.IISDefaults.AuthenticationScheme;
        // if user uses windows auth, should we load the groups from windows
        public static bool IncludeWindowsGroups = false;

        public static string InvalidUser = "Invalid User";

        public static string InvalidEmailExists = "Invalid Email Exists";
        public static string InvalidCredentialsErrorMessage = "Invalid email or password";

        public static string PasswordAndConfirmPasswordNotMatch = "The password and confirmation password do not match.";
        public static string PasswordAndCurrentPasswordMatch = "New Password should not be same as old password.";
        public static string InvalidCurrentPasswordIncorrectly = "The old password is incorrectly.";
    }
}
