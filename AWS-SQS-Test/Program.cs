using Amazon.SQS;
using Amazon;
using DotNetEnv;

namespace AWS_SQS_Test
{
    public class Program
    {
        public static void Main(string[] args)
        {
            Env.TraversePath().Load();

            var builder = WebApplication.CreateBuilder(args);
            ApplyAwsCredentialsFromConfig(builder.Configuration);

            // Add services to the container.
            builder.Services.AddSingleton<IAmazonSQS>(_ => new AmazonSQSClient(RegionEndpoint.EUNorth1));
            builder.Services.AddControllers();
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
        }

        private static void ApplyAwsCredentialsFromConfig(IConfiguration config)
        {
            var accessKey = config["AWS_ACCESS_KEY_ID"];
            var secretKey = config["AWS_SECRET_ACCESS_KEY"];
            var region = config["AWS_REGION"];
            if (!string.IsNullOrEmpty(accessKey)) Environment.SetEnvironmentVariable("AWS_ACCESS_KEY_ID", accessKey);
            if (!string.IsNullOrEmpty(secretKey)) Environment.SetEnvironmentVariable("AWS_SECRET_ACCESS_KEY", secretKey);
            if (!string.IsNullOrEmpty(region)) Environment.SetEnvironmentVariable("AWS_REGION", region);
        }
    }
}
