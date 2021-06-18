CREATE DATABASE Pizzaria
GO

USE Pizzaria
GO

CREATE TABLE Usuario
(
	Id INT PRIMARY KEY IDENTITY,
	Nome VARCHAR(60),
	Senha VARCHAR(20),
	Foto VARBINARY(MAX)
)
GO

CREATE TABLE TipoProduto
(
	Id INT PRIMARY KEY IDENTITY,
	Descricao VARCHAR(50)
)
GO

CREATE TABLE Tamanho
(
	Id INT PRIMARY KEY IDENTITY,
	Tam VARCHAR(50)
)

CREATE TABLE Produto
(
	Codigo INT NOT NULL,
	IdTipo INT NOT NULL FOREIGN KEY REFERENCES TipoProduto (Id),
	IdTamanho INT NOT NULL FOREIGN KEY REFERENCES Tamanho (Id),
	Veg BIT,
	Nome VARCHAR(50),
	Preco DECIMAL(12,2),
	Foto VARBINARY(MAX),
	PRIMARY KEY CLUSTERED ([Codigo]) 
	WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON,
	ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO

CREATE TABLE Cliente
(
	Id INT NOT NULL,
	Nome VARCHAR(100),
	Telefone VARCHAR(20),
	CEP VARCHAR(20),
	Rua VARCHAR(50),
	Numero INT,
	Bairro VARCHAR(50),
	Cidade VARCHAR(50),
	Estado VARCHAR(50),
	PRIMARY KEY CLUSTERED ([Id])
	WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON,
	ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
)ON [PRIMARY]
GO

CREATE TABLE Vendas
(
	IdPedido INT NOT NULL PRIMARY KEY IDENTITY,
	IdCliente INT NOT NULL FOREIGN KEY REFERENCES Cliente (Id),
	IdVendedor INT NOT NULL FOREIGN KEY REFERENCES Usuario (Id),
	Data DATETIME,
	Total DECIMAL(12,2)
)
GO

CREATE TABLE PedidoItem
(
	Id INT NOT NULL PRIMARY KEY IDENTITY,
	IdVendas INT NOT NULL FOREIGN KEY REFERENCES Vendas (IdPedido),
	IdProduto INT NOT NULL FOREIGN KEY REFERENCES Produto (Codigo),
	Quantidade INT NOT NULL,
	Preco DECIMAL(12,2)
)
GO

INSERT INTO Usuario
SELECT 'Adriana Lima', 'Adriana123', BulkColumn
FROM Openrowset(Bulk 'C:\Users\Documents\Pizzaria\Resources\Adriana.jpg', Single_Blob) AS Foto

INSERT INTO Usuario
SELECT 'Alessandra Ambrósio', 'Ale123', BulkColumn
FROM Openrowset(Bulk 'C:\Users\Documents\Pizzaria\Resources\Alessandra.jpg', Single_Blob) AS Foto

INSERT INTO Usuario
SELECT 'Audrey Hepburn', 'Audrey123', BulkColumn
FROM Openrowset(Bulk 'C:\Users\Documents\Pizzaria\Resources\Audrey.jpg', Single_Blob) AS Foto

INSERT INTO Usuario
SELECT 'Keanu Reeves', 'Keanu123', BulkColumn
FROM Openrowset(Bulk 'C:\Users\Documents\Pizzaria\Resources\Keanu.jpg', Single_Blob) AS Foto

INSERT INTO Usuario
SELECT 'Nicolas Cage', 'Nicolas123', BulkColumn
FROM Openrowset(Bulk 'C:\Users\Documents\Pizzaria\Resources\Nicolas.jpg', Single_Blob) AS Foto

INSERT INTO Usuario
SELECT 'Rodrigo Santoro', 'Rod123', BulkColumn
FROM Openrowset(Bulk 'C:\Users\Documents\Pizzaria\Resources\Rodrigo.jpg', Single_Blob) AS Foto

INSERT INTO TipoProduto VALUES ('Pizza Doce'), ('Pizza Salgada'), ('Bebida')

INSERT INTO Tamanho VALUES ('Broto'), 
('Pequena'), 
('Média'), 
('Grande'), 
('Lata'), 
('Garrafa 600 ml'), 
('2L'), 
('Garrafa 500ml')
