create table if not exists Images (
    guid UUID primary key,
    image bytea not null
);