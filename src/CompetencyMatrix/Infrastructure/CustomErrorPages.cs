using Microsoft.AspNetCore.Builder;

namespace CompetencyMatrix.Infrastructure
{
    public static class BuilderExtensions
    {
        public static IApplicationBuilder UseCustomErrorPages(this IApplicationBuilder app)
        {
            return app.UseMiddleware<CustomErrorPagesMiddleware>();
        }
    }
}
