# API
## En esta API se encuentra un CRUD, configuración de log de eventos y cadena de conexión de a la base de datos
* .Net 7
* Infrastructure
* Microsoft.EntityFrameworkCore.Design 7.0.14
* Swashbuckle.AspNetCore 6.4.0
## Cadena de conexión BD
### Se tien que crear una BD en un servidor para la correcta fucionalidad de este proyecto y otros
#### appsettings.json
```json
{
  "ConnectionStrings": {
    "ApplicationConnection": "cadena"
  }
}
```
## Configuración Log de Eventos
### Aquí se configura desde que nivel se estarán guardando los eventos, la cadena de conexión, el nombre de la tabla y si se crea automáticamente
#### appsettings.json
```json
{
  "Serilog": {
    "MinimumLevel": "Information",
    "WriteTo": [
      {
        "Name": "MSSqlServer",
        "Args": {
          "connectionString": "cadena",
          "tableName": "LogEvents",
          "autoCreateSqlTable": true
        }
      }
    ]
  }
}
```
### Teniendo esto configurado se registran todos los eventos realizado en la API desde los proyectos de Application,  Infrastructure y API automáticamente o igual podemos utilizar la dependencia ILogger. Ejemplo:
```c#
private readonly ILogger<GetAllUserQuery> _logger;

_logger.LogError(ex.Message);
```
## Migraciones
### En este punto ya podemos realizar migraciones a la base de datos con los siguientes comandos
```
Add-Migration InitialCreate

Update-Database
```
## CRUD y otros métodos
### Como se muestra en las imagenes al ejecutar el proyecto podemos ver una interfaz en la cual se puede interactúa para realizar Test ya que aparte de un CRUD contiene otros métodos con los cuales podemos realizar diferentes pruebas
![image](https://github.com/Chisfx/App/assets/101854771/688518e9-a22c-41fa-b069-05fc623aca00)
![image](https://github.com/Chisfx/App/assets/101854771/afe9ca16-6afd-4022-8f1b-558e56121993)
### EndPoint 
```js
/api/User/CreateUsersTest/{top = 100} // Crea N registros aleatorios, como default tiene 100
/api/User/GetAll/{test?}/{top?} // Obtiene todos los registros de la BD o con el parámetro test=true y top=N obtiene N registros aleatorios
/api/User/{id} // Obtiene todos los registro por id
/api/User/Post // Crea un registro
/api/User/Put // Actualiza un registro
/api/User/Delete/{id} // Elimina un registro
/api/User/GetGroupAge // Obtiene un agrupamiento por edad de usuarios
/api/User/GetGroupAgeTop/{top} // Obtiene un Top del agrupamiento por edad de usuarios
/api/User/GroupHost // Obtiene un agrupamiento por dominio de email de usuarios
/api/User/GetGroupHostTop/{top} // Obtiene un Top del agrupamiento por dominio de email de usuarios
```
