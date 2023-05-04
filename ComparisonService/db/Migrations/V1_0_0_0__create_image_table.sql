create table if not exists Images (
    guid UUID primary key,
    embeddings jsonb not null
);