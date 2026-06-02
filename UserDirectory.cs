using System;
using System.Collections.Generic;
using System.Data;

namespace gym_mangment_system
{
    public sealed class UserDirectoryEntry
    {
        public UserDirectoryEntry() { }

        public int Id { get; set; }
        public string FullName { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public AppSession.UserRole Role { get; set; }

        public static string RoleToDisplay(AppSession.UserRole role)
        {
            return role == AppSession.UserRole.Admin
                ? "Admin  (مدير)"
                : "Recipient  (مستلم)";
        }

        public static AppSession.UserRole RoleFromDisplay(string display)
        {
            if (string.IsNullOrEmpty(display)) return AppSession.UserRole.Receptionist;
            return display.StartsWith("Admin", StringComparison.OrdinalIgnoreCase)
                ? AppSession.UserRole.Admin
                : AppSession.UserRole.Receptionist;
        }
    }

    /// <summary>
    /// Users list persisted via <see cref="GymDataStore"/>.
    /// </summary>
    public static class UserDirectory
    {
        private static List<UserDirectoryEntry> Accounts => GymDataStore.Data.Users;

        public static bool TryAuthenticate(string username, string password, out string displayName, out AppSession.UserRole role)
        {
            displayName = "";
            role = AppSession.UserRole.Receptionist;
            if (string.IsNullOrWhiteSpace(username)) return false;

            var u = username.Trim();
            var p = password ?? "";

            foreach (var a in Accounts)
            {
                if (a.Username.Equals(u, StringComparison.OrdinalIgnoreCase) && a.Password == p)
                {
                    displayName = string.IsNullOrWhiteSpace(a.FullName) ? a.Username : a.FullName;
                    role = a.Role;
                    return true;
                }
            }
            return false;
        }

        public static void FillDataTable(DataTable dt)
        {
            dt.Rows.Clear();
            var sorted = new List<UserDirectoryEntry>(Accounts);
            sorted.Sort((a, b) => a.Id.CompareTo(b.Id));
            foreach (var a in sorted)
            {
                dt.Rows.Add(a.Id, a.FullName, a.Username, UserDirectoryEntry.RoleToDisplay(a.Role));
            }
        }

        public static bool UsernameExists(string username, int? exceptId)
        {
            string u = username.Trim();
            foreach (var a in Accounts)
                if (a.Username.Equals(u, StringComparison.OrdinalIgnoreCase)
                    && (!exceptId.HasValue || a.Id != exceptId.Value))
                    return true;
            return false;
        }

        public static void Add(string fullName, string username, string password, AppSession.UserRole role)
        {
            int max = 0;
            foreach (var x in Accounts)
                if (x.Id > max) max = x.Id;
            int id = max + 1;
            Accounts.Add(new UserDirectoryEntry
            {
                Id = id,
                FullName = fullName?.Trim() ?? "",
                Username = username.Trim(),
                Password = password ?? "",
                Role = role
            });
            GymDataStore.Save();
        }

        public static void Update(int id, string fullName, string username, string passwordOrNullKeep, AppSession.UserRole role)
        {
            UserDirectoryEntry a = null;
            foreach (var x in Accounts)
                if (x.Id == id) { a = x; break; }
            if (a == null) return;
            if (a.Username.Equals("admin", StringComparison.OrdinalIgnoreCase) && role != AppSession.UserRole.Admin)
                role = AppSession.UserRole.Admin;

            a.FullName = fullName?.Trim() ?? "";
            a.Username = username.Trim();
            if (!string.IsNullOrEmpty(passwordOrNullKeep))
                a.Password = passwordOrNullKeep;
            a.Role = role;
            GymDataStore.Save();
        }

        public static bool Remove(int id)
        {
            UserDirectoryEntry a = null;
            foreach (var x in Accounts)
                if (x.Id == id) { a = x; break; }
            if (a == null) return false;
            if (a.Username.Equals("admin", StringComparison.OrdinalIgnoreCase)) return false;
            bool ok = Accounts.Remove(a);
            if (ok) GymDataStore.Save();
            return ok;
        }
    }
}
