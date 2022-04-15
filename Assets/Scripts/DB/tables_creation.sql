BEGIN TRANSACTION;
CREATE TABLE IF NOT EXISTS "GENDER" (
	"gender_num"	INTEGER NOT NULL UNIQUE,
	"gender_label"	TEXT NOT NULL UNIQUE,
	PRIMARY KEY("gender_num" AUTOINCREMENT)
);
CREATE TABLE IF NOT EXISTS "AGENT" (
	"agent_num"	TEXT NOT NULL UNIQUE,
	"agent_label"	TEXT NOT NULL,
	"agent_birth_date"	INTEGER NOT NULL,
	"agent_death_date"	INTEGER,
	"record_num"	TEXT NOT NULL,
	"species_num"	INTEGER NOT NULL,
	"gender_num"	INTEGER NOT NULL,
	FOREIGN KEY("gender_num") REFERENCES "GENDER"("gender_num"),
	FOREIGN KEY("species_num") REFERENCES "SPECIES"("species_num"),
	FOREIGN KEY("record_num") REFERENCES "RECORD"("record_num"),
	PRIMARY KEY("agent_num")
);
CREATE TABLE IF NOT EXISTS "AGENT_DATA" (
	"agent_data_time"	INTEGER NOT NULL,
	"agent_data_hydration_level"	REAL,
	"agent_data_hunger_level"	REAL,
	"agent_data_world_num"	TEXT NOT NULL,
	"agent_data_agent_num"	TEXT NOT NULL,
	"agent_data_pack_num"	TEXT,
	FOREIGN KEY("agent_data_agent_num") REFERENCES "AGENT"("agent_num"),
	FOREIGN KEY("agent_data_pack_num") REFERENCES "PACK"("pack_num"),
	FOREIGN KEY("agent_data_world_num") REFERENCES "WORLD"("world_num"),
	PRIMARY KEY("agent_data_time","agent_data_agent_num")
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
	"record_num"	TEXT NOT NULL UNIQUE,
	"record_start_time"	REAL NOT NULL,
	"record_stop_time"	REAL NOT NULL,
	PRIMARY KEY("record_num")
);
CREATE TABLE IF NOT EXISTS "SPECIES" (
	"species_num"	INTEGER NOT NULL UNIQUE,
	"species_label"	TEXT NOT NULL UNIQUE,
	"parent_species_num"	INTEGER,
	"species_carcass_energy_contribution"	REAL,
	"species_max_water_needs"	REAL,
	"species_max_energy_needs"	REAL,
	"species_max_speed"	REAL,
	"species_gestation_period"	REAL,
	"species_maturity_age"	REAL,
	"species_max_age"	REAL,
	"species_digestion_time"	REAL,
	"species_prey_consumption_time"	REAL,
	"species_range"	REAL,
	"species_max_health"	REAL,
	"species_damage"	REAL,
	FOREIGN KEY("parent_species_num") REFERENCES "SPECIES"("species_num"),
	PRIMARY KEY("species_num")
);
CREATE TABLE IF NOT EXISTS "PREY_LIST" (
	"prey_num"	INTEGER NOT NULL UNIQUE,
	"prey_species_num"	INTEGER NOT NULL,
	"predator_species_num"	INTEGER NOT NULL,
	FOREIGN KEY("predator_species_num") REFERENCES "SPECIES"("species_num"),
	FOREIGN KEY("prey_species_num") REFERENCES "SPECIES"("species_num"),
	PRIMARY KEY("prey_num" AUTOINCREMENT)
);
INSERT OR IGNORE INTO "GENDER" VALUES (NULL,'Mâle');
INSERT OR IGNORE INTO "GENDER" VALUES (NULL,'Femelle');
INSERT OR IGNORE INTO "WORLD" VALUES ('test', 15, 0.8, 25);
INSERT OR IGNORE INTO "SPECIES" VALUES (NULL,'Mammifère',NULL);
INSERT OR IGNORE INTO "SPECIES" VALUES (NULL,'Cervidés',1);
INSERT OR IGNORE INTO "SPECIES" VALUES (NULL,'Cerf Élaphe',2);
INSERT OR IGNORE INTO "SPECIES" VALUES (NULL,'Chevreuil',2);
INSERT OR IGNORE INTO "SPECIES" VALUES (NULL,'Daim',2);
INSERT OR IGNORE INTO "SPECIES" VALUES (NULL,'Renne',2);
INSERT OR IGNORE INTO "SPECIES" VALUES (NULL,'Élan',2);
INSERT OR IGNORE INTO "SPECIES" VALUES (NULL,'Canidés',1);
INSERT OR IGNORE INTO "SPECIES" VALUES (NULL,'Loup',8);
INSERT OR IGNORE INTO "SPECIES" VALUES (NULL,'Wolf2',8);
INSERT OR IGNORE INTO "SPECIES" VALUES (NULL,'alligator',NULL);
INSERT OR IGNORE INTO "SPECIES" VALUES (NULL,'Elephant2',1);
INSERT OR IGNORE INTO "SPECIES" VALUES (NULL,'Iguana',NULL);
INSERT OR IGNORE INTO "SPECIES" VALUES (NULL,'Lizard',NULL);
INSERT OR IGNORE INTO "SPECIES" VALUES (NULL,'Panda',1);
INSERT OR IGNORE INTO "SPECIES" VALUES (NULL,'Penguin',NULL);
INSERT OR IGNORE INTO "SPECIES" VALUES (NULL,'Rabbit',1);
INSERT OR IGNORE INTO "SPECIES" VALUES (NULL,'Racoon',NULL);
INSERT OR IGNORE INTO "SPECIES" VALUES (NULL,'Snake',NULL);
INSERT OR IGNORE INTO "SPECIES" VALUES (NULL,'Tiger',NULL);
INSERT OR IGNORE INTO "SPECIES" VALUES (NULL,'tortoise',NULL);
INSERT OR IGNORE INTO "SPECIES" VALUES (NULL,'wolf',8);
INSERT OR IGNORE INTO "SPECIES" VALUES (NULL,'Zebra',NULL);
INSERT INTO "PREY_LIST" VALUES (1,17,10);
INSERT INTO "PREY_LIST" VALUES (2,19,10);
COMMIT;
