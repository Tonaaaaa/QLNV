CREATE DATABASE QLNhanSu;
GO
USE QLNhanSu;
GO

CREATE TABLE Phongban (
	MaPB char (2) PRIMARY KEY not null,
	TenPB nvarchar(30) not null
);

DROP TABLE IF EXISTS Nhanvien;

CREATE TABLE Nhanvien (
	MaNV char (6) PRIMARY KEY not null,
	TenNV nvarchar (20) not null ,
	Ngaysinh varchar (10) not null,
	MaPB char (2) not null,
	FOREIGN KEY (MaPB) REFERENCES Phongban(MaPB)
);

