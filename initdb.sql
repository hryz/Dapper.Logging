create table product
(
    id      serial
        constraint product_pk
            primary key,
    name    varchar(100),
    code    varchar(10)    not null,
    price   decimal(18, 4) not null,
    deleted bool           not null
);

INSERT INTO public.product (name, code, price, deleted)
VALUES ('MacBook Pro', 'MBP', 2200.0000, false);
INSERT INTO public.product (name, code, price, deleted)
VALUES ('Dell XPS15', 'XPS', 2000.0000, false);
INSERT INTO public.product (name, code, price, deleted)
VALUES ('Surface Book', 'SURF', 2000.0000, false);
INSERT INTO public.product (name, code, price, deleted)
VALUES ('HP Spectre', 'SPEC', 2300.0000, true);
INSERT INTO public.product (name, code, price, deleted)
VALUES ('LG Gram', 'GRAM', 1500.0000, false);