# Ecommerce MVC

Proyecto base de ecommerce en ASP.NET Core MVC con autenticación segura usando ASP.NET Core Identity y SQL Server.

## Configuración local recomendada

No guardes credenciales reales en `appsettings.json`. Usa User Secrets en desarrollo o variables de entorno en producción.

```bash
dotnet user-secrets init --project Ecommerce/Ecommerce.csproj
dotnet user-secrets set "ConnectionStrings:DefaultConnection" "Server=localhost;Database=EcommerceDb;User Id=ecommerce_app;Password=TU_PASSWORD_FUERTE;TrustServerCertificate=True;MultipleActiveResultSets=true" --project Ecommerce/Ecommerce.csproj
dotnet user-secrets set "Email:Host" "smtp.tu-dominio.com" --project Ecommerce/Ecommerce.csproj
dotnet user-secrets set "Email:SenderEmail" "no-reply@tu-dominio.com" --project Ecommerce/Ecommerce.csproj
dotnet user-secrets set "Email:UserName" "smtp_user" --project Ecommerce/Ecommerce.csproj
dotnet user-secrets set "Email:Password" "TU_PASSWORD_SMTP" --project Ecommerce/Ecommerce.csproj
```

## Base de datos

La aplicación está preparada para SQL Server con autenticación SQL. Después de configurar la cadena de conexión, crea la migración inicial y aplica la base de datos:

```bash
dotnet ef migrations add InitialIdentitySchema --project Ecommerce/Ecommerce.csproj
dotnet ef database update --project Ecommerce/Ecommerce.csproj
```

## Seguridad aplicada en el módulo de autenticación

- Contraseñas hasheadas y verificadas por ASP.NET Core Identity.
- Longitud mínima de 12 caracteres y requisitos de complejidad.
- Bloqueo temporal tras 5 intentos fallidos.
- Cookies `HttpOnly`, `Secure`, `SameSite=Strict` y expiración de 30 minutos con renovación deslizante.
- Tokens de recuperación de contraseña con vigencia de 2 horas.
- Respuestas de recuperación sin enumerar cuentas existentes.
- Formularios protegidos con antiforgery tokens.
- Rate limiting en rutas de autenticación.
- Enlaces de recuperación enviados por SMTP en producción; en desarrollo se registran en logs para facilitar pruebas locales.
