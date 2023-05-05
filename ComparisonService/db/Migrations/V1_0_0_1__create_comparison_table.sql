create table if not exists Comparisons (
    id BIGSERIAL primary key,
    embeddings1 double precision[] not null,
    embeddings2 double precision[] not null,
    equals boolean not null
);