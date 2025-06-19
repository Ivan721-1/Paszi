using Microsoft.EntityFrameworkCore;
using RoleAuthenticationApp.Data;
using RoleAuthenticationApp.Models;

namespace RoleAuthenticationApp
{
    public class DatabaseHelper
    {
        public void InitializeDatabase()
        {
            using var context = new RoleAuthDbContext();

            // Создаем базу данных если её нет
            context.Database.EnsureCreated();

            // Или используем миграции (рекомендуется)
            // context.Database.Migrate();
        }

        public void AddRole(string roleName, string usbSerial, byte[] faceData)
        {
            using var context = new RoleAuthDbContext();

            // Проверяем, существует ли уже роль с таким USB-ключом
            var existingRole = context.Roles.FirstOrDefault(r => r.USBSerial == usbSerial);
            if (existingRole != null)
            {
                throw new InvalidOperationException($"Роль с USB-ключом '{usbSerial}' уже существует: '{existingRole.RoleName}'. Удалите существующую роль или используйте другой USB-ключ.");
            }

            var role = new Role
            {
                RoleName = roleName,
                USBSerial = usbSerial,
                FaceData = faceData,
                CreatedAt = DateTime.Now
            };

            context.Roles.Add(role);
            context.SaveChanges();
        }

        public Tuple<string, byte[]> GetRoleByUsbSerial(string usbSerial)
        {
            using var context = new RoleAuthDbContext();

            var role = context.Roles
                .FirstOrDefault(r => r.USBSerial == usbSerial);

            if (role != null)
            {
                // Обновляем время последнего использования
                role.LastUsed = DateTime.Now;
                context.SaveChanges();

                return Tuple.Create(role.RoleName, role.FaceData);
            }

            return Tuple.Create<string, byte[]>(null, null);
        }

        public List<Role> GetAllRoles()
        {
            using var context = new RoleAuthDbContext();
            return context.Roles.OrderBy(r => r.RoleName).ToList();
        }

        public bool DeleteRole(int roleId)
        {
            using var context = new RoleAuthDbContext();

            var role = context.Roles.Find(roleId);
            if (role != null)
            {
                context.Roles.Remove(role);
                context.SaveChanges();
                return true;
            }
            return false;
        }
    }
}
