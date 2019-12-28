CREATE TABLE public."ExceptionModels"
(
    "ExceptionGuid" uuid NOT NULL,
    "Origin" text COLLATE pg_catalog."default" NOT NULL,
    "Type" text COLLATE pg_catalog."default" NOT NULL,
    "Code" text COLLATE pg_catalog."default",
    "Detail" text COLLATE pg_catalog."default",
    "StatusCode" text COLLATE pg_catalog."default",
    "ExceptionDate" timestamp without time zone NOT NULL,
    "ExceptionConsumedDate" timestamp without time zone NOT NULL,
    CONSTRAINT "PK_ExceptionModels" PRIMARY KEY ("ExceptionGuid")
)

TABLESPACE pg_default;

ALTER TABLE public."ExceptionModels"
    OWNER to postgres;