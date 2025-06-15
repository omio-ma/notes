using Notes.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Notes.Application.Interfaces;
using Notes.Application.Services;
using Notes.Domain.Interfaces;
using Notes.Infrastructure.Repositories;
using FluentValidation.AspNetCore;
using Notes.API.Validators;
using FluentValidation;

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

            app.UseAuthorization();
            app.MapControllers();

            app.Run();
        }
    }
}
