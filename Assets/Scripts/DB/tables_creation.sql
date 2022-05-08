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
	"agent_death_cause"	TEXT,
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
	"species_stamina_max" REAL,
	"species_litter_max"	INTEGER,
	"species_type"   TEXT,
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
INSERT INTO "SPECIES" VALUES (1,'Wolf2',8,50.0,100.0,15.0,8.0,30.0,270.0,4745.0,1.0,0.01,1.0,250.0,50.0,1.0,8, "predator");
INSERT INTO "SPECIES" VALUES (2,'alligator',0,360.0,100.0,5.0,5.0,63.0,3650.0,50.0,1.0,0.01,1.0,300.0,75.0,1.0,2, "predator");
INSERT INTO "SPECIES" VALUES (8,'Rabbit',1,5.0,10.0,5.0,4.0,35.0,5.0,4380.0,1.0,0.01,1.0,25.0,2.0,1.0,10,"prey");
INSERT INTO "SPECIES" VALUES (9,'Racoon',1,20.0,50.0,6.0,4.0,63.0,300.0,20.0,1.0,0.01,1.0,45.0,15.0,1.0,4,"prey");
INSERT INTO "SPECIES" VALUES (11,'Tiger',0,360.0,100.0,10.0,10.0,63.0,3650.0,50.0,1.0,0.01,1.0,300.0,75.0,1.0,2, "predator");
INSERT INTO "SPECIES" VALUES (13,'Zebra',1,20.0,50.0,6.0,4.0,63.0,300.0,20.0,1.0,0.01,1.0,45.0,15.0,1.0,4,"prey");
INSERT INTO "SPECIES" VALUES (14,'Grass',0,100.0,100.0,25.0,0.0,63.0,5.0,365.0,1.0,0.01,1.0,1.0,0.0,1.0,0.0,"autotroph");
INSERT INTO "PREY_LIST" VALUES (1,8,1);
INSERT INTO "PREY_LIST" VALUES (2,8,9);
INSERT INTO "PREY_LIST" VALUES (3,8,11);
INSERT INTO "PREY_LIST" VALUES (4,14,8);
INSERT INTO "PREY_LIST" VALUES (6,8,4);
INSERT INTO "PREY_LIST" VALUES (7,14,13);
INSERT INTO "PREY_LIST" VALUES (8,13,1);
INSERT INTO "PREY_LIST" VALUES (9,9,1);
COMMIT;
