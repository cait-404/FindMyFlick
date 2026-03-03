var builder = DistributedApplication.CreateBuilder(args);

builder.AddProject<Projects.FindMyFlickWebsite_Server>("findmyflickwebsite-server");

builder.Build().Run();
