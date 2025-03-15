-- Crear base de datos y seleccionar shop
CREATE DATABASE IF NOT EXISTS shop;
USE shop;

-- Tabla de productos disponibles
DROP TABLE IF EXISTS Productos;
CREATE TABLE Productos (
    ProductoID INT PRIMARY KEY,
    Nombre VARCHAR(100) NOT NULL,
    Precio DECIMAL(10,2) NOT NULL,
    Categoria VARCHAR(100) NOT NULL
);

-- Tabla para el carrito de compras
DROP TABLE IF EXISTS Carrito;
CREATE TABLE Carrito (
    ProductoID INT PRIMARY KEY,
    Nombre VARCHAR(100) NOT NULL,
    Precio DECIMAL(10,2) NOT NULL,
    Categoria VARCHAR(100) NOT NULL,
    Cantidad INT NOT NULL
);

-- Insertar 26 productos de prueba en la tabla Productos
INSERT INTO Productos (ProductoID, Nombre, Precio, Categoria) VALUES
(1, 'Portátil Dell', 799.99, 'Electrónica'),
(2, 'Smartphone Samsung', 699.99, 'Electrónica'),
(3, 'Tablet Apple', 499.99, 'Electrónica'),
(4, 'Televisor LG 42"', 399.99, 'Electrónica'),
(5, 'Auriculares Sony', 59.99, 'Audio'),
(6, 'Reloj inteligente', 199.99, 'Wearables'),
(7, 'Cámara Canon', 899.99, 'Fotografía'),
(8, 'Impresora HP', 149.99, 'Oficina'),
(9, 'Monitor 24"', 129.99, 'Accesorios'),
(10, 'Teclado mecánico', 89.99, 'Accesorios'),
(11, 'Ratón inalámbrico', 29.99, 'Accesorios'),
(12, 'Silla ergonómica', 249.99, 'Muebles'),
(13, 'Escritorio moderno', 299.99, 'Muebles'),
(14, 'Memoria USB 32GB', 19.99, 'Accesorios'),
(15, 'Disco duro externo 1TB', 79.99, 'Accesorios'),
(16, 'Altavoz Bluetooth', 39.99, 'Audio'),
(17, 'Sistema de sonido', 129.99, 'Audio'),
(18, 'Router inalámbrico', 59.99, 'Redes'),
(19, 'Cargador portátil', 25.99, 'Accesorios'),
(20, 'Soporte para portátil', 49.99, 'Accesorios'),
(21, 'Lámpara de escritorio', 34.99, 'Iluminación'),
(22, 'Mousepad grande', 9.99, 'Accesorios'),
(23, 'Camiseta deportiva', 24.99, 'Ropa'),
(24, 'Pantalón casual', 39.99, 'Ropa'),
(25, 'Zapatos deportivos', 59.99, 'Calzado'),
(26, 'Bolso de mano', 79.99, 'Accesorios');

-- Crear la base de datos y usarla
CREATE DATABASE IF NOT EXISTS shop;
USE shop;

-- Tabla de productos disponibles
DROP TABLE IF EXISTS Productos;
CREATE TABLE Productos (
    ProductoID INT PRIMARY KEY,
    Nombre VARCHAR(100) NOT NULL,
    Precio DECIMAL(10,2) NOT NULL,
    Categoria VARCHAR(100) NOT NULL
);

-- Tabla para el carrito de compras
DROP TABLE IF EXISTS Carrito;
CREATE TABLE Carrito (
    ProductoID INT NOT NULL,
    Nombre VARCHAR(100) NOT NULL,
    Precio DECIMAL(10,2) NOT NULL,
    Categoria VARCHAR(100) NOT NULL,
    Cantidad INT NOT NULL,
    OwnerId VARCHAR(450) NOT NULL,
    PRIMARY KEY (ProductoID, OwnerId)
);
