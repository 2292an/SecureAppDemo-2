En consola:

docker --version

docker pull mcr.microsoft.com/mssql/server:2022-latest

docker run -e "ACCEPT_EULA=Y" \
           -e "SA_PASSWORD=Secur3app." \
           -p 1433:1433 \
           --name sql-auth-demo \
           -d mcr.microsoft.com/mssql/server:2022-latest

docker ps

Instalar dotnet ef

dotnet tool install --global dotnet-ef

En consola desde el proyecto:

dotnet ef migrations add InitialCreate --output-dir Data/Migrations
dotnet ef database update

docker exec -it  sql-auth-demo /opt/mssql-tools18/bin/sqlcmd \
   -S localhost -U sa -P "Secur3app." -C

use AuthDemoDb
go

select * from dbo.user
go

Password must contain at least 8 characters, one uppercase, one lowercase, one number and one special character.