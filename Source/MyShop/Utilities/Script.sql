create database MyShop;
use MyShop;

create table Users (
	id int identity(1,1) not null,
	username varchar(50),
	password varchar(1000),
	name nvarchar(50),
	gender nvarchar(4),
	dob date,
	address nvarchar(200),
	phone_number varchar(11),
	role varchar(10) default 'Customer',
	constraint PK_Users primary key (id)
)

create table Categories (
	id int identity(1,1) not null,
	name nvarchar(100),
	description ntext,
	image text,
	constraint PK_Categories primary key (id)
)

create table Products (
	id int identity(1,1) not null,
	name nvarchar(100),
	description ntext,
	brand nvarchar(100),
	price money,
	promotion_price money,
	stock int,	
	image text,
	category_id int,
	promotion_id int,
	constraint PK_Products primary key (id)
)

create table Orders (
	id int identity(1,1) not null,
	customer_id int,
	total_revenue money,
	total_profit money,
	order_date date,
	status varchar(20),
	constraint PK_Orders primary key(id)
)

create table OrderItems (
	id int identity(1,1) not null,
	order_id int,
	product_id int,
	quantity int,
	total_price money,
	constraint PK_OrderItems primary key(id)
)

create table Promotions (
	id int identity(1,1) not null,
	code text not null,
	discount_percentage int,
	constraint PK_Promotions primary key(id)
)

alter table Products add 
constraint FK_Products_Categories foreign key (category_id) references Categories (id),
constraint FK_Products_Promotions foreign key (promotion_id) references Promotions (id)

alter table OrderItems add
constraint FK_OrderItems_Orders foreign key (order_id) references Orders (id),
constraint FK_OrderItems_Products foreign key (product_id) references Products (id)