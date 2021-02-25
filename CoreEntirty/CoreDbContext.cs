using CoreEntirty.Extensions;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace CoreEntirty
{
   public class CoreDbContext: DbContext
    {
        private Action<DbContextOptionsBuilder> _options;

        public CoreDbContext(Action<DbContextOptionsBuilder> options)
        {
            _options = options;
        }
        public CoreDbContext(DbContextOptions<CoreDbContext> dbContextOptions):base(dbContextOptions)
        { 
        
        }
        /// <summary>
        /// OnConfiguring
        /// </summary>
        /// <param name="optionsBuilder"></param>
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            _options?.Invoke(optionsBuilder);
        }
        /// <summary>
        /// 将对应的数据模型转为ef模型实体
        /// </summary>
        /// <param name="modelBuilder"></param>
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            var entityTypes = AssemblyHelper
                                .GetTypesFromAssembly()
                                .Where(type =>
                                    !type.Namespace.IsNullOrWhiteSpace() &&
                                    type.GetTypeInfo().IsClass &&
                                    type.GetTypeInfo().BaseType != null &&
                                    type != typeof(BaseEntity) &&
                                    typeof(BaseEntity).IsAssignableFrom(type));

            if (entityTypes?.Count() > 0)
            {
                foreach (var entityType in entityTypes)
                {
                    if (modelBuilder.Model.FindEntityType(entityType) != null)
                        continue;
                    modelBuilder.Model.AddEntityType(entityType);
                }
            }
        }
        //public DbSet<Tb_User> Tb_User { get; set; }
    }
}
