Drop Table Addresses;
Drop Table Cities;


Create Table Cities(
	id Int Identity(1,1) primary key NOT NULL, 
	postalCode Varchar(6) UNIQUE NOT NULL, 
	name Varchar(60) NOT NULL,
	dateOfCreation datetime,
	dateOfUpdate datetime
)

Create Table Addresses(
	id Int Identity(1,1) primary key NOT NULL, 
	name Varchar(50) NOT NULL, 
	surname Varchar(50) NOT NULL, 	
	birthdate date,
	phoneNumber varchar(10),
	status bit,
	city int foreign key references Cities(id),
	dateOfCreation datetime,
	dateOfUpdate datetime
)


INSERT into Cities(postalCode, name, dateOfCreation, dateOfUpdate)
values ('62-800', 'Kalisz', GETDATE(),GETDATE())

INSERT into Cities(postalCode, name, dateOfCreation, dateOfUpdate)
values ('63-400', 'Ostrów Wielkopolski', GETDATE(),GETDATE())

INSERT into Cities(postalCode, name, dateOfCreation, dateOfUpdate)
values ('61-626', 'Poznañ', GETDATE(),GETDATE())

INSERT into Addresses(name, surname, city, dateOfCreation, dateOfUpdate)
values ('Adam', 'Nowak', 1, GETDATE(),GETDATE())

