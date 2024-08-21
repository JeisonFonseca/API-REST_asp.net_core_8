-- Creación del ENUM para el rol de usuario
CREATE TYPE UserRole AS ENUM ('Administrador', 'Editor', 'Lector');

-- Creación del ENUM para el estado de la amistad
CREATE TYPE FriendshipStatus AS ENUM ('Pendiente', 'Aceptado', 'Bloqueado');

-- Creación del ENUM para los tipos de reacciones
CREATE TYPE ReactionType AS ENUM ('Me gusta', 'Me encanta', 'Me importa', 'Jaja', 'Guau', 'Enfadado', 'Triste');

-- Tabla Users
CREATE TABLE "Users" (
  "userId" SERIAL PRIMARY KEY,
  "username" VARCHAR(50) UNIQUE,
  "email" VARCHAR(100) UNIQUE NOT NULL,
  "passwordHash" VARCHAR(255) NOT NULL,
  "firstName" VARCHAR(50),
  "lastName" VARCHAR(50),
  "secondLastName" VARCHAR(50),
  "dateOfBirth" DATE,
  "bio" TEXT,
  "createdAt" TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
  "updatedAt" TIMESTAMP,
  "profileImage" VARCHAR(255),
  "role" UserRole NOT NULL
);

-- Tabla Friendships
CREATE TABLE "Friendships" (
  "friendshipId" SERIAL PRIMARY KEY,
  "userId" INT NOT NULL,
  "friendId" INT NOT NULL,
  "status" FriendshipStatus NOT NULL,
  "createdAt" TIMESTAMP,
  CONSTRAINT "FK_Friendships.userId" FOREIGN KEY ("userId") REFERENCES "Users"("userId") ON DELETE CASCADE,
  CONSTRAINT "FK_Friendships.friendId" FOREIGN KEY ("friendId") REFERENCES "Users"("userId") ON DELETE CASCADE,
  CONSTRAINT "UC_Friendships_user_friend" UNIQUE ("userId", "friendId")
);

-- Tabla Posts
CREATE TABLE "Posts" (
  "postId" SERIAL PRIMARY KEY,
  "userId" INT NOT NULL,
  "content" TEXT,
  "mediaUrl" VARCHAR(255),
  "createdAt" TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
  CONSTRAINT "FK_Posts.userId" FOREIGN KEY ("userId") REFERENCES "Users"("userId") ON DELETE CASCADE
);

-- Tabla Comments
CREATE TABLE "Comments" (
  "commentId" SERIAL PRIMARY KEY,
  "postId" INT NOT NULL,
  "userId" INT NOT NULL,
  "content" TEXT,
  "mediaUrl" VARCHAR(255),
  "createdAt" TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
  CONSTRAINT "FK_Comments.postId" FOREIGN KEY ("postId") REFERENCES "Posts"("postId") ON DELETE CASCADE,
  CONSTRAINT "FK_Comments.userId" FOREIGN KEY ("userId") REFERENCES "Users"("userId") ON DELETE CASCADE
);

-- Tabla Messages
CREATE TABLE "Messages" (
  "messageId" SERIAL PRIMARY KEY,
  "senderId" INT NOT NULL,
  "receiverId" INT NOT NULL,
  "content" TEXT,
  "mediaUrl" VARCHAR(255),
  "sentAt" TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
  "readAt" TIMESTAMP,
  CONSTRAINT "FK_Messages.senderId" FOREIGN KEY ("senderId") REFERENCES "Users"("userId") ON DELETE CASCADE,
  CONSTRAINT "FK_Messages.receiverId" FOREIGN KEY ("receiverId") REFERENCES "Users"("userId") ON DELETE CASCADE
);

-- Tabla Reactions
CREATE TABLE "Reactions" (
  "reactionId" SERIAL PRIMARY KEY,
  "postId" INT NOT NULL,
  "userId" INT NOT NULL,
  "type" ReactionType NOT NULL,
  "createdAt" TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
  CONSTRAINT "FK_Reactions.userId" FOREIGN KEY ("userId") REFERENCES "Users"("userId") ON DELETE CASCADE,
  CONSTRAINT "FK_Reactions.postId" FOREIGN KEY ("postId") REFERENCES "Posts"("postId") ON DELETE CASCADE,
  CONSTRAINT "UC_Reactions_post_user" UNIQUE ("postId", "userId")
);


-- DATOS DE PRUEBA -- 

-- Insertar usuarios de prueba
INSERT INTO "Users" ("username", "email", "passwordHash", "firstName", "lastName", "secondLastName", "dateOfBirth", "bio", "profileImage", "role")
VALUES
('jdoe', 'jdoe@example.com', 'hashedpassword1', 'John', 'Doe', 'Smith', '1985-05-15', 'Software engineer', 'image1.png', 'Administrador'),
('asmith', 'asmith@example.com', 'hashedpassword2', 'Anna', 'Smith', NULL, '1990-07-22', 'Project manager', 'image2.png', 'Editor'),
('bwilson', 'bwilson@example.com', 'hashedpassword3', 'Bob', 'Wilson', 'Brown', '1992-11-30', 'UX designer', 'image3.png', 'Lector');

-- Insertar amistades de prueba
INSERT INTO "Friendships" ("userId", "friendId", "status", "createdAt")
VALUES
(1, 2, 'Aceptado', CURRENT_TIMESTAMP),
(1, 3, 'Pendiente', CURRENT_TIMESTAMP),
(2, 3, 'Bloqueado', CURRENT_TIMESTAMP);

-- Insertar publicaciones de prueba
INSERT INTO "Posts" ("userId", "content", "mediaUrl", "createdAt")
VALUES
(1, 'Este es mi primer post!', 'post_image1.png', CURRENT_TIMESTAMP),
(2, '¡Hola a todos!', NULL, CURRENT_TIMESTAMP),
(3, 'Check out my new design project.', 'post_image2.png', CURRENT_TIMESTAMP);

-- Insertar comentarios de prueba
INSERT INTO "Comments" ("postId", "userId", "content", "mediaUrl", "createdAt")
VALUES
(1, 2, '¡Gran post, John!', NULL, CURRENT_TIMESTAMP),
(1, 3, '¡Bien hecho!', NULL, CURRENT_TIMESTAMP),
(2, 1, '¡Hola, Anna! ¿Cómo estás?', NULL, CURRENT_TIMESTAMP);

-- Insertar mensajes de prueba
INSERT INTO "Messages" ("senderId", "receiverId", "content", "mediaUrl", "sentAt", "readAt")
VALUES
(1, 2, '¡Hola Anna! ¿Tienes un momento para hablar?', NULL, CURRENT_TIMESTAMP, NULL),
(2, 1, 'Hola John, claro que sí.', NULL, CURRENT_TIMESTAMP, CURRENT_TIMESTAMP),
(3, 1, '¿Podrías revisar mi diseño cuando tengas tiempo?', 'design_preview.png', CURRENT_TIMESTAMP, NULL);

-- Insertar reacciones de prueba
INSERT INTO "Reactions" ("postId", "userId", "type", "createdAt")
VALUES
(1, 2, 'Me gusta', CURRENT_TIMESTAMP),
(1, 3, 'Me encanta', CURRENT_TIMESTAMP),
(2, 1, 'Jaja', CURRENT_TIMESTAMP);

