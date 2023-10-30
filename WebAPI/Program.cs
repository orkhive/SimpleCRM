using SimpleCRM.DI;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers(options =>
{
    //options.InputFormatters //Set a list of input formatters, the first is the default
    //options.OutputFormatters //Set a list of output formatters, the first is the default
    options.ReturnHttpNotAcceptable = true; //Returns a 406 If the Accept header isn't supported
})
.AddNewtonsoftJson() //Overwrites the default Json Seralize/Deserialize logic - Newtonsoft is faster
.AddXmlDataContractSerializerFormatters(); //Adds XML Input & Output Formatting

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var di = new DependencyBuilder(builder.Configuration);
di.Build(builder.Services);

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
