create table if not exists Images (
    guid UUID primary key,
    embeddings double precision[] not null
);