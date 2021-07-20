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
            var assembly = Assembly.GetExecutingAssembly();
            foreach (Type type in assembly.ExportedTypes)
            {
                if (type.IsClass && type != typeof(BaseEntity) && typeof(BaseEntity).IsAssignableFrom(type))
                {
                    var method = modelBuilder.GetType().GetMethods().Where(x => x.Name == "Entity").FirstOrDefault();

                    if (method != null)
                    {
                        method = method.MakeGenericMethod(new Type[] { type });
                        method.Invoke(modelBuilder, null);
                    }
                }
            }

            base.OnModelCreating(modelBuilder);
        }
        //public DbSet<Tb_User> Tb_User { get; set; }
    }
}
