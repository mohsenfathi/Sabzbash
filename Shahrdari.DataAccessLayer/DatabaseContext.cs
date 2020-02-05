using Shahrdari.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shahrdari.DataAccessLayer
{
    /// <summary>
    /// لایه داده
    /// </summary>
    public class DatabaseContext : DbContext
    {
        /// <summary>
        /// متد سازنده
        /// </summary>
        public DatabaseContext()
        {
            // Database.SetInitializer(new MigrateDatabaseToLatestVersion<DatabaseContext, Configuration>());
            var ensureDLLIsCopied = System.Data.Entity.SqlServer.SqlProviderServices.Instance;
        }

        /// <summary>
        /// لاگ های سیستم
        /// </summary>
        public DbSet<M_Logs> Logs { get; set; }

        /// <summary>
        /// نقش های کاربران
        /// </summary>
        public DbSet<M_PersonelRoles> PersonelRoles { get; set; }

        /// <summary>
        /// مقدارهای مربوط به نقش کاربر
        /// </summary>
        public DbSet<M_PersonelRoleValues> PersonelRoleValues { get; set; }

        /// <summary>
        /// پرسنل
        /// </summary>
        public DbSet<M_Personels> Personels { get; set; }

        /// <summary>
        /// کاربران سیستم
        /// </summary>
        public DbSet<M_Users> Users { get; set; }

        /// <summary>
        /// مغادیر لوکاپ
        /// </summary>
        public DbSet<M_PublicCategory> PublicCategory { get; set; }

        /// <summary>
        /// دسته بندی ها
        /// </summary>
        public DbSet<M_ServicesCategories> ServicesCategories { get; set; }

        /// <summary>
        /// درخواست ها
        /// </summary>
        public DbSet<M_ServicesRequests> ServicesRequests { get; set; }

        /// <summary>
        /// آموزش ها
        /// </summary>
        public DbSet<M_Learns> Learns { get; set; }

        /// <summary>
        /// آدرس ها
        /// </summary>
        public DbSet<M_Addresses> Addresses { get; set; }

        /// <summary>
        /// کدهای فعالسازی
        /// </summary>
        public DbSet<M_SmsAuthorise> SmsAuthorise { get; set; }

        /// <summary>
        /// غرفه
        /// </summary>
        public DbSet<M_BoothInfo> BoothInfo { get; set; }

        /// <summary>
        /// خودرو
        /// </summary>
        public DbSet<M_CarInfo> CarInfo { get; set; }

        public DbSet<M_Pouriya_Date> Pouriya_Date { get; set; }

        /// <summary>
        /// موارد مربوط به درخواست
        /// </summary>
        public DbSet<M_ServicesRequestItems> ServicesRequestItems { get; set; }

        /// <summary>
        /// نظرات
        /// </summary>
        public DbSet<M_CommentItems> CommentItems { get; set; }
        
        /// <summary>
        /// پیام ها
        /// </summary>
        public DbSet<M_Messages> Messages { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public DbSet<M_UserPayment> UserPayment { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public DbSet<M_Location> Location { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public DbSet<M_Accounts> Accounts { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public DbSet<M_ListenEar> ListenEar { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public DbSet<M_News> News { get; set; }
    }
}
