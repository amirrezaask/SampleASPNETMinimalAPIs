WebApplication
    .CreateBuilder(args)
    .WithConfigurations()
    .WithAuthentication() // Adds Authentication and Authorization to DI
    .WithSwagger() // Adds Swagger stuff to DI
    .WithDB() // Adds Database to DI
    .Build() // Build the DI tree
    .ConfigurePipeline() // Registers all middlewares and configures request pipeline.
    .MapAPIs() // Map my Application APIs
    .Run(); // Run Server :)
