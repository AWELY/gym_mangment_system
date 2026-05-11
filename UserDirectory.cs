using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace gym_mangment_system
{
    public sealed class UserDirectoryEntry
    {
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
    /// In-memory user list shared between login and Users management screen.
    /// </summary>
    public static class UserDirectory
    {
        private static readonly List<UserDirectoryEntry> _accounts = new List<UserDirectoryEntry>
        {
            new UserDirectoryEntry { Id = 1, FullName = "المدير العام", Username = "admin", Password = "admin", Role = AppSession.UserRole.Admin },
            new UserDirectoryEntry { Id = 2, FullName = "مستلم النظام", Username = "reception", Password = "1234", Role = AppSession.UserRole.Receptionist },
        };

        public static bool TryAuthenticate(string username, string password, out string displayName, out AppSession.UserRole role)
        {
            displayName = "";
            role = AppSession.UserRole.Receptionist;
            if (string.IsNullOrWhiteSpace(username)) return false;

            var u = username.Trim();
            var p = password ?? "";

            foreach (var a in _accounts)
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
            foreach (var a in _accounts.OrderBy(x => x.Id))
            {
                dt.Rows.Add(a.Id, a.FullName, a.Username, UserDirectoryEntry.RoleToDisplay(a.Role));
            }
        }

        public static bool UsernameExists(string username, int? exceptId)
        {
            return _accounts.Any(a =>
                a.Username.Equals(username.Trim(), StringComparison.OrdinalIgnoreCase)
                && (!exceptId.HasValue || a.Id != exceptId.Value));
        }

        public static void Add(string fullName, string username, string password, AppSession.UserRole role)
        {
            int id = _accounts.Count == 0 ? 1 : _accounts.Max(x => x.Id) + 1;
            _accounts.Add(new UserDirectoryEntry
            {
                Id = id,
                FullName = fullName?.Trim() ?? "",
                Username = username.Trim(),
                Password = password ?? "",
                Role = role
            });
        }

        public static void Update(int id, string fullName, string username, string passwordOrNullKeep, AppSession.UserRole role)
        {
            var a = _accounts.FirstOrDefault(x => x.Id == id);
            if (a == null) return;
            if (a.Username.Equals("admin", StringComparison.OrdinalIgnoreCase) && role != AppSession.UserRole.Admin)
                role = AppSession.UserRole.Admin;

            a.FullName = fullName?.Trim() ?? "";
            a.Username = username.Trim();
            if (!string.IsNullOrEmpty(passwordOrNullKeep))
                a.Password = passwordOrNullKeep;
            a.Role = role;
        }

        public static bool Remove(int id)
        {
            var a = _accounts.FirstOrDefault(x => x.Id == id);
            if (a == null) return false;
            if (a.Username.Equals("admin", StringComparison.OrdinalIgnoreCase)) return false;
            return _accounts.Remove(a);
        }
    }
}
