# App
## En este repositorio se encuentran 6 proyectos en los cuales viene documentado en cada uno que tecnologías se ocuparon, así como configuraciones, pruebas y ejecución.
El esquema que se utilizo fue un catálogo de usuarios
```c#
/// <summary>
/// Represents a user entity.
/// </summary>
public class User
{
    /// <summary>
    /// Gets or sets the user ID.
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// Gets or sets the user name.
    /// </summary>
    public string Name { get; set; } = default!;

    /// <summary>
    /// Gets or sets the user email.
    /// </summary>
    public string Email { get; set; } = default!;

    /// <summary>
    /// Gets or sets the user age.
    /// </summary>
    public int Age { get; set; }
}
```
* [Domain](App.Domain/README.md)
* [Application](App.Application/README.md)
* [Infrastructure](App.Infrastructure/README.md)
* [Api](App.Api/README.md)
* [Web](app.web/README.md)
* [Maui](App.Maui/README.md)
