# UniLife

Sistema web desarrollado con ASP.NET Core MVC para la gestión de la vida universitaria, incluyendo cursos, eventos y lugares recomendados.

---

## Información del proyecto

**URL del sistema**  
https://unilife-dgal.onrender.com/

**Repositorio**  
https://github.com/vyordan347-cpu/Unilife.git

---

## Tecnologías

- ASP.NET Core MVC (.NET 8)  
- Entity Framework Core  
- SQLite  
- Identity (autenticación y roles)  
- Razor Views  
- Bootstrap + CSS personalizado  
- Redis (Render KeyValue)  
- Docker  
- Render  

---

## Funcionalidades

### Coordinador

- Gestión completa de cursos  
- Gestión completa de eventos  
- Gestión completa de lugares recomendados  
- Acceso restringido por rol  

---

### Usuario

- Inicio de sesión  
- Visualización de cursos  
- Visualización de eventos  
- Visualización de lugares recomendados  
- Control de acceso según rol  

---

## Credenciales de prueba

Correo: coordinador@unilife.com  
Contraseña: Admin123*  

---

## Pruebas realizadas

- Autenticación de usuario  
- Control de acceso por roles  
- CRUD de cursos  
- CRUD de eventos  
- CRUD de lugares  
- Validación de permisos  
- Navegación del sistema  
- Despliegue en producción  

---

## Variables de entorno (Render)

```
ASPNETCORE_ENVIRONMENT=Production
ASPNETCORE_URLS=http://0.0.0.0:8080

ConnectionStrings__DefaultConnection=Data Source=app.db
Redis__ConnectionString=red-d719v6t7vvec73e4c7ig:6379
```

---

## Arquitectura

El sistema sigue el patrón MVC:

- Model: gestión de datos  
- View: interfaz de usuario con Razor  
- Controller: lógica de negocio  

Incluye:

- Autenticación con Identity  
- Control de acceso por roles  
- Base de datos SQLite  
- Cache distribuido con Redis en producción  

---

## Despliegue

Aplicación desplegada en Render utilizando Docker.

---

## Notas

- Sistema funcional como MVP  
- Roles implementados correctamente  
- Interfaz mejorada en módulos principales  
- Base preparada para futuras mejoras  