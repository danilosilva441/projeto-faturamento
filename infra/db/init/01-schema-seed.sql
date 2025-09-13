-- Criar schema
CREATE SCHEMA IF NOT EXISTS faturamento;

-- Tabelas
CREATE TABLE IF NOT EXISTS faturamento.usuarios (
    id SERIAL PRIMARY KEY,
    nome VARCHAR(100) NOT NULL,
    email VARCHAR(150) UNIQUE NOT NULL,
    criado_em TIMESTAMP DEFAULT NOW()
);

CREATE TABLE IF NOT EXISTS faturamento.operacoes (
    id SERIAL PRIMARY KEY,
    nome VARCHAR(100) NOT NULL,
    descricao TEXT,
    ativo BOOLEAN NOT NULL DEFAULT TRUE,
    criado_em TIMESTAMP DEFAULT NOW()
);

CREATE TABLE IF NOT EXISTS faturamento.faturamentos (
    id SERIAL PRIMARY KEY,
    operacao_id INT NOT NULL REFERENCES faturamento.operacoes(id) ON DELETE CASCADE,
    data DATE NOT NULL,
    valor NUMERIC(12,2) NOT NULL CHECK (valor >= 0),
    criado_em TIMESTAMP DEFAULT NOW(),
    UNIQUE (operacao_id, data)
);

CREATE TABLE IF NOT EXISTS faturamento.metas (
    id SERIAL PRIMARY KEY,
    operacao_id INT NOT NULL REFERENCES faturamento.operacoes(id) ON DELETE CASCADE,
    mes INT NOT NULL CHECK (mes BETWEEN 1 AND 12),
    ano INT NOT NULL,
    valor_meta NUMERIC(12,2) NOT NULL CHECK (valor_meta >= 0),
    criado_em TIMESTAMP DEFAULT NOW()
);

-- Dados base
INSERT INTO faturamento.usuarios (nome, email) VALUES
('Danilo Silva', 'danilo@example.com'),
('Maria Souza', 'maria@example.com')
ON CONFLICT DO NOTHING;

INSERT INTO faturamento.operacoes (nome, descricao) VALUES
('Motorista App - Danilo', 'Faturamento diário como motorista de aplicativo'),
('Filial Recife', 'Loja física - Recife/PE')
ON CONFLICT DO NOTHING;

-- Metas exemplo (ajuste depois à vontade)
INSERT INTO faturamento.metas (operacao_id, mes, ano, valor_meta) VALUES
(1, 7, 2025, 25000.00),
(1, 8, 2025, 26000.00),
(1, 9, 2025, 27000.00),
(2, 7, 2025, 40000.00),
(2, 8, 2025, 42000.00)
ON CONFLICT DO NOTHING;

-- Seed: últimos 6 meses de faturamento para ambas as operações
DO $$
DECLARE
    d DATE := CURRENT_DATE - INTERVAL '180 days';
    fim DATE := CURRENT_DATE;
    v NUMERIC;
BEGIN
    WHILE d <= fim LOOP
        -- Operação 1 (faixa R$1.000~R$3.000)
        v := (RANDOM() * (3000-1000) + 1000)::NUMERIC(12,2);
        INSERT INTO faturamento.faturamentos (operacao_id, data, valor)
        VALUES (1, d, v)
        ON CONFLICT (operacao_id, data) DO NOTHING;

        -- Operação 2 (faixa R$2.000~R$5.000)
        v := (RANDOM() * (5000-2000) + 2000)::NUMERIC(12,2);
        INSERT INTO faturamento.faturamentos (operacao_id, data, valor)
        VALUES (2, d, v)
        ON CONFLICT (operacao_id, data) DO NOTHING;

        d := d + INTERVAL '1 day';
    END LOOP;
END $$;
