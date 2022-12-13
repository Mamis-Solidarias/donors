using MamisSolidarias.WebAPI.Donors.StartUp;

var builder = WebApplication.CreateBuilder(args);
ServiceRegistrar.Register(builder);

var app = builder.Build();
MiddlewareRegistrator.Register(app);

app.Run();