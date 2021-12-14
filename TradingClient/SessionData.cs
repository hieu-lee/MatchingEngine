namespace TradingClient
{
    public static class SessionData
    {
        public static User CurrentUser;
        public static string UserId => CurrentUser.Id;
        public static string UserEmail => CurrentUser.Email;

        public static void UpdateCurrentUser(User user)
        {
            CurrentUser = user;
        }
    }
}
