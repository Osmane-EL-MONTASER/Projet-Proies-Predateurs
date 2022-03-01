--SCRIPT DE CREATION DE LA BASE DE DONNES
--
--Fait par EL MONTASER Osmane le 01/03/2022.

CREATE TABLE "SPECIES" (
	"species_num"	INTEGER NOT NULL UNIQUE,
	"species_label"	TEXT NOT NULL UNIQUE,
	PRIMARY KEY("species_num" AUTOINCREMENT)
)

CREATE TABLE "WORLD" (
	"world_num"	INTEGER NOT NULL UNIQUE,
	"world_temperature"	REAL NOT NULL,
	"world_humidity"	REAL NOT NULL,
	"world_wind_speed"	INTEGER NOT NULL,
	PRIMARY KEY("world_num")
)

CREATE TABLE "RECORD" (
	"record_num"	INTEGER NOT NULL UNIQUE,
	"record_start_time"	INTEGER NOT NULL,
	"record_stop_time"	INTEGER NOT NULL,
	PRIMARY KEY("record_num" AUTOINCREMENT)
)

CREATE TABLE "GENDER" (
	"gender_num"	INTEGER NOT NULL UNIQUE,
	"gender_label"	TEXT NOT NULL,
	PRIMARY KEY("gender_num" AUTOINCREMENT)
)

CREATE TABLE "PACK" (
	"pack_num"	INTEGER NOT NULL UNIQUE,
	"pack_creation_time"	INTEGER NOT NULL,
	PRIMARY KEY("pack_num" AUTOINCREMENT)
)

CREATE TABLE "AGENT" (
	"agent_num"	INTEGER NOT NULL UNIQUE,
	"agent_label"	TEXT NOT NULL,
	"agent_birth_date"	INTEGER NOT NULL,
	"agent_death_date"	INTEGER NOT NULL,
	"agent_hydration_level"	NUMERIC NOT NULL,
	"agent_hunger_level"	REAL NOT NULL,
	"record_num"	INTEGER NOT NULL,
	"world_num"	INTEGER NOT NULL,
	"species_num"	INTEGER NOT NULL,
	"pack_num"	INTEGER NOT NULL,
	"gender_num"	INTEGER NOT NULL,
	FOREIGN KEY("pack_num") REFERENCES "PACK"("pack_num"),
	FOREIGN KEY("species_num") REFERENCES "SPECIES"("species_num"),
	FOREIGN KEY("world_num") REFERENCES "WORLD"("world_num"),
	PRIMARY KEY("agent_num"),
	FOREIGN KEY("record_num") REFERENCES "RECORD"("record_num"),
	FOREIGN KEY("gender_num") REFERENCES "GENDER"("gender_num")
);
