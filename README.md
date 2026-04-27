UniLife 1.0

Sistema web universitario desarrollado con ASP.NET Core MVC orientado a centralizar la gestión de la vida académica del estudiante.

Descripción

UniLife es una plataforma web que permite gestionar de manera integrada:

Cursos académicos
Eventos universitarios
Lugares recomendados cercanos

Esta versión corresponde a un MVP (Minimum Viable Product) enfocado en la gestión básica y el control de acceso mediante roles.

Tecnologías utilizadas
ASP.NET Core MVC (.NET 8)
Entity Framework Core
SQLite
ASP.NET Identity (autenticación y gestión de roles)
Bootstrap y CSS personalizado
GitHub (control de versiones)
Render (despliegue)
Redis (cache distribuido)
Roles del sistema
Coordinador
Gestión de cursos (CRUD)
Gestión de eventos (CRUD)
Gestión de lugares recomendados (CRUD)
Alumno (en desarrollo)
Visualización de cursos
Visualización de eventos
Visualización de lugares
Docente (en desarrollo)
Gestión de contenido académico (próximamente)
Funcionalidades implementadas
Autenticación
Inicio de sesión con ASP.NET Identity
Control de acceso basado en roles
Protección de controladores mediante [Authorize]
Cursos
Crear, listar, editar y eliminar cursos
Visualización en formato de tarjetas (cards)
Eventos
Crear, listar, editar y eliminar eventos
Filtro por tipo de evento
Lugares recomendados
Crear, listar, editar y eliminar lugares
Filtro por tipo
Interfaz de usuario
Diseño personalizado del módulo de autenticación (Login) basado en Figma
Panel de Coordinador mejorado
Visualización de información en formato de tarjetas
Control visual por roles (elementos ocultos según permisos)
Base de datos

Se utiliza SQLite como motor de base de datos para esta versión:

Data Source=app.db
Ejecución del proyecto
Clonar repositorio
git clone https://github.com/TU-USUARIO/Unilife.git
cd Unilife
Ejecutar la aplicación
dotnet build
dotnet run
Usuario de prueba
Correo: coordinador@unilife.com
Contraseña: Admin123*
Rol: Coordinador
Despliegue

Aplicación desplegada en Render:

(Colocar aquí la URL del sistema)

Variables de entorno (Render)
ASPNETCORE_ENVIRONMENT=Production
ASPNETCORE_URLS=http://0.0.0.0:8080
ConnectionStrings__DefaultConnection=Data Source=app.db
Redis__ConnectionString=red-xxxx:6379
Limitaciones de la versión 1.0
No existe registro de usuarios desde la interfaz
Roles asignados manualmente mediante SeedData
Módulos de alumno y docente en desarrollo
No se incluye carga de archivos
No se cuenta con panel analítico avanzado
Próximas mejoras
Implementación completa de roles (Alumno y Docente)
Gestión de tareas y entregas
Paneles personalizados por rol
Sistema de notificaciones
Dashboard con métricas
Mejora de seguridad y validaciones