namespace Empower.DTO
{
    public static class ExceptionMessages
    {
        #region Common Messages
        public static string NoResults = "[]";
        public static string Successful = "Successful";
        #endregion
        #region Email Address
        public static string EmailIsRequired = "Email address is required";
        public static string InValidEmailAddress = "Invalid email address";
        public static string PasswordIsRequired = "Password is required";
        #endregion
               
        #region User
        public static string ActiveDirectoryIdInValid = "Invalid data, please try again";
        public static string UserTokenIsRequired = "User token is required";
        public static string UserIdIsRequired = "User id is required";
        public static string InvalidUser = "Invalid username or password";
        public static string UserIsDeleted = "User is already deleted";
        public static string UserDoesNotExists = "User is does not exists";
        public static string UsersDoesNotExists = "Users does not exists";
        public static string SomeUsersDoesNotExists = "Some users does not exists";
        public static string UserEmailOrPasswordDoesNotMatch = "Login incorrect. Please enter a valid Email Address/Password";
        #endregion

        #region Token
        public static string TokenExpirationDateIsRequired = "Token expiration date is required";
        public static string TokenDoesNotExists = "Token does not exists";
        public static string TokenIsExpired = "Token is expired";
        public static string TokenIsDeleted = "Token is already deleted";
        public static string UnAuthorizedAccess = "You are unauthorized to access this resource";     
        #endregion

    }
}