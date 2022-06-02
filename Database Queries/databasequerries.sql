--create database articleshare
--drop table articledetails
--drop table articlelogs
--drop table articles
--drop table users

--3 = super admin
--2 = admin
--1 = normal user

--1 = new/no content
--2 = under review/content added
--3 = review disapproved
--4 = reviewapproved/published
--5 = edited

create table users(
	email varchar(100) not null primary key,
	password varchar(100) not null,
	mobile varchar(10) not null,
	userlevel integer not null,
	createdDate datetime not null
)

create table articles(
	email varchar(100) not null foreign key references users(email),
	articleid int identity(1,1) not null primary key,
	articlename varchar(30) not null
)

create table articlelogs(
	id int identity(1,1) not null primary key,
	articleid int not null foreign key references articles(articleid),
	articlestatus int not null,
	statuschangedate datetime not null,
	reviewcomments varchar(512) 
)

create table articledetails(
	articleid int foreign key references articles(articleid) not null,
	author varchar(100) not null,
	articlename varchar(100) not null,
	articledate datetime not null,
	articlecontent nvarchar(max) not null
)

--INSERT INTO articles(email)
--OUTPUT INSERTED.articleid
--VALUES ('gyaneswarsingh53@gmail.com')


--select * from users
--select * from articles
--select * from articlelogs
--select * from articledetails

--update  articledetails set articlename = 'Mughlai Korma' where articleid = 2
select * from articlelogs where articleid = 2

select * from articledetails ad 
inner join 
(select top 1 with ties id ,articleid ,statuschangedate,articlestatus
from articlelogs
order by row_number() over (partition by articleid order by statuschangedate desc)) al 
on ad.articleid = al.articleid where al.articlestatus > 2 and ad.articlename like '%korma%'

--select top 1 with ties id ,articleid ,statuschangedate,articlestatus
--from articlelogs
--order by row_number() over (partition by articleid order by statuschangedate desc)

--alter table articledetails
--alter column articlecontent nvarchar(max)


select userlevel from users where email = 'gyaneswarsingh53@gmail.com' and password = 'Hello@13'


