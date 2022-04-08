BEGIN TRANSACTION;
CREATE TABLE IF NOT EXISTS "GENDER" (
	"gender_num"	INTEGER NOT NULL UNIQUE,
	"gender_label"	TEXT NOT NULL UNIQUE,
	PRIMARY KEY("gender_num" AUTOINCREMENT)
);
CREATE TABLE IF NOT EXISTS "SPECIES" (
	"species_num"	INTEGER NOT NULL UNIQUE,
	"species_label"	TEXT NOT NULL UNIQUE,
	"parent_species_num"	INTEGER,
	FOREIGN KEY("parent_species_num") REFERENCES "SPECIES"("species_num"),
	PRIMARY KEY("species_num")
);
CREATE TABLE IF NOT EXISTS "AGENT" (
	"agent_num"	TEXT NOT NULL UNIQUE,
	"agent_label"	TEXT NOT NULL,
	"agent_birth_date"	INTEGER NOT NULL,
	"agent_death_date"	INTEGER,
	"record_num"	INTEGER NOT NULL,
	"species_num"	INTEGER NOT NULL,
	"gender_num"	INTEGER NOT NULL,
	FOREIGN KEY("species_num") REFERENCES "SPECIES"("species_num"),
	FOREIGN KEY("gender_num") REFERENCES "GENDER"("gender_num"),
	FOREIGN KEY("record_num") REFERENCES "RECORD"("record_num"),
	PRIMARY KEY("agent_num")
);
CREATE TABLE IF NOT EXISTS "AGENT_DATA" (
	"agent_data_time"	INTEGER NOT NULL UNIQUE,
	"agent_data_hydration_level"	REAL,
	"agent_data_hunger_level"	REAL,
	"agent_data_world_num"	TEXT NOT NULL,
	"agent_data_agent_num"	TEXT NOT NULL,
	"agent_data_pack_num"	TEXT,
	FOREIGN KEY("agent_data_agent_num") REFERENCES "AGENT"("agent_num"),
	FOREIGN KEY("agent_data_pack_num") REFERENCES "PACK"("pack_num"),
	FOREIGN KEY("agent_data_world_num") REFERENCES "WORLD"("world_num"),
	PRIMARY KEY("agent_data_time")
);
CREATE TABLE IF NOT EXISTS "PACK" (
	"pack_num"	TEXT NOT NULL UNIQUE,
	"pack_creation_time"	INTEGER NOT NULL,
	PRIMARY KEY("pack_num")
);
CREATE TABLE IF NOT EXISTS "WORLD" (
	"world_num"	TEXT NOT NULL UNIQUE,
	"world_temperature"	REAL NOT NULL,
	"world_humidity"	REAL NOT NULL,
	"world_wind_speed"	INTEGER NOT NULL,
	PRIMARY KEY("world_num")
);
CREATE TABLE IF NOT EXISTS "RECORD" (
	"record_num"	INTEGER NOT NULL UNIQUE,
	"record_start_time"	REAL NOT NULL,
	"record_stop_time"	REAL NOT NULL,
	PRIMARY KEY("record_num" AUTOINCREMENT)
);
INSERT INTO "GENDER" VALUES (2,'Mâle');
INSERT INTO "GENDER" VALUES (3,'Femelle');
INSERT INTO "SPECIES" VALUES (1,'Mammifère',NULL);
INSERT INTO "SPECIES" VALUES (2,'Cervidés',1);
INSERT INTO "SPECIES" VALUES (3,'Cerf Élaphe',2);
INSERT INTO "SPECIES" VALUES (4,'Chevreuil',2);
INSERT INTO "SPECIES" VALUES (5,'Daim',2);
INSERT INTO "SPECIES" VALUES (6,'Renne',2);
INSERT INTO "SPECIES" VALUES (7,'Élan',2);
INSERT INTO "SPECIES" VALUES (8,'Canidés',1);
INSERT INTO "SPECIES" VALUES (9,'Loup',8);
INSERT INTO "SPECIES" VALUES (10,'Wolf2',8);
COMMIT;