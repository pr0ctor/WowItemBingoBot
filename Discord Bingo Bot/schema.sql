drop table if exists bingocardcompletions;
drop table if exists bingocardsubmissions;
drop table if exists bingocards;
drop table if exists discordusers;
drop table if exists wowcharacters;
drop table if exists wowgear;

create table wowgear(
	id bigint unique not null auto_increment,
	itemid int,
	itemname varchar(250),
	itemimageurl varchar(250),
	itemimagename varchar(250),
	primary key(id)
);

create table discordusers(
	snowflake bigint unique not null,
	username varchar(120),
	primary key(snowflake)
);

create table wowcharacters(
	id bigint unique not null auto_increment,
	charactername char(12),
	primary key(id)
);

create table bingocards(
	id bigint unique not null auto_increment,
	sessioncode varchar(1000),
	layout varchar(10000),
	boardstatecode varchar(1000),
	completed boolean default 0,
	cardowner bigint not null,
	primary key(id),
	foreign key(cardowner) references discordusers(snowflake)
);

create table bingocardsubmissions(
	id bigint unique not null auto_increment,
	discorduser bigint not null,
	wowcharacter bigint not null,
	gearitem bigint not null,
	bingocard bigint not null,
	screenshoturl text not null,
	submitted timestamp not null default current_timestamp,
	primary key(id),
	foreign key(discorduser) references discordusers(snowflake),
	foreign key(gearitem) references wowgear(id),
	foreign key(wowcharacter) references wowcharacters(id),
	foreign key(bingocard) references bingocards(id)
);

create table bingocardcompletions(
	id bigint unique not null auto_increment,
	discorduser bigint not null,
	bingocard bigint not null,
	completedon timestamp not null default current_timestamp,
	primary key(id),
	foreign key(discorduser) references discordusers(snowflake),
	foreign key(bingocard) references bingocards(id)
);

-- Load source data for wow gear items
--Mythic+ dungeons season 1
load data local infile 'D:/Projects/Visual Studio/DiscordBingo/Discord Bingo Bot/Discord Bingo Bot/SourceData/arakara.csv' into table wowgear fields terminated by ',' enclosed by '"' lines terminated by '\r\n' ignore 1 lines (itemid, itemname, itemimageurl, itemimagename);
load data local infile 'D:/Projects/Visual Studio/DiscordBingo/Discord Bingo Bot/Discord Bingo Bot/SourceData/cityofthreads.csv' into table wowgear fields terminated by ',' enclosed by '"' lines terminated by '\r\n' ignore 1 lines (itemid, itemname, itemimageurl, itemimagename);
--load data local infile 'D:/Projects/Visual Studio/DiscordBingo/Discord Bingo Bot/Discord Bingo Bot/SourceData/grimbatol.csv' into table wowgear fields terminated by ',' enclosed by '"' lines terminated by '\r\n' ignore 1 lines (itemid, itemname, itemimageurl, itemimagename);
--load data local infile 'D:/Projects/Visual Studio/DiscordBingo/Discord Bingo Bot/Discord Bingo Bot/SourceData/mistsoftirnascithe.csv' into table wowgear fields terminated by ',' enclosed by '"' lines terminated by '\r\n' ignore 1 lines (itemid, itemname, itemimageurl, itemimagename);
--load data local infile 'D:/Projects/Visual Studio/DiscordBingo/Discord Bingo Bot/Discord Bingo Bot/SourceData/siegeofboralus.csv' into table wowgear fields terminated by ',' enclosed by '"' lines terminated by '\r\n' ignore 1 lines (itemid, itemname, itemimageurl, itemimagename);
load data local infile 'D:/Projects/Visual Studio/DiscordBingo/Discord Bingo Bot/Discord Bingo Bot/SourceData/thedawnbreaker.csv' into table wowgear fields terminated by ',' enclosed by '"' lines terminated by '\r\n' ignore 1 lines (itemid, itemname, itemimageurl, itemimagename);
--load data local infile 'D:/Projects/Visual Studio/DiscordBingo/Discord Bingo Bot/Discord Bingo Bot/SourceData/thenecroticwake.csv' into table wowgear fields terminated by ',' enclosed by '"' lines terminated by '\r\n' ignore 1 lines (itemid, itemname, itemimageurl, itemimagename);
load data local infile 'D:/Projects/Visual Studio/DiscordBingo/Discord Bingo Bot/Discord Bingo Bot/SourceData/thestonevault.csv' into table wowgear fields terminated by ',' enclosed by '"' lines terminated by '\r\n' ignore 1 lines (itemid, itemname, itemimageurl, itemimagename);
--End M+

--Other Dungeons
load data local infile 'D:/Projects/Visual Studio/DiscordBingo/Discord Bingo Bot/Discord Bingo Bot/SourceData/cinderbrew.csv' into table wowgear fields terminated by ',' enclosed by '"' lines terminated by '\r\n' ignore 1 lines (itemid, itemname, itemimageurl, itemimagename);
load data local infile 'D:/Projects/Visual Studio/DiscordBingo/Discord Bingo Bot/Discord Bingo Bot/SourceData/darkflame.csv' into table wowgear fields terminated by ',' enclosed by '"' lines terminated by '\r\n' ignore 1 lines (itemid, itemname, itemimageurl, itemimagename);
load data local infile 'D:/Projects/Visual Studio/DiscordBingo/Discord Bingo Bot/Discord Bingo Bot/SourceData/priory.csv' into table wowgear fields terminated by ',' enclosed by '"' lines terminated by '\r\n' ignore 1 lines (itemid, itemname, itemimageurl, itemimagename);
load data local infile 'D:/Projects/Visual Studio/DiscordBingo/Discord Bingo Bot/Discord Bingo Bot/SourceData/rookery.csv' into table wowgear fields terminated by ',' enclosed by '"' lines terminated by '\r\n' ignore 1 lines (itemid, itemname, itemimageurl, itemimagename);



insert into wowcharacters(id, charactername) values(1, 'Me');