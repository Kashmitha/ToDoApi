//using Microsoft.AspNetCore.Authentication.JwtBearer;
//using Microsoft.IdentityModel.Tokens;
//using System.Text;
//using Microsoft.EntityFrameworkCore;
//using ToDoApi.Data;
//using Microsoft.OpenApi.Models;

//var builder = WebApplication.CreateBuilder(args);

////JWT setup
//var jwtSettings = builder.Configuration.GetSection("Jwt");
//builder.Services.AddAuthentication(options =>
//{
//    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
//    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
//}).AddJwtBearer(options =>
//{
//    options.RequireHttpsMetadata = false; //use true in production
//    options.SaveToken = true;
//    options.TokenValidationParameters = new TokenValidationParameters
//    {
//        ValidateIssuer = true,
//        ValidateAudience = true,
//        ValidateLifetime = true,
//        ValidateIssuerSigningKey = true,
//        ValidIssuer = jwtSettings["Issuer"],
//        ValidAudience = jwtSettings["Audience"],
//        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("your-very-strong-secret-key-here"))
//    };
//});

////Allow CORS for all origins, methods, and headers (for development purposes only)
//builder.Services.AddCors(options =>
//{
//    options.AddPolicy("AllowFrontend",
//        builder => builder.WithOrigins("https://localhost:7185/swagger/index.html") // or your frontend path
//                          .AllowAnyMethod()
//                          .AllowAnyHeader());
//});



//// Add services to the container.
//builder.Services.AddControllers();
//builder.Services.AddEndpointsApiExplorer();

//builder.Services.AddSwaggerGen(options =>
//{
//    options.SwaggerDoc("v1", new OpenApiInfo { Title = "Todo API", Version = "v1" });

//    // Add JWT Authentication to Swagger
//    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
//    {
//        Name = "Authorization",
//        Type = SecuritySchemeType.ApiKey,
//        Scheme = "Bearer",
//        BearerFormat = "JWT",
//        In = ParameterLocation.Header,
//        Description = "Enter 'Bearer' [space] and then your valid token.\n\nExample: **Bearer eyJhbGci...**"
//    });

//    options.AddSecurityRequirement(new OpenApiSecurityRequirement
//    {
//        {
//            new OpenApiSecurityScheme
//            {
//                Reference = new OpenApiReference
//                {
//                    Type = ReferenceType.SecurityScheme,
//                    Id = "Bearer"
//                }
//            },
//            Array.Empty<string>()
//        }
//    });
//});


//builder.Services.AddDbContext<TodoContext>(options => 
//    options.UseSqlServer(builder.Configuration.GetConnectionString("Defaultconnection")));

//builder.Services.AddAutoMapper(typeof(Program));
////builder.Services.AddEndpointsApiExplorer();

//var app = builder.Build();

//// Configure the HTTP request pipeline.
//if (app.Environment.IsDevelopment())
//{
//    app.UseSwagger();
//    app.UseSwaggerUI();
//}

//app.UseCors("AllowFrontend");
//app.UseAuthentication();
//app.UseAuthorization();

//app.MapControllers();

//app.Run();
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.EntityFrameworkCore;
using ToDoApi.Data;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

// JWT setup
var jwtSettings = builder.Configuration.GetSection("Jwt");
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(options =>
{
    options.RequireHttpsMetadata = false; // Use true in production
    options.SaveToken = true;
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = jwtSettings["Issuer"],
        ValidAudience = jwtSettings["Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings["Key"]))
    };
});

// Allow CORS for frontend
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowFrontend", builder =>
        builder.WithOrigins("http://127.0.0.1:5500/login.html") // Update with your frontend URL
               .AllowAnyMethod()
               .AllowAnyHeader());
});

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo { Title = "Todo API", Version = "v1" });

    // Add JWT support in Swagger
    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "Enter 'Bearer' followed by your token.\nExample: Bearer abc123..."
    });

    options.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            Array.Empty<string>()
        }
    });
});

builder.Services.AddDbContext<TodoContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("Defaultconnection")));

builder.Services.AddAutoMapper(typeof(Program));

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors("AllowFrontend");

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
