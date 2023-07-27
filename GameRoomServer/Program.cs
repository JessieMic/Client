using GameRoomServer;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSignalR();
builder.Logging.AddConsole();
var app = builder.Build();

//LiteNetServer liteNetServer = new LiteNetServer(5555);
//Task.Run(() => liteNetServer.Run());
app.UseHttpsRedirection();


app.UseAuthorization();

app.MapControllers();
app.MapHub<GameHub>("/GameHub");
app.MapHub<InGameHub>("/InGameHub");

app.Run();
