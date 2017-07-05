using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure.Internal;

namespace CompetencyMatrix.Models
{
    public partial class CompetencyMatrixContext
    {
        private readonly string _connectionString;
        public CompetencyMatrixContext()
        {
        }

        public CompetencyMatrixContext(DbContextOptions<CompetencyMatrixContext> options) : base(options)
        {
            var extension = options.FindExtension<SqlServerOptionsExtension>();
            _connectionString = extension.ConnectionString;
        }
    }
}
