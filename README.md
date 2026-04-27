UniLife

Sistema web desarrollado con ASP.NET Core MVC para la gestión de la vida universitaria, incluyendo cursos, eventos y lugares recomendados.

URL del sistema

https://unilife-dgal.onrender.com/

Repositorio

https://github.com/vyordan347-cpu/Unilife.git

Tecnologías utilizadas
ASP.NET Core MVC (.NET 8)
Entity Framework Core
SQLite
Identity (autenticación y roles)
Razor Views
Bootstrap + CSS personalizado
Redis (Render KeyValue)
Docker
Render (deploy)
Funcionalidades
Coordinador
Panel exclusivo para el rol "Coordinador"
Gestión de cursos
Crear cursos
Editar cursos
Eliminar cursos
Visualizar cursos
Gestión de eventos
Crear eventos
Editar eventos
Eliminar eventos
Filtro por tipo
Gestión de lugares recomendados
Crear lugares
Editar lugares
Eliminar lugares
Filtro por tipo
Usuario
Inicio de sesión
Visualización de cursos
Visualización de eventos
Visualización de lugares recomendados
Restricción de acceso según rol
Control visual de funcionalidades
Credenciales de prueba

Correo: coordinador@unilife.com

Contraseña: Admin123*

Pruebas realizadas
Login de usuario
Control de acceso por roles
CRUD de cursos
CRUD de eventos
CRUD de lugares recomendados
Validación de permisos según rol
Navegación entre módulos
Despliegue en Render
Variables de entorno (Render)
ASPNETCORE_ENVIRONMENT=Production
ASPNETCORE_URLS=http://0.0.0.0:8080
ConnectionStrings__DefaultConnection=Data Source=app.db
Redis__ConnectionString=red-d719v6t7vvec73e4c7ig:6379
Explicación de variables
ASPNETCORE_ENVIRONMENT → Define el entorno de ejecución
ASPNETCORE_URLS → Puerto requerido por Render
ConnectionStrings__DefaultConnection → Base de datos SQLite
Redis__ConnectionString → Conexión a Redis (KeyValue)
Redis
En desarrollo local: uso de memoria (DistributedMemoryCache)
En producción: uso de Redis (Render KeyValue)
Docker

El despliegue se realiza mediante un Dockerfile ubicado en la raíz del proyecto.

Notas finales
El sistema implementa correctamente el patrón MVC
Se integró autenticación y control de roles
Se aplicó separación de responsabilidades en el desarrollo
Se mejoró la interfaz de usuario con diseño personalizado
Se realizó despliegue en producción usando Docker y Render