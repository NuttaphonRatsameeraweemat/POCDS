using DS.Data.Repository.EF;
using System;
using System.Collections.Generic;
using System.Text;

namespace DS.Data
{
    public class DSUnitOfWork : EfUnitOfWork<DSDBContext>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PtUnitOfWork" /> class.
        /// </summary>
        /// <param name="dsDbContext">The Digital Signature database context what inherits from DbContext of EF.</param>
        public DSUnitOfWork(DSDBContext dsDbContext) : base(dsDbContext)
        { }
    }
}
