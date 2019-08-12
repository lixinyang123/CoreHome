//BaseSql=======================================================================================================================

create database articles;

create table article(
    id int(5) auto_increment primary key,
    time varchar(20),
    title varchar(20) UNIQUE,
    overview varchar(500)
);

insert into article values(null,'时间','标题','概述');

select *from article;
//倒序查询
select *from article order by id desc；

select count(*) from article;

select *from article where title='标题';

//偏移查询
select *from id limit [偏移量（index*pageSize）],[查询记录数(pageSize)];
select *from id limit 2,3;
//倒序查询最新上传的博客
select *from article order by id desc limit 2,3;

//TestSql======================================================================================================================
insert into article values(null,'title1','2019/05/12 01:17:12','overview1');
insert into article values(null,'title2','2019/05/12 01:17:12','overview2');
insert into article values(null,'title3','2019/05/13 01:17:12','overview3');
insert into article values(null,'title4','2019/05/14 01:17:12','overview4');
insert into article values(null,'title5','2019/05/15 01:17:12','overview5');
insert into article values(null,'title6','2019/05/16 01:17:12','overview6');
insert into article values(null,'title7','2019/05/17 01:17:12','overview7');
insert into article values(null,'title8','2019/05/17 01:17:12','overview8');

insert into article values(null,'Cookie与隐私策略','2019/05/11 01:17:12','我们十分重视你的隐私。本隐私声明阐述了我们处理的个人数据以及处理个人数据的方式和目的。请阅读本隐私声明中特定于产品的详细信息，这些详细信息提供了其他相关信息。');
