using GameRoomServer;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
//builder.Services.AddSwaggerGen();
builder.Services.AddSignalR();
builder.Logging.AddConsole();
var app = builder.Build();

// Configure the HTTP request pipeline.
//if (app.Environment.IsDevelopment())
//{
//    app.UseSwagger();
//    app.UseSwaggerUI();
//}
LiteNetServer liteNetServer = new LiteNetServer(5555);
Task.Run(() => liteNetServer.Run());
//new Thread(liteNetServer.Run).Start();
app.UseHttpsRedirection();


app.UseAuthorization();

app.MapControllers();
app.MapHub<GameHub>("/GameHub");

app.Run();
