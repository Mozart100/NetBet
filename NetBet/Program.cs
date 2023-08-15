using NetBet.Converters;
using NetBet.Startup;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.


builder.Services.NativeServiceRegistration();
builder.Services.CustomServiceRegistration();

// Add the DateOnlyJsonConverter to the JsonSerializerOptions
builder.Services.AddControllers().AddJsonOptions(options =>
{
    options.JsonSerializerOptions.Converters.Add(new DateOnlyJsonConverter());
});


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
