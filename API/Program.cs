//ulazna tacka pr
using Infrastructure.Data;
using Core.Interfaces;
using API.Middleware;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using API.Errors;
using StackExchange.Redis;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


builder.Services.AddAuthentication(options =>{

    options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;

}).AddJwtBearer(options=>{
    options.SaveToken=true;
    options.RequireHttpsMetadata=false;
    options.TokenValidationParameters=new TokenValidationParameters(){
        ValidateIssuer=true,
        ValidateAudience=false,  //false!!!!
        ValidIssuer=builder.Configuration["JWT:ValidIssuer"],
        ValidAudience=builder.Configuration["JWT:ValidAudience"],
        IssuerSigningKey=new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["JWT:Secret"]))
    };
});
//builder.Services.AddAuthorization();
builder.Services.AddDbContext<EProdajaMuzejContext>();
builder.Services.AddSingleton<IConnectionMultiplexer>(c=>{
    var options=ConfigurationOptions.Parse(builder.Configuration.GetConnectionString("Redis"));
    return ConnectionMultiplexer.Connect(options);
});
builder.Services.AddScoped<IUlaznicaRepository, UlaznicaRepository>();
builder.Services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
/*builder.Services.AddControllers().AddJsonOptions(options =>
{
    options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.Preserve;
});*/
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
builder.Services.Configure<ApiBehaviorOptions>(options =>
{
    options.InvalidModelStateResponseFactory = actionContext =>
    {
        var errors = actionContext.ModelState.
        Where(e => e.Value.Errors.Count > 0)
        .SelectMany(x => x.Value.Errors)
        .Select(x => x.ErrorMessage).ToArray();

        var errorResponse = new ApiValidationErrorResponse
        {
            Errors = errors
        };
        return new BadRequestObjectResult(errorResponse);
    };
});
builder.Services.AddCors(c =>  
{  
    c.AddPolicy("AllowOrigin", options => options.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());  
});
var app = builder.Build();
app.UseCors(options => options.AllowAnyOrigin());
app.UseMiddleware<ExceptionMiddleware>();
// Configure the HTTP request pipeline.

app.UseStatusCodePagesWithReExecute("/errors/{0}");
if(app.Environment.IsDevelopment()){
app.UseSwagger();
app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
