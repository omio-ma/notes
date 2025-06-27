using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.EntityFrameworkCore;
using Notes.Application.Interfaces;
using Notes.Application.Services;
using Notes.API.Validators;
using Notes.Domain.Interfaces;
using Notes.Infrastructure.Persistence;
using Notes.Infrastructure.Repositories;

namespace Notes.API
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container
            builder.Services.AddControllers();
            builder.Services.AddEndpointsApiExplorer();

            // Add CORS
            var allowedOrigins = builder.Configuration.GetSection("AllowedOrigins").Get<string[]>();
            builder.Services.AddCors(options =>
            {
                options.AddPolicy("AllowFrontend", policy =>
                {
                    policy.WithOrigins(allowedOrigins ?? [])
                        .AllowAnyHeader()
                        .AllowAnyMethod();
                });
            });

            builder.Services.AddScoped<INoteRepository, NoteRepository>();
            builder.Services.AddScoped<INoteService, NoteService>();

            builder.Services.AddDbContext<NotesDbContext>(options =>
                options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

            builder.Services
                .AddValidatorsFromAssemblyContaining<NoteRequestValidator>()
                .AddValidatorsFromAssemblyContaining<PatchNoteRequestValidator>();

            builder.Services
                .AddFluentValidationAutoValidation()
                .AddFluentValidationClientsideAdapters();


            var app = builder.Build();

            app.UseCors("AllowFrontend");
            app.UseAuthorization();
            app.MapControllers();

            app.Run();
        }
    }
}