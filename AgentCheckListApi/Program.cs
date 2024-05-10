using AgentCheckListApi.Helper;
using AgentCheckListApi.Interfaces;
using AgentCheckListApi.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();

builder.Services.Configure<MongoDb>(builder.Configuration.GetSection("MongoDb"));
//        services.AddSingleton(typeof(MongoDbService<>));
builder.Services.AddSingleton(typeof(MongoDbService<>));
builder.Services.AddSingleton<IRegisterationService , RegisteratoinService >();
builder.Services.AddSingleton<IInspectionService , InspectionService >();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

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

app.Run();
