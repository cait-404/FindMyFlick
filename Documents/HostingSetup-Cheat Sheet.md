For hosting set up:
1. Create a resource group/logical container for everything
		Name: FindMyFlick-app
		Region: East US
2. Search for "Azure Database for postgreSQL", select "Flexible Server" - allow access for Azure services
3. Search "App Service" > Create
4. In Program.cs, should need to add something like: 
app.UseDefaultFiles();
app.UseStaticFiles();

app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.MapFallbackToFile("index.html"); // for React Router

5. In package.json add a build script to direct outputs to the proper spots.
6. Create a GitHub Actions workflow - should look something like this:
	name: Build and Deploy to Azure

on:
  push:
    branches: [ main ]

env:
  AZURE_WEBAPP_NAME: your-app-service-name   # change this
  DOTNET_VERSION: '8.0.x'                    # change to your .NET version
  NODE_VERSION: '20.x'

jobs:
  build-and-deploy:
    runs-on: ubuntu-latest

    steps:
    - name: Checkout code
      uses: actions/checkout@v4

    # --- Build React frontend ---
    - name: Set up Node.js
      uses: actions/setup-node@v4
      with:
        node-version: ${{ env.NODE_VERSION }}
        cache: 'npm'
        cache-dependency-path: frontend/package-lock.json  # adjust path to your React folder

    - name: Install React dependencies
      run: npm ci
      working-directory: ./frontend   # adjust to your React folder name

    - name: Build React app
      run: npm run build
      working-directory: ./frontend

    # Copy React build output into ASP.NET wwwroot
    - name: Copy React build to wwwroot
      run: |
        mkdir -p ./backend/wwwroot    # adjust to your ASP.NET folder name
        cp -r ./frontend/build/* ./backend/wwwroot/

    # --- Build and publish ASP.NET backend ---
    - name: Set up .NET
      uses: actions/setup-dotnet@v4
      with:
        dotnet-version: ${{ env.DOTNET_VERSION }}

    - name: Build
      run: dotnet build --configuration Release
      working-directory: ./backend

    - name: Publish
      run: dotnet publish --configuration Release -o ./publish
      working-directory: ./backend

    # --- Deploy to Azure ---
    - name: Deploy to Azure App Service
      uses: azure/webapps-deploy@v3
      with:
        app-name: ${{ env.AZURE_WEBAPP_NAME }}
        publish-profile: ${{ secrets.AZURE_WEBAPP_PUBLISH_PROFILE }}
        package: ./backend/publish
		
7. If needed, add a startup migration call in program.cs
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
    db.Database.Migrate();
}