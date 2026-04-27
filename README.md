UniLife 1.0

Sistema web universitario desarrollado con ASP.NET Core MVC orientado a centralizar la vida académica del estudiante.

Descripción

UniLife es una plataforma web que permite gestionar:

Cursos académicos
Eventos universitarios
Lugares recomendados cercanos

Esta versión corresponde a un MVP (Minimum Viable Product) enfocado en la gestión básica y control por roles.

Tecnologías utilizadas
ASP.NET Core MVC (.NET 8)
Entity Framework Core
SQLite
ASP.NET Identity (autenticación y roles)
Bootstrap + CSS personalizado
GitHub (control de versiones)
Render (deploy)
Redis (cache distribuido)
Roles del sistema
--------------------------------
Coordinador
--------------------------------
Gestiona cursos (CRUD)
Gestiona eventos (CRUD)
Gestiona lugares recomendados (CRUD)
Alumno (en desarrollo)
--------------------------------
Visualiza cursos
Visualiza eventos
Visualiza lugares
Docente (en desarrollo)
--------------------------------
Gestión de contenido académico (próximamente)
Funcionalidades implementadas
Autenticación
Login con ASP.NET Identity
Control de acceso por roles
Protección de controladores con [Authorize]
--------------------------------
Crear evento
Listar eventos
Editar evento
Eliminar evento
Filtro por tipo de evento
Lugares recomendados
--------------------------------
Crear lugar
Listar lugares
Editar lugar
Eliminar lugar
Filtro por tipo
UI / UX
--------------------------------
Diseño personalizado para Login (basado en Figma)
Panel de Coordinador mejorado
Visualización tipo cards en módulos principales
Control visual por roles (botones ocultos según permisos)
--------------------------------