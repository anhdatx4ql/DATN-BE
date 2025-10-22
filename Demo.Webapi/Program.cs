
using Demo.Webapi.BLayer;
using Demo.Webapi.BLayer.BaseBL;
using Demo.Webapi.Common.Entities;
using Demo.Webapi.DL;
using Demo.Webapi.DL.BaseDL;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddControllers().AddJsonOptions(options =>
{
    options.JsonSerializerOptions.PropertyNamingPolicy = null;
}
);
builder.Services.AddCors(p => p.AddPolicy("MyCors", build =>
{
    build.WithOrigins("*").AllowAnyMethod().AllowAnyHeader();
}));

builder.Services.AddScoped<IEmployeeBL, EmployeeBL>();
builder.Services.AddScoped<IEmployeeDL, EmployeeDL>();
builder.Services.AddScoped<IAccountBL, AccountBL>();
builder.Services.AddScoped<IAccountDL, AccountDL>();
builder.Services.AddScoped(typeof(IBaseDL<>), typeof(BaseDL<>));
builder.Services.AddScoped(typeof(IBaseBL<>), typeof(BaseBL<>));
builder.Services.AddScoped<IReceiptPaymentDetailBL, ReceiptPaymentDetailBL>();
builder.Services.AddScoped<IReceiptPaymentDetailDL, ReceiptPaymentDetailDL>();
builder.Services.AddScoped<IReceiptPaymentBL, ReceiptPaymentBL>();
builder.Services.AddScoped<IReceiptPaymentDL, ReceiptPaymentDL>();

builder.Services.AddScoped<IAssetDL, AssetDL>();
builder.Services.AddScoped<IAssetBL, AssetBL>();
// Đảm bảo closed-generic resolution cho Asset sử dụng AssetBL
builder.Services.AddScoped<IBaseBL<Asset>, AssetBL>();

DatabaseContext.connectionString = builder.Configuration.GetConnectionString("PostgreSqlProduction");

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.UseCors("MyCors");

app.Run();
