using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using MQTTBackend.BLLs;
using MQTTBackend.Interfaces;
using MQTTBackend.Services;

public class Startup
{
    public Startup(IConfiguration configuration)
    {
        Configuration = configuration;
    }
    public IConfiguration Configuration { get; }

    public void ConfigureServices(IServiceCollection services)
    {
        services.AddCors(options => 
        {
            options.AddPolicy("AllowSpecificOrigin",
                              builder => builder.WithOrigins("http://localhost:3000").AllowAnyHeader().AllowAnyMethod());
        });
        services.AddControllers();
        services.AddSingleton<RabbitMQService>();
        services.AddSingleton<IFakeOrderSetting, FakeOrderSetting>();
        services.AddSwaggerGen();
    }

    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
        if(env.IsDevelopment())
        {
            app.UseDeveloperExceptionPage();
        }

        app.UseHttpsRedirection();
        app.UseStaticFiles();
        app.UseRouting();
        app.UseAuthorization();
        app.UseSwagger();
        app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1"));
        app.UseCors("AllowSpecificOrigin");
        
        app.UseEndpoints(endpoints =>
        {
            endpoints.MapControllers();
        });
    }
}