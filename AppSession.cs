namespace gym_mangment_system
{
    /// <summary>
    /// Holds the current logged-in user's session data across the application.
    /// </summary>
    public static class AppSession
    {
        public enum UserRole { Admin, Receptionist }

        public static UserRole CurrentRole { get; set; } = UserRole.Admin;
        public static string   Username    { get; set; } = "المدير";

        public static bool IsAdmin        => CurrentRole == UserRole.Admin;
        public static bool IsReceptionist => CurrentRole == UserRole.Receptionist;
    }
}
