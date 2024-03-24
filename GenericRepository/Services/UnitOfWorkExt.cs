using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace GenericRepository.Services
{
    public class UnitOfWorkExt<TDBContext> : UnitOfWork<TDBContext>, IUnitOfWorkExt<TDBContext>
        where TDBContext : DbContext, new()
    {
        public UnitOfWorkExt(TDBContext dBContext) : base(dBContext)
        {
           
        }

        
    }
}
