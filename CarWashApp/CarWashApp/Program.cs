using CarWashApp;
using CarWashApp.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Reflection;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddDbContext<ApplicationDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"), 
        sqlServer => sqlServer.UseNetTopologySuite());
});

builder.Services.AddCors();

builder.Services.AddAutoMapper(typeof(Program));
builder.Services.AddIdentity<IdentityUser, IdentityRole>()
        .AddEntityFrameworkStores<ApplicationDbContext>()
        .AddDefaultTokenProviders();

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options => options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = false,
        ValidateAudience = false,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["jwt:key"])),
        ClockSkew = TimeSpan.Zero
    });

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("OwnerAccess", policy =>
         policy.RequireAssertion(context =>
                     context.User.IsInRole("Admin")
                     || context.User.IsInRole("Owner")));

    options.AddPolicy("CustomerAccess", policy =>
         policy.RequireAssertion(context =>
                     context.User.IsInRole("Admin")
                     || context.User.IsInRole("Customer")));

    options.AddPolicy("UserAccess", policy =>
         policy.RequireAssertion(context =>
                     context.User.IsInRole("Owner")
                     || context.User.IsInRole("Customer")));
});

builder.Services.AddTransient<IHostedService, UpdateRevenuesService>();
builder.Services.AddTransient<IHostedService, UpdateAppointmentsService>();

//builder.Services.AddTransient<IHostedService, UpdateCarWashService>();

builder.Services.AddControllers().AddNewtonsoftJson();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c => {
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme()
    {
        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = $"JWT Authorization header using the Bearer scheme. Enter 'Bearer' [space] and then your token in the text input below.",
    });
    c.AddSecurityRequirement(new OpenApiSecurityRequirement {
        {
            new OpenApiSecurityScheme {
                Reference = new OpenApiReference {
                    Type = ReferenceType.SecurityScheme,
                        Id = "Bearer"
                }
            },
            new string[] {}
        }
    });

    var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
    c.IncludeXmlComments(xmlPath);
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseStaticFiles();

app.UseHttpsRedirection();

app.UseAuthentication();

app.UseAuthorization();

app.UseCors(builder => builder.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader().WithExposedHeaders("totalAmountPages"));

app.MapControllers();

app.Run();