# API REST-ASP.NET CORE 8 DOCKERIZED

## Descripción

Este proyecto tiene como objetivo familiarizar a los estudiantes con las prácticas de contenedorización y orquestación de aplicaciones mediante el uso de Docker y Docker Compose. A través de esta API REST, los estudiantes aprenderán a desplegar una aplicación en conjunto con una base de datos PostgreSQL, enfocándose en la automatización, la reproducibilidad y la escalabilidad. 

### Características Principales

- **Autenticación:** Sistema de autenticación de usuarios utilizando tokens JWT.
- **Creación y Administración de Usuarios:**
  - Registro de nuevos usuarios.
  - Actualización y eliminación de usuarios existentes.
- **Control de Acceso:**
  - Tres niveles de acceso para los usuarios: Administrador, Editor y Lector.
  - Políticas de acceso documentadas para cada nivel de usuario.
- **Publicación de Posts:**
  - Permitir a los usuarios escribir posts de diferentes tipos (texto, imagen, vídeo) almacenados en una base de datos PostgreSQL.

## Tecnologías Utilizadas

- **ASP.NET Core 8**
- **Entity Framework Core**
- **PostgreSQL**
- **Docker & Docker Compose**
- **JWT (JSON Web Token)**

## Instalación

Para clonar e instalar el proyecto localmente:

1. Clona el repositorio desde GitHub:

    ```bash
    git clone https://github.com/JeisonFonseca/API-REST_asp.net_core_8.git
    ```

2. **Recomendación:** Ejecuta el proyecto utilizando Visual Studio 2022 para facilitar el proceso.

3. Asegúrate de que el archivo `db-init.sql` esté en la ruta `C:\Dockerfiles`. Este archivo es necesario para la configuración inicial de la base de datos.

## Uso

Para iniciar la aplicación:

1. Abre el proyecto en Visual Studio 2022.
2. Selecciona `docker-compose` como el startup item.
3. Ejecuta la aplicación. 

**Nota:** La base de datos incluye datos precargados, por lo que podrás empezar a interactuar con la API inmediatamente después de la instalación.

## Configuración

Todas las configuraciones importantes, incluidas las de JWT y la conexión a la base de datos, se encuentran en el archivo `appsettings.json`. 

## API Endpoints

### Autenticación
- **POST /api/Users/Login:** Autentica a un usuario y devuelve un token JWT.

### Creación y Administración de Usuarios
- **POST /api/Users:** Registra un nuevo usuario.
- **PUT /api/Users/{id}:** Actualiza un usuario existente.
- **DELETE /api/Users/{id}:** Elimina un usuario existente.

### Control de Acceso
- **GET /api/Users/GetUser?id={id}:** Obtiene la información de un usuario específico. (Protegido con JWT)
- **Políticas de acceso:** Documentadas en la API para los roles de Administrador, Editor y Lector.

### Publicación de Posts
- **POST /api/Posts:** Permite a los usuarios escribir posts de diferentes tipos.

## Contribución

Este proyecto no acepta contribuciones externas.

## Licencia

No hay licencias para este proyecto.

## Contacto

Para preguntas o problemas relacionados con este proyecto, por favor contacta con la Escuela de Computación del Instituto Tecnológico de Costa Rica (TEC) en Cartago.