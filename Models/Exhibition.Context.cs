﻿//------------------------------------------------------------------------------
// <auto-generated>
//     這個程式碼是由範本產生。
//
//     對這個檔案進行手動變更可能導致您的應用程式產生未預期的行為。
//     如果重新產生程式碼，將會覆寫對這個檔案的手動變更。
// </auto-generated>
//------------------------------------------------------------------------------

namespace EXhibition.Models
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    
    public partial class ExhibitionEntities : DbContext
    {
        public ExhibitionEntities()
            : base("name=ExhibitionEntities")
        {
            Database.Connection.ConnectionString = Environment.GetEnvironmentVariable("SQL_CONNECTSTRING");
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<events> events { get; set; }
        public virtual DbSet<exhibitinfo> exhibitinfo { get; set; }
        public virtual DbSet<exhibitors> exhibitors { get; set; }
        public virtual DbSet<hosts> hosts { get; set; }
        public virtual DbSet<tickets> tickets { get; set; }
        public virtual DbSet<users> users { get; set; }
    }
}