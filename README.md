![.NET](https://img.shields.io/badge/.NET-8-blue)
![Render](https://img.shields.io/badge/Deploy-Render-green)
![Status](https://img.shields.io/badge/Status-MVP-orange)
UniLife

Sistema web desarrollado con ASP.NET Core MVC para la gestión de la vida universitaria.

Información del proyecto

URL del sistema
https://unilife-dgal.onrender.com/

Repositorio
https://github.com/vyordan347-cpu/Unilife.git

Tecnologías

ASP.NET Core MVC (.NET 8)
Entity Framework Core
SQLite
Identity (autenticación y roles)
Razor Views
Bootstrap + CSS personalizado
Redis (Render KeyValue)
Docker
Render

Funcionalidades
Coordinador
Gestión completa de cursos
Gestión completa de eventos
Gestión completa de lugares recomendados
Acceso exclusivo mediante rol
Usuario
Inicio de sesión
Visualización de cursos
Visualización de eventos
Visualización de lugares
Restricción de acceso según rol
Credenciales de prueba

Correo: coordinador@unilife.com

Contraseña: Admin123*

Pruebas realizadas
Autenticación de usuario
Control de acceso por roles
CRUD de cursos
CRUD de eventos
CRUD de lugares
Validación de permisos
Navegación del sistema
Despliegue en producción
Variables de entorno
ASPNETCORE_ENVIRONMENT=Production
ASPNETCORE_URLS=http://0.0.0.0:8080
ConnectionStrings__DefaultConnection=Data Source=app.db
Redis__ConnectionString=red-d719v6t7vvec73e4c7ig:6379
Arquitectura

El sistema sigue el patrón MVC:

Model: entidades y lógica de datos
View: Razor Views para la interfaz
Controller: lógica de negocio y flujo

Incluye:

Autenticación con Identity
Control de acceso por roles
Integración con base de datos SQLite
Uso de cache (Redis en producción)
Despliegue

Aplicación desplegada en Render mediante Docker.

Notas
Sistema funcional como MVP
Roles implementados correctamente
Interfaz mejorada en módulos principales
Base lista para futuras ampliaciones