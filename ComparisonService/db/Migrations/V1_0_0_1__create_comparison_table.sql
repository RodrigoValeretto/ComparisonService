create table if not exists Comparisons (
    id BIGSERIAL primary key,
    embedding1 double precision[] not null,
    embedding2 double precision[] not null,
    equals boolean not null
);