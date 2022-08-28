CREATE DATABASE signup_db
    WITH
    OWNER = postgres
    ENCODING = 'UTF8'
    LC_COLLATE = 'English_United Kingdom.1252'
    LC_CTYPE = 'English_United Kingdom.1252'
    TABLESPACE = pg_default
    CONNECTION LIMIT = -1
    IS_TEMPLATE = False;

CREATE SCHEMA IF NOT EXISTS public
    AUTHORIZATION postgres;

COMMENT ON SCHEMA public
    IS 'standard public schema';

GRANT ALL ON SCHEMA public TO PUBLIC;

GRANT ALL ON SCHEMA public TO postgres;

CREATE EXTENSION IF NOT EXISTS "uuid-ossp";

CREATE TABLE Users (
	ID uuid DEFAULT uuid_generate_v4 () PRIMARY KEY,
	Email VARCHAR ( 100 ) UNIQUE NOT NULL,
	PasswordHash CHAR ( 44 ) NOT NULL,
	Salt CHAR ( 24 ) NOT NULL,
	CreatedOn TIMESTAMP DEFAULT current_timestamp NOT NULL
);