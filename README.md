UniLife

Sistema web desarrollado con ASP.NET Core MVC para la gestión de la vida universitaria, incluyendo cursos, eventos y lugares recomendados.

URL del sistema

(Aquí colocas tu link de Render)

Repositorio

https://github.com/vyordan347-cpu/Unilife

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
Autenticación

Inicio de sesión con ASP.NET Identity
Control de acceso por roles
Protección de controladores con [Authorize]

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

Validaciones implementadas

Generación automática de código de aula
Restricción de acceso a funciones según rol
Usuario (vista general)

Visualización de cursos
Visualización de eventos
Visualización de lugares recomendados

Control visual de interfaz

Ocultamiento de botones según rol
Acceso restringido a funcionalidades administrativas
UI / UX

Diseño personalizado del Login basado en Figma
Panel de Coordinador mejorado
Visualización de cursos en formato de tarjetas
Interfaz moderna y responsiva
Separación de estilos por módulos (login, cursos, panel)

Credenciales de prueba

Correo: coordinador@unilife.com

Contraseña: Admin123*

Pruebas realizadas

Inicio de sesión correcto
Control de acceso por roles
CRUD completo de cursos
CRUD completo de eventos
CRUD completo de lugares
Validación de permisos por rol
Interfaz adaptada según tipo de usuario

Despliegue en Render

La aplicación se encuentra desplegada en Render utilizando Docker.

Variables de entorno (Render)
ASPNETCORE_ENVIRONMENT=Production  
ASPNETCORE_URLS=http://0.0.0.0:8080  
ConnectionStrings__DefaultConnection=Data Source=app.db  
Redis__ConnectionString=red-xxxx:6379  
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

El sistema cumple con las funcionalidades principales del caso propuesto
Se implementó correctamente el patrón MVC
Se integró autenticación, roles y control de acceso
Se mejoró la experiencia de usuario mediante diseño personalizado
Se realizó despliegue en producción utilizando Docker y Render