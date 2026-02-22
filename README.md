# Asegurando un API

La solución de este documento pretende presentar buenas prácticas para poder asegurar un API adecuadamente, con el fin de cubrir exitosamente las más comunes y mayores amenazas según OWASP.

Se hizo integración con una base de datos en sql server corriendo en un docker y autenticación con JWT respecto a la configuración dentro de la misma base de datos.

## 1. Instalar dotnet

Se debe de proceder a instalar dotnet, no se da el paso a paso porque depende de cada computadora.

## 2. Instalar instancia de sql server

En consola:

docker --version

docker pull mcr.microsoft.com/mssql/server:2022-latest

docker run -e "ACCEPT_EULA=Y" \
 -e "SA_PASSWORD=Secur3app." \
 -p 1433:1433 \
 --name sql-auth-demo \
 -d mcr.microsoft.com/mssql/server:2022-latest

El comando anterior puede fallar dependiendo de la máquina que se esté usando y el chip (Intel vs M1). Por lo que se debe de ajustar.

docker ps

## 3. Probar acceso a base de datos

docker exec -it sql-auth-demo /opt/mssql-tools18/bin/sqlcmd \
 -S localhost -U sa -P "Secur3app." -C

```
use AuthDemoDb
go
```

```
select * from dbo.[user]
go
```

## 4. Instalar dotnet ef

Instalar dotnet ef

dotnet tool install --global dotnet-ef

# 5. Crear appsettings.local.json

Este archivo se crea a nivel raíz donde se encuentra appsettings.json. Su propósito es poder manejar la configuración sin necesitar hacer commit al repositorio.

Usar template como este para el demo actual:

```
{
  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "Microsoft.AspNetCore": "Warning"
    }
  },
   "Jwt": {
    "Issuer": "mi-api",
    "Audience": "mi-api-client",
    "Secret": "9f4c2a7d8b1e5f6a3c9d0e2f7a1b8c6d4e9f0a2b7c1d5e8f3a6b9c2d1e4f7a8",
    "ExpirationMinutes": 60
  },
  "ConnectionStrings": {
    "DefaultConnection": "Server=localhost,1433;Database=AuthDemoDb;User Id=sa;Password=Secur3app.;TrustServerCertificate=True;"
  },
  "SeedData": {
    "DefaultAdminPwd": "Admin123!"
  }
}
```

## 6. Correr la aplicación

En consola desde el proyecto:

dotnet run

## 7. Ejemplo de autorización

Request en formato curl:

```
curl --location 'http://localhost:5202/api/authorization/authorize' \
--header 'Content-Type: application/json' \
--data '{
    "username": "admin",
    "password": "Admin123!"
}'
```

Esto genera el token de autorización necesario para búsqueda de usuarios (permitido a ambos roles)

## 8. Buscar usuarios

Request en formato curl:

```
curl --location 'http://localhost:5202/api/users' \
--header 'Authorization: ••••••'
```

En autorización se debe de escoger Auth Type: Bearer Token y se provee el bearer token previamente generado.

## 9. Crear usuarios (permitido para rol Admin)

Request en formato curl:

```
curl --location 'http://localhost:5202/api/users' \
--header 'Content-Type: application/json' \
--header 'Authorization: Bearer 9f4c2a7d8b1e5f6a3c9d0e2f7a1b8c6d4e9f0a2b7c1d5e8f3a6b9c2d1e4f7a8' \
--data-raw '{
  "username": "DataWarehouseOperatorUser",
  "email": "newadmin@email.com",
  "firstName": "Daniela",
  "lastName": "Bonilla",
  "password": ".DataWarehouseOperator-1Flow3r!",
  "roleNames": [ "DataWarehouseOperator" ]
}'
```

Se puede escoger entre los roles:

- Admin
- DataWarehouseOperator

Solo Admin tiene permitido crear nuevos usuarios.

## 10. Comprobación de roles

Una vez que se han creado 2 usuarios con roles distintos se puede intentar usar el bearer token de cada uno para poder ejecutar tanto la acción de GET como de POST sobre users con el fin de verificar si realmente se está validando el tipo de rol de acceso.

Si alguien con rol `DataWarehouseOperator` intenta crear un usuario el servidor debería de informar que hay un error 403. En cambio si tiene el rol de Admin se puede llamar la acción sin ningún problema.
